using AsyncRestouran;

namespace UnitTest
{
    [TestClass]
    public sealed class Test1
    {
        private ICrudServiceAsync<Order> _service;
        private readonly string _testFilePath = "orders_test.json";

        [TestInitialize] 
        public void Setup()
        {
          
            _service = new OrderCrud(_testFilePath); 

            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
        }

        [TestMethod]
        public async Task CreateAsync_Should_AddItem_And_IncreaseCount()
        {
            var order = Order.CreateNew();
            Assert.AreEqual(0, _service.Count()); 
            await _service.CreateAsync(order);
   
            Assert.AreEqual(1, _service.Count()); 
        }
        [TestCleanup] 
        public void Cleanup()
        {
            
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
        }
    }
}
