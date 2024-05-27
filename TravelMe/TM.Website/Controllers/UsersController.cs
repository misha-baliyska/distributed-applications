using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using System;
using TM.Infrastructure.Messaging.Responses.UsersResponses;
using NuGet.Common;
using Newtonsoft.Json;
using System.Text;
using TM.Infrastructure.Messaging.Requests.UsersRequests;
using Microsoft.EntityFrameworkCore;
using TM.Data.Entities;
using TM.Website.Filters;
using System.Drawing.Printing;
using TM.Website.Models;
using TM.Infrastructure.Messaging.Responses.TripResponses;

namespace TM.Website.Controllers
{
    public class UsersController : Controller
    {
        private const int PageSize = 1;

        private readonly Uri uri = new("https://localhost:7166/api/users");
        HttpClient client;

        public UsersController()
        {
            client = new HttpClient();
            client.BaseAddress = uri;
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json;");
        }

        [LoggedUserFilter]
        [AdminFilter]
        public async Task<IActionResult> Index(int pageIndex = 1)
        {
            HttpResponseMessage response = client.GetAsync(uri + "/Get").Result;

            if (response.IsSuccessStatusCode)
            {
                var jsonContent = await response.Content.ReadAsStringAsync();
                var responseData = System.Text.Json.JsonSerializer.Deserialize<GetUserResponse>(jsonContent, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                PagenatedList<UserViewModel> paginatedList = await PagenatedList<UserViewModel>.CreateAsync(responseData.Users, pageIndex, PageSize);
                return View(paginatedList);
            }

            return View();
        }


        [LoggedUserFilter]
        [AdminFilter]
        public IActionResult Create()
        {
            //return StatusCode(500, AuthUser.LoggedUser.FirstName);
            return View();
        }


        [HttpPost]
        [LoggedUserFilter]
        [AdminFilter]
        public async Task<IActionResult> Create(UserModel user)
        {
            
            using (HttpClient client = new())
            {
                client.BaseAddress = uri;

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Accept", "application/json");

                string data = JsonConvert.SerializeObject(user);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(uri + "/CreateUser", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

                return View(user);

            }
        }

        [LoggedUserFilter]
        [AdminFilter]
        public async Task<IActionResult> Delete(int? id)
        {
            client.BaseAddress = uri;

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json");


            HttpResponseMessage response = await client.DeleteAsync(uri + $"/DeleteUser/{id}");

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
            var response = await client.GetAsync(uri + $"/GetUserById/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var userJson = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<UserModel>(userJson);
            return View(user);
        }


        [HttpPost]
        [ActionName("Save")]
        [LoggedUserFilter]
        [AdminFilter]
        public async Task<IActionResult> Edit(int id, UserModel model)
        {
            if (!ModelState.IsValid)
            {
                // Return the same view with the current model to show validation errors
                return View(model);
            }

            // Create the UpdateUserRequest object
            var updateRequest = new UpdateUserRequest(id, model);

            string data = JsonConvert.SerializeObject(updateRequest);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PutAsync(uri + $"/UpdateUser/{id}", content);

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

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserModel model)
        {
            List<UserViewModel> modelList = new List<UserViewModel>();
            HttpResponseMessage response = client.GetAsync(uri + "/Get").Result;

            if (response.IsSuccessStatusCode)
            {
                var jsonContent = await response.Content.ReadAsStringAsync();
                var responseData = System.Text.Json.JsonSerializer.Deserialize<GetUserResponse>(jsonContent, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                UserViewModel loggedUser = responseData.Users.FirstOrDefault(user => user.Username == model.Username && user.Password == model.Password);
                if (loggedUser == null)
                {
                    ModelState.AddModelError("login error", "Wrong Username or Password");
                    return View(model);
                }
                HttpContext.Session.SetInt32("loggedUserId", loggedUser.Id);
                AuthUser.LoggedUser = loggedUser;
                return RedirectToAction("Index", "Home");
            }
            return View();

        }

        [HttpGet]
        public async Task<IActionResult> SearchByUsername(string name, int pageIndex = 1)
        {

            HttpResponseMessage response = client.GetAsync(uri + $"/SearchUserByUsername/{name}").Result;

            if (response.IsSuccessStatusCode)
            {
                var jsonContent = await response.Content.ReadAsStringAsync();

                var responseData = System.Text.Json.JsonSerializer.Deserialize<GetUserResponse>(jsonContent, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

                PagenatedList<UserViewModel> paginatedList = await PagenatedList<UserViewModel>.CreateAsync(responseData.Users, pageIndex, PageSize);
                ViewData["CurrentFilter"] = name;
                return View("Index", paginatedList);
            }
            return View("Index");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserModel user)
        {
            HttpResponseMessage usersResponse = client.GetAsync(uri + "/Get").Result;

            if (usersResponse.IsSuccessStatusCode)
            {
                var jsonContent = await usersResponse.Content.ReadAsStringAsync();
                var responseData = System.Text.Json.JsonSerializer.Deserialize<GetUserResponse>(jsonContent, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

                if (responseData.Users.FirstOrDefault(user => user.Username == user.Username) != null)
                {
                    ModelState.AddModelError("User error", "This username is taken");
                    return View(user);
                }
                if (responseData.Users.FirstOrDefault(user => user.Email == user.Email) != null)
                {
                    ModelState.AddModelError("User error", "This mail is already taken");
                    return View(user);
                }
            }

            using (HttpClient client = new())
            {
                client.BaseAddress = uri;

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Accept", "application/json");

                string data = JsonConvert.SerializeObject(user);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(uri + "/CreateUser", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

                return View(user);

            }
        }

        [LoggedUserFilter]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            AuthUser.LoggedUser = null;
            //return StatusCode(500, AuthUser.LoggedUser.FirstName);
            return RedirectToAction("Index");
        }
    }
}

