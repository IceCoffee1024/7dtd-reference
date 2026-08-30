using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001844 RID: 6212
	public class TwitchVoteType
	{
		// Token: 0x0600BF7A RID: 49018 RVA: 0x004714A2 File Offset: 0x0046F6A2
		public bool IsInPreset(string preset)
		{
			return this.PresetNames == null || this.PresetNames.Contains(preset);
		}

		// Token: 0x0600BF7B RID: 49019 RVA: 0x004714BA File Offset: 0x0046F6BA
		public bool CanUse()
		{
			return this.Enabled && (this.MaxTimesPerDay == -1 || this.MaxTimesPerDay > this.CurrentDayCount);
		}

		// Token: 0x0600BF7C RID: 49020 RVA: 0x004714E0 File Offset: 0x0046F6E0
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

		// Token: 0x040090B5 RID: 37045
		public static string PropTitle = "title";

		// Token: 0x040090B6 RID: 37046
		public static string PropTitleKey = "title_key";

		// Token: 0x040090B7 RID: 37047
		public static string PropSpawnBlocked = "spawn_blocked";

		// Token: 0x040090B8 RID: 37048
		public static string PropExcludeTimeIndex = "exclude_time_index";

		// Token: 0x040090B9 RID: 37049
		public static string PropMaxTimesPerDay = "max_times_per_day";

		// Token: 0x040090BA RID: 37050
		public static string PropAllowedStartHour = "allowed_start_hour";

		// Token: 0x040090BB RID: 37051
		public static string PropAllowedEndHour = "allowed_end_hour";

		// Token: 0x040090BC RID: 37052
		public static string PropBloodMoonDay = "blood_moon_day";

		// Token: 0x040090BD RID: 37053
		public static string PropBloodMoonAllowed = "blood_moon_allowed";

		// Token: 0x040090BE RID: 37054
		public static string PropGuaranteedGroups = "guaranteed_group";

		// Token: 0x040090BF RID: 37055
		public static string PropCooldownOnEnd = "cooldown_on_end";

		// Token: 0x040090C0 RID: 37056
		public static string PropUseMystery = "use_mystery";

		// Token: 0x040090C1 RID: 37057
		public static string PropActionLockout = "action_lockout";

		// Token: 0x040090C2 RID: 37058
		public static string PropGroup = "group";

		// Token: 0x040090C3 RID: 37059
		public static string PropEnabled = "enabled";

		// Token: 0x040090C4 RID: 37060
		public static string PropVoteChoiceCount = "vote_choice_count";

		// Token: 0x040090C5 RID: 37061
		public static string PropAllowedWithActions = "allowed_with_actions";

		// Token: 0x040090C6 RID: 37062
		public static string PropIsBoss = "is_boss";

		// Token: 0x040090C7 RID: 37063
		public static string PropManualStart = "manual_start";

		// Token: 0x040090C8 RID: 37064
		public static string PropIcon = "icon";

		// Token: 0x040090C9 RID: 37065
		public static string PropPresets = "presets";

		// Token: 0x040090CA RID: 37066
		public string Name;

		// Token: 0x040090CB RID: 37067
		public string Title;

		// Token: 0x040090CC RID: 37068
		public string Icon;

		// Token: 0x040090CD RID: 37069
		public string Group;

		// Token: 0x040090CE RID: 37070
		public bool SpawnBlocked = true;

		// Token: 0x040090CF RID: 37071
		public bool BloodMoonDay = true;

		// Token: 0x040090D0 RID: 37072
		public bool BloodMoonAllowed = true;

		// Token: 0x040090D1 RID: 37073
		public bool CooldownOnEnd;

		// Token: 0x040090D2 RID: 37074
		public bool UseMystery;

		// Token: 0x040090D3 RID: 37075
		public bool ActionLockout;

		// Token: 0x040090D4 RID: 37076
		public bool AllowedWithActions = true;

		// Token: 0x040090D5 RID: 37077
		public int MaxTimesPerDay = -1;

		// Token: 0x040090D6 RID: 37078
		public int AllowedStartHour;

		// Token: 0x040090D7 RID: 37079
		public int AllowedEndHour = 24;

		// Token: 0x040090D8 RID: 37080
		public int VoteChoiceCount = 3;

		// Token: 0x040090D9 RID: 37081
		public int CurrentDayCount;

		// Token: 0x040090DA RID: 37082
		public string GuaranteedGroup = "";

		// Token: 0x040090DB RID: 37083
		public bool Enabled = true;

		// Token: 0x040090DC RID: 37084
		public bool ManualStart;

		// Token: 0x040090DD RID: 37085
		public bool IsBoss;

		// Token: 0x040090DE RID: 37086
		public List<string> PresetNames;
	}
}
