using System.Text.Json;
using WeatherApp.Models;

namespace WeatherApp.Services
{
    public class SearchHistoryService
    {
        private const string HistoryKey = "search_history";
        private const int MaxHistoryItems = 10;

        public void AddSearchHistory(string cityName, double latitude, double longitude)
        {
            try
            {
                var history = GetSearchHistory();

                var existingItem = history.FirstOrDefault(h => h.CityName.Equals(cityName, StringComparison.OrdinalIgnoreCase));
                if (existingItem != null)
                {
                    history.Remove(existingItem);
                }

                history.Insert(0, new SearchHistoryItem
                {
                    CityName = cityName,
                    Latitude = latitude,
                    Longitude = longitude,
                    SearchedAt = DateTime.Now
                });

                if (history.Count > MaxHistoryItems)
                {
                    history = history.Take(MaxHistoryItems).ToList();
                }

                SaveSearchHistory(history);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error adding search history: {ex.Message}");
            }
        }

        public List<SearchHistoryItem> GetSearchHistory()
        {
            try
            {
                var json = Preferences.Default.Get(HistoryKey, string.Empty);
                if (string.IsNullOrEmpty(json))
                    return new List<SearchHistoryItem>();

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<List<SearchHistoryItem>>(json, options) ?? new List<SearchHistoryItem>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting search history: {ex.Message}");
                return new List<SearchHistoryItem>();
            }
        }

        public void ClearSearchHistory()
        {
            try
            {
                Preferences.Default.Remove(HistoryKey);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error clearing search history: {ex.Message}");
            }
        }

        public void RemoveSearchHistory(string cityName)
        {
            try
            {
                var history = GetSearchHistory();
                history.RemoveAll(h => h.CityName.Equals(cityName, StringComparison.OrdinalIgnoreCase));
                SaveSearchHistory(history);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error removing search history: {ex.Message}");
            }
        }

        private void SaveSearchHistory(List<SearchHistoryItem> history)
        {
            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var json = JsonSerializer.Serialize(history, options);
                Preferences.Default.Set(HistoryKey, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving search history: {ex.Message}");
            }
        }
    }
}
