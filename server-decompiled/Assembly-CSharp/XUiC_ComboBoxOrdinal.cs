using System;
using UnityEngine.Scripting;

// Token: 0x02000E96 RID: 3734
[Preserve]
public abstract class XUiC_ComboBoxOrdinal<TValue> : XUiC_ComboBox<TValue> where TValue : struct, IEquatable<TValue>, IComparable<TValue>, IFormattable, IConvertible
{
	// Token: 0x17000D58 RID: 3416
	// (get) Token: 0x060072EC RID: 29420 RVA: 0x002C0A9F File Offset: 0x002BEC9F
	// (set) Token: 0x060072ED RID: 29421 RVA: 0x002C0AA7 File Offset: 0x002BECA7
	[XuiXmlAttribute("format_string", false)]
	public string FormatString { get; set; }

	// Token: 0x17000D59 RID: 3417
	// (get) Token: 0x060072EE RID: 29422 RVA: 0x002C04F4 File Offset: 0x002BE6F4
	// (set) Token: 0x060072EF RID: 29423 RVA: 0x002C0AB0 File Offset: 0x002BECB0
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

	// Token: 0x17000D5A RID: 3418
	// (get) Token: 0x060072F0 RID: 29424 RVA: 0x00100DD1 File Offset: 0x000FEFD1
	public override int IndexElementCount
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return -1;
		}
	}

	// Token: 0x17000D5B RID: 3419
	// (get) Token: 0x060072F1 RID: 29425 RVA: 0x00100DD1 File Offset: 0x000FEFD1
	public override int IndexMarkerIndex
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return -1;
		}
	}

	// Token: 0x17000D5C RID: 3420
	// (get) Token: 0x060072F2 RID: 29426 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsNumber
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return true;
		}
	}

	// Token: 0x060072F3 RID: 29427 RVA: 0x002C0B24 File Offset: 0x002BED24
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

	// Token: 0x060072F4 RID: 29428 RVA: 0x002C0B85 File Offset: 0x002BED85
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateLabel()
	{
		base.ValueText = ((this.CustomValueFormatter != null) ? this.CustomValueFormatter(this.currentValue) : this.currentValue.ToString(this.FormatString, Utils.StandardCulture));
	}

	// Token: 0x060072F5 RID: 29429 RVA: 0x002C0BC4 File Offset: 0x002BEDC4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isDifferentValue(TValue _oldVal, TValue _currentValue)
	{
		return !_oldVal.Equals(_currentValue);
	}

	// Token: 0x060072F6 RID: 29430 RVA: 0x002C0BD7 File Offset: 0x002BEDD7
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void BackPressed()
	{
		if (this.currentValue.CompareTo(this.Min) < 0)
		{
			this.currentValue = (this.Wrap ? this.Max : this.Min);
		}
	}

	// Token: 0x060072F7 RID: 29431 RVA: 0x002C0C0F File Offset: 0x002BEE0F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void ForwardPressed()
	{
		if (this.currentValue.CompareTo(this.Max) > 0)
		{
			this.currentValue = (this.Wrap ? this.Min : this.Max);
		}
	}

	// Token: 0x060072F8 RID: 29432 RVA: 0x002C0C47 File Offset: 0x002BEE47
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isMax()
	{
		return this.currentValue.CompareTo(this.Max) == 0;
	}

	// Token: 0x060072F9 RID: 29433 RVA: 0x002C0C63 File Offset: 0x002BEE63
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isMin()
	{
		return this.currentValue.CompareTo(this.Min) == 0;
	}

	// Token: 0x060072FA RID: 29434 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isEmpty()
	{
		return false;
	}

	// Token: 0x060072FB RID: 29435 RVA: 0x002C0C7F File Offset: 0x002BEE7F
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_ComboBoxOrdinal()
	{
	}

	// Token: 0x04005673 RID: 22131
	public TValue Min;

	// Token: 0x04005674 RID: 22132
	public TValue Max;
}
