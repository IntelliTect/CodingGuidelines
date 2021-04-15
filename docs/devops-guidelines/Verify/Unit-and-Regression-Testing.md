**Unit and Regression Testing**

Developers will use Test-driven development (TDD) technique which combines test-first development where you write a test before you write just enough code to fulfill that test. 

The benefits of Test-driven development are more than just simple validation of correctness but can also drive the design of the code ensuring it meets the test case.

Unit tests are executed as part of the build process. Failing unit tests are required to be fixed prior to merges or releases. Test-driven development creates a regression-test suite as a side effect that can minimize human manual testing, while finding problems earlier, leading to quicker fixes.

Regression tests are composed of the entire suite of unit tests + additional higher-level tests as needed. Regression tests are automatically verified as part of the build process prior to release.

The overall testing strategy will rely heavily on unit testing but will be augmented by automated UI testing and some manual testing where required.  Automated UI testing will be run through WorkSoft.

DCX will leverage the Ring deployment strategy
- Ring 1 is the primary development ring. It will be utilized by developers for development and developer testing.
- Ring 2 is the integration testing ring. It will be leveraged by developers and testing personnel to verify the code builds and deploys correctly, that all automated unit and regression tests pass and that the system integrates with Phillips 66 test enterprise resources, e.g., SAP test environments or OSS test database. All deployments to Ring 2 must be approved by the DCX Architect and the DCX QA Lead.

See [DevOps Strategy/Environments](https://p66-default.visualstudio.com/Digital%20Customer%20Experience/_wiki/wikis/DigitalCustomerExperience.wiki?pagePath=%2FProject%20Artifacts%2FDevOps%20Strategy%2FEnvironments&pageId=46&wikiVersion=GBwikiMaster) for more details on Rings.
