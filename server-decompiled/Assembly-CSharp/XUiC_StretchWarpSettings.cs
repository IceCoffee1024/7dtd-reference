using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001068 RID: 4200
[Preserve]
public class XUiC_StretchWarpSettings : XUiC_SignWarpSettings
{
	// Token: 0x17000F9E RID: 3998
	// (get) Token: 0x060084DB RID: 34011 RVA: 0x0032B525 File Offset: 0x00329725
	public override SignData.SignWarp CurrentWarp
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.currentWarp;
		}
	}

	// Token: 0x060084DC RID: 34012 RVA: 0x0032B530 File Offset: 0x00329730
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

	// Token: 0x060084DD RID: 34013 RVA: 0x0032B724 File Offset: 0x00329924
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

	// Token: 0x060084DE RID: 34014 RVA: 0x0032B854 File Offset: 0x00329A54
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

	// Token: 0x0400629D RID: 25245
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData.StretchWarp currentWarp;

	// Token: 0x0400629E RID: 25246
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxOffsetX;

	// Token: 0x0400629F RID: 25247
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxOffsetY;

	// Token: 0x040062A0 RID: 25248
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxRotation;

	// Token: 0x040062A1 RID: 25249
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxDistance;

	// Token: 0x040062A2 RID: 25250
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxWidth;

	// Token: 0x040062A3 RID: 25251
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxExponent;
}
