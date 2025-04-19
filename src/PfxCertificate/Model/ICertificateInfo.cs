namespace TirsvadCLI.PfxCertificate.Model
{
    interface ICertificateInfo
    {
        string CertificatePath { get; }
        string CertificateFilename { get; }
        string CertificatePassword { get; }
        string CertSubject { get; }
        string CertOrganization { get; }
        string CertOrganizationUnit { get; }
        string CertLocality { get; }
        string CertState { get; }
        string CertCountry { get; }
        int CertValidityYears { get; }
        bool InstallInStore { get; }

        void SetCertificateFilename(string certificateFilename);
        void SetCertificatePassword(string certificatePassword);
        void SetCertSubject(string certSubject);
        void SetCertOrganization(string certOrganization);
        void SetCertOrganizationUnit(string certOrganizationUnit);
        void SetCertLocality(string certLocality);
        void SetCertState(string certState);
        void SetCertCountry(string certCountry);
        void SetCertValidityYears(int certValidityYears);
        void SetInstallInStore(bool installInStore);
    }
}
