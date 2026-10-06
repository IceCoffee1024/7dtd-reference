using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001819 RID: 6169
	public class UpdateMessage
	{
		// Token: 0x0600BF27 RID: 48935 RVA: 0x004679A4 File Offset: 0x00465BA4
		[PublicizedFrom(EAccessModifier.Private)]
		public static string DifficultyValueLocalized()
		{
			int num = GameStats.GetInt(EnumGameStats.GameDifficulty) + 1;
			return Localization.Get(string.Format("goDifficulty{0}", num) + ((num == 2) ? "_nodefault" : ""), false, null);
		}

		// Token: 0x0600BF28 RID: 48936 RVA: 0x004679E8 File Offset: 0x00465BE8
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool IsGameModded()
		{
			GameServerInfo gameServerInfo = (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer ? SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo : SingletonMonoBehaviour<ConnectionManager>.Instance.LastGameServerInfo);
			return gameServerInfo != null && gameServerInfo.GetValue(GameInfoBool.ModdedConfig);
		}

		// Token: 0x0600BF29 RID: 48937 RVA: 0x00467A28 File Offset: 0x00465C28
		[PublicizedFrom(EAccessModifier.Private)]
		public static string ModdedValueLocalized()
		{
			GameServerInfo gameServerInfo = (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer ? SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo : SingletonMonoBehaviour<ConnectionManager>.Instance.LastGameServerInfo);
			if (gameServerInfo == null)
			{
				return "--";
			}
			if (!gameServerInfo.GetValue(GameInfoBool.ModdedConfig))
			{
				return Localization.Get("xuiComboYesNoOff", false, null);
			}
			return Localization.Get("xuiComboYesNoOn", false, null);
		}

		// Token: 0x0600BF2A RID: 48938 RVA: 0x00467A84 File Offset: 0x00465C84
		[PublicizedFrom(EAccessModifier.Private)]
		public static string GetLocalizedPPRateValue()
		{
			if (TwitchManager.Current.ViewerData.PointRate == 1f)
			{
				return Localization.Get("xuiTwitchPointGenerationStandard", false, null);
			}
			if (TwitchManager.Current.ViewerData.PointRate == 2f)
			{
				return Localization.Get("xuiTwitchPointGenerationDouble", false, null);
			}
			if (TwitchManager.Current.ViewerData.PointRate == 3f)
			{
				return Localization.Get("xuiTwitchPointGenerationTriple", false, null);
			}
			if (TwitchManager.Current.ViewerData.PointRate == 0f)
			{
				return Localization.Get("goDisabled", false, null);
			}
			return Localization.Get("xuiTwitchPointGenerationStandard", false, null);
		}

		// Token: 0x0600BF2B RID: 48939 RVA: 0x00467B2C File Offset: 0x00465D2C
		public UpdateMessage()
		{
			TwitchLeaderboardStats.StatEntry topKillerViewer = TwitchManager.LeaderboardStats.TopKillerViewer;
			this.TopKillerValue = ((topKillerViewer != null) ? topKillerViewer.Name : null) ?? "--";
			TwitchLeaderboardStats.StatEntry topGoodViewer = TwitchManager.LeaderboardStats.TopGoodViewer;
			this.TopGoodValue = ((topGoodViewer != null) ? topGoodViewer.Name : null) ?? "--";
			TwitchLeaderboardStats.StatEntry topBadViewer = TwitchManager.LeaderboardStats.TopBadViewer;
			this.TopBadValue = ((topBadViewer != null) ? topBadViewer.Name : null) ?? "--";
			TwitchLeaderboardStats.StatEntry currentGoodViewer = TwitchManager.LeaderboardStats.CurrentGoodViewer;
			this.BestHelperValue = ((currentGoodViewer != null) ? currentGoodViewer.Name : null) ?? "--";
			this.TotalGoodActionsValue = TwitchManager.LeaderboardStats.TotalGood.ToString();
			this.TotalBadActionsValue = TwitchManager.LeaderboardStats.TotalBad.ToString();
			this.LargestPimpPotValue = TwitchManager.LeaderboardStats.LargestPimpPot.ToString();
			this.DifficultyValue = UpdateMessage.DifficultyValueLocalized();
			this.DayCycleValue = string.Format(Localization.Get("goMinutes", false, null), GamePrefs.GetInt(EnumGamePrefs.DayNightLength));
			this.PPRateValue = UpdateMessage.GetLocalizedPPRateValue();
			this.ModdedValue = UpdateMessage.ModdedValueLocalized();
			base..ctor();
		}

		// Token: 0x04008EF2 RID: 36594
		public string updateSignature;

		// Token: 0x04008EF3 RID: 36595
		public string status;

		// Token: 0x04008EF4 RID: 36596
		public int[] actionCooldowns;

		// Token: 0x04008EF5 RID: 36597
		public Dictionary<string, int> bitBalances;

		// Token: 0x04008EF6 RID: 36598
		public Dictionary<string, bool> hasChatted;

		// Token: 0x04008EF7 RID: 36599
		public string ActionPresetKey = TwitchManager.Current.CurrentActionPreset.Name;

		// Token: 0x04008EF8 RID: 36600
		public string VotePresetKey = TwitchManager.Current.CurrentVotePreset.Name;

		// Token: 0x04008EF9 RID: 36601
		public string EventPresetKey = TwitchManager.Current.CurrentEventPreset.Name;

		// Token: 0x04008EFA RID: 36602
		public int Difficulty = GameStats.GetInt(EnumGameStats.GameDifficulty) + 1;

		// Token: 0x04008EFB RID: 36603
		public int DayMinutes = GamePrefs.GetInt(EnumGamePrefs.DayNightLength);

		// Token: 0x04008EFC RID: 36604
		public int PPRate = (int)TwitchManager.Current.ViewerData.PointRate;

		// Token: 0x04008EFD RID: 36605
		public bool IsModded = UpdateMessage.IsGameModded();

		// Token: 0x04008EFE RID: 36606
		public int GoodRewardTime = TwitchManager.LeaderboardStats.GoodRewardTime;

		// Token: 0x04008EFF RID: 36607
		public string ActionPresetValue = TwitchManager.Current.CurrentActionPreset.Title;

		// Token: 0x04008F00 RID: 36608
		public string VotePresetValue = TwitchManager.Current.CurrentVotePreset.Title;

		// Token: 0x04008F01 RID: 36609
		public string EventPresetValue = TwitchManager.Current.CurrentEventPreset.Title;

		// Token: 0x04008F02 RID: 36610
		public string TopKillerValue;

		// Token: 0x04008F03 RID: 36611
		public string TopGoodValue;

		// Token: 0x04008F04 RID: 36612
		public string TopBadValue;

		// Token: 0x04008F05 RID: 36613
		public string BestHelperValue;

		// Token: 0x04008F06 RID: 36614
		public string TotalGoodActionsValue;

		// Token: 0x04008F07 RID: 36615
		public string TotalBadActionsValue;

		// Token: 0x04008F08 RID: 36616
		public string LargestPimpPotValue;

		// Token: 0x04008F09 RID: 36617
		public string DifficultyValue;

		// Token: 0x04008F0A RID: 36618
		public string DayCycleValue;

		// Token: 0x04008F0B RID: 36619
		public string PPRateValue;

		// Token: 0x04008F0C RID: 36620
		public string ModdedValue;
	}
}
