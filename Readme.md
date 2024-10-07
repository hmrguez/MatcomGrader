# Matcom Grader

## Como hacer el setup?

```bash
dotnet restore
```

## Como usar el tester?

Para poder usar el tester es necesario hacer 2 cosas:

1. Hacer un setup del problema a resolver
2. Configurar el run del tester

### Configurar run del tester

En `MatcomGrader/Program.cs` esta toda la logica para correr los proyectos, incluidos los archivos de configuracion
entre los que estan las variables de entorno

```csharp
psi.EnvironmentVariables["GLOBAL"] = "TRUE";
psi.EnvironmentVariables["NUMBER_OF_CASES"] = "2";
```

Global acepta `TRUE` o `FALSE` y se refiere a como es el formateo del markdown, si es aplanado en plan `45/100` casos, o
`10/30 para categoria 1 y 35/70 para categoria 2`. Mas de esto en el epigrafe de formateo

NUMBER_OF_CASES es el total de casos de prueba ejecutado pero su uso es cuando se van a calcular los
casos de prueba, el complemento de lo que no sea correcto, mal, exception o timeout se agrega a excepciones, TODO hay
que hacer esto por el total de casos en el tester, pq de lo contrario este numero esta mal si le sumas lo siguiente.
Ademas se usa para ver cuantos casos automaticos se generan.

Ademas en este archivo es "necesario" modificar esta linea

```csharp
string[] headers = ["Name", "✅ Passed", "⭕️ Wrong", "‼️ Exceptions", "⏰ Timeouts"];
```

Aqui se especifican las columnas del markdown. Por lo general se generan automaticamente pero si la primera prueba tiene
errores de compilacion si es necesario tenerlo bien. TODO pendiente

### Setup del problema a resolver

Este tester funciona como aquel que empezo Leandro. Se tiene una carpeta Solution con todas las pruebas. Luego para
probar cada prueba individual se copia cada solucion a la carpeta de testeo donde "sobreescribe" la plantilla que estaba
ahi y luego se corren los tests. Luego cuando se acaban todas las pruebas se busca el archivo `Solutions/_base.cs` y se
copia para `AutoGrader/Solution.cs` sobreescribir y tener aunque sea una plantilla en la carpeta de testeo

Para empezar primero ajustar los archivos `Solutions/_base.cs` y `AutoGrader/Solution.cs` para que sean iguales las
plantillas que les damos a los estudiantes

Luego se debe definir el problema. O sea en `AutoGrader/Problem.cs` hay una implementacion de ITestProblem, ajustarla a
nuestro problema.

```csharp
public interface ITestProblem<TInput, TOutput>
{
    IEnumerable<object[]> GenerateTestCases(int seed, int numberOfCases);
    TOutput CorrectSolution(TInput input);

    TOutput StudentSolution(TInput input);
    
    bool CompareSolutions(TOutput expected, TOutput actual);
}
```

1. `TInput` es el tipo del parametro de entrada del problema (TODO implementar para varios parametros, de otra forma las
   pruebas reciben un solo parametro y si hacen falta varios se hace una clase de wrapper) y `TOutput` es lo que debe
   devolver la prueba
2. `CorrectSolution` es obvio lo q es, la solucion nuestra del problema
3. `StudentSolution` es devolver la solucion de la plantilla de `AutoGrader/Solution.cs`, que en la mayoria de casos no
   hay ni que hacerlo, pq todas las plantillas son iguales lo unico que cambia es los parametros
4. `CompareSolutions` es el metodo que vamos a usar para comprobar que dos soluciones son correctas, o sea por ejemplo
   en problemas donde el resultado es int uno haria `return expected == actual`
5. `GenerateTestCases` es el metodo que usamos para generar casos. Devuelve un IEnumerable de array de object que seria
   un IEnumerable de los parametros de cada caso de prueba, o sea un IEnumerable de [input, expectedOutput]. En el
   ejemplo se ve mas claro. Como funciona el tester es que por default solo rellenando esto ya deberia funcionar el
   tester completo, ya que ejecuta los casos de pruebas que se hayan generado en este metodo. Se pasa un seed por si se
   usa un Random que los casos de pruebas sean uniformes

## Como correr?

```bash
dotnet run --project MatcomGrader/
```

## FAQ

- [Formateo del MD](docs/format.md)
- [Ejemplo de TwoSum](docs/twosum.md)
- [Ejemplo de Implementacion](docs/implementation.md)