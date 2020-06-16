# C# Coding Standards

## Naming

### Casing
- :heavy_check_mark: DO: Use ‘camelCasing’ for variables and method arguments
- :heavy_check_mark: DO: Use ‘PascalCasing’ for everything else including namespaces, interfaces, types, methods, properties, events, etc.

### Namespaces
- :heavy_check_mark: DO: Use meaningful namespaces that relate to the company, product, and layer. Use Microsoft/System namespace naming convention where appropriate, while replacing Microsoft/System with the appropriate starting namespace.
- :heavy_check_mark: DO: Have the namespace follow the name of the directory it is contained within.

### Types
- :heavy_check_mark: DO: Use names that describe the intent of the class, interface, or struct.
- :heavy_check_mark: DO: Prefix interface names with “I”. Do not prefix any other type names.
- :heavy_check_mark: DO: Type names should be as descriptive as possible. To distinguish the parameters as being a type parameter, the name should include a T prefix.
- :heavy_check_mark: DO: Non descriptive type parameters are allowed when the descriptive type parameter name would not add any value. E.g. T in the Stack<T> class is appropriate since the indication that T is a type parameters is sufficiently descriptive; the stack works for any type T.
- :heavy_check_mark: CONSIDER: If a type parameter has a constraint, consider using the constraint in the type name.  e.g a type parameter with the IComponent constraint would be called TComponent.
- :heavy_check_mark: DO: Suffix custom attribute classes with Attribute.
- :heavy_check_mark: DO: Suffix custom exception types with Exception.
- :heavy_check_mark: DO: Enums type names must be singular unless flags.
- **TODO**: Do not begin groups of enumerated types with a common prefix e.g. Color.ColorRed  should just be Color.Red.

### Methods
- :heavy_check_mark: CONSIDER: Use verb-noun method names that perform an operation on an object. This Indicates what is being performed and what object is being acted on.
- :heavy_check_mark: DO: Indicate the “what” and not the “how”. When the name includes “how” the implementation is exposed.  Changing the implementation or semantics of the object, method, or data implies changing the name.

### Variables and fields
- :heavy_check_mark: DO: Prefix fields (if they are indeed needed) with underscore and then use ‘PascalCasing’ and wrap them with a property (even if the end result is private).
Exception: Unless you need to pass a member value by ref, and it cannot be a local variable. Example: Maintaining an internal reference count using Interlocked.
This largely removes the need for readonly fields in favor of properties.
- :no_entry: AVOID: Hungarian notation
- :no_entry: AVOID: incorporating the data type into the name.
- :heavy_check_mark: DO: Use C# alias types rather than their System namespace counterparts (ie. ‘int’ instead of ‘System.Int32’)
- :heavy_check_mark: DO: Use plural names for collections
- :heavy_check_mark: DO: Make boolean names should contain Is/Are/Can/Has which implies Yes/No or True/False values. 
- :no_entry: AVOID: Double negatives when referring to booleans.
- :no_entry: AVOID: var in cases where the the type of var is not obvious. Common exceptions would be constructors and generic method calls where it is clear the generic type is the return value.

### Abbreviations
- :no_entry: AVOID: Abbreviations unless project approved or generally accepted and documented.
- :heavy_check_mark: DO: If an object property name fully or partially uses a two letter abbreviation, it will be all uppercase; if it is three letters it will be mixed.

