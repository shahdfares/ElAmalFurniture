using ElAmal.Core.Contract.Services.Mail;
using ElAmal.Core.Contract.Services.Otp;
using ElAmal.Core.Entities.Mail;
using ElAmal.Core.Helper;
using StackExchange.Redis;
using System.Security.Cryptography;
using System.Text;

namespace ElAmal.Application.Services.Otp
{
    public class OtpServices : IOtpServices
    {
        private readonly IMailSettingsService _mailServices;
        private IDatabase _database;

        private const int OtpExpiryMinutes = 10;
        private const int RateLimitSeconds = 60;
        private const int MaxAttempts = 5;

        public OtpServices(IMailSettingsService mailServices, IConnectionMultiplexer redis)
        {
            _mailServices = mailServices;
            _database = redis.GetDatabase();
        }
        public async Task<ReturnResult<object>> GenerateAndSendOtpAsync(string email)
        {
            var rateLimitResult = await CheckOtpRateLimitAsync();

            if (!rateLimitResult.IsSuccess)
                return rateLimitResult;

            // Generate a 6-digit OTP
            var otp = new Random().Next(100000, 999999).ToString();

            // Hash the OTP before saving
            var hashedOtp = HashOtp(otp);

            // Save hashed OTP in Redis with expiry time
            await _database.StringSetAsync($"otp:elamal@admin.com", hashedOtp, TimeSpan.FromMinutes(OtpExpiryMinutes)); // 

            // Create and send OTP email
            var mail = BuildOtpEmail(email, otp);

            await _mailServices.SendEmailAsync(mail);

            return ReturnResult<object>.Success(null!, "OTP sent successfully");
        }

        public async Task<ReturnResult<object>> ResendOtpAsync(string email)
        {
            // Rate limit check
            var rateLimitResult = await CheckOtpRateLimitAsync();

            if (!rateLimitResult.IsSuccess)
                return rateLimitResult;

            // Generate new OTP
            var otp = new Random().Next(100000, 999999).ToString();

            var hashedOtp = HashOtp(otp);

            // Store new OTP
            await _database.StringSetAsync($"otp:elamal@admin.com", hashedOtp, TimeSpan.FromMinutes(OtpExpiryMinutes));

            var mail = BuildOtpEmail(email, otp);

            await _mailServices.SendEmailAsync(mail);

            return ReturnResult<object>.Success(data: null!, "OTP resent successfully");
        }


        public async Task<bool> VerifyOtpAsync(string email, string otp)
        {
            // Check maximum verification attempts
            var attemptsResult = await CheckOtpAttemptsAsync(email);

            if (!attemptsResult.IsSuccess)
                return false;

            var key = $"otp:{email}";

            // Get stored hashed OTP from Redis
            var storedHashedOtp = await _database.StringGetAsync(key);

            // Compare stored hash with hash of user OTP
            if (string.IsNullOrEmpty(storedHashedOtp) || storedHashedOtp != HashOtp(otp))
                return false;

            // OTP is correct -> delete OTP and attempts counter
            await _database.KeyDeleteAsync(key);
            await _database.KeyDeleteAsync($"otp:attempts:{email}");

            return true;
        }

        private async Task<ReturnResult<object>> CheckOtpRateLimitAsync()
        {
            //email = email.Trim().ToLowerInvariant();

            var key = $"otp:rate_limit: elamal@admin.com";  // 

            var added = await _database.StringSetAsync(key, "1", TimeSpan.FromSeconds(RateLimitSeconds),
            when: When.NotExists);

            if (!added)
                return ReturnResult<object>.Fail(message: "Please wait 60 seconds before requesting another OTP.");

            return ReturnResult<object>.Success(data: null!, message: "Success");
        }

        private Email BuildOtpEmail(string to,  string otp)
        {
            return new Email
            {
                To = to,
                Subject = $"Your OTP code",
                Body = $"Hello, ElAmal Furniture, Your OTP is <b>{otp}</b>."
            };
        }
        private async Task<ReturnResult<object>> CheckOtpAttemptsAsync(string email)
        {
            var attemptsKey = $"otp:attempts:{email}";

            // Get current attempts count
            var attemptsValue = await _database.StringGetAsync(attemptsKey);

            // If attempts reached the maximum limit → block verification
            if (attemptsValue.HasValue && int.Parse(attemptsValue!) >= MaxAttempts)
                return ReturnResult<object>.Fail(message: "Maximum OTP attempts reached. Please request a new OTP.");

            // Increment attempts count
            await _database.StringIncrementAsync(attemptsKey);

            // Ensure attempts expire with OTP expiration time
            await _database.KeyExpireAsync(attemptsKey, TimeSpan.FromMinutes(OtpExpiryMinutes));

            return ReturnResult<object>.Success(data: null!, message: "Success");
        }

        private static string HashOtp(string otp)
        {
            using var sha256 = SHA256.Create();                      // Create SHA256 instance

            var bytes = Encoding.UTF8.GetBytes(otp);                // Convert OTP to bytes

            var hash = sha256.ComputeHash(bytes);                  // Compute SHA256 hash

            return Convert.ToBase64String(hash);                  // Convert hash to Base64 string for storage
        }
    }
}
