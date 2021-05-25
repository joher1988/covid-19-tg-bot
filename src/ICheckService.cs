using System.Threading.Tasks;

namespace COVID19.Termin.Bot
{
    public interface ICheckService
    {
        Task<bool> CheckTermin();
    }
}