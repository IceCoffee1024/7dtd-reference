using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001063 RID: 4195
[Preserve]
public class XUiC_BulgeWarpSettings : XUiC_SignWarpSettings
{
	// Token: 0x17000F99 RID: 3993
	// (get) Token: 0x060084AF RID: 33967 RVA: 0x0032A6E7 File Offset: 0x003288E7
	public override SignData.SignWarp CurrentWarp
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.currentWarp;
		}
	}

	// Token: 0x060084B0 RID: 33968 RVA: 0x0032A6F0 File Offset: 0x003288F0
	public override void Init()
	{
		base.Init();
		this.cbxOffsetX = (XUiC_ComboBoxFloat)base.GetChildById("cbxOffsetX").GetChildById("value");
		this.cbxOffsetX.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Bulge Offset X");
		};
		this.cbxOffsetY = (XUiC_ComboBoxFloat)base.GetChildById("cbxOffsetY").GetChildById("value");
		this.cbxOffsetY.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Bulge Offset Y");
		};
		this.cbxAmount = (XUiC_ComboBoxFloat)base.GetChildById("cbxAmount").GetChildById("value");
		this.cbxAmount.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Bulge Amount");
		};
		SignData.BulgeWarp bulgeWarp = new SignData.BulgeWarp();
		base.SetDefaultValue("cbxOffsetX", bulgeWarp.offset.x);
		base.SetDefaultValue("cbxOffsetY", bulgeWarp.offset.y);
		base.SetDefaultValue("cbxAmount", bulgeWarp.amount);
	}

	// Token: 0x060084B1 RID: 33969 RVA: 0x0032A7FC File Offset: 0x003289FC
	public override void SetWarp(SignData.SignWarp warp)
	{
		this.currentWarp = warp as SignData.BulgeWarp;
		if (this.currentWarp == null)
		{
			this.cbxOffsetX.Value = 0.0;
			this.cbxOffsetY.Value = 0.0;
			this.cbxAmount.Value = 0.0;
			return;
		}
		this.cbxOffsetX.Value = (double)this.currentWarp.offset.x;
		this.cbxOffsetY.Value = (double)this.currentWarp.offset.y;
		this.cbxAmount.Value = (double)this.currentWarp.amount;
	}

	// Token: 0x060084B2 RID: 33970 RVA: 0x0032A8AC File Offset: 0x00328AAC
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
		this.currentWarp.amount = (float)this.cbxAmount.Value;
		Action onLayerSettingsChanged = this.OnLayerSettingsChanged;
		if (onLayerSettingsChanged == null)
		{
			return;
		}
		onLayerSettingsChanged();
	}

	// Token: 0x04006285 RID: 25221
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData.BulgeWarp currentWarp;

	// Token: 0x04006286 RID: 25222
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxOffsetX;

	// Token: 0x04006287 RID: 25223
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxOffsetY;

	// Token: 0x04006288 RID: 25224
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxAmount;
}
