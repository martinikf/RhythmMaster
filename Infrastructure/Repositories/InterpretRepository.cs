using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class InterpretRepository : Repository<Interpret, PersonId>, IInterpretRepository
{
    public InterpretRepository(ApplicationDbContext dbContext) : base(dbContext)
    { }
}