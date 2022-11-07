using UnityEngine.UI;
using LootLocker.Requests;
using UnityEngine;
//using static System.Net.Mime.MediaTypeNames;

public class DisplayLeaderboard : MonoBehaviour
{
    private int leaderboardID = 8491;      // LeaderboardID for one player
    private int MAX_SCORES = 10;
    public Text[] Entries;

    private void Start()
    {
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                Debug.Log("Success");
            }

            else
            {
                Debug.Log("Failed Session Setup");
            }
        });
    }

    public void ShowScores()
    {
        LootLockerSDKManager.GetScoreList(leaderboardID, MAX_SCORES, (response) =>
        {
            if (response.success)
            {
                LootLockerLeaderboardMember[] scores = response.items;

                for (int i = 0; i < scores.Length; i++)
                {
                    Entries[i].text = (scores[i].rank + ".    " + scores[i].member_id + "  |  Score = " + scores[i].score);
                }

                if (scores.Length < MAX_SCORES)
                {
                    for (int i = scores.Length; i < MAX_SCORES; i++)
                    {
                        if (i < 9)
                        {
                            Entries[i].text = (i + 1).ToString() + ".    none";
                        }
                        else
                        {
                            Entries[i].text = (i + 1).ToString() + ".  none";
                        }
                    }
                }
            }

            else
            {
                Debug.Log("Failed Show Scores");
            }
        });
    }
}
