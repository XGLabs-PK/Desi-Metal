using LootLocker.Requests;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

namespace XG.Studios
{
    public class PlayerLogin : MonoBehaviour
    {
        public int id;
        readonly int _maxScores = 7;
        static PlayerLogin _instance = null;
        public TextMeshProUGUI[] names;
        public TextMeshProUGUI[] entries;

        void Awake()
        {
            if (_instance == null)
                _instance = this;
            else
                Destroy(gameObject);
        }

        void Start()
        {
            LootLockerSDKManager.StartGuestSession((response) =>
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
            LootLockerSDKManager.GetScoreList(id, _maxScores, (response) =>
            {
                if (response.statusCode == 200)
                {
                    Debug.Log("Successful " + response.text);
                    JsonTextReader reader = new JsonTextReader(new System.IO.StringReader(response.text));
                    string prevValue = "";
                    int i = 0;

                    while (reader.Read())
                    {
                        if (reader.Value != null)
                        {
                            if (prevValue == "name")
                            {
                                names[i].text = (reader.Value.ToString());
                                i++;
                            }
                            else if (prevValue == "score")
                            {
                                entries[i].text = (reader.Value.ToString());
                            }

                            prevValue = reader.Value.ToString();
                        }
                    }
                }
                else
                {
                    Debug.Log("failed: " + response.Error);
                }
            });
        }
    }
}
