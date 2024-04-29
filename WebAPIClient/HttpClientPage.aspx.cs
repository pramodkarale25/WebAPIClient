using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;

namespace WebAPIClient
{
    public partial class HttpClientPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_Click(object sender, EventArgs e)
        {
            //GetAllEmployee();
            //CallCachingController();
            //GetToken();
            //TestTokenAuth();
            TestHttpVerb();
        }

        /// <summary>
        /// Basic Auth
        /// </summary>
        private void GetAllEmployee()
        {
            HttpClient client = new HttpClient();

            HttpRequestMessage request = new HttpRequestMessage()
            {
                RequestUri = new Uri("http://localhost:55291/api/TokenAuthentication"),
                Method = HttpMethod.Get,
            };

            string UNPass = Convert.ToBase64String(Encoding.Default.GetBytes("AdminUser:123456"));
            request.Headers.Add("Authorization", "Basic " + UNPass);
            //Need to change the PORT number where your WEB API service is running
            Task<HttpResponseMessage> employee = client.SendAsync(request);

            if (employee.Result.IsSuccessStatusCode)
            {
                var JsonContent = employee.Result.Content.ReadAsStringAsync().Result;
                List<Employee> empList = JsonConvert.DeserializeObject<List<Employee>>(JsonContent);
                foreach (var emp in empList)
                {
                    ulEmployees.InnerHtml += "<li>Name = " + emp.Name + " Gender = " + emp.Gender + " Dept = " + emp.Dept + " Salary = " + emp.Salary + "</li>";
                }
            }
        }

        private void CallCachingController()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:55291");
            client.DefaultRequestHeaders.Add("", "");
            Task<HttpResponseMessage> res = client.GetAsync("api/Caching/GetData");
            res.Wait();
            var result = res.Result;

            if (result.IsSuccessStatusCode)
            {
                var content = result.Content.ReadAsAsync<string>();
                content.Wait();
            }
        }

        private void TestTokenAuth()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:55291");
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + GetToken());

            var result = client.GetAsync("api/resource1").Result;

            if (result.IsSuccessStatusCode)
            {
                var res = result.Content.ReadAsAsync<string>();
            }
        }

        private void TestHttpVerb()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:55291");
            //client.DefaultRequestHeaders.Add("Accept", "application/json");

            //delete
            var result = client.DeleteAsync("api/HttpVerb?id=1").Result.Content.ReadAsStringAsync();

            //get
            User a = client.GetAsync("api/HttpVerb?user=546").Result.Content.ReadAsAsync<User>().Result;

            //post
            User ba = client.PostAsJsonAsync<User>("api/POSTUSER", a).Result.Content.ReadAsAsync<User>().Result;

            //put
            var put = client.PutAsJsonAsync<User>("api/HttpVerb", a).Result.Content.ReadAsAsync<string>().Result;


            //patch
            HttpRequestMessage request = new HttpRequestMessage()
            {
                RequestUri = new Uri("http://localhost:55291/api/PatchResource"),
                Method = new HttpMethod("PATCH"),
                Content = new ObjectContent<User>(a, new JsonMediaTypeFormatter(),"application/json")
            };
            var patch = client.SendAsync(request).Result.Content.ReadAsAsync<string>().Result;
        }

        private string GetToken()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:55291");

            var form = new Dictionary<string, string>
               {
                   {"grant_type", "password"},
                   {"username", "AdminUser"},
                   {"password", "123456"},
               };

            var res = client.PostAsync("token", new FormUrlEncodedContent(form));
            res.Wait();

            Task<TokenResponse> tokenResponse = res.Result.Content.ReadAsAsync<TokenResponse>();
            tokenResponse.Wait();

            TokenResponse x = tokenResponse.Result;
            return x.access_token;
        }
    }

    public class Employee
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Dept { get; set; }
        public int Salary { get; set; }
    }

    public class TokenResponse
    {
        public string access_token;
        public string token_type;
        public string expires_in;
    }

    public class User
    {
        public int id { get; set; }
        public string name { get; set; }
        public string salary { get; set; }
    }
}