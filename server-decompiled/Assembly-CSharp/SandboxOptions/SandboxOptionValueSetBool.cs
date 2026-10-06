using System;

namespace SandboxOptions
{
	// Token: 0x020018CB RID: 6347
	public class SandboxOptionValueSetBool : SandboxOptionValueSet
	{
		// Token: 0x0600C3EC RID: 50156 RVA: 0x00484758 File Offset: 0x00482958
		public override bool GetValue(int index, out object val)
		{
			bool flag;
			bool boolValue = this.GetBoolValue(index, out flag);
			val = flag;
			return boolValue;
		}

		// Token: 0x0600C3ED RID: 50157 RVA: 0x00484776 File Offset: 0x00482976
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

		// Token: 0x0600C3EE RID: 50158 RVA: 0x00484797 File Offset: 0x00482997
		public override bool GetBoolValue(int index)
		{
			return this.BoolValues[index];
		}

		// Token: 0x0600C3EF RID: 50159 RVA: 0x004847A4 File Offset: 0x004829A4
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

		// Token: 0x0600C3F0 RID: 50160 RVA: 0x004847D2 File Offset: 0x004829D2
		public override bool IsValidIndex(int index)
		{
			return index >= 0 && index < this.BoolValues.Length;
		}

		// Token: 0x0600C3F1 RID: 50161 RVA: 0x004847E5 File Offset: 0x004829E5
		public override int GetValueCount()
		{
			return this.BoolValues.Length;
		}

		// Token: 0x0600C3F2 RID: 50162 RVA: 0x004847EF File Offset: 0x004829EF
		public override string GetDisplayAtIndex(int index, string languageName = null)
		{
			if (index < 0 || index >= this.DisplayValues.Length)
			{
				return "";
			}
			return Localization.Get(this.DisplayValues[index], false, languageName);
		}

		// Token: 0x0600C3F3 RID: 50163 RVA: 0x00484815 File Offset: 0x00482A15
		public override string GetValueListString()
		{
			return string.Join<bool>(", ", this.BoolValues);
		}

		// Token: 0x04009474 RID: 38004
		public bool[] BoolValues;
	}
}
