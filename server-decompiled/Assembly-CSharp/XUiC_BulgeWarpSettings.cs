using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001044 RID: 4164
[Preserve]
public class XUiC_BulgeWarpSettings : XUiC_SignWarpSettings
{
	// Token: 0x17000F61 RID: 3937
	// (get) Token: 0x06008434 RID: 33844 RVA: 0x00324FFB File Offset: 0x003231FB
	public override SignData.SignWarp CurrentWarp
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.currentWarp;
		}
	}

	// Token: 0x06008435 RID: 33845 RVA: 0x00325004 File Offset: 0x00323204
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

	// Token: 0x06008436 RID: 33846 RVA: 0x00325110 File Offset: 0x00323310
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

	// Token: 0x06008437 RID: 33847 RVA: 0x003251C0 File Offset: 0x003233C0
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

	// Token: 0x0400620C RID: 25100
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData.BulgeWarp currentWarp;

	// Token: 0x0400620D RID: 25101
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxOffsetX;

	// Token: 0x0400620E RID: 25102
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxOffsetY;

	// Token: 0x0400620F RID: 25103
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxAmount;
}
