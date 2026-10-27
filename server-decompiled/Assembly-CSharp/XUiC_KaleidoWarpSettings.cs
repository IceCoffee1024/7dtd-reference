using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001065 RID: 4197
[Preserve]
public class XUiC_KaleidoWarpSettings : XUiC_SignWarpSettings
{
	// Token: 0x17000F9B RID: 3995
	// (get) Token: 0x060084C0 RID: 33984 RVA: 0x0032AC47 File Offset: 0x00328E47
	public override SignData.SignWarp CurrentWarp
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.currentWarp;
		}
	}

	// Token: 0x060084C1 RID: 33985 RVA: 0x0032AC50 File Offset: 0x00328E50
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

	// Token: 0x060084C2 RID: 33986 RVA: 0x0032ADF4 File Offset: 0x00328FF4
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

	// Token: 0x060084C3 RID: 33987 RVA: 0x0032AEF0 File Offset: 0x003290F0
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

	// Token: 0x0400628E RID: 25230
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData.KaleidoWarp currentWarp;

	// Token: 0x0400628F RID: 25231
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxOffsetX;

	// Token: 0x04006290 RID: 25232
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxOffsetY;

	// Token: 0x04006291 RID: 25233
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxOffsetScale;

	// Token: 0x04006292 RID: 25234
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxInt cbxSides;

	// Token: 0x04006293 RID: 25235
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxRotation;
}
