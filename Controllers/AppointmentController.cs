using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using hospital_project.Models;
using hospital_project.Models.ViewModels;
using System.Diagnostics;

using Microsoft.AspNet.Identity;

namespace hospital_project.Controllers
{
    public class AppointmentController : Controller
    {
        private static readonly HttpClientCertificate client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static AppointmentController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44324/api/appointmentdata/");
        }

        // GET: Appointment/List
        public ActionResult List()
        {
            //objective: communicate to appointments data api to retrieve a list of appointments
            // curl https://localhost:44324/api/AppointmentData/ListApppointments

            string url = "listappointments";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<AppointmentDto> Appointments = response.Content.ReadAsAsync<IEnumerable<AppointmentDto>>().Result;

            return View(Appointments);
        }

        // GET: Appointment/Details/1
        public ActionResult Details(int id)
        {
            //objective: look for the appointment through appointments data api
            // curl https://localhost:44324/api/AppointmentData/FindApppointment/{id}

            DetailsAppointment ViewModel = new DetailsAppointment();

            string url = "findappointments/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            AppointmentDto SelectedAppointment = response.Content.ReadAsAsync<AppointmentDto>().Result;
            ViewModel.SelectedAppointment = SelectedAppointment;

            return View(ViewModel);
        }

        // GET: Appointment/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Appointment/Create
        [HttpPost]
        public ActionResult Create(Appointment Appointment)
        {
            //objective: create a ne appointment to add in the list of data api
            // curl -H "Content-Type: application/json" -d @Appointment.json https://localhost:44324/api/AppointmentData/AddApppointment

            string url = "addappointment";
            string jsonpayload = jss.Serialize(Appointment);

            HttpContent content = new StringContent(jsonpayload);

            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }

        }

        // GET: Appointment/Edit/5
        public ActionResult Edit(int id)
        {
            //objective: look for the appointment through appointments data api

            string url = "findappointment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            AppointmentDto SelectedAppointment = response.Content.ReadAsAsync<AppointmentDto>().Result;

            return View(SelectedAppointment);
        }

        // POST: Appointment/Update/1
        [HttpPost]
        public ActionResult Update(int id, Appointment Appointment)
        {
            //objective: look for the appointment through appointments data api

            string url = "updateappointment/" + id;
            string jsonpayload = jss.Serialize(Appointment);

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            HttpContent content = new StringContent(jsonpayload);
            AppointmentDto SelectedAppointment = response.Content.ReadAsAsync<AppointmentDto>().Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }


        }

        // GET: Appointment/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            //objective: delete the appointment through appointments data api

            string url = "findappointment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            AppointmentDto SelectedAppointment = response.Content.ReadAsAsync<AppointmentDto>().Result;

            return View(SelectedAppointment);
        }

        // POST: Appointment/Delete/1
        [HttpPost]
        public ActionResult Delete(int id)
        {
            //objective: delete the appointment through appointments data api

            string url = "deleteappointment/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }

        }
    }
}
