/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Galaxy.Math;

namespace Galaxy.Math {

    ///<summary>
    ///
    ///<summary>
    [System.Serializable]
    public class Range {

        public float Min;
        public float Max;

        public Range(float min, float max) {
            Min = min;
            Max = max;
        }

        public float Evaluate(float x) {
            if (x >= 1f) {
                return Max;
            }
            else if (x <= 0f) {
                return Min;
            }
            return (Max - Min) * x + Min;
        }

        public float Ratio(float v) {
            if (v >= Max) {
                return 1f;
            }
            else if (v <= Min) {
                return 0f;
            }
            return (v - Min) / (Max -Min);
        }

    }
    
}