using System.Net;
using System.Text;
using UsersManagement.Model;

namespace UsersManagement
{
    public class HttpClientFunctions
    {
        public static async Task<ApiResponse> PostAsync(string uri, string jsonContent)
        {
            try
            {
                StringContent data = new(jsonContent, Encoding.UTF8, "application/json");

                using HttpClient httpClient = new();

                HttpResponseMessage? httpResponse = await httpClient.PostAsync(uri, data);
                return new ApiResponse() { Success = httpResponse.IsSuccessStatusCode, Content = await httpResponse.Content.ReadAsStringAsync() };
            }
            catch (Exception ex)
            {
                if (ex.InnerException is not null && ex.InnerException.Message == "Nenhuma conexão pôde ser feita porque a máquina de destino as recusou ativamente.")
                    return new ApiResponse() { Success = false, Content = null, Error = ErrorTypes.ServerUnavaliable };

                throw;
            }
        }

        public static async Task<ApiResponse> GetAsync(string uri, string userToken)
        {
            try
            {
                using HttpClient httpClient = new();
                httpClient.DefaultRequestHeaders.Add("authorization", "bearer " + userToken);
                HttpResponseMessage httpResponse = await httpClient.GetAsync(uri);

                return new ApiResponse()
                {
                    Success = httpResponse.IsSuccessStatusCode,
                    Error = httpResponse.StatusCode == HttpStatusCode.Unauthorized ? ErrorTypes.Unauthorized : null,
                    Content = await httpResponse.Content.ReadAsStringAsync()
                };
            }
            catch (Exception ex)
            {
                if (ex.InnerException is not null && (ex.InnerException.Message == "Nenhuma conexão pôde ser feita porque a máquina de destino as recusou ativamente." || ex.InnerException.Message.Contains("Este host não é conhecido.")))
                    return new ApiResponse() { Success = false, Content = null, Error = ErrorTypes.ServerUnavaliable };

                throw;
            }
        }

    }
}
