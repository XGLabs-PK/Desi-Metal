using System;
using DiscordPresence;
using UnityEngine;

namespace XGStudios
{
    public class DiscordManager : MonoBehaviour
    {
        public string details;
        public string state;
        
        void Awake()
        { 
            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            PresenceManager.UpdatePresence(detail:details, state:state);
        }
    }
}
