using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200102A RID: 4138
[Preserve]
public class XUiC_SkewWarpSettings : XUiC_SignWarpSettings
{
	// Token: 0x17000F27 RID: 3879
	// (get) Token: 0x0600833A RID: 33594 RVA: 0x00320568 File Offset: 0x0031E768
	public override SignData.SignWarp CurrentWarp
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.currentWarp;
		}
	}

	// Token: 0x0600833B RID: 33595 RVA: 0x00320570 File Offset: 0x0031E770
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

	// Token: 0x0600833C RID: 33596 RVA: 0x0032067C File Offset: 0x0031E87C
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

	// Token: 0x0600833D RID: 33597 RVA: 0x0032072C File Offset: 0x0031E92C
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

	// Token: 0x04006141 RID: 24897
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData.SkewWarp currentWarp;

	// Token: 0x04006142 RID: 24898
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxSkewX;

	// Token: 0x04006143 RID: 24899
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxSkewY;

	// Token: 0x04006144 RID: 24900
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxRotation;
}
