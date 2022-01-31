using ENSEK.Metering.Services.Interfaces;
using Xunit;

namespace ENSEK.Metering.Services.UnitTests
{
    public class ValidationServiceTests
    {
        private readonly IValidationService _sut = new ValidationService();

        [Theory]
        [InlineData("temp.csv")]
        [InlineData("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx.csv")]
        public void IsValidFile_WhenValid_ShouldReturnTrue(string filename)
        {
            var result = _sut.IsValidFile(filename);
            
            Assert.True(result);
        }

        [Theory]
        [InlineData("temp.txt")]
        [InlineData("")]
        [InlineData(null)]
        public void IsValidFile_WhenInvalid_ShouldReturnFalse(string filename)
        {
            var result = _sut.IsValidFile(filename);

            Assert.False(result);
        }

        [Theory]
        [InlineData("1234,22/04/2019 09:24,11111")]
        public void IsValidRow_WhenValidShouldReturnTrue(string rowDataAsString)
        {
            var rowData = rowDataAsString.Split(',');

            var result = _sut.IsValidRow(rowData);

            Assert.True(result);
        }

        [Theory]
        [InlineData("1234,22/04/99999 09:24,11111")]
        [InlineData("1234,22/04/99999 09:24,0")]
        public void IsValidRow_WhenInvalid_ShouldReturnFalse(string rowDataAsString)
        {
            var rowData = rowDataAsString.Split(',');

            var result = _sut.IsValidRow(rowData);

            Assert.False(result);
        }

        [Theory]
        [InlineData(10000)]
        [InlineData(55555)]
        [InlineData(99999)]
        public void IsValidReading_WhenValid_ShouldReturnTrue(int meterReading)
        {
            var result = _sut.IsValidReading(meterReading);

            Assert.True(result);
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(0)]
        [InlineData(null)]
        public void IsValidReading_WhenInvalid_ShouldReturnFalse(int meterReading)
        {
            var result = _sut.IsValidReading(meterReading);

            Assert.False(result);
        }
    }
}