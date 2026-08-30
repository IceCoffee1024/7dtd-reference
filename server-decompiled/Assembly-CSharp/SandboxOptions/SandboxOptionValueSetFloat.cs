using System;

namespace SandboxOptions
{
	// Token: 0x0200187A RID: 6266
	public class SandboxOptionValueSetFloat : SandboxOptionValueSet
	{
		// Token: 0x0600C117 RID: 49431 RVA: 0x0047A1F0 File Offset: 0x004783F0
		public override bool GetValue(int index, out object val)
		{
			float num;
			bool floatValue = this.GetFloatValue(index, out num);
			val = num;
			return floatValue;
		}

		// Token: 0x0600C118 RID: 49432 RVA: 0x0047A20E File Offset: 0x0047840E
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

		// Token: 0x0600C119 RID: 49433 RVA: 0x0047A234 File Offset: 0x00478434
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

		// Token: 0x0600C11A RID: 49434 RVA: 0x0047A262 File Offset: 0x00478462
		public override bool IsValidIndex(int index)
		{
			return index >= 0 && index < this.FloatValues.Length;
		}

		// Token: 0x0600C11B RID: 49435 RVA: 0x0047A275 File Offset: 0x00478475
		public override int GetValueCount()
		{
			return this.FloatValues.Length;
		}

		// Token: 0x0600C11C RID: 49436 RVA: 0x0047A280 File Offset: 0x00478480
		public override string GetDisplayAtIndex(int index)
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
			return string.Format(text2, (text != "") ? text : (this.FloatValues[index] * 100f));
		}

		// Token: 0x0600C11D RID: 49437 RVA: 0x0047A33A File Offset: 0x0047853A
		public override string GetValueListString()
		{
			return string.Join<float>(", ", this.FloatValues);
		}

		// Token: 0x04009286 RID: 37510
		public float[] FloatValues;
	}
}
