using System;
using UnityEngine.Scripting;

// Token: 0x02000E7F RID: 3711
[Preserve]
public class XUiC_ComboBoxBool : XUiC_ComboBox<bool>
{
	// Token: 0x17000D44 RID: 3396
	// (get) Token: 0x060072FD RID: 29437 RVA: 0x002BC76B File Offset: 0x002BA96B
	// (set) Token: 0x060072FE RID: 29438 RVA: 0x002BC773 File Offset: 0x002BA973
	[XuiXmlAttribute("localization_prefix", false)]
	public string LocalizationPrefix { get; set; }

	// Token: 0x17000D45 RID: 3397
	// (get) Token: 0x060072FF RID: 29439 RVA: 0x002BC77C File Offset: 0x002BA97C
	// (set) Token: 0x06007300 RID: 29440 RVA: 0x002BC784 File Offset: 0x002BA984
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

	// Token: 0x17000D46 RID: 3398
	// (get) Token: 0x06007301 RID: 29441 RVA: 0x002BC7A3 File Offset: 0x002BA9A3
	// (set) Token: 0x06007302 RID: 29442 RVA: 0x002BC7B2 File Offset: 0x002BA9B2
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

	// Token: 0x17000D47 RID: 3399
	// (get) Token: 0x06007303 RID: 29443 RVA: 0x002BC7D2 File Offset: 0x002BA9D2
	// (set) Token: 0x06007304 RID: 29444 RVA: 0x002BC7E1 File Offset: 0x002BA9E1
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

	// Token: 0x17000D48 RID: 3400
	// (get) Token: 0x06007305 RID: 29445 RVA: 0x002BC7EE File Offset: 0x002BA9EE
	// (set) Token: 0x06007306 RID: 29446 RVA: 0x000027FC File Offset: 0x000009FC
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

	// Token: 0x17000D49 RID: 3401
	// (get) Token: 0x06007307 RID: 29447 RVA: 0x00038DD7 File Offset: 0x00036FD7
	// (set) Token: 0x06007308 RID: 29448 RVA: 0x000027FC File Offset: 0x000009FC
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

	// Token: 0x17000D4A RID: 3402
	// (get) Token: 0x06007309 RID: 29449 RVA: 0x0002F168 File Offset: 0x0002D368
	public override int IndexElementCount
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return 2;
		}
	}

	// Token: 0x17000D4B RID: 3403
	// (get) Token: 0x0600730A RID: 29450 RVA: 0x002BC7F2 File Offset: 0x002BA9F2
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

	// Token: 0x0600730B RID: 29451 RVA: 0x002BC800 File Offset: 0x002BAA00
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

	// Token: 0x0600730C RID: 29452 RVA: 0x002BC87C File Offset: 0x002BAA7C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isDifferentValue(bool _oldVal, bool _currentValue)
	{
		return _oldVal != _currentValue;
	}

	// Token: 0x0600730D RID: 29453 RVA: 0x002BC885 File Offset: 0x002BAA85
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void BackPressed()
	{
		this.currentValue = !this.currentValue;
	}

	// Token: 0x0600730E RID: 29454 RVA: 0x002BC885 File Offset: 0x002BAA85
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void ForwardPressed()
	{
		this.currentValue = !this.currentValue;
	}

	// Token: 0x0600730F RID: 29455 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isMax()
	{
		return false;
	}

	// Token: 0x06007310 RID: 29456 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isMin()
	{
		return false;
	}

	// Token: 0x06007311 RID: 29457 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isEmpty()
	{
		return false;
	}

	// Token: 0x06007312 RID: 29458 RVA: 0x002BB667 File Offset: 0x002B9867
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void incrementalChangeValue(double _value)
	{
		this.TryPageUp();
	}
}
