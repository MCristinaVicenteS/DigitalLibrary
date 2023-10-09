using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversalLibrary.Data.Entities;
using UniversalLibrary.Helpers;
using UniversalLibrary.Models;

namespace UniversalLibrary.Data
{
    public class LoanOnlineRepository : GenericRepository<LoanOnline>, ILoanOnlineRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public LoanOnlineRepository(DataContext contex, IUserHelper userHelper) : base(contex)
        {
            _context = contex;
            _userHelper = userHelper;
        }

        public IQueryable GetAllWithUsers()
        {
            return _context.LoanOnlines.Include(p => p.User);
        }

        public async Task<List<LoanOnline>> GetLoanOnlineAsync(string userName)            /*Task<List<LoanOnline>> GetLoanOnlineAsync(string userName)*/
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);

            if (user == null)
            {
                return null;
            }

            //se existir user -> ver qual é o role para definir os acessos
            //role admin tem acesso a todos os Loans online
            if (await _userHelper.IsUserInRoleAsync(user, "Admin"))
            {
                //se for true -> vou buscar td as encomendas à BD (tabela LoansOnline)
                //incluir od itens e os books q estão na tabela

                //return await _context.LoanOnlines
                //    .Include(u => u.User)
                //    .Include(l => l.Items)                  //tem ligação directa
                //    .ThenInclude(i => i.BookOnline)               //n tem ligação directa
                //    .OrderByDescending(o => o.LoanDate)
                //    .ToListAsync();

                //qd associar roles -> inserir else if -> para o role reader
                return await _context.LoanOnlines
                    .Include(i => i.Items)
                    .ThenInclude(i => i.BookOnline)
                    .OrderByDescending(o => o.LoanDate).ToListAsync();
            }

            //o Reader acede só aos seus loans online
            // NOTA: O Employee não tem acesso ao Loans online
            //else if (await _userHelper.IsUserInRoleAsync(user, "Reader"))
            //{
            //    return await _context.LoanOnlines
            //        .Include(l => l.User)
            //        .Include(l => l.Items)
            //        .ThenInclude(i => i.BookOnline)
            //        .Where(o => o.User == user)
            //        .OrderByDescending(o => o.LoanDate)
            //        .ToListAsync();
            //}

            //return null;

            return await _context.LoanOnlines
                .Include(i => i.Items)
                .ThenInclude(b => b.BookOnline)
                .Where(u => u.User == user)
                .OrderByDescending(l => l.LoanDate).ToListAsync();
        }

        public async Task<List<LoanOnlineDetailTemp>> GetDetailTempAsync(string userName)
        {
            //associar ao user
            var user = await _userHelper.GetUserByEmailAsync(userName);

            if(user == null)
            {
                return null;
            }

            //se n for nulo -> vai buscar os dados temporários -> incluir os Books q leva
            return await _context.LoanOnlineDetailTemps
                .Include(b => b.BookOnline)
                .Where(u => u.User == user)
                .OrderBy(o => o.BookOnline.Title).ToListAsync();

        }

        public async Task AddItemToLoanAsync(AddItemViewModel model, string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if(user == null)
            {
                return;
            }

            //verificar se o book escolhido está na tabela books -> se sim -> trás o book c esse id
            var bookOnline = await _context.BookOnlines.FindAsync(model.BookId);
            if(bookOnline == null)
            {
                return;
            }

            //tendo user e book -> crio um objecto LoanOnlineDetailTemp
            //verifico se já há alg criado no _context
            var loanOnlineDetailTemp = await _context.LoanOnlineDetailTemps
                .Where(l => l.User == user && l.BookOnline == bookOnline)
                .FirstOrDefaultAsync();

            // se o loanOnlineDetailTemp for nulo -> crio um novo
            if(loanOnlineDetailTemp == null)
            {
                loanOnlineDetailTemp = new LoanOnlineDetailTemp
                {
                    User = user,
                    BookOnline = bookOnline,
                };

                _context.LoanOnlineDetailTemps.Add(loanOnlineDetailTemp); //****Adiciono aqui o objecto ao context****
            }

            else
            {
                if(loanOnlineDetailTemp.BookOnline == model.Books) //DUVIDA: está correcto?se já existir um livro na lista igual ao q foi adicionado -> apaga
                {
                    _context.LoanOnlineDetailTemps.Remove(loanOnlineDetailTemp);
                }
            }

            await _context.SaveChangesAsync(); //grava na BD
        }

        public async Task DeleteDetailTempAsync(int id)
        {
            var loanOnlineDetailTemp = await _context.LoanOnlineDetailTemps.FindAsync(id);

            if (loanOnlineDetailTemp == null)
            {
                return;
            }

            //se n for nulo -> vai ao _contex e remove o objecto loanOnlineDetailTemp
            _context.LoanOnlineDetailTemps.Remove(loanOnlineDetailTemp);

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
            var loanTemp = await _context.LoanOnlineDetailTemps
                .Include(b => b.BookOnline)
                .Where(u => u.User == user)
                .ToListAsync();

            //verificar se a lista temporária é nula ou está vazia
            if(loanTemp == null || loanTemp.Count == 0)
            {
                return false;
            }

            //transferir esta lista para outra tabela
            //seleccionar um book temporário um a um e converter para LoanOnlineDetail -> no final uma lista
            var details = loanTemp.Select(l => new LoanOnlineDetail
            {
                BookOnline = l.BookOnline
            }).ToList();

            //criar um objecto LoanOnline
            var loanOnline = new LoanOnline
            {
                LoanDate = DateTime.UtcNow,        //hora do pc
                User = user,
                Items = details
            };

            //Remover da BD td os items da tabela temporária
            _context.LoanOnlineDetailTemps.RemoveRange(loanTemp);
            await _context.SaveChangesAsync(true);
            //actualizar a BD
            await CreateAsync(loanOnline);
            return true;
        }


        //Aceitar o Loan online -> só o admin
        public async Task AcceptLoanOnline(FinishBookViewModel model)
        {
            var loan = await _context.LoanOnlines.FindAsync(model.Id);
            if(loan == null)
            {
                return;
            }

            loan.ReturnDate = (DateTime)model.LimitLoanDate;
            _context.LoanOnlines.Update(loan);
            await _context.SaveChangesAsync();
        }

        

        public async Task AddItemToCartLoanAsync(BookOnline bookOnline, string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return;
            }

            //ver se o book escolhido está na BD -> se sim -> trás o book c esse id
            var bookBd = await _context.BookOnlines.FindAsync(bookOnline.Id);

            if (bookBd == null)
            {
                return;
            }

            //tendo user e boook -> crio o objecto LoanOnlineDetailTemp
            //1º verifico se já há alg criado no _context
            var loanOnlineDetailTemp = await _context.LoanOnlineDetailTemps
                .Where(l => l.User.Id == user.Id && l.BookOnline.Id == bookBd.Id)
                .FirstOrDefaultAsync();

             //se o loanOnlineDetailTemp for nulo -> crio um
            if (loanOnlineDetailTemp == null)
            {
                loanOnlineDetailTemp = new LoanOnlineDetailTemp
                {
                    User = user,
                    BookOnline = bookBd,
                };

                _context.LoanOnlineDetailTemps.Add(loanOnlineDetailTemp);
            }

            else
            {
                //Se o livro já tiver sido escolhido -> apago
                if (loanOnlineDetailTemp.BookOnline == bookOnline)
                {
                    _context.LoanOnlineDetailTemps.Remove(loanOnlineDetailTemp);
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<LoanOnline> GetLoanOnlineAsync(int id)
        {
            return await _context.LoanOnlines.FindAsync(id);
        }



        //criar o método para apagar o loan e dp ser chamado no controlador
    }
}
