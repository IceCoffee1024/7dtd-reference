using System;
using System.Collections.Generic;
using System.Text;

namespace SandboxOptions
{
	// Token: 0x020018C8 RID: 6344
	public class SandboxOptionPreset
	{
		// Token: 0x17001887 RID: 6279
		// (get) Token: 0x0600C3D7 RID: 50135 RVA: 0x00484601 File Offset: 0x00482801
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

		// Token: 0x17001888 RID: 6280
		// (get) Token: 0x0600C3D8 RID: 50136 RVA: 0x00484624 File Offset: 0x00482824
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

		// Token: 0x17001889 RID: 6281
		// (get) Token: 0x0600C3D9 RID: 50137 RVA: 0x00484647 File Offset: 0x00482847
		public string SandboxCode
		{
			get
			{
				return this.saveOptionsToCode();
			}
		}

		// Token: 0x0600C3DA RID: 50138 RVA: 0x0048464F File Offset: 0x0048284F
		public bool ForceShowOption(SandboxOptions option)
		{
			return this.AlwaysShow != null && this.AlwaysShow.Contains(option);
		}

		// Token: 0x0600C3DB RID: 50139 RVA: 0x00484668 File Offset: 0x00482868
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

		// Token: 0x04009462 RID: 37986
		public string Name;

		// Token: 0x04009463 RID: 37987
		public string LocalizedName;

		// Token: 0x04009464 RID: 37988
		public string Description = "";

		// Token: 0x04009465 RID: 37989
		public string DescriptionKey = "";

		// Token: 0x04009466 RID: 37990
		public string Icon = "";

		// Token: 0x04009467 RID: 37991
		public string Group = "";

		// Token: 0x04009468 RID: 37992
		public List<SandboxOptions> AlwaysShow;

		// Token: 0x04009469 RID: 37993
		public bool IsDefault;

		// Token: 0x0400946A RID: 37994
		public bool IsUserPreset;

		// Token: 0x0400946B RID: 37995
		public bool IsModded;

		// Token: 0x0400946C RID: 37996
		public bool IsCustomPreset;

		// Token: 0x0400946D RID: 37997
		public short DifficultyRating;

		// Token: 0x0400946E RID: 37998
		public readonly Dictionary<SandboxOptions, int> PresetValues = new Dictionary<SandboxOptions, int>();
	}
}
