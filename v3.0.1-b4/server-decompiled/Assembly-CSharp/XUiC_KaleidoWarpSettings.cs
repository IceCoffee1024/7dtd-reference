using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200102D RID: 4141
[Preserve]
public class XUiC_KaleidoWarpSettings : XUiC_SignWarpSettings
{
	// Token: 0x17000F2A RID: 3882
	// (get) Token: 0x06008353 RID: 33619 RVA: 0x00320D33 File Offset: 0x0031EF33
	public override SignData.SignWarp CurrentWarp
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.currentWarp;
		}
	}

	// Token: 0x06008354 RID: 33620 RVA: 0x00320D3C File Offset: 0x0031EF3C
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

	// Token: 0x06008355 RID: 33621 RVA: 0x00320EE0 File Offset: 0x0031F0E0
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

	// Token: 0x06008356 RID: 33622 RVA: 0x00320FDC File Offset: 0x0031F1DC
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

	// Token: 0x0400614E RID: 24910
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData.KaleidoWarp currentWarp;

	// Token: 0x0400614F RID: 24911
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxOffsetX;

	// Token: 0x04006150 RID: 24912
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxOffsetY;

	// Token: 0x04006151 RID: 24913
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxOffsetScale;

	// Token: 0x04006152 RID: 24914
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxInt cbxSides;

	// Token: 0x04006153 RID: 24915
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxRotation;
}
