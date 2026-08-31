**Defect Tracking and Resolution**	

Defect should be tracked in the same way as User Stories. If appropriate, link the original user story to the defect. 

Defects generated from automated testing tool/platform, when validated, should be recorded the same as any ohter defect.

Defects are reviewed and planned with other user stories on the backlog .

If a defect is high priority, process shoudl be followed but it may be expedited: Add the defect to the backlog, create a branch for the defect, write unit tests for the defect, fix the code, push the branch, create  PR, when approved. Existing CI/CD pipelines should then deploy the fix to production.
