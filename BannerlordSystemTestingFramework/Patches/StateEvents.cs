using HarmonyLib;
using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace ModTestingFramework
{
    public class StateEvents
    {

        public static event Action<string> OnStateActivate;
        public static event Action OnMainMenuReady;

        public static GameState CurrentGameState { get; private set; }
        public static bool MainMenuReady { get; private set; } = false;

        [HarmonyPatch(typeof(GameState), "OnActivate")]
        class PatchOnInitialize
        {
            static void Postfix(GameState __instance)
            {
                CurrentGameState = __instance;
                OnStateActivate?.Invoke(__instance.ToString());
            }
        }

        [HarmonyPatch(typeof(MBInitialScreenBase), "OnFrameTick")]
        class MainMenuReadyPatch
        {
            static void Postfix()
            {
                if (!MainMenuReady && MBMusicManager.Current != null)
                {
                    MainMenuReady = true;
                    OnMainMenuReady?.Invoke();
                }
            }
        }
    }
}
