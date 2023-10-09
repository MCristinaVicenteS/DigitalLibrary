using Org.BouncyCastle.Asn1.Misc;
using System;
using System.IO;
using UniversalLibrary.Data.Entities;
using UniversalLibrary.Models;

namespace UniversalLibrary.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public Author ToAuthor(AuthorViewModel model, string path, bool isNew)
        {
            return new Author
            {
                Id = isNew ? 0 : model.Id,
                User = model.User,
                ImageAuthor = path,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Nif = model.Nif,
                DateOfBirth = model.DateOfBirth,
                Nationality = model.Nationality,
                Biography = model.Biography,
                MajorWorks = model.MajorWorks,
                AwardsAndRecognitions = model.AwardsAndRecognitions,
                OtherDetails = model.OtherDetails
            };
        }

        public AuthorViewModel ToAuthorViewModel(Author author)
        {
            return new AuthorViewModel
            {
                Id = author.Id,
                User = author.User,
                ImageAuthor = author.ImageAuthor,
                FirstName = author.FirstName,
                LastName = author.LastName,
                Nif = author.Nif,
                DateOfBirth = author.DateOfBirth,
                Nationality = author.Nationality,
                Biography = author.Biography,
                MajorWorks = author.MajorWorks,
                AwardsAndRecognitions = author.AwardsAndRecognitions,
                OtherDetails = author.OtherDetails
            };
        }

        public Book ToBook(BookViewModel model, string path, bool isNew)
        {
            return new Book
            {
                Id = isNew ? 0 : model.Id, //Se for true-> é um livro novo -> o valor fica zero e dp cola o id da BD
                                           //Se for false -> o valor do id vem através do edit -> usa o Id q recebe
                //User = model.User,
                UserId = model.UserId,
                Title = model.Title,
                Isbn = model.Isbn,
                Image = path,
                Author = model.Author,
                AuthorId = model.AuthorId,
                Category = model.Category,
                CategoryId = model.CategoryId,
                IsAvailable = model.IsAvailable,
                Stock = model.Stock,
                BookPublisher = model.BookPublisher,
                BookPublisherId = model.BookPublisherId,
                PhisicalLibrary = model.PhisicalLibrary,
                PhisicalId = model.PhisicalId,
                Year = model.Year,
                PageNumber = model.PageNumber
            };
        }

        public BookViewModel ToBookViewModel(Book book)
        {
            return new BookViewModel
            {
                Id = book.Id,
                Title = book.Title,
                Isbn = book.Isbn,
                Image = book.Image,
                Author = book.Author,
                AuthorId = book.AuthorId,
                Category = book.Category,
                CategoryId = book.CategoryId,
                IsAvailable = book.IsAvailable,
                Stock = book.Stock,
                BookPublisher = book.BookPublisher,
                BookPublisherId = book.BookPublisherId,
                PhisicalLibrary = book.PhisicalLibrary,
                PhisicalId = book.PhisicalId,
                Year = book.Year,
                PageNumber = book.PageNumber
            };
        }

        public BookPublisher ToBookPublisher(BookPublisherViewModel model, string path, bool isNew)
        {
            return new BookPublisher
            {
                Id = isNew ? 0 : model.Id,
                User = model.User,
                PublisherName = model.PublisherName,
                ImagLog = path
            };
        }

        public BookPublisherViewModel ToBookPublisherViewModel(BookPublisher bookPublisher)
        {
            return new BookPublisherViewModel
            {
                Id = bookPublisher.Id,
                User = bookPublisher.User,
                PublisherName = bookPublisher.PublisherName,
                ImagLog = bookPublisher.ImagLog
            };
        }

        public Employee ToEmployee(EmployeeViewModel model, string path, bool isNew)
        {
            return new Employee
            {
                Id = isNew ? 0 : model.Id,
                User = model.User,
                FirstName = model.FirstName,
                LastName = model.LastName,
                ImageEmployee = path,
                Nif = model.Nif,
                Since = model.Since,
                Contract = model.Contract,
                TypeOfEmployee = model.TypeOfEmployee
            };
        }
        
        public EmployeeViewModel ToEmployeeViewModel(Employee employee)
        {
            return new EmployeeViewModel
            { 
                Id = employee.Id,
                User = employee.User,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                ImageEmployee = employee.ImageEmployee,
                Nif = employee.Nif,
                Since = employee.Since,
                Contract = employee.Contract,
                TypeOfEmployee = employee.TypeOfEmployee
            };
        }

        public BookOnline ToBookOnline(BookOnlineViewModel model, string path, string pathFile, bool isNew)
        {
            return new BookOnline
            {
                Id = isNew ? 0 : model.Id,
                UserId = model.UserId,
                Title = model.Title,
                Isbn = model.Isbn,
                Image = path,
                Pdf = pathFile,
                Author = model.Author,
                AuthorId = model.AuthorId,
                Category = model.Category,
                CategoryId = model.CategoryId,
                BookPublisher = model.BookPublisher,
                BookPublisherId = model.BookPublisherId,
                Year = model.Year,
                PageNumber = model.PageNumber,
            };
        }

        public BookOnlineViewModel ToBookOnlineViewModel(BookOnline bookOnline)
        {
            return new BookOnlineViewModel
            {
                Id = bookOnline.Id,
                UserId = bookOnline.UserId,
                Title = bookOnline.Title,
                Isbn = bookOnline.Isbn,
                Image = bookOnline.Image,
                Pdf = bookOnline.Pdf,
                Author = bookOnline.Author,
                AuthorId = bookOnline.AuthorId,
                Category = bookOnline.Category,
                CategoryId = bookOnline.CategoryId,
                BookPublisher = bookOnline.BookPublisher,
                BookPublisherId = bookOnline.BookPublisherId,
                Year = bookOnline.Year,
                PageNumber = bookOnline.PageNumber
            };
        }

        public ReturnBook ToReturnBook(ReturnBookViewModel model, string path, bool isNew)
        {
            return new ReturnBook
            {
                Id = isNew ? 0 : model.Id,


            };          

        }

        public ReturnBookViewModel ToReturnBookViewModel(ReturnBook returnBook)
        {
            throw new NotImplementedException();
        }

        public Reader ToReader(ReaderViewModel model, string path, bool isNew)
        {
            return new Reader
            {
                Id = isNew ? 0 : model.Id,
                UserId = model.UserId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Nif = model.Nif,
                Address = model.Address,
                PhoneNumber = model.PhoneNumber,
                Image = path
            };
        }

        public ReaderViewModel ToReaderViewModel(Reader reader)
        {
            return new ReaderViewModel
            {
                Id = reader.Id,
                UserId = reader.UserId,
                FirstName = reader.FirstName,
                LastName = reader.LastName,
                Nif = reader.Nif,
                Address = reader.Address,
                PhoneNumber = reader.PhoneNumber,
                Image = reader.Image
            };
        }
    }
}
