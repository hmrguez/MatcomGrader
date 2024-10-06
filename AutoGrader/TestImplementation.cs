using Stackish;

namespace AutoGrader;

public class MyProblemTests : GenericTest<MyProblem, int, IStack>
{
    [Test]
    public void TestingPushPop()
    {
        var user = Problem.StudentSolution(5);
        var mySolution = Problem.CorrectSolution(5);

        user.Push(2);
        mySolution.Push(2);
        user.Push(5);
        mySolution.Push(5);


        var temp = user.Pop();
        var temp2 = mySolution.Pop();

        Assert.That(temp2, Is.EqualTo(temp));

        temp = user.Pop();
        temp2 = mySolution.Pop();

        Assert.That(temp2, Is.EqualTo(temp));
    }
}