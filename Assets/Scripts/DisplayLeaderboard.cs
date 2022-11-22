using UnityEngine.UI;
using LootLocker.Requests;
using UnityEngine;

public class DisplayLeaderboard : MonoBehaviour
{
    private int leaderboardID_1P = 8491;
    private int leaderboardID_2P = 8496;
    private int MAX_SCORES = 10;
    public Text[] Entries_1P;
    public Text[] Entries_2P;

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

    public void ShowScores_1P()
    {
        LootLockerSDKManager.GetScoreList(leaderboardID_1P, MAX_SCORES, (response) =>
        {
            if (response.success)
            {
                LootLockerLeaderboardMember[] scores = response.items;

                for (int i = 0; i < scores.Length; i++)
                {
                    Entries_1P[i].text = (scores[i].rank + ".    " + scores[i].member_id + "  |  Score = " + scores[i].score);
                }

                if (scores.Length < MAX_SCORES)
                {
                    for (int i = scores.Length; i < MAX_SCORES; i++)
                    {
                        if (i < 9)
                        {
                            Entries_1P[i].text = (i + 1).ToString() + ".    none";
                        }
                        else
                        {
                            Entries_1P[i].text = (i + 1).ToString() + ".  none";
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

    public void ShowScores_2P()
    {
        LootLockerSDKManager.GetScoreList(leaderboardID_2P, MAX_SCORES, (response) =>
        {
            if (response.success)
            {
                LootLockerLeaderboardMember[] scores = response.items;

                for (int i = 0; i < scores.Length; i++)
                {
                    Entries_2P[i].text = (scores[i].rank + ".    " + scores[i].member_id + "  |  Score = " + scores[i].score);
                }

                if (scores.Length < MAX_SCORES)
                {
                    for (int i = scores.Length; i < MAX_SCORES; i++)
                    {
                        if (i < 9)
                        {
                            Entries_2P[i].text = (i + 1).ToString() + ".    none";
                        }
                        else
                        {
                            Entries_2P[i].text = (i + 1).ToString() + ".  none";
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
