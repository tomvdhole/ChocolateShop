using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChocolateShopApi.Common.Exceptions;
using ChocolateShopApi.Models;
using ChocolateShopApi.Services;
using ChocolateShopViewModels;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ChocolateShopApi.Controllers
{
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public abstract class EntityController<TController, TEntity, TViewModel> : ControllerBase
        where TEntity : EntityBase
        where TViewModel : EntityViewModelBase
    {
        private ILogger<TController> Logger { get; }
        protected IEntityService<TEntity> Service { get; }

        private object JsonError(string msg) => new { Error = msg };

        public EntityController(ILogger<TController> logger, IEntityService<TEntity> service)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            Service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<EntityViewModel>>> GetEntities()
        {
            Logger.LogInformation("Request: {RequestUrl}", Request.GetDisplayUrl());
            Logger.LogInformation("Get Entities of type {EntityType}", typeof(TEntity));

            try
            {
                var entities = await Service.GetEntities();

                Logger.LogTrace("Received Entities: {ReceivedEntities}", entities);

                var models = GetViewModels(entities);
                return Ok(models);
            }
            catch (Exception e)
            {
                Logger.LogCritical("Critical error: {CriticalErrorStackTrace}", e.StackTrace);
                return StatusCode(500);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<EntityViewModel>> GetEntity([FromRoute] int id)
        {
            Logger.LogInformation("Request: {RequestUrl}", Request.GetDisplayUrl());
            Logger.LogInformation("Get Entity of type {EntityType} with id: {EntityId}", typeof(TEntity), id);

            if (id <= 0)
            {
                Logger.LogWarning("id must be > 0, passed id = {EntityId}", id);
                return BadRequest(JsonError($"id must be > 0, passed id = {id}"));
            }

            try
            {
                var entity = await Service.GetEntity(id);
                if (entity == null)
                {
                    Logger.LogWarning("Entity with {EntityWithIdNotFound} not found", id);
                    return NotFound();
                }

                Logger.LogTrace("Received Entity: {ReceivedEntity}", entity);

                var model = GetViewModel(entity);
                return Ok(model);
            }
            catch (EntityException e)
            {
                Logger.LogError("{EntityException}", e.Message);
                return BadRequest(JsonError(e.Message));
            }
            catch (Exception e)
            {
                Logger.LogCritical("Critical error: {CriticalErrorTrace}", e.StackTrace);
                return StatusCode(500);
            }
        }

        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(201)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<EntityViewModel>> Post([FromBody] TViewModel model)
        {
            Logger.LogInformation("Request: {RequestUrl}", Request.GetDisplayUrl());
            Logger.LogInformation("Post ViewModel {ViewModelType} to an Entity of type {EntityType}", typeof(EntityViewModel), typeof(TEntity));

            if (model == null)
            {
                Logger.LogWarning("Posted element not filled in");
                return BadRequest(JsonError("Posted element not filled in"));
            }

            if (model.Id != 0)
            {
                Logger.LogWarning($"id must be 0, passed id = {model.Id}");
                return BadRequest(JsonError($"id must be 0, passed id = {model.Id}"));
            }

            if (string.IsNullOrEmpty(model.Name))
            {
                Logger.LogWarning("name not filled in");
                return BadRequest(JsonError("name not filled in"));
            }

            try
            {
                var entity = await PostEntity(model);
                model.Id = entity.Id;
                Logger.LogTrace("Saved Entity: {SavedEntity}", entity);

                return CreatedAtAction("GetEntity", new { id = model.Id }, model);
            }
            catch (DbUpdateException e)
            {
                Logger.LogError("Error while saving: {DbUpdateException}", e.Message);
                return BadRequest("Error while saving to database, please try again later");
            }
            catch (Exception e)
            {
                Logger.LogCritical("Critical error: {CriticalErrorStackTrace}", e.StackTrace);
                return StatusCode(500);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] TViewModel model)
        {
            Logger.LogInformation("Request: {RequestUrl}", Request.GetDisplayUrl());
            Logger.LogInformation("Put ViewModel {ViewModelType} to an Entity of type {EntityType} with id {IdToUpdate}", typeof(EntityViewModel), typeof(TEntity), id);

            if (id <= 0)
            {
                Logger.LogWarning($"id must be  > 0, passed id = {id}");
                return BadRequest(JsonError($"id must be > 0, passed id = {id}"));
            }

            if (id != model.Id)
            {
                Logger.LogWarning($"passed id = {id}, must be equal to passed model id {model.Id}");
                return BadRequest(JsonError($"passed id = {id}, must be equal to passed model id {model.Id}"));
            }

            try
            {
                await PutEntity(model);
                Logger.LogTrace("Updated Entity: {SavedEntity}", model);

                return NoContent();
            }
            catch (EntityException e)
            {
                Logger.LogError("{EntityException}", e.Message);
                return BadRequest(JsonError(e.Message));
            }
            catch (DbUpdateConcurrencyException e)
                when (e.Message.Contains("Database operation expected to affect 1 row(s) but actually affected 0 row(s)."))
            {
                Logger.LogError("{EntityNotFound}", e.Message);
                return NotFound();
            }
            catch (DbUpdateConcurrencyException e)
            {
                Logger.LogError(e, "Update Concurrency Exception: {DbUpdateConcurrencyExceptionWithId}", model.Id);
                return Conflict();
            }
            catch (DbUpdateException e)
            {
                Logger.LogError("Error while saving: {DbUpdateException}", e.Message);
                return BadRequest(JsonError("Error while saving to database, please try again later"));
            }
            catch (Exception e)
            {
                Logger.LogCritical("Critical error: {CriticalErrorStackTrace}", e.StackTrace);
                return StatusCode(500);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            Logger.LogInformation("Request: {RequestUrl}", Request.GetDisplayUrl());
            Logger.LogInformation("Delete entity: {EntityName}, with id: {id}", typeof(TEntity).Name, id);

            if (id <= 0)
            {
                Logger.LogWarning("id must be > 0, passed id = {EntityId}", id);
                return BadRequest(JsonError($"id must be > 0, passed id = {id}"));
            }

            var entity = await Service.GetEntity(id);
            if (entity == null)
            {
                Logger.LogWarning("Entity not found {EntityIdNotFound}", id);
                return NotFound();
            }

            try
            {
                await Service.DeleteEntity(entity);
                Logger.LogTrace("Deleted entity: {DeletedEntity}", entity);
                return NoContent();
            }
            catch(DbUpdateConcurrencyException e)
            {
                Logger.LogError(e, "Update Concurrency Exception: {DbUpdateConcurrencyExceptionWithId}", entity.Id);
                return Conflict();
            }
            catch(DbUpdateException e)
            {
                Logger.LogError("Error while saving: {DbUpdateException}", e.Message);
                return BadRequest(JsonError("Error while saving to database, please try again later"));
            }
            catch (Exception e)
            {
                Logger.LogCritical("Critical error: {CriticalErrorStackTrace}", e.StackTrace);
                return StatusCode(500);
            }
        }

        protected abstract IEnumerable<TViewModel> GetViewModels(IEnumerable<TEntity> entities);
        protected abstract TViewModel GetViewModel(TEntity entity);
        protected abstract Task<TEntity> PostEntity(TViewModel model);
        protected abstract Task PutEntity(TViewModel model);
    }
}

