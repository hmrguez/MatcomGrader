using System.Runtime.InteropServices.JavaScript;
using Common;
using NUnit.Framework.Interfaces;

namespace AutoGrader;

[TestFixture]
public abstract class GenericTest<TProblem, TInput, TOutput>
    where TProblem : ITestProblem<TInput, TOutput>, new()
{
    [TearDown]
    public void TearDown()
    {
        // Retrieve the category of the current test
        var categoryList = TestContext.CurrentContext.Test.Properties["Category"] as List<object>;
        var category = (categoryList != null && categoryList.Count > 0) ? categoryList[0] as string : "Uncategorized";

        // Initialize counts for this category if not already present
        if (!_countsByCategory.ContainsKey(category))
        {
            _countsByCategory[category] = new TestOutcomeCounts();
        }

        var counts = _countsByCategory[category];

        // Determine the outcome of the test
        var outcome = TestContext.CurrentContext.Result.Outcome.Status;
        var message = TestContext.CurrentContext.Result.Message;

        switch (outcome)
        {
            case TestStatus.Passed:
                counts.PassedCount++;
                break;
            case TestStatus.Failed:
                if (message != null && message.Contains("TIMEOUT"))
                    counts.TimeoutCount++;
                else if (message != null && message.Contains("WRONG"))
                    counts.WrongCount++;
                else
                    counts.ExceptionCount++;
                break;
        }
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        if (Global)
        {
            // Aggregate results across all categories
            int totalPassed = 0;
            int totalWrong = 0;
            int totalException = 0;
            int totalTimeout = 0;

            foreach (var counts in _countsByCategory.Values)
            {
                totalPassed += counts.PassedCount;
                totalWrong += counts.WrongCount;
                totalException += counts.ExceptionCount;
                totalTimeout += counts.TimeoutCount;
            }

            var totalTests = NumberOfCases;
            var totalCounted = totalPassed + totalWrong + totalException + totalTimeout;

            // Adjust for any discrepancies
            if (totalCounted < totalTests)
            {
                var difference = totalTests - totalCounted;
                totalException += difference;
            }

            // Write flat results
            var mdHeaders = new[] { "Name", "✅ Passed", "⭕️ Wrong", "‼️ Exceptions", "⏰ Timeouts" };
            var writer = new MarkdownWriter(MdPath, mdHeaders);
            writer.AddRow(Name, totalPassed.ToString(), totalWrong.ToString(), totalException.ToString(),
                totalTimeout.ToString());
        }
        else
        {
            // Write grouped results by category
            // Prepare headers
            var headers = new List<string> { "Name" };
            var categoryList = _countsByCategory.Keys.ToList();
            categoryList.Sort(); // Optional: sort categories alphabetically

            headers.AddRange(categoryList);
            var writer = new MarkdownWriter(MdPath, headers.ToArray());

            // Prepare data row
            var dataRow = new List<string> { Name };

            foreach (var category in categoryList)
            {
                var counts = _countsByCategory[category];
                var countsStr =
                    $"✅ {counts.PassedCount} / ⭕️ {counts.WrongCount} / ‼️ {counts.ExceptionCount} / ⏰ {counts.TimeoutCount}";
                dataRow.Add(countsStr);
            }

            writer.AddRow(dataRow.ToArray());
        }

        Console.WriteLine("Test summary written to results.md");
    }

    // Configuration Constants
    private static readonly int NumberOfCases = int.Parse(Environment.GetEnvironmentVariable("NUMBER_OF_CASES"));
    private const int RandomSeed = 10;
    private const string MdPath = "../../../../results.md";
    private static readonly string Name = Environment.GetEnvironmentVariable("CURRENT_SOLUTION_FILE")!;
    private static readonly bool Global = Environment.GetEnvironmentVariable("GLOBAL")! == "TRUE";

    private Dictionary<string, TestOutcomeCounts> _countsByCategory = new Dictionary<string, TestOutcomeCounts>();

    private class TestOutcomeCounts
    {
        public int PassedCount { get; set; } = 0;
        public int WrongCount { get; set; } = 0;
        public int ExceptionCount { get; set; } = 0;
        public int TimeoutCount { get; set; } = 0;
    }


    protected static readonly TProblem Problem = new();


    [Test]
    [TestCaseSource(nameof(GetTestCases))]
    public void AutoGeneratedSolutions(TInput input, TOutput expectedOutput)
    {
        var task = Task.Run(() => Problem.StudentSolution(input));

        if (task.Wait(TimeSpan.FromSeconds(5)))
        {
            var actualOutput = task.Result;
            var isEqual = Problem.CompareSolutions(expectedOutput, actualOutput);

            Assert.IsTrue(isEqual, $"WRONG Expected: {expectedOutput}, Actual: {actualOutput}");
        }
        else
        {
            Assert.Fail("TIMEOUT");
        }
    }

    private static IEnumerable<TestCaseData> GetTestCases()
    {
        foreach (var testCase in Problem.GenerateTestCases(RandomSeed, NumberOfCases))
            yield return new TestCaseData(testCase[0], testCase[1])
                .SetName($"Input_{testCase[0]}_Expected_{testCase[1]}");
    }
}