using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System.Text;

namespace Psycho.Pages
{
    public class FileNameModel : PageModel
    {
        public FileNameModel() { }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            // получаем данные из TempData
            string fil = PageState.Result;
            byte[] fileData = Encoding.UTF8.GetBytes(fil);// TempData["file"] as byte[];

            string fileName = "results.csv";

            return File(fileData, "text/csv", fileName);
        }

    }
}
