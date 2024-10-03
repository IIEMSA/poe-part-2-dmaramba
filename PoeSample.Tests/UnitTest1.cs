using PoeSample.Services;

namespace PoeSample.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            TestingService service = new TestingService();
            var result = service.Sum(1, 1);

            Assert.Equal(2, result);
        }
    }
}