using System;

namespace SocialNetwork.Core.Interfaces
{
	public interface IRepository<TEntity>
		where TEntity : class
	{
		TEntity Add(TEntity entity);

        TEntity Update(TEntity entity);

		void Delete(TEntity entity);

		TEntity? GetById(int id);

		IReadOnlyList<TEntity> Get();

		IReadOnlyList<TEntity> Filter(Func<TEntity, bool> predicate);
    }
}

