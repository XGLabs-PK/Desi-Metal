using UnityEngine;
using Random = UnityEngine.Random;

namespace AllIn1VfxToolkit.Demo.Scripts
{
    [ExecuteInEditMode]
    public class AllIn1VfxParticleSystemTime : MonoBehaviour
    {
        [SerializeField]
        bool updateEveryFrame = true;

        [Header("If Y component is 0 X component will be used instead")]
        [SerializeField]
        Vector2 simulationTimeRange = Vector2.zero;

        [Space]
        [Header("If null we search in this GameObject")]
        [SerializeField]
        ParticleSystem targetPs;

        void Start()
        {
            SetSimulationTime();
        }

        void Update()
        {
            if (updateEveryFrame) SetSimulationTime();
        }

        void OnValidate()
        {
            SetSimulationTime();
        }

        bool isValid = true;

        void SetSimulationTime()
        {
            if (targetPs == null)
            {
                targetPs = GetComponent<ParticleSystem>();

                if (targetPs == null && isValid)
                {
                    Debug.LogError("The object " + gameObject.name +
                                   " has no Particle System and you are trying to access it. Please take a look");

                    isValid = false;
                }
            }

            if (!isValid) return;

            if (simulationTimeRange.y > 0f)
                targetPs.Simulate(Random.Range(simulationTimeRange.x, simulationTimeRange.y), true, true);
            else targetPs.Simulate(simulationTimeRange.x, true, true);
        }
    }
}
