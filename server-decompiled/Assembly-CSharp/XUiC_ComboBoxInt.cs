using System;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x02000E7D RID: 3709
[Preserve]
public class XUiC_ComboBoxInt : XUiC_ComboBoxOrdinal<long>
{
	// Token: 0x17000D3C RID: 3388
	// (get) Token: 0x060072DF RID: 29407 RVA: 0x002BC36B File Offset: 0x002BA56B
	// (set) Token: 0x060072E0 RID: 29408 RVA: 0x002BC373 File Offset: 0x002BA573
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

	// Token: 0x17000D3D RID: 3389
	// (get) Token: 0x060072E1 RID: 29409 RVA: 0x002BC37C File Offset: 0x002BA57C
	// (set) Token: 0x060072E2 RID: 29410 RVA: 0x002BC384 File Offset: 0x002BA584
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

	// Token: 0x17000D3E RID: 3390
	// (get) Token: 0x060072E3 RID: 29411 RVA: 0x002BC38D File Offset: 0x002BA58D
	// (set) Token: 0x060072E4 RID: 29412 RVA: 0x002BC395 File Offset: 0x002BA595
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

	// Token: 0x17000D3F RID: 3391
	// (get) Token: 0x060072E5 RID: 29413 RVA: 0x002BC39E File Offset: 0x002BA59E
	// (set) Token: 0x060072E6 RID: 29414 RVA: 0x002BC3BD File Offset: 0x002BA5BD
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

	// Token: 0x060072E7 RID: 29415 RVA: 0x002BC3DD File Offset: 0x002BA5DD
	public XUiC_ComboBoxInt()
	{
		this.Max = long.MaxValue;
		this.Min = long.MinValue;
	}

	// Token: 0x060072E8 RID: 29416 RVA: 0x002BC40B File Offset: 0x002BA60B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void BackPressed()
	{
		this.currentValue -= this.incrementSize;
		base.BackPressed();
	}

	// Token: 0x060072E9 RID: 29417 RVA: 0x002BC426 File Offset: 0x002BA626
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void ForwardPressed()
	{
		this.currentValue += this.incrementSize;
		base.ForwardPressed();
	}

	// Token: 0x060072EA RID: 29418 RVA: 0x002BC441 File Offset: 0x002BA641
	[XuiXmlAttribute("value_min", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeValueMin(string _value)
	{
		if (!_value.EqualsCaseInsensitive("@def"))
		{
			this.Min = StringParsers.ParseSInt64(_value, 0, -1, NumberStyles.Integer);
		}
	}

	// Token: 0x060072EB RID: 29419 RVA: 0x002BC45F File Offset: 0x002BA65F
	[XuiXmlAttribute("value_max", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeValueMax(string _value)
	{
		if (!_value.EqualsCaseInsensitive("@def"))
		{
			this.Max = StringParsers.ParseSInt64(_value, 0, -1, NumberStyles.Integer);
		}
	}

	// Token: 0x060072EC RID: 29420 RVA: 0x002BC47D File Offset: 0x002BA67D
	[XuiXmlAttribute("value_increment", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeValueIncrement(string _value)
	{
		if (!_value.EqualsCaseInsensitive("@def"))
		{
			this.incrementSize = StringParsers.ParseSInt64(_value, 0, -1, NumberStyles.Integer);
		}
	}

	// Token: 0x060072ED RID: 29421 RVA: 0x002BC49C File Offset: 0x002BA69C
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

	// Token: 0x04005619 RID: 22041
	[PublicizedFrom(EAccessModifier.Private)]
	public long incrementSize = 1L;
}
