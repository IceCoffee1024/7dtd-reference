using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001059 RID: 4185
[Preserve]
public class XUiC_GridWarpSettings : XUiC_SignWarpSettings
{
	// Token: 0x17000F94 RID: 3988
	// (get) Token: 0x06008494 RID: 33940 RVA: 0x00329C5B File Offset: 0x00327E5B
	public override SignData.SignWarp CurrentWarp
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.currentWarp;
		}
	}

	// Token: 0x06008495 RID: 33941 RVA: 0x00329C64 File Offset: 0x00327E64
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

	// Token: 0x06008496 RID: 33942 RVA: 0x00329EB0 File Offset: 0x003280B0
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnModeChanged()
	{
		this.OnValueChangedGeneric("Changed Grid Mode");
	}

	// Token: 0x06008497 RID: 33943 RVA: 0x00329EC0 File Offset: 0x003280C0
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

	// Token: 0x06008498 RID: 33944 RVA: 0x0032A020 File Offset: 0x00328220
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

	// Token: 0x06008499 RID: 33945 RVA: 0x0032A0FC File Offset: 0x003282FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateConditionalVisibility()
	{
		bool flag = this.cbxMode.SelectedIndex == 1;
		this.cbxAspectRow.ViewComponent.IsVisible = flag;
		this.cbxShiftRow.ViewComponent.IsVisible = flag;
	}

	// Token: 0x04006266 RID: 25190
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData.GridWarp currentWarp;

	// Token: 0x04006267 RID: 25191
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> cbxMode;

	// Token: 0x04006268 RID: 25192
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxOffsetX;

	// Token: 0x04006269 RID: 25193
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxOffsetY;

	// Token: 0x0400626A RID: 25194
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxRotation;

	// Token: 0x0400626B RID: 25195
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxScale;

	// Token: 0x0400626C RID: 25196
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController cbxAspectRow;

	// Token: 0x0400626D RID: 25197
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxAspect;

	// Token: 0x0400626E RID: 25198
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController cbxShiftRow;

	// Token: 0x0400626F RID: 25199
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxShift;
}
