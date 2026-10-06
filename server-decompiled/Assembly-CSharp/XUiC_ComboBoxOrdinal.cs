using System;
using UnityEngine.Scripting;

// Token: 0x02000E8B RID: 3723
[Preserve]
public abstract class XUiC_ComboBoxOrdinal<TValue> : XUiC_ComboBox<TValue> where TValue : struct, IEquatable<TValue>, IComparable<TValue>, IFormattable, IConvertible
{
	// Token: 0x17000D52 RID: 3410
	// (get) Token: 0x060072C7 RID: 29383 RVA: 0x002BF533 File Offset: 0x002BD733
	// (set) Token: 0x060072C8 RID: 29384 RVA: 0x002BF53B File Offset: 0x002BD73B
	[XuiXmlAttribute("format_string", false)]
	public string FormatString { get; set; }

	// Token: 0x17000D53 RID: 3411
	// (get) Token: 0x060072C9 RID: 29385 RVA: 0x002BEF88 File Offset: 0x002BD188
	// (set) Token: 0x060072CA RID: 29386 RVA: 0x002BF544 File Offset: 0x002BD744
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

	// Token: 0x17000D54 RID: 3412
	// (get) Token: 0x060072CB RID: 29387 RVA: 0x0010087D File Offset: 0x000FEA7D
	public override int IndexElementCount
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return -1;
		}
	}

	// Token: 0x17000D55 RID: 3413
	// (get) Token: 0x060072CC RID: 29388 RVA: 0x0010087D File Offset: 0x000FEA7D
	public override int IndexMarkerIndex
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return -1;
		}
	}

	// Token: 0x17000D56 RID: 3414
	// (get) Token: 0x060072CD RID: 29389 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsNumber
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return true;
		}
	}

	// Token: 0x060072CE RID: 29390 RVA: 0x002BF5B8 File Offset: 0x002BD7B8
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

	// Token: 0x060072CF RID: 29391 RVA: 0x002BF619 File Offset: 0x002BD819
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateLabel()
	{
		base.ValueText = ((this.CustomValueFormatter != null) ? this.CustomValueFormatter(this.currentValue) : this.currentValue.ToString(this.FormatString, Utils.StandardCulture));
	}

	// Token: 0x060072D0 RID: 29392 RVA: 0x002BF658 File Offset: 0x002BD858
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isDifferentValue(TValue _oldVal, TValue _currentValue)
	{
		return !_oldVal.Equals(_currentValue);
	}

	// Token: 0x060072D1 RID: 29393 RVA: 0x002BF66B File Offset: 0x002BD86B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void BackPressed()
	{
		if (this.currentValue.CompareTo(this.Min) < 0)
		{
			this.currentValue = (this.Wrap ? this.Max : this.Min);
		}
	}

	// Token: 0x060072D2 RID: 29394 RVA: 0x002BF6A3 File Offset: 0x002BD8A3
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void ForwardPressed()
	{
		if (this.currentValue.CompareTo(this.Max) > 0)
		{
			this.currentValue = (this.Wrap ? this.Min : this.Max);
		}
	}

	// Token: 0x060072D3 RID: 29395 RVA: 0x002BF6DB File Offset: 0x002BD8DB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isMax()
	{
		return this.currentValue.CompareTo(this.Max) == 0;
	}

	// Token: 0x060072D4 RID: 29396 RVA: 0x002BF6F7 File Offset: 0x002BD8F7
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isMin()
	{
		return this.currentValue.CompareTo(this.Min) == 0;
	}

	// Token: 0x060072D5 RID: 29397 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isEmpty()
	{
		return false;
	}

	// Token: 0x060072D6 RID: 29398 RVA: 0x002BF713 File Offset: 0x002BD913
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_ComboBoxOrdinal()
	{
	}

	// Token: 0x0400564E RID: 22094
	public TValue Min;

	// Token: 0x0400564F RID: 22095
	public TValue Max;
}
