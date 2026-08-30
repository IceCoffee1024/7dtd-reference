using System;
using System.Collections.Generic;
using UnityEngine;

namespace Twitch
{
	// Token: 0x02001864 RID: 6244
	public class TwitchVote
	{
		// Token: 0x1700179F RID: 6047
		// (get) Token: 0x0600C0E8 RID: 49384 RVA: 0x00476774 File Offset: 0x00474974
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

		// Token: 0x170017A0 RID: 6048
		// (get) Token: 0x0600C0E9 RID: 49385 RVA: 0x004767E0 File Offset: 0x004749E0
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

		// Token: 0x0600C0EA RID: 49386 RVA: 0x00476806 File Offset: 0x00474A06
		public bool IsInPreset(TwitchVotePreset preset)
		{
			return (this.PresetNames == null && !preset.IsEmpty) || (this.PresetNames != null && this.PresetNames.Contains(preset.Name));
		}

		// Token: 0x0600C0EB RID: 49387 RVA: 0x00476838 File Offset: 0x00474A38
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

		// Token: 0x0600C0EC RID: 49388 RVA: 0x00476918 File Offset: 0x00474B18
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

		// Token: 0x0600C0ED RID: 49389 RVA: 0x00476CCA File Offset: 0x00474ECA
		public void AddCooldownAddition(TwitchActionCooldownAddition newCooldown)
		{
			if (this.CooldownAdditions == null)
			{
				this.CooldownAdditions = new List<TwitchActionCooldownAddition>();
			}
			this.CooldownAdditions.Add(newCooldown);
		}

		// Token: 0x0600C0EE RID: 49390 RVA: 0x00476CEB File Offset: 0x00474EEB
		public void AddRequirement(BaseTwitchRequirement requirement)
		{
			if (this.TwitchRequirements == null)
			{
				this.TwitchRequirements = new List<BaseTwitchRequirement>();
			}
			this.TwitchRequirements.Add(requirement);
		}

		// Token: 0x0600C0EF RID: 49391 RVA: 0x00476D0C File Offset: 0x00474F0C
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

		// Token: 0x0400917D RID: 37245
		public static string PropTitleVarKey = "title_var_key";

		// Token: 0x0400917E RID: 37246
		public static string PropDisplayVarKey = "display_var_key";

		// Token: 0x0400917F RID: 37247
		public static string PropTitle = "title";

		// Token: 0x04009180 RID: 37248
		public static string PropTitleKey = "title_key";

		// Token: 0x04009181 RID: 37249
		public static string PropDescription = "description";

		// Token: 0x04009182 RID: 37250
		public static string PropDescriptionKey = "description_key";

		// Token: 0x04009183 RID: 37251
		public static string PropDisplay = "display";

		// Token: 0x04009184 RID: 37252
		public static string PropDisplayKey = "display_key";

		// Token: 0x04009185 RID: 37253
		public static string PropEventName = "event_name";

		// Token: 0x04009186 RID: 37254
		public static string PropVoteType = "vote_type";

		// Token: 0x04009187 RID: 37255
		public static string PropGroup = "group";

		// Token: 0x04009188 RID: 37256
		public static string PropTitleFormatKey = "title_format_key";

		// Token: 0x04009189 RID: 37257
		public static string PropDescriptionFormatKey = "description_format_key";

		// Token: 0x0400918A RID: 37258
		public static string PropDisplayFormatKey = "display_format_key";

		// Token: 0x0400918B RID: 37259
		public static string PropStartGameStage = "start_gamestage";

		// Token: 0x0400918C RID: 37260
		public static string PropEndGameStage = "end_gamestage";

		// Token: 0x0400918D RID: 37261
		public static string PropAllowedStartHour = "allowed_start_hour";

		// Token: 0x0400918E RID: 37262
		public static string PropAllowedEndHour = "allowed_end_hour";

		// Token: 0x0400918F RID: 37263
		public static string PropDisplayType = "display_type";

