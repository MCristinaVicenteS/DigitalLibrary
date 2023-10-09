using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversalLibrary.Data.Entities;
using UniversalLibrary.Helpers;

namespace UniversalLibrary.Data
{
    public class ReturnBookRepository : GenericRepository<ReturnBook>, IReturnBookRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public ReturnBookRepository(DataContext context, IUserHelper userHelper) : base(context)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public IQueryable GetAllWithUsers()
        {
            return _context.ReturnBooks.Include(p => p.User);
        }


        //public IEnumerable<SelectListItem> GetComboUsers()
        //{
        //    var list = _context.Users.Select(p => new SelectListItem
        //    {
        //        Text = p.Nif.ToString(),
        //        Value = p.Id.ToString()
        //    }).ToList();

        //    list.Insert(0, new SelectListItem
        //    {
        //        Text = "Select a User:",
        //        Value = "0"
        //    });

        //    return list;
        //}


        public IEnumerable<SelectListItem> GetComboBooks()
        {
            var list = _context.LoanDetails.Select(p => new SelectListItem
            {
                Text = p.Book.Title,
                Value = p.Id.ToString(),
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "Select a Book:",
                Value = "0"
            });

            return list;
        }

        

        public IEnumerable<SelectListItem> GetComboLoans()
        {
            var list = _context.Loans.Select(p => new SelectListItem
            {
                Text = p.LoanDate.ToString(),
                Value = p.Id.ToString(),
            }) .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "Select a Loan Date:",
                Value = "0"

            });

            return list;
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
    }
}
