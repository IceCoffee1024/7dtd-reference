using System;
using System.Collections.Generic;
using UnityEngine;

namespace Twitch
{
	// Token: 0x020018C1 RID: 6337
	public class TwitchVote
	{
		// Token: 0x17001889 RID: 6281
		// (get) Token: 0x0600C354 RID: 50004 RVA: 0x0047E65C File Offset: 0x0047C85C
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

		// Token: 0x1700188A RID: 6282
		// (get) Token: 0x0600C355 RID: 50005 RVA: 0x0047E6C8 File Offset: 0x0047C8C8
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

		// Token: 0x0600C356 RID: 50006 RVA: 0x0047E6EE File Offset: 0x0047C8EE
		public bool IsInPreset(TwitchVotePreset preset)
		{
			return (this.PresetNames == null && !preset.IsEmpty) || (this.PresetNames != null && this.PresetNames.Contains(preset.Name));
		}

		// Token: 0x0600C357 RID: 50007 RVA: 0x0047E720 File Offset: 0x0047C920
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

		// Token: 0x0600C358 RID: 50008 RVA: 0x0047E800 File Offset: 0x0047CA00
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

		// Token: 0x0600C359 RID: 50009 RVA: 0x0047EBB2 File Offset: 0x0047CDB2
		public void AddCooldownAddition(TwitchActionCooldownAddition newCooldown)
		{
			if (this.CooldownAdditions == null)
			{
				this.CooldownAdditions = new List<TwitchActionCooldownAddition>();
			}
			this.CooldownAdditions.Add(newCooldown);
		}

		// Token: 0x0600C35A RID: 50010 RVA: 0x0047EBD3 File Offset: 0x0047CDD3
		public void AddRequirement(BaseTwitchRequirement requirement)
		{
			if (this.TwitchRequirements == null)
			{
				this.TwitchRequirements = new List<BaseTwitchRequirement>();
			}
			this.TwitchRequirements.Add(requirement);
		}

		// Token: 0x0600C35B RID: 50011 RVA: 0x0047EBF4 File Offset: 0x0047CDF4
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

		// Token: 0x0400932C RID: 37676
		public static string PropTitleVarKey = "title_var_key";

		// Token: 0x0400932D RID: 37677
		public static string PropDisplayVarKey = "display_var_key";

		// Token: 0x0400932E RID: 37678
		public static string PropTitle = "title";

		// Token: 0x0400932F RID: 37679
		public static string PropTitleKey = "title_key";

		// Token: 0x04009330 RID: 37680
		public static string PropDescription = "description";

		// Token: 0x04009331 RID: 37681
		public static string PropDescriptionKey = "description_key";

		// Token: 0x04009332 RID: 37682
		public static string PropDisplay = "display";

		// Token: 0x04009333 RID: 37683
		public static string PropDisplayKey = "display_key";

		// Token: 0x04009334 RID: 37684
		public static string PropEventName = "event_name";

		// Token: 0x04009335 RID: 37685
		public static string PropVoteType = "vote_type";

		// Token: 0x04009336 RID: 37686
		public static string PropGroup = "group";

		// Token: 0x04009337 RID: 37687
		public static string PropTitleFormatKey = "title_format_key";

		// Token: 0x04009338 RID: 37688
		public static string PropDescriptionFormatKey = "description_format_key";

		// Token: 0x04009339 RID: 37689
		public static string PropDisplayFormatKey = "display_format_key";

		// Token: 0x0400933A RID: 37690
		public static string PropStartGameStage = "start_gamestage";

		// Token: 0x0400933B RID: 37691
		public static string PropEndGameStage = "end_gamestage";

		// Token: 0x0400933C RID: 37692
		public static string PropAllowedStartHour = "allowed_start_hour";

		// Token: 0x0400933D RID: 37693
		public static string PropAllowedEndHour = "allowed_end_hour";

		// Token: 0x0400933E RID: 37694
		public static string PropDisplayType = "display_type";

