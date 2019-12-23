# C# Coding Standards

## Naming

### Casing
- DO: Use ‘camelCasing’ for variables and method arguments
- DO: Use ‘PascalCasing’ for everything else including namespaces, interfaces, types, methods, properties, events, etc.

### Namespaces
- DO: Use meaningful namespaces that relate to the company, product, and layer. Use Microsoft/System namespace naming convention where appropriate, while replacing Microsoft/System with the appropriate starting namespace.
- DO: Have the namespace follow the name of the directory it is contained within.

### Types
- DO: Use names that describe the intent of the class, interface, or struct.
- DO: Prefix interface names with “I”. Do not prefix any other type names.
- DO: Type names should be as descriptive as possible. To distinguish the parameters as being a type parameter, the name should include a T prefix.
- DO: Non descriptive type parameters are allowed when the descriptive type parameter name would not add any value. E.g. T in the Stack<T> class is appropriate since the indication that T is a type parameters is sufficiently descriptive; the stack works for any type T.
- CONSIDER: If a type parameter has a constraint, consider using the constraint in the type name.  e.g a type parameter with the IComponent constraint would be called TComponent.
- DO: Suffix custom attribute classes with Attribute.
- DO: Suffix custom exception types with Exception.
- DO: Enums type names must be singular unless flags.
- **TODO**: Do not begin groups of enumerated types with a common prefix e.g. Color.ColorRed  should just be Color.Red.

### Methods
- CONSIDER: Use verb-noun method names that perform an operation on an object. This Indicates what is being performed and what object is being acted on.
- DO: Indicate the “what” and not the “how”. When the name includes “how” the implementation is exposed.  Changing the implementation or semantics of the object, method, or data implies changing the name.

### Variables and fields
- DO: Prefix fields (if they are indeed needed) with underscore and then use ‘PascalCasing’ and wrap them with a property (even if the end result is private).
Exception: Unless you need to pass a member value by ref, and it cannot be a local variable. Example: Maintaining an internal reference count using Interlocked.
This largely removes the need for readonly fields in favor of properties.
- AVOID: Hungarian notation
- AVOID: incorporating the data type into the name.
- DO: Use C# alias types rather than their System namespace counterparts (ie. ‘int’ instead of ‘System.Int32’)
- DO: Use plural names for collections
- DO: Make boolean names should contain Is/Are/Can/Has which implies Yes/No or True/False values. 
- AVOID: Double negatives when referring to booleans.
- AVOID: var in cases where the the type of var is not obvious. Common exceptions would be constructors and generic method calls where it is clear the generic type is the return value.

### Abbreviations
- AVOID: Abbreviations unless project approved or generally accepted and documented.
- DO: If an object property name fully or partially uses a two letter abbreviation, it will be all uppercase; if it is three letters it will be mixed.

