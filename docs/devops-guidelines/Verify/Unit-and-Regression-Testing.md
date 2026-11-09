**Unit and Regression Testing**

Developers will use Test-driven development (TDD) technique which combines test-first development where you write a test before you write just enough code to fulfill that test. 

The benefits of Test-driven development are more than just simple validation of correctness but can also drive the design of the code ensuring it meets the test case.

Unit tests are executed as part of the build process. Failing unit tests are required to be fixed prior to merges or releases. Test-driven development creates a regression-test suite as a side effect that can minimize human manual testing, while finding problems earlier, leading to quicker fixes.

Regression tests are composed of the entire suite of unit tests + additional higher-level tests as needed. Regression tests are automatically verified as part of the build process prior to release.

A good overall testing strategy should rely heavily on unit testing but may  be augmented by automated UI testing and manual testing where required.

<todo: Test Pyramid>
<todo: UI testing tools>

