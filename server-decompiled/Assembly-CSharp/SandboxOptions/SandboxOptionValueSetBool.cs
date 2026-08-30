using System;

namespace SandboxOptions
{
	// Token: 0x0200189F RID: 6303
	public class SandboxOptionValueSetBool : SandboxOptionValueSet
	{
		// Token: 0x0600C294 RID: 49812 RVA: 0x0048021C File Offset: 0x0047E41C
		public override bool GetValue(int index, out object val)
		{
			bool flag;
			bool boolValue = this.GetBoolValue(index, out flag);
			val = flag;
			return boolValue;
		}

		// Token: 0x0600C295 RID: 49813 RVA: 0x0048023A File Offset: 0x0047E43A
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

		// Token: 0x0600C296 RID: 49814 RVA: 0x0048025B File Offset: 0x0047E45B
		public override bool GetBoolValue(int index)
		{
			return this.BoolValues[index];
		}

		// Token: 0x0600C297 RID: 49815 RVA: 0x00480268 File Offset: 0x0047E468
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

		// Token: 0x0600C298 RID: 49816 RVA: 0x00480296 File Offset: 0x0047E496
		public override bool IsValidIndex(int index)
		{
			return index >= 0 && index < this.BoolValues.Length;
		}

		// Token: 0x0600C299 RID: 49817 RVA: 0x004802A9 File Offset: 0x0047E4A9
		public override int GetValueCount()
		{
			return this.BoolValues.Length;
		}

		// Token: 0x0600C29A RID: 49818 RVA: 0x004802B3 File Offset: 0x0047E4B3
		public override string GetDisplayAtIndex(int index, string languageName = null)
		{
			if (index < 0 || index >= this.DisplayValues.Length)
			{
				return "";
			}
			return Localization.Get(this.DisplayValues[index], false, languageName);
		}

		// Token: 0x0600C29B RID: 49819 RVA: 0x004802D9 File Offset: 0x0047E4D9
		public override string GetValueListString()
		{
			return string.Join<bool>(", ", this.BoolValues);
		}

		// Token: 0x040093AE RID: 37806
		public bool[] BoolValues;
	}
}
