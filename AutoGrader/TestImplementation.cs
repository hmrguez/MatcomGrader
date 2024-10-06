using Stackish;

namespace AutoGrader;

public class MyProblemTests : GenericTest<MyProblem, int, IStack>
{
    [Test, Category("Category 2")]
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
    
    [Test, Category("Category 2")]
    public void TestingPushPop2()
    {
        var user = Problem.StudentSolution(5);
        var mySolution = Problem.CorrectSolution(5);

        user.Push(4);
        mySolution.Push(4);
        user.Push(7);
        mySolution.Push(7);


        var temp = user.Pop();
        var temp2 = mySolution.Pop();

        Assert.That(temp2, Is.EqualTo(temp));

        temp = user.Pop();
        temp2 = mySolution.Pop();

        Assert.That(temp2, Is.EqualTo(temp));
    }
}