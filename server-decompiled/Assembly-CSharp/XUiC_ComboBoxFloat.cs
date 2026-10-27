using System;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x02000E98 RID: 3736
[Preserve]
public class XUiC_ComboBoxFloat : XUiC_ComboBoxOrdinal<double>
{
	// Token: 0x17000D61 RID: 3425
	// (get) Token: 0x0600730B RID: 29451 RVA: 0x002C0E93 File Offset: 0x002BF093
	// (set) Token: 0x0600730C RID: 29452 RVA: 0x002C0E9B File Offset: 0x002BF09B
	public override long ValueGeneric { get; set; }

	// Token: 0x17000D62 RID: 3426
	// (get) Token: 0x0600730D RID: 29453 RVA: 0x002C0EA4 File Offset: 0x002BF0A4
	// (set) Token: 0x0600730E RID: 29454 RVA: 0x002C0EAC File Offset: 0x002BF0AC
	public override long ValueMinGeneric { get; set; }

	// Token: 0x17000D63 RID: 3427
	// (get) Token: 0x0600730F RID: 29455 RVA: 0x002C0EB5 File Offset: 0x002BF0B5
	// (set) Token: 0x06007310 RID: 29456 RVA: 0x002C0EBD File Offset: 0x002BF0BD
	public override long ValueMaxGeneric { get; set; }

	// Token: 0x17000D64 RID: 3428
	// (get) Token: 0x06007311 RID: 29457 RVA: 0x002C0EC6 File Offset: 0x002BF0C6
	// (set) Token: 0x06007312 RID: 29458 RVA: 0x002C0EE3 File Offset: 0x002BF0E3
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

	// Token: 0x06007313 RID: 29459 RVA: 0x002C0F01 File Offset: 0x002BF101
	public XUiC_ComboBoxFloat()
	{
		this.Max = 1.0;
		this.Min = 0.0;
	}

	// Token: 0x06007314 RID: 29460 RVA: 0x002C0F36 File Offset: 0x002BF136
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void BackPressed()
	{
		this.currentValue -= this.incrementSize;
		base.BackPressed();
	}

	// Token: 0x06007315 RID: 29461 RVA: 0x002C0F51 File Offset: 0x002BF151
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void ForwardPressed()
	{
		this.currentValue += this.incrementSize;
		base.ForwardPressed();
	}

	// Token: 0x06007316 RID: 29462 RVA: 0x002C0F6C File Offset: 0x002BF16C
	[XuiXmlAttribute("value_min", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeValueMin(string _value)
	{
		if (!_value.EqualsCaseInsensitive("@def"))
		{
			this.Min = StringParsers.ParseDouble(_value, 0, -1, NumberStyles.Any);
		}
	}

	// Token: 0x06007317 RID: 29463 RVA: 0x002C0F8E File Offset: 0x002BF18E
	[XuiXmlAttribute("value_max", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeValueMax(string _value)
	{
		if (!_value.EqualsCaseInsensitive("@def"))
		{
			this.Max = StringParsers.ParseDouble(_value, 0, -1, NumberStyles.Any);
		}
	}

	// Token: 0x06007318 RID: 29464 RVA: 0x002C0FB0 File Offset: 0x002BF1B0
	[XuiXmlAttribute("value_increment", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeValueIncrement(string _value)
	{
		if (!_value.EqualsCaseInsensitive("@def"))
		{
			this.incrementSize = StringParsers.ParseDouble(_value, 0, -1, NumberStyles.Any);
		}
	}

	// Token: 0x06007319 RID: 29465 RVA: 0x002C0FD4 File Offset: 0x002BF1D4
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

	// Token: 0x04005676 RID: 22134
	[PublicizedFrom(EAccessModifier.Private)]
	public double incrementSize = 1.0;
}
