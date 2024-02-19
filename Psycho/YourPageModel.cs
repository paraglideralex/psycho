using System.Collections;
using System.Diagnostics.Metrics;
using System.Text;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Psycho;

using static System.Runtime.InteropServices.JavaScript.JSType;

public class YourPageModel : PageModel
{
    private static string logFile = "answer.csv";

    private static string questions = "wwwroot/names.txt";

    public List<QuestionModel> Questions { get; set; } = new();

    public List<FieldData> QuestionsField { get; set; }

    public List<FrameData> Frames { get; set; } = new();

    [BindProperty]
    public string FirstName { get; set; }

    [BindProperty]
    public string LastName { get; set; }

    [BindProperty]
    public string MiddleName { get; set; }


    public static void CheckFile()
    {
        // Проверяем, существует ли файл
        if (System.IO.File.Exists(logFile))
        {
            // Удаляем файл
            System.IO.File.Delete(logFile);
        }
    }
    /*
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
    */
    /*
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
    */

    private string Report(string answer)
    {
        string ans = "";
        ans += $"Фамилия;{LastName}\r\nИмя;{FirstName}\r\nОтчество;{MiddleName}\r\n";

        foreach (var frame in Frames)
        {
            foreach (var field in frame.FrameContent)
            {
                ans += $"{field.FieldName};{field.Value}\r\n";
            }
            
        }
        ans += $"Ваш подтип;{answer}\r\n";
        return ans;
    }

    /*
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

    private readonly AppDbContext db;
    */

    private static List<string> ListFill()
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

    public async Task FillFrames()
    {
        Frames.Clear();
        var arr = new List<FieldData>();
        var lines = ListFill();
        int count = 0;
        foreach (string line in lines)
        {
            arr.Add(new FieldData { FieldName = line, Value = (count % 4 + 1).ToString() });
            count++;
            if (count % 4 == 0)
            {
                List<FieldData> copiedList = new List<FieldData>(arr);
                Frames.Add(new FrameData(copiedList));

                arr.Clear();
            }
        }
    }

    public async Task FillFrames(List<string> values)
    {
        var arr = new List<FieldData>();
        var lines = ListFill();
        int count = 0;
        foreach (string line in lines)
        {
            arr.Add(new FieldData { FieldName = line, Value = values[count].ToString() });
            count++;
            if (count % 4 == 0)
            {
                List<FieldData> copiedList = new List<FieldData>(arr);
                Frames.Add(new FrameData(copiedList));

                arr.Clear();
            }
        }
    }
    public YourPageModel()
    {
        QuestionsField = new();
        FillFrames();
        //this.db = db;
        //Frames.AddRange(db.Fra);
        //Frames.AddRange(PageState.Frames);
    }

    public byte[] CreateBinary(string answer)
    {
        // получаем данные из TempData
        string fil = Report(answer);
        //string fil = PageState.Result;
        byte[] fileData = Encoding.UTF8.GetBytes(fil);// TempData["file"] as byte[];
        return fileData;

        //string fileName = "results.txt";

        //return File(fileData, "text/txt", fileName);
    }


    [HttpPost]
    public IActionResult OnPost(List<string> values)
    {
        //UpdateHistory(values);

        //if(CheckDuplicates(values, out int counter))
        //{
        //    return OnDuplicates(counter);
        //}
        Frames.Clear();
        FillFrames(values);

        var process = new TestProcessor(values);
        var answer = process.Process();

        //TempData["SuccessMessage"] = $"Ваш подтип: {answer}";

        


        //TempData["file"] = Report(answer);

        //PageState.Result = Report(answer);

        //Frames.Clear();

        //CleanHistory();

        //return RedirectToPage();

        return File(CreateBinary(answer), "text/txt", "results.txt");
    }

    public async Task OnGetAsync()
    {
        //await FillNew();
    }
}
