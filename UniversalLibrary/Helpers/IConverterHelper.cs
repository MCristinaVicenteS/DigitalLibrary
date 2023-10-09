using UniversalLibrary.Data.Entities;
using UniversalLibrary.Models;

namespace UniversalLibrary.Helpers
{
    public interface IConverterHelper
    {
        //------BOOK-------

        //converter o viewModelBook em Book e vice versa
        //o bool serve para qd editar -> para saber se o id é inserido aqui ou se vem pela tabela
        //no path -> passa o caminho para a imagem
        Book ToBook(BookViewModel model, string path, bool isNew);


        //converter o Book em BookViewModel
        BookViewModel ToBookViewModel(Book book);


        //------AUTHOR-------

        //converter o AuthorViewModel em Author
        Author ToAuthor(AuthorViewModel model, string path, bool isNew);


        //converter o Author em AuthorViewModel
        AuthorViewModel ToAuthorViewModel(Author author);


        //-----BOOKPUBLISHER------

        //converter o BookPublisherViewModel em BookPublisher
        BookPublisher ToBookPublisher(BookPublisherViewModel model, string path, bool isNew);

        //converter o BookPublisher em BookPublisherViewModel
        BookPublisherViewModel ToBookPublisherViewModel(BookPublisher bookPublisher);


        //-----EMPLOYEE-----

        //converter o EmployeeViewModel em Employee
        Employee ToEmployee(EmployeeViewModel model, string path, bool isNew);

        //converter o Employee em EmployeeViewModel
        EmployeeViewModel ToEmployeeViewModel(Employee employee);


        //-----BOOKONLINE-----

        //converter o BookOnlineViewModel em BookOnline
        BookOnline ToBookOnline(BookOnlineViewModel model, string path, string pathFile, bool isNew);

        //converter o BookOnline em BookOnlineViewModel
        BookOnlineViewModel ToBookOnlineViewModel(BookOnline bookOnline);


        //------RETURN BOOK-----

        //Converter o ReturnBookViewModel em ReturnBook
        ReturnBook ToReturnBook(ReturnBookViewModel model, string path, bool isNew);

        //Converter o ReturnBook em ReturnBookViweModel
        ReturnBookViewModel ToReturnBookViewModel(ReturnBook returnBook);


        //-----READER-----

        //Converter o ReaderViewModel em Reader
        Reader ToReader(ReaderViewModel model, string path, bool isNew);

        //Converter o Reader em ReaderViewModel
        ReaderViewModel ToReaderViewModel(Reader reader);

    }
}
