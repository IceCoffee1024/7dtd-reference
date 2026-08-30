using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200102E RID: 4142
[Preserve]
public class XUiC_PerspectiveWarpSettings : XUiC_SignWarpSettings
{
	// Token: 0x17000F2B RID: 3883
	// (get) Token: 0x0600835D RID: 33629 RVA: 0x003210C3 File Offset: 0x0031F2C3
	public override SignData.SignWarp CurrentWarp
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.currentWarp;
		}
	}

	// Token: 0x0600835E RID: 33630 RVA: 0x003210CC File Offset: 0x0031F2CC
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

	// Token: 0x0600835F RID: 33631 RVA: 0x00321228 File Offset: 0x0031F428
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

	// Token: 0x06008360 RID: 33632 RVA: 0x00321308 File Offset: 0x0031F508
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

	// Token: 0x04006154 RID: 24916
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData.PerspectiveWarp currentWarp;

	// Token: 0x04006155 RID: 24917
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxRotationX;

	// Token: 0x04006156 RID: 24918
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxRotationY;

	// Token: 0x04006157 RID: 24919
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxRotationZ;

	// Token: 0x04006158 RID: 24920
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxStrength;
}
