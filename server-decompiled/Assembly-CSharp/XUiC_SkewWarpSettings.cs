using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001052 RID: 4178
[Preserve]
public class XUiC_SkewWarpSettings : XUiC_SignWarpSettings
{
	// Token: 0x17000F8D RID: 3981
	// (get) Token: 0x06008455 RID: 33877 RVA: 0x00328778 File Offset: 0x00326978
	public override SignData.SignWarp CurrentWarp
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.currentWarp;
		}
	}

	// Token: 0x06008456 RID: 33878 RVA: 0x00328780 File Offset: 0x00326980
	public override void Init()
	{
		base.Init();
		this.cbxSkewX = (XUiC_ComboBoxFloat)base.GetChildById("cbxSkewX").GetChildById("value");
		this.cbxSkewX.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Skew X");
		};
		this.cbxSkewY = (XUiC_ComboBoxFloat)base.GetChildById("cbxSkewY").GetChildById("value");
		this.cbxSkewY.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Skew Y");
		};
		this.cbxRotation = (XUiC_ComboBoxFloat)base.GetChildById("cbxRotation").GetChildById("value");
		this.cbxRotation.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Skew Rotation");
		};
		SignData.SkewWarp skewWarp = new SignData.SkewWarp();
		base.SetDefaultValue("cbxSkewX", skewWarp.amount.x);
		base.SetDefaultValue("cbxSkewY", skewWarp.amount.y);
		base.SetDefaultValue("cbxRotation", skewWarp.rotation);
	}

	// Token: 0x06008457 RID: 33879 RVA: 0x0032888C File Offset: 0x00326A8C
	public override void SetWarp(SignData.SignWarp warp)
	{
		this.currentWarp = warp as SignData.SkewWarp;
		if (this.currentWarp == null)
		{
			this.cbxSkewX.Value = 0.0;
			this.cbxSkewY.Value = 0.0;
			this.cbxRotation.Value = 0.0;
			return;
		}
		this.cbxSkewX.Value = (double)this.currentWarp.amount.x;
		this.cbxSkewY.Value = (double)this.currentWarp.amount.y;
		this.cbxRotation.Value = (double)this.currentWarp.rotation;
	}

	// Token: 0x06008458 RID: 33880 RVA: 0x0032893C File Offset: 0x00326B3C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnValueChangedGeneric(string _changeDescription)
	{
		if (this.currentWarp == null)
		{
			return;
		}
		Action<string, bool> onPreLayerSettingsChanged = this.OnPreLayerSettingsChanged;
		if (onPreLayerSettingsChanged != null)
		{
			onPreLayerSettingsChanged(_changeDescription, false);
		}
		this.currentWarp.amount = new Vector2((float)this.cbxSkewX.Value, (float)this.cbxSkewY.Value);
		this.currentWarp.rotation = (float)this.cbxRotation.Value;
		Action onLayerSettingsChanged = this.OnLayerSettingsChanged;
		if (onLayerSettingsChanged == null)
		{
			return;
		}
		onLayerSettingsChanged();
	}

	// Token: 0x04006243 RID: 25155
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData.SkewWarp currentWarp;

	// Token: 0x04006244 RID: 25156
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxSkewX;

	// Token: 0x04006245 RID: 25157
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxSkewY;

	// Token: 0x04006246 RID: 25158
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxRotation;
}
