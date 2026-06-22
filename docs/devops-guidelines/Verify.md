[[_TOC_]]

Verifying quality involves many tools and many steps from proper work management through monitoring issues that may occur in a production environment.

Standard scope of quality verification in DevOps includes:
1. Automated unit tests of code
2. Performing regular peer based code reviews
3. Using branch policies requiring Pull Requests into the branch be reviewed and pass quality checks
4. Analysing code to check for code quality and security issues
5. Automated integration testing 
6. Functional testing
7. Load and performance testing 
8. Security testing

# Verification Strategy
Each project should develop test and verify strategy that is appropriate for the project. Factors include number of users, type of project (public facing web site vs. extranet portal vs. data analysis), data privacy and or risk to the business for data exposure.

A project cannot simply follow a recipe blindly.

# Testing Pyramid
Of the techniques mentioned, the most powerful is automated unit testing. If appropriate for your project, it is recommended that this become 

## Unit Test References


# Automated Unit Testing

**Capabilities Inventory**

|Capability|Ideal State|Tools In Inventory|
|--|--|--|
|Code Review|Code reviews are done consistently using pre-defined standards and objective measures of code and design quality.  Code is compared to the agreed to architecture to ensure compliance.  Reviews are documented, any required changes are documented and worked.  Follow-ups are scheduled and tracked.|Azure DevOps|
|Infrastructure Review|Infrastructure is scanned automatically using pre-defined guardrails.  Alerts are generated if a change is outside predefined limits.|AWS CloudWatch|
|Licensing Compliance|All code is scanned automatically on check-in for license compliance.  Approved licenses are agreed to and documented.  License compliance is also added to the code review process.||
|Static Quality Analysis|All code is scanned automatically on check-in for quality issues.  Quality issues are required to be remedied prior to merging to an upstream branch.|SonarQube|
|Unit and Regression Testing|Complex code and foundational code is covered by unit tests.  Complex code and foundational code is automatically identified to ensure compliance.  Unit tests are executed as part of the build process.  Failing unit tests are required to be fixed prior to merges or releases.  Regression tests are composed of the entire suite of unit tests + additional higher-level tests as needed.  Regression tests are automatically verified as part of the build process prior to release.|Junit, NUnit, Selenium, WorkSoft|
|Performance Testing|Performance testing is part of the deployment pipeline.  All applications are held to documented performance standards.|LoadRunner|
|Security Analysis|Code is automatically scanned to protect against malicious components, detect plaintext credentials, validating appropriate IAM, and monitoring for behavioral anomalies.||

**Infrastructure Review**

- Infrastructure will be reviewed using the code review process as all infrastructure will be created via code.

- These reviews will be focused on ensuring the services are being configured properly and are in alignment with the architectural guidelines.

**Licensing Compliance**

- Licensing compliance will be enforced by manual reviews of any external components or libraries during the code review process.  If the enterprise procures an automated license compliance capability, the project will leverage the enterprise tools.

**Static Quality Analysis**

- Static Quality Analysis will be part of the build pipeline.  SonarQube is expected to be leveraged for this task.

- Quality metrics will be used to ensure the project is meeting the non-functional quality and security requirements.


**Performance Testing**
<todo: />

**Security Analysis (Static)**

- Include Static Security Analysis (Static) as part of an automated Continuous Integration build pipeline. Code analysis capabilities are built in to GitHub, and, several good external tools exist including SonarQube which will report on vulnerabilities and fail builds if scans do not complete.  
- Scans should focus on the OWASP Top 10 and the SANS Top 25.
<todo: reference tools />

**Security Analysis (Dynamic)**
- Security Analysis (Dynamic) should be executed as part of a Continuous Deployment pipeline.
<todo: reference tools />


