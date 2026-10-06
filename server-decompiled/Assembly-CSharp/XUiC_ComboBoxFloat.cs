using System;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x02000E8D RID: 3725
[Preserve]
public class XUiC_ComboBoxFloat : XUiC_ComboBoxOrdinal<double>
{
	// Token: 0x17000D5B RID: 3419
	// (get) Token: 0x060072E6 RID: 29414 RVA: 0x002BF927 File Offset: 0x002BDB27
	// (set) Token: 0x060072E7 RID: 29415 RVA: 0x002BF92F File Offset: 0x002BDB2F
	public override long ValueGeneric { get; set; }

	// Token: 0x17000D5C RID: 3420
	// (get) Token: 0x060072E8 RID: 29416 RVA: 0x002BF938 File Offset: 0x002BDB38
	// (set) Token: 0x060072E9 RID: 29417 RVA: 0x002BF940 File Offset: 0x002BDB40
	public override long ValueMinGeneric { get; set; }

	// Token: 0x17000D5D RID: 3421
	// (get) Token: 0x060072EA RID: 29418 RVA: 0x002BF949 File Offset: 0x002BDB49
	// (set) Token: 0x060072EB RID: 29419 RVA: 0x002BF951 File Offset: 0x002BDB51
	public override long ValueMaxGeneric { get; set; }

	// Token: 0x17000D5E RID: 3422
	// (get) Token: 0x060072EC RID: 29420 RVA: 0x002BF95A File Offset: 0x002BDB5A
	// (set) Token: 0x060072ED RID: 29421 RVA: 0x002BF977 File Offset: 0x002BDB77
	public override double RelativeValue
	{
		get
		{
			return (this.Value - this.Min) / (this.Max - this.Min);
		}
		set
		{
			this.Value = (this.Max - this.Min) * value + this.Min;
		}
	}

	// Token: 0x060072EE RID: 29422 RVA: 0x002BF995 File Offset: 0x002BDB95
	public XUiC_ComboBoxFloat()
	{
		this.Max = 1.0;
		this.Min = 0.0;
	}

	// Token: 0x060072EF RID: 29423 RVA: 0x002BF9CA File Offset: 0x002BDBCA
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void BackPressed()
	{
		this.currentValue -= this.incrementSize;
		base.BackPressed();
	}

	// Token: 0x060072F0 RID: 29424 RVA: 0x002BF9E5 File Offset: 0x002BDBE5
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void ForwardPressed()
	{
		this.currentValue += this.incrementSize;
		base.ForwardPressed();
	}

	// Token: 0x060072F1 RID: 29425 RVA: 0x002BFA00 File Offset: 0x002BDC00
	[XuiXmlAttribute("value_min", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeValueMin(string _value)
	{
		if (!_value.EqualsCaseInsensitive("@def"))
		{
			this.Min = StringParsers.ParseDouble(_value, 0, -1, NumberStyles.Any);
		}
	}

	// Token: 0x060072F2 RID: 29426 RVA: 0x002BFA22 File Offset: 0x002BDC22
	[XuiXmlAttribute("value_max", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeValueMax(string _value)
	{
		if (!_value.EqualsCaseInsensitive("@def"))
		{
			this.Max = StringParsers.ParseDouble(_value, 0, -1, NumberStyles.Any);
		}
	}

	// Token: 0x060072F3 RID: 29427 RVA: 0x002BFA44 File Offset: 0x002BDC44
	[XuiXmlAttribute("value_increment", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeValueIncrement(string _value)
	{
		if (!_value.EqualsCaseInsensitive("@def"))
		{
			this.incrementSize = StringParsers.ParseDouble(_value, 0, -1, NumberStyles.Any);
		}
	}

	// Token: 0x060072F4 RID: 29428 RVA: 0x002BFA68 File Offset: 0x002BDC68
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void incrementalChangeValue(double _value)
	{
		double num = (this.Max - this.Min) * _value * 0.5;
		if (num > this.incrementSize)
		{
			num = this.incrementSize;
		}
		else if (num < -this.incrementSize)
		{
			num = -this.incrementSize;
		}
		double value = this.Value;
		double num2 = this.Value + num;
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

	// Token: 0x04005651 RID: 22097
	[PublicizedFrom(EAccessModifier.Private)]
	public double incrementSize = 1.0;
}
