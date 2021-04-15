[DevOps Strategy](/Project-Artifacts/DevOps-Strategy)

**Capabilities Inventory**

|Capability|Ideal State|Tools In Inventory|
|--|--|--|
|Performance Monitoring|Well established baselines and KPIs that can be tested against, monitored, and improved as a part of CI/CD pipeline and release cycles|AWS CloudWatch, Microsoft SCOM|
|Security Monitoring|Automatically monitor and alert for appropriate credential rotation events, critical vulnerability patches, and user activity anomalies|AWS CloudTrail, SkyHigh, Dome9, QRadar|
|Feedback Management|Build feedback submission tools into user interfaces to obtain feedback directly from customers; leverage Product Owner to translate feedback into feature-oriented user stories to be captured and prioritized by planning tools for future releases|Azure DevOps|
|Consumption Management|Utilize CloudHealth to monitor and report off cloud costs across AWS & Azure; tags applied at resource creation based on ServiceNow application CI|CloudHealth|
|Telemetry|Data is captured for all of the major types of telemetry data, business metrics, application metrics, infrastructure metrics and deployment pipeline metrics.  Telemetry data is analyzed and leveraged as KPIs to improve the product as part of the feedback loop.| |

**Performance Monitoring**
- DCX will follow the [guidance from CloudRunway](https://phillips66.sharepoint.com/sites/IT_CloudRunway/DOH/Pages/Monitoring%20and%20Logging.aspx) on monitoring and logging.

- [Amazon CloudWatch Metrics](https://docs.aws.amazon.com/lambda/latest/dg/monitoring-functions.html) will be used to monitor the performance of AWS Lambda functions against expectations.  

- [Amazon CloudWatch Metrics](https://docs.aws.amazon.com/AmazonS3/latest/dev/cloudwatch-monitoring.html) will also be used to monitor the performance of AWS S3.

- Other PaaS/SaaS vendors will be monitored using their native tools.  

- BloomReach performance monitoring guidelines can be found [here](https://documentation.bloomreach.com/library/concepts/web-application/jmx-management-beans-support.html).

**Security Monitoring**
- DCX will follow the [guidance from CloudRunway](https://phillips66.sharepoint.com/sites/IT_CloudRunway/DOH/Pages/Security.aspx) on security best practices.

**Feedback Management**
- 
**Consumption Management**
- DCX will utilize CloudHealth to monitor and report cloud costs across AWS & Azure.  Tags will be applied in accordance with the guidance from Cloud Runway.

**Telemetry**
- Telemetry data will be captured [Amazon CloudWatch Metrics](https://docs.aws.amazon.com/AmazonS3/latest/dev/cloudwatch-monitoring.html) and recorded to [Azure Application Insights](https://docs.microsoft.com/en-us/azure/azure-monitor/app/app-insights-overview).
