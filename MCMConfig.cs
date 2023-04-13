using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base.Global;

namespace SensicalExecutionPenalties {
	internal sealed class MCMConfig : AttributeGlobalSettings<MCMConfig> {
		public override string Id => "SensicalExecutionPenalties";
		public override string DisplayName => "Sensical Execution Penalties";
		public override string FolderName => "SensicalExecutionPenalties";
		public override string FormatType => "xml";

		[SettingPropertyBool("Enable Execution Relation Notifications", Order = 1, HintText = "Enables notifications for all 100-200+ heroes you change relations with.")]
		[SettingPropertyGroup("General")]
		public bool EnableRelationChanges { get; set; } = false;

		[SettingPropertyBool("Enable AI Execution", Order = 2, HintText = "Enables AI heroes to execute other AI heroes (or you probably).")]
		[SettingPropertyGroup("General")]
		public bool AIExecution { get; set; } = true;

		// REQUIREMENTS

		[SettingPropertyInteger("Minimum Age", 0, 100, Order = 1, HintText = "Set the minimum age a hero needs to react to an execution.")]
		[SettingPropertyGroup("Relation Change Requirements")]
		public int MinimumAge { get; set; } = 14;

		// TRAIT MODIFIERS

		[SettingPropertyInteger("Honor Modifier", 0, 100, Order = 1, HintText = "Multiplies this by the honor value to find a relation decrease/increase.")]
		[SettingPropertyGroup("Trait Modifiers")]
		public int HonorModifier { get; set; } = 10;

		[SettingPropertyInteger("Mercy Modifier", 0, 100, Order = 2, HintText = "Multiplies this by the mercy value to find a relation decrease/increase.")]
		[SettingPropertyGroup("Trait Modifiers")]
		public int MercyModifier { get; set; } = 20;

		// FACTIONS

		[SettingPropertyInteger("Same Faction", 0, 100, Order = 1, HintText = "Relation LOSS if executed faction member. This is multiplied by 4 if victim is a king/independent leader.")]
		[SettingPropertyGroup("Faction Relation Change")]
		public int SameFactionRelationLoss { get; set; } = 15;

		[SettingPropertyInteger("Enemy Faction", 0, 100, Order = 2, HintText = "Relation GAIN if executed enemy faction member. This is multiplied by 4 if victim is a king/independent leader.")]
		[SettingPropertyGroup("Faction Relation Change")]
		public int EnemyFactionRelationGain { get; set; } = 15;

		// FAMILIAL

		[SettingPropertyInteger("Spouse", 0, 100, Order = 1, HintText = "Relation LOSS if executed spouse.")]
		[SettingPropertyGroup("Familial Relation Change")]
		public int SpouseRelationLoss { get; set; } = 100;

		[SettingPropertyInteger("Parent/Child", 0, 100, Order = 2, HintText = "Relation LOSS if executed parent/child.")]
		[SettingPropertyGroup("Familial Relation Change")]
		public int ParentChildRelationLoss { get; set; } = 75;

		[SettingPropertyInteger("Sibling", 0, 100, Order = 3, HintText = "Relation LOSS if executed sibling.")]
		[SettingPropertyGroup("Familial Relation Change")]
		public int SiblingRelationLoss { get; set; } = 60;

		[SettingPropertyInteger("Same Clan", 0, 100, Order = 4, HintText = "Relation LOSS if executed clan member.")]
		[SettingPropertyGroup("Familial Relation Change")]
		public int SameClanRelationLoss { get; set; } = 20;
	}
}
