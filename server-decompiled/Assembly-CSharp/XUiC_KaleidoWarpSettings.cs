using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001046 RID: 4166
[Preserve]
public class XUiC_KaleidoWarpSettings : XUiC_SignWarpSettings
{
	// Token: 0x17000F63 RID: 3939
	// (get) Token: 0x06008445 RID: 33861 RVA: 0x0032555B File Offset: 0x0032375B
	public override SignData.SignWarp CurrentWarp
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.currentWarp;
		}
	}

	// Token: 0x06008446 RID: 33862 RVA: 0x00325564 File Offset: 0x00323764
	public override void Init()
	{
		base.Init();
		this.cbxOffsetX = (XUiC_ComboBoxFloat)base.GetChildById("cbxOffsetX").GetChildById("value");
		this.cbxOffsetX.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Kaleido Offset X");
		};
		this.cbxOffsetY = (XUiC_ComboBoxFloat)base.GetChildById("cbxOffsetY").GetChildById("value");
		this.cbxOffsetY.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Kaleido Offset Y");
		};
		this.cbxOffsetScale = (XUiC_ComboBoxFloat)base.GetChildById("cbxOffsetScale").GetChildById("value");
		this.cbxOffsetScale.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Kaleido Offset Scale");
		};
		this.cbxSides = (XUiC_ComboBoxInt)base.GetChildById("cbxSides").GetChildById("value");
		this.cbxSides.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Kaleido Sides");
		};
		this.cbxRotation = (XUiC_ComboBoxFloat)base.GetChildById("cbxRotation").GetChildById("value");
		this.cbxRotation.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Kaleido Rotation");
		};
		SignData.KaleidoWarp kaleidoWarp = new SignData.KaleidoWarp();
		base.SetDefaultValue("cbxOffsetX", kaleidoWarp.offset.x);
		base.SetDefaultValue("cbxOffsetY", kaleidoWarp.offset.y);
		base.SetDefaultValue("cbxOffsetScale", kaleidoWarp.offsetScale);
		base.SetDefaultValue("cbxSides", kaleidoWarp.sides);
		base.SetDefaultValue("cbxRotation", kaleidoWarp.rotation);
	}

	// Token: 0x06008447 RID: 33863 RVA: 0x00325708 File Offset: 0x00323908
	public override void SetWarp(SignData.SignWarp warp)
	{
		this.currentWarp = warp as SignData.KaleidoWarp;
		if (this.currentWarp == null)
		{
			this.cbxOffsetX.Value = 0.0;
			this.cbxOffsetY.Value = 0.0;
			this.cbxOffsetScale.Value = 0.0;
			this.cbxSides.Value = 0L;
			this.cbxRotation.Value = 0.0;
			return;
		}
		this.cbxOffsetX.Value = (double)this.currentWarp.offset.x;
		this.cbxOffsetY.Value = (double)this.currentWarp.offset.y;
		this.cbxOffsetScale.Value = (double)this.currentWarp.offsetScale;
		this.cbxSides.Value = (long)this.currentWarp.sides;
		this.cbxRotation.Value = (double)this.currentWarp.rotation;
	}

	// Token: 0x06008448 RID: 33864 RVA: 0x00325804 File Offset: 0x00323A04
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
		this.currentWarp.offsetScale = (float)this.cbxOffsetScale.Value;
		this.currentWarp.sides = (int)this.cbxSides.Value;
		this.currentWarp.rotation = (float)this.cbxRotation.Value;
		Action onLayerSettingsChanged = this.OnLayerSettingsChanged;
		if (onLayerSettingsChanged == null)
		{
			return;
		}
		onLayerSettingsChanged();
	}

	// Token: 0x04006215 RID: 25109
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData.KaleidoWarp currentWarp;

	// Token: 0x04006216 RID: 25110
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxOffsetX;

	// Token: 0x04006217 RID: 25111
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxOffsetY;

	// Token: 0x04006218 RID: 25112
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxOffsetScale;

	// Token: 0x04006219 RID: 25113
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxInt cbxSides;

	// Token: 0x0400621A RID: 25114
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxRotation;
}
