[DevOps Strategy](/Project-Artifacts/DevOps-Strategy)

**Capabilities Inventory**

|Capability|Ideal State|Tools In Inventory|
|--|--|--|
|Infrastructure Provisioning|Infrastructure is provisioned and configured as code along with the rest of the workload in an automated CI/CD pipeline|Terraform, AWS CloudFormation, Azure Resource Manager|
|Infrastructure Configuration|Infrastructure is provisioned and configured as code along with the rest of the workload in an automated CI/CD pipeline|Terraform, AWS Config, AWS CLI, Azure Policy, Azure CLI|
|Application Provisioning|Provision via Continuous Deployment pipeline using GitHub Actions, Azure Pipelines, or similar; users and administrators provisioned with proper access|Azure DevOps, GitHub|
|Application Configuration|Configuration settings are exposed based on user roles.  Changes to configuration items are audited and sent through and appropriate approval process.||

**Infrastructure Provisioning**
- Infrastructure will be provisioned using Terraform templates.  This will allow the team to standardize on a tool several team members already know that the company is licensed for, is supported for common cloud platforms and provides provisiong of most native capabilities.

- All provisioning templates regardless of technology will be stored in git repos and deployed use the pipeline toofl of choice such as GitHub Actions or Azure DevOps Pipelines.

- Infrastructure provisioning should follow [resource naming guidelines](todo ) and [tagging guidelines](todo ) .

**Infrastructure Configuration**
- Since all resources will be deployed and configured via code (Terraform, CloudFormation, etc.), configurations will be stored in version control (Git Repo).

- Any updates will be reviewed via a Pull Request, then can be deployed by the service of choice. Terraform has the capability to update existing resources or replace resources that cannot be updated in place.

It is critical that any resource that maintains state (databases, blob storage, aka a stateful resource) be backed up and restorable in the event of a resource replacement, or, stateful resources should be excluded from full automated provisioning.

**Application Provisioning**
- Applications should be provisioned via automated pipelines
- Pipeline templates will be stored in git repos along with the the application code

**Application Configuration**
- Non secret application configuration information should be stored with source code.

- Sensitive information, passwords or API keys, should be stored in a cloud native secret store, or, the pipeline tool of choice.

- Secrets based only on deployment environment can be 

