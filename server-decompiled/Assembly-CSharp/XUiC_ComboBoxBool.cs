using System;
using UnityEngine.Scripting;

// Token: 0x02000E8E RID: 3726
[Preserve]
public class XUiC_ComboBoxBool : XUiC_ComboBox<bool>
{
	// Token: 0x17000D5F RID: 3423
	// (get) Token: 0x060072F5 RID: 29429 RVA: 0x002BFB1B File Offset: 0x002BDD1B
	// (set) Token: 0x060072F6 RID: 29430 RVA: 0x002BFB23 File Offset: 0x002BDD23
	[XuiXmlAttribute("localization_prefix", false)]
	public string LocalizationPrefix { get; set; }

	// Token: 0x17000D60 RID: 3424
	// (get) Token: 0x060072F7 RID: 29431 RVA: 0x002BFB2C File Offset: 0x002BDD2C
	// (set) Token: 0x060072F8 RID: 29432 RVA: 0x002BFB34 File Offset: 0x002BDD34
	public override bool Value
	{
		get
		{
			return this.currentValue;
		}
		set
		{
			if (this.currentValue != value)
			{
				this.currentValue = value;
				this.IsDirty = true;
				this.UpdateLabel();
			}
		}
	}

	// Token: 0x17000D61 RID: 3425
	// (get) Token: 0x060072F9 RID: 29433 RVA: 0x002BFB53 File Offset: 0x002BDD53
	// (set) Token: 0x060072FA RID: 29434 RVA: 0x002BFB62 File Offset: 0x002BDD62
	public override double RelativeValue
	{
		get
		{
			return (double)(this.Value ? 1 : 0);
		}
		set
		{
			if (!this.UsesIndexMarkers)
			{
				return;
			}
			this.Value = value >= 0.5;
		}
	}

	// Token: 0x17000D62 RID: 3426
	// (get) Token: 0x060072FB RID: 29435 RVA: 0x002BFB82 File Offset: 0x002BDD82
	// (set) Token: 0x060072FC RID: 29436 RVA: 0x002BFB91 File Offset: 0x002BDD91
	public override long ValueGeneric
	{
		get
		{
			return this.Value ? 1L : 0L;
		}
		set
		{
			this.Value = value != 0L;
		}
	}

	// Token: 0x17000D63 RID: 3427
	// (get) Token: 0x060072FD RID: 29437 RVA: 0x002BFB9E File Offset: 0x002BDD9E
	// (set) Token: 0x060072FE RID: 29438 RVA: 0x000027FC File Offset: 0x000009FC
	public override long ValueMinGeneric
	{
		get
		{
			return 0L;
		}
		set
		{
		}
	}

	// Token: 0x17000D64 RID: 3428
	// (get) Token: 0x060072FF RID: 29439 RVA: 0x00038EC7 File Offset: 0x000370C7
	// (set) Token: 0x06007300 RID: 29440 RVA: 0x000027FC File Offset: 0x000009FC
	public override long ValueMaxGeneric
	{
		get
		{
			return 1L;
		}
		set
		{
		}
	}

	// Token: 0x17000D65 RID: 3429
	// (get) Token: 0x06007301 RID: 29441 RVA: 0x0002F260 File Offset: 0x0002D460
	public override int IndexElementCount
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return 2;
		}
	}

	// Token: 0x17000D66 RID: 3430
	// (get) Token: 0x06007302 RID: 29442 RVA: 0x002BFBA2 File Offset: 0x002BDDA2
	public override int IndexMarkerIndex
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (!this.currentValue)
			{
				return 0;
			}
			return 1;
		}
	}

	// Token: 0x06007303 RID: 29443 RVA: 0x002BFBB0 File Offset: 0x002BDDB0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateLabel()
	{
		if (this.CustomValueFormatter != null)
		{
			base.ValueText = this.CustomValueFormatter(this.currentValue);
			return;
		}
		base.ValueText = ((!string.IsNullOrEmpty(this.LocalizationPrefix)) ? Localization.Get(this.LocalizationPrefix + (this.currentValue ? "On" : "Off"), false, null) : (this.currentValue ? "Yes" : "No"));
	}

	// Token: 0x06007304 RID: 29444 RVA: 0x002BFC2C File Offset: 0x002BDE2C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isDifferentValue(bool _oldVal, bool _currentValue)
	{
		return _oldVal != _currentValue;
	}

	// Token: 0x06007305 RID: 29445 RVA: 0x002BFC35 File Offset: 0x002BDE35
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void BackPressed()
	{
		this.currentValue = !this.currentValue;
	}

	// Token: 0x06007306 RID: 29446 RVA: 0x002BFC35 File Offset: 0x002BDE35
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void ForwardPressed()
	{
		this.currentValue = !this.currentValue;
	}

	// Token: 0x06007307 RID: 29447 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isMax()
	{
		return false;
	}

	// Token: 0x06007308 RID: 29448 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isMin()
	{
		return false;
	}

	// Token: 0x06007309 RID: 29449 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isEmpty()
	{
		return false;
	}

	// Token: 0x0600730A RID: 29450 RVA: 0x002BEA17 File Offset: 0x002BCC17
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void incrementalChangeValue(double _value)
	{
		this.TryPageUp();
	}
}
