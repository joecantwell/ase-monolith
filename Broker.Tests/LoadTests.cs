// <copyright company="Action Point Innovation Ltd.">
// Copyright (c) 2013 All Right Reserved
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY 
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Broker.Domain.Models;
using NUnit.Framework;
using System.Timers;

namespace Broker.Tests
{
    public class LoadTests
    {
        private readonly string _regNo;
        private const int FakeUserCount = 5;
        private Timer _serviceTimer;
        private int _totalTimeInMilliseconds;
        private const int Interval = 10; // milliseconds

        public LoadTests()
        {

            _regNo = string.Format("151-T-{0}", new Random().Next(1000, 99000));
            _serviceTimer = new Timer();
            _serviceTimer.Elapsed += serviceTimer_Elapsed; ;
           _serviceTimer.Interval = Interval; 
        }


        void serviceTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _totalTimeInMilliseconds += Interval;
        }

        [Test]
        public async void LoadWebDeployment()
        {
            // Arrange
            Uri baseUri = new Uri("http://brokerui.azurewebsites.net/");

            // Act
            _totalTimeInMilliseconds = 0;
            _serviceTimer.Start();
            int count = await AddLoadToWebDeployment(baseUri, FakeUserCount, _regNo);

            // Assert
            _serviceTimer.Stop();

            Console.WriteLine("Total Time for {0} Concurrent Users was {1} milliseconds", FakeUserCount, _totalTimeInMilliseconds);
            Assert.AreEqual(FakeUserCount, count);

        }

        [Test]
        public async void LoadActorDeployment()
        {
            // Arrange
            Uri baseUri = new Uri("http://actorui.azurewebsites.net/");

            // Act
            _totalTimeInMilliseconds = 0;
            Console.WriteLine("Timer reset to {0}.", _totalTimeInMilliseconds);
            _serviceTimer.Start();
            int count = await AddLoadToWebDeployment(baseUri, FakeUserCount, _regNo);

            // Assert
            _serviceTimer.Stop();

            Console.WriteLine("Total Time for {0} Concurrent Users was {1} milliseconds", FakeUserCount, _totalTimeInMilliseconds);
            Assert.AreEqual(FakeUserCount, count);
        }


        private async Task<int> AddLoadToWebDeployment(Uri baseUri, int fakeUserCount, string regNo)
        {
            VehicleDetailsDto car = null;
            using (var client = new HttpClient { BaseAddress = baseUri })
            {
                var response = await client.GetAsync( string.Format("CarInsurance/CheckCar?regNo={0}", regNo)).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                car = response.Content.ReadAsAsync<VehicleDetailsDto>().Result;
            }


            var tasksToCallService = new List<Task>(); // task collection

            Uri endPoint = new Uri(baseUri, "CarInsurance/Create");
            for (int i = 0; i < fakeUserCount; i++)
            {
                var fakeUserCall = Task.Run(() =>
                {
                    using (var client = new WebClient())
                    {
                        client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                        // payload taken from fiddler
                        string payload = string.Format("JsonVehicle=%7B%22VehicleId%22%3A0%2C%22VehicleRef%22%3A%22{0}%22%2C%22ModelName%22%3A%22VolksWagen+Passat%22%2C%22ModelDesc%22%3A%222008+Volkswagen+Passat+1.9+TDI+Bluemotion+103+BHP+5+DR%22%2C%22ManufYear%22%3A2008%2C%22CurrentRegistration%22%3A%22151-t-34345%22%2C%22Colour%22%3A%22Blue%22%2C%22BodyType%22%3A%22Estate%22%2C%22FuelType%22%3A%22Diesel%22%2C%22Transmission%22%3A%22Manual%22%2C%22IsImport%22%3Atrue%2C%22UTCDateAdded%22%3Anull%7D&CarQuoteRequest.CountyId=22&CarQuoteRequest.NoClaimsDiscountYears=5&CarValue=34%2C000.00&CarQuoteRequest.DriverAge=44&CarQuoteRequest.EmailAddress=jc@test.io&CarQuoteRequest.Telephone=087365345252", car.VehicleRef);

                        byte[] content = Encoding.ASCII.GetBytes(payload);

                        client.UploadDataAsync(endPoint, "POST", content);
                    }
                });
               
               tasksToCallService.Add(fakeUserCall);
            }

            await Task.WhenAll(tasksToCallService);

            int count =
                tasksToCallService.Count(
                    resp => resp.Status == TaskStatus.RanToCompletion);

            return count;

        }
    }
}
