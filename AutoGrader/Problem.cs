using System.Collections;
using Stackish;

namespace AutoGrader;

public interface ITestProblem<TInput, TOutput>
{
    IEnumerable<object[]> GenerateTestCases(int seed, int numberOfCases);
    TOutput CorrectSolution(TInput input);

    TOutput StudentSolution(TInput input);
    
    bool CompareSolutions(TOutput expected, TOutput actual);
}



public class StackImplementation : IStack
{
    private Stack<int> stack = new();

    public void Push(int value)
    {
        stack.Push(value);
    }

    public int Pop()
    {
        if (stack.Count == 0)
        {
            return -1;
        }
        return stack.Pop();
    }
}

public class MyProblem : ITestProblem<int, IStack>
{
    public IEnumerable<object[]> GenerateTestCases(int seed, int numberOfCases)
    {
        // yield return [new Input { a = a, k = k }];
        // var random = new Random(seed);
        //
        // for (int i = 0; i < numberOfCases; i++)
        // {
        //     int input = random.Next(1, 15); 
        //     int expectedOutput = CorrectSolution(input);
        //
        //     yield return new object[] { input, expectedOutput };
        // }

        return [];
    }

    public IStack CorrectSolution(int input)
    {
        return new StackImplementation();
    }

    public IStack StudentSolution(int input)
    {
        return Exam.Solution.Solve(input);
    }

    public bool CompareSolutions(IStack expected, IStack actual)
    {
        return expected == actual;
    }
}