using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace FixedAssets.IdentityExtensions
{
    public class CustomPasswordValidator : PasswordValidator, IIdentityValidator<string>
    {
        override public Task<IdentityResult> ValidateAsync(string item)
        {
            if (String.IsNullOrEmpty(item) || item.Length < RequiredLength)
            {
                return Task.FromResult(IdentityResult.Failed(String.Format("Hasło musi mieć przynajmniej {0} znaków.", RequiredLength)));
            }

            string pattern = @"^(?=.*[0-9])(?=.*[!@#$%^&*])[0-9a-zA-Z!@#$%^&*0-9]{6,}$";

            if (!Regex.IsMatch(item, pattern))
            {
                return Task.FromResult(IdentityResult.Failed("Hasło powinno zawierać przynajmniej 1 znak specjalny, cyfrę ('0'-'9') oraz wielką literę ('A'-'Z')"));
            }

            return Task.FromResult(IdentityResult.Success);
        }
    }
}