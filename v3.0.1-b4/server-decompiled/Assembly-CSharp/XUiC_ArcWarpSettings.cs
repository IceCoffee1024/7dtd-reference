using System;
using UnityEngine.Scripting;

// Token: 0x0200102F RID: 4143
[Preserve]
public class XUiC_ArcWarpSettings : XUiC_SignWarpSettings
{
	// Token: 0x17000F2C RID: 3884
	// (get) Token: 0x06008366 RID: 33638 RVA: 0x003213C0 File Offset: 0x0031F5C0
	public override SignData.SignWarp CurrentWarp
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.currentWarp;
		}
	}

	// Token: 0x06008367 RID: 33639 RVA: 0x003213C8 File Offset: 0x0031F5C8
	public override void Init()
	{
		base.Init();
		this.cbxRotation = (XUiC_ComboBoxFloat)base.GetChildById("cbxRotation").GetChildById("value");
		this.cbxRotation.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Arc Rotation");
		};
		this.cbxRadius = (XUiC_ComboBoxFloat)base.GetChildById("cbxRadius").GetChildById("value");
		this.cbxRadius.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Arc Radius");
		};
		this.cbxWidth = (XUiC_ComboBoxFloat)base.GetChildById("cbxWidth").GetChildById("value");
		this.cbxWidth.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Arc Width");
		};
		SignData.ArcWarp arcWarp = new SignData.ArcWarp();
		base.SetDefaultValue("cbxRotation", arcWarp.rotation);
		base.SetDefaultValue("cbxRadius", arcWarp.radius);
		base.SetDefaultValue("cbxWidth", arcWarp.width);
	}

	// Token: 0x06008368 RID: 33640 RVA: 0x003214C8 File Offset: 0x0031F6C8
	public override void SetWarp(SignData.SignWarp warp)
	{
		this.currentWarp = warp as SignData.ArcWarp;
		if (this.currentWarp == null)
		{
			this.cbxRotation.Value = 0.0;
			this.cbxRadius.Value = 0.0;
			this.cbxWidth.Value = 0.0;
			return;
		}
		this.cbxRotation.Value = (double)this.currentWarp.rotation;
		this.cbxRadius.Value = (double)this.currentWarp.radius;
		this.cbxWidth.Value = (double)this.currentWarp.width;
	}

	// Token: 0x06008369 RID: 33641 RVA: 0x0032156C File Offset: 0x0031F76C
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
		this.currentWarp.rotation = (float)this.cbxRotation.Value;
		this.currentWarp.radius = (float)this.cbxRadius.Value;
		this.currentWarp.width = (float)this.cbxWidth.Value;
		Action onLayerSettingsChanged = this.OnLayerSettingsChanged;
		if (onLayerSettingsChanged == null)
		{
			return;
		}
		onLayerSettingsChanged();
	}

	// Token: 0x04006159 RID: 24921
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData.ArcWarp currentWarp;

	// Token: 0x0400615A RID: 24922
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxRotation;

	// Token: 0x0400615B RID: 24923
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxRadius;

	// Token: 0x0400615C RID: 24924
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxWidth;
}
