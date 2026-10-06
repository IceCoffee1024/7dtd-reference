using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001054 RID: 4180
[Preserve]
public class XUiC_TwirlWarpSettings : XUiC_SignWarpSettings
{
	// Token: 0x17000F8F RID: 3983
	// (get) Token: 0x06008465 RID: 33893 RVA: 0x00328C47 File Offset: 0x00326E47
	public override SignData.SignWarp CurrentWarp
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.currentWarp;
		}
	}

	// Token: 0x06008466 RID: 33894 RVA: 0x00328C50 File Offset: 0x00326E50
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

	// Token: 0x06008467 RID: 33895 RVA: 0x00328DA8 File Offset: 0x00326FA8
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

	// Token: 0x06008468 RID: 33896 RVA: 0x00328E80 File Offset: 0x00327080
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

	// Token: 0x0400624B RID: 25163
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData.TwirlWarp currentWarp;

	// Token: 0x0400624C RID: 25164
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxOffsetX;

	// Token: 0x0400624D RID: 25165
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxOffsetY;

	// Token: 0x0400624E RID: 25166
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxAmount;

	// Token: 0x0400624F RID: 25167
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxFreq;
}
