using System;
using System.Collections.Generic;
using System.Globalization;

namespace Twitch
{
	// Token: 0x02001802 RID: 6146
	public class CooldownPreset
	{
		// Token: 0x0600BD8A RID: 48522 RVA: 0x00462D14 File Offset: 0x00460F14
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

		// Token: 0x0600BD8B RID: 48523 RVA: 0x00462D64 File Offset: 0x00460F64
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

		// Token: 0x0600BD8C RID: 48524 RVA: 0x00462E78 File Offset: 0x00461078
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

		// Token: 0x04008E3F RID: 36415
		public static string PropName = "name";

		// Token: 0x04008E40 RID: 36416
		public static string PropTitle = "title";

		// Token: 0x04008E41 RID: 36417
		public static string PropTitleKey = "title_key";

		// Token: 0x04008E42 RID: 36418
		public static string PropCooldownType = "cooldown_type";

		// Token: 0x04008E43 RID: 36419
		public static string PropIsDefault = "is_default";

		// Token: 0x04008E44 RID: 36420
		public static string PropStartCooldown = "start_cooldown";

		// Token: 0x04008E45 RID: 36421
		public static string PropDeathCooldown = "death_cooldown";

		// Token: 0x04008E46 RID: 36422
		public static string PropBMStartOffset = "bm_start_offset";

		// Token: 0x04008E47 RID: 36423
		public static string PropBMEndOffset = "bm_end_offset";

		// Token: 0x04008E48 RID: 36424
		public string Name;

		// Token: 0x04008E49 RID: 36425
		public bool IsDefault;

		// Token: 0x04008E4A RID: 36426
		public string Title;

		// Token: 0x04008E4B RID: 36427
		public CooldownPreset.CooldownTypes CooldownType = CooldownPreset.CooldownTypes.Fill;

		// Token: 0x04008E4C RID: 36428
		public float CooldownFillMax;

		// Token: 0x04008E4D RID: 36429
		public int NextCooldownTime;

		// Token: 0x04008E4E RID: 36430
		public int StartCooldownTime;

		// Token: 0x04008E4F RID: 36431
		public int AfterDeathCooldownTime;

		// Token: 0x04008E50 RID: 36432
		public int BMStartOffset;

		// Token: 0x04008E51 RID: 36433
		public int BMEndOffset;

		// Token: 0x04008E52 RID: 36434
		public List<TwitchCooldownEntry> CooldownMaxEntries = new List<TwitchCooldownEntry>();

		// Token: 0x02001803 RID: 6147
		public enum CooldownTypes
		{
			// Token: 0x04008E54 RID: 36436
			Always,
			// Token: 0x04008E55 RID: 36437
			Fill,
			// Token: 0x04008E56 RID: 36438
			None
		}
	}
}
