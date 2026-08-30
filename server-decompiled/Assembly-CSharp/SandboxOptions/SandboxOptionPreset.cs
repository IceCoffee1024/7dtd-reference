using System;
using System.Collections.Generic;
using System.Text;

namespace SandboxOptions
{
	// Token: 0x0200189C RID: 6300
	public class SandboxOptionPreset
	{
		// Token: 0x170017E5 RID: 6117
		// (get) Token: 0x0600C27F RID: 49791 RVA: 0x004800C5 File Offset: 0x0047E2C5
		public string DisplayName
		{
			get
			{
				if (string.IsNullOrEmpty(this.LocalizedName))
				{
					return this.Name;
				}
				return Localization.Get(this.LocalizedName, false, null);
			}
		}

		// Token: 0x170017E6 RID: 6118
		// (get) Token: 0x0600C280 RID: 49792 RVA: 0x004800E8 File Offset: 0x0047E2E8
		public string DisplayDescription
		{
			get
			{
				if (string.IsNullOrEmpty(this.DescriptionKey))
				{
					return this.Description;
				}
				return Localization.Get(this.DescriptionKey, false, null);
			}
		}

		// Token: 0x170017E7 RID: 6119
		// (get) Token: 0x0600C281 RID: 49793 RVA: 0x0048010B File Offset: 0x0047E30B
		public string SandboxCode
		{
			get
			{
				return this.saveOptionsToCode();
			}
		}

		// Token: 0x0600C282 RID: 49794 RVA: 0x00480113 File Offset: 0x0047E313
		public bool ForceShowOption(SandboxOptions option)
		{
			return this.AlwaysShow != null && this.AlwaysShow.Contains(option);
		}

		// Token: 0x0600C283 RID: 49795 RVA: 0x0048012C File Offset: 0x0047E32C
		[PublicizedFrom(EAccessModifier.Private)]
		public string saveOptionsToCode()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(SandboxOptionManager.currentVersion);
			foreach (SandboxOptions sandboxOptions in this.PresetValues.Keys)
			{
				stringBuilder.Append(string.Format("{0}{1}", SandboxOptionManager.IndexToAlpha2((int)sandboxOptions), SandboxOptionManager.IndexToAlpha(this.PresetValues[sandboxOptions])));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0400939C RID: 37788
		public string Name;

		// Token: 0x0400939D RID: 37789
		public string LocalizedName;

		// Token: 0x0400939E RID: 37790
		public string Description = "";

		// Token: 0x0400939F RID: 37791
		public string DescriptionKey = "";

		// Token: 0x040093A0 RID: 37792
		public string Icon = "";

		// Token: 0x040093A1 RID: 37793
		public string Group = "";

		// Token: 0x040093A2 RID: 37794
		public List<SandboxOptions> AlwaysShow;

		// Token: 0x040093A3 RID: 37795
		public bool IsDefault;

		// Token: 0x040093A4 RID: 37796
		public bool IsUserPreset;

		// Token: 0x040093A5 RID: 37797
		public bool IsModded;

		// Token: 0x040093A6 RID: 37798
		public bool IsCustomPreset;

		// Token: 0x040093A7 RID: 37799
		public short DifficultyRating;

		// Token: 0x040093A8 RID: 37800
		public readonly Dictionary<SandboxOptions, int> PresetValues = new Dictionary<SandboxOptions, int>();
	}
}
