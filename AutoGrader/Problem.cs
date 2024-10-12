namespace AutoGrader;

public interface ITestProblem<TOutput>
{
    IEnumerable<object[]> GenerateTestCases(int seed, int numberOfCases);
    TOutput CorrectSolution(params object[] parameters);

    TOutput StudentSolution(params object[] parameters);

    bool CompareSolutions(TOutput expected, TOutput actual);
}

public class MyProblem : ITestProblem<int>
{
    public IEnumerable<object[]> GenerateTestCases(int seed, int numberOfCases)
    {
        var random = new Random(seed);

        for (int i = 0; i < numberOfCases; i++)
        {
            int input = random.Next(1, 15);
            int expectedOutput = CorrectSolution(input, input + 2);

            yield return [new object[] { input, input + 2 }, expectedOutput];
        }
    }

    public int CorrectSolution(params object[] parameters)
    {
        var a = (int)parameters[0];
        var b = (int)parameters[1];

        return a * 2 + b * 2;
    }

    public int StudentSolution(params object[] parameters)
    {
        var a = (int)parameters[0];
        var b = (int)parameters[1];

        return Exam.Solution.Solve(a, b);
    }

    public bool CompareSolutions(int expected, int actual)
    {
        return expected == actual;
    }
}