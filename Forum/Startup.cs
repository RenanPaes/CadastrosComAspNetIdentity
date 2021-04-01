using Forum.App_Start.Identity;
using Forum.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

[assembly: OwinStartup(typeof(Forum.Startup))]
namespace Forum
{
    public class Startup
    {
        public void Configuration(IAppBuilder builder)
        {
            builder.CreatePerOwinContext<DbContext>(() =>
            {
                var dbContext = new IdentityDbContext<UsuarioAplicacao>("DefaultConnection");
                dbContext.Database.CreateIfNotExists();

                return dbContext;
            });

            builder.CreatePerOwinContext<IUserStore<UsuarioAplicacao>>((options, owinContext) =>
            {
                var dbContext = owinContext.Get<DbContext>();
                return new UserStore<UsuarioAplicacao>(dbContext);
            });

            builder.CreatePerOwinContext<UserManager<UsuarioAplicacao>>((options, owinContext) =>
            {
                var userStore = owinContext.Get<IUserStore<UsuarioAplicacao>>();
                var userManager = new UserManager<UsuarioAplicacao>(userStore);

                var userValidator = new UserValidator<UsuarioAplicacao>(userManager);
                userValidator.RequireUniqueEmail = true;

                userManager.UserValidator = userValidator;
                userManager.PasswordValidator = new SenhaValidador
                {
                    TamanhoRequerido = 6,
                    ObrigatorioCaracteresEspecias = true,
                    ObrigatorioDigitos = true,
                    ObrigatorioLowerCase = true,
                    ObrigatorioUpperCase = true
                };

                userManager.EmailService = new EmailService();
                userManager.UserTokenProvider = 
                    new DataProtectorTokenProvider<UsuarioAplicacao>(options.DataProtectionProvider.Create("ForumRenan"));

                return userManager;
            });
        }
    }
}