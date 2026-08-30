using System;
using System.Collections.Generic;
using System.Text;

namespace SandboxOptions
{
	// Token: 0x02001876 RID: 6262
	public class SandboxOptionPreset
	{
		// Token: 0x17001773 RID: 6003
		// (get) Token: 0x0600C0F9 RID: 49401 RVA: 0x00479FC1 File Offset: 0x004781C1
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

		// Token: 0x17001774 RID: 6004
		// (get) Token: 0x0600C0FA RID: 49402 RVA: 0x00479FE4 File Offset: 0x004781E4
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

		// Token: 0x17001775 RID: 6005
		// (get) Token: 0x0600C0FB RID: 49403 RVA: 0x0047A007 File Offset: 0x00478207
		public string SandboxCode
		{
			get
			{
				return this.saveOptionsToCode();
			}
		}

		// Token: 0x0600C0FC RID: 49404 RVA: 0x0047A00F File Offset: 0x0047820F
		public bool ForceShowOption(SandboxOptions option)
		{
			return this.AlwaysShow != null && this.AlwaysShow.Contains(option);
		}

		// Token: 0x0600C0FD RID: 49405 RVA: 0x0047A028 File Offset: 0x00478228
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

		// Token: 0x04009273 RID: 37491
		public string Name;

		// Token: 0x04009274 RID: 37492
		public string LocalizedName;

		// Token: 0x04009275 RID: 37493
		public string Description = "";

		// Token: 0x04009276 RID: 37494
		public string DescriptionKey = "";

		// Token: 0x04009277 RID: 37495
		public string Icon = "";

		// Token: 0x04009278 RID: 37496
		public string Group = "";

		// Token: 0x04009279 RID: 37497
		public List<SandboxOptions> AlwaysShow;

		// Token: 0x0400927A RID: 37498
		public bool IsDefault;

		// Token: 0x0400927B RID: 37499
		public bool IsUserPreset;

		// Token: 0x0400927C RID: 37500
		public bool IsModded;

		// Token: 0x0400927D RID: 37501
		public bool IsCustomPreset;

		// Token: 0x0400927E RID: 37502
		public short DifficultyRating;

		// Token: 0x0400927F RID: 37503
		public readonly Dictionary<SandboxOptions, int> PresetValues = new Dictionary<SandboxOptions, int>();
	}
}
