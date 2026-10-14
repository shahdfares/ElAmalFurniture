using AutoMapper;
using ElAmal.Core.Contract.Repository;
using ElAmal.Core.Contract.Services.SpecProperty;
using ElAmal.Core.Entities.Products.TechnicalSpecifications;
using ElAmal.Core.Helper;
using ElAmal.Shared.DTOs.TechnicalSpecification;

namespace ElAmal.Core.Contract.Services.TechnicalSpecification
{
    public class SpecificatonPropertyServices : ISpecificatonPropertyServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SpecificatonPropertyServices(IUnitOfWork unitOfWork,
            IMapper mapper)
        {
           _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ReturnResult<IReadOnlyList<TechniacalSpecResponseDto>>> GetAllAsync()
        {
            var data = await _unitOfWork.GetRepository<SpecificationProperty, Guid>().GetAllAsync();

            if (data is null)
                return  ReturnResult<IReadOnlyList<TechniacalSpecResponseDto>>.Fail("No Technical Specification Founded");

            var response = _mapper.Map<IReadOnlyList<TechniacalSpecResponseDto>>(data);

            return ReturnResult<IReadOnlyList<TechniacalSpecResponseDto>>.Success(response);
        }

        public  async Task<ReturnResult<TechniacalSpecResponseDto>> AddAsync(TechniacalSpecRequestDto responseDto)
        {
            if (responseDto is  null)
                return ReturnResult<TechniacalSpecResponseDto>.Fail("Invalid Request");

            if(string.IsNullOrWhiteSpace(responseDto!.Name))
                return ReturnResult<TechniacalSpecResponseDto>.Fail("Name is required");

            responseDto.Name = responseDto.Name.Trim();

            //Check if Name exsits
            var exsit = await _unitOfWork.GetRepository<SpecificationProperty, Guid>()
                .ExistsAsync(e => e.Name == responseDto.Name);

            if(exsit) //True
                return ReturnResult<TechniacalSpecResponseDto>.Fail("Name already exsits");

            var techProp = _mapper.Map<SpecificationProperty>(responseDto);

            await _unitOfWork.GetRepository<SpecificationProperty, Guid>().AddAsync(techProp);

            await _unitOfWork.CompleteAsync();

            return ReturnResult<TechniacalSpecResponseDto>
                .Success(_mapper.Map<TechniacalSpecResponseDto>(techProp),
                "Property added successfully");
        }

        public async Task<ReturnResult<TechniacalSpecResponseDto>> UpdateAsync(Guid id,TechniacalSpecRequestDto responseDto)
        {
            if (responseDto is  null && id == Guid.Empty)
                return ReturnResult<TechniacalSpecResponseDto>.Fail("Invalid Request");

            if (string.IsNullOrWhiteSpace(responseDto!.Name))
                return ReturnResult<TechniacalSpecResponseDto>.Fail("Name is required");

            responseDto.Name = responseDto.Name.Trim();

            //Check if Name exsits
            var exsit = await _unitOfWork.GetRepository<SpecificationProperty, Guid>()
                .ExistsAsync(e => e.Name == responseDto.Name);

            if (exsit) //True
                return ReturnResult<TechniacalSpecResponseDto>.Fail("Name already exsits");

            var techProp = await _unitOfWork.GetRepository<SpecificationProperty, Guid>().GetEntityAync(id);

            if(techProp is null)
                return ReturnResult<TechniacalSpecResponseDto>.Fail("Property not found");

            techProp.Name = responseDto.Name;

             _unitOfWork.GetRepository<SpecificationProperty, Guid>().Update(techProp);

            await _unitOfWork.CompleteAsync();

            return ReturnResult<TechniacalSpecResponseDto>
                .Success(_mapper.Map<TechniacalSpecResponseDto>(techProp),
                "Property updated successfully");
        }

        public async Task<ReturnResult<TechniacalSpecResponseDto>> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                return ReturnResult<TechniacalSpecResponseDto>.Fail("Invalid Request");

            var exsits = await _unitOfWork.GetRepository<SpecificationProperty, Guid>().ExistsAsync(e => e.Id == id);

            if(!exsits)
                return ReturnResult<TechniacalSpecResponseDto>.Fail("Property not found");

            var prop = await _unitOfWork.GetRepository<SpecificationProperty, Guid>().GetEntityAync(id);

            if(prop is null)
                return ReturnResult<TechniacalSpecResponseDto>.Fail("Property not found");

            _unitOfWork.GetRepository<SpecificationProperty, Guid>().Delete(prop);

            await _unitOfWork.CompleteAsync();

            return ReturnResult<TechniacalSpecResponseDto>
                .Success(_mapper.Map<TechniacalSpecResponseDto>(prop),"TechProp Deleted Successfuly");
        }
    }
}
