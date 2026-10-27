using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001066 RID: 4198
[Preserve]
public class XUiC_PerspectiveWarpSettings : XUiC_SignWarpSettings
{
	// Token: 0x17000F9C RID: 3996
	// (get) Token: 0x060084CA RID: 33994 RVA: 0x0032AFD7 File Offset: 0x003291D7
	public override SignData.SignWarp CurrentWarp
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.currentWarp;
		}
	}

	// Token: 0x060084CB RID: 33995 RVA: 0x0032AFE0 File Offset: 0x003291E0
	public override void Init()
	{
		base.Init();
		this.cbxRotationX = (XUiC_ComboBoxFloat)base.GetChildById("cbxRotationX").GetChildById("value");
		this.cbxRotationX.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Perspective Rotation X");
		};
		this.cbxRotationY = (XUiC_ComboBoxFloat)base.GetChildById("cbxRotationY").GetChildById("value");
		this.cbxRotationY.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Perspective Rotation Y");
		};
		this.cbxRotationZ = (XUiC_ComboBoxFloat)base.GetChildById("cbxRotationZ").GetChildById("value");
		this.cbxRotationZ.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Perspective Rotation Z");
		};
		this.cbxStrength = (XUiC_ComboBoxFloat)base.GetChildById("cbxStrength").GetChildById("value");
		this.cbxStrength.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Perspective Strength");
		};
		SignData.PerspectiveWarp perspectiveWarp = new SignData.PerspectiveWarp();
		base.SetDefaultValue("cbxRotationX", perspectiveWarp.rotation.x);
		base.SetDefaultValue("cbxRotationY", perspectiveWarp.rotation.y);
		base.SetDefaultValue("cbxRotationZ", perspectiveWarp.rotation.z);
		base.SetDefaultValue("cbxStrength", perspectiveWarp.strength);
	}

	// Token: 0x060084CC RID: 33996 RVA: 0x0032B13C File Offset: 0x0032933C
	public override void SetWarp(SignData.SignWarp warp)
	{
		this.currentWarp = warp as SignData.PerspectiveWarp;
		if (this.currentWarp == null)
		{
			this.cbxRotationX.Value = 0.0;
			this.cbxRotationY.Value = 0.0;
			this.cbxRotationZ.Value = 0.0;
			this.cbxStrength.Value = 0.0;
			return;
		}
		this.cbxRotationX.Value = (double)this.currentWarp.rotation.x;
		this.cbxRotationY.Value = (double)this.currentWarp.rotation.y;
		this.cbxRotationZ.Value = (double)this.currentWarp.rotation.z;
		this.cbxStrength.Value = (double)this.currentWarp.strength;
	}

	// Token: 0x060084CD RID: 33997 RVA: 0x0032B21C File Offset: 0x0032941C
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
		this.currentWarp.rotation = new Vector3((float)this.cbxRotationX.Value, (float)this.cbxRotationY.Value, (float)this.cbxRotationZ.Value);
		this.currentWarp.strength = (float)this.cbxStrength.Value;
		Action onLayerSettingsChanged = this.OnLayerSettingsChanged;
		if (onLayerSettingsChanged == null)
		{
			return;
		}
		onLayerSettingsChanged();
	}

	// Token: 0x04006294 RID: 25236
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData.PerspectiveWarp currentWarp;

	// Token: 0x04006295 RID: 25237
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxRotationX;

	// Token: 0x04006296 RID: 25238
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxRotationY;

	// Token: 0x04006297 RID: 25239
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxRotationZ;

	// Token: 0x04006298 RID: 25240
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxStrength;
}
