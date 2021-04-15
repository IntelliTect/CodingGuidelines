[[_TOC_]]

Project code and artifacts are built locally using tools dictated by the software architecture of a project.

Regardless of architecture, projects should automated builds to support *Continuous Integration* or *CI*, and, automated deployment to staged environments called [Rings](point to Rings defintion)

# Continuous Integration

# Continous Deployment

# Ring Deployment

*** insert favorite Ring picture here ***

## Ring 1, Development
Development testing, verifying services, proof of concepts and development spikes
Complete access to the enviroment
Restricted access to P66 systems

## Ring 2, Integration Test
Continues integration
Deployed to automatically after successful builds
Includes automated integration tests

## Ring 3, Verify
Deployed to on succesful builds in Ring 1

## Ring 4, Stage
Similar/same as Production
May include a copy of Production data
Used when manual verification by Stakeholders or Customers is required 

## Ring 5, Launch
Production environment
Requires an approved ServiceNow CR prior to deployment
