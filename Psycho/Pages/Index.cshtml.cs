using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Psycho.Pages;
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public List<QuestionModel> Questions { get; set; }

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        Questions = new List<QuestionModel>
        {
        };
    }

    [BindProperty]
    public List<string> Fields { get; set; }
    public IActionResult OnPost(List<string> values)
    {
        // Передаем данные из списка значений в список строк
        Fields.AddRange(values);

        // Укажите путь к файлу, куда нужно сохранить данные
        string filePath = "pathto.txt";

        // Записываем содержимое в файл
        System.IO.File.WriteAllLines(filePath, Fields);

        return RedirectToPage();
    }

}
