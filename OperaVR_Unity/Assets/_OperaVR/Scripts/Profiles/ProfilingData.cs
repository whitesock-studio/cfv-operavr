using System;
using UnityEngine;

namespace OperaVR
{
    [CreateAssetMenu(menuName = "Profiling/Profiling Data", fileName = "Profiling_")]
    public class ProfilingData : ScriptableObject
    {
        public ProfilingRange[] Ranges = Array.Empty<ProfilingRange>();
        
        [Tooltip("This is the data returned when the value is higher than the last Range")]
        public ProfileData FallbackData;

        public ProfileData GetData(int value)
        {
            for (var i = 0; i < Ranges.Length; i++)
            {
                var range = Ranges[i];
                if (value > range.Value)
                {
                    continue;
                }
                return range.Data;
            }
            return FallbackData;
        }
    }

    [Serializable]
    public class ProfilingRange
    {
        public int Value;
        public ProfileData Data;
    }
}
