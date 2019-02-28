using System.Linq;
using System.Threading.Tasks;

namespace Guacamole.Data
{
    public interface IRepository<TEntity>
    {
        IQueryable<TEntity> All();

        void Add(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        void SaveChanges();

        Task SaveChangesAsync();
    }
}