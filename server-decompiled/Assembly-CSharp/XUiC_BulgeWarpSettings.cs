using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001053 RID: 4179
[Preserve]
public class XUiC_BulgeWarpSettings : XUiC_SignWarpSettings
{
	// Token: 0x17000F8E RID: 3982
	// (get) Token: 0x0600845D RID: 33885 RVA: 0x003289E3 File Offset: 0x00326BE3
	public override SignData.SignWarp CurrentWarp
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.currentWarp;
		}
	}

	// Token: 0x0600845E RID: 33886 RVA: 0x003289EC File Offset: 0x00326BEC
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

	// Token: 0x0600845F RID: 33887 RVA: 0x00328AF8 File Offset: 0x00326CF8
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

	// Token: 0x06008460 RID: 33888 RVA: 0x00328BA8 File Offset: 0x00326DA8
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

	// Token: 0x04006247 RID: 25159
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData.BulgeWarp currentWarp;

	// Token: 0x04006248 RID: 25160
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxOffsetX;

	// Token: 0x04006249 RID: 25161
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxOffsetY;

	// Token: 0x0400624A RID: 25162
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxAmount;
}
