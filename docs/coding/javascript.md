# JavaScript Guidelines
Minimize the danger caused by a scripting (not strongly typed) language.

## Sections

- [JavaScript Guidelines](#javascript-guidelines)
  - [Sections](#sections)
- [Guidelines](#guidelines)
  - [Indenting](#indenting)
  - [String Concatenation](#string-concatenation)
  - [CamelCasing](#camelcasing)
  - [Semi-colons](#semi-colons)
  - [Control Structures](#control-structures)
    - [switch](#switch)
    - [try](#try)
    - [for in](#for-in)
  - [Functions](#functions)
    - [Functions and Methods](#functions-and-methods)
    - [Function Calls](#function-calls)
    - [Function Declarations](#function-declarations)
  - [Variables and Arrays](#variables-and-arrays)
    - [Arrays](#arrays)
  - [Comments](#comments)
  - [JS code placement](#js-code-placement)
  - ["with" statement](#with-statement)
  - [Operators](#operators)
    - [True or false comparisons](#true-or-false-comparisons)
    - [Comma Operator](#comma-operator)
    - [Avoiding unreachable code](#avoiding-unreachable-code)
  - [Constructors](#constructors)
  - [Use literal expressions](#use-literal-expressions)
  - [eval is evil](#eval-is-evil)
  - [Preventing XSS](#preventing-xss)
  - [Typeof](#typeof)
  - [Modifying the DOM](#modifying-the-dom)

# Guidelines

## Indenting

Use an indent of 2 spaces, with no tabs. No trailing whitespace.

## String Concatenation
- :heavy_check_mark: DO prefer template literals to string concatenation.
- :heavy_check_mark: DO use a space between the `+` and the concatenated parts to improve readability.
```js
var string = 'Foo' + bar;
string = bar + 'foo';
string = bar() + 'foo';
string = 'foo' + 'bar';
```
- :heavy_check_mark: WHEN using the concatenating assignment operator ('+='), use a space on each side as with the assignment operator:
```js
var string += 'Foo';
string += bar;
string += baz();
```
- :heavy_check_mark: DO configure ESLint rules `space-infix-ops` and `space-unary-ops` with option `"error"`.

## CamelCasing
Multi-word variables and functions in JavaScript should be lowerCamelCased. The first letter of each variable or function should be lowercase, while the first letter of subsequent words should be capitalized. There should be no underscores between the words.
- :heavy_check_mark: DO configure ESLint rule `camelcase` with option `["error", {"properties": "always"}]`.

## Semi-colons

JavaScript allows any expression to be used as a statement and uses semi-colons to mark the end of a statement. However, it attempts to make this optional with "semi-colon insertion", which can mask some errors and will also cause JS aggregation to fail. All statements should be followed by `;` except for the following: `for`, `function`, `if`, `switch`, `try`, `while`

The exceptions to this are functions declared like
```js
Drupal.behaviors.tableSelect = function (context) {
	// Statements...
};
```
and
```js
do {
    // Statements...
} while (condition);
```
These should all be followed by a semi-colon. In addition the `return` value expression must start on the same line as the return keyword in order to avoid semi-colon insertion.

- :heavy_check_mark: DO use a semi-colon except where it's not allowed
- :heavy_check_mark: DO configure ESLint rule `semi` with option `["error", "always"]`.

## Control Structures
These include `if`, `for`, `while`, `switch`, etc. Here is an example if statement, since it is the most complicated of them:
```js
if (condition1 || condition2) {
  action1();
}
else if (condition3 && condition4) {
  action2();
}
else {
  defaultAction();
}
```

Control statements should have a new line between the control keyword and opening parenthesis, to distinguish them from function calls.
- :grey_question: STRONGLY CONSIDER using curly braces even in situations where they are technically optional. Having them increases readability and decreases the likelihood of logic errors being introduced when new lines are added.

### switch
For `switch` statements:
```js
switch (condition) {
  case 1:
    action1();
    break;

  case 2:
    action2();
    break;

  default:
    defaultAction();
}
```

### try
The try class of statements should have the following form:
```js
try {
  // Statements...
}
catch (error) {
  // Error handling...
}
finally {
  // Statements...
}
```
Omit the `catch` or `finally` clauses if they are not needed, but never omit both.

### for in

The `for in` statement allows for looping through the names of all of the properties of an object. Unfortunately, all of the members which were inherited through the prototype chain will also be included in the loop. This has the disadvantage of serving up method functions when the interest is in data members. To prevent this, the body of every for in statement should be wrapped in an if statement that does filtering. It can select for a particular type or range of values, or it can exclude functions, or it can exclude properties from the prototype. For example:
```js
for (var variable in object) 
{
  if (filter) {
    // Statements...
  }
}
```
- :grey_question: CONSIDER configuring ESLint rule `guard-for-in` with option `"error"`, or avoiding `for in` in favor of `Object.keys(obj).forEach(` which doesn't include properties in the prototype chain.

## Functions

### Function Calls
Functions should be called with no spaces between the function name, the opening parenthesis, and the first parameter; spaces between commas and each parameter, and no space between the last parameter, the closing parenthesis, and the semicolon. Here's an example:
```js
foobar = foo(bar, baz, quux);
```
If a function literal is anonymous, there should be one space between the word function and the left parenthesis. If the space is omitted, then it can appear that the function's name is actually "function".
```js
div.onclick = function (e) {
  return false;
};
```
- :heavy_check_mark: DO configure ESLint rule `func-call-spacing` with option `["error", "never"]`.
- :heavy_check_mark: DO configure ESLint rule `space-before-function-paren` with option `["error", {"anonymous": "always", "named": "never", "asyncArrow": "always"}]`.

### Function Declarations
```js
function funStuff(field) {
  alert("This JS file does fun message popups.");
  return field;
}
```
Arguments with default values go at the end of the argument list. Always attempt to return a meaningful value from a function if one is appropriate. Please note the special notion of anonymous functions explained above.
- :heavy_check_mark: DO configure ESLint rule `default-param-last` with option `"error"`.

## Variables and Arrays
All variables should be declared with `const` unless you need to use `let` before they are used and should only be declared once. Doing this makes the program easier to read and makes it easier to detect undeclared variables that may become implied globals.
- :no_entry: AVOID variables in global scope. The only time something should be made global is when setting the namespace for the module.

### Arrays
Arrays should be formatted with a space separating each element and assignment operator, if applicable:
```js
someArray = ['hello', 'world']
```

## Comments
Inline documentation for source files should follow the [jsdoc formatting conventions](https://jsdoc.app/about-getting-started.html).
Non-documentation comments are strongly encouraged. A general rule of thumb is that if you look at a section of code and think "Wow, I don't want to try and describe that", you need to comment it before you forget how it works. Comments can be removed by JS compression utilities later, so they don't negatively impact on the file download size.

Non-documentation comments should use capitalized sentences with punctuation. All caps are used in comments only when referencing constants, e.g., TRUE. Comments should be on a separate line immediately before the code line or block they reference. For example:
```js
// Unselect all other checkboxes.
```

If each line of a list needs a separate comment, the comments may be given on the same line and may be formatted to a uniform indent for readability.
C style comments (`/* */`) and standard C++ comments (`//`) are both fine.

## JS code placement
JavaScript code should not be embedded in the HTML where possible, as it adds significantly to page weight with no opportunity for mitigation by caching and compression.

## "with" statement
The `with` statement was intended to provide a shorthand for accessing members in deeply nested objects. For example, it is possible to use the following shorthand (but not recommended) to access foo.bar.foobar.abc, etc:
```js
with (foo.bar.foobar) {
  const abc = true;
  const xyz = true;
}
```
However it's impossible to know by looking at the above code which abc and xyz will get modified. Does `foo.bar.foobar` get modified? Or is it the global variables abc and xyz? Instead you should use the explicit longer version:
```js
foo.bar.foobar.abc = true;
foo.bar.foobar.xyz = true;
```
or if you really want to use a shorthand, use the following alternative method:
```js
let o = foo.bar.foobar;
o.abc = true;
o.xyz = true;
```
- :heavy_check_mark: DO configure ESLint rule `no-with` with option `"error"`.

## Operators

### True or false comparisons
The `==` and `!=` operators do type coercion before comparing. This is bad because it causes:
```js
' \t\r\n' == 0
```
to be true. This can mask type errors. When comparing to any of the following values, use the `===` or `!==` operators, which do not do type coercion:
```js
0 '' undefined null false true
```
`!=` and `==` are good for handling `undefined | null`
- :no_entry: AVOID the `==` and `!=` operators in favor of `===` or `!==`, unless checking against the literal `null` when checking against both `null` and `undefined` is desired.
- :heavy_check_mark: DO - Configure ESLint rule `eqeqeq` with option `["error", "smart"]`.

### Comma Operator
The comma operator causes the expressions on either side of it to be executed in left-to-right order, and returns the value of the expression on the right, and should be avoided. Example usage is:
```js
var x = (y = 3, z = 9);
```
This sets x to 9. This can be confusing for users not familiar with the syntax and makes the code more difficult to read and understand. So avoid the use of the comma operator except for in the control part of for statements. This does not apply to the comma separator (used in object literals, array literals, etc.)
- :no_entry: AVOID the comma operator
- :heavy_check_mark: DO - Configure ESLint rule `no-sequences` with option `"error"`.

### Avoiding unreachable code
To prevent unreachable code, a `return`, `break`, `continue`, or `throw` statement should be followed by a } or `case` or `default`.
- :heavy_check_mark: DO - Configure ESLint rule `no-unreachable` with option `"error"`.

## Constructors
Constructors are functions that are designed to be used with the `new` prefix. The `new` prefix creates a new object based on the function's prototype, and binds that object to the function's implied this parameter. JavaScript doesn't issue compile-time warning or run-time warnings if a required `new` is omitted. If you neglect to use the `new` prefix, no new object will be made and this will be bound to the global object (bad). Constructor functions should be given names with an initial uppercase and a function with an initial uppercase name should not be called unless it has the `new` prefix.
- :heavy_check_mark: DO use ES2015 class syntax, downleveling via Babel transpilation if obsolete browsers must be supported.

## Use literal expressions
Use literal expressions instead of the new operator:
- :heavy_check_mark: Instead of `new Array()` use `[]`
- :heavy_check_mark: Instead of `new Object()` use `{}`
- :no_entry: Don't use the wrapper forms `new Number`, `new String`, `new Boolean`.

In most cases, the wrapper forms should be the same as the literal expressions. However, this isn't always the case, take the following as an example:
```js
var literalNum = 0;
var objectNum = new Number(0);
if (literalNum) { } // false because 0 is a false value, will not be executed.
if (objectNum) { }  // true because objectNum exists as an object, will be executed.
if (objectNum.valueOf()) { } // false because the value of objectNum is 0.
```
- :heavy_check_mark: DO configure ESLint rules `no-new-wrappers`, `no-new-object`, and `no-array-constructor` with option `"error"`.

## eval is evil
`eval()` is evil. It effectively requires the browser to create an entirely new scripting environment (just like creating a new web page), import all variables from the current scope, execute the script, collect the garbage, and export the variables back into the original environment. Additionally, the code cannot be cached for optimization purposes. It is probably the most powerful and most misused method in JavaScript. It also has aliases. So do not use the `Function` constructor and do not pass strings to `setTimeout()` or `setInterval()`. NEVER pass untrusted strings to `eval()` or similar functions.
- :heavy_check_mark: DO configure ESLint rules `no-implied-eval` and `no-new-func` with option `"error"`.

## Preventing XSS
Rendering untrusted strings to HTML should be done through the appropriate mechanisms for the framework being used, if any. For example, `{{interpolation}}` for Vue and Angular, `{interpolation}` for React, or `data-bind="text: stringVariable"` for Knockout. If no framework is being used, untrusted input should be rendered by setting the [`textContent`](https://developer.mozilla.org/en-US/docs/Web/API/Node/textContent) of the desired DOM node or by creating a new text node with [`document.createTextNode()`](https://developer.mozilla.org/en-US/docs/Web/API/Document/createTextNode).

## Typeof
When using a typeof check, don't use the parenthesis for the typeof. The following is the correct coding standard:
```js
if (typeof myVariable == 'string') {
  // ...
}
```
