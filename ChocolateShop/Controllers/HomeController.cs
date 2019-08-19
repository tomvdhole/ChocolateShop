using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http.Extensions;
using ChocolateShopApp.Clients;
using ChocolateShopViewModels;
using ChocolateShopApp.Common.Options;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using ChocolateShopApp.Common.Exceptions;

namespace ChocolateShop.Controllers
{
    public class HomeController : Controller
    {
        private ILogger<HomeController> Logger { get; set; }
        private WebApiOptions Options { get; set; }
        private IClient<EntityImageViewModel> Client { get; set; }

        public HomeController(ILogger<HomeController> logger, IOptionsMonitor<WebApiOptions> options, IClient<EntityImageViewModel> client)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            options = options ?? throw new ArgumentNullException(nameof(options));
            Options = options.CurrentValue ?? throw new NullReferenceException(nameof(options.CurrentValue));
            Client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<IActionResult> Index()
        {
            Logger.LogInformation($"Request: {Request.GetDisplayUrl()}");
            Logger.LogInformation($"Get Home page");

            try
            {
                var models = await Client.GetEntities(Options.GetProducts);
                Logger.LogTrace("Received models: ", models);

                return View(models);
            }
            catch(ClientException e)
            {
                Logger.LogError("{ClientException}", e.Message);

                return View();
            }
            catch(Exception e)
            {
                Logger.LogCritical("Critical error: {CriticalErrorStackTrace}", e.StackTrace);

                return View();
            }
        }

        public IActionResult Privacy()
        {
            Logger.LogInformation($"Request: {Request.GetDisplayUrl()}");
            Logger.LogInformation($"Get Privacy page");

            return View();
        }
    }
}

