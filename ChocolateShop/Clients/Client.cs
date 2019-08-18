using ChocolateShopApp.Common.Exceptions;
using ChocolateShopApp.Common.Options;
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

        public Task DeleteEntity(string path, TViewModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TViewModel>> GetEntities(string path)
        {
            Logger.LogInformation($"Get Entities from path: {path}");

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
           
            Logger.LogTrace("Error happened in called webapi");
            throw new ClientException("Error happened in called webapi");
        }

        public Task<TViewModel> GetEntity(string path, int id)
        {
            throw new NotImplementedException();
        }

        public Task SaveEntity(string path, TViewModel model)
        {
            throw new NotImplementedException();
        }

        public Task UpdateEntity(string path, TViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}
