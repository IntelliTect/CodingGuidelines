[[_TOC_]]

Project code and artifacts are built locally using tools dictated by the software architecture of a project.

Regardless of architecture, projects should automated builds to support *Continuous Integration* or *CI*, and, automated deployment to staged environments

# Continuous Integration
Build the solution on every commit. Build should inluding executing and passing all unit tests as well as security code scans.

# Continous Deployment
When software is succesfully built, it should be deployed immediately. It is most common to deploy in stages so that deploymnets can be verified and tested.

|Stage | Description |
|-------|----------|
|Dev |Deploy to dev on every successful build of the system. Run integration tests.|
|Test |Proceeds whenever Dev passes all tests. Additional integration tests are performed. In some cases, manual tests or automated UI tests are executed, or, 3rd party test systems are included in integration tests|
|Pre production|As close to production as reasonable. May have a copy of production data to test migrations on and/or run additional production tests|
|Production|final production environment or environments | 

## Ring Deployment Model

Often, deployment to Production isn't deployment to a single instance of a system. when multiple systems exist, supporting multiple regions around the world for example, a ringed deployment should be used. This allows for controlled rollouts and limits the impact of deployment issues only discovered as 

Supports rolling and red/green deployments.