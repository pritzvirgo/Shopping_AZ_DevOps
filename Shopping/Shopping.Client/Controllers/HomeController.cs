using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shopping.Client.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipelines;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Shopping.Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IHttpClientFactory httpClientFactory, ILogger<HomeController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpClient = httpClientFactory.CreateClient("ShoppingAPIClient");
        }

        public IActionResult Index()
        {
            var message = "Index";

            return View(message);
        }

        public async Task<IActionResult> Products()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/product");
                var content = await response.Content.ReadAsStringAsync();
                var productList = JsonConvert.DeserializeObject<IEnumerable<Product>>(content);
                return View("Products", productList);
            }
            catch (Exception ex)
            {
                var productList = new List<Product>() { new Product() {
                    Name = "Demo Product",
                    Description = "This is Demo Product, loresum description.",
                    ImageFile = "product-1.png",
                    Price = 1500.00M,
                    Category = "Alibaba Xing"
                }
                };
                return View("Products", productList);
            }
           
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
