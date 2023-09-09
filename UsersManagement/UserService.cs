using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using UsersManagement.Model;

namespace UsersManagement
{

    public partial class UserService : IUserService
    {
        private readonly string ServerUrl;// "";

        public UserService(string serverUrl)
        {
            ServerUrl = serverUrl;
        }

        public async Task<ApiResponse> GetUserAsync(string token)
        {
            return await HttpClientFunctions.GetAsync(ServerUrl + "/user", token);
        }

        public async Task<ApiResponse> AddUserAsync(string name, string email, string password)
        {
            try
            {
                string json = JsonSerializer.Serialize(new { name, email, password });

                return await HttpClientFunctions.PostAsync(ServerUrl + "/user", json);
            }
            catch (Exception ex) { throw; }
        }

        public async Task<ApiResponse> GetUserTokenAsync(string email, string password)
        {
            try
            {
                email = email.ToLower();
                Regex regexEmail = RegexEmail();

                if (regexEmail.IsMatch(email))
                {
                    (bool success, string? message) = await GetUserSessionAsync(email, password);

                    return new ApiResponse() { Success = success, Content = message };
                }
                else return new ApiResponse() { Success = false, Content = "Invalid Email" };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ApiResponse> RecoverPasswordAsync(string email)
        {
            string json = JsonSerializer.Serialize(new { email });
            return await HttpClientFunctions.PostAsync(ServerUrl + "/user/recoverpassword", json);
        }

        private async Task<(bool, string?)> GetUserSessionAsync(string email, string password)
        {
            try
            {
                string json = JsonSerializer.Serialize(new { email, password });

                var resp = await HttpClientFunctions.PostAsync(ServerUrl + "/user/session", json);

                if (resp is not null && resp.Content is not null)
                {
                    JsonNode? jResp = JsonNode.Parse(resp.Content);

                    if (resp.Success && jResp is not null && jResp?["token"]?.GetValue<string>() is not null)
                        return (true, jResp?["token"]?.GetValue<string>());
                    else if (!resp.Success && jResp is not null)
                    {
                        if (jResp?["errors"]?.GetValue<string>() is not null)
                            return (false, jResp?["errors"]?.GetValue<string>());
                        else if (jResp?["error"]?.GetValue<string>() is not null)
                            return (false, jResp?["error"]?.GetValue<string>());
                    }

                    else throw new Exception("Response nao mapeado: " + resp.Content);
                }

                return (false, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [GeneratedRegex("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,4}$", RegexOptions.IgnoreCase, "pt-BR")]
        private static partial Regex RegexEmail();
    }
}