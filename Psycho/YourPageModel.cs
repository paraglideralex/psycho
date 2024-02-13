using System.Collections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Psycho;


public class YourPageModel : PageModel
{
    private static string logFile = "answer.csv";
    private string questions = "wwwroot/names.txt";
    int frameCounter;

    public List<QuestionModel> Questions { get; set; } = new();

    public List<FieldData> QuestionsField { get; set; }

    public List<FrameData> Frames { get; set; }

    public YourPageModel()
    {
        QuestionsField = new();
        Frames = new();
        Frames.AddRange(PageState.Frames);
    }

    public static void CheckFile()
    {
        // Проверяем, существует ли файл
        if (System.IO.File.Exists(logFile))
        {
            // Удаляем файл
            System.IO.File.Delete(logFile);
        }
    }

    [BindProperty]
    public string FirstName { get; set; }

    [BindProperty]
    public string LastName { get; set; }

    [BindProperty]
    public string MiddleName { get; set; }

    [BindProperty]
    public List<FieldData> Fields { get; set; } = new();

    private bool Repeats(List<string> values, out int index)
    {
        index = 0;
        foreach (var frame in Frames)
        {
            index++;
            PageState.Frame++;
            if (HasDuplicates(frame.FrameContent))
            {
                PageState.FirstName = FirstName; PageState.LastName = LastName; PageState.MiddleName = MiddleName;

                TempData["SuccessMessage"] = $"Исправьте повторяющиеся значения рамки #{PageState.Frame}.";
                PageState.Frame = 0;
                return true;
            }
        }
        return false;
    }

    private bool CheckDuplicates(List<string> list)
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

    private bool HasDuplicates(List<FieldData> fieldDataList)
    {
        // Проверка, есть ли хотя бы одна группа с более чем одним элементом (дубликаты)
        return fieldDataList.GroupBy(data => data.Value)
                            .Any(group => group.Count() > 1);
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

    public IActionResult OnPost(List<string> values)
    {
        int counter = 0;
        var arr = new List<string>();
        PageState.Frames.Clear();
        PageState.FillFrames(values);
        PageState.FirstName = FirstName; PageState.LastName = LastName; PageState.MiddleName = MiddleName;
        foreach (var val in values)
        {
            counter++;
            arr.Add(val);
            if (counter % 4 == 0)
            {
                if (CheckDuplicates(arr))
                {
                    TempData["SuccessMessage"] = $"Исправьте повторяющиеся значения рамки #{counter / 4}.";
                    PageState.Frame = 0;
                    //PageState.Frames.Clear();
                    return RedirectToPage();
                    
                }
                arr.Clear();
            }

            //if (HasDuplicates(frame.FrameContent))
            //{
            //    TempData["SuccessMessage"] = $"Исправьте повторяющиеся значения рамки #{counter}.";
            //    PageState.Frame = 0;
            //    return RedirectToPage();
            //}
            //foreach (var field in frame.FrameContent)
            //{
            //    Fields.Add(new FieldData { FieldName = field.FieldName, Value = values[counter] });
            //    counter++;
            //}
        }

        
        // Записываем содержимое в файл

        var process = new TestProcessor(values);
        var answer = process.Process();

        //Log();

        //if(Repeats(values, out int ind))
        //{
        //    return RedirectToPage();
        //}

        TempData["SuccessMessage"] = $"Ваш подтип: {answer}";
        Frames.Clear();
        

        return RedirectToPage();
    }

    /*
    private List<string> ListFill()
    {
        string path = questions;
        List<string> lines = new List<string>();


        // Используем StreamReader для чтения файла построчно
        using (StreamReader reader = new StreamReader(path))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                lines.Add(line);
            }
        }

        return lines;
    }

    private async Task FillFrames()
    {
        var arr = new List<FieldData>();
        var lines = ListFill();
        int count = 0;
        foreach (string line in lines)
        {
            arr.Add(new FieldData { FieldName = line});
            count++;
            if (count % 4 == 0)
            {
                List<FieldData> copiedList = new List<FieldData>(arr);
                Frames.Add(new FrameData(copiedList));

                arr.Clear();
            }
        }
    }
    */

    public async Task OnGetAsync()
    {
        //await FillNew();
    }
}
