using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x0200186A RID: 6250
	public class TwitchVoteType
	{
		// Token: 0x0600C0FF RID: 49407 RVA: 0x0047723E File Offset: 0x0047543E
		public bool IsInPreset(string preset)
		{
			return this.PresetNames == null || this.PresetNames.Contains(preset);
		}

		// Token: 0x0600C100 RID: 49408 RVA: 0x00477256 File Offset: 0x00475456
		public bool CanUse()
		{
			return this.Enabled && (this.MaxTimesPerDay == -1 || this.MaxTimesPerDay > this.CurrentDayCount);
		}

		// Token: 0x0600C101 RID: 49409 RVA: 0x0047727C File Offset: 0x0047547C
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

		// Token: 0x040091D1 RID: 37329
		public static string PropTitle = "title";

		// Token: 0x040091D2 RID: 37330
		public static string PropTitleKey = "title_key";

		// Token: 0x040091D3 RID: 37331
		public static string PropSpawnBlocked = "spawn_blocked";

		// Token: 0x040091D4 RID: 37332
		public static string PropExcludeTimeIndex = "exclude_time_index";

		// Token: 0x040091D5 RID: 37333
		public static string PropMaxTimesPerDay = "max_times_per_day";

		// Token: 0x040091D6 RID: 37334
		public static string PropAllowedStartHour = "allowed_start_hour";

		// Token: 0x040091D7 RID: 37335
		public static string PropAllowedEndHour = "allowed_end_hour";

		// Token: 0x040091D8 RID: 37336
		public static string PropBloodMoonDay = "blood_moon_day";

		// Token: 0x040091D9 RID: 37337
		public static string PropBloodMoonAllowed = "blood_moon_allowed";

		// Token: 0x040091DA RID: 37338
		public static string PropGuaranteedGroups = "guaranteed_group";

		// Token: 0x040091DB RID: 37339
		public static string PropCooldownOnEnd = "cooldown_on_end";

		// Token: 0x040091DC RID: 37340
		public static string PropUseMystery = "use_mystery";

		// Token: 0x040091DD RID: 37341
		public static string PropActionLockout = "action_lockout";

		// Token: 0x040091DE RID: 37342
		public static string PropGroup = "group";

		// Token: 0x040091DF RID: 37343
		public static string PropEnabled = "enabled";

		// Token: 0x040091E0 RID: 37344
		public static string PropVoteChoiceCount = "vote_choice_count";

		// Token: 0x040091E1 RID: 37345
		public static string PropAllowedWithActions = "allowed_with_actions";

		// Token: 0x040091E2 RID: 37346
		public static string PropIsBoss = "is_boss";

		// Token: 0x040091E3 RID: 37347
		public static string PropManualStart = "manual_start";

		// Token: 0x040091E4 RID: 37348
		public static string PropIcon = "icon";

		// Token: 0x040091E5 RID: 37349
		public static string PropPresets = "presets";

		// Token: 0x040091E6 RID: 37350
		public string Name;

		// Token: 0x040091E7 RID: 37351
		public string Title;

		// Token: 0x040091E8 RID: 37352
		public string Icon;

		// Token: 0x040091E9 RID: 37353
		public string Group;

		// Token: 0x040091EA RID: 37354
		public bool SpawnBlocked = true;

		// Token: 0x040091EB RID: 37355
		public bool BloodMoonDay = true;

		// Token: 0x040091EC RID: 37356
		public bool BloodMoonAllowed = true;

		// Token: 0x040091ED RID: 37357
		public bool CooldownOnEnd;

		// Token: 0x040091EE RID: 37358
		public bool UseMystery;

		// Token: 0x040091EF RID: 37359
		public bool ActionLockout;

		// Token: 0x040091F0 RID: 37360
		public bool AllowedWithActions = true;

		// Token: 0x040091F1 RID: 37361
		public int MaxTimesPerDay = -1;

		// Token: 0x040091F2 RID: 37362
		public int AllowedStartHour;

		// Token: 0x040091F3 RID: 37363
		public int AllowedEndHour = 24;

		// Token: 0x040091F4 RID: 37364
		public int VoteChoiceCount = 3;

		// Token: 0x040091F5 RID: 37365
		public int CurrentDayCount;

		// Token: 0x040091F6 RID: 37366
		public string GuaranteedGroup = "";

		// Token: 0x040091F7 RID: 37367
		public bool Enabled = true;

		// Token: 0x040091F8 RID: 37368
		public bool ManualStart;

		// Token: 0x040091F9 RID: 37369
		public bool IsBoss;

		// Token: 0x040091FA RID: 37370
		public List<string> PresetNames;
	}
}
