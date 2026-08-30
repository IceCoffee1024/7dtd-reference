using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001049 RID: 4169
[Preserve]
public class XUiC_StretchWarpSettings : XUiC_SignWarpSettings
{
	// Token: 0x17000F66 RID: 3942
	// (get) Token: 0x06008460 RID: 33888 RVA: 0x00325E39 File Offset: 0x00324039
	public override SignData.SignWarp CurrentWarp
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.currentWarp;
		}
	}

	// Token: 0x06008461 RID: 33889 RVA: 0x00325E44 File Offset: 0x00324044
	public override void Init()
	{
		base.Init();
		this.cbxOffsetX = (XUiC_ComboBoxFloat)base.GetChildById("cbxOffsetX").GetChildById("value");
		this.cbxOffsetX.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Stretch Offset X");
		};
		this.cbxOffsetY = (XUiC_ComboBoxFloat)base.GetChildById("cbxOffsetY").GetChildById("value");
		this.cbxOffsetY.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Stretch Offset Y");
		};
		this.cbxRotation = (XUiC_ComboBoxFloat)base.GetChildById("cbxRotation").GetChildById("value");
		this.cbxRotation.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Stretch Rotation");
		};
		this.cbxDistance = (XUiC_ComboBoxFloat)base.GetChildById("cbxDistance").GetChildById("value");
		this.cbxDistance.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Stretch Distance");
		};
		this.cbxWidth = (XUiC_ComboBoxFloat)base.GetChildById("cbxWidth").GetChildById("value");
		this.cbxWidth.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Stretch Width");
		};
		this.cbxExponent = (XUiC_ComboBoxFloat)base.GetChildById("cbxExponent").GetChildById("value");
		this.cbxExponent.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Stretch Exponent");
		};
		SignData.StretchWarp stretchWarp = new SignData.StretchWarp();
		base.SetDefaultValue("cbxOffsetX", stretchWarp.offset.x);
		base.SetDefaultValue("cbxOffsetY", stretchWarp.offset.y);
		base.SetDefaultValue("cbxRotation", stretchWarp.rotation);
		base.SetDefaultValue("cbxDistance", stretchWarp.distance);
		base.SetDefaultValue("cbxWidth", stretchWarp.width);
		base.SetDefaultValue("cbxExponent", stretchWarp.exponent);
	}

	// Token: 0x06008462 RID: 33890 RVA: 0x00326038 File Offset: 0x00324238
	public override void SetWarp(SignData.SignWarp warp)
	{
		this.currentWarp = warp as SignData.StretchWarp;
		if (this.currentWarp == null)
		{
			this.cbxOffsetX.Value = 0.0;
			this.cbxOffsetY.Value = 0.0;
			this.cbxRotation.Value = 0.0;
			this.cbxDistance.Value = 0.0;
			this.cbxWidth.Value = 0.0;
			this.cbxExponent.Value = 0.0;
			return;
		}
		this.cbxOffsetX.Value = (double)this.currentWarp.offset.x;
		this.cbxOffsetY.Value = (double)this.currentWarp.offset.y;
		this.cbxRotation.Value = (double)this.currentWarp.rotation;
		this.cbxDistance.Value = (double)this.currentWarp.distance;
		this.cbxWidth.Value = (double)this.currentWarp.width;
		this.cbxExponent.Value = (double)this.currentWarp.exponent;
	}

	// Token: 0x06008463 RID: 33891 RVA: 0x00326168 File Offset: 0x00324368
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
		this.currentWarp.offset = new Vector2((float)this.cbxOffsetX.Value, (float)this.cbxOffsetY.Value);
		this.currentWarp.rotation = (float)this.cbxRotation.Value;
		this.currentWarp.distance = (float)this.cbxDistance.Value;
		this.currentWarp.width = (float)this.cbxWidth.Value;
		this.currentWarp.exponent = (float)this.cbxExponent.Value;
		Action onLayerSettingsChanged = this.OnLayerSettingsChanged;
		if (onLayerSettingsChanged == null)
		{
			return;
		}
		onLayerSettingsChanged();
	}

	// Token: 0x04006224 RID: 25124
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData.StretchWarp currentWarp;

	// Token: 0x04006225 RID: 25125
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxOffsetX;

	// Token: 0x04006226 RID: 25126
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxOffsetY;

	// Token: 0x04006227 RID: 25127
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxRotation;

	// Token: 0x04006228 RID: 25128
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxDistance;

	// Token: 0x04006229 RID: 25129
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxWidth;

	// Token: 0x0400622A RID: 25130
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxExponent;
}
