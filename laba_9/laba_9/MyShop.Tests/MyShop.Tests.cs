using Xunit;

namespace Program.Tests
{
    public class OrderManagerTests
    {
        private readonly OrderManager orderManager;

        public OrderManagerTests()
        {
            orderManager = new OrderManager();
            orderManager.InitializeProducts(10, 5);
        }

        [Fact]
        public void ProcessOrder_WithValidProductAndDiscount_ShouldApplyDiscount()
        {
            // Arrange
            var product = orderManager.GetProducts()[0];
            var discountCode = orderManager.GetDiscountCodes()[0];

            // Act
            double finalPrice = orderManager.ProcessOrder(product.id, discountCode);

            // Assert
            Assert.True(finalPrice < product.price, "Discount should be applied");
        }

        [Fact]
        public void ProcessOrder_WithInvalidProductId_ShouldThrowException()
        {
            // Arrange
            int invalidProductId = -1;

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                orderManager.ProcessOrder(invalidProductId, "INVALID_CODE"));
        }

        [Fact]
        public void ApplyDiscountCode_WithInvalidCode_ShouldReturnFalse()
        {
            // Act
            bool result = orderManager.ApplyDiscountCode("INVALID_CODE");

            // Assert
            Assert.False(result, "Invalid discount code should not be applied");
        }

        [Fact]
        public void ApplyDiscountCode_WithValidCode_ShouldReturnTrue()
        {
            // Arrange
            var discountCode = orderManager.GetDiscountCodes()[0];

            // Act
            bool result = orderManager.ApplyDiscountCode(discountCode);

            // Assert
            Assert.True(result, "Valid discount code should be applied");
        }

        [Fact]
        public void GetEarnedMoney_AfterMultipleOrders_ShouldAccumulateTotal()
        {
            // Arrange
            var product1 = orderManager.GetProducts()[0];
            var product2 = orderManager.GetProducts()[1];
            var discountCode1 = orderManager.GetDiscountCodes()[0];
            var discountCode2 = orderManager.GetDiscountCodes()[1];

            // Act
            orderManager.ProcessOrder(product1.id, discountCode1);
            orderManager.ProcessOrder(product2.id, discountCode2);

            // Assert
            Assert.True(orderManager.GetEarnedMoney() > 0, "Earned money should accumulate");
        }
    }
}
