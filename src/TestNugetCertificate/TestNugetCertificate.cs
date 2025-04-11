namespace TirsvadCLI.TestNugetCertificate;

using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using TirsvadCLI.NugetCertificate;
using TirsvadCLI.NugetCertificate.Model;

[TestClass]
public sealed class TestNugetCertificate
{
    [TestClass]
    public class NugetCertificateTests
    {
        private const string TestCertificatePath = "./TestCertificates";

        [TestInitialize]
        public void TestInitialize()
        {
            // Ensure the test directory exists
            if (!Directory.Exists(TestCertificatePath))
            {
                Directory.CreateDirectory(TestCertificatePath);
            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // Delete the test certificate files if they exist
            foreach (var file in Directory.GetFiles(TestCertificatePath))
            {
                // TestCleanup method TirsvadCLI.TestNugetCertificate.TestNugetCertificate+NugetCertificateTests.TestCleanup threw exception. System.UnauthorizedAccessException: Access to the path 'testCert.pfx' is denied..
                for (int i = 0; i < 5; i++)
                {
                    try
                    {
                        File.Delete(file);
                        break; // Exit the loop if successful
                    }
                    catch (UnauthorizedAccessException)
                    {
                        // If access is denied, wait and retry
                        Task.Delay(100).Wait();
                    }
                }
            }
        }

        [TestMethod]
        public async Task CreateCertificate_ShouldCreateCertificateFile()
        {
            var certificateInfo = new CertificateInfo(
                    TestCertificatePath,
                    "testCert.pfx",
                    "TestPassword123!",
                    "TestSubject",
                    "TestOrganization",
                    "TestOrgUnit",
                    "TestLocality",
                    "TestState",
                    "US",
                    1,
                    installInStore: false);


            var nugetCertificate = new NugetCertificate(certificateInfo);

            // Act
            Task createCertificateTask = nugetCertificate.CreateCertificateAsync();

            // Assert
            var certFilePath = Path.Combine(TestCertificatePath, certificateInfo.CertificateFilename);
            await createCertificateTask;
            Assert.IsTrue(File.Exists(certFilePath), "The certificate file was not created.");
        }

        [TestMethod]
        public void CreateCertificate_ShouldInstallCertificateInStore_WhenInstallInStoreIsTrue()
        {
            // Arrange
            var certificateInfo = new CertificateInfo(
                    TestCertificatePath,
                    "testCert1.pfx",
                    "TestPassword123!",
                    "TestSubject1",
                    "TestOrganization",
                    "TestOrgUnit",
                    "TestLocality",
                    "TestState",
                    "US",
                    1,
                    installInStore: true);

            var nugetCertificate = new NugetCertificate(certificateInfo);

            // Act
            nugetCertificate.CreateCertificateAsync();

            // Assert
            using var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            var certs = store.Certificates.Find(X509FindType.FindBySubjectName, certificateInfo.CertSubject, false);
            Assert.IsTrue(certs.Count > 0, "The certificate was not installed in the store.");
        }

        [TestMethod]
        public async Task RemoveCertificateFromStore_ShouldRemoveCertificate()
        {
            // Arrange
            var certificateInfo = new CertificateInfo(
                    TestCertificatePath,
                    "testCert2.pfx",
                    "TestPassword123!",
                    "TestSubject2",
                    "TestOrganization",
                    "TestOrgUnit",
                    "TestLocality",
                    "TestState",
                    "US",
                    1,
                    installInStore: true);

            var nugetCertificate = new NugetCertificate(certificateInfo);

            // Act
            await nugetCertificate.CreateCertificateAsync();

            // Assert that the certificate was installed
            using (var store = new X509Store(StoreName.My, StoreLocation.CurrentUser))
            {
                store.Open(OpenFlags.ReadOnly);
                var certs = store.Certificates.Find(X509FindType.FindBySubjectName, certificateInfo.CertSubject, false);
                Assert.IsTrue(certs.Count > 0, "The certificate was not installed in the store.");
            }

            // Act - Remove the certificate
            await NugetCertificate.RemoveCertificateFromStoreAsync(certificateInfo.CertSubject);
            // Assert that the certificate was removed
            using (var store = new X509Store(StoreName.My, StoreLocation.CurrentUser))
            {
                store.Open(OpenFlags.ReadOnly);
                var certs = store.Certificates.Find(X509FindType.FindBySubjectName, certificateInfo.CertSubject, false);
                Assert.IsTrue(certs.Count == 0, "The certificate was not removed from the store.");
            }
        }
    }
}
