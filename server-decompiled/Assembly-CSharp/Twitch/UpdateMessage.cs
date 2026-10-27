using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x0200184A RID: 6218
	public class UpdateMessage
	{
		// Token: 0x0600C03B RID: 49211 RVA: 0x0046B350 File Offset: 0x00469550
		[PublicizedFrom(EAccessModifier.Private)]
		public static string DifficultyValueLocalized()
		{
			int num = GameStats.GetInt(EnumGameStats.GameDifficulty) + 1;
			return Localization.Get(string.Format("goDifficulty{0}", num) + ((num == 2) ? "_nodefault" : ""), false, null);
		}

		// Token: 0x0600C03C RID: 49212 RVA: 0x0046B394 File Offset: 0x00469594
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool IsGameModded()
		{
			GameServerInfo gameServerInfo = (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer ? SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo : SingletonMonoBehaviour<ConnectionManager>.Instance.LastGameServerInfo);
			return gameServerInfo != null && gameServerInfo.GetValue(GameInfoBool.ModdedConfig);
		}

		// Token: 0x0600C03D RID: 49213 RVA: 0x0046B3D4 File Offset: 0x004695D4
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

		// Token: 0x0600C03E RID: 49214 RVA: 0x0046B430 File Offset: 0x00469630
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

		// Token: 0x0600C03F RID: 49215 RVA: 0x0046B4D8 File Offset: 0x004696D8
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

		// Token: 0x04008FDB RID: 36827
		public string updateSignature;

		// Token: 0x04008FDC RID: 36828
		public string status;

		// Token: 0x04008FDD RID: 36829
		public int[] actionCooldowns;

		// Token: 0x04008FDE RID: 36830
		public Dictionary<string, int> bitBalances;

		// Token: 0x04008FDF RID: 36831
		public Dictionary<string, bool> hasChatted;

		// Token: 0x04008FE0 RID: 36832
		public string ActionPresetKey = TwitchManager.Current.CurrentActionPreset.Name;

		// Token: 0x04008FE1 RID: 36833
		public string VotePresetKey = TwitchManager.Current.CurrentVotePreset.Name;

		// Token: 0x04008FE2 RID: 36834
		public string EventPresetKey = TwitchManager.Current.CurrentEventPreset.Name;

		// Token: 0x04008FE3 RID: 36835
		public int Difficulty = GameStats.GetInt(EnumGameStats.GameDifficulty) + 1;

		// Token: 0x04008FE4 RID: 36836
		public int DayMinutes = GamePrefs.GetInt(EnumGamePrefs.DayNightLength);

		// Token: 0x04008FE5 RID: 36837
		public int PPRate = (int)TwitchManager.Current.ViewerData.PointRate;

		// Token: 0x04008FE6 RID: 36838
		public bool IsModded = UpdateMessage.IsGameModded();

		// Token: 0x04008FE7 RID: 36839
		public int GoodRewardTime = TwitchManager.LeaderboardStats.GoodRewardTime;

		// Token: 0x04008FE8 RID: 36840
		public string ActionPresetValue = TwitchManager.Current.CurrentActionPreset.Title;

		// Token: 0x04008FE9 RID: 36841
		public string VotePresetValue = TwitchManager.Current.CurrentVotePreset.Title;

		// Token: 0x04008FEA RID: 36842
		public string EventPresetValue = TwitchManager.Current.CurrentEventPreset.Title;

		// Token: 0x04008FEB RID: 36843
		public string TopKillerValue;

		// Token: 0x04008FEC RID: 36844
		public string TopGoodValue;

		// Token: 0x04008FED RID: 36845
		public string TopBadValue;

		// Token: 0x04008FEE RID: 36846
		public string BestHelperValue;

		// Token: 0x04008FEF RID: 36847
		public string TotalGoodActionsValue;

		// Token: 0x04008FF0 RID: 36848
		public string TotalBadActionsValue;

		// Token: 0x04008FF1 RID: 36849
		public string LargestPimpPotValue;

		// Token: 0x04008FF2 RID: 36850
		public string DifficultyValue;

		// Token: 0x04008FF3 RID: 36851
		public string DayCycleValue;

		// Token: 0x04008FF4 RID: 36852
		public string PPRateValue;

		// Token: 0x04008FF5 RID: 36853
		public string ModdedValue;
	}
}
