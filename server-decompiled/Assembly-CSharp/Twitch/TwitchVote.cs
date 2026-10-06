using System;
using System.Collections.Generic;
using UnityEngine;

namespace Twitch
{
	// Token: 0x02001890 RID: 6288
	public class TwitchVote
	{
		// Token: 0x17001841 RID: 6209
		// (get) Token: 0x0600C240 RID: 49728 RVA: 0x0047ACB0 File Offset: 0x00478EB0
		public string VoteDescription
		{
			get
			{
				switch (this.DisplayType)
				{
				case TwitchVote.VoteDisplayTypes.Single:
				case TwitchVote.VoteDisplayTypes.Special:
					return this.Title;
				case TwitchVote.VoteDisplayTypes.GoodBad:
					return this.Title + " / " + this.VoteLine1;
				case TwitchVote.VoteDisplayTypes.HordeBuffed:
					return this.Title + " (" + this.VoteLine1 + ")";
				default:
					return "";
				}
			}
		}

		// Token: 0x17001842 RID: 6210
		// (get) Token: 0x0600C241 RID: 49729 RVA: 0x0047AD1C File Offset: 0x00478F1C
		public string VoteHeight
		{
			get
			{
				TwitchVote.VoteDisplayTypes displayType = this.DisplayType;
				if (displayType == TwitchVote.VoteDisplayTypes.Single || displayType == TwitchVote.VoteDisplayTypes.Special)
				{
					return "-50";
				}
				return "-90";
			}
		}

		// Token: 0x0600C242 RID: 49730 RVA: 0x0047AD42 File Offset: 0x00478F42
		public bool IsInPreset(TwitchVotePreset preset)
		{
			return (this.PresetNames == null && !preset.IsEmpty) || (this.PresetNames != null && this.PresetNames.Contains(preset.Name));
		}

