using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KioskApplication.Models;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace KioskApplication.Controllers
{
    public class HomeController : Controller
    {
        private const string API_ROUTE = "http://membershipsystem20180613091139.azurewebsites.net/api/members/";
        private static HttpClient client = new HttpClient();
        private User LoggedInUser;
        private DateTime lastActionTime;

        [HttpGet]
        public IActionResult Index()
        {
            if (LoggedInUser == null)
            {
                return View();
            }
            else
            {
                DateTime now = DateTime.Now;
                TimeSpan duration = now - lastActionTime;
                if (duration.TotalMinutes > 1)
                {
                    LoggedInUser = null;
                    ViewData["Message"] = "You have been timed out. Please log in again.";
                    return View();
                }
                else
                {
                    return Index(LoggedInUser.Name, LoggedInUser.Pin);
                }                
            }
        }

        [HttpPost]
        public IActionResult Index(string id, string pin)
        {
            User usr;
            if (LoggedInUser!=null && LoggedInUser.Id == id && LoggedInUser.Pin == pin)
            {
                usr = LoggedInUser;
            }
            else
            {
                usr = new User() { Id = id, Pin = pin };
            }
            
            string NextView = null;

            if (id != null)
            {
                if (pin != null)
                {
                    if (Get(id, pin, usr))
                    {
                        SetLoggedInUser(usr);
                        NextView = "~/Views/Home/LoggedInWithPinIndex.cshtml";
                    }
                    
                }
                else
                {
                    if (Get(id, usr))
                    {
                        SetLoggedInUser(usr);
                        NextView = "~/Views/Home/LoggedInIndex.cshtml";
                    }
                }
            }

            return (NextView==null)? View() : View(NextView);
        }

        private bool Get(string id, User usr)
        {
            usr.Name = id;
            return true;
            var response = client.GetAsync(API_ROUTE + id).Result;
            if (response.IsSuccessStatusCode)
            {
                JObject jobj = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                usr.Name = jobj.Value<string>("name");
            }
            return response.IsSuccessStatusCode;
        }

        private bool Get(string id, string pin, User usr)
        {
            usr.Name = id;
            usr.Pin = pin;
            return true;
            var response = client.GetAsync(API_ROUTE + id + "&pin=" + pin).Result;
            if (response.IsSuccessStatusCode)
            {
                JObject jobj = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                usr.Name = jobj.Value<string>("name");
                usr.Balance = jobj.Value<int>("balance");
            }

            return response.IsSuccessStatusCode;
        }

        private void SetLoggedInUser(User user)
        {
            LoggedInUser = user;
            lastActionTime = DateTime.Now;
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
