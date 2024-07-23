using Newtonsoft.Json;

namespace Questao2
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            string teamName = "Paris Saint-Germain";
            int year = 2013;
            int totalGoals = await getTotalScoredGoals(teamName, year);

            Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

            teamName = "Chelsea";
            year = 2014;
            totalGoals = await getTotalScoredGoals(teamName, year);

            Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);
        }

        public static async Task<int> getTotalScoredGoals(string team, int year)
        {
            int totalGoals = 0;
            int currentPage = 1;
            bool hasMorePages = true;

            using (HttpClient client = new HttpClient())
            {
                while (hasMorePages)
                {
                    string url = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team1={team}&page={currentPage}";
                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<ApiResponse>(jsonResponse);

                        foreach (var match in result.data)
                        {
                            totalGoals += int.Parse(match.team1goals);
                        }

                        hasMorePages = currentPage < result.total_pages;
                        currentPage++;
                    }
                    else
                    {
                        hasMorePages = false;
                    }
                }

                currentPage = 1;
                hasMorePages = true;

                while (hasMorePages)
                {
                    string url = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team2={team}&page={currentPage}";
                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<ApiResponse>(jsonResponse);

                        foreach (var match in result.data)
                        {
                            totalGoals += int.Parse(match.team2goals);
                        }

                        hasMorePages = currentPage < result.total_pages;
                        currentPage++;
                    }
                    else
                    {
                        hasMorePages = false;
                    }
                }
            }

            return totalGoals;
        }
    }
}