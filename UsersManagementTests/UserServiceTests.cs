using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.Json.Nodes;
using UsersManagementTests;

namespace UsersManagement.Tests
{
    [TestClass()]
    public class UserServiceTests
    {
        [TestMethod()]
        public void Get_Invalid_User()
        {
            string email = "emanuel@teste.com.br";
            string password = "12345";

            UserService userService = new(ValidInfos.ServerUrl);

            Model.ApiResponse resp = userService.GetUserTokenAsync(email, password).Result;

            Assert.IsFalse(resp.Success);
        }

        [TestMethod()]
        public void Get_Invalid_Email()
        {
            string email = "emanuel";
            string password = "12345";

            UserService userService = new(ValidInfos.ServerUrl);

            Model.ApiResponse resp = userService.GetUserTokenAsync(email, password).Result;

            Assert.IsFalse(resp.Success);
        }


        [TestMethod()]
        public void Get_InvalidPassword()
        {
            string email = ValidInfos.Email;
            string password = "Invalid";

            UserService userService = new(ValidInfos.ServerUrl);

            Model.ApiResponse resp = userService.GetUserTokenAsync(email, password).Result;

            Console.WriteLine(resp.Content);

            if (!resp.Success && resp?.Content is not null)
            {
               Assert.AreEqual("User/Password incorrect", resp.Content);
                return;
            }
            Assert.Fail();
        }

        [TestMethod()]
        public void GetUser_And_SingIn()
        {
            string email = ValidInfos.Email;
            string password = ValidInfos.Password;

            UserService userService = new(ValidInfos.ServerUrl);

            Model.ApiResponse resp = userService.GetUserTokenAsync(email, password).Result;

            Console.WriteLine(resp.Content);

            if (resp.Success && resp?.Content is not null)
            {
                string token = resp.Content;

                var userResp = userService.GetUserAsync(token).Result;

                if (userResp.Success && userResp.Content is not null)
                {
                    JsonNode? userResponse = JsonNode.Parse(userResp.Content);

                    if (userResponse is not null)
                        Assert.AreEqual(6, userResponse["id"]?.GetValue<int>() ?? 0);
                    return;
                }

            }
            Assert.Fail();
        }

        //[TestMethod()]
        //public void GetUserTest()
        //{
        //    string token = ValidInfos.Token;

        //    UserService userService = new(ValidInfos.ServerUrl);

        //    var resp = userService.GetUserAsync(token).Result;

        //    Assert.IsTrue(resp.Success);
        //}
    }
}