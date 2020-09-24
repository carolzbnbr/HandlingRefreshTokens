using System.Collections.Generic;
using System.Threading.Tasks;
using HandlingRefreshTokens.Models;

namespace HandlingRefreshTokens.Services
{
    public interface IPeopleService
    {
        Task<IEnumerable<PersonInfo>> GetPeopleAsync();
    }
}