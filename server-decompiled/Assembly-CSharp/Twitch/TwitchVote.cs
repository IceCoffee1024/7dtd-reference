using System;
using System.Collections.Generic;
using UnityEngine;

namespace Twitch
{
	// Token: 0x0200183E RID: 6206
	public class TwitchVote
	{
		// Token: 0x1700172D RID: 5933
		// (get) Token: 0x0600BF63 RID: 48995 RVA: 0x004709D8 File Offset: 0x0046EBD8
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

		// Token: 0x1700172E RID: 5934
		// (get) Token: 0x0600BF64 RID: 48996 RVA: 0x00470A44 File Offset: 0x0046EC44
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

		// Token: 0x0600BF65 RID: 48997 RVA: 0x00470A6A File Offset: 0x0046EC6A
		public bool IsInPreset(TwitchVotePreset preset)
		{
			return (this.PresetNames == null && !preset.IsEmpty) || (this.PresetNames != null && this.PresetNames.Contains(preset.Name));
		}

		// Token: 0x0600BF66 RID: 48998 RVA: 0x00470A9C File Offset: 0x0046EC9C
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

		// Token: 0x0600BF67 RID: 48999 RVA: 0x00470B7C File Offset: 0x0046ED7C
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

		// Token: 0x0600BF68 RID: 49000 RVA: 0x00470F2E File Offset: 0x0046F12E
		public void AddCooldownAddition(TwitchActionCooldownAddition newCooldown)
		{
			if (this.CooldownAdditions == null)
			{
				this.CooldownAdditions = new List<TwitchActionCooldownAddition>();
			}
			this.CooldownAdditions.Add(newCooldown);
		}

		// Token: 0x0600BF69 RID: 49001 RVA: 0x00470F4F File Offset: 0x0046F14F
		public void AddRequirement(BaseTwitchRequirement requirement)
		{
			if (this.TwitchRequirements == null)
			{
				this.TwitchRequirements = new List<BaseTwitchRequirement>();
			}
			this.TwitchRequirements.Add(requirement);
		}

		// Token: 0x0600BF6A RID: 49002 RVA: 0x00470F70 File Offset: 0x0046F170
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

		// Token: 0x04009061 RID: 36961
		public static string PropTitleVarKey = "title_var_key";

		// Token: 0x04009062 RID: 36962
		public static string PropDisplayVarKey = "display_var_key";

		// Token: 0x04009063 RID: 36963
		public static string PropTitle = "title";

		// Token: 0x04009064 RID: 36964
		public static string PropTitleKey = "title_key";

		// Token: 0x04009065 RID: 36965
		public static string PropDescription = "description";

		// Token: 0x04009066 RID: 36966
		public static string PropDescriptionKey = "description_key";

		// Token: 0x04009067 RID: 36967
		public static string PropDisplay = "display";

		// Token: 0x04009068 RID: 36968
		public static string PropDisplayKey = "display_key";

		// Token: 0x04009069 RID: 36969
		public static string PropEventName = "event_name";

		// Token: 0x0400906A RID: 36970
		public static string PropVoteType = "vote_type";

		// Token: 0x0400906B RID: 36971
		public static string PropGroup = "group";

		// Token: 0x0400906C RID: 36972
		public static string PropTitleFormatKey = "title_format_key";

		// Token: 0x0400906D RID: 36973
		public static string PropDescriptionFormatKey = "description_format_key";

		// Token: 0x0400906E RID: 36974
		public static string PropDisplayFormatKey = "display_format_key";

		// Token: 0x0400906F RID: 36975
		public static string PropStartGameStage = "start_gamestage";

		// Token: 0x04009070 RID: 36976
		public static string PropEndGameStage = "end_gamestage";

		// Token: 0x04009071 RID: 36977
		public static string PropAllowedStartHour = "allowed_start_hour";

		// Token: 0x04009072 RID: 36978
		public static string PropAllowedEndHour = "allowed_end_hour";