		// Token: 0x0600C243 RID: 49731 RVA: 0x0047AD74 File Offset: 0x00478F74
		public bool CanUse(int hour, int gamestage, EntityPlayer player)
		{
			if ((this.StartGameStage != -1 && this.StartGameStage > gamestage) || (this.EndGameStage != -1 && this.EndGameStage < gamestage) || hour < this.AllowedStartHour || hour > this.AllowedEndHour || (this.MaxTimesPerDay != -1 && this.MaxTimesPerDay <= this.CurrentDayCount))
			{
				return false;
			}
			if (this.tempCooldown > 0f && TwitchManager.Current.CurrentUnityTime - this.tempCooldownSet < this.tempCooldown)
			{
				return false;
			}
			this.tempCooldown = 0f;
			this.tempCooldownSet = 0f;
			if (this.TwitchRequirements == null)
			{
				return true;
			}
			for (int i = 0; i < this.TwitchRequirements.Count; i++)
			{
				if (!this.TwitchRequirements[i].CanPerform(player))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600C244 RID: 49732 RVA: 0x0047AE54 File Offset: 0x00479054
		public virtual void ParseProperties(DynamicProperties properties)
		{
			this.Properties = properties;
			string text = "";
			string text2 = "";
			properties.ParseLocalizedString(TwitchVote.PropTitleVarKey, ref text);
			properties.ParseLocalizedString(TwitchVote.PropDisplayVarKey, ref text2);
			if (text == "" && text2 != "")
			{
				text = text2;
			}
			else if (text != "" && text2 == "")
			{
				text2 = text;
			}
			properties.ParseLocalizedString(TwitchVote.PropTitleKey, ref this.Title);
			if (this.Title == "")
			{
				properties.ParseString(TwitchVote.PropTitle, ref this.Title);
			}
			string text3 = "";
			properties.ParseLocalizedString(TwitchVote.PropTitleFormatKey, ref text3);
			if (text3 != "")
			{
				this.Title = string.Format(text3, text);
			}
			properties.ParseLocalizedString(TwitchVote.PropDescriptionKey, ref this.Description);
			if (this.Description == "")
			{
				properties.ParseString(TwitchVote.PropDescription, ref this.Description);
			}
			text3 = "";
			properties.ParseLocalizedString(TwitchVote.PropDescriptionFormatKey, ref text3);
			if (text3 != "")
			{
				this.Description = string.Format(text3, text);
			}
			if (properties.Values.ContainsKey(TwitchVote.PropDisplayKey))
			{
				this.Display = Localization.Get(properties.Values[TwitchVote.PropDisplayKey], false, null);
			}
			else
			{
				properties.ParseString(TwitchVote.PropDisplay, ref this.Display);
			}
			text3 = "";
			properties.ParseLocalizedString(TwitchVote.PropDisplayFormatKey, ref text3);
			if (text3 != "")
			{
				this.Display = string.Format(text3, text2);
			}
			properties.ParseString(TwitchVote.PropEventName, ref this.GameEvent);
			properties.ParseString(TwitchVote.PropGroup, ref this.Group);
			properties.ParseInt(TwitchVote.PropStartGameStage, ref this.StartGameStage);
			properties.ParseInt(TwitchVote.PropEndGameStage, ref this.EndGameStage);
			properties.ParseInt(TwitchVote.PropAllowedStartHour, ref this.AllowedStartHour);
			properties.ParseInt(TwitchVote.PropAllowedEndHour, ref this.AllowedEndHour);
			properties.ParseString(TwitchVote.PropVoteLine1, ref this.VoteLine1);
			properties.ParseString(TwitchVote.PropVoteLine2, ref this.VoteLine2);
			properties.ParseEnum<TwitchVote.VoteDisplayTypes>(TwitchVote.PropDisplayType, ref this.DisplayType);
			properties.ParseInt(TwitchVote.PropMaxTimesPerDay, ref this.MaxTimesPerDay);
			string text4 = "";
			properties.ParseString(TwitchVote.PropVoteType, ref text4);
			if (text4 != "")
			{
				this.VoteTypes = text4.Split(',', StringSplitOptions.None);
				this.MainVoteType = TwitchManager.Current.VotingManager.GetVoteType(this.VoteTypes[0]);
			}
			properties.ParseBool(TwitchVote.PropEnabled, ref this.Enabled);
			this.OriginalEnabled = this.Enabled;
			properties.ParseString(TwitchVote.PropTitleColor, ref this.TitleColor);
			if (this.Display == "")
			{
				this.Display = this.Title;
			}
			this.Properties.ParseLocalizedString(TwitchVote.PropVoteLine1Key, ref this.VoteLine1);
			this.Properties.ParseLocalizedString(TwitchVote.PropVoteLine2Key, ref this.VoteLine2);
			this.VoteTip = this.Description;
			this.Properties.ParseString(TwitchVote.PropVoteTip, ref this.VoteTip);
			this.Properties.ParseLocalizedString(TwitchVote.PropVoteTipKey, ref this.VoteTip);
			if (properties.Values.ContainsKey(TwitchVote.PropPresets))
			{
				this.PresetNames = new List<string>();
				this.PresetNames.AddRange(properties.Values[TwitchVote.PropPresets].Split(',', StringSplitOptions.None));
			}
			if (!GameEventManager.GameEventSequences.ContainsKey(this.GameEvent))
			{
				Debug.LogError(string.Format("TwitchVote: Game Event Sequence '{0}' does not exist!", this.GameEvent));
			}
		}

		// Token: 0x0600C245 RID: 49733 RVA: 0x0047B206 File Offset: 0x00479406
		public void AddCooldownAddition(TwitchActionCooldownAddition newCooldown)
		{
			if (this.CooldownAdditions == null)
			{
				this.CooldownAdditions = new List<TwitchActionCooldownAddition>();
			}
			this.CooldownAdditions.Add(newCooldown);
		}

		// Token: 0x0600C246 RID: 49734 RVA: 0x0047B227 File Offset: 0x00479427
		public void AddRequirement(BaseTwitchRequirement requirement)
		{
			if (this.TwitchRequirements == null)
			{
				this.TwitchRequirements = new List<BaseTwitchRequirement>();
			}
			this.TwitchRequirements.Add(requirement);
		}

		// Token: 0x0600C247 RID: 49735 RVA: 0x0047B248 File Offset: 0x00479448
		public void HandleVoteComplete()
		{
			if (this.CooldownAdditions != null)
			{
				float actionCooldownModifier = TwitchManager.Current.ActionCooldownModifier;
				for (int i = 0; i < this.CooldownAdditions.Count; i++)
				{
					TwitchActionCooldownAddition twitchActionCooldownAddition = this.CooldownAdditions[i];
					if (twitchActionCooldownAddition.IsAction && TwitchActionManager.TwitchActions.ContainsKey(twitchActionCooldownAddition.ActionName))
					{
						TwitchAction twitchAction = TwitchActionManager.TwitchActions[twitchActionCooldownAddition.ActionName];
						twitchAction.tempCooldown = twitchActionCooldownAddition.CooldownTime * actionCooldownModifier;
						twitchAction.tempCooldownSet = Time.time;
					}
					else if (!twitchActionCooldownAddition.IsAction && TwitchActionManager.TwitchVotes.ContainsKey(twitchActionCooldownAddition.ActionName))
					{
						TwitchVote twitchVote = TwitchActionManager.TwitchVotes[twitchActionCooldownAddition.ActionName];
						twitchVote.tempCooldown = twitchActionCooldownAddition.CooldownTime * actionCooldownModifier;
						twitchVote.tempCooldownSet = Time.time;
					}
				}
			}
		}

		// Token: 0x04009243 RID: 37443
		public static string PropTitleVarKey = "title_var_key";

		// Token: 0x04009244 RID: 37444
		public static string PropDisplayVarKey = "display_var_key";

		// Token: 0x04009245 RID: 37445
		public static string PropTitle = "title";

		// Token: 0x04009246 RID: 37446
		public static string PropTitleKey = "title_key";

		// Token: 0x04009247 RID: 37447
		public static string PropDescription = "description";

		// Token: 0x04009248 RID: 37448
		public static string PropDescriptionKey = "description_key";

		// Token: 0x04009249 RID: 37449
		public static string PropDisplay = "display";

		// Token: 0x0400924A RID: 37450
		public static string PropDisplayKey = "display_key";

		// Token: 0x0400924B RID: 37451
		public static string PropEventName = "event_name";

		// Token: 0x0400924C RID: 37452
		public static string PropVoteType = "vote_type";

		// Token: 0x0400924D RID: 37453
		public static string PropGroup = "group";

		// Token: 0x0400924E RID: 37454
		public static string PropTitleFormatKey = "title_format_key";

		// Token: 0x0400924F RID: 37455
		public static string PropDescriptionFormatKey = "description_format_key";

		// Token: 0x04009250 RID: 37456
		public static string PropDisplayFormatKey = "display_format_key";

		// Token: 0x04009251 RID: 37457
		public static string PropStartGameStage = "start_gamestage";

		// Token: 0x04009252 RID: 37458
		public static string PropEndGameStage = "end_gamestage";

		// Token: 0x04009253 RID: 37459
		public static string PropAllowedStartHour = "allowed_start_hour";

		// Token: 0x04009254 RID: 37460
		public static string PropAllowedEndHour = "allowed_end_hour";

		// Token: 0x04009255 RID: 37461
		public static string PropDisplayType = "display_type";

		// Token: 0x04009256 RID: 37462
		public static string PropTitleColor = "title_color";

		// Token: 0x04009257 RID: 37463
		public static string PropVoteLine1 = "line1_desc";

		// Token: 0x04009258 RID: 37464
		public static string PropVoteLine1Key = "line1_desc_key";

		// Token: 0x04009259 RID: 37465
		public static string PropVoteLine2 = "line2_desc";

		// Token: 0x0400925A RID: 37466
		public static string PropVoteLine2Key = "line2_desc_key";

		// Token: 0x0400925B RID: 37467
		public static string PropEnabled = "enabled";

		// Token: 0x0400925C RID: 37468
		public static string PropMaxTimesPerDay = "max_times_per_day";

		// Token: 0x0400925D RID: 37469
		public static string PropVoteTip = "vote_tip";

		// Token: 0x0400925E RID: 37470
		public static string PropVoteTipKey = "vote_tip_key";

		// Token: 0x0400925F RID: 37471
		public static string PropPresets = "presets";

		// Token: 0x04009260 RID: 37472
		public static HashSet<string> ExtendsExcludes = new HashSet<string>
		{
			TwitchVote.PropStartGameStage,
			TwitchVote.PropEndGameStage
		};

		// Token: 0x04009261 RID: 37473
		public string VoteName;

		// Token: 0x04009262 RID: 37474
		public string Title;

		// Token: 0x04009263 RID: 37475
		public string Description;

		// Token: 0x04009264 RID: 37476
		public string Display = "";

		// Token: 0x04009265 RID: 37477
		public string GameEvent;

		// Token: 0x04009266 RID: 37478
		public string[] VoteTypes;

		// Token: 0x04009267 RID: 37479
		public TwitchVoteType MainVoteType;

		// Token: 0x04009268 RID: 37480
		public string Group = "";

		// Token: 0x04009269 RID: 37481
		public bool Enabled = true;

		// Token: 0x0400926A RID: 37482
		public bool OriginalEnabled;

		// Token: 0x0400926B RID: 37483
		public string TitleColor = "";

		// Token: 0x0400926C RID: 37484
		public int StartGameStage = -1;

		// Token: 0x0400926D RID: 37485
		public int EndGameStage = -1;

		// Token: 0x0400926E RID: 37486
		public int AllowedStartHour;

		// Token: 0x0400926F RID: 37487
		public int AllowedEndHour = 24;

		// Token: 0x04009270 RID: 37488
		public string VoteLine1 = "";

		// Token: 0x04009271 RID: 37489
		public string VoteLine2 = "";

		// Token: 0x04009272 RID: 37490
		public int MaxTimesPerDay = -1;

		// Token: 0x04009273 RID: 37491
		public int CurrentDayCount;

		// Token: 0x04009274 RID: 37492
		public float tempCooldownSet;

		// Token: 0x04009275 RID: 37493
		public float tempCooldown;

		// Token: 0x04009276 RID: 37494
		public string VoteTip = "";

		// Token: 0x04009277 RID: 37495
		public DynamicProperties Properties;

		// Token: 0x04009278 RID: 37496
		public List<TwitchActionCooldownAddition> CooldownAdditions;

		// Token: 0x04009279 RID: 37497
		public List<BaseTwitchRequirement> TwitchRequirements;

		// Token: 0x0400927A RID: 37498
		public TwitchVote.VoteDisplayTypes DisplayType = TwitchVote.VoteDisplayTypes.GoodBad;

		// Token: 0x0400927B RID: 37499
		public List<string> PresetNames;

		// Token: 0x02001891 RID: 6289
		public enum VoteDisplayTypes
		{
			// Token: 0x0400927D RID: 37501
			Single,
			// Token: 0x0400927E RID: 37502
			GoodBad,
			// Token: 0x0400927F RID: 37503
			Special,
			// Token: 0x04009280 RID: 37504
			HordeBuffed
		}
	}
}
