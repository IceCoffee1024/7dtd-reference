using System;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x02000E69 RID: 3689
[Preserve]
public class XUiC_ComboBoxFloat : XUiC_ComboBoxOrdinal<double>
{
	// Token: 0x17000D27 RID: 3367
	// (get) Token: 0x0600724E RID: 29262 RVA: 0x002B9053 File Offset: 0x002B7253
	// (set) Token: 0x0600724F RID: 29263 RVA: 0x002B905B File Offset: 0x002B725B
	public override long ValueGeneric { get; set; }

	// Token: 0x17000D28 RID: 3368
	// (get) Token: 0x06007250 RID: 29264 RVA: 0x002B9064 File Offset: 0x002B7264
	// (set) Token: 0x06007251 RID: 29265 RVA: 0x002B906C File Offset: 0x002B726C
	public override long ValueMinGeneric { get; set; }

	// Token: 0x17000D29 RID: 3369
	// (get) Token: 0x06007252 RID: 29266 RVA: 0x002B9075 File Offset: 0x002B7275
	// (set) Token: 0x06007253 RID: 29267 RVA: 0x002B907D File Offset: 0x002B727D
	public override long ValueMaxGeneric { get; set; }

	// Token: 0x17000D2A RID: 3370
	// (get) Token: 0x06007254 RID: 29268 RVA: 0x002B9086 File Offset: 0x002B7286
	// (set) Token: 0x06007255 RID: 29269 RVA: 0x002B90A3 File Offset: 0x002B72A3
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

	// Token: 0x06007256 RID: 29270 RVA: 0x002B90C1 File Offset: 0x002B72C1
	public XUiC_ComboBoxFloat()
	{
		this.Max = 1.0;
		this.Min = 0.0;
	}

	// Token: 0x06007257 RID: 29271 RVA: 0x002B90F6 File Offset: 0x002B72F6
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void BackPressed()
	{
		this.currentValue -= this.incrementSize;
		base.BackPressed();
	}

	// Token: 0x06007258 RID: 29272 RVA: 0x002B9111 File Offset: 0x002B7311
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void ForwardPressed()
	{
		this.currentValue += this.incrementSize;
		base.ForwardPressed();
	}

	// Token: 0x06007259 RID: 29273 RVA: 0x002B912C File Offset: 0x002B732C
	[XuiXmlAttribute("value_min", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeValueMin(string _value)
	{
		if (!_value.EqualsCaseInsensitive("@def"))
		{
			this.Min = StringParsers.ParseDouble(_value, 0, -1, NumberStyles.Any);
		}
	}

	// Token: 0x0600725A RID: 29274 RVA: 0x002B914E File Offset: 0x002B734E
	[XuiXmlAttribute("value_max", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeValueMax(string _value)
	{
		if (!_value.EqualsCaseInsensitive("@def"))
		{
			this.Max = StringParsers.ParseDouble(_value, 0, -1, NumberStyles.Any);
		}
	}

	// Token: 0x0600725B RID: 29275 RVA: 0x002B9170 File Offset: 0x002B7370
	[XuiXmlAttribute("value_increment", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeValueIncrement(string _value)
	{
		if (!_value.EqualsCaseInsensitive("@def"))
		{
			this.incrementSize = StringParsers.ParseDouble(_value, 0, -1, NumberStyles.Any);
		}
	}

	// Token: 0x0600725C RID: 29276 RVA: 0x002B9194 File Offset: 0x002B7394
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

	// Token: 0x04005576 RID: 21878
	[PublicizedFrom(EAccessModifier.Private)]
	public double incrementSize = 1.0;
}
