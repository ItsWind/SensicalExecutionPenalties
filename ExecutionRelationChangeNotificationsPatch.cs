using HarmonyLib;
using MCM.Abstractions.Base.Global;
using TaleWorlds.CampaignSystem.Actions;

namespace SensicalExecutionPenalties {
    [HarmonyPatch(typeof(KillCharacterAction), "ApplyInternal")]
    internal class KillCharacterActionPatch {
        [HarmonyPrefix]
        private static void Prefix(KillCharacterAction.KillCharacterActionDetail actionDetail) {
            // Replace true with mcm config
            if (!GlobalSettings<MCMConfig>.Instance.EnableRelationChanges && actionDetail == KillCharacterAction.KillCharacterActionDetail.Executed)
                SubModule.HideRelationNotifications = true;
        }

        [HarmonyPostfix]
        private static void Postfix() {
            SubModule.HideRelationNotifications = false;
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
