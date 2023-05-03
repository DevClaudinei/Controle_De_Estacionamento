using Application.Models.Request.Parking;
using Application.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppServices.Services.Interfaces
{
    public interface IParkingAppService
    {
        Task<long> CheckInParking(CheckInRequest checkInRequest);
        IEnumerable<ParkingResponse> GetAll();
        Task<CheckOutRequest> GetById(long id);
        Task<ParkingResponse> Get(long id);
        Task<ParkingResponse> GetByPlate(string plate);
        void CheckOutParking(long id, CheckOutRequest checkOutRequest);
        void ExcludeParkingInfo(long id);
    }
}
