using System;

namespace SandboxOptions
{
	// Token: 0x0200187B RID: 6267
	public class SandboxOptionValueSetInt : SandboxOptionValueSet
	{
		// Token: 0x0600C11F RID: 49439 RVA: 0x0047A34C File Offset: 0x0047854C
		public override bool GetValue(int index, out object val)
		{
			int num;
			bool intValue = this.GetIntValue(index, out num);
			val = num;
			return intValue;
		}

		// Token: 0x0600C120 RID: 49440 RVA: 0x0047A36A File Offset: 0x0047856A
		public override bool GetIntValue(int index, out int val)
		{
			if (index >= 0 && index < this.IntValues.Length)
			{
				val = this.IntValues[index];
				return true;
			}
			val = 0;
			return false;
		}

		// Token: 0x0600C121 RID: 49441 RVA: 0x0047A38C File Offset: 0x0047858C
		public override int GetIntIndex(int val)
		{
			for (int i = 0; i < this.IntValues.Length; i++)
			{
				if (val == this.IntValues[i])
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600C122 RID: 49442 RVA: 0x0047A3BA File Offset: 0x004785BA
		public override bool IsValidIndex(int index)
		{
			return index >= 0 && index < this.IntValues.Length;
		}

		// Token: 0x0600C123 RID: 49443 RVA: 0x0047A3CD File Offset: 0x004785CD
		public override int GetValueCount()
		{
			return this.IntValues.Length;
		}

		// Token: 0x0600C124 RID: 49444 RVA: 0x0047A3D8 File Offset: 0x004785D8
		public override string GetDisplayAtIndex(int index)
		{
			if (index < 0 || index >= this.IntValues.Length)
			{
				return "";
			}
			string text = null;
			if (this.AlternateDisplayValues != null && this.AlternateDisplayValues.Length > index)
			{
				text = this.AlternateDisplayValues[index];
			}
			string text2;
			if (this.DisplayValues != null && this.DisplayValues.Length > index && this.DisplayValues[index] != null)
			{
				text2 = Localization.Get(this.DisplayValues[index], false, null);
			}
			else if (this.DisplayFormat != null)
			{
				text2 = Localization.Get(this.DisplayFormat, false, null);
			}
			else
			{
				text2 = "{0}";
			}
			return string.Format(text2, (text != null) ? text : this.IntValues[index]);
		}

		// Token: 0x0600C125 RID: 49445 RVA: 0x0047A47E File Offset: 0x0047867E
		public override string GetValueListString()
		{
			return string.Join<int>(", ", this.IntValues);
		}

		// Token: 0x04009287 RID: 37511
		public int[] IntValues;
	}
}
