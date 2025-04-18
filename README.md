[![NuGet Downloads][nuget-shield]][nuget-url][![Contributors][contributors-shield]][contributors-url][![Forks][forks-shield]][forks-url][![Stargazers][stars-shield]][stars-url][![Issues][issues-shield]][issues-url][![License][license-shield]][license-url][![LinkedIn][linkedin-shield]][linkedin-url]

# ![Logo][Logo] Pfx Certificate Library

## Overview
Pfx Certificate Library is a .NET tool designed to simplify the creation, management, and installation of self-signed certificates for use in development environments. It provides an easy-to-use API for generating certificates and managing their lifecycle.

## Table of Contents
- [Overview](#overview)
- [Requirements](#requirements)
- [Features](#features)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
    - [Install via NuGet Package Manager Console](#install-via-nuget-package-manager-console)
    - [Install via Visual Studio NuGet Package Manager](#install-via-visual-studio-nuget-package-manager)
    - [Install via .NET CLI](#install-via-dotnet-cli)
    - [Clone the repo](#clone-the-repo)
- [Usage](#usage)
- [Contributing](#contributing)
- [Bug / Issue Reporting](#bug--issue-reporting)
- [License](#license)
- [Contact](#contact)
- [Acknowledgments](#acknowledgments)

## Features
- Generate self-signed certificates with customizable properties.
- Export certificates to `.pfx` files with private keys.
- Install certificates into the local certificate store.
- Remove certificates from the store by subject name.
- Fully asynchronous API for modern .NET applications.

## Getting Started
To get started with the Form library, you will need to install the library in your .NET project. You can do this using NuGet Package Manager or by adding the package reference directly to your project file.

### Prerequisites
- .NET 9.0 or later

## Installation
To use the library, you will need to download and then add a reference to the library in your project. Follow the instructions below to install the library and get started.

#### Install via NuGet Package Manager Console
You can install the library using the NuGet Package Manager Console. Open the console and run the following command:
```bash
Install-Package TirsvadCLI.PfxCertificateManager
```

#### Install via Visual Studio NuGet Package Manager
1. Open your project in Visual Studio.
2. Right-click on your project in the Solution Explorer and select "Manage NuGet Packages".
3. Search for "TirsvadCLI.PfxCertificateManager" in the NuGet Package Manager.
4. Click "Install" to add the library to your project. 

#### Install via .NET CLI
You can also install the library using the .NET CLI. Open a terminal and run the following command:
```bash
dotnet add package TirsvadCLI.PfxCertificateManager
```

#### Clone the repo
![Repo size][repos-size-shield]

If you want to clone the repository and build the library from source, you can do so using Git. Make sure you have Git installed on your machine. Then, run the following command in your terminal:

```bash
git clone git@github.com:TirsvadCLI/Dotnet.Lib.PfxCertificateManager.git
```

## Usage
1. **Create a Self-Signed Certificate**  
   Use the `CreateCertificateAsync` method to generate a self-signed certificate and save it as a `.pfx` file:
    ```csharp
    await CertificateManager.CreateCertificateAsync( 
        commonName: "MySubject",          // Common Name (CN) 
        organization: "MyOrganization",   // Organization (O)
        organizationUnit: "MyOrgUnit",    // Organizational Unit (OU) 
        country: "US",                    // Country (C)
        state: "MyState",                 // State (S)
        locality: "MyLocality",           // Locality (L)
        password: "MyPassword123!"        // Password for the .pfx file );
    ```
2. **Add a Certificate to the Store**  
   Use the `AddCertificateAsync` method to add a `.pfx` certificate to a specific certificate store:
    ```csharp
    await CertificateManager.AddCertificateAsync( 
        pfxPath: "MySubject.pfx",        // Path to the .pfx file 
        password: "MyPassword123!",      // Password for the .pfx file 
        storeName: StoreName.My,         // Certificate store name (e.g., My, Root) 
        storeLocation: StoreLocation.CurrentUser // Store location (e.g., CurrentUser, LocalMachine) );
    ```
3. **Remove a Certificate from the Store**  
   Use the `RemoveCertificateAsync` method to remove a certificate from a specific store by its thumbprint:
   ```csharp
    await CertificateManager.RemoveCertificateAsync( 
         thumbprint: "THUMBPRINT",        // Thumbprint of the certificate 
         storeName: StoreName.My,        // Certificate store name (e.g., My, Root) 
         storeLocation: StoreLocation.CurrentUser // Store location (e.g., CurrentUser, LocalMachine) );
    ```
4. **Display Certificate Details**  
   The `DisplayCertificateDetails` method is used internally to print certificate details to the console. You can use it to inspect certificates:
   ```csharp
   var cert = new X509Certificate2("MySubject.pfx", "MyPassword123!");
   CertificateManager.DisplayCertificateDetails(cert);
   ```
   
### Notes:
- The `CreateCertificateAsync` method generates a self-signed certificate valid for 5 years by default.
- The `AddCertificateAsync` and `RemoveCertificateAsync` methods are only supported on Windows for managing the certificate store.
- Ensure you have the necessary permissions to access the certificate store when using `AddCertificateAsync` or `RemoveCertificateAsync`.

These examples demonstrate how to use the core features of the `CertificateManager` class. For more advanced usage, refer to the source code or extend the functionality as needed.
   
## Contributing
Contributions are what make the open-source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
   - **Note**: Before committing, ensure you have created appropriate tests for your changes. This helps maintain the quality and reliability of the project.
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## Bug / Issue Reporting  
If you encounter a bug or have an issue to report, please follow these steps:  

1. **Go to the Issues Page**  
  Navigate to the [GitHub Issues page](https://github.com/TirsvadCLI/Dotnet.Lib.PfxCertificateManager/issues).  

2. **Click "New Issue"**  
  Click the green **"New Issue"** button to create a new issue.  

3. **Provide Details**  
  - **Title**: Write a concise and descriptive title for the issue.  
  - **Description**: Include the following details:  
    - Steps to reproduce the issue.  
    - Expected behavior.  
    - Actual behavior.  
    - Environment details (e.g., OS, .NET version, etc.).  
  - **Attachments**: Add screenshots, logs, or any other relevant files if applicable.  

4. **Submit the Issue**  
  Once all details are filled in, click **"Submit new issue"** to report it.  

Your feedback is valuable and helps improve the project!

## License
Distributed under the GPL-3.0 [License][license-url].

## Contact
Jens Tirsvad Nielsen - [LinkedIn][linkedin-url]

## Acknowledgments
- [dotnet](https://dotnet.microsoft.com/)

<!-- MARKDOWN LINKS & IMAGES -->
[contributors-shield]: https://img.shields.io/github/contributors/TirsvadCLI/Dotnet.Lib.PfxCertificateManager?style=for-the-badge
[contributors-url]: https://github.com/TirsvadCLI/Dotnet.Lib.PfxCertificateManager/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/TirsvadCLI/Dotnet.Lib.PfxCertificateManager?style=for-the-badge
[forks-url]: https://github.com/TirsvadCLI/Dotnet.Lib.PfxCertificateManager/network/members
[stars-shield]: https://img.shields.io/github/stars/TirsvadCLI/Dotnet.Lib.PfxCertificateManager?style=for-the-badge
[stars-url]: https://github.com/TirsvadCLI/Dotnet.Lib.PfxCertificateManager/stargazers
[issues-shield]: https://img.shields.io/github/issues/TirsvadCLI/Dotnet.Lib.PfxCertificateManager?style=for-the-badge
[issues-url]: https://github.com/TirsvadCLI/Dotnet.Lib.PfxCertificateManager/issues
[license-shield]: https://img.shields.io/github/license/TirsvadCLI/Dotnet.Lib.PfxCertificateManager?style=for-the-badge
[license-url]: https://github.com/TirsvadCLI/Dotnet.Lib.PfxCertificateManager/blob/master/LICENSE
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://www.linkedin.com/in/jens-tirsvad-nielsen-13b795b9/
[repos-size-shield]: https://img.shields.io/github/repo-size/TirsvadCLI/Dotnet.Lib.PfxCertificateManager?style=for-the-badg

[nuget-shield]: https://img.shields.io/nuget/dt/TirsvadCLI.PfxCertificateManager?style=for-the-badge
[nuget-url]: https://www.nuget.org/packages/TirsvadCLI.PfxCertificateManager/

[Logo]: https://raw.githubusercontent.com/TirsvadCLI/Dotnet.Lib.PfxCertificateManager/master/image/logo/32x32/logo.png
