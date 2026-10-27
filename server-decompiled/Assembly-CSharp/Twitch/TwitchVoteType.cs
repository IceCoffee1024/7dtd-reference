using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x020018C7 RID: 6343
	public class TwitchVoteType
	{
		// Token: 0x0600C36B RID: 50027 RVA: 0x0047F126 File Offset: 0x0047D326
		public bool IsInPreset(string preset)
		{
			return this.PresetNames == null || this.PresetNames.Contains(preset);
		}

		// Token: 0x0600C36C RID: 50028 RVA: 0x0047F13E File Offset: 0x0047D33E
		public bool CanUse()
		{
			return this.Enabled && (this.MaxTimesPerDay == -1 || this.MaxTimesPerDay > this.CurrentDayCount);
		}

		// Token: 0x0600C36D RID: 50029 RVA: 0x0047F164 File Offset: 0x0047D364
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

		// Token: 0x04009380 RID: 37760
		public static string PropTitle = "title";

		// Token: 0x04009381 RID: 37761
		public static string PropTitleKey = "title_key";

		// Token: 0x04009382 RID: 37762
		public static string PropSpawnBlocked = "spawn_blocked";

		// Token: 0x04009383 RID: 37763
		public static string PropExcludeTimeIndex = "exclude_time_index";

		// Token: 0x04009384 RID: 37764
		public static string PropMaxTimesPerDay = "max_times_per_day";

		// Token: 0x04009385 RID: 37765
		public static string PropAllowedStartHour = "allowed_start_hour";

		// Token: 0x04009386 RID: 37766
		public static string PropAllowedEndHour = "allowed_end_hour";

		// Token: 0x04009387 RID: 37767
		public static string PropBloodMoonDay = "blood_moon_day";

		// Token: 0x04009388 RID: 37768
		public static string PropBloodMoonAllowed = "blood_moon_allowed";

		// Token: 0x04009389 RID: 37769
		public static string PropGuaranteedGroups = "guaranteed_group";

		// Token: 0x0400938A RID: 37770
		public static string PropCooldownOnEnd = "cooldown_on_end";

		// Token: 0x0400938B RID: 37771
		public static string PropUseMystery = "use_mystery";

		// Token: 0x0400938C RID: 37772
		public static string PropActionLockout = "action_lockout";

		// Token: 0x0400938D RID: 37773
		public static string PropGroup = "group";

		// Token: 0x0400938E RID: 37774
		public static string PropEnabled = "enabled";

		// Token: 0x0400938F RID: 37775
		public static string PropVoteChoiceCount = "vote_choice_count";

		// Token: 0x04009390 RID: 37776
		public static string PropAllowedWithActions = "allowed_with_actions";

		// Token: 0x04009391 RID: 37777
		public static string PropIsBoss = "is_boss";

		// Token: 0x04009392 RID: 37778
		public static string PropManualStart = "manual_start";

		// Token: 0x04009393 RID: 37779
		public static string PropIcon = "icon";

		// Token: 0x04009394 RID: 37780
		public static string PropPresets = "presets";

		// Token: 0x04009395 RID: 37781
		public string Name;

		// Token: 0x04009396 RID: 37782
		public string Title;

		// Token: 0x04009397 RID: 37783
		public string Icon;

		// Token: 0x04009398 RID: 37784
		public string Group;

		// Token: 0x04009399 RID: 37785
		public bool SpawnBlocked = true;

		// Token: 0x0400939A RID: 37786
		public bool BloodMoonDay = true;

		// Token: 0x0400939B RID: 37787
		public bool BloodMoonAllowed = true;

		// Token: 0x0400939C RID: 37788
		public bool CooldownOnEnd;

		// Token: 0x0400939D RID: 37789
		public bool UseMystery;

		// Token: 0x0400939E RID: 37790
		public bool ActionLockout;

		// Token: 0x0400939F RID: 37791
		public bool AllowedWithActions = true;

		// Token: 0x040093A0 RID: 37792
		public int MaxTimesPerDay = -1;

		// Token: 0x040093A1 RID: 37793
		public int AllowedStartHour;

		// Token: 0x040093A2 RID: 37794
		public int AllowedEndHour = 24;

		// Token: 0x040093A3 RID: 37795
		public int VoteChoiceCount = 3;

		// Token: 0x040093A4 RID: 37796
		public int CurrentDayCount;

		// Token: 0x040093A5 RID: 37797
		public string GuaranteedGroup = "";

		// Token: 0x040093A6 RID: 37798
		public bool Enabled = true;

		// Token: 0x040093A7 RID: 37799
		public bool ManualStart;

		// Token: 0x040093A8 RID: 37800
		public bool IsBoss;

		// Token: 0x040093A9 RID: 37801
		public List<string> PresetNames;
	}
}
