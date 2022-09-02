using UnityEngine;

namespace AllIn1VfxToolkit.Demo.Scripts
{
    public class AllIn1DoShake : MonoBehaviour
    {
        [SerializeField]
        float shakeAmount = 0.15f;
        [SerializeField]
        bool doShakeOnStart;
        [SerializeField]
        float shakeOnStartDelay;

        void Start()
        {
            if (doShakeOnStart)
            {
                if (shakeOnStartDelay < Time.deltaTime) DoShake();
                else Invoke(nameof(DoShake), shakeOnStartDelay);
            }
        }

        public void DoShake()
        {
            AllIn1Shaker.i.DoCameraShake(shakeAmount);
        }
    }
}
