using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace COVID19.Termin.Bot
{
    public class CheckService:ICheckService
    {
        private class ResponseDto
        {
            public int Total { get; set; }
            public DateTime next_slot { get; set; }
        }
        private readonly IHttpClientFactory _clientFactory;

        public CheckService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public async Task<bool> CheckTermin()
        {
            var httpClient = _clientFactory.CreateClient();
            var r = await httpClient.GetStringAsync(
                "https://www.doctolib.de/impfung-covid-19-corona/munchen?ref_visit_motive_ids[]=6768&ref_visit_motive_ids[]=6936&ref_visit_motive_ids[]=7109&ref_visit_motive_ids[]=7978");
            var regex = new Regex("search-result-(\\d+)");
            foreach (Match match in regex.Matches(r))
            {
                var matchGroup = match.Groups[1];
                var response = await httpClient.GetStringAsync($"https://www.doctolib.de/search_results/{matchGroup.Value}.json?limit=6&insurance_sector=public&ref_visit_motive_ids%5B%5D=6768&ref_visit_motive_ids%5B%5D=6936&ref_visit_motive_ids%5B%5D=7109&ref_visit_motive_ids%5B%5D=7978&speciality_id=5593&search_result_format=json");
                var result = JsonConvert.DeserializeObject<ResponseDto>(response);
                if (result.Total > 0 || result.next_slot < DateTime.Now.AddDays(14))
                    return true;
            }

            return false;
        }
    }
}