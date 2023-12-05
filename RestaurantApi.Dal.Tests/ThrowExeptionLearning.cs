
namespace RestaurantApi.Dal.Tests
{
    public class ThrowExeptionLearning
    {
        [Fact]
        public void TryCatchAnExption()
        {
            var act = () => { throw new ArgumentException(); };
            Assert.Throws<ArgumentException>(act);
        }
        [Fact]
        public async Task TryCatchAnExptionAsync()
        {
            var act = async () =>
            {
                await Run();
            };
            _ = Assert.ThrowsAsync<ArgumentException>(act);

            async Task Run()
            {
                await Task.Delay(1000);
                throw new ArgumentException();
            }
        }
        [Fact]
        public  void TryCatchAnExptionInOtherMethod()
        {
            var act = async () =>
            {
                await Run();
            };
            _ = Assert.ThrowsAsync<ArgumentException>(act);

            async Task Run()
            {
                await Task.Delay(1000);
                throw new ArgumentException();
            }
        }
    }
}
