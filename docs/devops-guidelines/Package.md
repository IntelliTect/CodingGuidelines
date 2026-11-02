**Capabilities Inventory**

|Capability|Ideal State|Tools In Inventory|
|--|--|--|
|Package Configuration|Packages are managed by a package manager which groups software together into a release.  This will include infrastructure, application code and external libraries. All artifacts are tracked by version. Dependencies between various components and versions are tracked to reduce the risk of errors and create a repeatable build process.|Azure DevOps|
|Approval Workflow|Releases are managed by approvals at each quality gate.&nbsp;&nbsp;Nothing moves without an approval.&nbsp;&nbsp;Approval workflow supports delegation and escalation.|Azure DevOps, ServiceNow|


**Package Configuration**
Packages (builds) and deployments will be automated with Azure DevOps Pipelines and assigned an automated version that increments with each build. Azure DevOps creates reliable builds on all platforms through integration with Azure Repos using Git for pull requests, checks, and statuses. Code will be built, tested and deployed via Azure DevOps Repos and Pipelines.

**Approval Workflow**

The approval workflow provided by Azure DevOps will be used for all flows except the final push to production.  

*Deployments to production will require the creation of a change request in ServiceNow.*

The workflow will be as follows:

* Developers can build to their local environment and Ring 1 as needed.
* Developer initiates pull request.
* Development Lead and Architect review code and infrastructure.
* Development Lead and Architect approve code to be merged.
* Merge triggers a build to Ring 1.
* All unit tests and automated scans are triggered.
* If all unit tests and automated scans pass, the Architect and Quality Assurance Lead are notified.
* Quality Assurance Lead and Architect approve deployments to Ring 2 (Integration/Build Testing).
* Quality Assurance Lead approves deployments to Ring 3 (Functional Testing) and Ring 4 (UAT).
* Quality Assurance Lead and Product Owner approve deployment to Ring 5 (Production).
