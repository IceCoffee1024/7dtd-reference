using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001055 RID: 4181
[Preserve]
public class XUiC_KaleidoWarpSettings : XUiC_SignWarpSettings
{
	// Token: 0x17000F90 RID: 3984
	// (get) Token: 0x0600846E RID: 33902 RVA: 0x00328F43 File Offset: 0x00327143
	public override SignData.SignWarp CurrentWarp
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.currentWarp;
		}
	}

	// Token: 0x0600846F RID: 33903 RVA: 0x00328F4C File Offset: 0x0032714C
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

	// Token: 0x06008470 RID: 33904 RVA: 0x003290F0 File Offset: 0x003272F0
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

	// Token: 0x06008471 RID: 33905 RVA: 0x003291EC File Offset: 0x003273EC
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

	// Token: 0x04006250 RID: 25168
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData.KaleidoWarp currentWarp;

	// Token: 0x04006251 RID: 25169
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxOffsetX;

	// Token: 0x04006252 RID: 25170
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxOffsetY;

	// Token: 0x04006253 RID: 25171
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxOffsetScale;

	// Token: 0x04006254 RID: 25172
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxInt cbxSides;

	// Token: 0x04006255 RID: 25173
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxRotation;
}
