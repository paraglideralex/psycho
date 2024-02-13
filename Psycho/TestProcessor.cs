namespace Psycho;

public class TestProcessor
{
    private List<string> data;
    public TestProcessor(List<string> items)
    {
        FillValues(items);
    }

    private List<List<int>> values = new();

    private void FillValues(List<string> items)
    {
        var arr = new List<int>();
        int count = 0;
        foreach (string line in items)
        {
            arr.Add(Convert.ToInt32(line));
            count++;
            if (count % 4 == 0)
            {
                var list = new List<int>(arr);
                values.Add(list);

                arr.Clear();
            }
        }
    }

    private int Compute()
    {
        int first = 0;
        int second = 0;
        int third = 0;
        int forth = 0;
        foreach (var i in values)
        {
            first += i[0];
            second += i[1];
            third += i[2];
            forth += i[3];
        }

        var list = new List<int>() { first, second, third, forth};
        int ans = list.Max();
        return ans;
    }

    private string Classify(int number)
    {
        int lastStringPosition = number.ToString().Length - 1;
        string num = number.ToString();
        string let = num.Substring(lastStringPosition);
        int last = Convert.ToInt32(let);

        return last switch
        {
            0 => "Норм работяга",
            1 => "Образцовый работник",
            2 => "Секретный физик",
            3 => "Пассивный агрессор",
            4 => "Арбузер",
            5 => "Газлайтер",
            6 => "Лидер",
            7 => "Коуч",
            8 => "Альфа-самец",
            9 => "Теневой онанист",
        };
    }
    public string Process()
    {
        int value = Compute();
        string ans = Classify(value);

        return ans;
    }
}
