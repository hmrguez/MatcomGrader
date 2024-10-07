# Ejemplo de Two Sum

Veamos el ejemplo del siguiente problema:

*Dado un array a y un entero k, determine si existen 2 numeros en a que sumen k*

## Plantilla

```csharp
namespace Exam;

public static class Solution
{
    public static bool Solve(int[] a, int k)
    {
        throw new NotImplementedException();
    }
}
```

> Se ponen en el mismo namespace o puedes poner using, no hay problema

Ahora como ya se menciono se copia la plantilla al `Solutions/_base.cs` y al `AutoGrader/Solution.cs`. La clase de Input tiene que estar en el proyecto de `AutoGrader`


## Problem

Ahora necesitamos hacer la implementacion del problema. Vamos a `AutoGrader/Problem.cs` y cambiamos la implementacion de `MyProblem`

```csharp
public class MyProblem : ITestProblem<bool>
{
    public IEnumerable<object[]> GenerateTestCases(int seed, int numberOfCases)
    {
        var random = new Random(seed);
        
        // Alguna logica para generar array y k random
        int[] a = []
        int k = random.Next(10000);
        
        for(int i=0; i<numberOfCases; i++)
        {
            var input = new Input { a = a, k = k };
            yield return [input, CorrectSolution(input)]
        }
    }

    public bool CorrectSolution(params object[] parameters)
    {
        return ...;
    }

    public bool StudentSolution(params object[] parameters)
    {
        var a = (int[])parameters[0];
        var k = (int)parameters[1];
        
        return Exam.Solution.Solve(a, k);
    }

    public bool CompareSolutions(int expected, int actual)
    {
        return expected == actual;
    }
}
```

## Configurar runner

Lo unico tecnicamente importante aqui seria el numero de casos de pruebas

