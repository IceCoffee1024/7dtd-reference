using System;
using UnityEngine.Scripting;

// Token: 0x0200105B RID: 4187
[Preserve]
public class XUiC_SignTextSettings : XUiC_SignLayerSettings
{
	// Token: 0x0600846B RID: 33899 RVA: 0x00328A98 File Offset: 0x00326C98
	public override void Init()
	{
		base.Init();
		this.txtText = (XUiC_TextInput)base.GetChildById("txtText").GetChildById("value");
		this.txtText.OnChangeHandler += this.TxtText_OnChangeHandler;
		this.cbxFont = (XUiC_ComboBoxList<string>)base.GetChildById("cbxFont").GetChildById("value");
		this.cbxFont.OnValueChanged += this.CbxFont_OnValueChanged;
		this.cbxDirection = (XUiC_ComboBoxFloat)base.GetChildById("cbxDirection").GetChildById("value");
		this.cbxDirection.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Text Direction");
		};
		this.cbxSpacing = (XUiC_ComboBoxFloat)base.GetChildById("cbxSpacing").GetChildById("value");
		this.cbxSpacing.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Text Spacing");
		};
		this.cbxSoftness = (XUiC_ComboBoxFloat)base.GetChildById("cbxSoftness").GetChildById("value");
		this.cbxSoftness.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Text Softness");
		};
		this.cbxDilate = (XUiC_ComboBoxFloat)base.GetChildById("cbxDilate").GetChildById("value");
		this.cbxDilate.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Text Dilation");
		};
		SignData.TextSignLayer textSignLayer = new SignData.TextSignLayer();
		base.SetDefaultValue("cbxDirection", textSignLayer.direction);
		base.SetDefaultValue("cbxSpacing", textSignLayer.spacing);
		base.SetDefaultValue("cbxSoftness", textSignLayer.softness);
		base.SetDefaultValue("cbxDilate", textSignLayer.dilate);
		this.PopulateFontList();
	}

	// Token: 0x0600846C RID: 33900 RVA: 0x00328C5C File Offset: 0x00326E5C
	public override void SetLayer(SignData.SignLayer layer)
	{
		this.currentLayer = layer as SignData.TextSignLayer;
		if (this.currentLayer == null)
		{
			this.txtText.Text = string.Empty;
			this.cbxDirection.Value = 0.0;
			this.cbxSpacing.Value = 0.0;
			this.cbxSoftness.Value = 0.0;
			this.cbxDilate.Value = 0.0;
			return;
		}
		this.txtText.Text = this.currentLayer.text;
		int num = this.cbxFont.Elements.IndexOf(this.currentLayer.font);
		if (num < 0 || num > this.cbxFont.Elements.Count - 1)
		{
			Log.Error("Error reading TextSignLayer data: could not find matching font with name \"" + this.currentLayer.font + "\".");
			this.cbxFont.MaxIndex = this.cbxFont.Elements.Count - 1;
			this.cbxFont.SelectedIndex = this.cbxFont.MaxIndex;
		}
		else
		{
			this.cbxFont.SelectedIndex = num;
			this.cbxFont.MaxIndex = this.cbxFont.Elements.Count - 2;
		}
		this.cbxDirection.Value = (double)this.currentLayer.direction;
		this.cbxSpacing.Value = (double)this.currentLayer.spacing;
		this.cbxSoftness.Value = (double)this.currentLayer.softness;
		this.cbxDilate.Value = (double)this.currentLayer.dilate;
	}

	// Token: 0x0600846D RID: 33901 RVA: 0x00328E04 File Offset: 0x00327004
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnValueChangedGeneric(string _changeDescription)
	{
		if (this.currentLayer == null)
		{
			return;
		}
		Action<string, bool> onPreLayerSettingsChanged = this.OnPreLayerSettingsChanged;
		if (onPreLayerSettingsChanged != null)
		{
			onPreLayerSettingsChanged(_changeDescription, false);
		}
		this.currentLayer.direction = (float)this.cbxDirection.Value;
		this.currentLayer.spacing = (float)this.cbxSpacing.Value;
		this.currentLayer.softness = (float)this.cbxSoftness.Value;
		this.currentLayer.dilate = (float)this.cbxDilate.Value;
		Action onLayerSettingsChanged = this.OnLayerSettingsChanged;
		if (onLayerSettingsChanged == null)
		{
			return;
		}
		onLayerSettingsChanged();
	}

	// Token: 0x0600846E RID: 33902 RVA: 0x00328E9C File Offset: 0x0032709C
	[PublicizedFrom(EAccessModifier.Private)]
	public void TxtText_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		if (this.currentLayer == null)
		{
			return;
		}
		if (_changeFromCode)
		{
			return;
		}
		Action<string, bool> onPreLayerSettingsChanged = this.OnPreLayerSettingsChanged;
		if (onPreLayerSettingsChanged != null)
		{
			onPreLayerSettingsChanged("Changed Text Content", false);
		}
		this.currentLayer.text = _text;
		Action onLayerSettingsChanged = this.OnLayerSettingsChanged;
		if (onLayerSettingsChanged == null)
		{
			return;
		}
		onLayerSettingsChanged();
	}

	// Token: 0x0600846F RID: 33903 RVA: 0x00328EEC File Offset: 0x003270EC
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxFont_OnValueChanged(XUiController _sender, string _oldValue, string _newValue)
	{
		if (this.currentLayer == null)
		{
			return;
		}
		Action<string, bool> onPreLayerSettingsChanged = this.OnPreLayerSettingsChanged;
		if (onPreLayerSettingsChanged != null)
		{
			onPreLayerSettingsChanged("Changed Text Font", false);
		}
		this.cbxFont.MaxIndex = this.cbxFont.Elements.Count - ((_newValue == this.UnknownFontString) ? 1 : 2);
		this.currentLayer.font = _newValue;
		Action onLayerSettingsChanged = this.OnLayerSettingsChanged;
		if (onLayerSettingsChanged == null)
		{
			return;
		}
		onLayerSettingsChanged();
	}

	// Token: 0x06008470 RID: 33904 RVA: 0x00328F64 File Offset: 0x00327164
	[PublicizedFrom(EAccessModifier.Private)]
	public void PopulateFontList()
	{
		this.cbxFont.Elements.Clear();
		foreach (string text in SignDataManager.Instance.FontNames)
		{
			this.cbxFont.Elements.Add(text);
		}
		this.cbxFont.Elements.Add(this.UnknownFontString);
		this.cbxFont.MaxIndex = this.cbxFont.Elements.Count - 2;
	}

	// Token: 0x0400624D RID: 25165
	[PublicizedFrom(EAccessModifier.Private)]
	public string UnknownFontString = Localization.Get("lblSignFontUnknown", false, null);

	// Token: 0x0400624E RID: 25166
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtText;

	// Token: 0x0400624F RID: 25167
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> cbxFont;

	// Token: 0x04006250 RID: 25168
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxDirection;

	// Token: 0x04006251 RID: 25169
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxSpacing;

	// Token: 0x04006252 RID: 25170
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxSoftness;

	// Token: 0x04006253 RID: 25171
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxDilate;

	// Token: 0x04006254 RID: 25172
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData.TextSignLayer currentLayer;
}
