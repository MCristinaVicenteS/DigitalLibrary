using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniversalLibrary.Models
{
    public class AddItemViewModel
    {
        //****os books vão aparecer numa combobox -> para isso preciso do Id do Book****
        [Display(Name = "Book")]
        [Range(1, int.MaxValue, ErrorMessage = "You have to select a book.")]  //****Range 1 para dar erro -> caso n seleccione um livro****
        public int BookId { get; set; }

        //****uma lista com uma lista de livros -> cria a combobox****
        //****a selectListItem -> é uma classe q dá os itens e os envia para uma lista Html
        public IEnumerable<SelectListItem> Books { get; set; }
        


    }
}
