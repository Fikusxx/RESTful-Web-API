using Application.Persistence.Contracts;
using Domain;


namespace Persistence.Repositories;


public class LeaveTypeRepository : GenericRepository<LeaveType>, ILeaveTypeRepository
{
    private readonly ApplicationDbContext db;

    public LeaveTypeRepository(ApplicationDbContext db) : base(db)
    { 
        this.db = db;
    }
}
