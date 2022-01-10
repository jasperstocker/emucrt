using System;
using UnityEngine;

namespace EmulationCRT
{
    [Serializable]
    public class Settings
    {
        public string name = "my settings";
        
        [Range(0, 5)] public float brightness = 1.5f;
        [Range(0, 10)] public float blur = 3.0f;
        [Range(0, 10)] public float blurSpread = 0.5f;
        [Range(0, 1)] public float blend = 1.0f;
        [Range(0, 1)] public float blackPoint = 0.0f;
        [Range(0, 1)] public float whitePoint = 1.0f;
        private EmuCRT.StretchModes _stretchModes = EmuCRT.StretchModes.x43;
    }
}