using System.Security.Cryptography.X509Certificates;

namespace TirsvadCLI.PfxCertificate.Tests;

using TirsvadCLI.PfxCertificate;

[TestClass]
public class CertificateTests
{
    private const string TestCommonName = "TestCertificate";
    private const string TestOrganization = "TestOrg";
    private const string TestOrganizationUnit = "TestUnit";
    private const string TestCountry = "US";
    private const string TestState = "TestState";
    private const string TestLocality = "TestLocality";
    private const string TestPassword = "TestPassword";
    private const string TestPfxFileName = "TestCertificate.pfx";

    [TestMethod]
    public async Task CreateCertificateAsync_ShouldCreateCertificate()
    {
        // Act
        var certificate = await PfxCertificate.CreateCertificateAsync(
            TestCommonName,
            TestOrganization,
            TestOrganizationUnit,
            TestCountry,
            TestState,
            TestLocality,
            TestPassword);

        // Assert
        Assert.IsNotNull(certificate, "Certificate creation failed.");
        Assert.AreEqual($"CN={TestCommonName}, O={TestOrganization}, OU={TestOrganizationUnit}, C={TestCountry}, S={TestState}, L={TestLocality}", certificate.Subject);
        Assert.IsTrue(File.Exists(TestPfxFileName), "PFX file was not created.");
    }

    [TestMethod]
    public async Task AddCertificateAsync_ShouldAddCertificateToStore()
    {
        // Arrange
        await PfxCertificate.CreateCertificateAsync(
            TestCommonName,
            TestOrganization,
            TestOrganizationUnit,
            TestCountry,
            TestState,
            TestLocality,
            TestPassword);
        // Act
        await PfxCertificate.AddCertificateAsync(TestPfxFileName, TestPassword, StoreName.My, StoreLocation.CurrentUser);

        // Assert
        Task.Delay(1000).Wait(); // Wait for the file system to settle
        using var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
        store.Open(OpenFlags.ReadOnly);
        var certs = store.Certificates.Find(X509FindType.FindBySubjectName, TestCommonName, false);
        Assert.IsTrue(certs.Count > 0, "Certificate was not added to the store.");
    }

    [TestMethod]
    public async Task RemoveCertificateAsync_ShouldRemoveCertificateFromStore()
    {
        Task.Delay(1000).Wait(); // Wait for the file system to settle
        // Arrange
        await PfxCertificate.CreateCertificateAsync(
            TestCommonName,
            TestOrganization,
            TestOrganizationUnit,
            TestCountry,
            TestState,
            TestLocality,
            TestPassword);
        await PfxCertificate.AddCertificateAsync(TestPfxFileName, TestPassword, StoreName.My, StoreLocation.CurrentUser);
        Task.Delay(1000).Wait(); // Wait for the file system to settle

        using var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
        store.Open(OpenFlags.ReadOnly);
        var certs = store.Certificates.Find(X509FindType.FindBySubjectName, TestCommonName, false);
        Assert.IsTrue(certs.Count > 0, "Certificate was not added to the store.");

        // Act
        await PfxCertificate.RemoveCertificateAsync(certs[0].Thumbprint, StoreName.My, StoreLocation.CurrentUser);

        // Assert
        certs = store.Certificates.Find(X509FindType.FindBySubjectName, TestCommonName, false);
        Assert.AreEqual(0, certs.Count, "Certificate was not removed from the store.");
    }

    [TestCleanup]
    public void Cleanup()
    {
        // Delete the PFX file if it exists
        if (File.Exists(TestPfxFileName))
        {
            const int maxRetries = 5;
            const int delayMilliseconds = 500;

            for (int attempt = 0; attempt < maxRetries; attempt++)
            {
                try
                {
                    File.Delete(TestPfxFileName);
                    break; // Exit the loop if deletion succeeds
                }
                catch (IOException)
                {
                    if (attempt == maxRetries - 1)
                    {
                        throw; // Rethrow the exception if all retries fail
                    }
                    Thread.Sleep(delayMilliseconds); // Wait before retrying
                }
            }
        }


        // Remove the certificate from the store
        using var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
        store.Open(OpenFlags.ReadWrite);
        var certs = store.Certificates.Find(X509FindType.FindBySubjectName, TestCommonName, false);
        foreach (var cert in certs)
        {
            store.Remove(cert);
        }
    }
}
