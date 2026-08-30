using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001026 RID: 4134
[Preserve]
public class XUiC_SignColorSettings : XUiC_SignLayerSettings
{
	// Token: 0x0600831E RID: 33566 RVA: 0x0031FBB0 File Offset: 0x0031DDB0
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

	// Token: 0x0600831F RID: 33567 RVA: 0x0031FC24 File Offset: 0x0031DE24
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

	// Token: 0x06008320 RID: 33568 RVA: 0x0031FC81 File Offset: 0x0031DE81
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

	// Token: 0x06008321 RID: 33569 RVA: 0x0031FCBC File Offset: 0x0031DEBC
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

	// Token: 0x0400612A RID: 24874
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_AdvancedColorPicker colorPicker;

	// Token: 0x0400612B RID: 24875
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> cbxMaskMode;

	// Token: 0x0400612C RID: 24876
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData.SignLayer currentLayer;

	// Token: 0x0400612D RID: 24877
	public Action OnMaskModeChanged;
}
