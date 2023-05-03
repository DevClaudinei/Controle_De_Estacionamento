using DomainModels.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainServices.Services.Interfaces
{
    public interface IParkingService
    {
        Task<long> CheckInParking(Parking parking);
        IEnumerable<Parking> GetAll();
        Task<Parking> GetById(long id);
        Task<Parking> GetByPlate(string plate);
        void CheckOutParking(long id, Parking parking);
        void ExcludeParkingInfo(long id);
    }
}
