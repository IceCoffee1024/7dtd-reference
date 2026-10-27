using System;
using UnityEngine.Scripting;

// Token: 0x02000E99 RID: 3737
[Preserve]
public class XUiC_ComboBoxBool : XUiC_ComboBox<bool>
{
	// Token: 0x17000D65 RID: 3429
	// (get) Token: 0x0600731A RID: 29466 RVA: 0x002C1087 File Offset: 0x002BF287
	// (set) Token: 0x0600731B RID: 29467 RVA: 0x002C108F File Offset: 0x002BF28F
	[XuiXmlAttribute("localization_prefix", false)]
	public string LocalizationPrefix { get; set; }

	// Token: 0x17000D66 RID: 3430
	// (get) Token: 0x0600731C RID: 29468 RVA: 0x002C1098 File Offset: 0x002BF298
	// (set) Token: 0x0600731D RID: 29469 RVA: 0x002C10A0 File Offset: 0x002BF2A0
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

	// Token: 0x17000D67 RID: 3431
	// (get) Token: 0x0600731E RID: 29470 RVA: 0x002C10BF File Offset: 0x002BF2BF
	// (set) Token: 0x0600731F RID: 29471 RVA: 0x002C10CE File Offset: 0x002BF2CE
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

	// Token: 0x17000D68 RID: 3432
	// (get) Token: 0x06007320 RID: 29472 RVA: 0x002C10EE File Offset: 0x002BF2EE
	// (set) Token: 0x06007321 RID: 29473 RVA: 0x002C10FD File Offset: 0x002BF2FD
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

	// Token: 0x17000D69 RID: 3433
	// (get) Token: 0x06007322 RID: 29474 RVA: 0x002C110A File Offset: 0x002BF30A
	// (set) Token: 0x06007323 RID: 29475 RVA: 0x000027FC File Offset: 0x000009FC
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

	// Token: 0x17000D6A RID: 3434
	// (get) Token: 0x06007324 RID: 29476 RVA: 0x00038E87 File Offset: 0x00037087
	// (set) Token: 0x06007325 RID: 29477 RVA: 0x000027FC File Offset: 0x000009FC
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

	// Token: 0x17000D6B RID: 3435
	// (get) Token: 0x06007326 RID: 29478 RVA: 0x0002F260 File Offset: 0x0002D460
	public override int IndexElementCount
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return 2;
		}
	}

	// Token: 0x17000D6C RID: 3436
	// (get) Token: 0x06007327 RID: 29479 RVA: 0x002C110E File Offset: 0x002BF30E
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

	// Token: 0x06007328 RID: 29480 RVA: 0x002C111C File Offset: 0x002BF31C
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

	// Token: 0x06007329 RID: 29481 RVA: 0x002C1198 File Offset: 0x002BF398
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isDifferentValue(bool _oldVal, bool _currentValue)
	{
		return _oldVal != _currentValue;
	}

	// Token: 0x0600732A RID: 29482 RVA: 0x002C11A1 File Offset: 0x002BF3A1
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void BackPressed()
	{
		this.currentValue = !this.currentValue;
	}

	// Token: 0x0600732B RID: 29483 RVA: 0x002C11A1 File Offset: 0x002BF3A1
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void ForwardPressed()
	{
		this.currentValue = !this.currentValue;
	}

	// Token: 0x0600732C RID: 29484 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isMax()
	{
		return false;
	}

	// Token: 0x0600732D RID: 29485 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isMin()
	{
		return false;
	}

	// Token: 0x0600732E RID: 29486 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isEmpty()
	{
		return false;
	}

	// Token: 0x0600732F RID: 29487 RVA: 0x002BFF83 File Offset: 0x002BE183
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void incrementalChangeValue(double _value)
	{
		this.TryPageUp();
	}
}
