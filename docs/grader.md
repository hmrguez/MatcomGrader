# Grader

Se usa la clase en `AutoGrader/Grader.cs` para determinar el string que se imprime en la columna de Score del markdown

Es simplemente una funcion en la clase que recibe un Dictionary<string, TestOutcomeCounts> y devuelve el string a
imprimir

Aqui se puede hacer la logica de quienes llevan luz verde, quienes amarilla o quienes roja, o cualquier otro string que
se quiera poner

Por default tengo puesto poner luz verde a los que tengan todos los casos de prueba aprobados y luz roja en caso
contrario. Pero esto esta bueno ya que se pasa el diccionario agrupado por categorias, o sea aqui es donde digamos para
una categoria completa bien se puede poner luz amarilla:

```csharp
namespace AutoGrader;

public class Grader
{
    public static string Grade(Dictionary<string, TestOutcomeCounts> results)
    {
        var allCorrect = results.All(x =>
            x.Value is { TimeoutCount: 0, WrongCount: 0, ExceptionCount: 0 });

        if (allCorrect)
            return "🟢";
            
        if(results["Good Guys"].Value is { TimeoutCount: 0, WrongCount: 0, ExceptionCount: 0 })
            return "🟡";
            
        return "🔴";
    }
}
```

O por ejemplo poner que los que aprueben el 80% de casos estan aprobados:

```csharp
namespace AutoGrader;

public class Grader
{
    public static string Grade(Dictionary<string, TestOutcomeCounts> results)
    {
        var totalCases = results.Sum(x =>
            x.Value.PassedCount + x.Value.ExceptionCount + x.Value.TimeoutCount + x.Value.WrongCount);
        var totalApproved = results.Sum(x => x.Value.PassedCount);
        
        if(totalCases == totalApproved) return "🟢";
        return totalApproved >= 3 * totalCases / 4 ? "🟡" : "🔴";
    }
}
```

> Esto no bypassea los casos con stack overflow o errores de compilacion, estos casos se manejan en el archivo `MatcomGrader/Program.cs` al final