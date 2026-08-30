using System;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x02000E7E RID: 3710
[Preserve]
public class XUiC_ComboBoxFloat : XUiC_ComboBoxOrdinal<double>
{
	// Token: 0x17000D40 RID: 3392
	// (get) Token: 0x060072EE RID: 29422 RVA: 0x002BC577 File Offset: 0x002BA777
	// (set) Token: 0x060072EF RID: 29423 RVA: 0x002BC57F File Offset: 0x002BA77F
	public override long ValueGeneric { get; set; }

	// Token: 0x17000D41 RID: 3393
	// (get) Token: 0x060072F0 RID: 29424 RVA: 0x002BC588 File Offset: 0x002BA788
	// (set) Token: 0x060072F1 RID: 29425 RVA: 0x002BC590 File Offset: 0x002BA790
	public override long ValueMinGeneric { get; set; }

	// Token: 0x17000D42 RID: 3394
	// (get) Token: 0x060072F2 RID: 29426 RVA: 0x002BC599 File Offset: 0x002BA799
	// (set) Token: 0x060072F3 RID: 29427 RVA: 0x002BC5A1 File Offset: 0x002BA7A1
	public override long ValueMaxGeneric { get; set; }

	// Token: 0x17000D43 RID: 3395
	// (get) Token: 0x060072F4 RID: 29428 RVA: 0x002BC5AA File Offset: 0x002BA7AA
	// (set) Token: 0x060072F5 RID: 29429 RVA: 0x002BC5C7 File Offset: 0x002BA7C7
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

	// Token: 0x060072F6 RID: 29430 RVA: 0x002BC5E5 File Offset: 0x002BA7E5
	public XUiC_ComboBoxFloat()
	{
		this.Max = 1.0;
		this.Min = 0.0;
	}

	// Token: 0x060072F7 RID: 29431 RVA: 0x002BC61A File Offset: 0x002BA81A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void BackPressed()
	{
		this.currentValue -= this.incrementSize;
		base.BackPressed();
	}

	// Token: 0x060072F8 RID: 29432 RVA: 0x002BC635 File Offset: 0x002BA835
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void ForwardPressed()
	{
		this.currentValue += this.incrementSize;
		base.ForwardPressed();
	}

	// Token: 0x060072F9 RID: 29433 RVA: 0x002BC650 File Offset: 0x002BA850
	[XuiXmlAttribute("value_min", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeValueMin(string _value)
	{
		if (!_value.EqualsCaseInsensitive("@def"))
		{
			this.Min = StringParsers.ParseDouble(_value, 0, -1, NumberStyles.Any);
		}
	}

	// Token: 0x060072FA RID: 29434 RVA: 0x002BC672 File Offset: 0x002BA872
	[XuiXmlAttribute("value_max", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeValueMax(string _value)
	{
		if (!_value.EqualsCaseInsensitive("@def"))
		{
			this.Max = StringParsers.ParseDouble(_value, 0, -1, NumberStyles.Any);
		}
	}

	// Token: 0x060072FB RID: 29435 RVA: 0x002BC694 File Offset: 0x002BA894
	[XuiXmlAttribute("value_increment", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeValueIncrement(string _value)
	{
		if (!_value.EqualsCaseInsensitive("@def"))
		{
			this.incrementSize = StringParsers.ParseDouble(_value, 0, -1, NumberStyles.Any);
		}
	}

	// Token: 0x060072FC RID: 29436 RVA: 0x002BC6B8 File Offset: 0x002BA8B8
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

	// Token: 0x0400561A RID: 22042
	[PublicizedFrom(EAccessModifier.Private)]
	public double incrementSize = 1.0;
}
