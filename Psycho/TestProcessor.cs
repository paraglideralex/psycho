namespace Psycho;

public class TestProcessor
{
    private List<string> data;
    public TestProcessor(List<string> values) => data = values;

    private int Compute(List<int> content)
    {
        return content.Sum();
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
        List<int> values = new ();
        foreach (var i in data)
        {
            values.Add(int.Parse (i));
        }

        int value = Compute (values);
        string ans = Classify(value);

        return ans;
    }
}
