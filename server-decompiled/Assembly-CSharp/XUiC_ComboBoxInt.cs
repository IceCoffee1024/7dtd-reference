using System;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x02000E8C RID: 3724
[Preserve]
public class XUiC_ComboBoxInt : XUiC_ComboBoxOrdinal<long>
{
	// Token: 0x17000D57 RID: 3415
	// (get) Token: 0x060072D7 RID: 29399 RVA: 0x002BF71B File Offset: 0x002BD91B
	// (set) Token: 0x060072D8 RID: 29400 RVA: 0x002BF723 File Offset: 0x002BD923
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

	// Token: 0x17000D58 RID: 3416
	// (get) Token: 0x060072D9 RID: 29401 RVA: 0x002BF72C File Offset: 0x002BD92C
	// (set) Token: 0x060072DA RID: 29402 RVA: 0x002BF734 File Offset: 0x002BD934
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

	// Token: 0x17000D59 RID: 3417
	// (get) Token: 0x060072DB RID: 29403 RVA: 0x002BF73D File Offset: 0x002BD93D
	// (set) Token: 0x060072DC RID: 29404 RVA: 0x002BF745 File Offset: 0x002BD945
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

	// Token: 0x17000D5A RID: 3418
	// (get) Token: 0x060072DD RID: 29405 RVA: 0x002BF74E File Offset: 0x002BD94E
	// (set) Token: 0x060072DE RID: 29406 RVA: 0x002BF76D File Offset: 0x002BD96D
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

	// Token: 0x060072DF RID: 29407 RVA: 0x002BF78D File Offset: 0x002BD98D
	public XUiC_ComboBoxInt()
	{
		this.Max = long.MaxValue;
		this.Min = long.MinValue;
	}

	// Token: 0x060072E0 RID: 29408 RVA: 0x002BF7BB File Offset: 0x002BD9BB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void BackPressed()
	{
		this.currentValue -= this.incrementSize;
		base.BackPressed();
	}

	// Token: 0x060072E1 RID: 29409 RVA: 0x002BF7D6 File Offset: 0x002BD9D6
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void ForwardPressed()
	{
		this.currentValue += this.incrementSize;
		base.ForwardPressed();
	}

	// Token: 0x060072E2 RID: 29410 RVA: 0x002BF7F1 File Offset: 0x002BD9F1
	[XuiXmlAttribute("value_min", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeValueMin(string _value)
	{
		if (!_value.EqualsCaseInsensitive("@def"))
		{
			this.Min = StringParsers.ParseSInt64(_value, 0, -1, NumberStyles.Integer);
		}
	}

	// Token: 0x060072E3 RID: 29411 RVA: 0x002BF80F File Offset: 0x002BDA0F
	[XuiXmlAttribute("value_max", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeValueMax(string _value)
	{
		if (!_value.EqualsCaseInsensitive("@def"))
		{
			this.Max = StringParsers.ParseSInt64(_value, 0, -1, NumberStyles.Integer);
		}
	}

	// Token: 0x060072E4 RID: 29412 RVA: 0x002BF82D File Offset: 0x002BDA2D
	[XuiXmlAttribute("value_increment", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeValueIncrement(string _value)
	{
		if (!_value.EqualsCaseInsensitive("@def"))
		{
			this.incrementSize = StringParsers.ParseSInt64(_value, 0, -1, NumberStyles.Integer);
		}
	}

	// Token: 0x060072E5 RID: 29413 RVA: 0x002BF84C File Offset: 0x002BDA4C
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

	// Token: 0x04005650 RID: 22096
	[PublicizedFrom(EAccessModifier.Private)]
	public long incrementSize = 1L;
}
