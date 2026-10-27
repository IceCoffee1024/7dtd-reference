using System;
using System.Collections.Generic;
using System.Text;

namespace SandboxOptions
{
	// Token: 0x020018F9 RID: 6393
	public class SandboxOptionPreset
	{
		// Token: 0x170018CF RID: 6351
		// (get) Token: 0x0600C4EB RID: 50411 RVA: 0x00487FAD File Offset: 0x004861AD
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

		// Token: 0x170018D0 RID: 6352
		// (get) Token: 0x0600C4EC RID: 50412 RVA: 0x00487FD0 File Offset: 0x004861D0
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

		// Token: 0x170018D1 RID: 6353
		// (get) Token: 0x0600C4ED RID: 50413 RVA: 0x00487FF3 File Offset: 0x004861F3
		public string SandboxCode
		{
			get
			{
				return this.saveOptionsToCode();
			}
		}

		// Token: 0x0600C4EE RID: 50414 RVA: 0x00487FFB File Offset: 0x004861FB
		public bool ForceShowOption(SandboxOptions option)
		{
			return this.AlwaysShow != null && this.AlwaysShow.Contains(option);
		}

		// Token: 0x0600C4EF RID: 50415 RVA: 0x00488014 File Offset: 0x00486214
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

		// Token: 0x0400954B RID: 38219
		public string Name;

		// Token: 0x0400954C RID: 38220
		public string LocalizedName;

		// Token: 0x0400954D RID: 38221
		public string Description = "";

		// Token: 0x0400954E RID: 38222
		public string DescriptionKey = "";

		// Token: 0x0400954F RID: 38223
		public string Icon = "";

		// Token: 0x04009550 RID: 38224
		public string Group = "";

		// Token: 0x04009551 RID: 38225
		public List<SandboxOptions> AlwaysShow;

		// Token: 0x04009552 RID: 38226
		public bool IsDefault;

		// Token: 0x04009553 RID: 38227
		public bool IsUserPreset;

		// Token: 0x04009554 RID: 38228
		public bool IsModded;

		// Token: 0x04009555 RID: 38229
		public bool IsCustomPreset;

		// Token: 0x04009556 RID: 38230
		public short DifficultyRating;

		// Token: 0x04009557 RID: 38231
		public readonly Dictionary<SandboxOptions, int> PresetValues = new Dictionary<SandboxOptions, int>();
	}
}
