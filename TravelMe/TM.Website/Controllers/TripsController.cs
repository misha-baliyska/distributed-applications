using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
using TM.Data.Entities;
using TM.Infrastructure.Messaging.Requests.TripRequests;
using TM.Infrastructure.Messaging.Requests.UsersRequests;
using TM.Infrastructure.Messaging.Responses.TripResponses;
using TM.Infrastructure.Messaging.Responses.UsersResponses;
using TM.Website.Filters;
using TM.Website.Models;

namespace TM.Website.Controllers
{
    public class TripsController : Controller
    {
        //private readonly Uri uri = new("https://localhost:7166/api/trips");
        //HttpClient client;

        //public TripsController()
        //{
        //    client = new HttpClient();
        //    client.BaseAddress = uri;
        //}


        //public IActionResult Index()
        //{
        //    List<TripViewModel> modelList = new List<TripViewModel>();
        //    HttpResponseMessage response = client.GetAsync(uri + "/trips").Result;

        //    if (response.IsSuccessStatusCode)
        //    {
        //        string data = response.Content.ReadAsStringAsync().Result;
        //        modelList = JsonConvert.DeserializeObject<List<TripViewModel>>(data);
        //    }

        //    return View(modelList);
        //}

        //public IActionResult Create()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> Create(UserModel user)
        //{
        //    var createUserRequest = new CreateUserRequest(user);

        //    string data = JsonConvert.SerializeObject(createUserRequest);
        //    StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

        //    HttpResponseMessage response = await client.PostAsync("/users", content);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        return RedirectToAction("Index");
        //    }

        //    // Optionally, handle errors here. For example:
        //    //ModelState.AddModelError(string.Empty, "An error occurred while creating the user.");
        //    return View(user);
        //}

        private const int PageSize = 10;

        private readonly Uri uri = new("https://localhost:7166/api/trips");
        HttpClient client;

        public TripsController()
        {
            client = new HttpClient();
            client.BaseAddress = uri;
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json;");
        }

        [LoggedUserFilter]
        public async Task<IActionResult> Index(int pageIndex = 1)
        {
            List<TripViewModel> modelList = new List<TripViewModel>();
            HttpResponseMessage response = client.GetAsync(uri + "/get").Result;

            if (response.IsSuccessStatusCode)
            {
                var jsonContent = await response.Content.ReadAsStringAsync();
                var responseData = System.Text.Json.JsonSerializer.Deserialize<GetTripResponse>(jsonContent, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                var paginatedList = await PagenatedList<TripViewModel>.CreateAsync(responseData.Trips, pageIndex, PageSize);
                return View(paginatedList);
            }

            return View();
        }

        //public async Task<IActionResult> Index(int a, int pageIndex = 1)
        //{
        //    var products = _context.Products.AsNoTracking();
        //    var paginatedList = await PaginatedList<Product>.CreateAsync(products, pageIndex, PageSize);
        //    return View(paginatedList);
        //}

        [LoggedUserFilter]
        [AdminFilter]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [LoggedUserFilter]
        [AdminFilter]
        public async Task<IActionResult> Create(TripModel trip)
        {

            using (HttpClient client = new())
            {
                client.BaseAddress = uri;

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Accept", "application/json");

                string data = JsonConvert.SerializeObject(trip);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(uri + "/CreateTrip", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

                return View(trip);

            }
        }

        [LoggedUserFilter]
        [AdminFilter]
        public async Task<IActionResult> Delete(int? id)
        {
            client.BaseAddress = uri;

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json");


            HttpResponseMessage response = await client.DeleteAsync(uri + $"/DeleteTrip/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View();
        }


        //[HttpGet]
        //public async Task<IActionResult> Edit(int id)
        //{
        //    var response = await client.GetAsync(uri + $"/GetUserById/{id}");
        //    if (!response.IsSuccessStatusCode)
        //    {
        //        return NotFound();
        //    }

        //    var userJson = await response.Content.ReadAsStringAsync();
        //    var user = JsonConvert.DeserializeObject<UserModel>(userJson);
        //    return View(user);
        //}


        [HttpGet]
        [LoggedUserFilter]
        [AdminFilter]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await client.GetAsync(uri + $"/GetTripById/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var tripJson = await response.Content.ReadAsStringAsync();
            var trip = JsonConvert.DeserializeObject<TripModel>(tripJson);
            return View(trip);
        }

        [HttpPost]
        [ActionName("Save")]
        [LoggedUserFilter]
        [AdminFilter]
        public async Task<IActionResult> Edit(int id, TripModel model)
        {
            if (!ModelState.IsValid)
            {
                // Return the same view with the current model to show validation errors
                return View(model);
            }

            // Create the UpdateUserRequest object
            var updateRequest = new UpdateTripRequest(id, model);

            string data = JsonConvert.SerializeObject(updateRequest);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PutAsync(uri + $"/UpdateTrip/{id}", content);

            //return StatusCode(500, data);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            // Log the response or extract error messages to provide feedback to the user
            var errorMessage = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"Update failed. StatusCode: {response.StatusCode}, ReasonPhrase: {response.ReasonPhrase}, Error: {errorMessage}");

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> SearchByDestination(string destination, int pageIndex = 1)
        {

            HttpResponseMessage response = client.GetAsync(uri + $"/SearchTripByDestination/{destination}").Result;

            if (response.IsSuccessStatusCode)
            {
                var jsonContent = await response.Content.ReadAsStringAsync();

                var responseData = System.Text.Json.JsonSerializer.Deserialize<GetTripResponse>(jsonContent, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

                PagenatedList<TripViewModel> paginatedList = await PagenatedList<TripViewModel>.CreateAsync(responseData.Trips, pageIndex, PageSize);
                ViewData["CurrentFilter"] = destination;
                return View("Index", paginatedList);
            }
            return View("Index");
        }
    }
}
