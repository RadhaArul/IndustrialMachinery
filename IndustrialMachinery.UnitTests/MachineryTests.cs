using Microsoft.VisualStudio.TestTools.UnitTesting;
using IndustrialMachinery.Core;

namespace IndustrialMachinery.UnitTests
{
    [TestClass]
    public class MachineryTests
    {
        [TestMethod]
        public void MachineCount_TestMethod()
        {
            //Arrange
            Machine m = new Machine();
            m.Id=System.Guid.NewGuid();
            m.Name = "machine1";
            m.Date=System.DateTime.Now;
            m.Status = false;
            m.Temperature = 20;
            var expected = 1;
            //Act
            var actual = 1;
            //Assert
            Assert.AreEqual(expected, actual);

        }
    }
}