using System;

namespace SandboxOptions
{
	// Token: 0x020018CC RID: 6348
	public class SandboxOptionValueSetFloat : SandboxOptionValueSet
	{
		// Token: 0x0600C3F5 RID: 50165 RVA: 0x00484830 File Offset: 0x00482A30
		public override bool GetValue(int index, out object val)
		{
			float num;
			bool floatValue = this.GetFloatValue(index, out num);
			val = num;
			return floatValue;
		}

		// Token: 0x0600C3F6 RID: 50166 RVA: 0x0048484E File Offset: 0x00482A4E
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

		// Token: 0x0600C3F7 RID: 50167 RVA: 0x00484873 File Offset: 0x00482A73
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

		// Token: 0x0600C3F8 RID: 50168 RVA: 0x0048489C File Offset: 0x00482A9C
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

		// Token: 0x0600C3F9 RID: 50169 RVA: 0x004848CA File Offset: 0x00482ACA
		public override bool IsValidIndex(int index)
		{
			return index >= 0 && index < this.FloatValues.Length;
		}

		// Token: 0x0600C3FA RID: 50170 RVA: 0x004848DD File Offset: 0x00482ADD
		public override int GetValueCount()
		{
			return this.FloatValues.Length;
		}

		// Token: 0x0600C3FB RID: 50171 RVA: 0x004848E8 File Offset: 0x00482AE8
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

		// Token: 0x0600C3FC RID: 50172 RVA: 0x004849A2 File Offset: 0x00482BA2
		public override string GetValueListString()
		{
			return string.Join<float>(", ", this.FloatValues);
		}

		// Token: 0x04009475 RID: 38005
		public float[] FloatValues;
	}
}
