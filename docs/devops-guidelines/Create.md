[[_TOC_]]

Specific tools used to create the source and related artifacts will be determined by a project team.

These guidelines are intended to be general guidance and will be applicable to most teams.

# Managing Artifacts
For technical and software projects, a variety of artifacts are created. All artifacts should be maintined in git based repos 

- Design artifacts such as high level designs, information architecutre, wire frames and similar should be committed to source and associated with a User Story whenever possible
- Software developers should create code in a branch created from a User Story or Bug/Defect

todo: Branching Strategies
todo: Repository Structure

**Capabilities Inventory**

|Capability|Ideal State|Typical Tools|
|--|--|--|
|Design|Designs are driven by user stories, captured ahead of development and vetted against a Definition of Ready (DoR) for design acceptance.  Design documents incorporate all aspects of the design, to include at a minimum UX/UI, infrastructure, data models, logic and integration. Design may optionally include a clickable prototype or a proof of concept.|Adobe XD, Zeplin, Adobe Photoshop, Adobe Illustrator|
|Implementation|Developers work on tasks tied to user stories.  Unit tests are developed for acceptance criteria defined in the design ahead of development of the actual product.  Tasks are completed in priority order.  Development is accelerated by the use of IDEs and feedback tools.|Visual Studio, Visual Studio Code, MuleSoft, SQL Server Management Studio, Oracle SQL Developer, PL/SQL Developer, Resharper
|Version Control|All artifacts are versioned and revisions to those artifacts are tracked by a version control system (git).  Merges are simple and reversion to a "known-good state" is easy.  The version control system supports distributed development and forms the backbone for the build/deployment pipelines.|GitHub, git based Azure DevOps Repos, et.al. |

# Design
Designs typically consist of wireframes, mockups or prototypes. 
- A wireframe is a low-fidelity visual representation of the site and will be used to depict the basic UI, detailing the layout and structure of the site. 
- A mockup is a mid to high-fidelity visual representation of the site and will be used to highlight color scheme, visual style and typography. 
- A prototype is a high-fidelity visual representation of the site and will be used to simulate user interaction by allowing the user to experience the content through clickable elements and interactions. 

## Design Guidelines
Use the design that meets your needs. We recommend starting with wireframes, using mockups and prototypes when necessary and their value is clear. 

- Designs should utilize an existing design system such as Material Design, as well as standardized componenents such as Font Awesome Icons and custom components.
- Do not develop eponymous designs unless they are truly required for differentiation. 
- Beware existing or complementary solutions.
- Regardless of design, usability should not be sacrified: value something that works well over somthing that looks pretty
- When possible, include user testing in design review. 

Designs should be reviewed by stakeholders and users, as well as the design team, development team and business analysts before finalization. User stories, acceptance criteria and UX personas are used to approve and validate designs.

# Implementation
Implementation is where the rubber meets the road.  The plans are laid.  The user stories are written.  The test cases have been identified and it's finally time to build something.

Implementation will follow the SCRUM methodology and utilize Test Driven Development (TDD).  All features will be logged as user stories with acceptance criteria before they are worked.

All development is expected to be performed using Visual Studio Code, Mulesoft Anypoint Studio and Eclipse.

## Source Management
Git repos in Azure DevOps are preferred. 

Associate commits using user stories using the syntax "#<work item number> ..." in commit messsages to track which user story the commit is for. This generates a source audit trail and can be used by release systems to track which stories are being delivered in a release.

Code should be committed on a regular basis. More frequent commits provide benefits:
- Team members can merge committed changes more often reducing overall merge effort
- Continous integration builds can be triggered to verify code when it is committed

## Coding Standards
Developers will follow the appropriate coding standards for the languages being used.  Coding standards can be found at 

## Unit Tests
Unit tests will be developed for each user story that comply with the and are adequate to test the feature.


