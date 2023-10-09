using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversalLibrary.Data.Entities;

namespace UniversalLibrary.Data
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;

        public CategoryRepository(DataContext context)
        {
            _context = context;
        }

        // Cria uma nova categoria na base de dados de forma assíncrona
        public async Task CreateAsync(Category entity)
        {
            _context.Categories.Add(entity);
            await _context.SaveChangesAsync();
        }

        // Remove uma categoria da base de dados de forma assíncrona
        public async Task DeleteAsync(Category entity)
        {
            _context.Categories.Remove(entity);
            await _context.SaveChangesAsync();
        }

        // Verifica se uma categoria com o ID fornecido existe no banco de dados de forma assíncrona
        public async Task<bool> ExistAsync(int id)
        {
            return await _context.Categories.AnyAsync(c => c.Id == id);
        }

        // Procura e retorna uma categoria com base no nome da categoria
        public Category FindByName(string categoryName)
        {
            return _context.Categories.FirstOrDefault(c => c.CategoryName == categoryName);
        }

        // Retorna todas as categorias da base de dados de forma assíncrona
        public IQueryable<Category> GetAll()
        {
            return _context.Categories;
        }

        public IQueryable GetAllWithUsers()
        {
            return _context.Categories.Include(p => p.User);
        }

        // Retorna uma categoria com base no ID.
        public async Task<Category> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public IEnumerable<SelectListItem> GetComboCategories()
        {
            var list = _context.Categories.Select(p => new SelectListItem
            {
                Text = p.CategoryName,
                Value = p.Id.ToString(),
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "Select a Category:",
                Value = "0"
            });

            return list;
        }

        // Atualiza uma categoria existente na base de dados de forma assíncrona
        public async Task UpdateAsync(Category entity)
        {
            _context.Categories.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
