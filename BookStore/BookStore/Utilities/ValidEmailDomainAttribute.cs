using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Utilities
{
    public class ValidEmailDomainAttribute : ValidationAttribute
    {
        private string allowedDomain;

        public ValidEmailDomainAttribute(string allowedDomain)
        {
            this.allowedDomain = allowedDomain;
        }

        public override bool IsValid(object value)
        {
            var email = value.ToString();
            string domain = email.Split("@")[1];

            return domain.ToUpper() == allowedDomain.ToUpper();
        }
    }
}
