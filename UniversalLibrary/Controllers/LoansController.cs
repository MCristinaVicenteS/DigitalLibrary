using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Cms;
using UniversalLibrary.Data;
using UniversalLibrary.Data.Entities;
using UniversalLibrary.Helpers;
using UniversalLibrary.Models;
using Vereyon.Web;

namespace UniversalLibrary.Controllers
{
    //EMPRESTIMO FÍSICO

        [Authorize] //é preciso login para aceder aos loans -> qd definir os roles -> só o admin e o employee
        public class LoansController : Controller
        {
            private readonly DataContext _context;
            private readonly ILoanRepository _loanRepository;
            private readonly IBookRepository _bookRepository;

            public LoansController(DataContext context, ILoanRepository loanRepository, IBookRepository bookRepository)
            {
                _context = context;
                _loanRepository = loanRepository;
                _bookRepository = bookRepository;
            }


            // GET: Loan
            public async Task<IActionResult> Index()
            {
                //sincronizar os loans com o user
                var model = await _loanRepository.GetLoanAsync(this.User.Identity.Name);
                return View(model);
            }


            //****QD clicar no botão create -> aparece a view com a lista temprária de livros do user****
            // GET: Loan/Create
            public async Task<IActionResult> Create()
            {
                var model = await _loanRepository.GetDetailTempAsync(this.User.Identity.Name);
                return View(model);
            }


            //****Traz os itens através do _bookRepository; e usar o modelo AddItemViewModel -> combobox ****
            public IActionResult AddBook()
            {
                var model = new AddItemViewModel
                {
                    Books = _bookRepository.GetComboBooks()
                };

                return View(model);
            }

            [HttpPost]
            public async Task<IActionResult> AddBook(AddItemViewModel model)
            {
                //o book vai receber um modelo -> se for válido -> passo-lhe o modelo e o user
                if (ModelState.IsValid)
                {
                    await _loanRepository.AddItemToLoanAsync(model, this.User.Identity.Name);
                    return RedirectToAction("Create");
                }

                //se correr alg coisa mal -> retorna a view e o user preenche outra vez
                return View(model);
            }


            public async Task<IActionResult> DeleteItem(int? id)
            {
                if (id == null)
                {
                    return new NotFoundViewResult("LoanNotFound");
                }

                await _loanRepository.DeleteDetailTempAsync(id.Value);
                return RedirectToAction("Create");
            }


            //Após confirmar a encomenda -> redireciona para o index
            public async Task<IActionResult> ConfirmLoan()
            {
                var response = await _loanRepository.ConfirmLoanAsync(this.User.Identity.Name);
                if (response)
                {
                    return RedirectToAction("Index");
                }

                //se n correr bem -> redireciona para o create
                return RedirectToAction("Create");
            }


            [Authorize/*(Roles = "Admin")*/]
            public async Task<IActionResult> Accept(int? id)
            {
                if (id == null)
                {
                    return new NotFoundViewResult("LoanNotFound");
                }

                var loan = await _loanRepository.GetLoanAsync(id.Value);

                if (loan == null)
                {
                    return new NotFoundViewResult("LoanNotFound");
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
                    await _loanRepository.AcceptLoan(model);
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
                    return new NotFoundViewResult("LoanNotFound");
                }

                var loan = await _loanRepository.GetLoanAsync(id.Value);
                if (loan == null)
                {
                    return new NotFoundViewResult("LoanNotFound");
                }

                return View(loan);
            }


            // POST: LoanOnlines/Delete/5
            [HttpPost, ActionName("Delete")]
            ////[ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirm(int? id)
            {
                var loan = await _loanRepository.GetLoanAsync(id.Value);

                try
                {
                    await _loanRepository.DeleteDetailTempAsync(id.Value);           //CRIAR LIMITAÇÕES.
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

            public IActionResult LoanNotFound()
            {
                return View();
            }

        }
}
