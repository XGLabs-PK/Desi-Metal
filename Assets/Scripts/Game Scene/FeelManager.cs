using MoreMountains.Feedbacks;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace XGStudios
{
    public class FeelManager : MonoBehaviour
    {
        public static FeelManager Instance;
        public MMFeedbacks carDamage;
        public MMFeedbacks carDestroyed;
        public MMFeedbacks enemyDamage;
        public MMFeedbacks enemyDestroyed;

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }
    }
}
