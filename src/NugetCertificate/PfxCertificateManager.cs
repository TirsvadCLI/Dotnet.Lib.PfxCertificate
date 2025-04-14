using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace PfxCertificateManager
{
    public static class CertificateManager
    {
        public static async Task<X509Certificate2> CreateCertificateAsync(string commonName, string organization, string organizationUnit, string country, string state, string locality, string password)
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

                Console.WriteLine("Certificate created successfully.");
                DisplayCertificateDetails(cert);

                return cert;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating certificate: {ex.Message}");
                return null;
            }
        }

        public static async Task AddCertificateAsync(string pfxPath, string password, StoreName storeName, StoreLocation storeLocation)
        {
            if (!File.Exists(pfxPath))
            {
                Console.WriteLine("PFX file not found.");
                return;
            }

            try
            {
                X509Certificate2 certificate = new X509Certificate2(pfxPath, password, X509KeyStorageFlags.PersistKeySet);
                DisplayCertificateDetails(certificate);

                Console.WriteLine("Certificate loaded successfully.");

                if (OperatingSystem.IsWindows())
                {
                    await Task.Run(() =>
                    {
                        using X509Store store = new X509Store(storeName, storeLocation);
                        store.Open(OpenFlags.ReadWrite);
                        store.Add(certificate);
                        store.Close();
                        Console.WriteLine($"Certificate added to {storeName} store in {storeLocation}.");
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding certificate: {ex.Message}");
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
                            Console.WriteLine($"Removed certificate with thumbprint: {thumbprint}");
                        }

                        store.Close();
                    });
                }
                else
                {
                    Console.WriteLine("Certificate removal is only supported on Windows.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing certificate: {ex.Message}");
            }
        }

        private static void DisplayCertificateDetails(X509Certificate2 cert)
        {
            Console.WriteLine("\nCertificate Details:");
            Console.WriteLine($"  Subject: {cert.Subject}");
            Console.WriteLine($"  Issuer: {cert.Issuer}");
            Console.WriteLine($"  Thumbprint: {cert.Thumbprint}");
            Console.WriteLine($"  Serial Number: {cert.SerialNumber}");
            Console.WriteLine($"  Valid From: {cert.NotBefore}");
            Console.WriteLine($"  Valid To: {cert.NotAfter}");
            Console.WriteLine($"  Public Key Algorithm: {cert.PublicKey.Oid.FriendlyName}");

            // Extracting organization-related details from the subject
            var subjectParts = cert.Subject.Split(", ");
            foreach (var part in subjectParts)
            {
                if (part.StartsWith("O=")) Console.WriteLine($"  Organization: {part.Substring(2)}");
                if (part.StartsWith("OU=")) Console.WriteLine($"  Organization Unit: {part.Substring(3)}");
                if (part.StartsWith("CN=")) Console.WriteLine($"  Common Name: {part.Substring(3)}");
                if (part.StartsWith("C=")) Console.WriteLine($"  Country: {part.Substring(2)}");
                if (part.StartsWith("ST=")) Console.WriteLine($"  State: {part.Substring(3)}");
                if (part.StartsWith("L=")) Console.WriteLine($"  Locality: {part.Substring(2)}");
            }
        }
    }
}
