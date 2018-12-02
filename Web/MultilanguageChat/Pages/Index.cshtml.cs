using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MultilanguageChat.Pages
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            var langQueryString = Request.Query["lang"];
            if (string.IsNullOrWhiteSpace(langQueryString))
            {
                return Redirect("/?lang=en");
            }

            return Page();
        }
    }
}
