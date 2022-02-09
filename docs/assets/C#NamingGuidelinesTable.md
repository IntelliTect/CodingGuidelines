# C# Naming Conventions - Quick Reference Table

| Kind                                 | Naming Convention |  Example                                                           |
| ------------------------------------ | ----------------- | ------------------------------------------------------------------ |
| Classes                              | PascalCase        | `class Car{}`                                                      |
| Types and Namespaces                 | PascalCase        | `namespace VehicleManufacturer{}`                                  |
| Parameters                           | camelCase         | `public Car(int odometerMileage, string manufacturer)`             |
| Methods                              | PascalCase        | `public void StartEngine()`                                        |
| Properties                           | PascalCase        | `public double FuelLevel { get; set; }`                            |
| Local Variables                      | camelCase         | `public int yearManufactured;`                                     |
| Local Functions                      | PascalCase        | `private string CalculateMilesUntilEmpty(double fuelLevel)`        |
| Fields                               | _PascalCase       | `public string _Day;`                                              |
| Enum Members                         | PascalCase        | `enum Status { Operational, Broken, InShop }`                      |
| Type Parameters                      | TPascalCase       | `public delegate TOutput Converter<TInput, TOutput>(TInput from);` |
| Interfaces                           | IPascalCase       | `interface ISampleInterface`                                       |
