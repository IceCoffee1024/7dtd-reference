using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001062 RID: 4194
[Preserve]
public class XUiC_SkewWarpSettings : XUiC_SignWarpSettings
{
	// Token: 0x17000F98 RID: 3992
	// (get) Token: 0x060084A7 RID: 33959 RVA: 0x0032A47C File Offset: 0x0032867C
	public override SignData.SignWarp CurrentWarp
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.currentWarp;
		}
	}

	// Token: 0x060084A8 RID: 33960 RVA: 0x0032A484 File Offset: 0x00328684
	public override void Init()
	{
		base.Init();
		this.cbxSkewX = (XUiC_ComboBoxFloat)base.GetChildById("cbxSkewX").GetChildById("value");
		this.cbxSkewX.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Skew X");
		};
		this.cbxSkewY = (XUiC_ComboBoxFloat)base.GetChildById("cbxSkewY").GetChildById("value");
		this.cbxSkewY.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Skew Y");
		};
		this.cbxRotation = (XUiC_ComboBoxFloat)base.GetChildById("cbxRotation").GetChildById("value");
		this.cbxRotation.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Skew Rotation");
		};
		SignData.SkewWarp skewWarp = new SignData.SkewWarp();
		base.SetDefaultValue("cbxSkewX", skewWarp.amount.x);
		base.SetDefaultValue("cbxSkewY", skewWarp.amount.y);
		base.SetDefaultValue("cbxRotation", skewWarp.rotation);
	}

	// Token: 0x060084A9 RID: 33961 RVA: 0x0032A590 File Offset: 0x00328790
	public override void SetWarp(SignData.SignWarp warp)
	{
		this.currentWarp = warp as SignData.SkewWarp;
		if (this.currentWarp == null)
		{
			this.cbxSkewX.Value = 0.0;
			this.cbxSkewY.Value = 0.0;
			this.cbxRotation.Value = 0.0;
			return;
		}
		this.cbxSkewX.Value = (double)this.currentWarp.amount.x;
		this.cbxSkewY.Value = (double)this.currentWarp.amount.y;
		this.cbxRotation.Value = (double)this.currentWarp.rotation;
	}

	// Token: 0x060084AA RID: 33962 RVA: 0x0032A640 File Offset: 0x00328840
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
		this.currentWarp.amount = new Vector2((float)this.cbxSkewX.Value, (float)this.cbxSkewY.Value);
		this.currentWarp.rotation = (float)this.cbxRotation.Value;
		Action onLayerSettingsChanged = this.OnLayerSettingsChanged;
		if (onLayerSettingsChanged == null)
		{
			return;
		}
		onLayerSettingsChanged();
	}

	// Token: 0x04006281 RID: 25217
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData.SkewWarp currentWarp;

	// Token: 0x04006282 RID: 25218
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxSkewX;

	// Token: 0x04006283 RID: 25219
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxSkewY;

	// Token: 0x04006284 RID: 25220
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxRotation;
}