### Tests
- :heavy_check_mark: DO: Test namespaces follow the target class names space with the addition of “.Tests” suffix. (E.g. Fezzik.Tests)
- :heavy_check_mark: DO: Mirror test file location with the corresponding file being tested.
- :heavy_check_mark: DO: Name test class using the target class name with the “Tests” suffix.  (E.g. Fezzik.Tests.SwordPlayTests)
- :heavy_check_mark: DO: Use TDD and/or strive for 100% code coverage unless you have a reason not to – too much effort.  If missing code coverage, provide documentation/comments as to why.
- :heavy_check_mark: DO: Use [ExcludeFromCodeCoverage] attribute for generated or other code to keep it from negatively impacting the coverage rate. (see http://msdn.microsoft.com/en-us/library/dd984116(v=vs.100).aspx)
- :heavy_check_mark: DO: Use unit test names that identify what is being tested
- :heavy_check_mark: CONSIDER: Naming test methods with pattern <MethodName>_<StateUnderTest>_<ExpectedBehavior>

### Miscellaneous
- :x: DO NOT: Use underscores to break up words in an identifier. Test methods, UI generated event handlers and references to Resource files are the exception.
- :heavy_check_mark: DO: Use American-style spelling.

## Layout

### Files
- :heavy_check_mark: DO: Put a single class per file unless varying arity, code is generated, creating nested classes, or there is a needed  “buddy class” to add DataAnnotations.
- :heavy_check_mark: DO: Give source files the name of the class in the file
- :heavy_check_mark: DO: Group members into sections based on type (Constants, Constructors, Fields and Properties, Methods, Interface implementations). Static members should be grouped with their respective instance members. (e.g. static methods grouped with instance methods)
- :no_entry: AVOID: Lines longer than 150 characters
- :heavy_check_mark: CONSIDER: Group members in a file by their member type. For examples, keep all properties together, all methods together, constructors/finalizers together.

### Properties
- :heavy_check_mark: DO: Use automatically implemented properties rather than backing fields unless backing field is required (validation or INotifyPropertyChanged implementations are examples where a backing field may be required).
- :x: DO NOT: Access fields outside of their property wrapper or the constructor.
Exception: MVVM situations where the property setter is being used for input validation.
- :heavy_check_mark: DO: Use read-only properties (C# 6.0) in favor of read-only fields

### Regions
- :no_entry: AVOID: Using regions that do not add value.
- :heavy_check_mark: CONSIDER: Using regions around interface implementations
- :heavy_check_mark: DO: Provide a name for all regions (specify text after #region)

### Attributes
- :heavy_check_mark: DO: Place class member attribute decorations onto separate lines and encapsulated in its own square brackets.
- :heavy_check_mark: CONSIDER: Placing attribute reflection code within the attribute – generally as static methods.

### Comments
- :x: DO NOT: duplicate source control metadata in comments. Omit things “Created by”, “Last edited on”, etc. 

### Whitespace
- :x: DO NOT: Mix tabs and spaces.
- :x: DO NOT: Mix indentation sizes.

## Coding

### Strings
- :heavy_check_mark: CONSIDER: Using string interpolation over string.Format when possible unless localization is required. 
DO NOT: hardcode locale specific strings. Put into resource files, or config files, or into constants so that they are easy to change and find. 
Exception: Messages used in Exceptions should be targeted at developers and, as such, do not require localization.  
See http://msdn.microsoft.com/en-us/library/dd465121.aspx for more detail around comparison and sorting of strings.

### Localization
- :heavy_check_mark: DO: Store all dates in UTC. Convert to locale specific values at the client.
- :heavy_check_mark: DO: Specify CultureInfo when calling Parse/TryParse when parsing strings that may be localized. 

### Methods
- :x: DO NOT: Return null from a method with a return type of IEnumerable<T>.
- :no_entry: AVOID: Null checks on IEnumerable<T> return values.
- :x: DO NOT: Return null from a method with a return type Task<T>.

### Exceptions
- :heavy_check_mark: DO: Throw the most specific (most derived) exception that makes sense.
- :x: DO NOT: Swallow errors by catching nonspecific exceptions such as System.Exception and System.SystemException unless the exception is re-thrown
- :heavy_check_mark: DO: Catch only exceptions you are explicitly handling. Use exception filters to further limit which exceptions you handle.
- :x: DO NOT: Over catch exceptions. Exceptions should often be allowed to propagate up the call stack. Use exception filters to avoid catching exceptions that you cannot handle.
- :heavy_check_mark: DO: In a catch statement that throws an exception, throw the original (using throw;)  or wrapped exception if the exception type is not appropriate to re-throw  (throw new Exception(exception)). This maintains the stack location of the original error.
- :no_entry: AVOID: Defining custom exception classes.
- :no_entry: AVOID Returning error codes.  Exceptions are the primary means of reporting errors in frameworks.
- :heavy_check_mark: CONSIDER: terminating the process by calling System.Environment.FailFast if code encounters a situation where it is unsafe for further execution.
- :no_entry: AVOID Creating APIs that when called can result in a system failure.  If such a failure can occur, call Environment.FailFast when the system failure occurs instead.
- :x: DO NOT: Use exceptions for normal flow of control.
- :x: DO NOT: Have public members that can either throw or not based on some option.
- :x: DO NOT: Have public members that return exceptions as return value or an out parameter.
- :no_entry: AVOID Explicitly throwing exceptions from finally blocks. (Implicitly thrown exceptions resulting from calling methods that throw are acceptable.)
- :x: DO NOT: Create or throw custom exception types that callers are not expected to handle. Throw one of the existing Framework exceptions instead. 
- :heavy_check_mark: CONSIDER: Creating and throwing custom exceptions if you have a unique program error that cannot be communicated using an existing framework exceptions
- :x: DO NOT: Create a new exception type if the exception would not be handled differently than an existing Framework exception – throw the existing framework exception instead.
- :heavy_check_mark: DO: Use try-finally for clean up work and avoid using try-catch.
- :heavy_check_mark: CONSIDER: Wrapping specific exceptions thrown from the lower layer in a more appropriate exception if the lower-layer exception does not make sense in the context of the higher-layer operation.
- :no_entry: AVOID Catching and wrapping in nonspecific exceptions.
- :x: DO NOT: Throw System.Exception, System.SystemException, or System.NullReferenceException. Throw the existing framework exception instead, such as System.InvalidOperationException. 
AVOID: Catching System.Exception or System.SystemException except in top-level exception handlers.
- :x: DO NOT: Throw or derive from System.ApplicationException.
- :heavy_check_mark: DO: Throw an InvalidOperationException if the object is not in a valid state.
- :heavy_check_mark: DO: Throw ArgumentException or one of its subtypes (ArgumentNullException) if bad arguments are passed to a member.  Prefer the most derived exception type, if applicable. Set the parameter name of the bad argument that was passed. 
- :x: DO NOT: Explicitly throw ComException, ExecutionEngineException, or SEHException.
- :no_entry: AVOID Deep exception hierarchies.
- :heavy_check_mark: DO: Make exceptions runtime serializable.
- :heavy_check_mark: DO: Provide exception constructors for default, string message, and Exception inner.
- :heavy_check_mark: CONSIDER: Providing exception properties for programmatic access to extra information relevant to the exception.
- :heavy_check_mark: CONSIDER: Providing a tester method for members that might throw exceptions in common scenarios to avoid performance problems (Tester-Doer Pattern).  E.g. TryParse().
- :heavy_check_mark: DO: Provide a Try-X method for every throwing X method.

### Miscellaneous
- :x: DO NOT: Initialize to default values. E.g. int I = 0; It makes code clearer when only ‘non default’ values are initialized. 
- :x: DO NOT: Use the ‘this’ modifier unless it is required (ie. passing this or removing an ambiguity or obfuscation). 
- :heavy_check_mark: CONSIDER: Using static when the static type name does not add value to the code. Such as “using static System.Console” inside of a console application.
