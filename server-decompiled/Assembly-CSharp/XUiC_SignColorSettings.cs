using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200104E RID: 4174
[Preserve]
public class XUiC_SignColorSettings : XUiC_SignLayerSettings
{
	// Token: 0x06008439 RID: 33849 RVA: 0x00327DC0 File Offset: 0x00325FC0
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

	// Token: 0x0600843A RID: 33850 RVA: 0x00327E34 File Offset: 0x00326034
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

	// Token: 0x0600843B RID: 33851 RVA: 0x00327E91 File Offset: 0x00326091
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

	// Token: 0x0600843C RID: 33852 RVA: 0x00327ECC File Offset: 0x003260CC
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

	// Token: 0x0400622C RID: 25132
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_AdvancedColorPicker colorPicker;

	// Token: 0x0400622D RID: 25133
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> cbxMaskMode;

	// Token: 0x0400622E RID: 25134
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData.SignLayer currentLayer;

	// Token: 0x0400622F RID: 25135
	public Action OnMaskModeChanged;
}
