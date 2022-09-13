using System;
using TMPro;
using UnityEngine;

namespace XG.Studios
{
    public class Debugging : MonoBehaviour
    {
        public TextMeshProUGUI fpsText;

        const float PollingTime = 1f;
        float _time;
        int _frameCount;

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
