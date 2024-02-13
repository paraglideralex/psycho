using System.Collections;
using System.Diagnostics.Metrics;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Psycho;


public class YourPageModel : PageModel
{
    private static string logFile = "answer.csv";
    private string questions = "wwwroot/names.txt";

    public List<QuestionModel> Questions { get; set; } = new();

    public List<FieldData> QuestionsField { get; set; }

    public List<FrameData> Frames { get; set; }

    [BindProperty]
    public string FirstName { get; set; }

    [BindProperty]
    public string LastName { get; set; }

    [BindProperty]
    public string MiddleName { get; set; }

    [BindProperty]
    public List<FieldData> Fields { get; set; } = new();

    public static void CheckFile()
    {
        // Проверяем, существует ли файл
        if (System.IO.File.Exists(logFile))
        {
            // Удаляем файл
            System.IO.File.Delete(logFile);
        }
    }

    /// <summary>
    /// Проверяет наличие дубликатов в полях на странице.
    /// </summary>
    /// <param name="values"></param>
    /// <param name="counter"></param>
    /// <returns></returns>
    private bool CheckDuplicates(List<string> values, out int counter)
    {
        counter = 0;
        var arr = new List<string>();
        foreach (var val in values)
        {
            counter++;
            arr.Add(val);
            if (counter % 4 == 0)
            {
                if (HasDuplicates(arr))
                {
                    return true;
                }
                arr.Clear();
            }
        }
        return false;
    }

    /// <summary>
    /// Проверяет наличие дубликатов в списке.
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    private bool HasDuplicates(List<string> list)
    {
        HashSet<string> set = new HashSet<string>();

        foreach (string item in list)
        {
            if (!set.Add(item))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Реализует поведение при наличии дубликатов.
    /// </summary>
    /// <param name="counter"></param>
    /// <returns></returns>
    private IActionResult OnDuplicates(int counter)
    {
        TempData["SuccessMessage"] = $"Исправьте повторяющиеся значения рамки #{counter / 4}.";
        PageState.Frame = 0;
        return RedirectToPage();
    }

    private void Log(string answer)
    {
        using var writer = new System.IO.StreamWriter(logFile, true, System.Text.Encoding.UTF8);
        writer.WriteLine($"Фамилия;{LastName}");
        writer.WriteLine($"Имя;{FirstName}");
        writer.WriteLine($"Отчество;{MiddleName}");

        var fiel = Fields.Select(f => $"{f.FieldName};{f.Value}");

        foreach (var st in fiel)
        {
            writer.WriteLine(st);
        }
        writer.WriteLine();
        writer.WriteLine($"Ваш подтип;{answer}");
        writer.WriteLine();
    }

    private void UpdateHistory(List<string> values)
    {
        PageState.Frames.Clear();
        PageState.FillFrames(values);
        PageState.FirstName = FirstName;
        PageState.LastName = LastName;
        PageState.MiddleName = MiddleName;
    }

    private void CleanHistory()
    {
        PageState.FillFrames();
        PageState.FirstName = "";
        PageState.LastName = "";
        PageState.MiddleName = "";
    }

    public YourPageModel()
    {
        QuestionsField = new();
        Frames = new();
        Frames.AddRange(PageState.Frames);
    }

    public IActionResult OnPost(List<string> values)
    {
        UpdateHistory(values);

        if(CheckDuplicates(values, out int counter))
        {
            return OnDuplicates(counter);
        }

        var process = new TestProcessor(values);
        var answer = process.Process();

        //Log();

        TempData["SuccessMessage"] = $"Ваш подтип: {answer}";
        Frames.Clear();

        CleanHistory();
        
        return RedirectToPage();
    }

    public async Task OnGetAsync()
    {
        //await FillNew();
    }
}
