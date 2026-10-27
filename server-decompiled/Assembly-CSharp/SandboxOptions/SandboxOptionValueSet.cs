using System;

namespace SandboxOptions
{
	// Token: 0x020018FA RID: 6394
	public abstract class SandboxOptionValueSet
	{
		// Token: 0x0600C4F1 RID: 50417 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void Init()
		{
		}

		// Token: 0x0600C4F2 RID: 50418
		public abstract bool GetValue(int index, out object val);

		// Token: 0x0600C4F3 RID: 50419 RVA: 0x004880EB File Offset: 0x004862EB
		public virtual bool GetBoolValue(int index, out bool val)
		{
			val = false;
			return false;
		}

		// Token: 0x0600C4F4 RID: 50420 RVA: 0x004880F1 File Offset: 0x004862F1
		public virtual bool GetIntValue(int index, out int val)
		{
			val = 0;
			return false;
		}

		// Token: 0x0600C4F5 RID: 50421 RVA: 0x004880F7 File Offset: 0x004862F7
		public virtual bool GetFloatValue(int index, out float val)
		{
			val = 0f;
			return false;
		}

		// Token: 0x0600C4F6 RID: 50422 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual bool GetBoolValue(int index)
		{
			return false;
		}

		// Token: 0x0600C4F7 RID: 50423 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual int GetBoolIndex(bool val)
		{
			return 0;
		}

		// Token: 0x0600C4F8 RID: 50424 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual int GetFloatIndex(float val)
		{
			return 0;
		}

		// Token: 0x0600C4F9 RID: 50425 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual int GetIntIndex(int val)
		{
			return 0;
		}

		// Token: 0x0600C4FA RID: 50426 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual bool IsValidIndex(int index)
		{
			return false;
		}

		// Token: 0x0600C4FB RID: 50427
		public abstract int GetValueCount();

		// Token: 0x0600C4FC RID: 50428
		public abstract string GetDisplayAtIndex(int index, string languageName = null);

		// Token: 0x0600C4FD RID: 50429 RVA: 0x000321FF File Offset: 0x000303FF
		public virtual string GetValueListString()
		{
			return "";
		}

		// Token: 0x0600C4FE RID: 50430 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public SandboxOptionValueSet()
		{
		}

		// Token: 0x04009558 RID: 38232
		public string[] DisplayValues;

		// Token: 0x04009559 RID: 38233
		public string[] AlternateDisplayValues;

		// Token: 0x0400955A RID: 38234
		public string DisplayFormat;

		// Token: 0x020018FB RID: 6395
		public class DisplayOverride
		{
			// Token: 0x0400955B RID: 38235
			public int Index;

			// Token: 0x0400955C RID: 38236
			public string DisplayText;
		}
	}
}
