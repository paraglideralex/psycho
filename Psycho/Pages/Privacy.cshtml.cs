using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System.Text;

namespace Psycho.Pages;
public class PrivacyModel : PageModel
{
    private readonly ILogger<PrivacyModel> _logger;

    public PrivacyModel(ILogger<PrivacyModel> logger) { _logger = logger; }

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

