[DevOps Strategy](/Project-Artifacts/DevOps-Strategy)

**Capabilities Inventory**

|Capability|Ideal State|Tools In Inventory|
|--|--|--|
|Infrastructure Provisioning|Infrastructure is provisioned and configured as code along with the rest of the workload in an automated CI/CD pipeline|AWS CloudFormation, Azure Resource Manager|
|Infrastructure Configuration|Infrastructure is provisioned and configured as code along with the rest of the workload in an automated CI/CD pipeline|AWS Config, AWS CLI, Azure Policy, Azure CLI|
|Application Provisioning|Automatically create application CI in ServiceNow based on what gets deployed; users and administrators provisioned with proper access|ServiceNow, AzureAD, Okta|
|Application Configuration|Configuration settings are exposed based on user roles.  Changes to configuration items are audited and sent through and appropriate approval process.|AWS OpWorks|

**Infrastructure Provisioning**
- Infrastructure will be provisioned using AWS CloudFormation templates.  This will allow the team to standardize on a tool several team members already know that the company is licensed for, has support for from AWS and will allow us to take advantage of all AWS native capabilities.  DCX is not intended to be cloud agnostic as it must take advantage of vendor specific services, e.g. CloudFront.

- All CloudFormation templates will be stored in Azure DevOps and deployed use the Azure DevOps deployment pipeline.

- Infrastructure provisioning will follow the [resource naming guidelines](https://phillips66.sharepoint.com/sites/IT_CloudRunway/DOH/Pages/Resource%20Naming.aspx) and [tagging guidelines](https://phillips66.sharepoint.com/sites/IT_CloudRunway/DOH/Pages/Resource%20Tags.aspx) outlined by CloudRunway.

**Infrastructure Configuration**
- Since all resources will be deployed and configured via code (Terraform, CloudFormation, etc.), configurations will be stored in version control (Git Repo).

- Any updates will be reviewed via a Pull Request, then can be deployed by the service of choice. Terraform and CloudFormation both have the capability to update existing resources if possible, and if not, they'll replace any resources that cannot be updated in place.

**Application Provisioning**
- Applications will consist primarily of cloud services and AWS Lambda functions and will be designed to be serverless.

- They will be provisioned using the [AWS Serverless Application Model (AWS SAM)](https://docs.aws.amazon.com/serverless-application-model/latest/developerguide/what-is-sam.html) templates.

- The templates will be stored in Azure DevOps and deployed using the Azure DevOps deployment pipeline.

**Application Configuration**
- Application configuration information will be stored in different places depending on the vendor.

- AWS Lambda functions will be configured according to the [AWS guidelines](https://docs.aws.amazon.com/lambda/latest/dg/lambda-configuration.html).

- Sensitive information, e.g., passwords or API keys, will be stored in [AWS Secrets Manager](https://aws.amazon.com/secrets-manager/) based on guidelines to be provided by CloudRunway.

- MuleSoft configuration will be stored and managed according to the MuleSoft best practices.  For more information on MuleSoft, go to the [MuleSoft site on Teams](https://teams.microsoft.com/l/team/19%3a90fdb0298b62405187a92b6ef779571d%40thread.skype/conversations?groupId=c3720085-0e68-401d-b28f-cdbf3ca059db&tenantId=4febecd1-b635-4bb7-96fd-6688487e52f1).

