namespace Application.Tests
{
    using NUnit.Framework;

    using Services;

    public class CryptoServiceTests
    {
        [Test]
        public void EmptyString_Test()
        {
            var emptyString = string.Empty;
            var expectedHash = "E3B0C44298FC1C149AFBF4C8996FB92427AE41E4649B934CA495991B7852B855";

            var emptyStringHash = CryptoService.ComputeHash(emptyString);
            Assert.AreEqual(emptyStringHash, expectedHash);
        }
    }
}