using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalLibrary.Data;
using UniversalLibrary.Data.Entities;
using UniversalLibrary.Helpers;

namespace UniversalLibrary
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //configurar o user -> usa a minha identidade User e o IdentityRole
            //configurar a pass -> neste caso sem protecção -> QUEREM MUDAR??
            //inserir o token na autenticação da web
            services.AddIdentity<User, IdentityRole>(cfg =>
            {
                cfg.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider; // Gerador de tokens
                cfg.SignIn.RequireConfirmedEmail = true; //qd fizer o registo -> só pode fazer o login dp de ir ao mail confirmar 

                cfg.User.RequireUniqueEmail = true;
                cfg.Password.RequireDigit = false;
                cfg.Password.RequiredUniqueChars = 0;
                cfg.Password.RequireUppercase = false;
                cfg.Password.RequireLowercase = false;
                cfg.Password.RequireNonAlphanumeric = false;
                cfg.Password.RequiredLength = 6;
            })
                .AddDefaultTokenProviders()  //começar a gerar os tokens
                .AddEntityFrameworkStores<DataContext>(); //dp do serviço estar implementado -> dp do login -> volta
                                                          //a usar o datacontext simples


            //gerar o token apartir do middleware -> passo as informações do token q estão no appsettingjson para aqui
            services.AddAuthentication()
                .AddCookie()
                .AddJwtBearer(cfg =>
                {
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = this.Configuration["Tokens:Issuer"],
                        ValidAudience = this.Configuration["Tokens:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(this.Configuration["Tokens:Key"]))
                    };
                });


            //configurar o datacontext -> chamar o serviço -> usar o sql com essa connectionstring
            services.AddDbContext<DataContext>(cfg =>
            {
                //vou buscar a configuração -> ou seja o appsettingjson -> DfaultConnection
                cfg.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection"));
            });

            //ADDTRANSIENT -> usa e deita fora -> deixa de ficar em memória
            services.AddTransient<SeedDb>(); //criar a BD se n existir

            services.AddControllersWithViews();


            //ADDSCOPED -> qq serviço/objecto fica criado e instanciado -> qd crio outro do mm tipo -> apaga o anterior e fica c o novo
            services.AddScoped<IUserHelper, UserHelper>();

            services.AddScoped<IImageHelper, ImageHelper>();

            services.AddScoped<IFileHelper, FileHelper>();

            services.AddScoped<IConverterHelper, ConverterHelper>();

            services.AddScoped<IMailHelper, MailHelper>();

            services.AddScoped<ICategoryRepository, CategoryRepository>();

            services.AddScoped<IAuthorRepository, AuthorRepository>();

            //Assim q detectar q é preciso um repositorio -> cria um
            services.AddScoped<IBookRepository, BookRepository>();

            services.AddScoped<IBookOnlineRepository, BookOnlineRepository>();

            services.AddScoped<IBookPublisherRepository, BookPublisherRepository>();

            services.AddScoped<ILoanOnlineRepository, LoanOnlineRepository>();

            services.AddScoped<ILoanRepository, LoanRepository>();

            services.AddScoped<IReturnBookRepository, ReturnBookRepository>();

            services.AddScoped<IAuthorRepository, AuthorRepository>();

            services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            services.AddScoped<IBookPublisherRepository, BookPublisherRepository>();

            services.AddScoped<IBookingRepository, BookingRepository>();

            services.AddScoped<ILibraryFeedbackRepository, LibraryFeedbackRepository>();

            services.AddScoped<IReaderRepository, ReaderRepository>();

            //alterar a página q aparece qd n pode ter acesso
            services.ConfigureApplicationCookie(options =>
            {                
                options.AccessDeniedPath = "/Account/NotAuthorized"; //chama esta action view
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Errors/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/Error/{0}"); //Para páginas q n existem
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication(); //configurar o serviço de autenticação
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                //endpoints.MapControllerRoute(
                //name: "areas",
                //pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            });
        }
    }
}
