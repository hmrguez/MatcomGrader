namespace AutoGrader;

public interface ITestProblem<TInput, TOutput>
{
    IEnumerable<object[]> GenerateTestCases(int seed, int numberOfCases);
    TOutput CorrectSolution(TInput input);

    TOutput StudentSolution(TInput input);
    
    bool CompareSolutions(TOutput expected, TOutput actual);
}

public class MyProblem : ITestProblem<int, int>
{
    public IEnumerable<object[]> GenerateTestCases(int seed, int numberOfCases)
    {
        var random = new Random(seed);

        for (int i = 0; i < numberOfCases; i++)
        {
            int input = random.Next(1, 15); 
            int expectedOutput = CorrectSolution(input);

            yield return new object[] { input, expectedOutput };
        }
    }

    public int CorrectSolution(int input)
    {
        return input * 2;
    }

    public int StudentSolution(int input)
    {
        return Exam.Solution.Solve(input);
    }

    public bool CompareSolutions(int expected, int actual)
    {
        return expected == actual;
    }
}