using System;
using UnityEngine.Scripting;

// Token: 0x02000E6A RID: 3690
[Preserve]
public class XUiC_ComboBoxBool : XUiC_ComboBox<bool>
{
	// Token: 0x17000D2B RID: 3371
	// (get) Token: 0x0600725D RID: 29277 RVA: 0x002B9247 File Offset: 0x002B7447
	// (set) Token: 0x0600725E RID: 29278 RVA: 0x002B924F File Offset: 0x002B744F
	[XuiXmlAttribute("localization_prefix", false)]
	public string LocalizationPrefix { get; set; }

	// Token: 0x17000D2C RID: 3372
	// (get) Token: 0x0600725F RID: 29279 RVA: 0x002B9258 File Offset: 0x002B7458
	// (set) Token: 0x06007260 RID: 29280 RVA: 0x002B9260 File Offset: 0x002B7460
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

	// Token: 0x17000D2D RID: 3373
	// (get) Token: 0x06007261 RID: 29281 RVA: 0x002B927F File Offset: 0x002B747F
	// (set) Token: 0x06007262 RID: 29282 RVA: 0x002B928E File Offset: 0x002B748E
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

	// Token: 0x17000D2E RID: 3374
	// (get) Token: 0x06007263 RID: 29283 RVA: 0x002B92AE File Offset: 0x002B74AE
	// (set) Token: 0x06007264 RID: 29284 RVA: 0x002B92BD File Offset: 0x002B74BD
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

	// Token: 0x17000D2F RID: 3375
	// (get) Token: 0x06007265 RID: 29285 RVA: 0x002B92CA File Offset: 0x002B74CA
	// (set) Token: 0x06007266 RID: 29286 RVA: 0x000027FC File Offset: 0x000009FC
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

	// Token: 0x17000D30 RID: 3376
	// (get) Token: 0x06007267 RID: 29287 RVA: 0x00038B77 File Offset: 0x00036D77
	// (set) Token: 0x06007268 RID: 29288 RVA: 0x000027FC File Offset: 0x000009FC
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

	// Token: 0x17000D31 RID: 3377
	// (get) Token: 0x06007269 RID: 29289 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override int IndexElementCount
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return 2;
		}
	}

	// Token: 0x17000D32 RID: 3378
	// (get) Token: 0x0600726A RID: 29290 RVA: 0x002B92CE File Offset: 0x002B74CE
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

	// Token: 0x0600726B RID: 29291 RVA: 0x002B92DC File Offset: 0x002B74DC
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

	// Token: 0x0600726C RID: 29292 RVA: 0x002B9358 File Offset: 0x002B7558
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isDifferentValue(bool _oldVal, bool _currentValue)
	{
		return _oldVal != _currentValue;
	}

	// Token: 0x0600726D RID: 29293 RVA: 0x002B9361 File Offset: 0x002B7561
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void BackPressed()
	{
		this.currentValue = !this.currentValue;
	}

	// Token: 0x0600726E RID: 29294 RVA: 0x002B9361 File Offset: 0x002B7561
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void ForwardPressed()
	{
		this.currentValue = !this.currentValue;
	}

	// Token: 0x0600726F RID: 29295 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isMax()
	{
		return false;
	}

	// Token: 0x06007270 RID: 29296 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isMin()
	{
		return false;
	}

	// Token: 0x06007271 RID: 29297 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isEmpty()
	{
		return false;
	}

	// Token: 0x06007272 RID: 29298 RVA: 0x002B8143 File Offset: 0x002B6343
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void incrementalChangeValue(double _value)
	{
		this.TryPageUp();
	}
}
