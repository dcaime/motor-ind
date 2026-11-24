using EvaluaIndividuos.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Serilog;

namespace EvaluaIndividuos.Database.MEBI
{
    public class MEBIFactory : IDictaminador
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public readonly IDbContextFactory<MEBIContext> _dbContextFactory;

        public MEBIFactory(IServiceScopeFactory scopeFactory, IDbContextFactory<MEBIContext> dbContextFactory)
        {
            _scopeFactory = scopeFactory;
            _dbContextFactory = dbContextFactory;
        }

        public long InsertRow(IIdentity row)
        {
            using var scope = _scopeFactory.CreateScope();
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<MEBIContext>();
                dbContext.Add(row);
                dbContext.SaveChanges();
                return row.ID;
            }
        }

        public void InsertRows(object[] rows)
        {
            using var scope = _scopeFactory.CreateScope();
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<MEBIContext>();
                dbContext.AddRange(rows);
                dbContext.SaveChanges();
            }
        }
    }
}
