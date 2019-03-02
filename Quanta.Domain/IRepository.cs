using System.Linq;
using System.Threading.Tasks;

namespace Quanta.Domain
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