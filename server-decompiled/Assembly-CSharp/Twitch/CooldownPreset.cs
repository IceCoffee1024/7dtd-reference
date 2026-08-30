using System;
using System.Collections.Generic;
using System.Globalization;

namespace Twitch
{
	// Token: 0x02001828 RID: 6184
	public class CooldownPreset
	{
		// Token: 0x0600BF0F RID: 48911 RVA: 0x00468AB0 File Offset: 0x00466CB0
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

		// Token: 0x0600BF10 RID: 48912 RVA: 0x00468B00 File Offset: 0x00466D00
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

		// Token: 0x0600BF11 RID: 48913 RVA: 0x00468C14 File Offset: 0x00466E14
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

		// Token: 0x04008F5B RID: 36699
		public static string PropName = "name";

		// Token: 0x04008F5C RID: 36700
		public static string PropTitle = "title";

		// Token: 0x04008F5D RID: 36701
		public static string PropTitleKey = "title_key";

		// Token: 0x04008F5E RID: 36702
		public static string PropCooldownType = "cooldown_type";

		// Token: 0x04008F5F RID: 36703
		public static string PropIsDefault = "is_default";

		// Token: 0x04008F60 RID: 36704
		public static string PropStartCooldown = "start_cooldown";

		// Token: 0x04008F61 RID: 36705
		public static string PropDeathCooldown = "death_cooldown";

		// Token: 0x04008F62 RID: 36706
		public static string PropBMStartOffset = "bm_start_offset";

		// Token: 0x04008F63 RID: 36707
		public static string PropBMEndOffset = "bm_end_offset";

		// Token: 0x04008F64 RID: 36708
		public string Name;

		// Token: 0x04008F65 RID: 36709
		public bool IsDefault;

		// Token: 0x04008F66 RID: 36710
		public string Title;

		// Token: 0x04008F67 RID: 36711
		public CooldownPreset.CooldownTypes CooldownType = CooldownPreset.CooldownTypes.Fill;

		// Token: 0x04008F68 RID: 36712
		public float CooldownFillMax;

		// Token: 0x04008F69 RID: 36713
		public int NextCooldownTime;

		// Token: 0x04008F6A RID: 36714
		public int StartCooldownTime;

		// Token: 0x04008F6B RID: 36715
		public int AfterDeathCooldownTime;

		// Token: 0x04008F6C RID: 36716
		public int BMStartOffset;

		// Token: 0x04008F6D RID: 36717
		public int BMEndOffset;

		// Token: 0x04008F6E RID: 36718
		public List<TwitchCooldownEntry> CooldownMaxEntries = new List<TwitchCooldownEntry>();

		// Token: 0x02001829 RID: 6185
		public enum CooldownTypes
		{
			// Token: 0x04008F70 RID: 36720
			Always,
			// Token: 0x04008F71 RID: 36721
			Fill,
			// Token: 0x04008F72 RID: 36722
			None
		}
	}
}
