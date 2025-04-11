using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace TirsvadCLI.NugetCertificate;

public class NugetCertificate
{
    Model.CertificateInfo NugetCert { get; }

    public NugetCertificate(Model.CertificateInfo nugetCert)
    {
        NugetCert = nugetCert;
    }

    public async Task CreateCertificateAsync()
    {
        using var rsa = RSA.Create(2048);
        var certificateRequest = new CertificateRequest(
            $"CN={NugetCert.CertSubject}, O={NugetCert.CertOrganization}, OU={NugetCert.CertOrganizationUnit}, L={NugetCert.CertLocality}, S={NugetCert.CertState}, C={NugetCert.CertCountry}",
            rsa,
            HashAlgorithmName.SHA256,
            RSASignaturePadding.Pkcs1);

        var certificate = certificateRequest.CreateSelfSigned(
            DateTimeOffset.Now,
            DateTimeOffset.Now.AddYears(NugetCert.CertValidityYears));

        certificate.FriendlyName = NugetCert.CertSubject;

        // Ensure the directory exists
        var certDirectory = Path.Combine(NugetCert.CertificatePath);
        if (!Directory.Exists(certDirectory))
        {
            Directory.CreateDirectory(certDirectory);
        }

        // Export the certificate with the private key
        var certBytes = certificate.Export(X509ContentType.Pfx, NugetCert.CertificatePassword);
        var certFilePath = Path.Combine(NugetCert.CertificatePath, NugetCert.CertificateFilename);
        await File.WriteAllBytesAsync(certFilePath, certBytes);

        // Install the certificate in the store if requested
        if (NugetCert.InstallInStore)
        {
            using var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadWrite);

            // Corrected the issue by directly creating an X509Certificate2 instance
            var x509Certificate = new X509Certificate2(certBytes, NugetCert.CertificatePassword, X509KeyStorageFlags.PersistKeySet);
            store.Add(x509Certificate);
        }
    }

    public static async Task RemoveCertificateFromStoreAsync(string certSubject)
    {
        using var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
        store.Open(OpenFlags.ReadWrite);

        // Use FindBySubjectName to match the test's verification logic
        var certs = store.Certificates.Find(X509FindType.FindBySubjectName, certSubject, false);
        foreach (var cert in certs)
        {
            store.Remove(cert);
        }

        // Simulate async operation (if needed for consistency)
        await Task.CompletedTask;
    }
}
