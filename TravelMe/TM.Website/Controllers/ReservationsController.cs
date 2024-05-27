using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text;
using TM.Infrastructure.Messaging.Requests.UsersRequests;
using TM.Infrastructure.Messaging.Responses.UsersResponses;
using TM.Infrastructure.Messaging.Responses.ReservationsResponses;
using TM.Infrastructure.Messaging.Requests.ReservationsRequests;
using TM.Data.Entities;
using TM.Infrastructure.Messaging.Requests.TripRequests;
using TM.Website.Filters;
using System.Net.Sockets;
using System.Drawing.Printing;
using TM.Infrastructure.Messaging.Responses.TripResponses;
using TM.Website.Models;

namespace TM.Website.Controllers
{
    public class ReservationsController : Controller
    {
        private const int PageSize = 5;

        private readonly Uri uri = new("https://localhost:7166/api/reservations");
        HttpClient client;

        public ReservationsController()
        {
            client = new HttpClient();
            client.BaseAddress = uri;
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json;");
        }

        [LoggedUserFilter]
        [AdminFilter]
        public async Task<IActionResult> Index(int pageIndex = 1)
        {

            List<TripViewModel> modelList = new List<TripViewModel>();
            HttpResponseMessage response = client.GetAsync(uri + "/get").Result;

            if (response.IsSuccessStatusCode)
            {
                var jsonContent = await response.Content.ReadAsStringAsync();
                var responseData = System.Text.Json.JsonSerializer.Deserialize<GetReservationResponse>(jsonContent, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                var paginatedList = await PagenatedList<ReservationViewModel>.CreateAsync(responseData.Reservations, pageIndex, PageSize);
                return View(paginatedList);
            }

            return View();
        }


        [LoggedUserFilter]
        public IActionResult Create()
        {

            return View();
        }


        [HttpPost]
        [LoggedUserFilter]
        public async Task<IActionResult> Create([FromRoute] int id, ReservationModel reservation)
        {
            
            if (!AuthUser.LoggedUser.IsAdmin)
            {
                reservation.UserId = AuthUser.LoggedUser.Id;
                reservation.TripId = id;
            }

            using (HttpClient client = new())
            {
                client.BaseAddress = uri;

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Accept", "application/json");

                string data = JsonConvert.SerializeObject(reservation);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(uri + "/CreateReservation", content);

                ////ViewData
                //var users = client.GetAsync("https://localhost:7166/api/users/get").Result;
                //List<User> json = users.Content.ReadAsStringAsync().Result;



                //HttpResponseMessage usersResponse = await client.GetAsync(new Uri("https://localhost:7166/api/users/get"));

                //if (usersResponse.IsSuccessStatusCode)
                //{
                //    //return StatusCode(500, "ed");
                //    var jsonContent = await usersResponse.Content.ReadAsStringAsync();
                //    var responseData = System.Text.Json.JsonSerializer.Deserialize<GetUserResponse>(jsonContent, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

                //    if (responseData.Users is List<UserViewModel> userList)
                //    {
                //        ViewData["UsersId"] = userList;
                //    }
                //}

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

                return View(reservation);

            }
        }

        [LoggedUserFilter]
        [AdminFilter]
        public async Task<IActionResult> Delete(int? id)
        {
            client.BaseAddress = uri;

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json");


            HttpResponseMessage response = await client.DeleteAsync(uri + $"/DeleteReservation/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpGet]
        [LoggedUserFilter]
        [AdminFilter]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await client.GetAsync(uri + $"/GetReservationById/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var tripJson = await response.Content.ReadAsStringAsync();
            var trip = JsonConvert.DeserializeObject<ReservationModel>(tripJson);
            return View(trip);
        }


        [HttpPost]
        [ActionName("Save")]
        [LoggedUserFilter]
        [AdminFilter]
        public async Task<IActionResult> Edit(int id, ReservationModel model)
        {
            //return StatusCode(500, "first");
            if (!ModelState.IsValid)
            {
                //return StatusCode(500, "dsa");
                // Return the same view with the current model to show validation errors
                return View(model);
            }

            // Create the UpdateUserRequest object
            var updateRequest = new UpdateReservationRequest(id, model);

            //return StatusCode(500, updateRequest);

            string data = JsonConvert.SerializeObject(updateRequest);


            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PutAsync(uri + $"/UpdateReservation/{id}", content);

           // return StatusCode(500, response);

            if (response.IsSuccessStatusCode)
            {
                return StatusCode(500, "first");
                return RedirectToAction("Index");
            }
            //return StatusCode(500, "second");

            // Log the response or extract error messages to provide feedback to the user
            var errorMessage = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"Update failed. StatusCode: {response.StatusCode}, ReasonPhrase: {response.ReasonPhrase}, Error: {errorMessage}");

            return View("Edit", model);
        }

        [HttpGet]
        public async Task<IActionResult> SearchByUser(string username, int pageIndex = 1)
        {
            int id;

            HttpResponseMessage response2 = client.GetAsync("https://localhost:7166/api/users" + $"/SearchUserByUsername/{username}").Result;

                var jsonContent2 = await response2.Content.ReadAsStringAsync();

                var responseData2 = System.Text.Json.JsonSerializer.Deserialize<GetUserResponse>(jsonContent2, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

                PagenatedList<UserViewModel> paginatedList2 = await PagenatedList<UserViewModel>.CreateAsync(responseData2.Users, pageIndex, PageSize);

            id = paginatedList2[0].Id;


                HttpResponseMessage response = client.GetAsync(uri + $"/SearchReservationByUserId/{id}").Result;

            if (response.IsSuccessStatusCode)
            {
                var jsonContent = await response.Content.ReadAsStringAsync();

                var responseData = System.Text.Json.JsonSerializer.Deserialize<GetReservationResponse>(jsonContent, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

                PagenatedList<ReservationViewModel> paginatedList = await PagenatedList<ReservationViewModel>.CreateAsync(responseData.Reservations, pageIndex, PageSize);
                ViewData["CurrentFilter"] = username;
                return View("Index", paginatedList);
            }
            return View("Index");
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


        //[HttpGet]
        //public async Task<IActionResult> Edit(int id)
        //{
        //    var response = await client.GetAsync(uri + $"/GetReservationById/{id}");
        //    if (!response.IsSuccessStatusCode)
        //    {
        //        return NotFound();
        //    }

        //    var userJson = await response.Content.ReadAsStringAsync();
        //    var user = JsonConvert.DeserializeObject<ReservationModel>(userJson);
        //    return View(user);
        //}

        //[HttpPost]
        //[ActionName("Save")]
        //public async Task<IActionResult> Edit(int id, ReservationModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        // Return the same view with the current model to show validation errors
        //        return View(model);
        //    }

        //    // Create the UpdateUserRequest object
        //    var updateRequest = new UpdateReservationRequest(id, model);

        //    string data = JsonConvert.SerializeObject(updateRequest);
        //    StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

        //    HttpResponseMessage response = await client.PutAsync(uri + $"/UpdateReservation/{id}", content);

        //    //return StatusCode(500, data);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        return RedirectToAction("Index");
        //    }

        //    // Log the response or extract error messages to provide feedback to the user
        //    var errorMessage = await response.Content.ReadAsStringAsync();
        //    ModelState.AddModelError(string.Empty, $"Update failed. StatusCode: {response.StatusCode}, ReasonPhrase: {response.ReasonPhrase}, Error: {errorMessage}");

        //    return View(model);
        //}
    }
}
