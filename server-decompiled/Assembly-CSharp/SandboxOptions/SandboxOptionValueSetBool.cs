using System;

namespace SandboxOptions
{
	// Token: 0x02001879 RID: 6265
	public class SandboxOptionValueSetBool : SandboxOptionValueSet
	{
		// Token: 0x0600C10E RID: 49422 RVA: 0x0047A118 File Offset: 0x00478318
		public override bool GetValue(int index, out object val)
		{
			bool flag;
			bool boolValue = this.GetBoolValue(index, out flag);
			val = flag;
			return boolValue;
		}

		// Token: 0x0600C10F RID: 49423 RVA: 0x0047A136 File Offset: 0x00478336
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

		// Token: 0x0600C110 RID: 49424 RVA: 0x0047A157 File Offset: 0x00478357
		public override bool GetBoolValue(int index)
		{
			return this.BoolValues[index];
		}

		// Token: 0x0600C111 RID: 49425 RVA: 0x0047A164 File Offset: 0x00478364
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

		// Token: 0x0600C112 RID: 49426 RVA: 0x0047A192 File Offset: 0x00478392
		public override bool IsValidIndex(int index)
		{
			return index >= 0 && index < this.BoolValues.Length;
		}

		// Token: 0x0600C113 RID: 49427 RVA: 0x0047A1A5 File Offset: 0x004783A5
		public override int GetValueCount()
		{
			return this.BoolValues.Length;
		}

		// Token: 0x0600C114 RID: 49428 RVA: 0x0047A1AF File Offset: 0x004783AF
		public override string GetDisplayAtIndex(int index)
		{
			if (index < 0 || index >= this.DisplayValues.Length)
			{
				return "";
			}
			return Localization.Get(this.DisplayValues[index], false, null);
		}

		// Token: 0x0600C115 RID: 49429 RVA: 0x0047A1D5 File Offset: 0x004783D5
		public override string GetValueListString()
		{
			return string.Join<bool>(", ", this.BoolValues);
		}

		// Token: 0x04009285 RID: 37509
		public bool[] BoolValues;
	}
}
