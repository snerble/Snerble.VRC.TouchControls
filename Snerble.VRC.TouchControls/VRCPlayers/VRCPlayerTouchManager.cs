using MelonLoader;
using Snerble.VRC.TouchControls.Parameters;
using Snerble.VRC.TouchControls.Touch;
using System;
using System.Collections.Generic;

namespace Snerble.VRC.TouchControls.VRCPlayers
{
    public class VRCPlayerTouchManager
    {
        private static SensorTest test;
        private static readonly List<TouchParameterDriver> _paramDrivers = new List<TouchParameterDriver>();

        [Hook]
        public static void OnApplicationStart()
        {
            VRCPlayerUtils.CurrentPlayerReady += VRCPlayerUtils_CurrentPlayerReady;
        }

        [Hook]
        public static void OnSceneWasUnloaded(int buildIndex, string sceneName)
        {
            Clear();
        }

        private static void VRCPlayerUtils_CurrentPlayerReady(object sender, VRCPlayer e)
        {
            Clear();

            test = new SensorTest();
            _paramDrivers.Add(new TouchParameterDriver(test.touchUnit, ParameterUtils.GetByName("Blade")));
        }

        private static void Clear()
        {
            test = null;
            _paramDrivers.Clear();
        }

        [Hook]
        public static void OnUpdate()
        {
            if (test == null)
                return;

            test.OnUpdate();

            foreach (var driver in _paramDrivers)
            {
                MelonLogger.Msg("Driving shit");
                driver.Update();
            }
        }
    }
}
