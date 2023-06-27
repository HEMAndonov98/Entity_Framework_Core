namespace EventMi.Infrastructure.Common.RepositoryContracts
{
    using System.Linq;
    
    public interface IDeletableEntityRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        IQueryable<TEntity> AllWithDeleted();

        IQueryable<TEntity> AllAsNoTrackingWithDeleted();

        void HardDelete(TEntity entity);

        void Undelete(TEntity entity);
    }
}
