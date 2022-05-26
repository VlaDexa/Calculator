using Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Tests
{
    [TestClass]
    public class RomanTests
    {
        private readonly Dictionary<char, int> romanNumerals = new Dictionary<char, int>()
            {
                { 'I', 1 },
                { 'V', 5 },
                { 'X', 10 },
                { 'L', 50 },
                { 'C', 100 },
                { 'D', 500 },
                { 'M', 1000 }
            };

        [TestMethod]
        public void Stringalization()
        {
            foreach (var pair in romanNumerals)
            {
                var roman = new Roman(pair.Key.ToString());
                Assert.AreEqual(pair.Key.ToString(), roman.ToString());
            }
            Assert.AreEqual("IX", new Roman(9).ToString());
        }

        [TestMethod]
        public void Validation()
        {
            const string first = "IVIX";
            const string second = "FIVE";
            Assert.IsTrue(Roman.IsValid(first));
            Assert.IsFalse(Roman.IsValid(second));
        }

        [TestMethod]
        public void CreationByString()
        {
            foreach (var pair in romanNumerals)
            {
                var roman = new Roman(pair.Key.ToString());
                Assert.AreEqual(pair.Value, roman.ToDecimal());
            }
            Assert.AreEqual(4, new Roman("IV").ToDecimal());
        }

        [TestMethod]
        public void CreationByInt()
        {
            foreach (var pair in romanNumerals)
                Assert.AreEqual(pair.Value, new Roman(pair.Value).ToDecimal());
        }

        [TestMethod]
        public void Operations()
        {
            decimal first = 20;
            decimal second = 3;
            var firstR = new Roman(first);
            var secondR = new Roman(second);
            Assert.AreEqual(new Roman(first + second), firstR + secondR);
            Assert.AreEqual(new Roman(first - second), firstR - secondR);
            Assert.AreEqual(new Roman(first * second), firstR * secondR);
            Assert.AreEqual(new Roman(first / second), firstR / secondR);
            Assert.AreEqual(new Roman(first % second), firstR % secondR);
        }

        [TestMethod]
        public void TryParse()
        {
            const string input = "VII";
            Assert.IsTrue(Roman.TryParse(input, out var roman));
            Assert.AreEqual(roman.ToString(), input);
        }
    }
}
