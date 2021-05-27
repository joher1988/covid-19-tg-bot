using System;
using System.Collections;
using System.Collections.Generic;
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
            public SearchResult search_result { get; set; }
        }
        private class SearchResult
        {
            public string city { get; set; }
            public string name_with_title { get; set; }
        }
        private readonly IHttpClientFactory _clientFactory;

        public CheckService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public async Task<IEnumerable<string>> CheckTermin()
        {
            var httpClient = _clientFactory.CreateClient();
            var r = await httpClient.GetStringAsync(
                "https://www.doctolib.de/impfung-covid-19-corona/munchen?ref_visit_motive_ids[]=6768&ref_visit_motive_ids[]=6936&ref_visit_motive_ids[]=7109&ref_visit_motive_ids[]=7978");
            var regex = new Regex("search-result-(\\d+)");

            var res = new List<string>();
            foreach (Match match in regex.Matches(r))
            {
                var matchGroup = match.Groups[1];
                var response = await httpClient.GetStringAsync($"https://www.doctolib.de/search_results/{matchGroup.Value}.json?limit=6&insurance_sector=public&ref_visit_motive_ids%5B%5D=6768&ref_visit_motive_ids%5B%5D=6936&ref_visit_motive_ids%5B%5D=7109&ref_visit_motive_ids%5B%5D=7978&speciality_id=5593&search_result_format=json");
                var result = JsonConvert.DeserializeObject<ResponseDto>(response);
                if (result.Total > 0 || result.next_slot < DateTime.Now.AddDays(14))
                   res.Add($"city:{result.search_result.city} name:{result.search_result.name_with_title} next_slot:{result.next_slot}");
            }

            return res;
        }
    }

    
}