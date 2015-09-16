
using System.Threading.Tasks;
using Broker.Domain.Models;

namespace Broker.Service.Contracts
{
    public interface ICarFinderService
    {
        Task<VehicleDetailsDto> FindVehicleByRegistrationNo(string regNo);
    }
}
