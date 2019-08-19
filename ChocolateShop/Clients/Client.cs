using ChocolateShopApp.Common.Exceptions;
using ChocolateShopApp.Common.Options;
using ChocolateShopViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ChocolateShopApp.Clients
{
    public class Client<TViewModel> : IClient<TViewModel>
        where TViewModel: EntityViewModelBase
    {
        private ILogger<Client<TViewModel>> Logger { get; set; }
        private HttpClient ApiClient { get; set; }
        private WebApiOptions Options { get; set; }

        public Client(ILogger<Client<TViewModel>> logger, IOptionsMonitor<WebApiOptions> options, HttpClient client)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            options = options ?? throw new ArgumentNullException(nameof(options));
            Options = options.CurrentValue ?? throw new NullReferenceException(nameof(options.CurrentValue));
            ApiClient = client ?? throw new ArgumentNullException(nameof(client));
            
            ApiClient.BaseAddress = new Uri(Options.BaseUrl);
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<IEnumerable<TViewModel>> GetEntities(string path)
        {
            Logger.LogInformation("Get Entities from path: {PathToEntities}", path);

            if (path == null)
            {
                Logger.LogWarning("Passed path must not be null, passed path: {PassedPath}", path);
                throw new ArgumentNullException(nameof(path));
            }
            
            var response = await ApiClient.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                var models = await response.Content.ReadAsAsync<IEnumerable<TViewModel>>();
                Logger.LogTrace("Received Entities: {ReceivedModels}", models);
                return models;
            }
            else
            {
                Logger.LogError("Error happened in called webapi");
                throw new ClientException("Error happened in called webapi");
            }
        }

        public async Task<TViewModel> GetEntity(string path, int id)
        {
            Logger.LogInformation("Get Entity with id {EntityId} from path: {PathToEntities}", id, path);

            if (path == null)
            {
                Logger.LogWarning("Passed path must not be null, passed path: {PassedPath}", path);
                throw new ArgumentNullException(nameof(path));
            }

            if(id <= 0)
            {
                Logger.LogWarning("id must be > 0, passed id = {EntityId}", id);
                throw new ClientException($"id must be > 0, passed id = {id}");
            }

            var response = await ApiClient.GetAsync(path + id);
            if (response.IsSuccessStatusCode)
            {
                var model = await response.Content.ReadAsAsync<TViewModel>();
                Logger.LogTrace("Received Entity: {ReceivedModel}", model);
                return model;
            }
            else
            {
                Logger.LogError("Error happened in called webapi");
                throw new ClientException("Error happened in called webapi");
            }
        }

        public async Task<TViewModel> SaveEntity(string path, TViewModel model)
        {
            Logger.LogInformation("Save model: {ModelToSave} to path: {PathToSave}", model, path);

            if (path == null)
            {
                Logger.LogWarning("Passed path must not be null, passed path: {PassedPath}", path);
                throw new ArgumentNullException(nameof(path));
            }

            if(model.Id != 0)
            {
                Logger.LogWarning("id must be 0, passed id = {PassedId}", model.Id);
                throw new ClientException($"id must be 0, passed id = {model.Id}");
            }

            if(string.IsNullOrEmpty(model.Name))
            {
                Logger.LogWarning("name not filled in");
                throw new ClientException($"name not filled in");
            }

            var response = await ApiClient.PostAsJsonAsync(path, model);
            if (response.IsSuccessStatusCode)
            { 
                Logger.LogTrace("Saved model: {ModelSaved}", model);
                return await GetEntity(Options.GetBrand, int.Parse(response.Headers.Location.Segments[3]));
            }
            else
            {
                Logger.LogError("Error happened in called webapi");
                throw new ClientException("Error happened in called webapi");
            }
        }

        public Task UpdateEntity(string path, TViewModel model)
        {
            throw new NotImplementedException();
        }

        public Task DeleteEntity(string path, TViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}
