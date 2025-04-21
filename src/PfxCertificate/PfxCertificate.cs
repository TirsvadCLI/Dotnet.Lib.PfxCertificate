namespace TirsvadCLI.PfxCertificate;

using System.Globalization;
using System.Resources;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

public static class PfxCertificate
{
    private static readonly ResourceManager _resourceManager = new ResourceManager("TirsvadCLI.PfxCertificate.Properties.Resource", typeof(PfxCertificate).Assembly);
    public static CultureInfo? Culture { get; private set; } = null;
    public static async Task<X509Certificate2>? CreateCertificateAsync(string commonName, string organization, string organizationUnit, string country, string state, string locality, string password)
    {
        try
        {
            using RSA rsa = RSA.Create(2048);

            var request = new CertificateRequest(
                $"CN={commonName}, O={organization}, OU={organizationUnit}, C={country}, S={state}, L={locality}",
                rsa,
                HashAlgorithmName.SHA256,
                RSASignaturePadding.Pkcs1);

            request.CertificateExtensions.Add(new X509BasicConstraintsExtension(false, false, 0, false));
            request.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.DigitalSignature | X509KeyUsageFlags.KeyEncipherment, false));
            request.CertificateExtensions.Add(new X509SubjectKeyIdentifierExtension(request.PublicKey, false));

            var cert = request.CreateSelfSigned(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddYears(5));

            byte[] pfxBytes = cert.Export(X509ContentType.Pfx, password);
            await File.WriteAllBytesAsync($"{commonName}.pfx", pfxBytes);

            Console.WriteLine($"{GetMsg("Certificate created successfully")}.");
            DisplayCertificateDetails(cert);

            return cert;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{GetMsg("Error creating certificate")}: {ex.Message}");
            return null; // Return a default instance instead of null;
        }
    }
    public static async Task AddCertificateAsync(string pfxPath, string password, StoreName storeName, StoreLocation storeLocation)
    {
        if (!File.Exists(pfxPath))
        {
            Console.WriteLine($"{GetMsg("PFX file not found")}.");
            return;
        }

        try
        {
            X509Certificate2 certificate = new X509Certificate2(pfxPath, password, X509KeyStorageFlags.PersistKeySet);
            DisplayCertificateDetails(certificate);

            Console.WriteLine($"{GetMsg("Certificate loaded successfully")}.");

            if (OperatingSystem.IsWindows())
            {
                await Task.Run(() =>
                {
                    using X509Store store = new X509Store(storeName, storeLocation);
                    store.Open(OpenFlags.ReadWrite);
                    store.Add(certificate);
                    store.Close();
                    Console.WriteLine($"{GetMsg("Certificate added to")} {storeName} {GetMsg("store in")} {storeLocation}.");
                });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{GetMsg("Error adding certificate")}: {ex.Message}");
        }
    }
    public static async Task RemoveCertificateAsync(string thumbprint, StoreName storeName, StoreLocation storeLocation)
    {
        try
        {
            if (OperatingSystem.IsWindows())
            {
                await Task.Run(() =>
                {
                    using X509Store store = new X509Store(storeName, storeLocation);
                    store.Open(OpenFlags.ReadWrite);

                    X509Certificate2Collection certs = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);

                    foreach (var cert in certs)
                    {
                        DisplayCertificateDetails(cert);
                        store.Remove(cert);
                        Console.WriteLine($"{GetMsg("Removed certificate with thumbprint")}: {thumbprint}");
                    }

                    store.Close();
                });
            }
            else
            {
                Console.WriteLine($"{GetMsg("Certificate removal is only supported on Windows")}.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{GetMsg("Error removing certificate")}: {ex.Message}");
        }
    }
    private static void DisplayCertificateDetails(X509Certificate2 cert)
    {
        Console.WriteLine($"\n{GetMsg("Certificate details")}:");
        Console.WriteLine($"    {GetMsg("Subject")}: {cert.Subject}");
        Console.WriteLine($"    {GetMsg("Issuer")}: {cert.Issuer}");
        Console.WriteLine($"    {GetMsg("Thumbprint")}: {cert.Thumbprint}");
        Console.WriteLine($"    {GetMsg("Serial number")}: {cert.SerialNumber}");
        Console.WriteLine($"    {GetMsg("Valid from")}: {cert.NotBefore}");
        Console.WriteLine($"    {GetMsg("Valid to")}: {cert.NotAfter}");
        Console.WriteLine($"    {GetMsg("Public key algorithm")}: {cert.PublicKey.Oid.FriendlyName}");

        // Extracting organization-related details from the subject
        var subjectParts = cert.Subject.Split(", ");
        foreach (var part in subjectParts)
        {
            if (part.StartsWith("O=")) Console.WriteLine($"    {GetMsg("Organization")}: {part.Substring(2)}");
            if (part.StartsWith("OU=")) Console.WriteLine($"    {GetMsg("Organization unit")}: {part.Substring(3)}");
            if (part.StartsWith("CN=")) Console.WriteLine($"    {GetMsg("Common name")}: {part.Substring(3)}");
            if (part.StartsWith("C=")) Console.WriteLine($"    {GetMsg("Country")}: {part.Substring(2)}");
            if (part.StartsWith("ST=")) Console.WriteLine($"    {GetMsg("State")}: {part.Substring(3)}");
            if (part.StartsWith("L=")) Console.WriteLine($"    {GetMsg("Locality")}: {part.Substring(2)}");
        }
    }
    /// <summary>
    /// Get a string from the resource file.
    /// </summary>
    /// <param name="msg">Msg to be translated</param>
    /// <returns>Translated msg</returns>
    private static string GetMsg(string msg)
    {
        return _resourceManager.GetString(msg, Culture) ?? msg;
    }
}
