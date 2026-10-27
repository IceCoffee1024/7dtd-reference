using System;
using UnityEngine.Scripting;

// Token: 0x02001067 RID: 4199
[Preserve]
public class XUiC_ArcWarpSettings : XUiC_SignWarpSettings
{
	// Token: 0x17000F9D RID: 3997
	// (get) Token: 0x060084D3 RID: 34003 RVA: 0x0032B2D4 File Offset: 0x003294D4
	public override SignData.SignWarp CurrentWarp
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.currentWarp;
		}
	}

	// Token: 0x060084D4 RID: 34004 RVA: 0x0032B2DC File Offset: 0x003294DC
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

	// Token: 0x060084D5 RID: 34005 RVA: 0x0032B3DC File Offset: 0x003295DC
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

	// Token: 0x060084D6 RID: 34006 RVA: 0x0032B480 File Offset: 0x00329680
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

	// Token: 0x04006299 RID: 25241
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData.ArcWarp currentWarp;

	// Token: 0x0400629A RID: 25242
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxRotation;

	// Token: 0x0400629B RID: 25243
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxRadius;

	// Token: 0x0400629C RID: 25244
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxWidth;
}
