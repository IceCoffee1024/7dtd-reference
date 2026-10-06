using System;
using UnityEngine.Scripting;

// Token: 0x02001057 RID: 4183
[Preserve]
public class XUiC_ArcWarpSettings : XUiC_SignWarpSettings
{
	// Token: 0x17000F92 RID: 3986
	// (get) Token: 0x06008481 RID: 33921 RVA: 0x003295D0 File Offset: 0x003277D0
	public override SignData.SignWarp CurrentWarp
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.currentWarp;
		}
	}

	// Token: 0x06008482 RID: 33922 RVA: 0x003295D8 File Offset: 0x003277D8
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

	// Token: 0x06008483 RID: 33923 RVA: 0x003296D8 File Offset: 0x003278D8
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

	// Token: 0x06008484 RID: 33924 RVA: 0x0032977C File Offset: 0x0032797C
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

	// Token: 0x0400625B RID: 25179
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData.ArcWarp currentWarp;

	// Token: 0x0400625C RID: 25180
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxRotation;

	// Token: 0x0400625D RID: 25181
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxRadius;

	// Token: 0x0400625E RID: 25182
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxWidth;
}
