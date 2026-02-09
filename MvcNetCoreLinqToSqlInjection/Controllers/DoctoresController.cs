using Microsoft.AspNetCore.Mvc;
using MvcNetCoreLinqToSqlInjection.Models;
using MvcNetCoreLinqToSqlInjection.Repositories;

namespace MvcNetCoreLinqToSqlInjection.Controllers
{
    public class DoctoresController : Controller
    {
        //private RepositoryDoctoresSQLServer repo;
        //private RepositoryDoctoresOracle repo;
        IRepositoryDoctores repo;
        //RECIBIMOS NUESTRO REPOSITORY
        public DoctoresController(IRepositoryDoctores repo)
        {
            this.repo = repo;
        }
        public IActionResult Index()
        {
            List<Doctor> doctores = this.repo.GetDoctores();
            return View(doctores);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Doctor doc)
        {
            await this.repo.CreateDoctorAsync(doc.IdDoctor, doc.Apellido, doc.Especialidad, doc.Salario, doc.IdHospital);
            return RedirectToAction("index");
        }
        public async Task<IActionResult> Delete(int id)
        {
            await this.repo.DeleteDoctorAsync(id);
            return RedirectToAction("Index");
        }
        public async  Task<IActionResult> Update(int id)
        {
            Doctor doc = await this.repo.FindDoctor(id);
            return View(doc);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Doctor doc)
        {
            await this.repo.UpdateDoctorAsync(doc.IdDoctor, doc.Apellido, doc.Especialidad, doc.Salario, doc.IdHospital);
            return RedirectToAction("index");
        }

        public IActionResult DoctoresEspecialidad()
        {
            List<Doctor> doctores = this.repo.GetDoctores();
            return View(doctores);
        }
        [HttpPost]
        public IActionResult DoctoresEspecialidad(string especialidad)
        {
            List<Doctor> doctores = this.repo.GetDoctoresEspecialidad(especialidad);
            return View(doctores);
        }
    }
}
