[![Open in Visual Studio Code](https://classroom.github.com/assets/open-in-vscode-718a45dd9cf7e7f842a935f5ebbe5719a5e09af4491e668f4dbf3b35d5cca122.svg)](https://classroom.github.com/online_ide?assignment_repo_id=13669283&assignment_repo_type=AssignmentRepo)

# Application Architecture Overview

![Solution Diagram.jpg](https://github.com/Naserien/519web-deploy/blob/main/Solution%20Diagram.jpg)

### Components:

- **GitHub Workflows**: Automates the deployment process through CI/CD pipelines using GitHub Actions. It handles tasks such as building, testing, and deploying the application.

- **Container Registry**: A storage system for Docker images. After the application is built, the image is pushed here.

- **Container App**: A service that manages and scales the containerized application, pulling the Docker image from the Container Registry.

- **Function App**: A serverless compute service that runs event-triggered code, which does not require managing infrastructure.

- **Web App**: The interface for users, potentially a front-end service like WordPress, interacting with the Container App and Function App.
  
- **Key Vault**: Secures sensitive information such as secrets, tokens, and certificates required by the Container App, Function App, and Web App.
  
- **Storage Queue**: Manages communication between services asynchronously, mainly used for processing tasks queued by the Function App.

- **Resource Group**: In Azure, it's a collection that holds related resources for an Azure solution, including the Web App, Function App and Container app.

- **Users**: The end-users accessing the Web App.

### Data Flow

1. **Code Push & CI/CD Trigger**: Developers push code to GitHub, triggering the GitHub Workflows for CI/CD.

2. **Build & Image Push**: Builds a Docker image and pushes it to the Container Registry.

3. **Deployment**: Deploys the Docker image to the Container App and configures the Function App and Key Vault.

4. **Service Interaction**:
   - The Web App communicates with the Container App to serve content.
   - The Function App may place tasks in the Storage Queue.
   - The Container App may process tasks or messages from the Storage Queue.

5. **Resource Management**: All services are managed within an Azure Resource Group for centralized governance.

### Notations:

- "N/S" indicates Network/Security, marking where network traffic and security are crucial.

## Getting Started

To work with this application architecture:

1. **Permissions**: Verify access to the GitHub repository, Container Registry, Key Vault, Storage Queue, and Azure Resource Group.

2. **Local Development**: Clone the repository and set up your local environment following the project's guidelines.

3. **Deployment**: Use the GitHub Workflows documentation for CI/CD guidance. Securely manage your environment variables and secrets in Key Vault.

4. **Monitoring & Maintenance**: After deployment, monitor application health with Azure's services. Storage Queue insights will inform on job statuses and potential issues.

For a detailed breakdown of each component, refer to the Azure's service documentation.
