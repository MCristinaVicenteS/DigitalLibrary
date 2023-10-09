using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    [Authorize] //é preciso login para aceder aos loans -> ATENÇÃO-> dp de definir roles -> só o admin e o reader
    public class LoanOnlinesController : Controller
    {
        private readonly DataContext _context;
        private readonly ILoanOnlineRepository _loanOnlineRepository;
        private readonly IBookOnlineRepository _bookOnlineRepository;

        public LoanOnlinesController(DataContext context, ILoanOnlineRepository loanOnlineRepository, IBookOnlineRepository bookOnlineRepository)
        {
            _context = context;
            _loanOnlineRepository = loanOnlineRepository;
            _bookOnlineRepository = bookOnlineRepository;
        }


        // GET: LoanOnlines
        public async Task<IActionResult> Index()
        {
            //sincronizar os loans com o user
            var model = await _loanOnlineRepository.GetLoanOnlineAsync(this.User.Identity.Name);
            return View(model);
        }      


        //****QD clicar no botão create -> aparece a view com a lista temprária de livros do user****
        // GET: LoanOnlines/Create
        public async Task<IActionResult> Create()
        {
            var model = await _loanOnlineRepository.GetDetailTempAsync(this.User.Identity.Name);
            return View(model);
        }


        //****Traz os itens através do _bookOnlineRepository; e usar o modelo AddItemViewModel -> combobox ****
        public IActionResult AddBook()
        {
            var model = new AddItemViewModel
            {
                Books = _bookOnlineRepository.GetComboBooks()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddBook(AddItemViewModel model)
        {
            //o book vai receber um modelo -> se for válido -> passo-lhe o modelo e o user
            if(ModelState.IsValid)
            {
                await _loanOnlineRepository.AddItemToLoanAsync(model, this.User.Identity.Name);
                return RedirectToAction("Create");
            }

            //se correr alg coisa mal -> retorna a view e o user preenche outra vez
            return View(model);
        }


        public async Task <IActionResult> DeleteItem(int ? id)
        {
            if(id == null)
            {
                return new NotFoundViewResult("LoanOnlineNotFound");
            }

            await _loanOnlineRepository.DeleteDetailTempAsync(id.Value);
            return RedirectToAction("Create");
        }


        //Após confirmar a encomenda -> redireciona para o index
        public async Task<IActionResult> ConfirmLoan()
        {
            var response = await _loanOnlineRepository.ConfirmLoanAsync(this.User.Identity.Name);
            if(response)
            {
                return RedirectToAction("Index");
            }

            //se n correr bem -> redireciona para o create
            return RedirectToAction("Create");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Accept(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("LoanOnlineNotFound");
            }

            var loan = await _loanOnlineRepository.GetLoanOnlineAsync(id.Value);

            if(loan == null)
            {
                return new NotFoundViewResult("LoanOnlineNotFound");
            }

            DateTime today = DateTime.Now;
            DateTime limitLoanDate = today.AddDays(30);

            var model = new FinishBookViewModel
            {
                Id = loan.Id,
                LimitLoanDate = limitLoanDate  //VERIFICAR se o calculo do dia fica no calendário -> senão tirar o calendário da viewmodel
            };

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> Accept(FinishBookViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _loanOnlineRepository.AcceptLoanOnline(model);
                return RedirectToAction("Index");
            }                       

            return View();
        }


        // GET: LoanOnlines/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {          
            if (id == null)
            {
                return new NotFoundViewResult("LoanOnlineNotFound");
            }

            var loan = await _loanOnlineRepository.GetLoanOnlineAsync(id.Value);
            if (loan == null)
            {
                return new NotFoundViewResult("LoanOnlineNotFound");
            }
            
            return View(loan);
        }


        // POST: LoanOnlines/Delete/5
        [HttpPost, ActionName("Delete")]
        ////[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int? id)
        {
            var loan = await _loanOnlineRepository.GetLoanOnlineAsync(id.Value);

            try
            {
                await _loanOnlineRepository.DeleteDetailTempAsync(id.Value);           //CRIAR LIMITAÇÕES.
                return RedirectToAction(nameof(Index));
            }

            catch (DbUpdateException ex)
            {
                if (true)
                {
                    ViewBag.ErrorTitle = $"{loan.User}, maybe being used.";
                    ViewBag.ErrorMessage = $"{loan.User}, cannot be deleted until the loan is in role. </br></br>" +
                        $"Once the loan ends, try deleting it.";
                }

                return View("Error");
            }
        }
                

        // GET: LoanOnlines/AddItemToCart/5
        public async Task<IActionResult> AddItemToCart(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("LoanOnlineNotFound");
            }

            var item = await _bookOnlineRepository.GetByIdAsync(id.Value);

            string user = _context.Users.FirstOrDefault()?.Id;

            await _loanOnlineRepository.AddItemToCartLoanAsync(item, this.User.Identity.Name);


         

            return View();
        }

        

        public IActionResult LoanOnlineNotFound()
        {
            return View();
        }
    }
}
