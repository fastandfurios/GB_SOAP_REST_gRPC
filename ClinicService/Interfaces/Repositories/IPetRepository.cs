using ClinicService.Data.Infrastructure.Models;

namespace ClinicService.Interfaces.Repositories
{
    public interface IPetRepository : IRepository<Pet, int>
    {
    }
}
