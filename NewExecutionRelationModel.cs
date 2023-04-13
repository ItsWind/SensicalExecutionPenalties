﻿using MCM.Abstractions.Base.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.GameComponents;

namespace SensicalExecutionPenalties {
    public class NewExecutionRelationModel : DefaultExecutionRelationModel {
        public static Dictionary<Hero, bool> IsHeroRuler = new();

        // Must be in a clan for hero to run through this natively.
        public override int GetRelationChangeForExecutingHero(Hero victim, Hero hero, out bool showQuickNotification) {
            showQuickNotification = false;
            int result = 0;

            if (!hero.IsHumanPlayerCharacter && hero.IsAlive && hero != victim && hero.Age >= GlobalSettings<MCMConfig>.Instance.MinimumAge) {
                // POSITIVE NUMBER IS BETTER MORALITY
                int traitModifier = (hero.GetTraitLevel(DefaultTraits.Honor) * GlobalSettings<MCMConfig>.Instance.HonorModifier) + (hero.GetTraitLevel(DefaultTraits.Mercy) * GlobalSettings<MCMConfig>.Instance.MercyModifier);
                
                // friend/enemy, relevant to hero
                if (hero.IsFriend(victim) || hero.IsEnemy(victim)) {
                    int relationChange = (int)Math.Round((double)hero.GetBaseHeroRelation(victim) / 2);

                    if (hero.IsFriend(victim) && traitModifier < 0)
                        relationChange -= Math.Abs(traitModifier);
                    else
                        relationChange -= traitModifier;

                    result -= relationChange;
                }

                // in the family
                if (hero.Spouse == victim)
                    result -= GlobalSettings<MCMConfig>.Instance.SpouseRelationLoss;
                else if (hero.Father == victim || victim.Father == hero || hero.Mother == victim || victim.Mother == hero)
                    result -= GlobalSettings<MCMConfig>.Instance.ParentChildRelationLoss;
                else if (hero.Siblings.Contains(victim) || victim.Siblings.Contains(hero))
                    result -= GlobalSettings<MCMConfig>.Instance.SiblingRelationLoss;
                else if (hero.Clan == victim.Clan)
                    result -= GlobalSettings<MCMConfig>.Instance.SameClanRelationLoss;

                // factions and wars
                if (hero.MapFaction == victim.MapFaction) {
                    int loss = GlobalSettings<MCMConfig>.Instance.SameFactionRelationLoss;
                    loss += Math.Abs(traitModifier);
                    if (IsHeroRuler[victim])
                        loss *= 4;
                    if (loss > 0)
                        loss = 0;
                    result -= loss;
                }

                if (hero.MapFaction != null && victim.MapFaction != null && hero.MapFaction.IsAtWarWith(victim.MapFaction)) {
                    int gain = GlobalSettings<MCMConfig>.Instance.EnemyFactionRelationGain;
                    gain -= Math.Abs(traitModifier);
                    if (IsHeroRuler[victim])
                        gain *= 4;
                    if (gain < 0)
                        gain = 0;
                    result += gain;
                }
            }

            return result;
        }
    }
}
