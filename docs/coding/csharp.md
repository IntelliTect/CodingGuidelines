# C# Coding Standards

## Sections
- [Naming](#naming)
  - [Abbreviations](#abbreviations)
  - [Casing](#casing)
  - [Namespaces](#namespaces)
  - [Types](#types)
  - [Methods](#methods)
  - [Variables and Fields](#variables-and-fields)
  - [Miscellaneous](#miscellaneous)

- [Layout](#layout)
  - [Comments](#comments)
  - [Files](#files)
  - [Whitespace](#whitespace)

- [Coding](#coding)
  - [Arrays](#arrays)
  - [Assemblies](#assemblies)
  - [Branches](#branches)
  - [Classes](#classes)
  - [Conversions](#conversions)
  - [Dispose()](#dispose)
  - [Enums](#enums)
  - [Equality](#equality)
  - [Exceptions](#exceptions)
  - [Fields](#fields)
  - [Flags](#flags)
  - [Increment/decrement](#incrementdecrement)
  - [Interfaces](#interfaces)
  - [Methods](#methods-1)
  - [Parameters](#parameters)
  - [Properties](#properties)
  - [Strings](#strings)
  - [Structs](#structs)
  - [Synchronization](#synchronization)
  - [Tasks](#tasks)
  - [Threads](#threads)
  - [ToString()](#tostring)
  - [Types](#types)
  - [Miscellaneous](#miscellaneous)



## Naming

### Abbreviations
- :x: DO NOT: use abbreviations or contractions within identifier names.

### Casing
- :heavy_check_mark: DO: capitalize both characters in two-character acronyms, except for the first word of a camelCased identifier.
- :heavy_check_mark: DO: capitalize only the first character in acronyms with three or more characters, except for the first word of a camelCased identifier.
- :x: DO NOT: capitalize any of the characters in acronyms at the beginning of a camelCased identifier.
- :heavy_check_mark: DO: use PascalCasing for all class names.
- :heavy_check_mark: DO: use camelCasing for local variable names.
- :heavy_check_mark: DO: use uppercase literal suffixes (e.g., 1.618033988749895M).
- :heavy_check_mark: DO: use camelCasing for variable declarations using tuple syntax. 
- :grey_question: CONSIDER: using PascalCasing for all tuple item names.
- :heavy_check_mark: DO: use PascalCasing for namespace names.
- :heavy_check_mark: DO: use camelCasing for parameter names.
- :grey_question: CONSIDER: using the same casing on a property's backing field as that used in the property, distinguishing the backing field with an "_" prefix. Do not, however, use two underscores; identifiers beginning with two underscores are reserved for the use of t.
- :no_entry: AVOID: naming fields with camelCase.
- :heavy_check_mark: DO: name properties with PascalCase.
- :heavy_check_mark: DO: use PascalCasing and an "I" prefix for interface names.

### Namespaces
- :heavy_check_mark: DO: prefix namespace names with a company name to prevent namespaces from different companies having the same name. 
- :heavy_check_mark: DO: use a stable, version-independent product name at the second level of a namespace name. 

### Types
- :x: DO NOT: define types without placing them into a namespace.
- :heavy_check_mark: DO: choose meaningful names for type parameters and prefix the name with T.
- :grey_question: CONSIDER: indicating a constraint in the name of a type parameter.
- :no_entry: AVOID: shadowing a type parameter of an outer type with an indentically named type parameter.

### Methods
- :heavy_check_mark: DO: give methods names that are verbs or verb phrases.  

### Variables and fields
- :heavy_check_mark: DO: favor clarity over brevity when naming identifiers.
- :x: DO NOT: use Hungarian notation (that is, do not encode the type of a variable in its name).
- :heavy_check_mark: DO: name classes with nouns or noun phrases.
- :heavy_check_mark: DO: use the C# keyword rather than the BCL name when specifying a data type (e.g., string rather than String).
- :x: DO NOT: use a constant for any value that can possibly change over time. The value of pi and the number of protons in an atom of gold are constants; the price of gold, the name of your company, and the version number of your program can change.
- :heavy_check_mark: DO: name properties using a noun, noun phrase, or adjective.
- :heavy_check_mark: DO: favor prefixing Boolean properties with "Is," "Can," or "Has," when that practice adds value.
- :heavy_check_mark: DO: declare all instance fields as private (and expose them via a property).
- :heavy_check_mark: DO: use constant fields for values that will never change.
- :no_entry: AVOID: constant fields for values that will change over time.
- :x: DO NOT: include “sentinel” values (such as a value called Maximum); such values can be confusing to the user.

### Miscellaneous
- :heavy_check_mark: DO: treat parameter names as part of the API, and avoid changing the names if version compatibility between APIs is important.
- :heavy_check_mark: DO: name the source file with the name of the public type it contains.
- :grey_question: CONSIDER: giving a property the same name as its type.
- :heavy_check_mark: DO: use the same name for constructor parameters (camelCase) and properties (PascalCase) if the constructor parameters are used to simply set the property.
- :heavy_check_mark: DO: name an exception classes with the "Exception" suffix.

## Layout

### Files
- :grey_question: CONSIDER: organizing the directory hierarchy for source code files to match the namespace hierarchy.
- :x: DO NOT:  place more than one class in a single source file.
- :grey_question: CONSIDER: creating a folder structure that matches the namespace hierarchy.

### Comments
- :x: DO NOT: use comments unless they describe something that is not obvious to someone other than the developer who wrote the code.
- :heavy_check_mark: DO: favor writing clearer code over entering comments to clarify a complicated algorithm.
- :heavy_check_mark: DO: provide XML comments on public APIs when they provide more context than the API signature alone. This includes member descriptions, parameter descriptions, and examples of calling the API.

### Whitespace
- :heavy_check_mark: DO: use parentheses to make code more readable, particularly if the operator precedence is not clear to the casual reader.
- :no_entry: AVOID: omitting braces, except for the simplest of single-line if statements.

## Coding

### Arrays
- :grey_question: CONSIDER: checking for null before accessing an array rather than assuming there is an array instance.
- :grey_question: CONSIDER: checking the array length before indexing into an array rather than assuming the length.
- :grey_question: CONSIDER: using the index from end operator (^) rather than Length - 1 with C# 8.0 or higher.
- :heavy_check_mark: DO: use parameter arrays when a method can handle any number—including zero—of additional arguments.
- :no_entry: AVOID: unsafe array covariance. Instead, CONSIDER converting the array to the read-only interface IEnumerable<T>, which can be safely converted via covariant conversions.

### Assemblies
- :heavy_check_mark: DO: apply AssemblyVersionAttribute to assemblies with public types.
- :grey_question: CONSIDER: applying AssemblyFileVersionAttribute and AssemblyCopyrightAttribute to provide additional information about the assembly.
- :heavy_check_mark: DO: apply the following information assembly attributes: System.Reflection.AssemblyCompanyAttribute, System.Reflection.AssemblyCopyrightAttribute, System.Reflection.AssemblyDescriptionAttribute, and System.Reflection.AssemblyProductAttribute.
- :heavy_check_mark: DO: name custom attribute classes with the suffix Attribute.
- :heavy_check_mark: DO: provide get-only properties (without public setters) on attributes with required property values.
- :heavy_check_mark: DO: provide constructor parameters to initialize properties on attributes with required properties. Each parameter should have the same name (albeit with different casing) as the corresponding property.
- :heavy_check_mark: DO: apply the AttributeUsageAttribute class to custom attributes.
	
### Branches
- :grey_question: CONSIDER: using an if-else statement instead of an overly complicated conditional expression.
- :grey_question: CONSIDER: refactoring the method to make the control flow easier to understand if you find yourself writing for loops with complex conditionals and multiple loop variables.
- :heavy_check_mark: DO: use the for loop when the number of loop iterations is known in advance and the “counter” that gives the number of iterations executed is needed in the loop.
- :heavy_check_mark: DO: use the while loop when the number of loop iterations is not known in advance and a counter is not needed.
- :x: DO NOT: use continue as the jump statement that exits a switch section. This is legal when the switch is inside a loop, but it is easy to become confused about the meaning of break in a later switch section.
- :no_entry: AVOID: using goto.

### Classes
- :heavy_check_mark: DO: implement IDisposable to support possible deterministic finalization on classes with finalizers.
- :heavy_check_mark: DO: place multiple generic semantically equivalent classes into a single file if they differ only by the number of generic parameters.
- :heavy_check_mark: DO: pass the instance of the class as the value of the sender for nonstatic events.
- :x: DO NOT: unnecessarily replicate existing managed classes that already perform the function of the unmanaged API.

### Conversions
- :no_entry: AVOID: direct enum/string conversions where the string must be localized into the user’s language.
- :x: DO NOT: provide an implicit conversion operator if the conversion is lossy.
- :x: DO NOT: throw exceptions from implicit conversions.

### Dispose()
- :heavy_check_mark: DO: implement the dispose pattern on objects with resources that are scarce or expensive.
- :heavy_check_mark: DO: unregister any AppDomain.ProcessExit events during dispose.
- :heavy_check_mark: DO: call System.GC.SuppressFinalize() from Dispose() to avoid repeating resource cleanup and delaying garbage collection on an object.
- :heavy_check_mark: DO: ensure that Dispose() is idempotent (it should be possible to call Dispose() multiple times).
- :heavy_check_mark: DO: keep Dispose() simple, focusing on the resource cleanup required by finalization.
- :no_entry: AVOID: calling Dispose() on owned objects that have a finalizer. Instead, rely on the finalization queue to clean up the instance.
- :heavy_check_mark: DO: invoke a base class’s Dispose() method when overriding Dispose().
- :grey_question: CONSIDER: ensuring that an object becomes unusable after Dispose() is called. After an object has been disposed, methods other than Dispose() (which could potentially be called multiple times) should throw an ObjectDisposedException.
- :heavy_check_mark: DO: implement IDisposable on types that own disposable fields (or properties) and dispose of those instances.
- :heavy_check_mark: DO: invoke a base class’s Dispose() method from the Dispose(bool disposing) method if one exists.

### Enums
- :grey_question: CONSIDER: adding new members to existing enums, but keep in mind the compatibility risk.
- :no_entry: AVOID: creating enums that represent an "incomplete" set of values, such as product version numbers.
- :no_entry: AVOID: creating "reserved for future use" values in an enum.
- :no_entry: AVOID: enums that contain a single value.
- :heavy_check_mark: DO: provide a value of 0 (none) for simple enums, knowing that 0 will be the default value when no explicit initialization is provided.
- :heavy_check_mark: DO: use the FlagsAttribute to mark enums that contain flags.
- :heavy_check_mark: DO: provide a None value equal to 0 for all flag enums.
- :no_entry: AVOID: creating flag enums where the zero value has a meaning other than "no flags are set."
- :grey_question: CONSIDER: using the Enumerable.Empty<T>() method instead.

### Equality
- :no_entry: AVOID: using equality conditionals with binary floating-point types. Either subtract the two values and see if their difference is less than a tolerance, or use the decimal type.
- :heavy_check_mark: DO: override the equality operators (Equals(), ==, and !=) and GetHashCode() on value types if equality is meaningful. (Also consider implementing the IEquatable<T> interface.)
- :no_entry: AVOID: overriding the equality-related members on mutable reference types or if the implementation would be significantly slower with such overriding.
- :heavy_check_mark: DO: implement all the equality-related methods when implementing IEquitable.
- :no_entry: AVOID: using the equality comparison operator (==) from within the implementation of the == operator overload.
- :heavy_check_mark: DO: ensure that custom comparison logic produces a consistent “total order.”

### Exceptions
- :no_entry: AVOID: explicitly throwing exceptions from finally blocks. (Implicitly thrown exceptions resulting from method calls are acceptable.)
- :heavy_check_mark: DO: favor try/finally and avoid using try/catch for cleanup code.
- :heavy_check_mark: DO: throw exceptions that describe which exceptional circumstance occurred, and if possible, how to prevent it.
- :no_entry: AVOID: general catch blocks and replace them with a catch of System.Exception.
- :no_entry: AVOID: catching exceptions for which the appropriate action is unknown. It is better to let an exception go unhandled than to handle it incorrectly.
- :no_entry: AVOID: catching and logging an exception before rethrowing it. Instead, allow the exception to escape until it can be handled appropriately.
- :heavy_check_mark: DO: prefer using an empty throw when catching and rethrowing an exception so as to preserve the call stack.
- :heavy_check_mark: DO: report execution failures by throwing exceptions rather than returning error codes.
- :x: DO NOT: have public members that return exceptions as return values or an out parameter. Throw exceptions to indicate errors; do not use them as return values to indicate errors.
- :x: DO NOT: use exceptions for handling normal, expected conditions; use them for exceptional, unexpected conditions.
- :no_entry: AVOID: throwing exceptions from property getters.
- :heavy_check_mark: DO: preserve the original property value if the property throws an exception.
- :heavy_check_mark: DO: use nameof(value) (which resolves to "value") for the paramName argument when creating ArgumentException() or ArgumentNullException() type exceptions (value" is the implicit name of the parameter on property setters)."
- :x: DO NOT: throw exceptions from finalizer methods.
- :heavy_check_mark: DO: throw an ArgumentException or one of its subtypes if bad arguments are passed to a member. Prefer the most derived exception type (e.g., ArgumentNullException), if applicable.
- :x: DO NOT: throw a System.SystemException or an exception type that derives from it.
- :x: DO NOT: throw a System.Exception, System.NullReferenceException, or System.ApplicationException.
- :heavy_check_mark: DO: use nameof for the paramName argument passed into argument exception types that take such a parameter. Examples of such exceptions include ArgumentException, ArgumentOutOfRangeException, and ArgumentNullException.
- :no_entry: AVOID: exception reporting or logging lower in the call stack.
- :x: DO NOT: over-catch. Exceptions should be allowed to propagate up the call stack unless it is clearly understood how to programmatically address those errors lower in the stack.
- :grey_question: CONSIDER: catching a specific exception when you understand why it was thrown in a given context and can respond to the failure programmatically.
- :no_entry: AVOID: catching System.Exception or System.SystemException except in top-level exception handlers that perform final cleanup operations before rethrowing the exception.
- :heavy_check_mark: DO: use throw; rather than throw exception object inside a catch block.
- :heavy_check_mark: DO: use exception filters to avoid rethrowing an exception from within a catch block.
- :heavy_check_mark: DO: use caution when rethrowing different exceptions.
- :no_entry: AVOID:  throwing exceptions from exception filters.
- :no_entry: AVOID:  exception filters with logic that might implicitly change over time.
- :no_entry: AVOID:  deep exception hierarchies.
- :x: DO NOT: create a new exception type if the exception would not be handled differently than an existing CLR exception. Throw the existing framework exception instead.
- :heavy_check_mark: DO: create a new exception type to communicate a unique program error that cannot be communicated using an existing CLR exception and that can be programmatically handled in a different way than any other existing CLR exception type.
- :heavy_check_mark: DO: provide a parameterless constructor on all custom exception types. Also provide constructors that take a message and an inner exception.
- :heavy_check_mark: DO: make exceptions runtime-serializable.
- :grey_question: CONSIDER: providing exception properties for programmatic access to extra information relevant to the exception.
- :grey_question: CONSIDER: wrapping specific exceptions thrown from the lower layer in a more appropriate exception if the lower-layer exception does not make sense in the context of the higher-layer operation.
- :heavy_check_mark: DO: specify the inner exception when wrapping exceptions.
- :heavy_check_mark: DO: target developers as the audience for exceptions, identifying both the problem and the mechanism to resolve it, where possible.
- :grey_question: CONSIDER: registering an unhandled exception event handler for debugging, logging, and emergency shutdown purposes.
- :heavy_check_mark: DO: use the SetLastErrorAttribute on Windows to turn APIs that use SetLastError error codes into methods that throw Win32Exception.

### Fields
- :grey_question: CONSIDER:  initializing static fields inline rather than explicitly using static constructors or declaration assigned values.
- :heavy_check_mark: DO: use public static readonly modified fields for predefined object instances prior to C# 6.0.
- :no_entry: AVOID: changing a public readonly modified field in pre-C# 6.0 to a read-only automatically implemented property in C# 6.0 (or later) if version API compatibility is required.
- :no_entry: AVOID: publicly exposed nested types. The only exception is if the declaration of such a type is unlikely or pertains to an advanced customization scenario.

### Flags
- :grey_question: CONSIDER: using the default 32-bit integer type as the underlying type of an enum. Use a smaller type only if you must do so for interoperability; use a larger type only if you are creating a flags enum with more than 32 flags.
- :grey_question: CONSIDER: providing special values for commonly used combinations of flags.
- :heavy_check_mark: DO: use powers of 2 to ensure that all flag combinations are represented uniquely.

### Increment/decrement
- :no_entry: AVOID: confusing usage of the increment and decrement operators.
- :heavy_check_mark: DO: be cautious when porting code between C, C++, and C# that uses increment and decrement operators; C and C++ implementations need not follow the same rules as C#.

### Interfaces
- :x: DO NOT: add abstract members to an interface that has already been published.
- :grey_question: CONSIDER: using extension methods or an additional interface in place of default interface members when adding methods to a published interface.
- :heavy_check_mark: DO: use extension methods when the interface providing the polymorphic behavior is not under your control.
- :heavy_check_mark: DO: use an additional interface when properties are necessary for extending polymorphic behavior for framework support prior to .NET Core 3.0.
- :grey_question: CONSIDER: interfaces over abstract classes for polymorphic behavior starting with in C# 8.0/.NET Core 3.0 and abstract classes prior to C# 8.0.
- :grey_question: CONSIDER: defining an interface if you need to support its functionality on types that already inherit from some other type.
- :no_entry: AVOID: using “marker” interfaces with no members; use attributes instead.

### Methods
- :no_entry: AVOID: frivolously defining extension methods, especially on types you don’t own.
- :heavy_check_mark: DO: implement GetHashCode(), Equals(), the == operator, and the != operator together—not one of these without the other three.
- :heavy_check_mark: DO: use the same algorithm when implementing Equals(), ==, and !=.
- :x: DO NOT: throw exceptions from implementations of GetHashCode(), Equals(), ==, and !=.
- :heavy_check_mark: DO: implement finalizer methods only on objects with resources that don't have finalizers but still require cleanup.
- :heavy_check_mark: DO: refactor a finalization method to call the same code as IDisposable, perhaps simply by calling the Dispose() method.
- :grey_question: CONSIDER: terminating the process by calling System.Environment.FailFast() if the program encounters a scenario where it is unsafe to continue execution.
- :no_entry: AVOID: misleading the caller with generic methods that are not as type-safe as they appear.
- :no_entry: AVOID: the anonymous method syntax in new code; prefer the more compact lambda expression syntax.
- :heavy_check_mark: DO: use the null-conditional operator prior to calling Invoke() starting in C# 6.0.
- :heavy_check_mark: DO: use System.Linq.Enumerable.Any() rather than calling patents.Count() when checking whether there are more than zero items.
- :heavy_check_mark: DO: use a collection’s Count property (if available) instead of calling the System.Linq.Enumerable.Count() method.
- :x: DO NOT: call an OrderBy() following a prior OrderBy() method call. Use ThenBy() to sequence items by more than one value.
- :grey_question: CONSIDER: using the standard query operators (method call form) if the query involves operations that do not have a query expression syntax, such as Count(), TakeWhile(), or Distinct().
- :heavy_check_mark: DO: create public managed wrappers around unmanaged methods that use the conventions of managed code, such as structured exception handling.
- :heavy_check_mark: DO: declare extern methods as private or internal.
- :heavy_check_mark: DO: provide public wrapper methods that use managed conventions such as structured exception handling, use of enums for special values, and so on.
- :heavy_check_mark: DO: extend SafeHandle or implement IDisposable and create a finalizer to ensure that unmanaged resources can be cleaned up effectively.

### Parameters
- :heavy_check_mark: DO: provide good defaults for all parameters where possible.
- :heavy_check_mark: DO: provide simple method overloads that have a small number of required parameters.
- :grey_question: CONSIDER: organizing overloads from the simplest to the most complex.
- :heavy_check_mark: DO: provide constructor optional parameters and/or convenience constructor overloads that initialize properties with good defaults.
- :heavy_check_mark: DO: simplify the wrapper methods by choosing default values for unnecessary parameters.
- :no_entry: AVOID: providing constructor parameters to initialize attribute properties corresponding to the optional arguments (and, therefore, avoid overloading custom attribute constructors).


### Properties
- :heavy_check_mark: DO: use properties for simple access to simple data with simple computations.
- :heavy_check_mark: DO: favor automatically implemented properties over properties with simple backing fields when no additional logic is required.
- :heavy_check_mark: DO: favor automatically implemented properties over fields.
- :heavy_check_mark: DO: favor automatically implemented properties over using fully expanded ones if there is no additional implementation logic.
- :no_entry: AVOID: accessing the backing field of a property outside the property, even from within the containing class.
- :heavy_check_mark: DO: create read-only properties if the property value should not be changed.
- :heavy_check_mark: DO: create read-only automatically implemented properties in C# 6.0 (or later) rather than read-only properties with a backing field if the property value should not be changed.
- :heavy_check_mark: DO: apply appropriate accessibility modifiers on implementations of getters and setters on all properties.
- :x: DO NOT: provide set-only properties or properties with the setter having broader accessibility than the getter.
- :heavy_check_mark: DO: provide sensible defaults for all properties, ensuring that defaults do not result in a security hole or significantly inefficient code.
- :heavy_check_mark: DO: allow properties to be set in any order, even if this results in a temporarily invalid object state.
- :heavy_check_mark: DO: implement non-nullable read/write reference fully implemented properties with a nullable backing field, a null-forgiveness operator when returning the field from the getter, and non-null validation in the property setter.
- :heavy_check_mark: DO: assign non-nullable reference-type properties before instantiation completes.
- :heavy_check_mark: DO: implement non-nullable reference-type automatically implemented properties as read-only.
- :heavy_check_mark: DO: use a nullable check for all reference-type properties and fields that are not initialized before instantiation completes.
- :heavy_check_mark: DO: favor read-only automatically implemented properties in C# 6.0 (or later) over read-only fields.

### Strings
- :heavy_check_mark: DO: favor composite formatting over use of the addition operator for concatenating strings when localization is a possibility.

### Structs
- :heavy_check_mark: DO: ensure that the default value of a struct is valid; encapsulation cannot prevent obtaining the default “all zero” value of a struct.
- :x: DO NOT: define a struct unless it logically represents a single value, consumes 16 bytes or less of storage, is immutable, and is infrequently boxed.

### Synchronization
- :heavy_check_mark: DO: declare a separate, read-only synchronization variable of type object for the synchronization target.
- :no_entry: AVOID: using the MethodImplAttribute for synchronization.
- :x: DO NOT: request exclusive ownership of the same two or more synchronization targets in different orders.
- :heavy_check_mark: DO: encapsulate mutable static data in public APIs with synchronization logic.
- :no_entry: AVOID: synchronization on simple reading or writing of values no bigger than a native (pointer-size) integer, as such operations are automatically atomic.

### Tasks
- :heavy_check_mark: DO: cancel unfinished tasks rather than allowing them to run during application shutdown.
- :heavy_check_mark: DO: inform the task factory that a newly created task is likely to be long-running so that it can manage it appropriately.
- :heavy_check_mark: DO: use TaskCreationOptions.LongRunning sparingly.
- :heavy_check_mark: DO: use tasks and related APIs in favor of System.Theading classes such as Thread and ThreadPool.

### Threads
- :x: DO NOT: fall into the common error of believing that more threads always make code faster.
- :heavy_check_mark: DO: carefully measure performance when attempting to speed up processor-bound problems through multithreading.
- :x: DO NOT: make an unwarranted assumption that any operation that is seemingly atomic in single-threaded code will be atomic in multithreaded code.
- :x: DO NOT: assume that all threads will observe all side effects of operations on shared memory in a consistent order.
- :heavy_check_mark: DO: ensure that code that concurrently acquires multiple locks always acquires them in the same order.
- :no_entry: AVOID: all race conditions—that is, conditions where program behavior depends on how the operating system chooses to schedule threads.
- :no_entry: AVOID: writing programs that produce unhandled exceptions on any thread.
- :no_entry: AVOID: calling Thread.Sleep() in production code.
- :heavy_check_mark: DO: use parallel loops when the computations performed can be easily split up into many mutually independent processor-bound computations that can be executed in any order on any thread.
- :no_entry: AVOID: locking on this, System.Type, or a string.
- :heavy_check_mark: DO: ensure that code that concurrently holds multiple locks always acquires them in the same order.

### ToString()
- :heavy_check_mark: DO: override ToString() whenever useful developer-oriented diagnostic strings can be returned.
- :grey_question: CONSIDER: trying to keep the string returned from ToString() short.
- :x: DO NOT: return an empty string or null from ToString().
- :x: DO NOT: throw exceptions or make observable side effects (change the object state) from ToString().
- :heavy_check_mark: DO provide an overloaded ToString(string format) or implement IFormattable if the return value requires formatting or is culture-sensitive (e.g., DateTime).
- :grey_question: CONSIDER: returning a unique string from ToString() so as to identify the object instance.

### Types
- :no_entry: AVOID: using implicitly typed local variables unless the data type of the assigned value is obvious.
- :no_entry: AVOID: binary floating-point types when exact decimal arithmetic is required; use the decimal floating-point type instead.
- :no_entry: AVOID: creating value types that consume more than 16 bytes of memory.
- :heavy_check_mark: DO: make value types immutable.
- :no_entry: AVOID: mutable value types.
- :no_entry: AVOID: implementing multiple constructions of the same generic interface in one type.
- :grey_question: CONSIDER: whether the readability benefit of defining your own delegate type outweighs the convenience of using a predefined generic delegate type.
- :grey_question: CONSIDER: omitting the types from lambda formal parameter lists when the types are obvious to the reader or when they are an insignificant detail.
- :heavy_check_mark: DO: use System.EventArgs or a type that derives from System.EventArgs for a TEventArgs type.
- :grey_question: CONSIDER: using a subclass of System.EventArgs as the event argument type (TEventArgs) unless you are sure the event will never need to carry any data.
- :grey_question: CONSIDER: using System.EventHandler<T> instead of manually creating new delegate types for event handlers unless the parameter names of a custom type offer significant clarification.

### Miscellaneous
- :heavy_check_mark: DO: favor consistency rather than variety within your code.
- :heavy_check_mark: DO: rely on System.WriteLine() and System.Environment.NewLine rather than \n to accommodate Windows-specific operating system idiosyncrasies with the same code that runs on Linux and iOS.
- :grey_question: CONSIDER: registering the finalization code with the AppDomain.ProcessExit to increase the probability that resource cleanup will execute before the process exits.
- :no_entry: AVOID: referencing other objects that are not being finalized during finalization.
- :no_entry: AVOID: capturing loop variables in anonymous functions.
- :heavy_check_mark: DO: check that the value of a delegate is not null before invoking it (possibly by using the null-conditional operator in C# 6.0).
- :heavy_check_mark: DO: pass null as the sender for static events.
- :x: DO NOT: pass null as the value of the eventArgs argument.
- :heavy_check_mark: DO: use query expression syntax to make queries easier to read, particularly if they involve complex from, let, join, or group clauses.
- :x: DO NOT: make any unwarranted assumptions about the order in which elements of a collection will be enumerated. If the collection is not documented as enumerating its elements in a particular order, it is not guaranteed to produce elements in any particula
- :x: DO NOT: represent an empty collection with a null reference.
- :grey_question: CONSIDER: using nonrecursive algorithms when iterating over potentially deep data structures.
- :heavy_check_mark: DO: use delegate types that match the signature of the desired method when an unmanaged API requires a function pointer.
- :heavy_check_mark: DO: use ref parameters rather than pointer types when possible.
