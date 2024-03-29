﻿using System.Collections;
using System.Diagnostics.Metrics;
using System.Text;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Psycho;

using SocialApp.Services;

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

    private string ReportExcel()
    {
        List<string> list = new List<string>();

        foreach (var frame in Frames)
        {
            foreach (var field in frame.FrameContent)
            {
                list.Add(field.Value);
            }
        }

        string answer = $"{DateTime.Now}\r\n{LastName} {FirstName} {MiddleName}\r\n{string.Join('\t', list)}";
        return answer;
    }

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
    }

    public byte[] CreateBinary(string answer)
    {
        // получаем данные из TempData
        //string fil = Report(answer);
        string fil = ReportExcel();

        byte[] fileData = Encoding.UTF8.GetBytes(fil);// TempData["file"] as byte[];
        return fileData;
    }

    public async Task<IActionResult> SendMessage(string emailTo)
    {
        EmailService emailService = new EmailService();
        await emailService.SendEmailAsync(emailTo, "Ответы на психологический тест", ReportExcel());
        return RedirectToPage();
    }

    [HttpPost]
    public async Task<IActionResult> OnPost(List<string> values)
    {
        Frames.Clear();
        FillFrames(values);

        // тут можно гонять процессор тестов, когда он будет кем-то допилен (не мной)
        var process = new TestProcessor(values); 
        var answer = process.Process();


        //using var writer = new StreamWriter("results.txt");
        //{
        //    writer.Write(ReportExcel());
        //    writer.Close();
        //}

        await SendMessage("vladshatalov@yandex.ru");

        return File(CreateBinary(answer), "text/txt", "results.txt");
    }

    public async Task OnGetAsync()
    {
    }
}
