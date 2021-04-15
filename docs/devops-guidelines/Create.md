[[_TOC_]]

Specific tools used to create the source and related artifacts will be determined by a project team.

These guidelines are intended to be general guidance and will be applicable to most teams.

# Managing Artifacts
For technical and software projects, a variety of artifacts are created. All artifacts should be maintined in git based repos 

- Design artifacts such as high level designs, information architecutre, wire frames and similar should be committed to source and associated with a User Story whenever possible
- Software developers create code in a branch created from a User Story or Bug/Defect

All artifacts 

Branching Strategies
Repository Structure


**Capabilities Inventory**

|Capability|Ideal State|Tools In Inventory|
|--|--|--|
|Design|Designs are driven by user stories, captured ahead of development and vetted against a Definition of Ready (DoR) for design acceptance.  Design documents incorporate all aspects of the design, to include at a minimum UX/UI, infrastructure, data models, logic and integration. Design may optionally include a clickable prototype or a proof of concept.|Adobe XD, Zeplin, Adobe Photoshop, Adobe Illustrator|
|Implementation|Developers work on tasks tied to user stories.  Unit tests are developed for acceptance criteria defined in the design ahead of development of the actual product.  Tasks are completed in priority order.  Development is accelerated by the use of IDEs and feedback tools.|Visual Studio, Visual Studio Code, MuleSoft, SQL Server Management Studio, Oracle SQL Developer, PL/SQL Developer, Automation Anywhere, PgAdmin, Resharper
|Version Control|All artifacts are versioned and revisions to those artifacts are tracked by a version control system.  Merges are simple and reversion to a "known-good state" is easy.  The version control system supports distributed development and forms the backbone for the build/deployment pipelines.|git based Azure DevOps Repos |

# Design
Designs may consist of wireframes, mockups or prototypes. A wireframe is a low-fidelity visual representation of the site and will be used to depict the basic UI, detailing the layout and structure of the site. A mockup is a mid to high-fidelity visual representation of the site and will be used to highlight color scheme, visual style and typography. A prototype is a high-fidelity visual representation of the site and will be used to simulate user interaction by allowing the user to experience the content through clickable elements and interactions. In the event the design process needs to be accelerated, wireframes may be substituted with mockups.

Designs will utilize Material Design, Font Awesome Icons and custom components. Phillips 66 brand guidelines will influence design concept and style. 

Designs will be reviewed by the design team, development team and business analysts before handing off for development. User stories, acceptance criteria and UX personas are used to approve and validate designs. Optionally, user testing may be included in design review. 

# Implementation
Implementation is where the rubber meets the road.  The plans are laid.  The user stories are written.  The test cases have been identified and it's finally time to build something.

Implementation will follow the SCRUM methodology and utilize Test Driven Development (TDD).  All features will be logged as user stories with acceptance criteria before they are worked.

All development is expected to be performed using Visual Studio Code, Mulesoft Anypoint Studio and Eclipse.

## Source Management

Git repos in Azure DevOps are preferred. 

Associate check ins using the #<work item number> in comments with Work Items to automatically generate release information as well as generate an audit trail

All code is to be checked into a version control system on a regular basis.  More frequent check-ins provide benefits:
- Team members can synchornize changes reducing efforts to code merge and staying up to date on changes
- Continous integration builds can be triggered to verify code

[Learning git](https://docs.microsoft.com/en-us/azure/devops/learn/git/what-is-git)


## Coding Standards
Developers will follow the appropriate coding standards for the languages being used.  Coding standards can be found at the following locations:

* [Phillips 66 C# Coding Standard](https://phillips66.sharepoint.com/sites/IT_AE66PMO/Digital%20Capability/Shared%20Documents/2.2%20-%20IN%20-%20Business%20Improvement/WS%20-%20DCX/Working%20Documents/10.%20Development%20Standards/Phillips%2066%20C%20Sharp%20Coding%20Standard.docx?d=wde85208bab2c413e9412f255d55f1ce6&csf=1&e=LaONdH)
* [Phillips 66 JavaScript Coding Standard](https://phillips66.sharepoint.com/sites/IT_AE66PMO/Digital%20Capability/Shared%20Documents/2.2%20-%20IN%20-%20Business%20Improvement/WS%20-%20DCX/Working%20Documents/10.%20Development%20Standards/Phillips%2066%20JavaScript%20Coding%20Standard.docx?d=w13ff595b078a47bca4db00eed07d381f&csf=1&e=SzX6fp)
* [Phillips 66 SQL Coding Standard](https://phillips66.sharepoint.com/sites/IT_AE66PMO/Digital%20Capability/Shared%20Documents/2.2%20-%20IN%20-%20Business%20Improvement/WS%20-%20DCX/Working%20Documents/10.%20Development%20Standards/Phillips%2066%20SQL%20Coding%20Standard.docx?d=wf8d0654de3f1478ea6bb410a5f3f4515&csf=1&e=tfbVRr)

## Unit Tests
Unit tests will be developed for each user story that comply with the [Phillips 66 Automated Unit Testing Guidelines](https://phillips66.sharepoint.com/sites/IT_AE66PMO/Digital%20Capability/Shared%20Documents/2.2%20-%20IN%20-%20Business%20Improvement/WS%20-%20DCX/Working%20Documents/10.%20Development%20Standards/Phillips%2066%20Automated%20Unit%20Testing%20Guidelines.docx?d=w0b8bf2bafdf942f99230863565fe36da&csf=1&e=aHc8nb) and are adequate to test the feature.

All features will be developed in accordance with the DCX architectural guidelines.  Specifically, the [Application Stacks (Development Framework)](/Project-Artifacts/Architectural-Overview/Application-Stacks-\(Development-Framework\)) guidelines and the [Backup and DR Strategy](/Project-Artifacts/Architectural-Overview/Backup-and-DR-Strategy).