### Tests
- DO: Test namespaces follow the target class names space with the addition of “.Tests” suffix. (E.g. Fezzik.Tests)
- DO: Mirror test file location with the corresponding file being tested.
- DO: Name test class using the target class name with the “Tests” suffix.  (E.g. Fezzik.Tests.SwordPlayTests)
- DO: Use TDD and/or strive for 100% code coverage unless you have a reason not to – too much effort.  If missing code coverage, provide documentation/comments as to why.
- DO: Use [ExcludeFromCodeCoverage] attribute for generated or other code to keep it from negatively impacting the coverage rate. (see http://msdn.microsoft.com/en-us/library/dd984116(v=vs.100).aspx)
- DO: Use unit test names that identify what is being tested
- CONSIDER: Naming test methods with pattern <MethodName>_<StateUnderTest>_<ExpectedBehavior>

### Miscellaneous
- DO NOT: Use underscores to break up words in an identifier. Test methods, UI generated event handlers and references to Resource files are the exception.
- DO: Use American-style spelling.

## Layout

### Files
- DO: Put a single class per file unless varying arity, code is generated, creating nested classes, or there is a needed  “buddy class” to add DataAnnotations.
- DO: Give source files the name of the class in the file
- DO: Group members into sections based on type (Constants, Constructors, Fields and Properties, Methods, Interface implementations). Static members should be grouped with their respective instance members. (e.g. static methods grouped with instance methods)
- AVOID: Lines longer than 150 characters
- CONSIDER: Group members in a file by their member type. For examples, keep all properties together, all methods together, constructors/finalizers together.

### Properties
- DO: Use automatically implemented properties rather than backing fields unless backing field is required (validation or INotifyPropertyChanged implementations are examples where a backing field may be required).
- DO NOT: Access fields outside of their property wrapper or the constructor.
Exception: MVVM situations where the property setter is being used for input validation.
- DO: Use read-only properties (C# 6.0) in favor of read-only fields

### Regions
- AVOID: Using regions that do not add value.
- CONSIDER: Using regions around interface implementations
- DO: Provide a name for all regions (specify text after #region)

### Attributes
- DO: Place class member attribute decorations onto separate lines and encapsulated in its own square brackets.
- CONSIDER: Placing attribute reflection code within the attribute – generally as static methods.

### Comments
- DO NOT: duplicate source control metadata in comments. Omit things “Created by”, “Last edited on”, etc. 

### Whitespace
- DO NOT: Mix tabs and spaces.
- DO NOT: Mix indentation sizes.

## Coding

### Strings
- CONSIDER: Using string interpolation over string.Format when possible unless localization is required. 
DO NOT: hardcode locale specific strings. Put into resource files, or config files, or into constants so that they are easy to change and find. 
Exception: Messages used in Exceptions should be targeted at developers and, as such, do not require localization.  
See http://msdn.microsoft.com/en-us/library/dd465121.aspx for more detail around comparison and sorting of strings.

### Localization
- DO: Store all dates in UTC. Convert to locale specific values at the client.
- DO: Specify CultureInfo when calling Parse/TryParse when parsing strings that may be localized. 

### Methods
- DO NOT: Return null from a method with a return type of IEnumerable<T>.
- AVOID: Null checks on IEnumerable<T> return values.
- DO NOT: Return null from a method with a return type Task<T>.

### Exceptions
- DO: Throw the most specific (most derived) exception that makes sense.
- DO NOT: Swallow errors by catching nonspecific exceptions such as System.Exception and System.SystemException unless the exception is re-thrown
- DO: Catch only exceptions you are explicitly handling. Use exception filters to further limit which exceptions you handle.
- DO NOT: Over catch exceptions. Exceptions should often be allowed to propagate up the call stack. Use exception filters to avoid catching exceptions that you cannot handle.
- DO: In a catch statement that throws an exception, throw the original (using throw;)  or wrapped exception if the exception type is not appropriate to re-throw  (throw new Exception(exception)). This maintains the stack location of the original error.
- AVOID: Defining custom exception classes.
- AVOID: Returning error codes.  Exceptions are the primary means of reporting errors in frameworks.
- CONSIDER: terminating the process by calling System.Environment.FailFast if code encounters a situation where it is unsafe for further execution.
- AVOID: Creating APIs that when called can result in a system failure.  If such a failure can occur, call Environment.FailFast when the system failure occurs instead.
- DO NOT: Use exceptions for normal flow of control.
- DO NOT: Have public members that can either throw or not based on some option.
- DO NOT: Have public members that return exceptions as return value or an out parameter.
- AVOID: Explicitly throwing exceptions from finally blocks. (Implicitly thrown exceptions resulting from calling methods that throw are acceptable.)
- DO NOT: Create or throw custom exception types that callers are not expected to handle. Throw one of the existing Framework exceptions instead. 
- CONSIDER: Creating and throwing custom exceptions if you have a unique program error that cannot be communicated using an existing framework exceptions
- DO NOT: Create a new exception type if the exception would not be handled differently than an existing Framework exception – throw the existing framework exception instead.
- DO: Use try-finally for clean up work and avoid using try-catch.
- CONSIDER: Wrapping specific exceptions thrown from the lower layer in a more appropriate exception if the lower-layer exception does not make sense in the context of the higher-layer operation.
- AVOID: Catching and wrapping in nonspecific exceptions.
- DO NOT: Throw System.Exception, System.SystemException, or System.NullReferenceException. Throw the existing framework exception instead, such as System.InvalidOperationException. 
AVOID: Catching System.Exception or System.SystemException except in top-level exception handlers.
- DO NOT: Throw or derive from System.ApplicationException.
- DO: Throw an InvalidOperationException if the object is not in a valid state.
- DO: Throw ArgumentException or one of its subtypes (ArgumentNullException) if bad arguments are passed to a member.  Prefer the most derived exception type, if applicable. Set the parameter name of the bad argument that was passed. 
- DO NOT: Explicitly throw ComException, ExecutionEngineException, or SEHException.
- AVOID: Deep exception hierarchies.
- DO: Make exceptions runtime serializable.
- DO: Provide exception constructors for default, string message, and Exception inner.
- CONSIDER: Providing exception properties for programmatic access to extra information relevant to the exception.
- CONSIDER: Providing a tester method for members that might throw exceptions in common scenarios to avoid performance problems (Tester-Doer Pattern).  E.g. TryParse().
- DO: Provide a Try-X method for every throwing X method.

### Miscellaneous
- DO NOT: Initialize to default values. E.g. int I = 0; It makes code clearer when only ‘non default’ values are initialized. 
- DO NOT: Use the ‘this’ modifier unless it is required (ie. passing this or removing an ambiguity or obfuscation). 
- CONSIDER: Using static when the static type name does not add value to the code. Such as “using static System.Console” inside of a console application.
