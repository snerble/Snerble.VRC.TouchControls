using Snerble.VRC.TouchControls.Components;
using Snerble.VRC.TouchControls.Shared.Sensors;
using System;
using System.Collections.Generic;
using Log = MelonLoader.MelonLogger;

namespace Snerble.VRC.TouchControls.VRCPlayers
{
    public class VRCPlayerTouchManager
    {
        private static readonly List<Sensor> Sensors = new List<Sensor>();

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
                if (!d.gameObject.name.StartsWith(SensorConstants.SensorIdentifier))
                    continue;

                try
                {
                    var s = new Sensor(d.gameObject);
                    Sensors.Add(s);
                }
                catch (Exception ex)
                {
                    ex = ex.InnerException ?? ex;
                    Log.Error("Error while configuring sensor '{0}': {1}\n{2}",
                        d.gameObject.name,
                        ex.Message,
                        ex.StackTrace);
                }
            }
        }

        private static void Clear()
        {
            Sensors.ForEach(x => x.Dispose());
            Sensors.Clear();
        }
    }
}
