namespace TirsvadCLI.PfxCertificate.Model;

public class CertificateInfo
{
    public string CertificatePath { get; private set; }
    public string CertificateFilename { get; private set; }
    public string CertificatePassword { get; private set; }
    public string CertSubject { get; private set; }
    public string CertOrganization { get; private set; }
    public string CertOrganizationUnit { get; private set; }
    public string CertLocality { get; private set; }
    public string CertState { get; private set; }
    public string CertCountry { get; private set; }
    public int CertValidityYears { get; private set; }
    public bool InstallInStore { get; private set; }
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
    public void SetCertificateFilename(string certificateFilename)
    {
        CertificateFilename = certificateFilename;
    }
    public void SetCertificatePassword(string certificatePassword)
    {
        CertificatePassword = certificatePassword;
    }
    public void SetCertSubject(string certSubject)
    {
        CertSubject = certSubject;
    }
    public void SetCertOrganization(string certOrganization)
    {
        CertOrganization = certOrganization;
    }
    public void SetCertOrganizationUnit(string certOrganizationUnit)
    {
        CertOrganizationUnit = certOrganizationUnit;
    }
    public void SetCertLocality(string certLocality)
    {
        CertLocality = certLocality;
    }
    public void SetCertState(string certState)
    {
        CertState = certState;
    }
    public void SetCertCountry(string certCountry)
    {
        CertCountry = certCountry;
    }
    public void SetCertValidityYears(int certValidityYears)
    {
        CertValidityYears = certValidityYears;
    }
    public void SetInstallInStore(bool installInStore)
    {
        InstallInStore = installInStore;
    }
}
