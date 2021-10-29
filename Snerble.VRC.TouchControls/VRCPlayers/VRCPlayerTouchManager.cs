using Snerble.VRC.TouchControls.Parameters;
using System;
using System.Collections.Generic;
using Log = MelonLoader.MelonLogger;

namespace Snerble.VRC.TouchControls.VRCPlayers
{
    public class VRCPlayerTouchManager
    {
        private static readonly List<ParameterDriver> _paramDrivers = new List<ParameterDriver>();

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

        private static void VRCPlayerUtils_CurrentPlayerReady(object sender, VRCPlayer player)
        {
            Clear();

#if DEBUG
            Log.Msg(ConsoleColor.Green, "Current player is ready"); 
#endif

            foreach (var d in player.gameObject.GetComponentsInChildren<DynamicBone>(true))
            {
                if (ParameterDriver.GetFromDynamicBone(d) is ParameterDriver driver)
                {
                    _paramDrivers.Add(driver);
                    Log.Msg("Bound '{0}'", d.gameObject.name);
                }
            }
        }

        private static void Clear() => _paramDrivers.Clear();

        [Hook]
        public static void OnUpdate()
        {
            try
            {
                foreach (var driver in _paramDrivers)
                    driver.Update();
            }
            catch (NullReferenceException) // Occurs when the avatar unloads
            {
                Clear();
            }
        }
    }
}
