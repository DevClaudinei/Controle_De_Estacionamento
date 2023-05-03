using EntityFrameworkCore.UnitOfWork.Interfaces;
using Infrastructure.Data.Context;
using System;

namespace DomainServices.Services
{
    public abstract class BaseService
    {
        protected IUnitOfWork UnitOfWork;
        protected IRepositoryFactory RepositoryFactory;

        public BaseService(
            IUnitOfWork<DataContext> unitOfWork,
            IRepositoryFactory<DataContext> repositoryFactory
        )
        {
            UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            RepositoryFactory = repositoryFactory ?? (IRepositoryFactory)UnitOfWork;
        }
    }
}
