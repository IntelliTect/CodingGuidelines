## Sections

- [Coding](#coding)



     - [Arrays](#arrays)

     - [Assemblies](#assemblies)

     - [Branches](#branches)

     - [Classes](#classes)

     - [Conversions](#conversions)

     - [Dispose()](#dispose())

     - [Enums](#enums)

     - [Equality](#equality)

     - [Exceptions](#exceptions)

     - [Fields](#fields)

     - [Flags](#flags)

     - [Increment/decrement](#incrementdecrement)

     - [Interfaces](#interfaces)

     - [Methods](#methods)

     - [Miscellaneous](#miscellaneous)

     - [Parameters](#parameters)

     - [Properties](#properties)

     - [Strings](#strings)

     - [Structs](#structs)

     - [Synchronization](#synchronization)

     - [Tasks](#tasks)

     - [Threads](#threads)

     - [ToString()](#tostring())

     - [Types](#types)

- [Layout](#layout)



     - [Comments](#comments)

     - [Files](#files)

     - [Whitespace](#whitespace)

- [Naming](#naming)



     - [Abbreviations](#abbreviations)

     - [Casing](#casing)

     - [Methods](#methods)

     - [Miscellaneous](#miscellaneous)

     - [Namespaces](#namespaces)

     - [Types](#types)

     - [Variables and fields](#variables and fields)



## Coding



### Arrays

- CONSIDER checking for null before accessing an array rather than assuming there is an array instance.

- CONSIDER checking the array length before indexing into an array rather than assuming the length.

- CONSIDER using the index from end operator (^) rather than Length - 1 with C# 8.0 or higher.

- DO use parameter arrays when a method can handle any number—including zero—of additional arguments.

- AVOID unsafe array covariance. Instead, CONSIDER converting the array to the read-only interface IEnumerable<T>, which can be safely converted via covariant conversions.

### Assemblies

- DO apply AssemblyVersionAttribute to assemblies with public types.

- CONSIDER applying AssemblyFileVersionAttribute and AssemblyCopyrightAttribute to provide additional information about the assembly.

- DO apply the following information assembly attributes: System.Reflection.AssemblyCompanyAttribute, System.Reflection.AssemblyCopyrightAttribute, System.Reflection.AssemblyDescriptionAttribute, and System.Reflection.AssemblyProductAttribute.

- DO name custom attribute classes with the suffix Attribute.

- DO provide get-only properties (without public setters) on attributes with required property values.

- DO provide constructor parameters to initialize properties on attributes with required properties. Each parameter should have the same name (albeit with different casing) as the corresponding property.

- DO apply the AttributeUsageAttribute class to custom attributes.

### Branches

- CONSIDER using an if-else statement instead of an overly complicated conditional expression.

- CONSIDER refactoring the method to make the control flow easier to understand if you find yourself writing for loops with complex conditionals and multiple loop variables.

- DO use the for loop when the number of loop iterations is known in advance and the “counter” that gives the number of iterations executed is needed in the loop.

- DO use the while loop when the number of loop iterations is not known in advance and a counter is not needed.

- DO NOT use continue as the jump statement that exits a switch section. This is legal when the switch is inside a loop, but it is easy to become confused about the meaning of break in a later switch section.

- AVOID using goto.

### Classes

- DO implement IDisposable to support possible deterministic finalization on classes with finalizers.

- DO place multiple generic semantically equivalent classes into a single file if they differ only by the number of generic parameters.

- AVOID shadowing a type parameter of an outer type with an identically named type parameter of a nested type.

- DO pass the instance of the class as the value of the sender for nonstatic events.

- DO NOT unnecessarily replicate existing managed classes that already perform the function of the unmanaged API.

### Conversions

- AVOID direct enum/string conversions where the string must be localized into the user’s language.

- DO NOT provide an implicit conversion operator if the conversion is lossy.

- DO NOT throw exceptions from implicit conversions.

### Dispose()

- DO implement the dispose pattern on objects with resources that are scarce or expensive.

- DO unregister any AppDomain.ProcessExit events during dispose.

- DO call System.GC.SuppressFinalize() from Dispose() to avoid repeating resource cleanup and delaying garbage collection on an object.

- DO ensure that Dispose() is idempotent (it should be possible to call Dispose() multiple times).

- DO keep Dispose() simple, focusing on the resource cleanup required by finalization.

- AVOID calling Dispose() on owned objects that have a finalizer. Instead, rely on the finalization queue to clean up the instance.

- DO invoke a base class’s Dispose() method when overriding Dispose().

- CONSIDER ensuring that an object becomes unusable after Dispose() is called. After an object has been disposed, methods other than Dispose() (which could potentially be called multiple times) should throw an ObjectDisposedException.

- DO implement IDisposable on types that own disposable fields (or properties) and dispose of those instances.

- DO invoke a base class’s Dispose() method from the Dispose(bool disposing) method if one exists.

### Enums

- CONSIDER adding new members to existing enums, but keep in mind the compatibility risk.

- AVOID creating enums that represent an “incomplete” set of values, such as product version numbers.

- AVOID creating “reserved for future use” values in an enum.

- AVOID enums that contain a single value.

- DO provide a value of 0 (none) for simple enums, knowing that 0 will be the default value when no explicit initialization is provided.

- DO use the FlagsAttribute to mark enums that contain flags.

- DO provide a None value equal to 0 for all flag enums.

- AVOID creating flag enums where the zero value has a meaning other than “no flags are set.”

- CONSIDER using the Enumerable.Empty<T>() method instead.

### Equality

- DO override the equality operators (Equals(), ==, and !=) and GetHashCode() on value types if equality is meaningful. (Also consider implementing the IEquatable<T> interface.)

- AVOID overriding the equality-related members on mutable reference types or if the implementation would be significantly slower with such overriding.

- DO implement all the equality-related methods when implementing IEquitable.

- AVOID using the equality comparison operator (==) from within the implementation of the == operator overload.

- DO ensure that custom comparison logic produces a consistent “total order.”

### Exceptions

- AVOID explicitly throwing exceptions from finally blocks. (Implicitly thrown exceptions resulting from method calls are acceptable.)

- DO favor try/finally and avoid using try/catch for cleanup code.

- DO throw exceptions that describe which exceptional circumstance occurred, and if possible, how to prevent it.

- AVOID general catch blocks and replace them with a catch of System.Exception.

- AVOID catching exceptions for which the appropriate action is unknown. It is better to let an exception go unhandled than to handle it incorrectly.

- AVOID catching and logging an exception before rethrowing it. Instead, allow the exception to escape until it can be handled appropriately.

- DO prefer using an empty throw when catching and rethrowing an exception so as to preserve the call stack.

- DO report execution failures by throwing exceptions rather than returning error codes.

- DO NOT have public members that return exceptions as return values or an out parameter. Throw exceptions to indicate errors; do not use them as return values to indicate errors.

- DO NOT use exceptions for handling normal, expected conditions; use them for exceptional, unexpected conditions.

- AVOID throwing exceptions from property getters.

- DO preserve the original property value if the property throws an exception.

- DO NOT throw exceptions from finalizer methods.

- DO throw an ArgumentException or one of its subtypes if bad arguments are passed to a member. Prefer the most derived exception type (e.g., ArgumentNullException), if applicable.

- DO NOT throw a System.SystemException or an exception type that derives from it.

- DO NOT throw a System.Exception, System.NullReferenceException, or System.ApplicationException.

- DO use nameof for the paramName argument passed into argument exception types that take such a parameter. Examples of such exceptions include ArgumentException, ArgumentOutOfRangeException, and ArgumentNullException.

- AVOID exception reporting or logging lower in the call stack.

- DO NOT over-catch. Exceptions should be allowed to propagate up the call stack unless it is clearly understood how to programmatically address those errors lower in the stack.

- CONSIDER catching a specific exception when you understand why it was thrown in a given context and can respond to the failure programmatically.

- AVOID catching System.Exception or System.SystemException except in top-level exception handlers that perform final cleanup operations before rethrowing the exception.

- DO use throw; rather than throw <exception object> inside a catch block.

- DO use exception filters to avoid rethrowing an exception from within a catch block.

- DO use caution when rethrowing different exceptions.

- AVOID throwing exceptions from exception filters.

- AVOID exception filters with logic that might implicitly change over time.

- AVOID deep exception hierarchies.

- DO NOT create a new exception type if the exception would not be handled differently than an existing CLR exception. Throw the existing framework exception instead.

- DO create a new exception type to communicate a unique program error that cannot be communicated using an existing CLR exception and that can be programmatically handled in a different way than any other existing CLR exception type.

- DO provide a parameterless constructor on all custom exception types. Also provide constructors that take a message and an inner exception.

- DO name an exception classes with the “Exception” suffix.

- DO make exceptions runtime-serializable.

- CONSIDER providing exception properties for programmatic access to extra information relevant to the exception.

- CONSIDER wrapping specific exceptions thrown from the lower layer in a more appropriate exception if the lower-layer exception does not make sense in the context of the higher-layer operation.

- DO specify the inner exception when wrapping exceptions.

- DO target developers as the audience for exceptions, identifying both the problem and the mechanism to resolve it, where possible.

- CONSIDER registering an unhandled exception event handler for debugging, logging, and emergency shutdown purposes.

- DO use the SetLastErrorAttribute on Windows to turn APIs that use SetLastError error codes into methods that throw Win32Exception.

### Fields

- CONSIDER initializing static fields inline rather than explicitly using static constructors or declaration assigned values.

- DO use public static readonly modified fields for predefined object instances prior to C# 6.0.

- AVOID changing a public readonly modified field in pre-C# 6.0 to a read-only automatically implemented property in C# 6.0 (or later) if version API compatibility is required.

- AVOID publicly exposed nested types. The only exception is if the declaration of such a type is unlikely or pertains to an advanced customization scenario.

- DO use PascalCasing and an “I” prefix for interface names.

### Flags

- CONSIDER using the default 32-bit integer type as the underlying type of an enum. Use a smaller type only if you must do so for interoperability; use a larger type only if you are creating a flags enum with more than 32 flags.

- CONSIDER providing special values for commonly used combinations of flags.

- DO use powers of 2 to ensure that all flag combinations are represented uniquely.

### Increment/decrement

- AVOID confusing usage of the increment and decrement operators.

- DO be cautious when porting code between C, C++, and C# that uses increment and decrement operators; C and C++ implementations need not follow the same rules as C#.

### Interfaces

- DO NOT add abstract members to an interface that has already been published.

- CONSIDER using extension methods or an additional interface in place of default interface members when adding methods to a published interface.

- DO use extension methods when the interface providing the polymorphic behavior is not under your control.

- DO use an additional interface when properties are necessary for extending polymorphic behavior for framework support prior to .NET Core 3.0.

- CONSIDER interfaces over abstract classes for polymorphic behavior starting with in C# 8.0/.NET Core 3.0 and abstract classes prior to C# 8.0.

- CONSIDER defining an interface if you need to support its functionality on types that already inherit from some other type.

- AVOID using “marker” interfaces with no members; use attributes instead.

### Methods

- AVOID frivolously defining extension methods, especially on types you don’t own.

- DO implement GetHashCode(), Equals(), the == operator, and the != operator together—not one of these without the other three.

- DO use the same algorithm when implementing Equals(), ==, and !=.

- DO NOT throw exceptions from implementations of GetHashCode(), Equals(), ==, and !=.

- DO implement finalizer methods only on objects with resources that don't have finalizers but still require cleanup.

- DO refactor a finalization method to call the same code as IDisposable, perhaps simply by calling the Dispose() method.

- CONSIDER terminating the process by calling System.Environment.FailFast() if the program encounters a scenario where it is unsafe to continue execution.

- AVOID misleading the caller with generic methods that are not as type-safe as they appear.

- AVOID the anonymous method syntax in new code; prefer the more compact lambda expression syntax.

- DO use the null-conditional operator prior to calling Invoke() starting in C# 6.0.

- DO use System.Linq.Enumerable.Any() rather than calling patents.Count() when checking whether there are more than zero items.

- DO use a collection’s Count property (if available) instead of calling the System.Linq.Enumerable.Count() method.

- DO NOT call an OrderBy() following a prior OrderBy() method call. Use ThenBy() to sequence items by more than one value.

- CONSIDER using the standard query operators (method call form) if the query involves operations that do not have a query expression syntax, such as Count(), TakeWhile(), or Distinct().

- DO create public managed wrappers around unmanaged methods that use the conventions of managed code, such as structured exception handling.

- DO declare extern methods as private or internal.

- DO provide public wrapper methods that use managed conventions such as structured exception handling, use of enums for special values, and so on.

- DO extend SafeHandle or implement IDisposable and create a finalizer to ensure that unmanaged resources can be cleaned up effectively.

### Miscellaneous

- DO favor consistency rather than variety within your code.

- DO rely on System.WriteLine() and System.Environment.NewLine rather than \n to accommodate Windows-specific operating system idiosyncrasies with the same code that runs on Linux and iOS.

- CONSIDER registering the finalization code with the AppDomain.ProcessExit to increase the probability that resource cleanup will execute before the process exits.

- AVOID referencing other objects that are not being finalized during finalization.

- AVOID capturing loop variables in anonymous functions.

- DO check that the value of a delegate is not null before invoking it.

- DO check that the value of a delegate is not null before invoking it (possibly by using the null-conditional operator in C# 6.0).

- DO pass null as the sender for static events.

- DO NOT pass null as the value of the eventArgs argument.

- DO use query expression syntax to make queries easier to read, particularly if they involve complex from, let, join, or group clauses.

- DO NOT make any unwarranted assumptions about the order in which elements of a collection will be enumerated. If the collection is not documented as enumerating its elements in a particular order, it is not guaranteed to produce elements in any particula

- DO NOT represent an empty collection with a null reference.

- CONSIDER using nonrecursive algorithms when iterating over potentially deep data structures.

- DO use delegate types that match the signature of the desired method when an unmanaged API requires a function pointer.

- DO use ref parameters rather than pointer types when possible.

### Parameters

- DO provide good defaults for all parameters where possible.

- DO provide simple method overloads that have a small number of required parameters.

- CONSIDER organizing overloads from the simplest to the most complex.

- DO provide constructor optional parameters and/or convenience constructor overloads that initialize properties with good defaults.

- AVOID providing constructor parameters to initialize attribute properties corresponding to the optional arguments (and, therefore, avoid overloading custom attribute constructors).

- DO simplify the wrapper methods by choosing default values for unnecessary parameters.

### Properties

- DO use properties for simple access to simple data with simple computations.

- DO favor automatically implemented properties over properties with simple backing fields when no additional logic is required.

- CONSIDER using the same casing on a property’s backing field as that used in the property, distinguishing the backing field with an “_” prefix. Do not, however, use two underscores; identifiers beginning with two underscores are reserved for the use of t

- DO favor automatically implemented properties over fields.

- DO favor automatically implemented properties over using fully expanded ones if there is no additional implementation logic.

- AVOID accessing the backing field of a property outside the property, even from within the containing class.

- DO use nameof(value) (which resolves to “value”) for the paramName argument when creating ArgumentException() or ArgumentNullException() type exceptions (value"" is the implicit name of the parameter on property setters).

- DO create read-only properties if the property value should not be changed.

- DO create read-only automatically implemented properties in C# 6.0 (or later) rather than read-only properties with a backing field if the property value should not be changed.

- DO apply appropriate accessibility modifiers on implementations of getters and setters on all properties.

- DO NOT provide set-only properties or properties with the setter having broader accessibility than the getter.

- DO provide sensible defaults for all properties, ensuring that defaults do not result in a security hole or significantly inefficient code.

- DO allow properties to be set in any order, even if this results in a temporarily invalid object state.

- DO allow properties to be set in any order, even if this results in a temporarily invalid object state.

- DO implement non-nullable read/write reference fully implemented properties with a nullable backing field, a null-forgiveness operator when returning the field from the getter, and non-null validation in the property setter.

- DO assign non-nullable reference-type properties before instantiation completes.

- DO implement non-nullable reference-type automatically implemented properties as read-only.

- DO use a nullable check for all reference-type properties and fields that are not initialized before instantiation completes.

- DO favor read-only automatically implemented properties in C# 6.0 (or later) over read-only fields.

### Strings

- DO favor composite formatting over use of the addition operator for concatenating strings when localization is a possibility.

### Structs

- DO ensure that the default value of a struct is valid; encapsulation cannot prevent obtaining the default “all zero” value of a struct.

- DO NOT define a struct unless it logically represents a single value, consumes 16 bytes or less of storage, is immutable, and is infrequently boxed.

### Synchronization

- DO declare a separate, read-only synchronization variable of type object for the synchronization target.

- AVOID using the MethodImplAttribute for synchronization.

- DO NOT request exclusive ownership of the same two or more synchronization targets in different orders.

- DO encapsulate mutable static data in public APIs with synchronization logic.

- AVOID synchronization on simple reading or writing of values no bigger than a native (pointer-size) integer, as such operations are automatically atomic.

### Tasks

- DO cancel unfinished tasks rather than allowing them to run during application shutdown.

- DO cancel unfinished tasks rather than allowing them to run during application shutdown.

- DO inform the task factory that a newly created task is likely to be long-running so that it can manage it appropriately.

- DO use TaskCreationOptions.LongRunning sparingly.

- DO use tasks and related APIs in favor of System.Theading classes such as Thread and ThreadPool.

### Threads

- DO NOT fall into the common error of believing that more threads always make code faster.

- DO carefully measure performance when attempting to speed up processor-bound problems through multithreading.

- DO NOT make an unwarranted assumption that any operation that is seemingly atomic in single-threaded code will be atomic in multithreaded code.

- DO NOT assume that all threads will observe all side effects of operations on shared memory in a consistent order.

- DO ensure that code that concurrently acquires multiple locks always acquires them in the same order.

- AVOID all race conditions—that is, conditions where program behavior depends on how the operating system chooses to schedule threads.

- AVOID writing programs that produce unhandled exceptions on any thread.

- AVOID calling Thread.Sleep() in production code.

- DO use parallel loops when the computations performed can be easily split up into many mutually independent processor-bound computations that can be executed in any order on any thread.

- AVOID locking on this, System.Type, or a string.

- DO ensure that code that concurrently holds multiple locks always acquires them in the same order.

### ToString()

- DO override ToString() whenever useful developer-oriented diagnostic strings can be returned.

- CONSIDER trying to keep the string returned from ToString() short.

- DO NOT return an empty string or null from ToString().

- DO NOT throw exceptions or make observable side effects (change the object state) from ToString().

- DO provide an overloaded ToString(string format) or implement IFormattable if the return value requires formatting or is culture-sensitive (e.g., DateTime).

- CONSIDER returning a unique string from ToString() so as to identify the object instance.

### Types

- AVOID using implicitly typed local variables unless the data type of the assigned value is obvious.

- AVOID binary floating-point types when exact decimal arithmetic is required; use the decimal floating-point type instead.

- AVOID using equality conditionals with binary floating-point types. Either subtract the two values and see if their difference is less than a tolerance, or  use the decimal type.

- AVOID creating value types that consume more than 16 bytes of memory.

- DO make value types immutable.

- AVOID mutable value types.

- AVOID implementing multiple constructions of the same generic interface in one type.

- CONSIDER whether the readability benefit of defining your own delegate type outweighs the convenience of using a predefined generic delegate type.

- CONSIDER omitting the types from lambda formal parameter lists when the types are obvious to the reader or when they are an insignificant detail.

- DO use System.EventArgs or a type that derives from System.EventArgs for a TEventArgs type.

- CONSIDER using a subclass of System.EventArgs as the event argument type (TEventArgs) unless you are sure the event will never need to carry any data.

- CONSIDER using System.EventHandler<T> instead of manually creating new delegate types for event handlers unless the parameter names of a custom type offer significant clarification.

## Layout



### Comments

- DO NOT use comments unless they describe something that is not obvious to someone other than the developer who wrote the code.

- DO favor writing clearer code over entering comments to clarify a complicated algorithm.

- DO provide XML comments on public APIs when they provide more context than the API signature alone. This includes member descriptions, parameter descriptions, and examples of calling the API.

### Files

- CONSIDER organizing the directory hierarchy for source code files to match the namespace hierarchy.

- DO NOT place more than one class in a single source file.

- CONSIDER creating a folder structure that matches the namespace hierarchy.

### Whitespace

- DO use parentheses to make code more readable, particularly if the operator precedence is not clear to the casual reader.

- AVOID omitting braces, except for the simplest of single-line if statements.

## Naming



### Abbreviations

- DO NOT use abbreviations or contractions within identifier names.

- DO NOT use any acronyms unless they are widely accepted, and even then use them consistently.

### Casing

- DO capitalize both characters in two-character acronyms, except for the first word of a camelCased identifier.

- DO capitalize only the first character in acronyms with three or more characters, except for the first word of a camelCased identifier.

- DO NOT capitalize any of the characters in acronyms at the beginning of a camelCased identifier.

- DO use PascalCasing for all class names.

- DO use camelCasing for local variable names.

- DO use uppercase literal suffixes (e.g., 1.618033988749895M).

- DO use camelCasing for variable declarations using tuple syntax. 

- CONSIDER using PascalCasing for all tuple item names.

- DO use PascalCasing for namespace names.

- DO use camelCasing for parameter names.

- AVOID naming fields with camelCase.

- DO favor prefixing Boolean properties with “Is,” “Can,” or “Has,” when that practice adds value.

- DO name properties with PascalCase.

### Methods

- DO give methods names that are verbs or verb phrases.

### Miscellaneous

- DO treat parameter names as part of the API, and avoid changing the names if version compatibility between APIs is important.

- DO name the source file with the name of the public type it contains.

- CONSIDER giving a property the same name as its type.

- DO use the same name for constructor parameters (camelCase) and properties (PascalCase) if the constructor parameters are used to simply set the property.

### Namespaces

- DO prefix namespace names with a company name to prevent namespaces from different companies having the same name.

- DO use a stable, version-independent product name at the second level of a namespace name.

### Types

- DO NOT define types without placing them into a namespace.

- DO choose meaningful names for type parameters and prefix the name with T.

- CONSIDER indicating a constraint in the name of a type parameter.

### Variables and fields

- DO favor clarity over brevity when naming identifiers.

- DO NOT use Hungarian notation (that is, do not encode the type of a variable in its name).

- DO name classes with nouns or noun phrases.

- DO use the C# keyword rather than the BCL name when specifying a data type (e.g., string rather than String).

- DO NOT use a constant for any value that can possibly change over time. The value of pi and the number of protons in an atom of gold are constants; the price of gold, the name of your company, and the version number of your program can change.

- DO name properties using a noun, noun phrase, or adjective.

- DO declare all instance fields as private (and expose them via a property).

- DO use constant fields for values that will never change.

- AVOID constant fields for values that will change over time.

- DO NOT include “sentinel” values (such as a value called Maximum); such values can be confusing to the user.

