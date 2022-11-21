using UnityEngine.UI;
using LootLocker.Requests;
using UnityEngine;
//using static System.Net.Mime.MediaTypeNames;

public class LeaderboardController_1P : MonoBehaviour
{
    private int leaderboardID = 8491;      // LeaderboardID for one player
    public InputField NameUser_WON;
    public InputField NameUser_LOST;
    string NameUser;
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
        if (NameUser_WON.text == "Enter Name" && NameUser_LOST.text == "Enter Name")
        {
            NameUser = "Unnamed Player: " + memberID.ToString();
        }
        else
        {
            if (NameUser_WON.text == "Enter Name")
            {
                NameUser = NameUser_LOST.text;
            }
            else
            {
                NameUser = NameUser_WON.text;
            }
        }

        LootLockerSDKManager.SubmitScore(NameUser.ToString(), userScore, leaderboardID, (response) =>
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
