using System;

namespace SandboxOptions
{
	// Token: 0x020018CD RID: 6349
	public class SandboxOptionValueSetInt : SandboxOptionValueSet
	{
		// Token: 0x0600C3FE RID: 50174 RVA: 0x004849B4 File Offset: 0x00482BB4
		public override bool GetValue(int index, out object val)
		{
			int num;
			bool intValue = this.GetIntValue(index, out num);
			val = num;
			return intValue;
		}

		// Token: 0x0600C3FF RID: 50175 RVA: 0x004849D2 File Offset: 0x00482BD2
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

		// Token: 0x0600C400 RID: 50176 RVA: 0x004849F4 File Offset: 0x00482BF4
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

		// Token: 0x0600C401 RID: 50177 RVA: 0x00484A22 File Offset: 0x00482C22
		public override bool IsValidIndex(int index)
		{
			return index >= 0 && index < this.IntValues.Length;
		}

		// Token: 0x0600C402 RID: 50178 RVA: 0x00484A35 File Offset: 0x00482C35
		public override int GetValueCount()
		{
			return this.IntValues.Length;
		}

		// Token: 0x0600C403 RID: 50179 RVA: 0x00484A40 File Offset: 0x00482C40
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

		// Token: 0x0600C404 RID: 50180 RVA: 0x00484AE6 File Offset: 0x00482CE6
		public override string GetValueListString()
		{
			return string.Join<int>(", ", this.IntValues);
		}

		// Token: 0x04009476 RID: 38006
		public int[] IntValues;
	}
}
