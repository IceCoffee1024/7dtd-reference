using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x020017ED RID: 6125
	public class UpdateMessage
	{
		// Token: 0x0600BDCF RID: 48591 RVA: 0x00463468 File Offset: 0x00461668
		[PublicizedFrom(EAccessModifier.Private)]
		public static string DifficultyValueLocalized()
		{
			int num = GameStats.GetInt(EnumGameStats.GameDifficulty) + 1;
			return Localization.Get(string.Format("goDifficulty{0}", num) + ((num == 2) ? "_nodefault" : ""), false, null);
		}

		// Token: 0x0600BDD0 RID: 48592 RVA: 0x004634AC File Offset: 0x004616AC
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool IsGameModded()
		{
			GameServerInfo gameServerInfo = (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer ? SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo : SingletonMonoBehaviour<ConnectionManager>.Instance.LastGameServerInfo);
			return gameServerInfo != null && gameServerInfo.GetValue(GameInfoBool.ModdedConfig);
		}

		// Token: 0x0600BDD1 RID: 48593 RVA: 0x004634EC File Offset: 0x004616EC
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

		// Token: 0x0600BDD2 RID: 48594 RVA: 0x00463548 File Offset: 0x00461748
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

		// Token: 0x0600BDD3 RID: 48595 RVA: 0x004635F0 File Offset: 0x004617F0
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

		// Token: 0x04008E2C RID: 36396
		public string updateSignature;

		// Token: 0x04008E2D RID: 36397
		public string status;

		// Token: 0x04008E2E RID: 36398
		public int[] actionCooldowns;

		// Token: 0x04008E2F RID: 36399
		public Dictionary<string, int> bitBalances;

		// Token: 0x04008E30 RID: 36400
		public Dictionary<string, bool> hasChatted;

		// Token: 0x04008E31 RID: 36401
		public string ActionPresetKey = TwitchManager.Current.CurrentActionPreset.Name;

		// Token: 0x04008E32 RID: 36402
		public string VotePresetKey = TwitchManager.Current.CurrentVotePreset.Name;

		// Token: 0x04008E33 RID: 36403
		public string EventPresetKey = TwitchManager.Current.CurrentEventPreset.Name;

		// Token: 0x04008E34 RID: 36404
		public int Difficulty = GameStats.GetInt(EnumGameStats.GameDifficulty) + 1;

		// Token: 0x04008E35 RID: 36405
		public int DayMinutes = GamePrefs.GetInt(EnumGamePrefs.DayNightLength);

		// Token: 0x04008E36 RID: 36406
		public int PPRate = (int)TwitchManager.Current.ViewerData.PointRate;

		// Token: 0x04008E37 RID: 36407
		public bool IsModded = UpdateMessage.IsGameModded();

		// Token: 0x04008E38 RID: 36408
		public int GoodRewardTime = TwitchManager.LeaderboardStats.GoodRewardTime;

		// Token: 0x04008E39 RID: 36409
		public string ActionPresetValue = TwitchManager.Current.CurrentActionPreset.Title;

		// Token: 0x04008E3A RID: 36410
		public string VotePresetValue = TwitchManager.Current.CurrentVotePreset.Title;

		// Token: 0x04008E3B RID: 36411
		public string EventPresetValue = TwitchManager.Current.CurrentEventPreset.Title;

		// Token: 0x04008E3C RID: 36412
		public string TopKillerValue;

		// Token: 0x04008E3D RID: 36413
		public string TopGoodValue;

		// Token: 0x04008E3E RID: 36414
		public string TopBadValue;

		// Token: 0x04008E3F RID: 36415
		public string BestHelperValue;

		// Token: 0x04008E40 RID: 36416
		public string TotalGoodActionsValue;

		// Token: 0x04008E41 RID: 36417
		public string TotalBadActionsValue;

		// Token: 0x04008E42 RID: 36418
		public string LargestPimpPotValue;

		// Token: 0x04008E43 RID: 36419
		public string DifficultyValue;

		// Token: 0x04008E44 RID: 36420
		public string DayCycleValue;

		// Token: 0x04008E45 RID: 36421
		public string PPRateValue;

		// Token: 0x04008E46 RID: 36422
		public string ModdedValue;
	}
}
