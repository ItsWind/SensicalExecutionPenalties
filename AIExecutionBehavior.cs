﻿using MCM.Abstractions.Base.Global;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Party;

namespace SensicalExecutionPenalties {
    public class AIExecutionBehavior : CampaignBehaviorBase {
        public override void RegisterEvents() {
            CampaignEvents.DailyTickHeroEvent.AddNonSerializedListener(this, TryExecutionForHero);
        }

        public override void SyncData(IDataStore dataStore) {
            //
        }

        private void TryExecutionForHero(Hero hero) {
            if (!GlobalSettings<MCMConfig>.Instance.AIExecution)
                return;

            if (hero.IsPrisoner) {
                PartyBase party = hero.PartyBelongedToAsPrisoner;
                if (!party.IsMobile)
                    return;

                Hero captor = party.LeaderHero;
                if (captor == null || captor == Hero.MainHero || !captor.IsAlive)
                    return;

                if (WouldHeroExecute(captor, hero))
                    KillCharacterAction.ApplyByExecution(hero, captor);
            }
        }

        private bool WouldHeroExecute(Hero captor, Hero prisoner) {
            int captorHonorLevel = captor.GetTraitLevel(DefaultTraits.Honor);
            int captorMercyLevel = captor.GetTraitLevel(DefaultTraits.Mercy);

            if (captorHonorLevel >= 1 || captorMercyLevel >= 1)
                return false;

            if (prisoner.IsEnemy(captor)) {
                int traitModifier = (captorHonorLevel * GlobalSettings<MCMConfig>.Instance.HonorModifier) + (captorMercyLevel * GlobalSettings<MCMConfig>.Instance.MercyModifier);
                int relation = captor.GetBaseHeroRelation(prisoner);

                int chanceOfExecuting = (int)Math.Round((double)Math.Abs(relation) / 2) - traitModifier;
                if (SubModule.Random.Next(1, 101) <= chanceOfExecuting)
                    return true;
            }

            return false;
        }
    }
}
