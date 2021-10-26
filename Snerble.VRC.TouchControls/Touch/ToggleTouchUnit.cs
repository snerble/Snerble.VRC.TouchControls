﻿using System.Collections.Generic;
using UnityEngine;

namespace Snerble.VRC.TouchControls.Touch
{
    public sealed class ToggleTouchUnit : TouchUnit
    {
        private float _threshold = 0.9f;

        public ToggleTouchUnit(DynamicBone dynamicBone)
            : base(new DynamicBoneTouchSensor(dynamicBone), DynamicBoneColliderTouchProbe.FromDynamicBones(dynamicBone))
        {
        }

        public ToggleTouchUnit(TouchSensor sensor, IEnumerable<TouchProbe> probes) : base(sensor, probes)
        {
        }

        public float Threshold
        {
            get => _threshold;
            set => _threshold = Mathf.Clamp(value, 0, 1);
        }

        public bool IsSet { get; set; } = false;

        private bool Latch { get; set; } = false;

        public override float Measure()
        {
            float f = base.Measure();

            if (!Latch && f >= Threshold)
            {
                IsSet = !IsSet;
                Latch = true;
            }
            // Latch release
            else if (Latch && f < Threshold)
            {
                Latch = false;
            }

            return IsSet ? 1 : f;
        }
    }
}
