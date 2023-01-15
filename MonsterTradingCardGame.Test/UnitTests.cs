using System.Reflection.Metadata;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using System.Security.Authentication;
using System.Text.Json.Nodes;
using System.Text;
using System.Transactions;
using MonsterTradingCardGame.BL;
using MonsterTradingCardGame.BL.Exceptions;
using MonsterTradingCardGame.DAL;
using MonsterTradingCardGame.Http;
using System.Net.Security;

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
            Assert.Multiple(() =>
            {
                Assert.That(user.Username, Is.EqualTo(name));
                Assert.That(user.Password, Is.EqualTo(password));
            });
        }

        [Test]
        public void TestCardConstructor()
        {
            //Arrange
            Card card = new("1fe8032rhicvsdipjw", "TestKarte", "Monster", "Feuer", 50);
            //Act
            string id = "1fe8032rhicvsdipjw";
            string name = "TestKarte";
            string type = "Monster";
            string element = "Feuer";
            int damage = 50;

            //Assert
            Assert.Multiple(()  =>
            {
                Assert.That(card.ID, Is.EqualTo(id));
                Assert.That(card.Name, Is.EqualTo(name));
                Assert.That(card.Type, Is.EqualTo(type));
                Assert.That(card.Element, Is.EqualTo(element));
                Assert.That(card.Damage, Is.EqualTo(damage));
            });
        }

        [Test]
        public void RegisterUser()
        {
            //Arrange
            UserLogic userLogic = new();
            //Act
            userLogic.RegisterUser("testuser1", "TestPassword");
            bool userExists = userLogic.CheckIfUserExists("testuser1");
            userLogic.DeleteUser("testuser1", "TestPassword");

            //Assert
            Assert.That(userExists, Is.EqualTo(true));

        }

        [Test]
        public void TestUserDoesNotExist()
        {
            //Arrange
            UserLogic userLogic = new();
            //Act
            bool userExists = userLogic.CheckIfUserExists("testuser3");

            //Assert
            Assert.That(userExists, Is.EqualTo(false));
        }
    

        [Test, Order(1)]
        public void TestUserAlreadyExists()
        {
            // Arrange
            UserLogic userLogic = new();
            userLogic.RegisterUser("testuser2", "TestPassword");

            // Act and Assert
            Assert.Throws<ConflictException>(() => userLogic.RegisterUser("testuser2", "TestPassword"));
          

        }

        [Test, Order(2)]
        public void TestDeleteUser()
        {
            //Arrange
            UserLogic userLogic = new();

            //Act
            userLogic.DeleteUser("testuser2", "TestPassword");
            bool userExists = userLogic.CheckIfUserExists("testuser2");

            //Assert
            Assert.IsFalse(userExists);
        }

        [Test]
        public void TestDeleteUserNotFound()
        {
            //Arrange
            UserLogic userLogic = new();

            //Act and Assert
            Assert.Throws<NotFoundException>(() => userLogic.DeleteUser("TestUser3", "TestPassword"));
        }

        [Test, Order(3)]
        public void TestGetUserCorrectArgument()
        {
            //Arrange
            UserLogic userLogic = new();
            userLogic.RegisterUser("testuser4", "TestPassword");

            //Act
            UserInfo Testinfo = new();
            Testinfo.Name = "no name";
            Testinfo.Bio = "no bio";
            Testinfo.Image = "no image";
            
            UserInfo userInfo = userLogic.GetUser("testuser4");
            
            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(userInfo.Name, Is.EqualTo(Testinfo.Name));
                Assert.That(userInfo.Bio, Is.EqualTo(Testinfo.Bio));
                Assert.That(userInfo.Image, Is.EqualTo(Testinfo.Image));
            });
            userLogic.DeleteUser("testuser4", "TestPassword");
        }

        [Test, Order(4)]

        public void TestChangeUserInfo()
        {
            //Arrange
            UserLogic userLogic = new();

            //Act
            userLogic.RegisterUser("testuser6", "TestPassword");
            userLogic.UpdateUserInfo("testuser6", "{\"Name\": \"TestName\",  \"Bio\": \"me playin...\", \"Image\": \":-)\"}");
            UserInfo userInfo = new();
            userInfo.Name = userLogic.GetUser("testuser6").Name;
            userInfo.Bio = userLogic.GetUser("testuser6").Bio;
            userInfo.Image = userLogic.GetUser("testuser6").Image;
            userLogic.DeleteUser("testuser6", "TestPassword");
            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(userInfo.Name, Is.EqualTo("TestName"));
                Assert.That(userInfo.Bio, Is.EqualTo("me playin..."));
                Assert.That(userInfo.Image, Is.EqualTo(":-)"));
            });

        }

        [Test]
        public void UpdateUserInfo_UserNotFound_ThrowsUserNotFoundException()
        {
            // Arrange
            var userLogic = new UserLogic();

            // Act and Assert
            Assert.Throws<UserNotFoundException>(() => userLogic.UpdateUserInfo("UserNotFound", "content"));
        }

        [Test, Order(5)]
        
        public void TestLogin()
        {
            //Arrange
            UserLogic userLogic = new();
            SessionLogic sessionLogic = new();

            //Act
            userLogic.RegisterUser("testuser5", "TestPassword");
            string token = "";
            sessionLogic.CreateSession("testuser5");
            token = sessionLogic.Login("testuser5", "TestPassword");
            userLogic.DeleteUser("testuser5", "TestPassword");
            
            //Assert
            Assert.That(token == "testuser5-mtcgtoken");

        }
        
        [Test]

        public void TestLoginEmptyString()
        {
            //Arrange
            UserLogic userLogic = new();

            //Act and Assert
            
            Assert.Throws<ArgumentNullException>(() => userLogic.RegisterUser("", "TestPassword"));

        }

        [Test]
        public void Login_WrongCredentials_ThrowsAuthenticationException()
        {
            // Arrange
            UserLogic userLogic = new();
            SessionLogic sessionLogic = new();
            
            userLogic.RegisterUser("testuser9", "TestPassword");

            // Act and Assert
            Assert.Throws<AuthenticationException>(() => sessionLogic.Login("testuser9", "WrongPassword"));
            userLogic.DeleteUser("testuser9", "TestPassword");
        }

        [Test]
        public void TestCheckIfSession()
        {
            //Arrange
            UserLogic userLogic = new();
            SessionLogic sessionLogic = new();

            //Act
            userLogic.RegisterUser("testuser7", "TestPassword");
            string token = "";
            sessionLogic.CreateSession("testuser7");
            token = sessionLogic.Login("testuser7", "TestPassword");
            bool session = sessionLogic.CheckIfSession("testuser7", token);
            userLogic.DeleteUser("testuser7", "TestPassword");

            //Assert
            Assert.IsTrue(session);
        }
        [Test]
        public void TestCheckIfSessionWithoutToken()
        {
            //Arrange
            UserLogic userLogic = new();
            SessionLogic sessionLogic = new();

            //Act
            userLogic.RegisterUser("testuser11", "TestPassword");

            //Assert
            Assert.Throws<AuthenticateTokenException>(()=> sessionLogic.CheckIfSession("testuser11", ""));
            userLogic.DeleteUser("testuser11", "TestPassword");
        }

        [Test]

        public void TestIfAdminExists()
        {
            //Arrange
            UserLogic userLogic = new();

            //Act
            bool adminExists = userLogic.CheckIfAdmin("admin-mtcgtoken");

            //Assert
            Assert.That(adminExists, Is.EqualTo(true));
        }

        [Test]
        public void TestGetUserNameFromToken()
        {
            //Arrange
            UserLogic userLogic = new();
            SessionLogic sessionLogic = new();

            //Act
            userLogic.RegisterUser("testuser8", "TestPassword");
            sessionLogic.Login("testuser8", "TestPassword");
            string userName = userLogic.GetUserNameFromToken("testuser8-mtcgtoken");
            userLogic.DeleteUser("testuser8", "TestPassword");
            //Assert
            Assert.That(userName == "testuser8");
        }

        [Test]
        public void TestCreatePackageCount5()
        {
            //Arrange

            List<Card> cardList = new();
            
            //Act
            for (int i = 0; i < 5; i++)
            {
                int damage = 1;
                Card tempCard = new("testid", "testname", "testtype", "testelement", damage);
                cardList.Add(tempCard);
            }

            //Assert
            Assert.That(cardList.Count == 5);
        }

        [Test]
        public void TestCreatePackageCountFalse()
        {
            //Arrange

            List<Card> cardList = new();

            //Act
            for (int i = 0; i < 4; i++)
            {
                int damage = 1;
                Card tempCard = new("testid", "testname", "testtype", "testelement", damage);
                cardList.Add(tempCard);
            }

            //Assert
            Assert.IsFalse(cardList.Count == 5);
        }

        [Test, Order(10)]
        public void TestBuyPackageNoPackage()
        {
            //Arrange
            UserLogic userLogic = new();
            SessionLogic sessionLogic = new();
            TransactionsRepository transActionsRepository = new();

            //Act
            userLogic.RegisterUser("testuser10", "TestPassword");
            sessionLogic.Login("testuser10", "TestPassword");

            //Assert
            Assert.Throws<NoItemAvaiableException>(() => transActionsRepository.BuyPackage("testuser10-mtcgtoken"));
            userLogic.DeleteUser("testuser10", "TestPassword");
        }

        [Test]
        public void TestIfUserHasMoney()
        {
            //Arrange
            UserLogic userLogic = new();
            SessionLogic sessionLogic = new();
            TransactionsRepository transactionsRepository = new();

            //Act
            userLogic.RegisterUser("testuser12", "TestPassword");
            sessionLogic.Login("testuser12", "TestPassword");

            //Assert
            Assert.IsTrue(transactionsRepository.CheckMoneyAndPay("testuser12-mtcgtoken"));

            userLogic.DeleteUser("testuser12", "TestPassword");

        }


        /*
        [Test, Order(11)]
        public void TestAddPackage()
        {
            //Arrange
            HttpRequest request = new("POST", "/packages", "[{\"Id\":\"845f0dc7-37d0-426e-994e-43fc3ac83c08\", \"Name\":\"WaterGoblin\",  \"Type\":\"Monster\",  \"Element\":\"Water\", \"Damage\": 10}, {\"Id\":\"99f8f8dc-e25e-4a95-aa2c-782823f36e2a\", \"Name\":\"Dragon\", \"Type\":\"Monster\", \"Element\":\"Fire\",  \"Damage\": 50}, {\"Id\":\"e85e3976-7c86-4d06-9a80-641c2019a79f\", \"Name\":\"WaterSpell\", \"Type\":\"Spell\", \"Element\":\"Water\", \"Damage\": 20}, {\"Id\":\"1cb6ab86-bdb2-47e5-b6e4-68c5ab389334\", \"Name\":\"Ork\", \"Type\":\"Monster\", \"Element\":\"Normal\", \"Damage\": 45}, {\"Id\":\"dfdd758f-649c-40f9-ba3a-8657f4b3439f\", \"Name\":\"FireSpell\", \"Type\":\"Spell\", \"Element\":\"Fire\", \"Damage\": 25}]");     
            HttpResponse response = new();
            UserLogic userLogic = new();
            SessionLogic sessionLogic = new();
            TransactionsRepository transActionsRepository = new();

            

            

        }
        */
        /*
        
        [Test]
        public void TestCreatePackageNotEnoughCards()
        {
            //Arrange
            StringContent packageCards;
            packageCards = new StringContent("[{\"Id\":\"845f0dc7-37d0-426e-994e-43fc3ac83c08\", \"Name\":\"WaterGoblin\",  \"Type\":\"Monster\",  \"Element\":\"Water\", \"Damage\": 10}, {\"Id\":\"99f8f8dc-e25e-4a95-aa2c-782823f36e2a\", \"Name\":\"Dragon\", \"Type\":\"Monster\", \"Element\":\"Fire\",  \"Damage\": 50}, {\"Id\":\"e85e3976-7c86-4d06-9a80-641c2019a79f\", \"Name\":\"WaterSpell\", \"Type\":\"Spell\", \"Element\":\"Water\", \"Damage\": 20}, {\"Id\":\"1cb6ab86-bdb2-47e5-b6e4-68c5ab389334\", \"Name\":\"Ork\", \"Type\":\"Monster\", \"Element\":\"Normal\", \"Damage\": 45}, {\"Id\":\"dfdd758f-649c-40f9-ba3a-8657f4b3439f\", \"Name\":\"FireSpell\", \"Type\":\"Spell\", \"Element\":\"Fire\", \"Damage\": 25}]", Encoding.UTF8, "application/json"); 
            List<Card> cardList = new();

            //Act
            for (int i = 0; i < 5; i++)
            {
                string tempDamage = packageCards[i]["Damage"].ToString();
                int damage = int.Parse(tempDamage);
                Card tempCard = new(packageCards[i]["Id"].ToString(), packageCards[i]["Name"].ToString(), packageCards[i]["Type"].ToString(), packageCards[i]["Element"].ToString(), damage);
                cardList.Add(tempCard);
            }
            
            //Assert
            Assert.That(cardList.Count == 5);
        }

        */

        /*
        [Test, Order(20)]
        public void TestCleanUp()
        {
            UserLogic userLogic = new();
            userLogic.DeleteUser("TestUser1", "TestPassword");
            userLogic.DeleteUser("TestUser5", "TestPassword");
        }
        */
        /*
        [Test, Order(20)]
        public void CleanUp()
        {
            UserLogic userLogic = new();
            if (userLogic.CheckIfUserExists("TestUser1"))
            {
                userLogic.DeleteUser("TestUser1", "TestPassword");
            }
            if (userLogic.CheckIfUserExists("TestUser4"))
            {
                userLogic.DeleteUser("TestUser4", "TestPassword");
            }
            if (userLogic.CheckIfUserExists("TestUser5"))
            {
                userLogic.DeleteUser("TestUser5", "TestPassword");
            }
        }
        */

    }
}