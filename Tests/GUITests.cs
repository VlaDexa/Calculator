using GUI;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class GUITests
    {
        [TestMethod]
        public void Validation()
        {
            {
                var (success, first, op, second) = Calculator.ValidateInput("IIX");
                Assert.IsTrue(success);
                Assert.AreEqual("IIX", first);
                Assert.IsNull(op);
                Assert.IsNull(second);
            }
            {
                var (success, first, op, second) = Calculator.ValidateInput("IIX*XII");
                Assert.IsTrue(success);
                Assert.AreEqual("IIX", first);
                Assert.AreEqual("*", op);
                Assert.AreEqual("XII", second);
            }
            {
                var (success, first, op, second) = Calculator.ValidateInput("LOL");
                Assert.IsFalse(success);
                Assert.IsNull(first);
                Assert.IsNull(op);
                Assert.IsNull(second);
            }
            {
                var (success, first, op, second) = Calculator.ValidateInput("15-9");
                Assert.IsTrue(success);
                Assert.AreEqual("15", first);
                Assert.AreEqual("-", op);
                Assert.AreEqual("9", second);
            }
        }
    }
}
