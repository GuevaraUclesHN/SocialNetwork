using System;
using SocialNetwork.Core.Interfaces;

namespace SocialNetwork.Infrastructure.EntityFramework.Repositories
{
	public class BaseRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
	{
        private readonly SocialNetworkContext context;

        public BaseRepository(SocialNetworkContext context)
		{
            this.context = context;
        }

        public TEntity Add(TEntity entity)
        {
            var result = context.Add(entity);
            context.SaveChanges();
            return result.Entity;
        }

        public void Delete(TEntity entity) => context.Remove(entity);

        public IReadOnlyList<TEntity> Filter(Func<TEntity, bool> predicate)
        {
            return context.Set<TEntity>().Where(predicate).ToList();
        }

        public IReadOnlyList<TEntity> Get()
        {
            return context.Set<TEntity>().ToList();
        }

        public TEntity? GetById(int id)
        {
            return context.Set<TEntity>().Find(id);
        }

        public TEntity Update(TEntity entity)
        {
            var result = context.Update(entity);
            context.SaveChanges();
            return result.Entity;
        }
    }
}

