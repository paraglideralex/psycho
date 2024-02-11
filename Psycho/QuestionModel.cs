namespace Psycho;

public class QuestionModel
{
    public string[] QuestionText { get; set; }
    public string AnswerName { get; set; }

    public QuestionModel() // Конструктор по умолчанию без параметров
    {
    }

    public QuestionModel(string[] one, string two) // Конструктор по умолчанию без параметров
    {
        QuestionText = one;
        AnswerName = two;
    }

}
