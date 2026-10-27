using System;

namespace SandboxOptions
{
	// Token: 0x020018FC RID: 6396
	public class SandboxOptionValueSetBool : SandboxOptionValueSet
	{
		// Token: 0x0600C500 RID: 50432 RVA: 0x00488104 File Offset: 0x00486304
		public override bool GetValue(int index, out object val)
		{
			bool flag;
			bool boolValue = this.GetBoolValue(index, out flag);
			val = flag;
			return boolValue;
		}

		// Token: 0x0600C501 RID: 50433 RVA: 0x00488122 File Offset: 0x00486322
		public override bool GetBoolValue(int index, out bool val)
		{
			if (index >= 0 && index < this.BoolValues.Length)
			{
				val = this.BoolValues[index];
				return true;
			}
			val = false;
			return false;
		}

		// Token: 0x0600C502 RID: 50434 RVA: 0x00488143 File Offset: 0x00486343
		public override bool GetBoolValue(int index)
		{
			return this.BoolValues[index];
		}

		// Token: 0x0600C503 RID: 50435 RVA: 0x00488150 File Offset: 0x00486350
		public override int GetBoolIndex(bool val)
		{
			for (int i = 0; i < this.BoolValues.Length; i++)
			{
				if (val == this.BoolValues[i])
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600C504 RID: 50436 RVA: 0x0048817E File Offset: 0x0048637E
		public override bool IsValidIndex(int index)
		{
			return index >= 0 && index < this.BoolValues.Length;
		}

		// Token: 0x0600C505 RID: 50437 RVA: 0x00488191 File Offset: 0x00486391
		public override int GetValueCount()
		{
			return this.BoolValues.Length;
		}

		// Token: 0x0600C506 RID: 50438 RVA: 0x0048819B File Offset: 0x0048639B
		public override string GetDisplayAtIndex(int index, string languageName = null)
		{
			if (index < 0 || index >= this.DisplayValues.Length)
			{
				return "";
			}
			return Localization.Get(this.DisplayValues[index], false, languageName);
		}

		// Token: 0x0600C507 RID: 50439 RVA: 0x004881C1 File Offset: 0x004863C1
		public override string GetValueListString()
		{
			return string.Join<bool>(", ", this.BoolValues);
		}

		// Token: 0x0400955D RID: 38237
		public bool[] BoolValues;
	}
}
