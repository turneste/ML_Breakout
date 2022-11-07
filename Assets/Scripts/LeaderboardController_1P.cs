using UnityEngine.UI;
using LootLocker.Requests;
using UnityEngine;
//using static System.Net.Mime.MediaTypeNames;

public class LeaderboardController_1P : MonoBehaviour
{
    private int leaderboardID = 8491;      // LeaderboardID for one player
    public InputField NameUser;
    //private int MAX_SCORES = 10;
    //public Text[] Entries;

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

    public void SubmitScoreOnePlayer()
    {
        System.Random rnd = new System.Random();
        int userScore = GameManager.Instance.ScoreUser;
        int memberID = rnd.Next(100000, 999999999);

        // Use unique/random player # if name is not entered
        if (NameUser.text == "Enter Name")
        {
            NameUser.text = "Unnamed Player: " + memberID.ToString();
        }

        LootLockerSDKManager.SubmitScore(NameUser.text.ToString(), userScore, leaderboardID, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Success");
            }

            else
            {
                Debug.Log("Failed Score Submission");
            }
        });
    }
}
