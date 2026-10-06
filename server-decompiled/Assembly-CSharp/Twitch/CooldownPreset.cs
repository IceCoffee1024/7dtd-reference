using System;
using System.Collections.Generic;
using System.Globalization;

namespace Twitch
{
	// Token: 0x02001854 RID: 6228
	public class CooldownPreset
	{
		// Token: 0x0600C067 RID: 49255 RVA: 0x0046CFEC File Offset: 0x0046B1EC
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

		// Token: 0x0600C068 RID: 49256 RVA: 0x0046D03C File Offset: 0x0046B23C
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

		// Token: 0x0600C069 RID: 49257 RVA: 0x0046D150 File Offset: 0x0046B350
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

		// Token: 0x04009021 RID: 36897
		public static string PropName = "name";

		// Token: 0x04009022 RID: 36898
		public static string PropTitle = "title";

		// Token: 0x04009023 RID: 36899
		public static string PropTitleKey = "title_key";

		// Token: 0x04009024 RID: 36900
		public static string PropCooldownType = "cooldown_type";

		// Token: 0x04009025 RID: 36901
		public static string PropIsDefault = "is_default";

		// Token: 0x04009026 RID: 36902
		public static string PropStartCooldown = "start_cooldown";

		// Token: 0x04009027 RID: 36903
		public static string PropDeathCooldown = "death_cooldown";

		// Token: 0x04009028 RID: 36904
		public static string PropBMStartOffset = "bm_start_offset";

		// Token: 0x04009029 RID: 36905
		public static string PropBMEndOffset = "bm_end_offset";

		// Token: 0x0400902A RID: 36906
		public string Name;

		// Token: 0x0400902B RID: 36907
		public bool IsDefault;

		// Token: 0x0400902C RID: 36908
		public string Title;

		// Token: 0x0400902D RID: 36909
		public CooldownPreset.CooldownTypes CooldownType = CooldownPreset.CooldownTypes.Fill;

		// Token: 0x0400902E RID: 36910
		public float CooldownFillMax;

		// Token: 0x0400902F RID: 36911
		public int NextCooldownTime;

		// Token: 0x04009030 RID: 36912
		public int StartCooldownTime;

		// Token: 0x04009031 RID: 36913
		public int AfterDeathCooldownTime;

		// Token: 0x04009032 RID: 36914
		public int BMStartOffset;

		// Token: 0x04009033 RID: 36915
		public int BMEndOffset;

		// Token: 0x04009034 RID: 36916
		public List<TwitchCooldownEntry> CooldownMaxEntries = new List<TwitchCooldownEntry>();

		// Token: 0x02001855 RID: 6229
		public enum CooldownTypes
		{
			// Token: 0x04009036 RID: 36918
			Always,
			// Token: 0x04009037 RID: 36919
			Fill,
			// Token: 0x04009038 RID: 36920
			None
		}
	}
}
