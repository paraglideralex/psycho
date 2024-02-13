namespace Psycho;

public static class PageState
{
    public static int Frame = 0;
    private static string questions = "wwwroot/names.txt";

    public static List<FrameData> Frames { get; set; } = new List<FrameData>();

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

    public static async Task FillFrames()
    {
        var arr = new List<FieldData>();
        var lines = ListFill();
        int count = 0;
        foreach (string line in lines)
        {
            arr.Add(new FieldData { FieldName = line, Value = (count%4+1).ToString() });
            count++;
            if (count % 4 == 0)
            {
                List<FieldData> copiedList = new List<FieldData>(arr);
                Frames.Add(new FrameData(copiedList));

                arr.Clear();
            }
        }
    }

    public static async Task FillFrames(List<string> values)
    {
        var arr = new List<FieldData>();
        int count = 0;
        foreach (string line in values)
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
}
