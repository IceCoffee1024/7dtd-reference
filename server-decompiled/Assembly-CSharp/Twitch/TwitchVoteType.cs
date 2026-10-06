using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001896 RID: 6294
	public class TwitchVoteType
	{
		// Token: 0x0600C257 RID: 49751 RVA: 0x0047B77A File Offset: 0x0047997A
		public bool IsInPreset(string preset)
		{
			return this.PresetNames == null || this.PresetNames.Contains(preset);
		}

		// Token: 0x0600C258 RID: 49752 RVA: 0x0047B792 File Offset: 0x00479992
		public bool CanUse()
		{
			return this.Enabled && (this.MaxTimesPerDay == -1 || this.MaxTimesPerDay > this.CurrentDayCount);
		}

		// Token: 0x0600C259 RID: 49753 RVA: 0x0047B7B8 File Offset: 0x004799B8
		public virtual void ParseProperties(DynamicProperties properties)
		{
			properties.ParseString(TwitchVoteType.PropTitle, ref this.Title);
			if (properties.Values.ContainsKey(TwitchVoteType.PropTitleKey))
			{
				this.Title = Localization.Get(properties.Values[TwitchVoteType.PropTitleKey], false, null);
			}
			properties.ParseString(TwitchVoteType.PropIcon, ref this.Icon);
			properties.ParseBool(TwitchVoteType.PropSpawnBlocked, ref this.SpawnBlocked);
			properties.ParseInt(TwitchVoteType.PropMaxTimesPerDay, ref this.MaxTimesPerDay);
			properties.ParseInt(TwitchVoteType.PropAllowedStartHour, ref this.AllowedStartHour);
			properties.ParseInt(TwitchVoteType.PropAllowedEndHour, ref this.AllowedEndHour);
			properties.ParseBool(TwitchVoteType.PropBloodMoonDay, ref this.BloodMoonDay);
			properties.ParseBool(TwitchVoteType.PropBloodMoonAllowed, ref this.BloodMoonAllowed);
			properties.ParseString(TwitchVoteType.PropGuaranteedGroups, ref this.GuaranteedGroup);
			properties.ParseBool(TwitchVoteType.PropCooldownOnEnd, ref this.CooldownOnEnd);
			properties.ParseBool(TwitchVoteType.PropUseMystery, ref this.UseMystery);
			properties.ParseBool(TwitchVoteType.PropActionLockout, ref this.ActionLockout);
			properties.ParseString(TwitchVoteType.PropGroup, ref this.Group);
			properties.ParseBool(TwitchVoteType.PropEnabled, ref this.Enabled);
			properties.ParseBool(TwitchVoteType.PropAllowedWithActions, ref this.AllowedWithActions);
			properties.ParseInt(TwitchVoteType.PropVoteChoiceCount, ref this.VoteChoiceCount);
			properties.ParseBool(TwitchVoteType.PropIsBoss, ref this.IsBoss);
			properties.ParseBool(TwitchVoteType.PropManualStart, ref this.ManualStart);
			if (properties.Values.ContainsKey(TwitchVoteType.PropPresets))
			{
				this.PresetNames = new List<string>();
				this.PresetNames.AddRange(properties.Values[TwitchVoteType.PropPresets].Split(',', StringSplitOptions.None));
			}
		}

		// Token: 0x04009297 RID: 37527
		public static string PropTitle = "title";

		// Token: 0x04009298 RID: 37528
		public static string PropTitleKey = "title_key";

		// Token: 0x04009299 RID: 37529
		public static string PropSpawnBlocked = "spawn_blocked";

		// Token: 0x0400929A RID: 37530
		public static string PropExcludeTimeIndex = "exclude_time_index";

		// Token: 0x0400929B RID: 37531
		public static string PropMaxTimesPerDay = "max_times_per_day";

		// Token: 0x0400929C RID: 37532
		public static string PropAllowedStartHour = "allowed_start_hour";

		// Token: 0x0400929D RID: 37533
		public static string PropAllowedEndHour = "allowed_end_hour";

		// Token: 0x0400929E RID: 37534
		public static string PropBloodMoonDay = "blood_moon_day";

		// Token: 0x0400929F RID: 37535
		public static string PropBloodMoonAllowed = "blood_moon_allowed";

		// Token: 0x040092A0 RID: 37536
		public static string PropGuaranteedGroups = "guaranteed_group";

		// Token: 0x040092A1 RID: 37537
		public static string PropCooldownOnEnd = "cooldown_on_end";

		// Token: 0x040092A2 RID: 37538
		public static string PropUseMystery = "use_mystery";

		// Token: 0x040092A3 RID: 37539
		public static string PropActionLockout = "action_lockout";

		// Token: 0x040092A4 RID: 37540
		public static string PropGroup = "group";

		// Token: 0x040092A5 RID: 37541
		public static string PropEnabled = "enabled";

		// Token: 0x040092A6 RID: 37542
		public static string PropVoteChoiceCount = "vote_choice_count";

		// Token: 0x040092A7 RID: 37543
		public static string PropAllowedWithActions = "allowed_with_actions";

		// Token: 0x040092A8 RID: 37544
		public static string PropIsBoss = "is_boss";

		// Token: 0x040092A9 RID: 37545
		public static string PropManualStart = "manual_start";

		// Token: 0x040092AA RID: 37546
		public static string PropIcon = "icon";

		// Token: 0x040092AB RID: 37547
		public static string PropPresets = "presets";

		// Token: 0x040092AC RID: 37548
		public string Name;

		// Token: 0x040092AD RID: 37549
		public string Title;

		// Token: 0x040092AE RID: 37550
		public string Icon;

		// Token: 0x040092AF RID: 37551
		public string Group;

		// Token: 0x040092B0 RID: 37552
		public bool SpawnBlocked = true;

		// Token: 0x040092B1 RID: 37553
		public bool BloodMoonDay = true;

		// Token: 0x040092B2 RID: 37554
		public bool BloodMoonAllowed = true;

		// Token: 0x040092B3 RID: 37555
		public bool CooldownOnEnd;

		// Token: 0x040092B4 RID: 37556
		public bool UseMystery;

		// Token: 0x040092B5 RID: 37557
		public bool ActionLockout;

		// Token: 0x040092B6 RID: 37558
		public bool AllowedWithActions = true;

		// Token: 0x040092B7 RID: 37559
		public int MaxTimesPerDay = -1;

		// Token: 0x040092B8 RID: 37560
		public int AllowedStartHour;

		// Token: 0x040092B9 RID: 37561
		public int AllowedEndHour = 24;

		// Token: 0x040092BA RID: 37562
		public int VoteChoiceCount = 3;

		// Token: 0x040092BB RID: 37563
		public int CurrentDayCount;

		// Token: 0x040092BC RID: 37564
		public string GuaranteedGroup = "";

		// Token: 0x040092BD RID: 37565
		public bool Enabled = true;

		// Token: 0x040092BE RID: 37566
		public bool ManualStart;

		// Token: 0x040092BF RID: 37567
		public bool IsBoss;

		// Token: 0x040092C0 RID: 37568
		public List<string> PresetNames;
	}
}
