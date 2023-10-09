using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversalLibrary.Data.Entities;
using UniversalLibrary.Helpers;
using UniversalLibrary.Models;

namespace UniversalLibrary.Data
{
    public class LoanRepository : GenericRepository<Loan>, ILoanRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public LoanRepository(DataContext contex, IUserHelper userHelper) : base(contex)
        {
            _context = contex;
            _userHelper = userHelper;
        }

        public IQueryable GetAllWithUsers()
        {
            return _context.Loans.Include(p => p.User);
        }


        public async Task<IQueryable<Loan>> GetLoanAsync(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);

            if (user == null)
            {
                return null;
            }

            //se existir user -> ver qual é o role para definir os acessos
            //role admin tem acesso a todos os Loans
            if (await _userHelper.IsUserInRoleAsync(user, "Admin"))
            {
                //se for true -> vou buscar td as encomendas à BD (tabela LoansOnline)
                //incluir od itens e os books q estão na tabela

                return _context.Loans
                    .Include(i => i.Items)                //tem ligação directa
                    .ThenInclude(i => i.Book)             //n tem ligação directa
                    .OrderByDescending(i => i.LoanDate);
            }

            //qd associar roles -> inserir else if -> para o role employwee
            return _context.Loans
                .Include(i => i.Items)
                .ThenInclude(b => b.Book)
                .Where(u => u.User == user)
                .OrderByDescending(l => l.LoanDate);
        }

        public async Task<IQueryable<LoanDetailTemp>> GetDetailTempAsync(string userName)
        {
            //associar ao user
            var user = await _userHelper.GetUserByEmailAsync(userName);

            if (user == null)
            {
                return null;
            }

            //se n for nulo -> vai buscar os dados temporários -> incluir os Books q leva
            return _context.LoanDetailTemps
                .Include(b => b.Book)
                .Where(u => u.User == user)
                .OrderBy(o => o.Book.Title);

        }

        public async Task AddItemToLoanAsync(AddItemViewModel model, string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return;
            }

            //verificar se o book escolhido está na tabela books -> se sim -> trás o book c esse id
            var book = await _context.Books.FindAsync(model.BookId);
            if (book == null)
            {
                return;
            }

            //tendo user e book -> crio um objecto LoanDetailTemp
            //verifico se já há alg criado no _context
            var loanDetailTemp = await _context.LoanDetailTemps
                .Where(l => l.User == user && l.Book == book)
                .FirstOrDefaultAsync();

            // se o loanDetailTemp for nulo -> crio um novo
            if (loanDetailTemp == null)
            {
                loanDetailTemp = new LoanDetailTemp
                {
                    User = user,
                    Book = book,
                };

                _context.LoanDetailTemps.Add(loanDetailTemp); //****Adiciono aqui o objecto ao context****
            }

            else
            {
                if (loanDetailTemp.Book == model.Books) //DUVIDA: está correcto?se já existir um livro na lista igual ao q foi adicionado -> apaga
                {
                    _context.LoanDetailTemps.Remove(loanDetailTemp);
                }
            }

            await _context.SaveChangesAsync(); //grava na BD
        }

        public async Task DeleteDetailTempAsync(int id)
        {
            var loanDetailTemp = await _context.LoanDetailTemps.FindAsync(id);

            if (loanDetailTemp == null)
            {
                return;
            }

            //se n for nulo -> vai ao _contex e remove o objecto loanDetailTemp
            _context.LoanDetailTemps.Remove(loanDetailTemp);

            await _context.SaveChangesAsync(); //actualizar a BD
        }

        //Neste método -> passo a tabela temporária para a tabela detail e no fim crio o loan
        public async Task<bool> ConfirmLoanAsync(string userName)
        {
            //verificar o user
            var user = await _userHelper.GetUserByEmailAsync(userName);

            if (user == null)
            {
                return false;
            }

            //se o user n for nulo -> vai buscar a tabela temporária à BD
            //recebe td os books deste user e converte numa lista
            var loanTemp = await _context.LoanDetailTemps
                .Include(b => b.Book)
                .Where(u => u.User == user)
                .ToListAsync();

            //verificar se a lista temporária é nula ou está vazia
            if (loanTemp == null || loanTemp.Count == 0)
            {
                return false;
            }

            //transferir esta lista para outra tabela
            //seleccionar um book temporário um a um e converter para LoanDetail -> no final uma lista
            var details = loanTemp.Select(l => new LoanDetail
            {
                Book = l.Book
            }).ToList();

            //criar um objecto LoanOnline
            var loan = new Loan
            {
                LoanDate = DateTime.UtcNow,        //hora do pc
                User = user,
                Items = details
            };

            await CreateAsync(loan);    //Gravar

            //Remover da BD td os items da tabela temporária
            _context.LoanDetailTemps.RemoveRange(loanTemp);
            await _context.SaveChangesAsync(true);
            //actualizar a BD
            await CreateAsync(loan);
            return true;
        }

        //Aceitar o Loan -> novamente pelo employee -> dupla segurança
        public async Task AcceptLoan(FinishBookViewModel model)
        {
            var loan = await _context.Loans.FindAsync(model.Id);
            if (loan == null)
            {
                return;
            }

            loan.ReturnDate = (DateTime)model.LimitLoanDate;
            _context.Loans.Update(loan);
            await _context.SaveChangesAsync();
        }

        public async Task<Loan> GetLoanAsync(int id)
        {
            return await _context.Loans.FindAsync(id);
        }


        //criar o método para apagar o loan e dp ser chamado no controlador
    }
}
