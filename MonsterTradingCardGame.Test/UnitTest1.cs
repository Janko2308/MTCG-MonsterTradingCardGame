using MonsterTradingCardGame.BL;
using System.Reflection.Metadata;

namespace MonsterTradingCardGame.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestUserConstructor()
        {
            //Arrange 
            User user = new("Janko", "1234");
            //Act auch Funktionen
            string name = "Janko";
            string password = "1234";
            //Assert
            Assert.That(user.Username, Is.EqualTo(name));
            Assert.That(user.Password, Is.EqualTo(password));
        }
        
        [Test]
        public void Test2()
        {
            Assert.Pass();
        }
    }
}