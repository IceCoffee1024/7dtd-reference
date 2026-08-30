using System;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x02000E68 RID: 3688
[Preserve]
public class XUiC_ComboBoxInt : XUiC_ComboBoxOrdinal<long>
{
	// Token: 0x17000D23 RID: 3363
	// (get) Token: 0x0600723F RID: 29247 RVA: 0x002B8E47 File Offset: 0x002B7047
	// (set) Token: 0x06007240 RID: 29248 RVA: 0x002B8E4F File Offset: 0x002B704F
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

	// Token: 0x17000D24 RID: 3364
	// (get) Token: 0x06007241 RID: 29249 RVA: 0x002B8E58 File Offset: 0x002B7058
	// (set) Token: 0x06007242 RID: 29250 RVA: 0x002B8E60 File Offset: 0x002B7060
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

	// Token: 0x17000D25 RID: 3365
	// (get) Token: 0x06007243 RID: 29251 RVA: 0x002B8E69 File Offset: 0x002B7069
	// (set) Token: 0x06007244 RID: 29252 RVA: 0x002B8E71 File Offset: 0x002B7071
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

	// Token: 0x17000D26 RID: 3366
	// (get) Token: 0x06007245 RID: 29253 RVA: 0x002B8E7A File Offset: 0x002B707A
	// (set) Token: 0x06007246 RID: 29254 RVA: 0x002B8E99 File Offset: 0x002B7099
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

	// Token: 0x06007247 RID: 29255 RVA: 0x002B8EB9 File Offset: 0x002B70B9
	public XUiC_ComboBoxInt()
	{
		this.Max = long.MaxValue;
		this.Min = long.MinValue;
	}

	// Token: 0x06007248 RID: 29256 RVA: 0x002B8EE7 File Offset: 0x002B70E7
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void BackPressed()
	{
		this.currentValue -= this.incrementSize;
		base.BackPressed();
	}

	// Token: 0x06007249 RID: 29257 RVA: 0x002B8F02 File Offset: 0x002B7102
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void ForwardPressed()
	{
		this.currentValue += this.incrementSize;
		base.ForwardPressed();
	}

	// Token: 0x0600724A RID: 29258 RVA: 0x002B8F1D File Offset: 0x002B711D
	[XuiXmlAttribute("value_min", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeValueMin(string _value)
	{
		if (!_value.EqualsCaseInsensitive("@def"))
		{
			this.Min = StringParsers.ParseSInt64(_value, 0, -1, NumberStyles.Integer);
		}
	}

	// Token: 0x0600724B RID: 29259 RVA: 0x002B8F3B File Offset: 0x002B713B
	[XuiXmlAttribute("value_max", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeValueMax(string _value)
	{
		if (!_value.EqualsCaseInsensitive("@def"))
		{
			this.Max = StringParsers.ParseSInt64(_value, 0, -1, NumberStyles.Integer);
		}
	}

	// Token: 0x0600724C RID: 29260 RVA: 0x002B8F59 File Offset: 0x002B7159
	[XuiXmlAttribute("value_increment", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeValueIncrement(string _value)
	{
		if (!_value.EqualsCaseInsensitive("@def"))
		{
			this.incrementSize = StringParsers.ParseSInt64(_value, 0, -1, NumberStyles.Integer);
		}
	}

	// Token: 0x0600724D RID: 29261 RVA: 0x002B8F78 File Offset: 0x002B7178
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

	// Token: 0x04005575 RID: 21877
	[PublicizedFrom(EAccessModifier.Private)]
	public long incrementSize = 1L;
}
