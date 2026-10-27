using System;

namespace SandboxOptions
{
	// Token: 0x020018FD RID: 6397
	public class SandboxOptionValueSetFloat : SandboxOptionValueSet
	{
		// Token: 0x0600C509 RID: 50441 RVA: 0x004881DC File Offset: 0x004863DC
		public override bool GetValue(int index, out object val)
		{
			float num;
			bool floatValue = this.GetFloatValue(index, out num);
			val = num;
			return floatValue;
		}

		// Token: 0x0600C50A RID: 50442 RVA: 0x004881FA File Offset: 0x004863FA
		public override bool GetFloatValue(int index, out float val)
		{
			if (index >= 0 && index < this.FloatValues.Length)
			{
				val = this.FloatValues[index];
				return true;
			}
			val = 0f;
			return false;
		}

		// Token: 0x0600C50B RID: 50443 RVA: 0x0048821F File Offset: 0x0048641F
		public override bool GetIntValue(int index, out int val)
		{
			if (index >= 0 && index < this.FloatValues.Length)
			{
				val = (int)(this.FloatValues[index] * 100f);
				return true;
			}
			val = 0;
			return false;
		}

		// Token: 0x0600C50C RID: 50444 RVA: 0x00488248 File Offset: 0x00486448
		public override int GetFloatIndex(float val)
		{
			for (int i = 0; i < this.FloatValues.Length; i++)
			{
				if (val == this.FloatValues[i])
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600C50D RID: 50445 RVA: 0x00488276 File Offset: 0x00486476
		public override bool IsValidIndex(int index)
		{
			return index >= 0 && index < this.FloatValues.Length;
		}

		// Token: 0x0600C50E RID: 50446 RVA: 0x00488289 File Offset: 0x00486489
		public override int GetValueCount()
		{
			return this.FloatValues.Length;
		}

		// Token: 0x0600C50F RID: 50447 RVA: 0x00488294 File Offset: 0x00486494
		public override string GetDisplayAtIndex(int index, string languageName = null)
		{
			if (index < 0 || index >= this.FloatValues.Length)
			{
				return "";
			}
			string text = "";
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
			return string.Format(text2, (text != "") ? text : (this.FloatValues[index] * 100f));
		}

		// Token: 0x0600C510 RID: 50448 RVA: 0x0048834E File Offset: 0x0048654E
		public override string GetValueListString()
		{
			return string.Join<float>(", ", this.FloatValues);
		}

		// Token: 0x0400955E RID: 38238
		public float[] FloatValues;
	}
}
