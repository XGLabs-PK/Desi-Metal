using System;
using DiscordPresence;
using UnityEngine;

namespace XGStudios
{
    public class DiscordManager : MonoBehaviour
    {
        public string detail;
        public string state;

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            PresenceManager.UpdatePresence(
                detail:detail,
                state:state
                );
        }
    }
}
