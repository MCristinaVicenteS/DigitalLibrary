using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using UniversalLibrary.Data.Entities;
using UniversalLibrary.Models;

namespace UniversalLibrary.Helpers
{
    public interface IUserHelper
    {
        //método para associar um mail ao user -> se já existir
        Task<User> GetUserByEmailAsync(string email);

        //método ara associar o id ao user
        Task<User> GetUserByIdAsync(string id);
                
        //método para criar o user
        Task<IdentityResult> AddUserAsync(User user, string password);

        //método para fazer o login -> vai receber a viewmodel login
        Task<SignInResult> LoginAsync(LoginViewModel model);

        //método para fazer logout -> n recebe nenhum parâmetro
        Task LogoutAsync();

        //método para alterar os dados do user
        Task<IdentityResult> UpdateUserAsync(User user);

        //método para alterar a pass
        Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);

        //método para verificar o role do user
        Task CheckRoleAsync(string roleName);

        //método para adicionar o role ao user
        Task AddUserToRoleAsync(User user, string roleName);

        //método para verificar se a associação existe
        Task<bool> IsUserInRoleAsync(User user, string roleName);

        //método para validar a pass -> recebe o user e a pass -> e vê se estão correctos
        //valida 1º as credenciais e só dp envia o token
        Task<SignInResult> ValidatePasswordAsync(User user, string password);


        //Tasks para confirmar a pass
        Task<string> GenerateEmailConfirmationTokenAsync(User user);

        //task para gerar/enviar o email -> é aqui q se verifica se o token é válido
        Task<IdentityResult> ConfirmEmailAsync(User user, string token);


        //Task para gerar o token para o reset da pass -> recebe um user e gera o token
        Task<string> GeneratePasswordResetTokenAsync(User user);

        //Task para fazer o reset e trocar a pass -> recebe um user, um token e a pass
        Task<IdentityResult> ResetPasswordAsync(User user, string token, string paswword);
    }
}
