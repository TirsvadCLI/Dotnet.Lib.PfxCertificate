﻿[![NuGet Downloads][nuget-shield]][nuget-url]
[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]
[![LinkedIn][linkedin-shield]][linkedin-url]

# ![Logo][Logo] Pfx Certificate Manager

## Overview
Pfx Certificate Manager is a .NET tool designed to simplify the creation, management, and installation of self-signed certificates for use in development environments. It provides an easy-to-use API for generating certificates and managing their lifecycle.

## Table of Contents
- [Overview](#overview)
- [Requirements](#requirements)
- [Features](#features)
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

## Requirements
- .NET 9 or later
- Windows operating system (for certificate store operations)

## Usage
1. **Install the Tool**  
   Clone the repository and build the project targeting `.NET 9`.  
   Example:
    ```powershell
    dotnet build
    ```
2. **Generate a Certificate**  
   Use the `CreateCertificateAsync` method to generate and optionally install a certificate:
    ```csharp
    var certificateInfo = new CertificateInfo( "path/to/certificates", "myCert.pfx", "MyPassword123!", "MySubject", "MyOrganization", "MyOrgUnit", "MyLocality", "MyState", "US", 1, installInStore: true);
    var nugetCertificate = new NugetCertificate(certificateInfo); await nugetCertificate.CreateCertificateAsync();
    ```
3. **Remove a Certificate**  
    Use the `RemoveCertificateFromStoreAsync` method to remove a certificate by subject:
    ```csharp
    await NugetCertificate.RemoveCertificateFromStoreAsync("MySubject");
    ```

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
  Navigate to the [GitHub Issues page](https://github.com/TirsvadCLI/TirsvadCLI.Lib.PfxCertificateManager/issues).  

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
<!-- MARKDOWN LINKS & IMAGES -->
[contributors-shield]: https://img.shields.io/github/contributors/TirsvadCLI/TirsvadCLI.Lib.PfxCertificateManager?style=for-the-badge
[contributors-url]: https://github.com/TirsvadCLI/TirsvadCLI.Lib.PfxCertificateManager/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/TirsvadCLI/TirsvadCLI.Lib.PfxCertificateManager?style=for-the-badge
[forks-url]: https://github.com/TirsvadCLI/TirsvadCLI.Lib.PfxCertificateManager/network/members
[stars-shield]: https://img.shields.io/github/stars/TirsvadCLI/TirsvadCLI.Lib.PfxCertificateManager?style=for-the-badge
[stars-url]: https://github.com/TirsvadCLI/TirsvadCLI.Lib.PfxCertificateManager/stargazers
[issues-shield]: https://img.shields.io/github/issues/TirsvadCLI/TirsvadCLI.Lib.PfxCertificateManager?style=for-the-badge
[issues-url]: https://github.com/TirsvadCLI/TirsvadCLI.Lib.PfxCertificateManager/issues
[license-shield]: https://img.shields.io/github/license/TirsvadCLI/TirsvadCLI.Lib.PfxCertificateManager?style=for-the-badge
[license-url]: https://github.com/TirsvadCLI/TirsvadCLI.Lib.PfxCertificateManager/blob/master/LICENSE
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://www.linkedin.com/in/jens-tirsvad-nielsen-13b795b9/
[nuget-shield]: https://img.shields.io/nuget/dt/TirsvadCLI.PfxCertificateManager?style=for-the-badge
[nuget-url]: https://www.nuget.org/packages/TirsvadCLI.PfxCertificateManager/

[Logo]: https://raw.githubusercontent.com/TirsvadCLI/TirsvadCLI.Lib.PfxCertificateManager/master/image/logo/32x32/logo.png
