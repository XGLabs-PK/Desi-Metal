using System;
using MoreMountains.Feedbacks;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace XGStudios.GameScene
{
    public class FeelManager : MonoBehaviour
    {
        public static FeelManager Instance;

        public MMFeedbacks weaponFiring;
        public MMFeedbacks carOnLowHealth;
        public MMFeedbacks carDamage;
        public MMFeedbacks enemyDamage;
        public MMFeedbacks carDestroyed;

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }
    }
}