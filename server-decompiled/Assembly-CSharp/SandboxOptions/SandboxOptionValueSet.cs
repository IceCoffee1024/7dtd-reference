using System;

namespace SandboxOptions
{
	// Token: 0x0200189D RID: 6301
	public abstract class SandboxOptionValueSet
	{
		// Token: 0x0600C285 RID: 49797 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void Init()
		{
		}

		// Token: 0x0600C286 RID: 49798
		public abstract bool GetValue(int index, out object val);

		// Token: 0x0600C287 RID: 49799 RVA: 0x00480203 File Offset: 0x0047E403
		public virtual bool GetBoolValue(int index, out bool val)
		{
			val = false;
			return false;
		}

		// Token: 0x0600C288 RID: 49800 RVA: 0x00480209 File Offset: 0x0047E409
		public virtual bool GetIntValue(int index, out int val)
		{
			val = 0;
			return false;
		}

		// Token: 0x0600C289 RID: 49801 RVA: 0x0048020F File Offset: 0x0047E40F
		public virtual bool GetFloatValue(int index, out float val)
		{
			val = 0f;
			return false;
		}

		// Token: 0x0600C28A RID: 49802 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual bool GetBoolValue(int index)
		{
			return false;
		}

		// Token: 0x0600C28B RID: 49803 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual int GetBoolIndex(bool val)
		{
			return 0;
		}

		// Token: 0x0600C28C RID: 49804 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual int GetFloatIndex(float val)
		{
			return 0;
		}

		// Token: 0x0600C28D RID: 49805 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual int GetIntIndex(int val)
		{
			return 0;
		}

		// Token: 0x0600C28E RID: 49806 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual bool IsValidIndex(int index)
		{
			return false;
		}

		// Token: 0x0600C28F RID: 49807
		public abstract int GetValueCount();

		// Token: 0x0600C290 RID: 49808
		public abstract string GetDisplayAtIndex(int index, string languageName = null);

		// Token: 0x0600C291 RID: 49809 RVA: 0x00032147 File Offset: 0x00030347
		public virtual string GetValueListString()
		{
			return "";
		}

		// Token: 0x0600C292 RID: 49810 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public SandboxOptionValueSet()
		{
		}

		// Token: 0x040093A9 RID: 37801
		public string[] DisplayValues;

		// Token: 0x040093AA RID: 37802
		public string[] AlternateDisplayValues;

		// Token: 0x040093AB RID: 37803
		public string DisplayFormat;

		// Token: 0x0200189E RID: 6302
		public class DisplayOverride
		{
			// Token: 0x040093AC RID: 37804
			public int Index;

			// Token: 0x040093AD RID: 37805
			public string DisplayText;
		}
	}
}
