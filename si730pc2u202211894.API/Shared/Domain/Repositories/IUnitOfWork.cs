namespace si730pc2u202211894.API.Shared.Domain.Repositories;

public interface IUnitOfWork
{
    Task CompleteAsync();
}