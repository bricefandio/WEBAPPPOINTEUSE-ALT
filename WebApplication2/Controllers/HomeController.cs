using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Data;
using System.Threading.Tasks;
using WebApplication2.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using WebApplication1.Models;


namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        string baseURL = "http://localhost:5045/";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            List<Eleve> eleves = new List<Eleve>();
            List<Promotion> promotions = new List<Promotion>();
            List<Groupe> groupes = new List<Groupe>();
            using (var client = new HttpClient())

            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getData = await client.GetAsync("api/Eleves");
                HttpResponseMessage getDataPromotion = await client.GetAsync("api/Promotions");
                HttpResponseMessage getDataGroupe = await client.GetAsync("api/Groupes");

                if (getData.IsSuccessStatusCode)
                {
                    string results = getData.Content.ReadAsStringAsync().Result;
                    eleves = JsonConvert.DeserializeObject<List<Eleve>>(results);

                    string resultgroup = getDataPromotion.Content.ReadAsStringAsync().Result;
                    promotions = JsonConvert.DeserializeObject<List<Promotion>>(resultgroup);

                    string resultsprom = getDataGroupe.Content.ReadAsStringAsync().Result;
                    groupes = JsonConvert.DeserializeObject<List<Groupe>>(resultsprom);


                }
                else
                {
                    Console.WriteLine("erreur calling web API");
                }

                ViewData["Eleve"] = eleves;
                ViewData["Promotion"] = promotions;
                ViewData["Groupe"] = groupes;


            }
            return View();
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