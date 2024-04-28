using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace WebAPIClient
{
    public partial class HttpClientPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_Click(object sender, EventArgs e)
        {
            HttpRequestMessage request = new HttpRequestMessage()
            {
                RequestUri = new Uri("http://localhost:51026/api/Employees"),
                Method = HttpMethod.Get,
            };

            HttpClient client = new HttpClient();
            string UNPass = Convert.ToBase64String(Encoding.Default.GetBytes("AdminUser:123456"));
            request.Headers.Add("Authorization", "Basic " + UNPass);
            //Need to change the PORT number where your WEB API service is running
            var result = client.SendAsync(request).Result;

            if (result.IsSuccessStatusCode)
            {
                var JsonContent = result.Content.ReadAsStringAsync().Result;
                List<Employee> empList = JsonConvert.DeserializeObject<List<Employee>>(JsonContent);
                foreach (var emp in empList)
                {
                    ulEmployees.InnerHtml +="<li>Name = " + emp.Name + " Gender = " + emp.Gender + " Dept = " + emp.Dept + " Salary = " + emp.Salary+"</li>";
                }
            }
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
}