# Ejemplo implementacion

Vamos a ver como es el tester para casos de implementacion con el ejemplo. Implementa Stack

*Sease la interfaz de IStack siguiente, haga una implementacion*

```csharp
namespace Exam;

public interface IStack
{
    public void Push(int value);
    public int Pop();
}
```

El template seria

```csharp
namespace Exam;

public static class Solution
{
    public static IStack Solve(int a)
    {
        throw new NotImplementedException();
    }
}
```

## Cosas para saber

El testeo de los ejercicios de implementacion es mas complicado que los otros. Para empezar no se me ocurre una manera
limpia de hacer casos autogenerados. Asi que el metodo de generar casos deberia devolver un array vacio y los casos se
implementan manuales

## Problem

```csharp
public class MyProblem : ITestProblem<int, IStack>
{
    public IEnumerable<object[]> GenerateTestCases(int seed, int numberOfCases)
    {
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
```

## Pruebas

Ahora como ya se dijo, las pruebas tienen que ser manuales. Asi que vamos a `AutoGrader/TestImplementation.cs` y ponemos
varios casos random

```csharp
using Exam;

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
```

> Para informacion en lo que hace el atributo Category ver [esto](format.md)