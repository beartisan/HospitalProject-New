using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Diagnostics;
using hospital_project.Models;
using hospital_project.Models.ViewModels;


namespace hospital_project.Controllers
{
    public class PatientController : Controller
    {

        private static readonly HttpClient client;
        static PatientController()
        {
            client = new HttpClient();
        }

        // GET: Patient/List
        public ActionResult List()
        {
            //objective: communicate to our patient data API to retrieve list of patients
            //curl https://localhost:44324/api/PatientData/ListPatients

            //HttpClient client = new HttpClient();
            string url = "patientdata/listpatients";
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<PatientDto> patients = response.Content.ReadAsAsync<IEnumerable<PatientDto>>().Result;


            return View(patients);
        }

        // GET: Patient/Details/2
        public ActionResult Details(int id)
        {
            //objective: communicate to our patient data API to retrieve a specific patient
            //curl https://localhost:44324/api/PatientData/FindPatient/{id}

            //HttpClient client = new HttpClient();
            string url = "https://localhost:44324/api/PatientData/FindPatient/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Debug.WriteLine(response.StatusCode);

            PatientDto SelectedPatient = response.Content.ReadAsAsync<PatientDto>().Result;

            return View(SelectedPatient);
        }

        // GET: Patient/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Patient/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Patient/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Patient/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Patient/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Patient/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