		// Token: 0x0400933F RID: 37695
		public static string PropTitleColor = "title_color";

		// Token: 0x04009340 RID: 37696
		public static string PropVoteLine1 = "line1_desc";

		// Token: 0x04009341 RID: 37697
		public static string PropVoteLine1Key = "line1_desc_key";

		// Token: 0x04009342 RID: 37698
		public static string PropVoteLine2 = "line2_desc";

		// Token: 0x04009343 RID: 37699
		public static string PropVoteLine2Key = "line2_desc_key";

		// Token: 0x04009344 RID: 37700
		public static string PropEnabled = "enabled";

		// Token: 0x04009345 RID: 37701
		public static string PropMaxTimesPerDay = "max_times_per_day";

		// Token: 0x04009346 RID: 37702
		public static string PropVoteTip = "vote_tip";

		// Token: 0x04009347 RID: 37703
		public static string PropVoteTipKey = "vote_tip_key";

		// Token: 0x04009348 RID: 37704
		public static string PropPresets = "presets";

		// Token: 0x04009349 RID: 37705
		public static HashSet<string> ExtendsExcludes = new HashSet<string>
		{
			TwitchVote.PropStartGameStage,
			TwitchVote.PropEndGameStage
		};

		// Token: 0x0400934A RID: 37706
		public string VoteName;

		// Token: 0x0400934B RID: 37707
		public string Title;

		// Token: 0x0400934C RID: 37708
		public string Description;

		// Token: 0x0400934D RID: 37709
		public string Display = "";

		// Token: 0x0400934E RID: 37710
		public string GameEvent;

		// Token: 0x0400934F RID: 37711
		public string[] VoteTypes;

		// Token: 0x04009350 RID: 37712
		public TwitchVoteType MainVoteType;

		// Token: 0x04009351 RID: 37713
		public string Group = "";

		// Token: 0x04009352 RID: 37714
		public bool Enabled = true;

		// Token: 0x04009353 RID: 37715
		public bool OriginalEnabled;

		// Token: 0x04009354 RID: 37716
		public string TitleColor = "";

		// Token: 0x04009355 RID: 37717
		public int StartGameStage = -1;

		// Token: 0x04009356 RID: 37718
		public int EndGameStage = -1;

		// Token: 0x04009357 RID: 37719
		public int AllowedStartHour;

		// Token: 0x04009358 RID: 37720
		public int AllowedEndHour = 24;

		// Token: 0x04009359 RID: 37721
		public string VoteLine1 = "";

		// Token: 0x0400935A RID: 37722
		public string VoteLine2 = "";

		// Token: 0x0400935B RID: 37723
		public int MaxTimesPerDay = -1;

		// Token: 0x0400935C RID: 37724
		public int CurrentDayCount;

		// Token: 0x0400935D RID: 37725
		public float tempCooldownSet;

		// Token: 0x0400935E RID: 37726
		public float tempCooldown;

		// Token: 0x0400935F RID: 37727
		public string VoteTip = "";

		// Token: 0x04009360 RID: 37728
		public DynamicProperties Properties;

		// Token: 0x04009361 RID: 37729
		public List<TwitchActionCooldownAddition> CooldownAdditions;

		// Token: 0x04009362 RID: 37730
		public List<BaseTwitchRequirement> TwitchRequirements;

		// Token: 0x04009363 RID: 37731
		public TwitchVote.VoteDisplayTypes DisplayType = TwitchVote.VoteDisplayTypes.GoodBad;

		// Token: 0x04009364 RID: 37732
		public List<string> PresetNames;

		// Token: 0x020018C2 RID: 6338
		public enum VoteDisplayTypes
		{
			// Token: 0x04009366 RID: 37734
			Single,
			// Token: 0x04009367 RID: 37735
			GoodBad,
			// Token: 0x04009368 RID: 37736
			Special,
			// Token: 0x04009369 RID: 37737
			HordeBuffed
		}
	}
}
