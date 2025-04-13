namespace TirsvadCLI.NugetCertificate.Model;

public class CertificateInfo
{
    public string CertificatePath { get; }
    public string CertificateFilename { get; }
    public string CertificatePassword { get; }
    public string CertSubject { get; }
    public string CertOrganization { get; }
    public string CertOrganizationUnit { get; }
    public string CertLocality { get; }
    public string CertState { get; }
    public string CertCountry { get; }
    public int CertValidityYears { get; }
    public bool InstallInStore { get; set; }
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
}
