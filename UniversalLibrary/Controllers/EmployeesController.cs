using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniversalLibrary.Data;
using UniversalLibrary.Data.Entities;
using UniversalLibrary.Helpers;
using UniversalLibrary.Models;

namespace UniversalLibrary.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;

        public EmployeesController(DataContext context, IUserHelper userHelper, IEmployeeRepository employeeRepository, IImageHelper imageHelper, IConverterHelper converterHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _employeeRepository = employeeRepository;
            _imageHelper = imageHelper;
            _converterHelper = converterHelper;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            return View(_employeeRepository.GetAll().OrderBy(p => p.FirstName)); //vai ao repositorio e trás td os employees e ordena por nome
        }

        // GET: Employees/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("EmployeeNotFound");
            }

            var employee = await _employeeRepository.GetByIdAsync(id.Value);

            if (employee == null)
            {
                return new NotFoundViewResult("EmployeeNotFound");
            }

            return View(employee);
        }

        // GET: Employees/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0) //se o model tiver uma imagem
                {
                    //usar este método do imagehelper -> envia o ficheiro e guarda nessa pasta -> books                    
                    path = await _imageHelper.UploadImageAsync(model.ImageFile, "employees");
                }

                //converter o EmployeeViewModel em Employee -> para continuar a gravar o Book na BD
                var employee = _converterHelper.ToEmployee(model, path, true); //ainda n tem id -> fica true

                //Associar o user ao Employee, antes de este ser criado                
                employee.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                await _employeeRepository.CreateAsync(employee); //se for válido -> fica guardado em memória
                return RedirectToAction(nameof(Index)); //qd estiver gravado -> redireciona para a action index -> mostra a lista de empregados                               
            }
            return View(model); //se n passar na validação -> deixa os dados nos campos mas n os grava
        }

        // GET: Employees/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("EmployeeNotFound");
            }

            //Dupla Segurança
            var employee = await _employeeRepository.GetByIdAsync(id.Value); //vai ver na memória se -> o id colocado no url existe ou n

            if (employee == null)
            {
                return new NotFoundViewResult("EmployeeNotFound");
            }

            //converter o Employee em EmployeeViewModel -> para aparecer a imagem
            var model = _converterHelper.ToEmployeeViewModel(employee);

            return View(model);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var path = model.ImageEmployee; //n é empty como no create -> para o caso de n querer alterar a imagem

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        path = await _imageHelper.UploadImageAsync(model.ImageFile, "employees");
                    }

                    var employee = _converterHelper.ToEmployee(model, path, false); //já tem id -> fica false


                    employee.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                    await _employeeRepository.UpdateAsync(employee);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await _employeeRepository.ExistAsync(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Employees/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("EmployeeNotFound");
            }

            var employee = await _employeeRepository.GetByIdAsync(id.Value);

            if (employee == null)
            {
                return new NotFoundViewResult("EmployeeNotFound");
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            try
            {
                await _employeeRepository.DeleteAsync(employee);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                if (true)
                {
                    ViewBag.ErrorTitle = $"{employee.FirstName}, may be on vacations.";
                    ViewBag.ErrorMessage = $"{employee.FirstName}, Wait until he returns </br></br>" +
                        $"Once he returns, try deleting the emloyee.";
                }

                return View("Error");
            }
        }


        public IActionResult EmployeeNotFound()
        {
            return View();
        }


        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
