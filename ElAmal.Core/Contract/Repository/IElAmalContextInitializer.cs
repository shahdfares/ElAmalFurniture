using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElAmal.Core.Contract.Repository
{
    public interface IElAmalContextInitializer
    {
        Task UpdateDatabaseAsync();

        Task SeedAsync();
    }
}
