using DomainModels.Entities;
using DomainServices.Exceptions;
using DomainServices.Services.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using Infrastructure.Data.Context;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainServices.Services
{
    public class PriceService : BaseService, IPriceService
    {
        public PriceService(
            IUnitOfWork<DataContext> unitOfWork,
            IRepositoryFactory<DataContext> repositoryFactory
        ) : base(unitOfWork, repositoryFactory) { }

        public async Task<long> Create(Price price)
        {
            var unitOfWork = UnitOfWork.Repository<Price>();

            var priceResult = unitOfWork.Add(price);
            await UnitOfWork.SaveChangesAsync();

            return priceResult.Id;
        }

        public void Delete(long id)
        {
            var unitOfWork = UnitOfWork.Repository<Price>();
            var priceFound = GetById(id).Result;

            unitOfWork.Remove(priceFound);
            UnitOfWork.SaveChanges();
        }

        public async Task<Price> GetById(long id)
        {
            var repository = RepositoryFactory.Repository<Price>();

            var query = repository.SingleResultQuery()
                .AndFilter(x => x.Id.Equals(id));

            return await repository.SingleOrDefaultAsync(query);
        }

        public async Task<Price> GetPriceByValidity(DateTime departureTime)
        {
            var repository = RepositoryFactory.Repository<Price>();

            var query =  repository.SingleResultQuery()
                .AndFilter(x => x.InitialTerm < departureTime && x.FinalTerm >= departureTime);

            return await repository.SingleOrDefaultAsync(query);
        }

        public IEnumerable<Price> GetPrices()
        {
            var repository = RepositoryFactory.Repository<Price>();

            var query = repository.MultipleResultQuery();

            return repository.Search(query);
        }

        public void Update(long id, Price price)
        {
            var unitOfWork = UnitOfWork.Repository<Price>();

            unitOfWork.Update(price);
            UnitOfWork.SaveChanges();
        }

        public bool PriceExists(long id)
        {
            var repository = RepositoryFactory.Repository<Price>();

            var existsParking = repository.Any(x => x.Id.Equals(id));

            if (!existsParking)
                throw new NotFoundException($"Price not found for Id {id}.");

            return true;
        }
    }
}
