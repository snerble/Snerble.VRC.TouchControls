using MelonLoader;
using Snerble.VRC.TouchControls.VRCPlayers;
using System;
using UnityEngine;
using Log = MelonLoader.MelonLogger;

namespace Snerble.VRC.TouchControls
{
    public class MyMod : MelonMod
    {
        private Lazy<SensorTest> _sensorTest;

        public override void OnApplicationStart()
        {
            Log.Msg("Application is live");

            HookHelper.InvokeHooks();
            OnConfigureHarmony();
        }

        public void OnConfigureHarmony()
        {
            HookHelper.InvokeHooks(new[] { HarmonyInstance });
        }

        public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
        {
            HookHelper.InvokeHooks(new object[] { buildIndex, sceneName });
        }

        public override void OnUpdate()
        {
            HookHelper.InvokeHooks();

            if (Input.GetKeyDown(KeyCode.T))
            {
                //VRCPlayerHooks.DumpExpressionParams(VRCPlayer.field_Internal_Static_VRCPlayer_0);
                //_sensorTest = new Lazy<SensorTest>(() => new SensorTest());
            }

            if (_sensorTest != null)
            {
                _sensorTest.Value.OnUpdate();
            }
        }
    }
}
