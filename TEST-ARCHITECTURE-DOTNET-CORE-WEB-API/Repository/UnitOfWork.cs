using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEST_ARCHITECTURE_DOTNET_CORE_WEB_API.Data;
using TEST_ARCHITECTURE_DOTNET_CORE_WEB_API.IRepository;

namespace TEST_ARCHITECTURE_DOTNET_CORE_WEB_API.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private IGenericRepository<Country> _countries;
        private IGenericRepository<Hotel> _hotels;
        private readonly DatabaseContext _context;

        public UnitOfWork(DatabaseContext context)
        {
            this._context = context;
        }

        public IGenericRepository<Country> Countries => _countries ?? new GenericRepository<Country>(this._context); 
        public IGenericRepository<Hotel> Hotels => _hotels ?? new GenericRepository<Hotel>(this._context);

        public void Dispose()
        {
            this._context.Dispose();
            GC.SuppressFinalize(this);
        }
        public async Task Save()
        {
            await this._context.SaveChangesAsync();
        }
    }
}
