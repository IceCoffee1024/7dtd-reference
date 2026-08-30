using System;

namespace SandboxOptions
{
	// Token: 0x02001877 RID: 6263
	public abstract class SandboxOptionValueSet
	{
		// Token: 0x0600C0FF RID: 49407 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void Init()
		{
		}

		// Token: 0x0600C100 RID: 49408
		public abstract bool GetValue(int index, out object val);

		// Token: 0x0600C101 RID: 49409 RVA: 0x0047A0FF File Offset: 0x004782FF
		public virtual bool GetBoolValue(int index, out bool val)
		{
			val = false;
			return false;
		}

		// Token: 0x0600C102 RID: 49410 RVA: 0x0047A105 File Offset: 0x00478305
		public virtual bool GetIntValue(int index, out int val)
		{
			val = 0;
			return false;
		}

		// Token: 0x0600C103 RID: 49411 RVA: 0x0047A10B File Offset: 0x0047830B
		public virtual bool GetFloatValue(int index, out float val)
		{
			val = 0f;
			return false;
		}

		// Token: 0x0600C104 RID: 49412 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual bool GetBoolValue(int index)
		{
			return false;
		}

		// Token: 0x0600C105 RID: 49413 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual int GetBoolIndex(bool val)
		{
			return 0;
		}

		// Token: 0x0600C106 RID: 49414 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual int GetFloatIndex(float val)
		{
			return 0;
		}

		// Token: 0x0600C107 RID: 49415 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual int GetIntIndex(int val)
		{
			return 0;
		}

		// Token: 0x0600C108 RID: 49416 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual bool IsValidIndex(int index)
		{
			return false;
		}

		// Token: 0x0600C109 RID: 49417
		public abstract int GetValueCount();

		// Token: 0x0600C10A RID: 49418
		public abstract string GetDisplayAtIndex(int index);

		// Token: 0x0600C10B RID: 49419 RVA: 0x00031E8B File Offset: 0x0003008B
		public virtual string GetValueListString()
		{
			return "";
		}

		// Token: 0x0600C10C RID: 49420 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public SandboxOptionValueSet()
		{
		}

		// Token: 0x04009280 RID: 37504
		public string[] DisplayValues;

		// Token: 0x04009281 RID: 37505
		public string[] AlternateDisplayValues;

		// Token: 0x04009282 RID: 37506
		public string DisplayFormat;

		// Token: 0x02001878 RID: 6264
		public class DisplayOverride
		{
			// Token: 0x04009283 RID: 37507
			public int Index;

			// Token: 0x04009284 RID: 37508
			public string DisplayText;
		}
	}
}
