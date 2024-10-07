# Formateo

## Grupos

Los casos de pruebas se pueden agrupar por categorias. Esto es util pq por ejemplo se pueden hacer casos de pruebas para
gente que saquen 3, otros para la barrera del 4 y otros para el 5, y de esta forma se pueden ver quienes entran en cada
caso.

Esto se hace usando el atributo `Category` arriba de un caso de pruebas. Por ejemplo

```csharp
[Test, Category("Good Guys")]
public void TestingGoodGuys() { ... }
```

Las categorias evidentemente se pueden repetir y se pueden poner en casos de pruebas multiples como los autogenerados

## Facetas

El formateo tiene dos formas: global y no global

Global significa que todos los casos de pruebas seran aplanados y el resultado se dara como tal

| Name                  | Score | ✅ Passed | ⭕️ Wrong | ‼️ Exceptions | ⏰ Timeouts |
|-----------------------|-------|----------|----------|---------------|------------|
| Hector Rodriguez d.cs | 🟢    | 2        | 0        | 0             | 0          |
| Miguel Sosa d.cs      | 🔴    | -        | -        | -             | -          |

No global significa que los casos de prueba que esten agrupados se mostraran agrupados. Por ejemplo

| Name                  | Score | Good Guys               |
|-----------------------|-------|-------------------------|
| Hector Rodriguez d.cs | 🟢    | ✅ 2 / ⭕️ 0 / ‼️ 0 / ⏰ 0 |

