using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x020017C7 RID: 6087
	public class UpdateMessage
	{
		// Token: 0x0600BC4A RID: 48202 RVA: 0x0045D6CC File Offset: 0x0045B8CC
		[PublicizedFrom(EAccessModifier.Private)]
		public static string DifficultyValueLocalized()
		{
			int num = GameStats.GetInt(EnumGameStats.GameDifficulty) + 1;
			return Localization.Get(string.Format("goDifficulty{0}", num) + ((num == 2) ? "_nodefault" : ""), false, null);
		}

		// Token: 0x0600BC4B RID: 48203 RVA: 0x0045D710 File Offset: 0x0045B910
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool IsGameModded()
		{
			GameServerInfo gameServerInfo = (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer ? SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo : SingletonMonoBehaviour<ConnectionManager>.Instance.LastGameServerInfo);
			return gameServerInfo != null && gameServerInfo.GetValue(GameInfoBool.ModdedConfig);
		}

		// Token: 0x0600BC4C RID: 48204 RVA: 0x0045D750 File Offset: 0x0045B950
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

		// Token: 0x0600BC4D RID: 48205 RVA: 0x0045D7AC File Offset: 0x0045B9AC
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

		// Token: 0x0600BC4E RID: 48206 RVA: 0x0045D854 File Offset: 0x0045BA54
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

		// Token: 0x04008D10 RID: 36112
		public string updateSignature;

		// Token: 0x04008D11 RID: 36113
		public string status;

		// Token: 0x04008D12 RID: 36114
		public int[] actionCooldowns;

		// Token: 0x04008D13 RID: 36115
		public Dictionary<string, int> bitBalances;

		// Token: 0x04008D14 RID: 36116
		public Dictionary<string, bool> hasChatted;

		// Token: 0x04008D15 RID: 36117
		public string ActionPresetKey = TwitchManager.Current.CurrentActionPreset.Name;

		// Token: 0x04008D16 RID: 36118
		public string VotePresetKey = TwitchManager.Current.CurrentVotePreset.Name;

		// Token: 0x04008D17 RID: 36119
		public string EventPresetKey = TwitchManager.Current.CurrentEventPreset.Name;

		// Token: 0x04008D18 RID: 36120
		public int Difficulty = GameStats.GetInt(EnumGameStats.GameDifficulty) + 1;

		// Token: 0x04008D19 RID: 36121
		public int DayMinutes = GamePrefs.GetInt(EnumGamePrefs.DayNightLength);

		// Token: 0x04008D1A RID: 36122
		public int PPRate = (int)TwitchManager.Current.ViewerData.PointRate;

		// Token: 0x04008D1B RID: 36123
		public bool IsModded = UpdateMessage.IsGameModded();

		// Token: 0x04008D1C RID: 36124
		public int GoodRewardTime = TwitchManager.LeaderboardStats.GoodRewardTime;

		// Token: 0x04008D1D RID: 36125
		public string ActionPresetValue = TwitchManager.Current.CurrentActionPreset.Title;

		// Token: 0x04008D1E RID: 36126
		public string VotePresetValue = TwitchManager.Current.CurrentVotePreset.Title;

		// Token: 0x04008D1F RID: 36127
		public string EventPresetValue = TwitchManager.Current.CurrentEventPreset.Title;

		// Token: 0x04008D20 RID: 36128
		public string TopKillerValue;

		// Token: 0x04008D21 RID: 36129
		public string TopGoodValue;

		// Token: 0x04008D22 RID: 36130
		public string TopBadValue;

		// Token: 0x04008D23 RID: 36131
		public string BestHelperValue;

		// Token: 0x04008D24 RID: 36132
		public string TotalGoodActionsValue;

		// Token: 0x04008D25 RID: 36133
		public string TotalBadActionsValue;

		// Token: 0x04008D26 RID: 36134
		public string LargestPimpPotValue;

		// Token: 0x04008D27 RID: 36135
		public string DifficultyValue;

		// Token: 0x04008D28 RID: 36136
		public string DayCycleValue;

		// Token: 0x04008D29 RID: 36137
		public string PPRateValue;

		// Token: 0x04008D2A RID: 36138
		public string ModdedValue;
	}
}
