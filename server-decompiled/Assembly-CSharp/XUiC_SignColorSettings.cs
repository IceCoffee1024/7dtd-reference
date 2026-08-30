using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200103F RID: 4159
[Preserve]
public class XUiC_SignColorSettings : XUiC_SignLayerSettings
{
	// Token: 0x06008410 RID: 33808 RVA: 0x003243D8 File Offset: 0x003225D8
	public override void Init()
	{
		base.Init();
		this.colorPicker = base.GetChildByType<XUiC_AdvancedColorPicker>();
		XUiC_AdvancedColorPicker xuiC_AdvancedColorPicker = this.colorPicker;
		xuiC_AdvancedColorPicker.OnColorChanged = (Action<Color>)Delegate.Combine(xuiC_AdvancedColorPicker.OnColorChanged, new Action<Color>(this.OnColorChangedHandler));
		this.cbxMaskMode = base.GetChildById("cbxMaskMode").GetChildByType<XUiC_ComboBoxList<string>>();
		this.cbxMaskMode.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Mask Mode");
			Action onMaskModeChanged = this.OnMaskModeChanged;
			if (onMaskModeChanged == null)
			{
				return;
			}
			onMaskModeChanged();
		};
	}

	// Token: 0x06008411 RID: 33809 RVA: 0x0032444C File Offset: 0x0032264C
	public override void SetLayer(SignData.SignLayer layer)
	{
		this.currentLayer = layer;
		XUiC_AdvancedColorPicker xuiC_AdvancedColorPicker = this.colorPicker;
		SignData.SignLayer signLayer = this.currentLayer;
		xuiC_AdvancedColorPicker.SetColor((signLayer != null) ? signLayer.renderSettings.color : Color.clear, false);
		XUiC_ComboBoxList<string> xuiC_ComboBoxList = this.cbxMaskMode;
		SignData.SignLayer signLayer2 = this.currentLayer;
		xuiC_ComboBoxList.SelectedIndex = (int)((signLayer2 != null) ? signLayer2.renderSettings.mode : SignData.SignRenderSettings.Mode.ColorOnly);
	}

	// Token: 0x06008412 RID: 33810 RVA: 0x003244A9 File Offset: 0x003226A9
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnColorChangedHandler(Color color)
	{
		Action<string, bool> onPreLayerSettingsChanged = this.OnPreLayerSettingsChanged;
		if (onPreLayerSettingsChanged != null)
		{
			onPreLayerSettingsChanged("Changed Color", false);
		}
		this.currentLayer.renderSettings.color = color;
		Action onLayerSettingsChanged = this.OnLayerSettingsChanged;
		if (onLayerSettingsChanged == null)
		{
			return;
		}
		onLayerSettingsChanged();
	}

	// Token: 0x06008413 RID: 33811 RVA: 0x003244E4 File Offset: 0x003226E4
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnValueChangedGeneric(string _changeDescription)
	{
		Action<string, bool> onPreLayerSettingsChanged = this.OnPreLayerSettingsChanged;
		if (onPreLayerSettingsChanged != null)
		{
			onPreLayerSettingsChanged(_changeDescription, false);
		}
		this.currentLayer.renderSettings.mode = (SignData.SignRenderSettings.Mode)this.cbxMaskMode.SelectedIndex;
		Action onLayerSettingsChanged = this.OnLayerSettingsChanged;
		if (onLayerSettingsChanged == null)
		{
			return;
		}
		onLayerSettingsChanged();
	}

	// Token: 0x040061F1 RID: 25073
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_AdvancedColorPicker colorPicker;

	// Token: 0x040061F2 RID: 25074
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> cbxMaskMode;

	// Token: 0x040061F3 RID: 25075
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData.SignLayer currentLayer;

	// Token: 0x040061F4 RID: 25076
	public Action OnMaskModeChanged;
}
