using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

namespace EmployeePayrollUsingREST_API
{
    [TestClass]

    public class RestSharpTestCase
    {
        RestClient client;

        [TestInitialize]
        public void Setup()
        {
            client = new RestClient("http://localhost:3000");
        }

        private IRestResponse getEmployeeList()
        {
            RestRequest request = new RestRequest("/employees", Method.GET);

            //act

            IRestResponse response = client.Execute(request);
            return response;
        }

        [TestMethod]
        public void onCallingGETApi_ReturnEmployeeList()
        {
            IRestResponse response = getEmployeeList();

            //assert
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
            List<Employee> dataResponse = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
            Assert.AreEqual(2, dataResponse.Count);
            foreach (var item in dataResponse)
            {
                System.Console.WriteLine("id: "+item.id+"Name: "+item.name+"Salary: "+item.Salary);
            }
        }
       
        [TestMethod]
        public void onCallinPostAPI_ShouldReturnAddedEmployee()
        {
            RestRequest request = new RestRequest("/employees", Method.POST);
            JObject jObjectbody = new JObject();
            jObjectbody.Add("name", "jhon");
            jObjectbody.Add("Salary", "23000");
            request.AddParameter("application/json", jObjectbody, ParameterType.RequestBody);

            //act
            IRestResponse response = client.Execute(request);
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.Created);
            Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual("jhon", dataResponse.name);
            Assert.AreEqual(150000, dataResponse.Salary);

        }

        [TestMethod]
        public void oncallingPUTAPI_ShouldReturnUpdatedEmployee()
        {
            //making a request for a particular employee to be updated
            RestRequest request = new RestRequest("employees/2", Method.PUT);
            JsonObject jobject = new JsonObject();
            jobject.Add("name", "Honey");
            jobject.Add("salary", 20000);
            request.AddParameter("application/json", jobject, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            //deserializing content added in json file
            Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
            //asserting for salary
            Assert.AreEqual(dataResponse.Salary, 120000);
            //writing content without deserializing from resopnse. 
            Console.WriteLine(response.Content);
        }

        [TestMethod]
        public void onCallingDeleteAPI_ShouldReturnSuccessStatus()
        {
            //request for deleting elements from json 
            RestRequest request = new RestRequest("employees/11", Method.DELETE);
            //executing request using rest client
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            //checking status codes.
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
        }

    }
}
    

   
