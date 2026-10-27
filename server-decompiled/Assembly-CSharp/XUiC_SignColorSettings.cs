using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200105E RID: 4190
[Preserve]
public class XUiC_SignColorSettings : XUiC_SignLayerSettings
{
	// Token: 0x0600848B RID: 33931 RVA: 0x00329AC4 File Offset: 0x00327CC4
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

	// Token: 0x0600848C RID: 33932 RVA: 0x00329B38 File Offset: 0x00327D38
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

	// Token: 0x0600848D RID: 33933 RVA: 0x00329B95 File Offset: 0x00327D95
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

	// Token: 0x0600848E RID: 33934 RVA: 0x00329BD0 File Offset: 0x00327DD0
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

	// Token: 0x0400626A RID: 25194
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_AdvancedColorPicker colorPicker;

	// Token: 0x0400626B RID: 25195
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> cbxMaskMode;

	// Token: 0x0400626C RID: 25196
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData.SignLayer currentLayer;

	// Token: 0x0400626D RID: 25197
	public Action OnMaskModeChanged;
}
