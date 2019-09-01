using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ChocolateShopApp.Clients;
using ChocolateShopApp.Common.Exceptions;
using ChocolateShopApp.Common.Options;
using ChocolateShopApp.Models;
using ChocolateShopViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ChocolateShopApp.Controllers
{
    public class ProductsController : Controller
    {
        private ILogger<ProductsController> Logger { get; set; }
        private WebApiOptions Options { get; set; }
        private IClient<EntityImageViewModel> Client { get; set; }

        public ProductsController(ILogger<ProductsController> logger, IOptionsMonitor<WebApiOptions> options, IClient<EntityImageViewModel> client)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            options = options ?? throw new ArgumentNullException(nameof(options));
            Options = options.CurrentValue ?? throw new NullReferenceException(nameof(options.CurrentValue));
            Client = client ?? throw new ArgumentNullException(nameof(client));
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            Logger.LogInformation($"Request: {Request.GetDisplayUrl()}");
            Logger.LogInformation($"Get Products Home page");

            try
            {
                var models = await Client.GetEntities(Options.Products);
                Logger.LogTrace("Reveived models: {ReceivedModels}", models);

                return View(models);
            }
            catch (ClientException e)
            {
                Logger.LogWarning("{ClientException}", e.Message);

                return View();
            }
            catch (Exception e)
            {
                Logger.LogCritical("Critical error: {CriticalErrorStackTrace}", e.StackTrace);

                return RedirectToAction("Error", "Error");
            }
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            Logger.LogInformation($"Request: {Request.GetDisplayUrl()}");
            Logger.LogInformation($"Get Products Create page");

            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ModelImageViewModel viewModel)
        {
            Logger.LogInformation($"Request: {Request.GetDisplayUrl()}");
            Logger.LogInformation($"Post Product Create page");

            if (ModelState.IsValid)
            {
                try
                { 
                    var model = new EntityImageViewModel { Name = viewModel.Name };
                    using (var stream = new MemoryStream())
                    {
                        await viewModel.Image.CopyToAsync(stream);
                        model.Image = stream.ToArray();
                    }

                    model = await Client.SaveEntity(Options.Products, model);
                    Logger.LogTrace("Saved model: {SavedModel}", model);

                    return RedirectToAction(nameof(Index));
                }
                catch (ClientException e)
                {
                    Logger.LogWarning("{ClientException}", e.Message);
                    //set the error in the view
                    return View();
                }
                catch (Exception e)
                {
                    Logger.LogCritical("Critical error: {CriticalErrorStackTrace}", e.StackTrace);

                    return RedirectToAction("Error", "Error");
                }
            }
            else
            {
                return View(viewModel);
            }
        }


        // GET: Products/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }


        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            Logger.LogInformation($"Request: {Request.GetDisplayUrl()}");
            Logger.LogInformation($"Get Products Edit page");

            if (id <= 0)
            {
                // return message in toast
                Logger.LogWarning("Passed id must be > 0, id passed = {PassedId}", id);
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var model = await Client.GetEntity($"{Options.Products}/", id);
                return View(model);
            }
            catch (ClientException e)
            {
                Logger.LogWarning("{ClientException}", e.Message);
                //set the error in the view
                return View();
            }
            catch (Exception e)
            {
                Logger.LogCritical("Critical error: {CriticalErrorStackTrace}", e.StackTrace);

                return RedirectToAction("Error", "Error");
            }

           
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: Products/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Products/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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
    }
}