		// Token: 0x04009190 RID: 37264
		public static string PropTitleColor = "title_color";

		// Token: 0x04009191 RID: 37265
		public static string PropVoteLine1 = "line1_desc";

		// Token: 0x04009192 RID: 37266
		public static string PropVoteLine1Key = "line1_desc_key";

		// Token: 0x04009193 RID: 37267
		public static string PropVoteLine2 = "line2_desc";

		// Token: 0x04009194 RID: 37268
		public static string PropVoteLine2Key = "line2_desc_key";

		// Token: 0x04009195 RID: 37269
		public static string PropEnabled = "enabled";

		// Token: 0x04009196 RID: 37270
		public static string PropMaxTimesPerDay = "max_times_per_day";

		// Token: 0x04009197 RID: 37271
		public static string PropVoteTip = "vote_tip";

		// Token: 0x04009198 RID: 37272
		public static string PropVoteTipKey = "vote_tip_key";

		// Token: 0x04009199 RID: 37273
		public static string PropPresets = "presets";

		// Token: 0x0400919A RID: 37274
		public static HashSet<string> ExtendsExcludes = new HashSet<string>
		{
			TwitchVote.PropStartGameStage,
			TwitchVote.PropEndGameStage
		};

		// Token: 0x0400919B RID: 37275
		public string VoteName;

		// Token: 0x0400919C RID: 37276
		public string Title;

		// Token: 0x0400919D RID: 37277
		public string Description;

		// Token: 0x0400919E RID: 37278
		public string Display = "";

		// Token: 0x0400919F RID: 37279
		public string GameEvent;

		// Token: 0x040091A0 RID: 37280
		public string[] VoteTypes;

		// Token: 0x040091A1 RID: 37281
		public TwitchVoteType MainVoteType;

		// Token: 0x040091A2 RID: 37282
		public string Group = "";

		// Token: 0x040091A3 RID: 37283
		public bool Enabled = true;

		// Token: 0x040091A4 RID: 37284
		public bool OriginalEnabled;

		// Token: 0x040091A5 RID: 37285
		public string TitleColor = "";

		// Token: 0x040091A6 RID: 37286
		public int StartGameStage = -1;

		// Token: 0x040091A7 RID: 37287
		public int EndGameStage = -1;

		// Token: 0x040091A8 RID: 37288
		public int AllowedStartHour;

		// Token: 0x040091A9 RID: 37289
		public int AllowedEndHour = 24;

		// Token: 0x040091AA RID: 37290
		public string VoteLine1 = "";

		// Token: 0x040091AB RID: 37291
		public string VoteLine2 = "";

		// Token: 0x040091AC RID: 37292
		public int MaxTimesPerDay = -1;

		// Token: 0x040091AD RID: 37293
		public int CurrentDayCount;

		// Token: 0x040091AE RID: 37294
		public float tempCooldownSet;

		// Token: 0x040091AF RID: 37295
		public float tempCooldown;

		// Token: 0x040091B0 RID: 37296
		public string VoteTip = "";

		// Token: 0x040091B1 RID: 37297
		public DynamicProperties Properties;

		// Token: 0x040091B2 RID: 37298
		public List<TwitchActionCooldownAddition> CooldownAdditions;

		// Token: 0x040091B3 RID: 37299
		public List<BaseTwitchRequirement> TwitchRequirements;

		// Token: 0x040091B4 RID: 37300
		public TwitchVote.VoteDisplayTypes DisplayType = TwitchVote.VoteDisplayTypes.GoodBad;

		// Token: 0x040091B5 RID: 37301
		public List<string> PresetNames;

		// Token: 0x02001865 RID: 6245
		public enum VoteDisplayTypes
		{
			// Token: 0x040091B7 RID: 37303
			Single,
			// Token: 0x040091B8 RID: 37304
			GoodBad,
			// Token: 0x040091B9 RID: 37305
			Special,
			// Token: 0x040091BA RID: 37306
			HordeBuffed
		}
	}
}
