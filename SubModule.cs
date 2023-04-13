using HarmonyLib;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SensicalExecutionPenalties {
    public class SubModule : MBSubModuleBase {
        public static Random Random = new();

        public static bool HideRelationNotifications = false;

        protected override void OnSubModuleLoad() {
            new Harmony("SensicalExecutionPenalties").PatchAll();
        }

        protected override void OnGameStart(Game game, IGameStarter starterObject) {
            starterObject.AddModel(new NewExecutionRelationModel());

            if (game.GameType is Campaign) {
                CampaignGameStarter campaignStarter = (CampaignGameStarter)starterObject;

                campaignStarter.AddBehavior(new AIExecutionBehavior());
            }
        }

        public static void PrintMessage(string message) {
            InformationManager.DisplayMessage(new InformationMessage(message));
        }
    }
}