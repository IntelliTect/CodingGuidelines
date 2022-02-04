# C# Naming conventions - quick reference table

| Kind                                 | Naming Convention |  Examples          |
|--------------------------------------|-------------------|--------------------|
| Classes                              | PascalCase        | `class Car{}`   |
| Types and Namespaces                 | PascalCase        | `namespace SampleNamespace{}`    |
| Parameters                           | camelCase         | `public Car(int odometerMileage, string manufacturer)`  |
| Methods                              | PascalCase        | `public void StartEngine()`  |
| Properties                           | PascalCase        | `public double FuelLevel { get; set; }`  |
| Local Variables                      | camelCase         | `public int yearManufactured;`   |
| Local Functions                      | PascalCase        | `private string CalculateMilesUntilEmpty(double fuelLevel)` |
| Private Instance Fields              | _IPascalCase      |                    |
| Private Static Field                 | _PascalCase       |                    |
| Public Constant Field                | _PascalCase       |                    |
| Private Constant Field               | _PascalCase       |                    |
| Public Static Readonly Fields        | _PascalCase       |                    |
| Private Static Readonly Fields       | _PascalCase       |                    |
| Public Fields (not recommended)      | _PascalCase       | `public string Day;` |
| Enum Members                         | PascalCase        |                    |
| Type Parameters                      | TPascalCase       | `public delegate TOutput Converter<TInput, TOutput>(TInput from);` |
| Interfaces                           | IPascalCase       | `interface ISampleInterface`   |
