using HarmonyLib;
using MCM.Abstractions.Base.Global;
using TaleWorlds.CampaignSystem.Actions;

namespace SensicalExecutionPenalties.Patches {
    [HarmonyPatch(typeof(KillCharacterAction), "ApplyInternal")]
    internal class KillCharacterActionPatch {
        [HarmonyPrefix]
        private static void Prefix(KillCharacterAction.KillCharacterActionDetail actionDetail) {
            // Replace true with mcm config
            if (actionDetail == KillCharacterAction.KillCharacterActionDetail.Executed) {
                if (!GlobalSettings<MCMConfig>.Instance.EnableRelationChanges)
                    SubModule.HideRelationNotifications = true;
                if (!GlobalSettings<MCMConfig>.Instance.EnableCharmXPGain)
                    SubModule.RelationChangeInProgress = true;
            }
        }

        [HarmonyPostfix]
        private static void Postfix() {
            SubModule.HideRelationNotifications = false;
            SubModule.RelationChangeInProgress = false;
        }
    }

    [HarmonyPatch(typeof(ChangeRelationAction), "ApplyInternal")]
    internal class ChangeRelationActionPatch {
        [HarmonyPrefix]
        private static void Prefix(ref bool showQuickNotification) {
            if (SubModule.HideRelationNotifications)
                showQuickNotification = false;
        }
    }
}
