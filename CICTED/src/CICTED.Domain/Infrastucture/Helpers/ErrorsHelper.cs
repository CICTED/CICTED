using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICTED.Domain.Infrastucture.Helpers
{
    public static class ErrorsHelper
    {
        public static string ConvertToHTML(this IdentityResult errors)
        {
            var html = new StringBuilder("<ul>");

            foreach (var error in errors.Errors)
                html.Append("</li>").Append(error.Description).Append("</li>");

            return html.Append("</ul>").ToString();
        }
    }
}
