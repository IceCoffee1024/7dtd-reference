using System;

namespace SandboxOptions
{
	// Token: 0x020018FE RID: 6398
	public class SandboxOptionValueSetInt : SandboxOptionValueSet
	{
		// Token: 0x0600C512 RID: 50450 RVA: 0x00488360 File Offset: 0x00486560
		public override bool GetValue(int index, out object val)
		{
			int num;
			bool intValue = this.GetIntValue(index, out num);
			val = num;
			return intValue;
		}

		// Token: 0x0600C513 RID: 50451 RVA: 0x0048837E File Offset: 0x0048657E
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

		// Token: 0x0600C514 RID: 50452 RVA: 0x004883A0 File Offset: 0x004865A0
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

		// Token: 0x0600C515 RID: 50453 RVA: 0x004883CE File Offset: 0x004865CE
		public override bool IsValidIndex(int index)
		{
			return index >= 0 && index < this.IntValues.Length;
		}

		// Token: 0x0600C516 RID: 50454 RVA: 0x004883E1 File Offset: 0x004865E1
		public override int GetValueCount()
		{
			return this.IntValues.Length;
		}

		// Token: 0x0600C517 RID: 50455 RVA: 0x004883EC File Offset: 0x004865EC
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

		// Token: 0x0600C518 RID: 50456 RVA: 0x00488492 File Offset: 0x00486692
		public override string GetValueListString()
		{
			return string.Join<int>(", ", this.IntValues);
		}

		// Token: 0x0400955F RID: 38239
		public int[] IntValues;
	}
}
