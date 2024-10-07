# Ejemplo de Two Sum

Veamos el ejemplo del siguiente problema:

*Dado un array a y un entero k, determine si existen 2 numeros en a que sumen k*

## Plantilla

Como tenemos 2 parametros y el TODO no esta hecho todavia XD, creamos en otro archivo la clase de input

```csharp
namespace Exam; 

public class Input {
    public int[] a { get; }
    public int k { get; }
}

```

```csharp
namespace Exam;

public static class Solution
{
    public static bool Solve(int Input)
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
public class MyProblem : ITestProblem<Input, bool>
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

    public bool CorrectSolution(Input input)
    {
        return ...;
    }

    public bool StudentSolution(Input input)
    {
        return Exam.Solution.Solve(input);
    }

    public bool CompareSolutions(IStack expected, IStack actual)
    {
        return expected == actual;
    }
}
```

## Configurar runner

Lo unico tecnicamente importante aqui seria el numero de casos de pruebas

