using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Forum.App_Start.Identity
{
    public class SenhaValidador : IIdentityValidator<string>
    {
        public int TamanhoRequerido { get; set; }
        public bool ObrigatorioCaracteresEspecias { get; set; }
        public bool ObrigatorioLowerCase { get; set; }
        public bool ObrigatorioUpperCase { get; set; }
        public bool ObrigatorioDigitos { get; set; }

        public async Task<IdentityResult> ValidateAsync(string senha)
        {
            var erros = new List<string>();

            if (ObrigatorioCaracteresEspecias && !VerificaCaracteresEspeciais(senha))
                erros.Add("A senha deve conter no mínimo um caracter especial.");

            if (!VerificaTamanhoRequerido(senha))
                erros.Add($"A senha deve conter no mínimo {TamanhoRequerido} caracteres.");

            if (ObrigatorioLowerCase && !VerificaLowerCase(senha))
                erros.Add("A senha deve conter no mínimo uma letra minúscula.");

            if (ObrigatorioUpperCase && !VerificaUpperCase(senha))
                erros.Add("A senha deve conter no mínimo uma letra maiúscula.");

            if (ObrigatorioDigitos && !VerificaDigitos(senha))
                erros.Add("A senha deve conter no mínimo um número");

            if (erros.Any())
                return IdentityResult.Failed(erros.ToArray());

            return IdentityResult.Success;
        }

        private bool VerificaTamanhoRequerido(string senha) => 
            senha?.Length >= TamanhoRequerido;

        private bool VerificaCaracteresEspeciais(string senha) =>
            Regex.IsMatch(senha, @"[^a-zA-Z0-9]");

        private bool VerificaLowerCase(string senha) =>
            senha.Any(char.IsLower);

        private bool VerificaUpperCase(string senha) =>
            senha.Any(char.IsUpper);

        private bool VerificaDigitos(string senha) =>
            senha.Any(char.IsDigit);
    }
}