using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001056 RID: 4182
[Preserve]
public class XUiC_PerspectiveWarpSettings : XUiC_SignWarpSettings
{
	// Token: 0x17000F91 RID: 3985
	// (get) Token: 0x06008478 RID: 33912 RVA: 0x003292D3 File Offset: 0x003274D3
	public override SignData.SignWarp CurrentWarp
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.currentWarp;
		}
	}

	// Token: 0x06008479 RID: 33913 RVA: 0x003292DC File Offset: 0x003274DC
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

	// Token: 0x0600847A RID: 33914 RVA: 0x00329438 File Offset: 0x00327638
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

	// Token: 0x0600847B RID: 33915 RVA: 0x00329518 File Offset: 0x00327718
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

	// Token: 0x04006256 RID: 25174
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData.PerspectiveWarp currentWarp;

	// Token: 0x04006257 RID: 25175
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxRotationX;

	// Token: 0x04006258 RID: 25176
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxRotationY;

	// Token: 0x04006259 RID: 25177
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxRotationZ;

	// Token: 0x0400625A RID: 25178
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxStrength;
}
