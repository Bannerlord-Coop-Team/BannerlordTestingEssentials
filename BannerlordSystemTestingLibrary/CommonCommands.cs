using TaleWorlds.Engine;

namespace ModTestingFramework
{
    class CommonCommands : TestCommands
    {
        public static void StartNewGame()
        {
            TaleWorlds.MountAndBlade.Module.CurrentModule.ExecuteInitialStateOptionWithId("StoryModeNewGame");
        }

        public static void ExitGame(string[] args)
        {
            Utilities.QuitGame();
        }
    }
}
