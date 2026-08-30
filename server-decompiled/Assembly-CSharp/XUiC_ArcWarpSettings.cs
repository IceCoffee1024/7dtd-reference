using System;
using UnityEngine.Scripting;

// Token: 0x02001048 RID: 4168
[Preserve]
public class XUiC_ArcWarpSettings : XUiC_SignWarpSettings
{
	// Token: 0x17000F65 RID: 3941
	// (get) Token: 0x06008458 RID: 33880 RVA: 0x00325BE8 File Offset: 0x00323DE8
	public override SignData.SignWarp CurrentWarp
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.currentWarp;
		}
	}

	// Token: 0x06008459 RID: 33881 RVA: 0x00325BF0 File Offset: 0x00323DF0
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

	// Token: 0x0600845A RID: 33882 RVA: 0x00325CF0 File Offset: 0x00323EF0
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

	// Token: 0x0600845B RID: 33883 RVA: 0x00325D94 File Offset: 0x00323F94
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

	// Token: 0x04006220 RID: 25120
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData.ArcWarp currentWarp;

	// Token: 0x04006221 RID: 25121
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxRotation;

	// Token: 0x04006222 RID: 25122
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxRadius;

	// Token: 0x04006223 RID: 25123
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxWidth;
}
