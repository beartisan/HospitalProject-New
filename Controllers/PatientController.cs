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
using hospital_project.Migrations;
using System.Web.UI.WebControls;
using System.Security.Policy;


namespace hospital_project.Controllers
{
    public class PatientController : Controller
    {

        private static readonly HttpClient client;
        static PatientController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44324/api/PatientData/");
        }

        // GET: Patient/List
        public ActionResult List()
        {
            //objective: communicate to our patient data API to retrieve list of patients
            //curl https://localhost:44324/api/PatientData/ListPatients

            //HttpClient client = new HttpClient();
            string url = "listpatients";
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

            DetailsPatient ViewModel = new DetailsPatient();

            //HttpClient client = new HttpClient();
            string url = "FindPatient/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            
            //Debug.WriteLine(response.StatusCode);
            
            PatientDto SelectedPatient = response.Content.ReadAsAsync<PatientDto>().Result;


            ViewModel.SelectedPatient = SelectedPatient;

            url = "listphysiciansforpatients/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<PhysicianDto> PrimaryPhysician = response.Content.ReadAsAsync<IEnumerable<PhysicianDto>>().Result;

            ViewModel.PrimaryPhysician = PrimaryPhysician;
            url = "listphysiciansnotforpatients/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<PhysicianDto> AvailablePhysicians = response.Content.ReadAsAsync<IEnumerable<PhysicianDto>>().Result;

            ViewModel.AvailablePhysicians = AvailablePhysicians;

            return View(ViewModel);
        }

        //POST: Patient/Associate/{patient_id}
        [HttpPost]
        public ActionResult Associate(int id, int physician_id)
        {
            //call api to our patient with associated physician

            string url = "patientwithassociatephysician/" + id + "/" + physician_id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);

        }


        //GET: Patient/UnAssociate/{patient_id}?PhysicianID={physician_id}
        [HttpGet]
        public ActionResult UnAssociate(int id, int physician_id)
        {
            //call api to our patient with associated physician

            string url = "patientwithunassociatephysician/" + id + "/" + physician_id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);

        }

        public ActionResult Error()
        { 
            return View();
        }



        // GET: Patient/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Patient/Create
        [HttpPost]
        public ActionResult Create(Patient patient)
        {
            //Debug.WriteLine("the inputed patient name is: ");
            //Debug.WriteLine(patient.patient_fname + patient.patient_surname);
            //objective: Add a new patient into our system using the API
            //curl -H "Content-Type: application/json" -d @patient.json https://localhost:44324/api/PatientData/AddPatient/

            string url = "addpatient";

            JavaScriptSerializer jss = new JavaScriptSerializer();
            string jsonpayload = jss.Serialize(patient);

            //Debug.WriteLine(jsonpayload);
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

        // GET: Patient/Edit/2
        public ActionResult Edit(int id)
        {
            string url = "FindPatient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            PatientDto SelectedPatient = response.Content.ReadAsAsync<PatientDto>().Result;

            return View(SelectedPatient);
        }

        // POST: Patient/Update/2
        [HttpPost]
        [Authorize]
        public ActionResult Edit(int id, Patients patient)
        {
            string jsonpayload = jss.Serialize(patient);
            string url = "patientdata/updatepatient/" + id;

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

        // GET: Patient/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
           
            string url = "patientdata/findpatient/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;
            PatientDto SelectedPatient = response.Content.ReadAsAsync<PatientDto>().Result;

            return View(SelectedPatient);
        }

        // POST: Patient/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id, FormCollection collection)
        {
            string url = "patientdata/deletepatient/" + id;
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
