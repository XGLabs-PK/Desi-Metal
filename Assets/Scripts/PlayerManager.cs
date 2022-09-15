using LootLocker.Requests;
using TMPro;
using UnityEngine;

namespace XG.Studios
{
    public class PlayerManager : MonoBehaviour
    {
        public int id;
        int _playerScore;
        public TMP_InputField memberID;
        public TextMeshProUGUI playerScore;
        public TextMeshProUGUI scoreSubmitted;

        void Start()
        {
            scoreSubmitted.text = "";
        }

        public void SubmitScore()
        {
            Debug.Log(playerScore.text);
            int.TryParse(playerScore.text.Replace(".", ""), out _playerScore);

            LootLockerSDKManager.SetPlayerName(memberID.text, response =>
            {
                if (response.success)
                    LootLockerSDKManager.SubmitScore(memberID.text, _playerScore, id, response =>
                    {
                        if (response.statusCode == 200)
                        {
                            Debug.Log("Successful");
                            scoreSubmitted.text = response.success ? "Score Submitted" : "Score Submission Failed";
                        }
                        else
                            Debug.Log("Failed to submit score: " + response.Error);
                    });
                else
                    Debug.Log("Failed to set name: " + response.Error);
            });
        }
    }
}
