using System;

namespace SandboxOptions
{
	// Token: 0x020018C9 RID: 6345
	public abstract class SandboxOptionValueSet
	{
		// Token: 0x0600C3DD RID: 50141 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void Init()
		{
		}

		// Token: 0x0600C3DE RID: 50142
		public abstract bool GetValue(int index, out object val);

		// Token: 0x0600C3DF RID: 50143 RVA: 0x0048473F File Offset: 0x0048293F
		public virtual bool GetBoolValue(int index, out bool val)
		{
			val = false;
			return false;
		}

		// Token: 0x0600C3E0 RID: 50144 RVA: 0x00484745 File Offset: 0x00482945
		public virtual bool GetIntValue(int index, out int val)
		{
			val = 0;
			return false;
		}

		// Token: 0x0600C3E1 RID: 50145 RVA: 0x0048474B File Offset: 0x0048294B
		public virtual bool GetFloatValue(int index, out float val)
		{
			val = 0f;
			return false;
		}

		// Token: 0x0600C3E2 RID: 50146 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual bool GetBoolValue(int index)
		{
			return false;
		}

		// Token: 0x0600C3E3 RID: 50147 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual int GetBoolIndex(bool val)
		{
			return 0;
		}

		// Token: 0x0600C3E4 RID: 50148 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual int GetFloatIndex(float val)
		{
			return 0;
		}

		// Token: 0x0600C3E5 RID: 50149 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual int GetIntIndex(int val)
		{
			return 0;
		}

		// Token: 0x0600C3E6 RID: 50150 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual bool IsValidIndex(int index)
		{
			return false;
		}

		// Token: 0x0600C3E7 RID: 50151
		public abstract int GetValueCount();

		// Token: 0x0600C3E8 RID: 50152
		public abstract string GetDisplayAtIndex(int index, string languageName = null);

		// Token: 0x0600C3E9 RID: 50153 RVA: 0x0003223F File Offset: 0x0003043F
		public virtual string GetValueListString()
		{
			return "";
		}

		// Token: 0x0600C3EA RID: 50154 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public SandboxOptionValueSet()
		{
		}

		// Token: 0x0400946F RID: 37999
		public string[] DisplayValues;

		// Token: 0x04009470 RID: 38000
		public string[] AlternateDisplayValues;

		// Token: 0x04009471 RID: 38001
		public string DisplayFormat;

		// Token: 0x020018CA RID: 6346
		public class DisplayOverride
		{
			// Token: 0x04009472 RID: 38002
			public int Index;

			// Token: 0x04009473 RID: 38003
			public string DisplayText;
		}
	}
}
