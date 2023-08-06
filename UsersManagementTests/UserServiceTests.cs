using Microsoft.VisualStudio.TestTools.UnitTesting;
using UsersManagementTests;

namespace UsersManagement.Tests
{

    //todo - criar um usuario publico para testes.
    //ou mockar as informações quando só precisar rodar testes de validação de código?

    [TestClass()]
    public class UserServiceTests
    {
        [TestMethod()]
        public void GetInvalidUserTest()
        {
            string email = "emanuel@teste.com.br";
            string password = "12345";

            UserService userService = new(ValidInfos.ServerUrl);

            Model.ApiResponse resp = userService.GetUserTokenAsync(email, password).Result;

            Assert.IsFalse(resp.Success);
        }

        [TestMethod()]
        public void GetValidUserTest()
        {
            string email = ValidInfos.Email;
            string password = ValidInfos.Password;

            UserService userService = new(ValidInfos.ServerUrl);

            Model.ApiResponse resp = userService.GetUserTokenAsync(email, password).Result;

            Console.WriteLine(resp.Content);

            Assert.IsTrue(resp.Success);
        }

        [TestMethod()]
        public void GetUserTest()
        {
            string token = ValidInfos.Token;

            UserService userService = new(ValidInfos.ServerUrl);

            var resp = userService.GetUserAsync(token).Result;

            Assert.IsTrue(resp.Success);
        }
    }
}