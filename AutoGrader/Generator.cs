namespace Grader;

public class Generator
{
    public static IEnumerable<object[]> GenerateTestCases(int seed, int numberOfCases)
    {
        var random = new Random(seed);

        for (int i = 0; i < numberOfCases; i++)
        {
            int input = random.Next(1, 15); // Example input range
            int expectedOutput = CorrectSolution(input);

            // Yield return an object array with input and expected output
            yield return new object[] { input, expectedOutput };
        }
    }

    public static int CorrectSolution(int input)
    {
        // Implement the correct logic here
        // For example, calculate the factorial
        return 5;
    }
}