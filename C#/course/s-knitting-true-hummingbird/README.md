# Knitting
This repository can be cloned to your local workstation in order to resolve the `Knitting` problem.

## Prerequisite
If this is your first code using Azure DevOps, install the [Azure Artifacts Credential Provider](https://github.com/microsoft/artifacts-credprovider#installation-on-windows). The Azure Artifacts Credential Provider automates the acquisition of credentials needed to restore NuGet packages as part of your .NET development workflow.

## Getting Started
    ```
    ```
- Clone this repository. This will create the repository locally.
    ```
    ```
- Navigate to the repository
    ```
    cd <repository_name>
    ```
- Create a branch from the main branch to start working (e.g. `homework`)
    ```
    git checkout -b homework
    ```
- Open the `Knitting.sln` solution in your favorite IDE
- Build the solution to make sure everything is well setup

## Submitting the assignment
Depending on the assignment, there will be specific criteria on what needs to be provided. In any case you will need to push your changes and then open a pull request.
To commit your changes, start from your repository and run the following commands:
- Prepare the changes for git:
```
git add . 
```
- Wrap your changes in a commit
```
git commit -m "<my_commit_message>"
```
- Push your changes to its `origin` which is your repo in Azure DevOps (e.g. yuor branch name is `homework`)
```
git push --set-upstream origin homework
```


> ℹ️ You can submit your assignment as many time as you want until the due date. Past the due dates, submissions will still work but won't be taken into account.

## Getting feedback on your assignment

Past the due date, the teacher assistants will start grading your assignments. Watch for their commit in your repository, the grades will be delivered as a `Grading comments.md` file in your repository.
