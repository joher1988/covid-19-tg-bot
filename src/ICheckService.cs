using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace COVID19.Termin.Bot
{
    public interface ICheckService
    {
        Task<IEnumerable<string>> CheckTermin();
    }
}