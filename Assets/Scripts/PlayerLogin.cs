using System.IO;
using LootLocker.Requests;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

namespace XGStudios
{
    public class PlayerLogin : MonoBehaviour
    {
        static PlayerLogin _instance;
        public int id = 6979;
        readonly int _maxScores = 7;
        public TextMeshProUGUI[] names;
        public TextMeshProUGUI[] killCounter;

        void Awake()
        {
            if (_instance == null)
                _instance = this;
            else
                Destroy(gameObject);
        }

        void Start()
        {
            LootLockerSDKManager.StartGuestSession(response =>
            {
                if (!response.success)
                {
                    Debug.Log("error starting LootLocker session");
                    return;
                }

                Debug.Log("successfully started LootLocker session");
            });
        }

        public void ShowScores()
        {
            LootLockerSDKManager.GetScoreList(id, _maxScores, response =>
            {
                if (response.statusCode == 200)
                {
                    Debug.Log("Successful " + response.text);
                    JsonTextReader reader = new JsonTextReader(new StringReader(response.text));
                    string prevValue = "";
                    int i = 0;

                    while (reader.Read())
                        if (reader.Value != null)
                        {
                            if (prevValue == "name")
                            {
                                names[i].text = reader.Value.ToString();
                                i++;
                            }
                            else if (prevValue == "score")
                                killCounter[i].text = reader.Value.ToString();

                            prevValue = reader.Value.ToString();
                        }
                }
                else
                    Debug.Log("failed: " + response.Error);
            });
        }
    }
}
