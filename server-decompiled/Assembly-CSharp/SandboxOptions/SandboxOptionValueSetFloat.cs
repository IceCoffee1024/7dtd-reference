using System;

namespace SandboxOptions
{
	// Token: 0x020018A0 RID: 6304
	public class SandboxOptionValueSetFloat : SandboxOptionValueSet
	{
		// Token: 0x0600C29D RID: 49821 RVA: 0x004802F4 File Offset: 0x0047E4F4
		public override bool GetValue(int index, out object val)
		{
			float num;
			bool floatValue = this.GetFloatValue(index, out num);
			val = num;
			return floatValue;
		}

		// Token: 0x0600C29E RID: 49822 RVA: 0x00480312 File Offset: 0x0047E512
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

		// Token: 0x0600C29F RID: 49823 RVA: 0x00480337 File Offset: 0x0047E537
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

		// Token: 0x0600C2A0 RID: 49824 RVA: 0x00480360 File Offset: 0x0047E560
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

		// Token: 0x0600C2A1 RID: 49825 RVA: 0x0048038E File Offset: 0x0047E58E
		public override bool IsValidIndex(int index)
		{
			return index >= 0 && index < this.FloatValues.Length;
		}

		// Token: 0x0600C2A2 RID: 49826 RVA: 0x004803A1 File Offset: 0x0047E5A1
		public override int GetValueCount()
		{
			return this.FloatValues.Length;
		}

		// Token: 0x0600C2A3 RID: 49827 RVA: 0x004803AC File Offset: 0x0047E5AC
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

		// Token: 0x0600C2A4 RID: 49828 RVA: 0x00480466 File Offset: 0x0047E666
		public override string GetValueListString()
		{
			return string.Join<float>(", ", this.FloatValues);
		}

		// Token: 0x040093AF RID: 37807
		public float[] FloatValues;
	}
}
