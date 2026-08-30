using System;

namespace SandboxOptions
{
	// Token: 0x020018A1 RID: 6305
	public class SandboxOptionValueSetInt : SandboxOptionValueSet
	{
		// Token: 0x0600C2A6 RID: 49830 RVA: 0x00480478 File Offset: 0x0047E678
		public override bool GetValue(int index, out object val)
		{
			int num;
			bool intValue = this.GetIntValue(index, out num);
			val = num;
			return intValue;
		}

		// Token: 0x0600C2A7 RID: 49831 RVA: 0x00480496 File Offset: 0x0047E696
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

		// Token: 0x0600C2A8 RID: 49832 RVA: 0x004804B8 File Offset: 0x0047E6B8
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

		// Token: 0x0600C2A9 RID: 49833 RVA: 0x004804E6 File Offset: 0x0047E6E6
		public override bool IsValidIndex(int index)
		{
			return index >= 0 && index < this.IntValues.Length;
		}

		// Token: 0x0600C2AA RID: 49834 RVA: 0x004804F9 File Offset: 0x0047E6F9
		public override int GetValueCount()
		{
			return this.IntValues.Length;
		}

		// Token: 0x0600C2AB RID: 49835 RVA: 0x00480504 File Offset: 0x0047E704
		public override string GetDisplayAtIndex(int index, string languageName = null)
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
				text2 = Localization.Get(this.DisplayValues[index], false, languageName);
			}
			else if (this.DisplayFormat != null)
			{
				text2 = Localization.Get(this.DisplayFormat, false, languageName);
			}
			else
			{
				text2 = "{0}";
			}
			return string.Format(text2, (text != null) ? text : this.IntValues[index]);
		}

		// Token: 0x0600C2AC RID: 49836 RVA: 0x004805AA File Offset: 0x0047E7AA
		public override string GetValueListString()
		{
			return string.Join<int>(", ", this.IntValues);
		}

		// Token: 0x040093B0 RID: 37808
		public int[] IntValues;
	}
}
