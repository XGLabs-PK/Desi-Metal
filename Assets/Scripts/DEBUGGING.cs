using TMPro;
using UnityEngine;

namespace XG.Studios
{
    public class Debugging : MonoBehaviour
    {
        const float PollingTime = 1f;
        int _frameCount;
        float _time;
        public TextMeshProUGUI fpsText;

        void Update()
        {
            _time += Time.deltaTime;
            _frameCount++;

            if (!(_time >= PollingTime)) return;
            int frameRate = Mathf.RoundToInt(_frameCount / _time);
            fpsText.text = $"DEBUGGING: {frameRate} FPS";
            _time -= PollingTime;
            _frameCount = 0;
        }
    }
}
