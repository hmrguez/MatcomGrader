namespace AutoGrader;

public class Grader
{
    public static string Grade(Dictionary<string, TestOutcomeCounts> results)
    {
        
        
        
        var totalCases = results.Sum(x =>
            x.Value.PassedCount + x.Value.ExceptionCount + x.Value.TimeoutCount + x.Value.WrongCount);
        var totalApproved = results.Sum(x => x.Value.PassedCount);

        
        return totalApproved >= 3 * totalCases / 4 ? "🟢" : "🔴";


            var allCorrect = results.All(x =>
                x.Value is { TimeoutCount: 0, WrongCount: 0, ExceptionCount: 0 });

        return allCorrect ? "🤩" : "🔴";
    }
}