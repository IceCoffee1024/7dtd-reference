using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200102C RID: 4140
[Preserve]
public class XUiC_TwirlWarpSettings : XUiC_SignWarpSettings
{
	// Token: 0x17000F29 RID: 3881
	// (get) Token: 0x0600834A RID: 33610 RVA: 0x00320A37 File Offset: 0x0031EC37
	public override SignData.SignWarp CurrentWarp
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.currentWarp;
		}
	}

	// Token: 0x0600834B RID: 33611 RVA: 0x00320A40 File Offset: 0x0031EC40
	public override void Init()
	{
		base.Init();
		this.cbxOffsetX = (XUiC_ComboBoxFloat)base.GetChildById("cbxOffsetX").GetChildById("value");
		this.cbxOffsetX.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Twirl Offset X");
		};
		this.cbxOffsetY = (XUiC_ComboBoxFloat)base.GetChildById("cbxOffsetY").GetChildById("value");
		this.cbxOffsetY.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Twirl Offset Y");
		};
		this.cbxAmount = (XUiC_ComboBoxFloat)base.GetChildById("cbxAmount").GetChildById("value");
		this.cbxAmount.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Twirl Amount");
		};
		this.cbxFreq = (XUiC_ComboBoxFloat)base.GetChildById("cbxFreq").GetChildById("value");
		this.cbxFreq.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Twirl Frequency");
		};
		SignData.TwirlWarp twirlWarp = new SignData.TwirlWarp();
		base.SetDefaultValue("cbxOffsetX", twirlWarp.offset.x);
		base.SetDefaultValue("cbxOffsetY", twirlWarp.offset.y);
		base.SetDefaultValue("cbxAmount", twirlWarp.amount);
		base.SetDefaultValue("cbxFreq", twirlWarp.frequency);
	}

	// Token: 0x0600834C RID: 33612 RVA: 0x00320B98 File Offset: 0x0031ED98
	public override void SetWarp(SignData.SignWarp warp)
	{
		this.currentWarp = warp as SignData.TwirlWarp;
		if (this.currentWarp == null)
		{
			this.cbxOffsetX.Value = 0.0;
			this.cbxOffsetY.Value = 0.0;
			this.cbxAmount.Value = 0.0;
			this.cbxFreq.Value = 0.0;
			return;
		}
		this.cbxOffsetX.Value = (double)this.currentWarp.offset.x;
		this.cbxOffsetY.Value = (double)this.currentWarp.offset.y;
		this.cbxAmount.Value = (double)this.currentWarp.amount;
		this.cbxFreq.Value = (double)this.currentWarp.frequency;
	}

	// Token: 0x0600834D RID: 33613 RVA: 0x00320C70 File Offset: 0x0031EE70
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
		this.currentWarp.frequency = (float)this.cbxFreq.Value;
		Action onLayerSettingsChanged = this.OnLayerSettingsChanged;
		if (onLayerSettingsChanged == null)
		{
			return;
		}
		onLayerSettingsChanged();
	}

	// Token: 0x04006149 RID: 24905
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData.TwirlWarp currentWarp;

	// Token: 0x0400614A RID: 24906
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxOffsetX;

	// Token: 0x0400614B RID: 24907
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxOffsetY;

	// Token: 0x0400614C RID: 24908
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxAmount;

	// Token: 0x0400614D RID: 24909
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxFreq;
}
