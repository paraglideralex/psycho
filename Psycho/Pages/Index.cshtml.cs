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
        //Questions = new List<QuestionModel>
        //{
        //    new QuestionModel { QuestionText = "Авантюризм, склонность к риску", AnswerName = "QuestionAnswers[0]" },
        //    new QuestionModel { QuestionText = "Настороженность, отстраненность", AnswerName = "QuestionAnswers[1]" },
        //    new QuestionModel { QuestionText = "Адаптивность, конформизм", AnswerName = "QuestionAnswers[2]" },
        //    new QuestionModel { QuestionText = "Неугомонность, нетерпеливость", AnswerName = "QuestionAnswers[3]" }
        //    // Добавьте остальные вопросы в список
        //};
        Questions = new List<QuestionModel>
        {

            // Добавьте остальные вопросы в список
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
