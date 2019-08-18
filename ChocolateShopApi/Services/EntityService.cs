using ChocolateShopApi.Common.Exceptions;
using ChocolateShopApi.Data;
using ChocolateShopApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChocolateShopApi.Services
{
    public class EntityService<T> : IEntityService<T> where T : EntityBase
    {
        private ILogger<EntityService<T>> Logger { get; }
        private Store Store { get; }

        public EntityService(ILogger<EntityService<T>> logger, Store store)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            Store = store ?? throw new ArgumentNullException(nameof(store));
        }

        public async Task<IEnumerable<T>> GetEntities()
        {
            Logger.LogInformation("Get Entities of type {EntityType}", typeof(T));

            var models = await Store.Set<T>().ToListAsync();
            Logger.LogTrace("Received Entities: {ReceivedEntities}", models);

            return models;
        }

        public async Task<T> GetEntity(int id)
        {
            Logger.LogInformation("Get Entity of type {EntityType} with Id {EntityId}", typeof(T), id);

            if (id <= 0)
            {
                Logger.LogWarning("id must be > 0, passed id = {EntityId}", id);
                throw new EntityException($"id must be > 0, passed id = {id}");
            }

            var model = await Store.Set<T>().FirstOrDefaultAsync(e => e.Id == id);
            Logger.LogTrace("Received entity: {ReceivedEntity}", model);

            return model;
        }

        public async Task<T> PostEntity(T model)
        {
            Logger.LogInformation("Post Entity of type: {EntityType} with data: {Entity}", typeof(T), model);

            if (model == null)
            {
                Logger.LogWarning("Posted Entity must not be null, posted entity: {PostedEntity}", model);
                throw new ArgumentNullException();
            }

            if (model.Id != 0)
            { 
                Logger.LogWarning("Id must be 0, passed id =  {EntityId}", model.Id);
                throw new EntityException($"id must be 0, passed id = {model.Id}");
            }

            if (string.IsNullOrEmpty(model.Name))
            {
                Logger.LogWarning("Name must be filled in, passed name = {EntityName}", model.Name);
                throw new EntityException("name must be filled in");
            }

            await Store.Set<T>().AddAsync(model);
            Logger.LogTrace("Added Entity: {AddedEntity}", model);
            await Store.SaveChangesAsync();
            Logger.LogTrace("Saved Entity: {SavedEntity}", model);

            return model;
        }

        public async Task<T> PutEntity(T model)
        {
            Logger.LogInformation("Put Entity of type: {EntityType} with id: {EntityId} with data: {EntityData}", typeof(T), model.Id, model);

            if (model == null)
            {
                Logger.LogWarning("Puted Entity must not be null, puted entity: {PutedEntity}", model);
                throw new ArgumentNullException();
            }

            if (model.Id <= 0)
            {
                Logger.LogWarning("Id must be > 0, passed id = {EntityId}", model.Id);
                throw new EntityException($"id must be  > 0, passed id = {model.Id}");
            }

            if (string.IsNullOrEmpty(model.Name))
            {
                Logger.LogWarning("Name must be filled in, passed name = {EntityName}", model.Name);
                throw new EntityException("name must be filled in");
            }
                       
            Store.Set<T>().Update(model);
            Logger.LogTrace("Added Entity: {AddedEntity} to update", model);
            await Store.SaveChangesAsync();
            Logger.LogTrace("Updated Entity: {UpdatedEntity}", model);

            return model;
        }

        public async Task DeleteEntity(T model)
        {
            Logger.LogInformation("Delete Entity of type: {EntityType} with id: {EntityId} with data: {EntityData}", typeof(T), model.Id, model);

            if (model == null)
            {
                Logger.LogWarning("Entity to delete must not be null, entity to delete: {EntityToDelete}", model);
                throw new ArgumentNullException();
            }

            if (model.Id <= 0)
            {
                Logger.LogWarning("Id must be > 0, passed id = {EntityId}", model.Id);
                throw new EntityException($"id must be  > 0, passed id = {model.Id}");
            }

            Store.Set<T>().Remove(model);
            Logger.LogTrace("Added Entity: {AddedEntity} to be removed", model);
            await Store.SaveChangesAsync();
            Logger.LogTrace("Deleted Entity: {DeletedEntity}", model);
        }
    }
}

