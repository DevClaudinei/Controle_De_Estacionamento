using DomainModels.Entities;
using DomainServices.Exceptions;
using DomainServices.Services.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using Infrastructure.Data.Context;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainServices.Services
{
    public class ParkingService : BaseService, IParkingService
    {
        public ParkingService(
            IUnitOfWork<DataContext> unitOfWork,
            IRepositoryFactory<DataContext> repositoryFactory
        ) : base(unitOfWork, repositoryFactory) { }

        public async Task<long> CheckInParking(Parking parking)
        {
            var unitOfWork = UnitOfWork.Repository<Parking>();

            var parkingResult = unitOfWork.Add(parking);
            await UnitOfWork.SaveChangesAsync();

            return parkingResult.Id;
        }

        public IEnumerable<Parking> GetAll()
        {
            var repository = RepositoryFactory.Repository<Parking>();

            var query = repository.MultipleResultQuery();

            return repository.Search(query);
        }

        public Task<Parking> GetByIdId(long id)
        {
            var repository = RepositoryFactory.Repository<Parking>();

            var query = repository.SingleResultQuery()
                .AndFilter(x => x.Id.Equals(id));

            return repository.SingleOrDefaultAsync(query);
        }

        public Task<Parking> GetByPlate(string plate)
        {
            var repository = RepositoryFactory.Repository<Parking>();

            var query = repository.SingleResultQuery()
                .AndFilter(x => x.Plate.Equals(plate));

            return repository.SingleOrDefaultAsync(query);
        }

        public void CheckOutParking(long id, Parking parking)
        {
            var unitOfWork = UnitOfWork.Repository<Parking>();
            var repository = RepositoryFactory.Repository<Parking>();

            var existsParking = repository.Any(x => x.Id.Equals(id));

            if (!existsParking) 
                throw new NotFoundException($"Parking not found for Id {id}.");

            unitOfWork.Update(parking);
            UnitOfWork.SaveChanges();
        }

        public void ExcludeParkingInfo(long id)
        {
            var unitOfWork = UnitOfWork.Repository<Parking>();

            var parkingFound = GetByIdId(id).Result;

            unitOfWork.Remove(parkingFound);
            UnitOfWork.SaveChanges();
        }
    }
}
