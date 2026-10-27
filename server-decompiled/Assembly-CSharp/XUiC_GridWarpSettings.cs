using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001069 RID: 4201
[Preserve]
public class XUiC_GridWarpSettings : XUiC_SignWarpSettings
{
	// Token: 0x17000F9F RID: 3999
	// (get) Token: 0x060084E6 RID: 34022 RVA: 0x0032B95F File Offset: 0x00329B5F
	public override SignData.SignWarp CurrentWarp
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.currentWarp;
		}
	}

	// Token: 0x060084E7 RID: 34023 RVA: 0x0032B968 File Offset: 0x00329B68
	public override void Init()
	{
		base.Init();
		this.cbxMode = base.GetChildById("cbxMode").GetChildByType<XUiC_ComboBoxList<string>>();
		this.cbxMode.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnModeChanged();
		};
		this.cbxOffsetX = (XUiC_ComboBoxFloat)base.GetChildById("cbxOffsetX").GetChildById("value");
		this.cbxOffsetX.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Grid Offset X");
		};
		this.cbxOffsetY = (XUiC_ComboBoxFloat)base.GetChildById("cbxOffsetY").GetChildById("value");
		this.cbxOffsetY.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Grid Offset Y");
		};
		this.cbxRotation = (XUiC_ComboBoxFloat)base.GetChildById("cbxRotation").GetChildById("value");
		this.cbxRotation.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Grid Rotation");
		};
		this.cbxScale = (XUiC_ComboBoxFloat)base.GetChildById("cbxScale").GetChildById("value");
		this.cbxScale.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Grid Scale");
		};
		this.cbxAspectRow = base.GetChildById("cbxAspect");
		this.cbxAspect = (XUiC_ComboBoxFloat)this.cbxAspectRow.GetChildById("value");
		this.cbxAspect.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Grid Aspect");
		};
		this.cbxShiftRow = base.GetChildById("cbxShift");
		this.cbxShift = (XUiC_ComboBoxFloat)this.cbxShiftRow.GetChildById("value");
		this.cbxShift.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Grid Shift");
		};
		SignData.GridWarp gridWarp = new SignData.GridWarp();
		base.SetDefaultValue("cbxMode", gridWarp.mode);
		base.SetDefaultValue("cbxOffsetX", gridWarp.offset.x);
		base.SetDefaultValue("cbxOffsetY", gridWarp.offset.y);
		base.SetDefaultValue("cbxRotation", gridWarp.rotation);
		base.SetDefaultValue("cbxScale", gridWarp.scale);
		base.SetDefaultValue("cbxAspect", gridWarp.aspect);
		base.SetDefaultValue("cbxShift", gridWarp.shift);
	}

	// Token: 0x060084E8 RID: 34024 RVA: 0x0032BBB4 File Offset: 0x00329DB4
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnModeChanged()
	{
		this.OnValueChangedGeneric("Changed Grid Mode");
	}

	// Token: 0x060084E9 RID: 34025 RVA: 0x0032BBC4 File Offset: 0x00329DC4
	public override void SetWarp(SignData.SignWarp warp)
	{
		this.currentWarp = warp as SignData.GridWarp;
		if (this.currentWarp == null)
		{
			this.cbxMode.SelectedIndex = 1;
			this.cbxOffsetX.Value = 0.0;
			this.cbxOffsetY.Value = 0.0;
			this.cbxRotation.Value = 0.0;
			this.cbxScale.Value = 1.0;
			this.cbxAspect.Value = 1.0;
			this.cbxShift.Value = 0.0;
			this.UpdateConditionalVisibility();
			return;
		}
		this.cbxMode.SelectedIndex = (int)this.currentWarp.mode;
		this.cbxOffsetX.Value = (double)this.currentWarp.offset.x;
		this.cbxOffsetY.Value = (double)this.currentWarp.offset.y;
		this.cbxRotation.Value = (double)this.currentWarp.rotation;
		this.cbxScale.Value = (double)this.currentWarp.scale;
		this.cbxAspect.Value = (double)this.currentWarp.aspect;
		this.cbxShift.Value = (double)this.currentWarp.shift;
		this.UpdateConditionalVisibility();
	}

	// Token: 0x060084EA RID: 34026 RVA: 0x0032BD24 File Offset: 0x00329F24
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
		this.currentWarp.mode = (SignData.GridWarp.Mode)this.cbxMode.SelectedIndex;
		this.currentWarp.offset = new Vector2((float)this.cbxOffsetX.Value, (float)this.cbxOffsetY.Value);
		this.currentWarp.rotation = (float)this.cbxRotation.Value;
		this.currentWarp.scale = (float)this.cbxScale.Value;
		this.currentWarp.aspect = (float)this.cbxAspect.Value;
		this.currentWarp.shift = (float)this.cbxShift.Value;
		this.UpdateConditionalVisibility();
		Action onLayerSettingsChanged = this.OnLayerSettingsChanged;
		if (onLayerSettingsChanged == null)
		{
			return;
		}
		onLayerSettingsChanged();
	}

	// Token: 0x060084EB RID: 34027 RVA: 0x0032BE00 File Offset: 0x0032A000
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateConditionalVisibility()
	{
		bool flag = this.cbxMode.SelectedIndex == 1;
		this.cbxAspectRow.ViewComponent.IsVisible = flag;
		this.cbxShiftRow.ViewComponent.IsVisible = flag;
	}

	// Token: 0x040062A4 RID: 25252
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData.GridWarp currentWarp;

	// Token: 0x040062A5 RID: 25253
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> cbxMode;

	// Token: 0x040062A6 RID: 25254
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxOffsetX;

	// Token: 0x040062A7 RID: 25255
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxOffsetY;

	// Token: 0x040062A8 RID: 25256
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxRotation;

	// Token: 0x040062A9 RID: 25257
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxScale;

	// Token: 0x040062AA RID: 25258
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController cbxAspectRow;

	// Token: 0x040062AB RID: 25259
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxAspect;

	// Token: 0x040062AC RID: 25260
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController cbxShiftRow;

	// Token: 0x040062AD RID: 25261
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxShift;
}
