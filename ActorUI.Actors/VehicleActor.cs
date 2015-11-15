

using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ActorUI.Actors.Messages;
using Akka.Actor;
using Broker.Domain.Commands;
using Broker.Domain.Models;
using Broker.Domain.Queries;
using Broker.Persistance;
using CarFinder.Api.Contracts;


namespace ActorUI.Actors
{
    public class VehicleActor : ReceiveActor
    {
        private readonly Entities _context;

        public VehicleActor(Entities context)
        {
            _context = context;
            Ready();
        }

        /// <summary>
        /// the .PipeTo extention is the secret sauce here. The receive function must return a 
        /// response and .PipeTo is the conduit to return the result from the task
        /// https://petabridge.com/blog/akkadotnet-async-actors-using-pipeto/
        /// </summary>
        private void Ready()
        {
            Receive<FindCarFromLocalStorage>(req =>
            {
                var senderClosure = Sender;

                IVehicleReader vehicleReader = new VehicleReader(_context);

                vehicleReader.GetVehicleByRegNo(req.CarRegNo).ContinueWith(s => s.Result).PipeTo(senderClosure);
            });

            Receive<FindCarFromService>(req =>
            {
                var senderClosure = Sender;

                var uri = string.Format("api/car/{0}", req.CarRegNo);
                var client = new HttpClient {BaseAddress = req.ServiceLocation};

                // doesn't exist locally. go to external service
                client.GetAsync(uri).ContinueWith(httpRequest =>
                {
                    var response = httpRequest.Result;

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var returnedVehicle = response.Content.ReadAsAsync<VehicleMetaData>();

                        var vehicle = new VehicleDetailsDto
                        {
                            BodyType = returnedVehicle.Result.BodyType,
                            Colour = returnedVehicle.Result.Colour,
                            CurrentRegistration = returnedVehicle.Result.CurrentRegistration,
                            FuelType = returnedVehicle.Result.FuelType,
                            IsImport = returnedVehicle.Result.IsImport,
                            ManufYear = returnedVehicle.Result.ManufYear,
                            ModelDesc = returnedVehicle.Result.VehicleDesc,
                            ModelName =
                                string.Format("{0} {1}", returnedVehicle.Result.Make, returnedVehicle.Result.Model),
                            Transmission = returnedVehicle.Result.Transmission,
                            VehicleRef = returnedVehicle.Result.VehicleRef
                        };

                        return vehicle;

                    }

                    return null;
                },
                    TaskContinuationOptions.AttachedToParent &
                    TaskContinuationOptions.ExecuteSynchronously)
                    .PipeTo(senderClosure);
            });

            Receive<SaveVehicleDetails>(req =>
            {
                var senderClosure = Sender;
                IVehicleWriter vehicleWriter = new VehicleWriter(_context);

                vehicleWriter.AddVehicle(req.Vehicle).ContinueWith(s => s.Result).PipeTo(senderClosure);
            });
        }


        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(
                3, // no of retries
                30000, // 30 second timeout 
                x =>
                {
                    if (x is NotSupportedException || x is NotImplementedException) 
                        return Directive.Stop;

                    return Directive.Restart;
                });

        }

        /// <summary>
        /// https://petabridge.com/blog/how-actors-recover-from-failure-hierarchy-and-supervision/
        /// </summary>
        /// <param name="reason"></param>
        /// <param name="message"></param>
        protected override void PreRestart(Exception reason, object message)
        {
            Self.Tell(message);
        }

    }
}
