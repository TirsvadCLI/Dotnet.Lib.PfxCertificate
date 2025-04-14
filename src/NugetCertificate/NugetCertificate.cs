namespace TirsvadCLI.NugetCertificate;

using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

public class NugetCertificate
{
    /// <summary>
    /// Certificate information.
    /// </summary>
    Model.CertificateInfo NugetCert { get; }

    /// <summary>
    /// Constructor for the NugetCertificate class.
    /// </summary>
    /// <param name="nugetCert"></param>
    public NugetCertificate(Model.CertificateInfo nugetCert)
    {
        NugetCert = nugetCert;
    }

    /// <summary>
    /// Creates a self-signed certificate and saves it to the specified path.
    /// </summary>
    /// <returns></returns>
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

        // Set FriendlyName only on Windows
        if (OperatingSystem.IsWindows())
        {
            certificate.FriendlyName = NugetCert.CertSubject;
        }

        // Ensure the directory exists
        var certDirectory = Path.Combine(NugetCert.CertificatePath);
        if (!Directory.Exists(certDirectory))
        {
            Directory.CreateDirectory(certDirectory);
        }

        // Export the certificate with the private key
        byte[]? certBytes = certificate.Export(X509ContentType.Pfx, NugetCert.CertificatePassword);
        var certFilePath = Path.Combine(NugetCert.CertificatePath, NugetCert.CertificateFilename);
        await File.WriteAllBytesAsync(certFilePath, certBytes);

        // Install the certificate in the store if requested
        if (NugetCert.InstallInStore)
        {
            using var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadWrite);

            // Use the X509Certificate2 constructor to load the certificate from the byte array
            var x509Certificate = new X509Certificate2(
                certBytes,
                NugetCert.CertificatePassword,
                X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.UserKeySet);

            // Add the certificate to the store
            store.Add(x509Certificate);
        }
    }


    /// <summary>
    /// Removes a certificate from the store based on its subject name.
    /// </summary>
    /// <param name="certSubject"></param>
    /// <returns></returns>
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