using Microsoft.EntityFrameworkCore;

namespace EvaluaIndividuos.Database.MEBA
{
    public class MEBAFactory
    {
        public readonly IDbContextFactory<MEBAContext> _dbContextFactory;

        public MEBAFactory(IDbContextFactory<MEBAContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }
    }
}
