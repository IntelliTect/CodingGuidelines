---

# Introduction

One of the bigger challenges to building software is managing the lifecycle of the source code, from authorship to deployment. Below is an outline of how to structure our source code, the goal of which is to achieve the following:

- Any change must be easily reviewable, including how the change affects dependencies.
- Building and deploying should be simple, fast, and reproducible.
- Changes should be testable in both isolation and integrated with the existing environment.
- Setting up new users/systems should require as few steps as possible and require little outside assistance.

---

# Review

There are generally two approaches when setting up source control for a project:

- Each application's code remains in its own repository
- All code is in one repository (monorepo)

Neither implementation is objectively better than the other; they are both highly dependent on the complexity of the application and delivery model. Below is a review of the benefits and challenges that come with each approach. 


## <u>Separate Repositories</u>

### Benefits

- Organization  
The structure of the repo is simple; it contains the source files and configuration from only one project.
- Isolation  
Each project exists in isolation. Changes can be made, and subsequently reviewed, knowing they have no direct or immediate impact on anything else.

### Challenges

- Dependencies
Coordinating changes between multiple, dependent projects is a tedious process. It is made worse over a project's lifecycle as it naturally grows in complexity. One example of this would be a web app that is dependent on a component library. When the app requires a change to a component, the dependent component repo must go through its code review and release process before the web app is able to move forward. If there are further changes required of the updated component library, the cycle starts over and the delay increases.
- Deployments
The ability to automate build and release pipelines suffers when the dependency graph grows in complexity. Coordinating deployments requires a significant effort from the team to track interdependent versions and where they should exist in the release cycle.


## <u>Single Repository (monorepo)</u>

### Benefits

- Organization
All files exist in one, well-structured directory. Users can easily search across projects without having to sync updates and branches across multiple repositories.
- Dependencies
Every project moves forward together. Changes in dependencies should never block progress since review cycles and upstream integration never happen orthogonally.
- Tooling
Most applications and services operate on the file system. Providing immediate access to all required files greatly improves the speed, efficiency, and resiliency of the CI/CD pipelines.

### Challenges

- Scalability
The life of a repository is long-lived. The VCS index continues to grow with every new file or changed line. While there are ways to mitigate this, the size of the repo will inevitably grow.

---

# Model Repository Implementation

Our project is comprised of multiple slices of related functionality and will be developed in parallel by a distributed team. The current approach will maintain a single repository ("monorepo") containing top-level directories for each independent project.

The structure of the repository will be similar to the following example:

- <u>**Root**</u> (repo directory)
  - Repo-level config files: e.g. `.gitignore`, `.gitattributes`, `.editorconfig`
  - Makefile  (or similar) to provide top-level hooks into available scripts
  - <u>**docs**</u>
    - README.md
    - installation.md
  - <u>**scripts**</u> (utilities for cross-project coordination)
    - README.md
    - Makefile
    - pipeline
      - build.sh
      - test.sh
    - utils
      - setup_machine.sh
  - <u>**infrastructure**</u>
    - README.md
    - Makefile
    - env
      - ring1
      - ring2
    - modules
      - lambda
      - s3
  - <u>**frontend**</u>
    - Project-level config files: e.g. `.gitignore`, `.npmrc`, `setup.cfg`
    - docs
      - README.md
      - installation.md
    - scripts
      - build.sh
      - test.sh
    - src
    - tests
  - <u>**api**</u>
  - <u>**shared_components**</u>
  - <u>**bloomreach**</u>


---

