namespace TirsvadCLI.PfxCertificate.Model;

/// <summary>
/// CertificateInfo class implements ICertificateInfo interface
/// This class is used to store the information about the certificate
/// </summary>
public class CertificateInfo : ICertificateInfo
{
    public string CertificatePath { get; private set; } ///> The path to the certificate file
    public string CertificateFilename { get; private set; } ///> The filename of the certificate file
    public string CertificatePassword { get; private set; } ///> The password for the certificate file
    public string CertSubject { get; private set; } ///> The subject of the certificate
    public string CertOrganization { get; private set; } ///> The organization of the certificate
    public string CertOrganizationUnit { get; private set; } ///> The organization unit of the certificate
    public string CertLocality { get; private set; } ///> The locality of the certificate
    public string CertState { get; private set; } ///> The state of the certificate
    public string CertCountry { get; private set; } ///> The country of the certificate
    public int CertValidityYears { get; private set; } ///> The validity years of the certificate
    public bool InstallInStore { get; private set; } ///> Whether to install the certificate in the store or not
    public CertificateInfo(string certificatePath, string certificateFilename, string certificatePassword,
        string certSubject, string certOrganization, string certOrganizationUnit, string certLocality,
        string certState, string certCountry, int certValidityYears = 5, bool installInStore = false)
    {
        CertificatePath = certificatePath;
        CertificateFilename = certificateFilename;
        CertificatePassword = certificatePassword;
        CertSubject = certSubject;
        CertOrganization = certOrganization;
        CertOrganizationUnit = certOrganizationUnit;
        CertLocality = certLocality;
        CertState = certState;
        CertCountry = certCountry;
        CertValidityYears = certValidityYears;
        InstallInStore = installInStore;

        // Ensure the directory exists
        var certDirectory = Path.GetFullPath(CertificatePath);
        if (!Directory.Exists(certDirectory))
        {
            Directory.CreateDirectory(certDirectory);
        }
    }
    /// <summary>
    /// Set the certificate path
    /// </summary>
    /// <param name="certificatePath"> The certificate path</param>
    public void SetCertificatePath(string certificatePath)
    {
        // Ensure the directory exists
        var certDirectory = Path.GetDirectoryName(certificatePath);
        if (!string.IsNullOrEmpty(certDirectory) && !Directory.Exists(certDirectory))
        {
            Directory.CreateDirectory(certDirectory);
        }
        // Set the full path for the certificate
        CertificatePath = Path.GetFullPath(certificatePath);
    }
    /// <summary>
    /// Set the certificate filename
    /// </summary>
    /// <param name="certificateFilename"> The certificate filename</param>
    public void SetCertificateFilename(string certificateFilename)
    {
        CertificateFilename = certificateFilename;
    }
    /// <summary>
    /// Set the certificate password
    /// </summary>
    /// <param name="certificatePassword"> The certificate password</param>
    public void SetCertificatePassword(string certificatePassword)
    {
        CertificatePassword = certificatePassword;
    }
    /// <summary>
    /// Set the certificate subject
    /// </summary>
    /// <param name="certSubject"> The certificate subject</param>
    public void SetCertSubject(string certSubject)
    {
        CertSubject = certSubject;
    }
    /// <summary>
    /// Set the certificate organization
    /// </summary>
    /// <param name="certOrganization"> The certificate organization</param>
    public void SetCertOrganization(string certOrganization)
    {
        CertOrganization = certOrganization;
    }
    /// <summary>
    /// Set the certificate organization unit
    /// </summary>
    /// <param name="certOrganizationUnit"> The certificate organization unit</param>
    public void SetCertOrganizationUnit(string certOrganizationUnit)
    {
        CertOrganizationUnit = certOrganizationUnit;
    }
    /// <summary>
    /// Set the certificate locality
    /// </summary>
    /// <param name="certLocality"> The certificate locality</param>
    public void SetCertLocality(string certLocality)
    {
        CertLocality = certLocality;
    }
    /// <summary>
    /// Set the certificate state
    /// </summary>
    /// <param name="certState"> The certificate state</param>
    public void SetCertState(string certState)
    {
        CertState = certState;
    }
    /// <summary>
    /// Set the certificate country
    /// </summary>
    /// <param name="certCountry"> The certificate country</param>
    public void SetCertCountry(string certCountry)
    {
        CertCountry = certCountry;
    }
    /// <summary>
    /// Set the certificate validity years
    /// </summary>
    /// <param name="certValidityYears"> The certificate validity years</param>
    public void SetCertValidityYears(int certValidityYears)
    {
        CertValidityYears = certValidityYears;
    }
    /// <summary>
    /// Set if the certificate should be installed in the store
    /// </summary>
    /// <param name="installInStore"> True if the certificate should be installed in the store, false otherwise</param>
    public void SetInstallInStore(bool installInStore)
    {
        InstallInStore = installInStore;
    }
    /// <summary>
    /// Check if the certificate path is valid
    /// </summary>
    /// <param name="certificatePath"> The certificate path</param>
    /// <returns>
    /// 0 = Valid certificate path<br/>
    /// 1 = Invalid certificate path, can't be null or empty<br/>
    /// 2 = Invalid certificate path too long<br/>
    /// </returns>
    public static int IsValidCertificatePath(string? certificatePath)
    {
        if (string.IsNullOrEmpty(certificatePath))
            return 1; // Invalid certificate path cannot be empty
        // Windows API has a limit of 260 characters for the path
        // including the drive letter and the null terminator
        // https://learn.microsoft.com/en-us/windows/win32/fileio/maximum-file-path-limitation
        if (OperatingSystem.IsWindows() && certificatePath.Length > 255)
            return 2; // Invalid certificate path too long
        return 0; // Valid certificate path
    }
    /// <summary>
    /// Check if the certificate filename is valid
    /// </summary>
    /// <param name="certificateFilename"> The certificate filename</param>
    /// <returns>
    /// 0 = Valid certificate filename<br/>
    /// 1 = Invalid certificate filename, can't be null or empty<br/>
    /// 2 = Invalid certificate filename, no extension<br/>
    /// 3 = Invalid certificate filename, not a pfx or p12 file extension<br/>
    /// 4 = Invalid certificate filename too long<br/>
    /// </returns>
    public static int IsValidCertificateFilename(string? certificateFilename)
    {
        if (string.IsNullOrEmpty(certificateFilename))
            return 1; // Invalid certificate filename cannot be empty
        string suffix = Path.GetExtension(certificateFilename);
        if (string.IsNullOrEmpty(suffix))
            return 2; // Invalid certificate filename, no extension
        if (suffix != ".pfx" && suffix != ".p12")
            return 3; // Invalid certificate filename, not a pfx or p12 file
        // Windows API has a limit of 260 characters for the path
        // including the drive letter and the null terminator
        if (OperatingSystem.IsWindows() && certificateFilename.Length > 255)
            return 4; // Invalid certificate filename too long
        return 0; // Valid certificate filename
    }
    /// <summary>
    /// Check if the certificate password is valid
    /// </summary>
    /// <param name="certificatePassword"> The certificate password</param>
    /// <returns>
    /// 0 = Valid certificate password<br/>
    /// 1 = Invalid certificate password, can't be null or empty<br/>
    /// 2 = Invalid certificate password too short<br/>
    /// 3 = Invalid certificate password too long<br/>
    /// </returns>
    public static int IsValidCertificatePassword(string? certificatePassword)
    {
        if (string.IsNullOrEmpty(certificatePassword))
            return 1; // Invalid certificate password cannot be empty
        if (certificatePassword.Length < 8)
            return 2; // Invalid certificate password too short
        if (certificatePassword.Length > 255)
            return 3; // Invalid certificate password too long
        return 0; // Valid certificate password
    }
    /// <summary>
    /// Check if the certificate subject is valid
    /// </summary>
    /// <param name="certSubject"> The certificate subject</param>
    /// <returns>
    /// 0 = Valid certificate subject<br/>
    /// 1 = Invalid certificate subject, can't be null or empty<br/>
    /// 2 = Invalid certificate subject too long<br/>
    /// </returns>
    public static int IsValidCertSubject(string? certSubject)
    {
        if (string.IsNullOrEmpty(certSubject))
            return 1; // Invalid certificate subject cannot be empty
        if (certSubject.Length > 255)
            return 2; // Invalid certificate subject too long
        return 0; // Valid certificate subject
    }
    /// <summary>
    /// Check if the certificate organization is valid
    /// </summary>
    /// <param name="certOrganization"> The certificate organization</param>
    /// <returns>
    /// 0 = Valid certificate organization<br/>
    /// 1 = Invalid certificate organization, can't be null or empty<br/>
    /// 2 = Invalid certificate organization too long<br/>
    /// </returns>
    public static int IsValidCertOrganization(string? certOrganization)
    {
        if (string.IsNullOrEmpty(certOrganization))
            return 1; // Invalid certificate organization cannot be empty
        if (certOrganization.Length > 255)
            return 2; // Invalid certificate organization too long
        return 0; // Valid certificate organization
    }
    /// <summary>
    /// Check if the certificate organization unit is valid
    /// </summary>
    /// <param name="certOrganizationUnit"> The certificate organization unit</param>
    /// <returns>
    /// 0 = Valid certificate organization unit<br/>
    /// 1 = Invalid certificate organization unit, can't be null or empty<br/>
    /// 2 = Invalid certificate organization unit too long<br/>
    /// </returns>
    public static int IsValidCertOrganizationUnit(string? certOrganizationUnit)
    {
        if (string.IsNullOrEmpty(certOrganizationUnit))
            return 1; // Invalid certificate organization unit cannot be empty
        if (certOrganizationUnit.Length > 255)
            return 2; // Invalid certificate organization unit too long
        return 0; // Valid certificate organization unit
    }
    /// <summary>
    /// Check if the certificate locality is valid
    /// </summary>
    /// <param name="certLocality">The certificate locality</param>
    /// <returns>
    /// 0 = Valid certificate locality<br/>
    /// 1 = Invalid certificate locality, can't be null or empty<br/>
    /// 2 = Invalid certificate locality too long<br/>
    /// </returns>
    public static int IsValidCertLocality(string? certLocality)
    {
        if (string.IsNullOrEmpty(certLocality))
            return 1; // Invalid certificate locality cannot be empty
        if (certLocality.Length > 255)
            return 2; // Invalid certificate locality too long
        return 0; // Valid certificate locality
    }
    /// <summary>
    /// Check if the certificate state is valid
    /// </summary>
    /// <param name="certState"> The certificate state</param>
    /// <returns>
    /// 0 = Valid certificate state<br/>
    /// 1 = Invalid certificate state, can't be null or empty<br/>
    /// 2 = Invalid certificate state too long<br/>
    /// </returns>
    public static int IsValidCertState(string? certState)
    {
        if (string.IsNullOrEmpty(certState))
            return 1; // Invalid certificate state cannot be empty
        if (certState.Length > 255)
            return 2; // Invalid certificate state too long
        return 0; // Valid certificate state
    }
    /// <summary>
    /// Check if the certificate country is valid
    /// </summary>
    /// <param name="certCountry"> The certificate country</param>
    /// <returns>
    /// 0 = Valid certificate country<br/>
    /// 1 = Invalid certificate country, can't be null or empty<br/>
    /// 2 = Invalid certificate country too long<br/>
    /// </returns>
    public static int IsValidCertCountry(string? certCountry)
    {
        if (string.IsNullOrEmpty(certCountry))
            return 1; // Invalid certificate country cannot be empty
        if (certCountry.Length > 255)
            return 2; // Invalid certificate country too long
        return 0; // Valid certificate country
    }
    /// <summary>
    /// Check if the certificate validity years is valid
    /// </summary>
    /// <param name="certValidityYears"> The certificate validity years</param>
    /// <returns>
    /// 0 = Valid certificate validity years<br/>
    /// 1 = Invalid certificate validity years, can't be null or empty<br/>
    /// 2 = Invalid certificate validity years, can't be empty<br/>
    /// 3 = Invalid certificate validity years, can't be less than 1<br/>
    /// 4 = Invalid certificate validity years, too long<br/>
    /// </returns>
    public static int IsValidCertValidityYears(string? certValidityYears)
    {
        if (string.IsNullOrEmpty(certValidityYears))
            return 1; // Invalid certificate validity years cannot be empty
        if (!int.TryParse(certValidityYears, out int certValidityYearsInt))
            return 2; // Invalid certificate validity years cannot be empty
        return IsValidCertValidityYears(certValidityYearsInt);
    }
    /// <summary>
    /// Check if the certificate validity years is valid
    /// </summary>
    /// <param name="certValidityYears"> The certificate validity years</param>
    /// <returns>
    /// 0 = Valid certificate validity years<br/>
    /// 3 = Invalid certificate validity years, can't be less than 1<br/>
    /// 4 = Invalid certificate validity years, too long<br/>
    /// </returns>
    public static int IsValidCertValidityYears(int certValidityYears)
    {
        if (certValidityYears < 1)
            return 3; // Invalid certificate validity years cannot be less than 1
        if (certValidityYears > 20)
            return 4; // Invalid certificate validity years too long
        return 0; // Valid certificate validity years
    }
}
