using System;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x02000E97 RID: 3735
[Preserve]
public class XUiC_ComboBoxInt : XUiC_ComboBoxOrdinal<long>
{
	// Token: 0x17000D5D RID: 3421
	// (get) Token: 0x060072FC RID: 29436 RVA: 0x002C0C87 File Offset: 0x002BEE87
	// (set) Token: 0x060072FD RID: 29437 RVA: 0x002C0C8F File Offset: 0x002BEE8F
	public override long ValueGeneric
	{
		get
		{
			return this.Value;
		}
		set
		{
			this.Value = value;
		}
	}

	// Token: 0x17000D5E RID: 3422
	// (get) Token: 0x060072FE RID: 29438 RVA: 0x002C0C98 File Offset: 0x002BEE98
	// (set) Token: 0x060072FF RID: 29439 RVA: 0x002C0CA0 File Offset: 0x002BEEA0
	public override long ValueMinGeneric
	{
		get
		{
			return this.Min;
		}
		set
		{
			this.Min = value;
		}
	}

	// Token: 0x17000D5F RID: 3423
	// (get) Token: 0x06007300 RID: 29440 RVA: 0x002C0CA9 File Offset: 0x002BEEA9
	// (set) Token: 0x06007301 RID: 29441 RVA: 0x002C0CB1 File Offset: 0x002BEEB1
	public override long ValueMaxGeneric
	{
		get
		{
			return this.Max;
		}
		set
		{
			this.Max = value;
		}
	}

	// Token: 0x17000D60 RID: 3424
	// (get) Token: 0x06007302 RID: 29442 RVA: 0x002C0CBA File Offset: 0x002BEEBA
	// (set) Token: 0x06007303 RID: 29443 RVA: 0x002C0CD9 File Offset: 0x002BEED9
	public override double RelativeValue
	{
		get
		{
			return (double)(this.Value - this.Min) / (double)(this.Max - this.Min);
		}
		set
		{
			this.Value = (long)((double)(this.Max - this.Min) * value) + this.Min;
		}
	}

	// Token: 0x06007304 RID: 29444 RVA: 0x002C0CF9 File Offset: 0x002BEEF9
	public XUiC_ComboBoxInt()
	{
		this.Max = long.MaxValue;
		this.Min = long.MinValue;
	}

	// Token: 0x06007305 RID: 29445 RVA: 0x002C0D27 File Offset: 0x002BEF27
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void BackPressed()
	{
		this.currentValue -= this.incrementSize;
		base.BackPressed();
	}

	// Token: 0x06007306 RID: 29446 RVA: 0x002C0D42 File Offset: 0x002BEF42
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void ForwardPressed()
	{
		this.currentValue += this.incrementSize;
		base.ForwardPressed();
	}

	// Token: 0x06007307 RID: 29447 RVA: 0x002C0D5D File Offset: 0x002BEF5D
	[XuiXmlAttribute("value_min", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeValueMin(string _value)
	{
		if (!_value.EqualsCaseInsensitive("@def"))
		{
			this.Min = StringParsers.ParseSInt64(_value, 0, -1, NumberStyles.Integer);
		}
	}

	// Token: 0x06007308 RID: 29448 RVA: 0x002C0D7B File Offset: 0x002BEF7B
	[XuiXmlAttribute("value_max", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeValueMax(string _value)
	{
		if (!_value.EqualsCaseInsensitive("@def"))
		{
			this.Max = StringParsers.ParseSInt64(_value, 0, -1, NumberStyles.Integer);
		}
	}

	// Token: 0x06007309 RID: 29449 RVA: 0x002C0D99 File Offset: 0x002BEF99
	[XuiXmlAttribute("value_increment", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeValueIncrement(string _value)
	{
		if (!_value.EqualsCaseInsensitive("@def"))
		{
			this.incrementSize = StringParsers.ParseSInt64(_value, 0, -1, NumberStyles.Integer);
		}
	}

	// Token: 0x0600730A RID: 29450 RVA: 0x002C0DB8 File Offset: 0x002BEFB8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void incrementalChangeValue(double _value)
	{
		long num = (long)((double)(this.Max - this.Min) * _value * 0.5);
		if (_value > 0.0 && num == 0L)
		{
			num = 1L;
		}
		else if (_value < 0.0 && num == 0L)
		{
			num = -1L;
		}
		if (num > this.incrementSize)
		{
			num = this.incrementSize;
		}
		else if (num < -this.incrementSize)
		{
			num = -this.incrementSize;
		}
		long value = this.Value;
		long num2 = this.Value + num;
		if (_value < 0.0 && num2 < this.Min && this.Wrap)
		{
			num2 = this.Max;
		}
		else if (_value > 0.0 && num2 > this.Max && this.Wrap)
		{
			num2 = this.Min;
		}
		this.Value = num2;
		base.TriggerValueChangedEvent(value);
	}

	// Token: 0x04005675 RID: 22133
	[PublicizedFrom(EAccessModifier.Private)]
	public long incrementSize = 1L;
}
