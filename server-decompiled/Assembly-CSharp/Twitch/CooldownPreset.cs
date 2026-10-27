using System;
using System.Collections.Generic;
using System.Globalization;

namespace Twitch
{
	// Token: 0x02001885 RID: 6277
	public class CooldownPreset
	{
		// Token: 0x0600C17B RID: 49531 RVA: 0x00470998 File Offset: 0x0046EB98
		public void AddCooldownMaxEntry(int start, int end, int cooldownMax, int cooldownTime)
		{
			if (this.CooldownMaxEntries == null)
			{
				this.CooldownMaxEntries = new List<TwitchCooldownEntry>();
			}
			this.CooldownMaxEntries.Add(new TwitchCooldownEntry
			{
				StartGameStage = start,
				EndGameStage = end,
				CooldownMax = cooldownMax,
				CooldownTime = cooldownTime
			});
		}

		// Token: 0x0600C17C RID: 49532 RVA: 0x004709E8 File Offset: 0x0046EBE8
		public void SetupCooldownInfo(int gameStage, EntityPlayerLocal localPlayer)
		{
			if (localPlayer == null)
			{
				return;
			}
			for (int i = 0; i < this.CooldownMaxEntries.Count; i++)
			{
				if (gameStage >= this.CooldownMaxEntries[i].StartGameStage && (gameStage <= this.CooldownMaxEntries[i].EndGameStage || this.CooldownMaxEntries[i].EndGameStage == -1))
				{
					float num = 1f;
					if (localPlayer.Party != null)
					{
						int num2 = 0;
						for (int j = 0; j < localPlayer.Party.MemberList.Count; j++)
						{
							if (localPlayer.Party.MemberList[j].TwitchActionsEnabled != EntityPlayer.TwitchActionsStates.Disabled)
							{
								num2++;
							}
						}
						num += (float)(num2 - 1) * 0.5f;
					}
					this.CooldownFillMax = (float)this.CooldownMaxEntries[i].CooldownMax * num;
					this.NextCooldownTime = this.CooldownMaxEntries[i].CooldownTime;
					return;
				}
			}
			this.CooldownFillMax = 100f;
			this.NextCooldownTime = 180;
		}

		// Token: 0x0600C17D RID: 49533 RVA: 0x00470AFC File Offset: 0x0046ECFC
		public virtual void ParseProperties(DynamicProperties properties)
		{
			if (properties.Values.ContainsKey(CooldownPreset.PropName))
			{
				this.Name = properties.Values[CooldownPreset.PropName];
			}
			if (properties.Values.ContainsKey(CooldownPreset.PropTitle))
			{
				this.Title = properties.Values[CooldownPreset.PropTitle];
			}
			if (properties.Values.ContainsKey(CooldownPreset.PropTitleKey))
			{
				this.Title = Localization.Get(properties.Values[CooldownPreset.PropTitleKey], false, null);
			}
			if (properties.Values.ContainsKey(CooldownPreset.PropCooldownType))
			{
				this.CooldownType = (CooldownPreset.CooldownTypes)Enum.Parse(typeof(CooldownPreset.CooldownTypes), properties.Values[CooldownPreset.PropCooldownType], true);
			}
			if (properties.Values.ContainsKey(CooldownPreset.PropIsDefault))
			{
				this.IsDefault = StringParsers.ParseBool(properties.Values[CooldownPreset.PropIsDefault], 0, -1, true);
			}
			if (properties.Values.ContainsKey(CooldownPreset.PropStartCooldown))
			{
				this.StartCooldownTime = StringParsers.ParseSInt32(properties.Values[CooldownPreset.PropStartCooldown], 0, -1, NumberStyles.Integer);
			}
			else
			{
				this.StartCooldownTime = 300;
			}
			if (properties.Values.ContainsKey(CooldownPreset.PropDeathCooldown))
			{
				this.AfterDeathCooldownTime = StringParsers.ParseSInt32(properties.Values[CooldownPreset.PropDeathCooldown], 0, -1, NumberStyles.Integer);
			}
			else
			{
				this.AfterDeathCooldownTime = 180;
			}
			if (properties.Values.ContainsKey(CooldownPreset.PropBMStartOffset))
			{
				this.BMStartOffset = StringParsers.ParseSInt32(properties.Values[CooldownPreset.PropBMStartOffset], 0, -1, NumberStyles.Integer);
			}
			if (properties.Values.ContainsKey(CooldownPreset.PropBMEndOffset))
			{
				this.BMEndOffset = StringParsers.ParseSInt32(properties.Values[CooldownPreset.PropBMEndOffset], 0, -1, NumberStyles.Integer);
			}
		}

		// Token: 0x0400910A RID: 37130
		public static string PropName = "name";

		// Token: 0x0400910B RID: 37131
		public static string PropTitle = "title";

		// Token: 0x0400910C RID: 37132
		public static string PropTitleKey = "title_key";

		// Token: 0x0400910D RID: 37133
		public static string PropCooldownType = "cooldown_type";

		// Token: 0x0400910E RID: 37134
		public static string PropIsDefault = "is_default";

		// Token: 0x0400910F RID: 37135
		public static string PropStartCooldown = "start_cooldown";

		// Token: 0x04009110 RID: 37136
		public static string PropDeathCooldown = "death_cooldown";

		// Token: 0x04009111 RID: 37137
		public static string PropBMStartOffset = "bm_start_offset";

		// Token: 0x04009112 RID: 37138
		public static string PropBMEndOffset = "bm_end_offset";

		// Token: 0x04009113 RID: 37139
		public string Name;

		// Token: 0x04009114 RID: 37140
		public bool IsDefault;

		// Token: 0x04009115 RID: 37141
		public string Title;

		// Token: 0x04009116 RID: 37142
		public CooldownPreset.CooldownTypes CooldownType = CooldownPreset.CooldownTypes.Fill;

		// Token: 0x04009117 RID: 37143
		public float CooldownFillMax;

		// Token: 0x04009118 RID: 37144
		public int NextCooldownTime;

		// Token: 0x04009119 RID: 37145
		public int StartCooldownTime;

		// Token: 0x0400911A RID: 37146
		public int AfterDeathCooldownTime;

		// Token: 0x0400911B RID: 37147
		public int BMStartOffset;

		// Token: 0x0400911C RID: 37148
		public int BMEndOffset;

		// Token: 0x0400911D RID: 37149
		public List<TwitchCooldownEntry> CooldownMaxEntries = new List<TwitchCooldownEntry>();

		// Token: 0x02001886 RID: 6278
		public enum CooldownTypes
		{
			// Token: 0x0400911F RID: 37151
			Always,
			// Token: 0x04009120 RID: 37152
			Fill,
			// Token: 0x04009121 RID: 37153
			None
		}
	}
}