		// Token: 0x04009073 RID: 36979
		public static string PropDisplayType = "display_type";

		// Token: 0x04009074 RID: 36980
		public static string PropTitleColor = "title_color";

		// Token: 0x04009075 RID: 36981
		public static string PropVoteLine1 = "line1_desc";

		// Token: 0x04009076 RID: 36982
		public static string PropVoteLine1Key = "line1_desc_key";

		// Token: 0x04009077 RID: 36983
		public static string PropVoteLine2 = "line2_desc";

		// Token: 0x04009078 RID: 36984
		public static string PropVoteLine2Key = "line2_desc_key";

		// Token: 0x04009079 RID: 36985
		public static string PropEnabled = "enabled";

		// Token: 0x0400907A RID: 36986
		public static string PropMaxTimesPerDay = "max_times_per_day";

		// Token: 0x0400907B RID: 36987
		public static string PropVoteTip = "vote_tip";

		// Token: 0x0400907C RID: 36988
		public static string PropVoteTipKey = "vote_tip_key";

		// Token: 0x0400907D RID: 36989
		public static string PropPresets = "presets";

		// Token: 0x0400907E RID: 36990
		public static HashSet<string> ExtendsExcludes = new HashSet<string>
		{
			TwitchVote.PropStartGameStage,
			TwitchVote.PropEndGameStage
		};

		// Token: 0x0400907F RID: 36991
		public string VoteName;

		// Token: 0x04009080 RID: 36992
		public string Title;

		// Token: 0x04009081 RID: 36993
		public string Description;

		// Token: 0x04009082 RID: 36994
		public string Display = "";

		// Token: 0x04009083 RID: 36995
		public string GameEvent;

		// Token: 0x04009084 RID: 36996
		public string[] VoteTypes;

		// Token: 0x04009085 RID: 36997
		public TwitchVoteType MainVoteType;

		// Token: 0x04009086 RID: 36998
		public string Group = "";

		// Token: 0x04009087 RID: 36999
		public bool Enabled = true;

		// Token: 0x04009088 RID: 37000
		public bool OriginalEnabled;

		// Token: 0x04009089 RID: 37001
		public string TitleColor = "";

		// Token: 0x0400908A RID: 37002
		public int StartGameStage = -1;

		// Token: 0x0400908B RID: 37003
		public int EndGameStage = -1;

		// Token: 0x0400908C RID: 37004
		public int AllowedStartHour;

		// Token: 0x0400908D RID: 37005
		public int AllowedEndHour = 24;

		// Token: 0x0400908E RID: 37006
		public string VoteLine1 = "";

		// Token: 0x0400908F RID: 37007
		public string VoteLine2 = "";

		// Token: 0x04009090 RID: 37008
		public int MaxTimesPerDay = -1;

		// Token: 0x04009091 RID: 37009
		public int CurrentDayCount;

		// Token: 0x04009092 RID: 37010
		public float tempCooldownSet;

		// Token: 0x04009093 RID: 37011
		public float tempCooldown;

		// Token: 0x04009094 RID: 37012
		public string VoteTip = "";

		// Token: 0x04009095 RID: 37013
		public DynamicProperties Properties;

		// Token: 0x04009096 RID: 37014
		public List<TwitchActionCooldownAddition> CooldownAdditions;

		// Token: 0x04009097 RID: 37015
		public List<BaseTwitchRequirement> TwitchRequirements;

		// Token: 0x04009098 RID: 37016
		public TwitchVote.VoteDisplayTypes DisplayType = TwitchVote.VoteDisplayTypes.GoodBad;

		// Token: 0x04009099 RID: 37017
		public List<string> PresetNames;

		// Token: 0x0200183F RID: 6207
		public enum VoteDisplayTypes
		{
			// Token: 0x0400909B RID: 37019
			Single,
			// Token: 0x0400909C RID: 37020
			GoodBad,
			// Token: 0x0400909D RID: 37021
			Special,
			// Token: 0x0400909E RID: 37022
			HordeBuffed
		}
	}
}
