using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversalLibrary.Data.Entities;
using UniversalLibrary.Helpers;

namespace UniversalLibrary.Data
{
    //objectivo: se n existir BD -> qd corro o programa é criada uma -> tem de ser aplicado no Program e no startup
    public class SeedDb
    {
        #region Propriedades
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private Random _random; //gera os livros aleatoriamente
        
        #endregion

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _random = new Random();
        }


        //método q vai criar o seed de forma assincrona
        public async Task SeedAsync()
        {
            //vai criar a BD -> se a BD j estiver criada -> continua
            bool dbWasCreated = await _context.Database.EnsureCreatedAsync();
            if(dbWasCreated)
            {
                return;
            }

            
            //MigrateAsync

            //verificar se os roles existem
            await _userHelper.CheckRoleAsync("Admin");
            await _userHelper.CheckRoleAsync("Reader");
            await _userHelper.CheckRoleAsync("Employee");
            await _userHelper.CheckRoleAsync("Anonymous");
            
            //verificar se o user já existe -> o que a aplicação vai criar -> vai ser o Admin
            var user = await _userHelper.GetUserByEmailAsync("crisjoelsimao@gmail.com");

            //se for nulo -> cria com estes dados
            if (user == null)
            {
                user = new User
                {
                    //NOTA - NÃO COLOQUEI COUNTRY NEM CITY PQ AINDA NÃO OS CRIÁMOS -> PARA IREM PARA UMA COMBOBOX
                    FirstName = "CristinaJoelSimao",
                    LastName = "VicenteRangelCazola",
                    Nif = 123456789,
                    Email = "crisjoelsimao@gmail.com",
                    UserName = "crisjoelsimao@gmail.com",
                    PhoneNumber = "210001122",
                    Address = "Rua Cinel",                    
                };

                //usa a classe userManager para criar o user por defeito -> recebe 2 parâmetros (user e pass)
                var result = await _userHelper.AddUserAsync(user, "123456");

                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not creat user in seeder");
                }

                //adicionar o role q já existe (criado no userHelper) ao role q vou passar
                //vou passar o admin para ficar associado ao user por default -> fica o admin
                await _userHelper.AddUserToRoleAsync(user, "Admin");

                //Associar ao email o token
                //gerar o email de confirmação q vai receber o token
                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);

                //confirmar o token
                await _userHelper.ConfirmEmailAsync(user, token);
            }

            //confirmar se o user está no role q foi escolhido
            var isInRole = await _userHelper.IsUserInRoleAsync(user, "Admin");

            //se o user criado n tiver o role escolhido -> cria a associação
            if (!isInRole)
            {
                await _userHelper.AddUserToRoleAsync(user, "Admin");
            }

            //Inserir categorias na tabela BD
            //se n existirem categorias -> usa o método para os adicioanar -> adiciona smp estes
            //associado a cada categoria -> fica o user que a criou -> neste caso o user admin
            if(!_context.Categories.Any())
            {
                AddCategories("Fantasia", user);
                AddCategories("Infantil", user);

                await _context.SaveChangesAsync();
            }

            if(!_context.BookPublishers.Any())
            {
                AddBookPublisher("Kalandraka", user);
                AddBookPublisher("Editorial Presença", user);

                await _context.SaveChangesAsync();
            }

            if (!_context.authors.Any())
            {
                AddAuthors("J.", "K. Rolling", "English", user, 123456789,"England" ); // DUVIDA: Atualizar para uma combobox para os paises
                AddAuthors("Georges", "Remi", "Belgian", user, 123456765, "Belgium");
                AddAuthors("Jimmy", "Liao", "Tailan", user, 123456754, "Thai");

                await _context.SaveChangesAsync();
            }

            //inserir books na tabela da BD
            //se n existirem books -> usa o método para os adicioanar -> adiciona smp estes
            //associado a cada book -> fica o user que o criou -> neste caso o user admin
            //if (!_context.Books.Any())
            //{
            //    AddBooks("Harry Potter e a pedra filosofal", "wwwroot/images/books/hp_pedraFilosofal.jpg", "J. K. Rolling", "Fantasy", "Editorial Preseça", 1997, "254", user);
            //    AddBooks("Tintim no Congo", user);

            //    await _context.SaveChangesAsync(); //adiciona à base de dados
            //}


        }

        public void AddAuthors(string firstName, string lastName, string nationality, User user, int nif, string country)
        {
            _context.authors.Add(new Author
            {
                FirstName = firstName,
                LastName = lastName,
                Nationality = nationality,
                User = user,
                Nif= nif,
                Country= country
            });
        }

        public void AddCategories(string categoryName, User user)
        {
            _context.Categories.Add(new Category
            {
                CategoryName = categoryName,
                User = user
            });
        }

        public void AddBookPublisher(string publisherName, User user)
        {
            _context.BookPublishers.Add(new BookPublisher
            {
                PublisherName = publisherName,
                User = user
            });
        }

        public void AddBooks(string name, string imageLabel, Author author, Category category, BookPublisher bookPublisher, DateTime year, int pageNumber,
            User user)
        {
            _context.Books.Add(new Book
            {
                Title = name,
                Image = imageLabel,
                Author = author,
                Category = category,
                IsAvailable = true,
                Stock = _random.Next(5),
                BookPublisher = bookPublisher,
                Year = year,
                PageNumber = pageNumber,
                User = user
            });
        }


    }
}
