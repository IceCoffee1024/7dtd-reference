using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200104A RID: 4170
[Preserve]
public class XUiC_GridWarpSettings : XUiC_SignWarpSettings
{
	// Token: 0x17000F67 RID: 3943
	// (get) Token: 0x0600846B RID: 33899 RVA: 0x00326273 File Offset: 0x00324473
	public override SignData.SignWarp CurrentWarp
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.currentWarp;
		}
	}

	// Token: 0x0600846C RID: 33900 RVA: 0x0032627C File Offset: 0x0032447C
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

	// Token: 0x0600846D RID: 33901 RVA: 0x003264C8 File Offset: 0x003246C8
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnModeChanged()
	{
		this.OnValueChangedGeneric("Changed Grid Mode");
	}

	// Token: 0x0600846E RID: 33902 RVA: 0x003264D8 File Offset: 0x003246D8
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

	// Token: 0x0600846F RID: 33903 RVA: 0x00326638 File Offset: 0x00324838
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

	// Token: 0x06008470 RID: 33904 RVA: 0x00326714 File Offset: 0x00324914
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateConditionalVisibility()
	{
		bool flag = this.cbxMode.SelectedIndex == 1;
		this.cbxAspectRow.ViewComponent.IsVisible = flag;
		this.cbxShiftRow.ViewComponent.IsVisible = flag;
	}

	// Token: 0x0400622B RID: 25131
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData.GridWarp currentWarp;

	// Token: 0x0400622C RID: 25132
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> cbxMode;

	// Token: 0x0400622D RID: 25133
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxOffsetX;

	// Token: 0x0400622E RID: 25134
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxOffsetY;

	// Token: 0x0400622F RID: 25135
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxRotation;

	// Token: 0x04006230 RID: 25136
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxScale;

	// Token: 0x04006231 RID: 25137
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController cbxAspectRow;

	// Token: 0x04006232 RID: 25138
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxAspect;

	// Token: 0x04006233 RID: 25139
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController cbxShiftRow;

	// Token: 0x04006234 RID: 25140
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxShift;
}
