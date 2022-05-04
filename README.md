# Nashtech Online Asset Management Application - :ferris_wheel:

[[_TOC_]]

## 1. Introduction
Nash Tech is expert in technology, delivering smart solutions that solve business challenges and create value. Since 2000, we has been delivering software development, cloud services, managed services, digital transformation and BPS from our development centers in Vietnam. Part of the Harvey Nash Group, we operate in the UK, Europe, USA, Australia, Singapore, Vietnam and Japan.

We have more than thousand employees. To support our employees to provide a million beautiful code lines per day, we have to provide them many devices such as PC, laptop, monitor. Therefore, we need an asset management system to manage our assets and record which asset is assigned to who.

We would like to establishment a project call ‘Online Asset Management’ which is a web based application. 

## 2. Usecase Model

![use-case-model.png](/.attachments/use-case-model-08226683-6b22-45e0-99bf-e9898daaab1c.png)

### 2.1 **Administrator** is responsible for registering with the tool and creating a profile. 
- **Create User:** Admin logs into the OPM and create a user profile.
- **Create Asset:** Admin logs into the OPM and create an asset data.
- **Assign Asset to User:** After creating an asset, the Admin assign it to a Staff.
- **View Assignment History of Asset:** Admin logs into the OPM and view an asset has been assigned to who, when, by who, etc.
- **Accept Asset Back:** After an asset is marked as return, the Admin accept it back.
- **Generate Report:** Admin logs into the OPM and get report in CSV format file type.

### 2.2 **Staff** is responsible for the asset which is assigned to.
- **Accept Asset:** After an asset is assigned to a Staff. Staff logs into the OPM and accept this assignment.
- **Decline Asset:** After an asset is assigned to a Staff. Staff logs into the OPM and decline this assignment.
- **Request to return Asset:** After an asset is accepted by a Staff, the Staff request to return this asset back to Admin by marking it as return.
- **View Assignment History of Asset:** Staff logs into the OPM and view history data of an asset that is accepted.


## 3. Architecture Goals
### 3.1 High level architecture

![Client Feedback SAD.jpg](/.attachments/Client%20Feedback%20SAD-663fb990-52f3-45b2-a04b-281a73f82c4d.jpg)

**Summary**
- **Azure App Service**: used to host React Frontend and ASP.Net Core API backend.
- **Azure SQL Server**: used for data storage.

## 4. OS, SDK, library, tooling and prerequisites
#### 4.1 Infrastructure

- **`Windows 10`** - The OS for developing and building this demo application.

#### 4.2 Back-end

- **[`.NET Core 5`](https://dotnet.microsoft.com/download)** - .NET Framework and .NET Core, including ASP.NET and ASP.NET Core
- **[`IdentityServer4`](https://identityserver.io)** - Identity and Access Control solution for .NET Core
- **[`EF Core`](https://github.com/dotnet/efcore)** - Modern object-database mapper for .NET. It supports LINQ queries, change tracking, updates, and schema migrations
- **[`serilog`](https://github.com/serilog/serilog)** - Simple .NET logging with fully-structured events

#### 4.3 Front-end

- **[`nodejs 10.x`](https://nodejs.org/en/download)** - JavaScript runtime built on Chrome's V8 JavaScript engine
- **[`typescript`](https://www.typescriptlang.org)** - A typed superset of JavaScript that compiles to plain JavaScript
- **[`create-react-app`](https://facebook.github.io/create-react-app)** - A modern web app by running one command

## 5. CI/CD

![architecture-devops-dotnet-webapp.svg](/.attachments/architecture-devops-dotnet-webapp-4fbba711-8e89-4faf-a6f2-4383bfab237e.svg)

The data flows through the scenario as follows:

1. A developer changes application source code.
2. Application code including the web.config file is committed to the source code repository in Azure Repos.
3. Continuous integration triggers application build and unit tests using Azure Test Plans.
4. Continuous deployment within Azure Pipelines triggers an automated deployment of application artifacts with environment-specific configuration values.
5. The artifacts are deployed to Azure App Service.
6. Azure Application Insights collects and analyzes health, performance, and usage data.
7. Developers monitor and manage health, performance, and usage information.
8. Backlog information is used to prioritize new features and bug fixes using Azure Boards.

Note: the above flow are reference from microsoft resource **[`Design a CI/CD pipeline using Azure DevOps`](https://docs.microsoft.com/en-us/azure/architecture/example-scenario/apps/devops-dotnet-webapp)**

## 6. Contributing

1. Fork it!
2. Create your feature branch: `git checkout -b my-new-feature`
3. Commit your changes: `git commit -am 'Add some feature'`
4. Push to the branch: `git push origin my-new-feature`
5. Submit a pull request :p

## Contributors ✨

Thanks goes to these wonderful people ([emoji key](https://allcontributors.org/docs/en/emoji-key))

<table>
  <tbody>
    <tr>
      <td align="center" valign="top">
        <img width="150" height="150" src="https://avatars.githubusercontent.com/u/289607?v=4&s=150" alt="avartar">
        <br>
        <a href="https://github.com/thiennn">Thien Nguyen</a>
      </td>
      <td align="center" valign="top">
        <img width="150" height="150" src="https://avatars.githubusercontent.com/u/7325455?v=4" alt="avartar">
        <br>
        <a href="https://github.com/phongnguyend">Phong Nguyen</a>
      </td>
     </tr>
  </tbody>
</table>
