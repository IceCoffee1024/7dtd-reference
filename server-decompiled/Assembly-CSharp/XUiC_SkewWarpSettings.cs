using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001043 RID: 4163
[Preserve]
public class XUiC_SkewWarpSettings : XUiC_SignWarpSettings
{
	// Token: 0x17000F60 RID: 3936
	// (get) Token: 0x0600842C RID: 33836 RVA: 0x00324D90 File Offset: 0x00322F90
	public override SignData.SignWarp CurrentWarp
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.currentWarp;
		}
	}

	// Token: 0x0600842D RID: 33837 RVA: 0x00324D98 File Offset: 0x00322F98
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

	// Token: 0x0600842E RID: 33838 RVA: 0x00324EA4 File Offset: 0x003230A4
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

	// Token: 0x0600842F RID: 33839 RVA: 0x00324F54 File Offset: 0x00323154
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

	// Token: 0x04006208 RID: 25096
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData.SkewWarp currentWarp;

	// Token: 0x04006209 RID: 25097
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxSkewX;

	// Token: 0x0400620A RID: 25098
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxSkewY;

	// Token: 0x0400620B RID: 25099
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxRotation;
}
