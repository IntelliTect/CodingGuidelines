# C# Naming Conventions - Quick Reference Table

| Kind                                 | Naming Convention |  Example                                                           |
| ------------------------------------ | ----------------- | ------------------------------------------------------------------ |
| Classes                              | PascalCase        | `class Car`                                                      |
| Types and Namespaces                 | PascalCase        | `namespace VehicleManufacturer;`                                   |
| Parameters                           | camelCase         | `public Car(int odometerMileage, string manufacturer)`             |
| Methods                              | PascalCase        | `public void StartEngine()`                                        |
| Properties                           | PascalCase        | `public double FuelLevel { get; set; }`                            |
| Local Variables                      | camelCase         | `int yearManufactured;`                                            |
| Local Functions                      | PascalCase        | `string CalculateMilesUntilEmpty(double fuelLevel)`                |
| Fields                               | _PascalCase       | `private string _Day;`                                             |
| Enum Members                         | PascalCase        | `enum Status { Unknown, Operational, Broken, InShop }`             |
| Type Parameters                      | TPascalCase       | `public TOutput Convert<TInput, TOutput>(TInput from)`             |
| Interfaces                           | IPascalCase       | `interface ISampleInterface`                                       |
