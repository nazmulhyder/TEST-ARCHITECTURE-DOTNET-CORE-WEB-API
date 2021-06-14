using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEST_ARCHITECTURE_DOTNET_CORE_WEB_API.Data;

namespace TEST_ARCHITECTURE_DOTNET_CORE_WEB_API.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Country> Countries { get; }
        IGenericRepository<Hotel> Hotels { get; }
        Task Save();
    }
}
