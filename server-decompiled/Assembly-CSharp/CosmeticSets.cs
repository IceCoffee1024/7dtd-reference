using System;
using System.Runtime.CompilerServices;

// Token: 0x0200123F RID: 4671
public static class CosmeticSets
{
	// Token: 0x04006F61 RID: 28513
	[TupleElementNames(new string[] { "Set", "Prefix", "Items", "Name", "Id" })]
	public static readonly ValueTuple<EntitlementSetEnum, string, ValueTuple<string, int>[]>[] All = new ValueTuple<EntitlementSetEnum, string, ValueTuple<string, int>[]>[]
	{
		new ValueTuple<EntitlementSetEnum, string, ValueTuple<string, int>[]>(EntitlementSetEnum.PopNPillsStore, "twitchdrops/popnpillsstoregearhat", new ValueTuple<string, int>[]
		{
			new ValueTuple<string, int>("armorPopNPillsHelmet", 1015)
		}),
		new ValueTuple<EntitlementSetEnum, string, ValueTuple<string, int>[]>(EntitlementSetEnum.PopNPillsStore, "twitchdrops/popnpillsstoregearoutfit", new ValueTuple<string, int>[]
		{
			new ValueTuple<string, int>("armorPopNPillsOutfit", 1016)
		}),
		new ValueTuple<EntitlementSetEnum, string, ValueTuple<string, int>[]>(EntitlementSetEnum.RedRidingRogue, "dlc/halloweencosmetic/redridingrogue", new ValueTuple<string, int>[]
		{
			new ValueTuple<string, int>("armorRedRidingRogueHelmet", 1008),
			new ValueTuple<string, int>("armorRedRidingRogueOutfit", 1009),
			new ValueTuple<string, int>("armorRedRidingRogueGloves", 1010),
			new ValueTuple<string, int>("armorRedRidingRogueBoots", 1011)
		}),
		new ValueTuple<EntitlementSetEnum, string, ValueTuple<string, int>[]>(EntitlementSetEnum.ScarletScoundrel, "dlc/halloweencosmetic/scarletscoundrel", new ValueTuple<string, int>[]
		{
			new ValueTuple<string, int>("armorScarletScoundrelHelmet", 1000),
			new ValueTuple<string, int>("armorScarletScoundrelOutfit", 1001),
			new ValueTuple<string, int>("armorScarletScoundrelGloves", 1002),
			new ValueTuple<string, int>("armorScarletScoundrelBoots", 1003)
		}),
		new ValueTuple<EntitlementSetEnum, string, ValueTuple<string, int>[]>(EntitlementSetEnum.ShotgunMessiahStore, "twitchdrops/shotgunmessiahstoregearhat", new ValueTuple<string, int>[]
		{
			new ValueTuple<string, int>("armorShotgunMessiahHelmet", 1014)
		}),
		new ValueTuple<EntitlementSetEnum, string, ValueTuple<string, int>[]>(EntitlementSetEnum.ShotgunMessiahStore, "twitchdrops/shotgunmessiahstoregearoutfit", new ValueTuple<string, int>[]
		{
			new ValueTuple<string, int>("armorShotgunMessiahOutfit", 1017)
		}),
		new ValueTuple<EntitlementSetEnum, string, ValueTuple<string, int>[]>(EntitlementSetEnum.WorkingStiffsStore, "twitchdrops/workingstiffsstoregearhat", new ValueTuple<string, int>[]
		{
			new ValueTuple<string, int>("armorWorkingStiffsHelmet", 1012)
		}),
		new ValueTuple<EntitlementSetEnum, string, ValueTuple<string, int>[]>(EntitlementSetEnum.WorkingStiffsStore, "twitchdrops/workingstiffsstoregearoutfit", new ValueTuple<string, int>[]
		{
			new ValueTuple<string, int>("armorWorkingStiffsOutfit", 1013)
		}),
		new ValueTuple<EntitlementSetEnum, string, ValueTuple<string, int>[]>(EntitlementSetEnum.WraithWoodWitch, "dlc/halloweencosmetic/wraithwoodwitch", new ValueTuple<string, int>[]
		{
			new ValueTuple<string, int>("armorWraithWoodWitchHelmet", 1004),
			new ValueTuple<string, int>("armorWraithWoodWitchOutfit", 1005),
			new ValueTuple<string, int>("armorWraithWoodWitchGloves", 1006),
			new ValueTuple<string, int>("armorWraithWoodWitchBoots", 1007)
		})
	};
}
