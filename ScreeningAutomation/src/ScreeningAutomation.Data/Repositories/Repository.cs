namespace ScreeningAutomation.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;
    using Models.Base;

    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ScreeningAutomationDbContext _dbContext;        

        public Repository(ScreeningAutomationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task DeleteAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }

            if (entity.Id == 0)
            {
                throw new ArgumentException("Id is 0");
            }

            var entityToBeDeleted = await _dbContext.Set<T>().FirstOrDefaultAsync(x => x.Id == entity.Id);
            if (entityToBeDeleted == null)
            {
                throw new KeyNotFoundException();
            }

            _dbContext.Set<T>().Remove(entityToBeDeleted);

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(IEnumerable<int> idList)
        {
            if (idList == null)
            {
                throw new ArgumentNullException();
            }

            var entitiesToBeDeleted = await _dbContext.Set<T>().Where(x => idList.Contains(x.Id)).ToListAsync();

            if (entitiesToBeDeleted.Count < idList.Count())
            {
                var idNotFounded = idList.Where(id => entitiesToBeDeleted.Any(bt => bt.Id == id)).ToList();
                throw new KeyNotFoundException($"Couldn't find follow id: {string.Join(",", idNotFounded)}");
            }

            _dbContext.Set<T>().RemoveRange(entitiesToBeDeleted);
            await _dbContext.SaveChangesAsync();
        }

        public IQueryable<T> GetAll()
        {
            return _dbContext.Set<T>();
        }

        public async Task<T> SaveAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }

            if (entity.IsNew())
            {
                entity.CreatedDate = DateTimeOffset.UtcNow;
            }
            else
            {
                var entityId = entity.Id;
                var originalEntity = await _dbContext.Set<T>()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => e.Id == entityId);
                if (originalEntity == null)
                {
                    throw new KeyNotFoundException($"Could not find entity with id={entityId}");
                }
                entity.CreatedDate = originalEntity.CreatedDate;
                entity.ModifiedDate = DateTimeOffset.UtcNow;
            }

            entity = entity.IsNew() ? _dbContext.Set<T>().Add(entity).Entity : _dbContext.Set<T>().Update(entity).Entity;
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task SaveRangeAsync(IEnumerable<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException();
            }

            var addedEntities = entities.Where(x => x.Id == 0).ToList();
            var updatedEntities = entities.Where(x => x.Id != 0).ToList();

            if (addedEntities.Any())
            {
                var createdDate = DateTimeOffset.UtcNow;
                addedEntities.ForEach(e =>
                {
                    e.CreatedDate = createdDate;
                });
                _dbContext.Set<T>().AddRange(addedEntities);
            }

            if (updatedEntities.Any())
            {
                var modifiedDate = DateTimeOffset.UtcNow;
                var createdInfo = await _dbContext.Set<T>()
                    .Where(e => updatedEntities.Any(ue => ue.Id == e.Id))
                    .AsNoTracking()
                    .ToDictionaryAsync(x => x.Id, y => new { y.CreatedDate });

                updatedEntities.ForEach(e =>
                {
                    if (!createdInfo.ContainsKey(e.Id))
                    {
                        throw new KeyNotFoundException($"Could not find entity with id={e.Id}");
                    }
                    e.CreatedDate = createdInfo[e.Id].CreatedDate;
                    e.ModifiedDate = modifiedDate;
                });

                _dbContext.Set<T>().UpdateRange(updatedEntities);
            }

            await _dbContext.SaveChangesAsync();
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _dbContext.Database.BeginTransaction();
        }
    }
}
