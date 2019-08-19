using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChocolateShopApp.Clients;
using ChocolateShopApp.Common.Exceptions;
using ChocolateShopApp.Common.Options;
using ChocolateShopViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ChocolateShopApp.Controllers
{
    public class BrandsController : Controller
    {
        private ILogger<BrandsController> Logger { get; set; }
        private WebApiOptions Options { get; set; }
        private IClient<EntityViewModel> Client { get; set; }

        public BrandsController(ILogger<BrandsController> logger, IOptionsMonitor<WebApiOptions> options, IClient<EntityViewModel> client)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            options = options ?? throw new ArgumentNullException(nameof(options));
            Options = options.CurrentValue ?? throw new NullReferenceException(nameof(options.CurrentValue));
            Client = client ?? throw new ArgumentNullException(nameof(client));
        }

        // GET: Brands
        public async Task<IActionResult> Index()
        {
            Logger.LogInformation($"Request: {Request.GetDisplayUrl()}");
            Logger.LogInformation($"Get Brands Home page");

            try
            {
                var models = await Client.GetEntities(Options.GetBrands);
                Logger.LogTrace("Reveived models: {ReceivedModels}", models);

                return View(models);
            }
            catch(ClientException e)
            {
                Logger.LogError("{ClientException}", e.Message);

                return View();
            }
            catch (Exception e)
            {
                Logger.LogCritical("Critical error: {CriticalErrorStackTrace}", e.StackTrace);

                return View();
            }
        }

        // GET: Brands/Create
        public IActionResult Create()
        {
            Logger.LogInformation($"Request: {Request.GetDisplayUrl()}");
            Logger.LogInformation($"Get Brand Create page");

            return View();
        }

        // POST: Brands/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EntityViewModel model)
        {
            //TO ADAPT

            Logger.LogInformation($"Request: {Request.GetDisplayUrl()}");
            Logger.LogInformation($"Post Brand Create page");

            if (ModelState.IsValid)
            {
                try
                {

                    model = await Client.SaveEntity(Options.PostBrand, model);
                    Logger.LogTrace("Saved model: {SavedModel}", model);

                    return RedirectToAction(nameof(Index));
                }
                catch(ClientException e)
                {
                    return View();
                }
                catch(ArgumentNullException e)
                {

                }
                catch(Exception e)
                {

                }
            }

            return View(model);
        }

        // GET: Brands/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Brands/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, EntityViewModel model)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Brands/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Brands/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, EntityViewModel model)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Brands/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
    }
}