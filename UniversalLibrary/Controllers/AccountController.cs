using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UniversalLibrary.Data.Entities;
using UniversalLibrary.Helpers;
using UniversalLibrary.Models;

namespace UniversalLibrary.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly IConfiguration _configuration; //para aceder ao app.JSON od estão as configurações do Token
        private readonly IMailHelper _mailHelper;

        public AccountController(IUserHelper userHelper, IConfiguration configuration, IMailHelper mailHelper)
        {
            _userHelper = userHelper;
            _configuration = configuration;
            _mailHelper = mailHelper;
        }

        public ActionResult Login()
        {
            if(User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(); //se n estiver autenticado -> fica na própria view
        }

        //para receber os dados q são inseridos no LoginViewModel
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userHelper.LoginAsync(model);
                if (result.Succeeded)
                {                    
                    //Se vier ter ao login por ter tentado entrar numa pag n permitida -> faz o login e dp volta para od quis entrar
                    if(this.Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(this.Request.Query["ReturnUrl"].First());
                    }

                }

                return this.RedirectToAction("Index", "Home");
            }

            this.ModelState.AddModelError(string.Empty, "Failded to login");
            return View(model);
        }


        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }



        public IActionResult Register() //isto é o Get -> mostra a action register q está na view login
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterNewUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Username);

                //ver se o user existe
                if(user == null)
                {
                    user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Nif = model.Nif,
                        Email = model.Username,
                        UserName = model.Username,
                        Address = model.Address,
                        PhoneNumber = model.PhoneNumber,
                    };

                    //se não existir -> adicionar
                    var result = await _userHelper.AddUserAsync(user, model.Password);

                    //se n conseguir criar o user -> aparece uma msg de erro e retorna a view model
                    if(result != IdentityResult.Success)
                    {
                        ModelState.AddModelError(string.Empty, "The user couldn't be created.");
                        return View(model);
                    }

                    //Criar o token passando o user
                    string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                                       
                    
                    //Gerar o link do token
                    //vai usar uma action q vai ter um objecto anónimo -> esta vai passar os dados para o Confirm
                    //vai usar um protocolo para passar os dados de uma action para outra -> via web
                    string tokenLink = Url.Action("ConfirmEmail", "Account", new
                    {
                        userid = user.Id,
                        token = myToken
                    }, protocol: HttpContext.Request.Scheme);


                    //confirmar o token
                    await _userHelper.ConfirmEmailAsync(user, myToken);

                    //**********************Enviar Email*****************************
                    //Gerar a response -> para enviar o mail para o user
                    //é a confirmação -> leva o parâmetro username, o texto q aparece no subject e o body(leva o link do token)
                    Response response = _mailHelper.SendEmail(model.Username, "Email confirmation", $"<h1>Email Confirmation</h1>" +
                        $"To allow the user, " +
                        $"click in this link:</br></br><a href = \"{tokenLink}\">Confirm Email</a>");
                    //**************************************************************

                    //Se conseguir fazer o sign in
                    if (response.IsSuccess)
                   {
                       ViewBag.Message = "The instructions to allow you to login, were sent to your email.";
                       return View(model);
                   }

                   //se não conseguir fazer o login -> envia uma msg de erro
                   ModelState.AddModelError(string.Empty, "The user coundn't be logged");                 

                    
                }
            }
            return View(model);
        }


        /*---------CREATE EMPLOYEE USER-----------------------*/
        public IActionResult RegisterEmployee() //isto é o Get -> mostra a action register q está na view login
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> RegisterEmployee(RegisterNewUserViewModel model)
        {
            await _userHelper.CheckRoleAsync("Employee");
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Username);



                //ver se o user existe
                if (user == null)
                {
                    user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Nif = model.Nif,
                        Email = model.Username,
                        UserName = model.Username,
                        Address = model.Address,
                        PhoneNumber = model.PhoneNumber,
                    };



                    //se não existir -> adicionar
                    var result = await _userHelper.AddUserAsync(user, model.Password);



                    //se n conseguir criar o user -> aparece uma msg de erro e retorna a view model
                    if (result != IdentityResult.Success)
                    {
                        ModelState.AddModelError(string.Empty, "The user couldn't be created.");
                        return View(model);
                    }



                    // Após criar o usuário com sucesso, adicione-o à função de Employee
                    await _userHelper.AddUserToRoleAsync(user, "Employee");



                    //se conseguir criar o user -> fica logo logado ->>DP VOU MUDAR
                    var loginViewModel = new LoginViewModel
                    {
                        Password = model.Password,
                        RememberMe = false,
                        Username = model.Username
                    };



                    //Fazer o sign in
                    var result2 = await _userHelper.LoginAsync(loginViewModel);



                    //Se conseguir fazer o sign in
                    if (result2.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }


                    //se não conseguir fazer o login -> envia uma msg de erro
                    ModelState.AddModelError(string.Empty, "The user coundn't be logged");
                }


                // **************************************************************************************Token e confirm email

                //Associar ao email o token
                //gerar o email de confirmação q vai receber o token
                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);

                //confirmar o token
                await _userHelper.ConfirmEmailAsync(user, token);

                // ************************************************************************************************* 


                //confirmar se o user está no role q foi escolhido
                var isInRole = await _userHelper.IsUserInRoleAsync(user, "Employee");

                //se o user criado n tiver o role escolhido -> cria a associação
                if (!isInRole)
                {
                    await _userHelper.AddUserToRoleAsync(user, "Employee");
                }
            }
            return View(model);
        }
        /*----------------------------------------------------------*/




        public async Task<IActionResult> ChangeUser() //isto é o GET -> vou mostrar os detalhes do user q vão aparecer nesta view
        {
            //encontrar o user
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

            //criar o modelo para aparecerem os dados
            var model = new ChangeUserViewModel();
            if(user != null)
            {
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.Nif = user.Nif;
                model.Address = user.Address;
                model.PhoneNumber = user.PhoneNumber;
            }

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> ChangeUser(ChangeUserViewModel model)
        {
            if(ModelState.IsValid)
            {
                //encontrar o user
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                if(user!=null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Nif = model.Nif;
                    user.Address = model.Address;
                    user.PhoneNumber = model.PhoneNumber;

                    var response = await _userHelper.UpdateUserAsync(user);

                    if(response.Succeeded)
                    {
                        ViewBag.UserMessage = "User Updated.";
                    }

                    else
                    {
                        ModelState.AddModelError(string.Empty, response.Errors.FirstOrDefault().Description);
                    }
                }
            }

            return View(model);
        }


        public IActionResult ChangePassword() //isto é o Get -> ms a caixa de texto aparece vazia para n se ver a pass
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                //verificar se o user existe
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

                //se o user existir -> posso mudar a pass
                if (user != null)
                {
                    var result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return this.RedirectToAction("ChangeUser");
                    }
                    else
                    {
                        this.ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "User not found.");
                }

            }

            return this.View(model);
        }


        //Criar o token para a API
        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
            //vou receber o user e a pass para validar. NOTA: podia receber mais factores
            if(this.ModelState.IsValid) 
            {
                //Se for válido -> verifico se o mail existe. Se existir -> verifico a pass
                //Se a pass estiver correcta -> valido a pass -> crio os claims -> é od começa a ser construido o token

                var user = await _userHelper.GetUserByEmailAsync(model.Username);
                if (user != null)
                {
                    var result = await _userHelper.ValidatePasswordAsync(
                        user,
                        model.Password);

                    if (result.Succeeded)
                    {
                        //qd constroi o token -> constroi uma zona od regista o user e uma zona od vai gerar um Guid q vai fica assoiado ao mail do user
                        //com isso -> consigo saber o percurso q o user fez na app

                        var claims = new[]     //nos claims é od tenho as permissões -> é o mecanismo do middleware para gerar o token em segurança
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        };

                        //criar um algoritmo para gerar a key -> transformo a key em bytes com a codificação Encoding -> encriptação SymmetricSecurityKey
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));

                        //criar uma credencial -> encripto através do algoritmo de encriptação 256bits (512 é + forte)
                        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        //criar o objecto token -> do tipo Jwt(bared Token)
                        //alg parâmetros já foram criados no _configuration
                        var token = new JwtSecurityToken(
                            _configuration["Tokens:Issuer"],
                            _configuration["Tokens:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddDays(30),              //expira em 30 dias
                            signingCredentials: credentials);

                        //o result é o q vai ser enviado para fora -> é o q vou ver no postman
                        //vou enviar um objecto anónimo (c 2 propriedades -> o token já criado e a validade)
                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        };

                        //dp de estar criado -> envio o result
                        //o 1ºparâmetro está vazio pq n quero enviar + nd c este objecto result -> se quiser -> coloco nessa posição
                        return this.Created(string.Empty, results);
                    }
                }
            }

            //se n conseguir criar o token -> envio um badRequest
            return BadRequest();

        }


        //Action para confimar o email
            //-> qd clicar no link do token, chama esta action -> e são enviados os parâmetros userId e Token
            //recebo estes parâmetros na confirmação e dp já posso fazer o login
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if(string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return NotFound();
            }

            //verificar o user através do id
            var user = await _userHelper.GetUserByIdAsync(userId);

            if(user == null)
            {
                return NotFound();
            }

            //se encontrar o user -> confirma-se o mail
            //é aqui q se recebe a resposta do email -> recebe-se um user e um token
            var result = await _userHelper.ConfirmEmailAsync(user, token);


            //verificar se o user e o token correspondem
            if(!result.Succeeded)
            {
                return NotFound();
            }

            return View();
        }


        //action q envia para a view recuperar pass
        public IActionResult RecoverPassword()
        {
            return View();
        }

        //Retorna a view q entrou no paâmetro -> para os campos n ficarem vazios
        [HttpPost]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordViewModel model)
        {
            if(this.ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Email);

                //Ckeck if the mail exist
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "The email doesn't correspond to a registered user.");
                    return View (model);
                }

                var myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);

                var link = this.Url.Action(
                    "ResetPassword",
                    "Account",
                    new { token = myToken }, protocol: HttpContext.Request.Scheme);

                Response response = _mailHelper.SendEmail(model.Email, "Universal Library - Password Reset", $"<h1>Universal Library - Password Reset</h1>" +
                    $"To reset the password, click in this link:</br></br>" +
                    $"<a href = \"{link}\">Reset Password</a>");

                if(response.IsSuccess)
                {
                    this.ViewBag.Message = "The instruction to recover your password has been sent to email.";
                }

                return this.View();
            }

            return this.View(model);
        }


        public IActionResult ResetPassword(string Token)
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await _userHelper.GetUserByEmailAsync(model.UserName);
            if (user != null)
            {
                var result = await _userHelper.ResetPasswordAsync(user, model.Token, model.Password);
                if (result.Succeeded)
                {
                    this.ViewBag.Message = "Password successfully deleted.";
                    return View();
                }

                this.ViewBag.Message = "Error while trying to delete the password.";
                return View(model);
            }

            this.ViewBag.Message = "User not found.";
            return View(model);
        }


        //action para quem n tem autorização de acesso
        public IActionResult NotAuthorized()
        {
            return View();
        }
    }
}
