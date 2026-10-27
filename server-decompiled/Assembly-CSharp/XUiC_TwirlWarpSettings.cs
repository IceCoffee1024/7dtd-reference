using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001064 RID: 4196
[Preserve]
public class XUiC_TwirlWarpSettings : XUiC_SignWarpSettings
{
	// Token: 0x17000F9A RID: 3994
	// (get) Token: 0x060084B7 RID: 33975 RVA: 0x0032A94B File Offset: 0x00328B4B
	public override SignData.SignWarp CurrentWarp
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.currentWarp;
		}
	}

	// Token: 0x060084B8 RID: 33976 RVA: 0x0032A954 File Offset: 0x00328B54
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

	// Token: 0x060084B9 RID: 33977 RVA: 0x0032AAAC File Offset: 0x00328CAC
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

	// Token: 0x060084BA RID: 33978 RVA: 0x0032AB84 File Offset: 0x00328D84
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

	// Token: 0x04006289 RID: 25225
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData.TwirlWarp currentWarp;

	// Token: 0x0400628A RID: 25226
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxOffsetX;

	// Token: 0x0400628B RID: 25227
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxOffsetY;

	// Token: 0x0400628C RID: 25228
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxAmount;

	// Token: 0x0400628D RID: 25229
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxFreq;
}
