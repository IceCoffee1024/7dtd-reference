using System;
using UnityEngine.Scripting;

// Token: 0x02000E7C RID: 3708
[Preserve]
public abstract class XUiC_ComboBoxOrdinal<TValue> : XUiC_ComboBox<TValue> where TValue : struct, IEquatable<TValue>, IComparable<TValue>, IFormattable, IConvertible
{
	// Token: 0x17000D37 RID: 3383
	// (get) Token: 0x060072CF RID: 29391 RVA: 0x002BC183 File Offset: 0x002BA383
	// (set) Token: 0x060072D0 RID: 29392 RVA: 0x002BC18B File Offset: 0x002BA38B
	[XuiXmlAttribute("format_string", false)]
	public string FormatString { get; set; }

	// Token: 0x17000D38 RID: 3384
	// (get) Token: 0x060072D1 RID: 29393 RVA: 0x002BBBD8 File Offset: 0x002B9DD8
	// (set) Token: 0x060072D2 RID: 29394 RVA: 0x002BC194 File Offset: 0x002BA394
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

	// Token: 0x17000D39 RID: 3385
	// (get) Token: 0x060072D3 RID: 29395 RVA: 0x000FFF9D File Offset: 0x000FE19D
	public override int IndexElementCount
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return -1;
		}
	}

	// Token: 0x17000D3A RID: 3386
	// (get) Token: 0x060072D4 RID: 29396 RVA: 0x000FFF9D File Offset: 0x000FE19D
	public override int IndexMarkerIndex
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return -1;
		}
	}

	// Token: 0x17000D3B RID: 3387
	// (get) Token: 0x060072D5 RID: 29397 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsNumber
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return true;
		}
	}

	// Token: 0x060072D6 RID: 29398 RVA: 0x002BC208 File Offset: 0x002BA408
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

	// Token: 0x060072D7 RID: 29399 RVA: 0x002BC269 File Offset: 0x002BA469
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateLabel()
	{
		base.ValueText = ((this.CustomValueFormatter != null) ? this.CustomValueFormatter(this.currentValue) : this.currentValue.ToString(this.FormatString, Utils.StandardCulture));
	}

	// Token: 0x060072D8 RID: 29400 RVA: 0x002BC2A8 File Offset: 0x002BA4A8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isDifferentValue(TValue _oldVal, TValue _currentValue)
	{
		return !_oldVal.Equals(_currentValue);
	}

	// Token: 0x060072D9 RID: 29401 RVA: 0x002BC2BB File Offset: 0x002BA4BB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void BackPressed()
	{
		if (this.currentValue.CompareTo(this.Min) < 0)
		{
			this.currentValue = (this.Wrap ? this.Max : this.Min);
		}
	}

	// Token: 0x060072DA RID: 29402 RVA: 0x002BC2F3 File Offset: 0x002BA4F3
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void ForwardPressed()
	{
		if (this.currentValue.CompareTo(this.Max) > 0)
		{
			this.currentValue = (this.Wrap ? this.Min : this.Max);
		}
	}

	// Token: 0x060072DB RID: 29403 RVA: 0x002BC32B File Offset: 0x002BA52B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isMax()
	{
		return this.currentValue.CompareTo(this.Max) == 0;
	}

	// Token: 0x060072DC RID: 29404 RVA: 0x002BC347 File Offset: 0x002BA547
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isMin()
	{
		return this.currentValue.CompareTo(this.Min) == 0;
	}

	// Token: 0x060072DD RID: 29405 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isEmpty()
	{
		return false;
	}

	// Token: 0x060072DE RID: 29406 RVA: 0x002BC363 File Offset: 0x002BA563
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_ComboBoxOrdinal()
	{
	}

	// Token: 0x04005617 RID: 22039
	public TValue Min;

	// Token: 0x04005618 RID: 22040
	public TValue Max;
}
