using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class LeaderBoard : MonoBehaviour
{
    public static LeaderBoard Instance { get; private set; }

    [SerializeField] private Transform leaderboardContainer; // UI Panel chứa các mục leaderboard
    [SerializeField] private GameObject leaderboardEntryPrefab; // Prefab cho mỗi mục (TextMeshProUGUI hoặc UI element)
    [SerializeField] private Transform endGameLeaderboardContainer; // Container cho bảng xếp hạng cuối trận

    private List<AnimalProperties> players = new List<AnimalProperties>();
    private Dictionary<AnimalProperties, GameObject> entryMap = new Dictionary<AnimalProperties, GameObject>();
    private Dictionary<AnimalProperties, GameObject> endGameEntryMap = new Dictionary<AnimalProperties, GameObject>();

    void Awake()
    {
        Instance = this;
    }

    public void RegisterPlayer(AnimalProperties player)
    {
        if (!players.Contains(player))
        {
            players.Add(player);
            CreateLeaderboardEntry(player);
            UpdateLeaderboard();
        }
    }

    public void UnregisterPlayer(AnimalProperties player)
    {
        if (players.Contains(player))
        {
            players.Remove(player);
            if (entryMap.ContainsKey(player))
            {
                Destroy(entryMap[player]);
                entryMap.Remove(player);
            }
            UpdateLeaderboard();
        }
    }

    public void UpdateLeaderboard()
    {
        // Sắp xếp người chơi theo điểm (từ cao đến thấp)
        var sortedPlayers = players.OrderByDescending(p => p.point).ToList();

        // Cập nhật UI
        for (int i = 0; i < sortedPlayers.Count; i++)
        {
            var player = sortedPlayers[i];
            if (entryMap.ContainsKey(player))
            {
                var entry = entryMap[player];
                entry.transform.SetSiblingIndex(i); // Sắp xếp thứ tự hiển thị
                var text = entry.GetComponent<TextMeshProUGUI>();
                if (text != null)
                {
                    text.text = $"{player.animalName}: {player.point}";
                }
            }
        }
    }

    private void CreateLeaderboardEntry(AnimalProperties player)
    {
        // Tạo UI entry cho người chơi
        var entry = Instantiate(leaderboardEntryPrefab, leaderboardContainer);
        entryMap[player] = entry;
        var text = entry.GetComponent<TextMeshProUGUI>();
        if (text != null)
        {
            text.text = $"{player.animalName}: {player.point}";
        }
    }

    public void ShowEndGameLeaderboard(AnimalProperties[] animalProperties)
    {
        // Xóa các mục cũ trong bảng xếp hạng cuối trận
        foreach (var entry in endGameEntryMap.Values)
        {
            Destroy(entry);
        }
        endGameEntryMap.Clear();

        // Sắp xếp người chơi theo điểm
        var sortedPlayers = animalProperties.OrderByDescending(p => p.point).ToList();

        // Tạo mục mới cho bảng xếp hạng cuối trận
        for (int i = 0; i < sortedPlayers.Count; i++)
        {
            var player = sortedPlayers[i];
            var entry = Instantiate(leaderboardEntryPrefab, endGameLeaderboardContainer);
            endGameEntryMap[player] = entry;
            var text = entry.GetComponent<TextMeshProUGUI>();
            if (text != null)
            {
                text.text = $"#{i + 1} {player.animalName}: {player.point}";
            }
        }
    }
}
