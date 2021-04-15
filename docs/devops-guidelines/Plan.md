[[_TOC_]]

# Planning and Managing Work

Value to be delivered by the project should be managed on a backlog

Project code shoudl be connected to work: Product Backlog Items/User Stories and Bugs/Defects.

This enables Azure DevOps to automatically generate release notes and audit reports providing traceability from inception through delivery.

See [P66 Agile Initiative]() for additional information.

## Work Management Tooling

|Capability|Ideal State|Primary Tool|
|--|-|--|
|Requirements Management|Requirements are captured as Epics/Features/User Stories on a Product Backlog.|Azure DevOps|
|Defect Tracking|Defects are tracked, triaged, prioritized with Product Backlog items.<br/>For addtional information see [See Testing Strategy - Defect Tracking](/Testing-Strategy/Defect-Tracking)|Azure DevOps|
|Incident Management|Incidents and defects should be tracked together.  Outages must have a complete root cause analysis with documented actions that will be taken to ensure the risk of a similar outage is reduced in the future.|ServiceNow|
|Release Planning|Release plans are driven by Epics/Features/User Stories in a backlog. Features that are not complete, may need to be disabled (Feature Flag) or held in a Development branch until they are ready. See [Version Control](/Version-Control) for additional information.|Azure DevOps|

Notes:
- Production defects are created as incidents reported to ServiceNow. If the incident is triaged and determined to be a defect in code, a Bug is created in the appropriate Azure DevOps project.


## Release Planning Guidance

Multiple options exist for managing releases. With any approach, deliverable work (features, partial features, bugs) are defined in a backlog and are delivered in iterations.

- The simplest approach to release planning is by grouping iterations into a release in Azure DevOps. See [Areas and Iterations](https://docs.microsoft.com/en-us/azure/devops/organizations/settings/about-areas-iterations?toc=%2fazure%2fdevops%2fboards%2fsprints%2ftoc.json&%3bbc=%2fazure%2fdevops%2fboards%2fsprints%2fbreadcrumb%2ftoc.json&view=azure-devops). 
- Larger teams with dependencies can leveral plans as described in [Plans (Agile at scale)](https://docs.microsoft.com/en-us/azure/devops/boards/plans/?toc=%2Fazure%2Fdevops%2Fboards%2Ftoc.json&bc=%2Fazure%2Fdevops%2Fboards%2Fbreadcrumb%2Ftoc.json&view=azure-devops)

A major goal of DevOps and DevOps Strategy is to progress to Continuous Deployment (CD). True continous deployment significantly reduces the need to plan for major releases. Release planning becomes a part of every feature instead of a special event requiring additional effort.

Continous Deployment is a goal for most teams. Before getting to CD, release planning is required with releases categorized as major or minor.

- _Major releases_ typically include new features and capabilities, workflow changes, major changes to interfaces and integration points. A major release typically includes the work of several sprints. Use 3 to 6 sprints as a guideline.
- _Minor releases_ are focused on smaller and incremental changes and fixes. A minor release typically doesn't include signifcant new capabilities. A minor release  or system changes more frequently (2 sprints or less). 
