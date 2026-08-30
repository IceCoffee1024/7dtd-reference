using System;
using UnityEngine.Scripting;

// Token: 0x02000E67 RID: 3687
[Preserve]
public abstract class XUiC_ComboBoxOrdinal<TValue> : XUiC_ComboBox<TValue> where TValue : struct, IEquatable<TValue>, IComparable<TValue>, IFormattable, IConvertible
{
	// Token: 0x17000D1E RID: 3358
	// (get) Token: 0x0600722F RID: 29231 RVA: 0x002B8C5F File Offset: 0x002B6E5F
	// (set) Token: 0x06007230 RID: 29232 RVA: 0x002B8C67 File Offset: 0x002B6E67
	[XuiXmlAttribute("format_string", false)]
	public string FormatString { get; set; }

	// Token: 0x17000D1F RID: 3359
	// (get) Token: 0x06007231 RID: 29233 RVA: 0x002B86B4 File Offset: 0x002B68B4
	// (set) Token: 0x06007232 RID: 29234 RVA: 0x002B8C70 File Offset: 0x002B6E70
	public override TValue Value
	{
		get
		{
			return this.currentValue;
		}
		set
		{
			if (!this.currentValue.Equals(value))
			{
				if (value.CompareTo(this.Max) > 0)
				{
					value = this.Max;
				}
				else if (value.CompareTo(this.Min) < 0)
				{
					value = this.Min;
				}
				this.currentValue = value;
				this.IsDirty = true;
				this.UpdateLabel();
			}
		}
	}

	// Token: 0x17000D20 RID: 3360
	// (get) Token: 0x06007233 RID: 29235 RVA: 0x000FEFA9 File Offset: 0x000FD1A9
	public override int IndexElementCount
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return -1;
		}
	}

	// Token: 0x17000D21 RID: 3361
	// (get) Token: 0x06007234 RID: 29236 RVA: 0x000FEFA9 File Offset: 0x000FD1A9
	public override int IndexMarkerIndex
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return -1;
		}
	}

	// Token: 0x17000D22 RID: 3362
	// (get) Token: 0x06007235 RID: 29237 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsNumber
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return true;
		}
	}

	// Token: 0x06007236 RID: 29238 RVA: 0x002B8CE4 File Offset: 0x002B6EE4
	public override void OnOpen()
	{
		if (this.currentValue.CompareTo(this.Max) > 0)
		{
			this.Value = this.Max;
		}
		else if (this.currentValue.CompareTo(this.Min) < 0)
		{
			this.Value = this.Min;
		}
		base.OnOpen();
	}

	// Token: 0x06007237 RID: 29239 RVA: 0x002B8D45 File Offset: 0x002B6F45
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateLabel()
	{
		base.ValueText = ((this.CustomValueFormatter != null) ? this.CustomValueFormatter(this.currentValue) : this.currentValue.ToString(this.FormatString, Utils.StandardCulture));
	}

	// Token: 0x06007238 RID: 29240 RVA: 0x002B8D84 File Offset: 0x002B6F84
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isDifferentValue(TValue _oldVal, TValue _currentValue)
	{
		return !_oldVal.Equals(_currentValue);
	}

	// Token: 0x06007239 RID: 29241 RVA: 0x002B8D97 File Offset: 0x002B6F97
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void BackPressed()
	{
		if (this.currentValue.CompareTo(this.Min) < 0)
		{
			this.currentValue = (this.Wrap ? this.Max : this.Min);
		}
	}

	// Token: 0x0600723A RID: 29242 RVA: 0x002B8DCF File Offset: 0x002B6FCF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void ForwardPressed()
	{
		if (this.currentValue.CompareTo(this.Max) > 0)
		{
			this.currentValue = (this.Wrap ? this.Min : this.Max);
		}
	}

	// Token: 0x0600723B RID: 29243 RVA: 0x002B8E07 File Offset: 0x002B7007
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isMax()
	{
		return this.currentValue.CompareTo(this.Max) == 0;
	}

	// Token: 0x0600723C RID: 29244 RVA: 0x002B8E23 File Offset: 0x002B7023
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isMin()
	{
		return this.currentValue.CompareTo(this.Min) == 0;
	}

	// Token: 0x0600723D RID: 29245 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isEmpty()
	{
		return false;
	}

	// Token: 0x0600723E RID: 29246 RVA: 0x002B8E3F File Offset: 0x002B703F
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_ComboBoxOrdinal()
	{
	}

	// Token: 0x04005573 RID: 21875
	public TValue Min;

	// Token: 0x04005574 RID: 21876
	public TValue Max;
}
