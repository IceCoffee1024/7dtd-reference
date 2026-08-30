using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000218 RID: 536
[Preserve]
public class ConsoleCmdGameStage : ConsoleCmdAbstract
{
	// Token: 0x17000183 RID: 387
	// (get) Token: 0x06001048 RID: 4168 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000184 RID: 388
	// (get) Token: 0x06001049 RID: 4169 RVA: 0x0005FE4F File Offset: 0x0005E04F
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x17000185 RID: 389
	// (get) Token: 0x0600104A RID: 4170 RVA: 0x0005FE4F File Offset: 0x0005E04F
	public override DeviceFlag AllowedDeviceTypesClient
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x0600104B RID: 4171 RVA: 0x00067039 File Offset: 0x00065239
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "gamestage" };
	}

	// Token: 0x0600104C RID: 4172 RVA: 0x00067049 File Offset: 0x00065249
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Shows the gamestage of the local player";
	}

	// Token: 0x0600104D RID: 4173 RVA: 0x00067050 File Offset: 0x00065250
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		EntityPlayer primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
		if (primaryPlayer != null)
		{
			string text = string.Empty;
			float num = 0f;
			float num2 = 0f;
			BiomeDefinition biomeStandingOn = primaryPlayer.biomeStandingOn;
			if (biomeStandingOn != null)
			{
				text = biomeStandingOn.m_sBiomeName;
			}
			if (primaryPlayer.QuestJournal.ActiveQuest != null)
			{
				num = primaryPlayer.QuestJournal.ActiveQuest.QuestClass.GameStageMod;
				num2 = primaryPlayer.QuestJournal.ActiveQuest.QuestClass.GameStageBonus;
			}
			Log.Out("\nPlayer Game Stage = (((Player Level * (1 + Biome Modifier + Quest Modifier) + Days Alive) + Biome Bonus + Quest Bonus) * Difficulty Bonus) * Global Game Stage Modifier\"\nPlayer Game Stage {0} = ((({2} * (1 + {3} + {4}) + {5}) + {6} + {7}) * {8}) * {9}\nPlayer Game Stage {0}\nPlayer Level {2}\nBiome Modifier {3}\nQuest Modifier {4}\nDays Alive {5}\nBiome Bonus {6}\nQuest Bonus {7}\nDifficulty Bonus {8}\nGlobal Game Stage Modifier {9}\nPlayer Loot Stage {1}\nBiome {10}", new object[]
			{
				primaryPlayer.gameStage,
				primaryPlayer.GetLootStage(0f, 0f),
				primaryPlayer.Progression.Level,
				primaryPlayer.biomeStandingOn.GameStageMod,
				num,
				(long)((primaryPlayer.world.worldTime - primaryPlayer.gameStageBornAtWorldTime) / 24000UL),
				primaryPlayer.biomeStandingOn.GameStageBonus,
				num2,
				GameStageDefinition.DifficultyBonus,
				EntityPlayer.GlobalGameStageModifier,
				text
			});
		}
	}
}
