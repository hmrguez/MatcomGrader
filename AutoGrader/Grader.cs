namespace AutoGrader;

public class Grader
{
    public static string Grade(Dictionary<string, TestOutcomeCounts> results)
    {
        var allCorrect = results.All(x =>
            x.Value is { TimeoutCount: 0, WrongCount: 0, ExceptionCount: 0 });

        return allCorrect ? "🟢" : "🔴";
    }
}