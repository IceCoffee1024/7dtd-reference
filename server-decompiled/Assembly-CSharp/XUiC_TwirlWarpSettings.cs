using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001045 RID: 4165
[Preserve]
public class XUiC_TwirlWarpSettings : XUiC_SignWarpSettings
{
	// Token: 0x17000F62 RID: 3938
	// (get) Token: 0x0600843C RID: 33852 RVA: 0x0032525F File Offset: 0x0032345F
	public override SignData.SignWarp CurrentWarp
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.currentWarp;
		}
	}

	// Token: 0x0600843D RID: 33853 RVA: 0x00325268 File Offset: 0x00323468
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

	// Token: 0x0600843E RID: 33854 RVA: 0x003253C0 File Offset: 0x003235C0
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

	// Token: 0x0600843F RID: 33855 RVA: 0x00325498 File Offset: 0x00323698
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

	// Token: 0x04006210 RID: 25104
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData.TwirlWarp currentWarp;

	// Token: 0x04006211 RID: 25105
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxOffsetX;

	// Token: 0x04006212 RID: 25106
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxOffsetY;

	// Token: 0x04006213 RID: 25107
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxAmount;

	// Token: 0x04006214 RID: 25108
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxFreq;
}
