using MelonLoader;
using System;
using UnityEngine;
using Log = MelonLoader.MelonLogger;

namespace Snerble.VRC.TouchControls
{
    public class TouchControlsMod : MelonMod
    {
        public override void OnApplicationStart()
        {
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
        }
    }
}
