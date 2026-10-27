using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200105D RID: 4189
[Preserve]
public class XUiC_SignNoiseSettings : XUiC_SignLayerSettings
{
	// Token: 0x06008482 RID: 33922 RVA: 0x003296B0 File Offset: 0x003278B0
	public override void Init()
	{
		base.Init();
		this.cbxSeed = (XUiC_ComboBoxInt)base.GetChildById("cbxSeed").GetChildById("value");
		this.cbxSeed.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Noise Seed");
		};
		this.cbxDetail = (XUiC_ComboBoxInt)base.GetChildById("cbxDetail").GetChildById("value");
		this.cbxDetail.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Noise Detail");
		};
		this.cbxSoftness = (XUiC_ComboBoxFloat)base.GetChildById("cbxSoftness").GetChildById("value");
		this.cbxSoftness.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Noise Softness");
		};
		this.cbxDilate = (XUiC_ComboBoxFloat)base.GetChildById("cbxDilate").GetChildById("value");
		this.cbxDilate.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Noise Dilation");
		};
		this.cbxFade = (XUiC_ComboBoxFloat)base.GetChildById("cbxFade").GetChildById("value");
		this.cbxFade.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Noise Fade");
		};
		this.defaultMaxSoftness = (float)this.cbxSoftness.Max;
		this.defaultMinDilate = (float)this.cbxDilate.Min;
		this.defaultMaxDilate = (float)this.cbxDilate.Max;
		SignData.NoiseSignLayer noiseSignLayer = new SignData.NoiseSignLayer();
		base.SetDefaultValue("cbxSeed", noiseSignLayer.seed);
		base.SetDefaultValue("cbxDetail", noiseSignLayer.detail);
		base.SetDefaultValue("cbxSoftness", noiseSignLayer.softness);
		base.SetDefaultValue("cbxDilate", noiseSignLayer.dilate);
		base.SetDefaultValue("cbxFade", noiseSignLayer.fade);
	}

	// Token: 0x06008483 RID: 33923 RVA: 0x00329880 File Offset: 0x00327A80
	public override void SetLayer(SignData.SignLayer layer)
	{
		this.currentLayer = layer as SignData.NoiseSignLayer;
		if (this.currentLayer == null)
		{
			this.cbxSeed.Value = 0L;
			this.cbxDetail.Value = 1L;
			this.cbxSoftness.Value = 0.0;
			this.cbxDilate.Value = 0.0;
			this.cbxFade.Value = 0.0;
			return;
		}
		this.cbxSoftness.Max = (double)Mathf.Max(this.currentLayer.softness, this.defaultMaxSoftness);
		this.cbxDilate.Min = (double)Mathf.Min(this.currentLayer.dilate, this.defaultMinDilate);
		this.cbxDilate.Max = (double)Mathf.Max(this.currentLayer.dilate, this.defaultMaxDilate);
		this.cbxSeed.Value = (long)this.currentLayer.seed;
		this.cbxDetail.Value = (long)this.currentLayer.detail;
		this.cbxSoftness.Value = (double)this.currentLayer.softness;
		this.cbxDilate.Value = (double)this.currentLayer.dilate;
		this.cbxFade.Value = (double)this.currentLayer.fade;
	}

	// Token: 0x06008484 RID: 33924 RVA: 0x003299D4 File Offset: 0x00327BD4
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
		this.currentLayer.seed = (int)this.cbxSeed.Value;
		this.currentLayer.detail = (int)this.cbxDetail.Value;
		this.currentLayer.softness = (float)this.cbxSoftness.Value;
		this.currentLayer.dilate = (float)this.cbxDilate.Value;
		this.currentLayer.fade = (float)this.cbxFade.Value;
		Action onLayerSettingsChanged = this.OnLayerSettingsChanged;
		if (onLayerSettingsChanged == null)
		{
			return;
		}
		onLayerSettingsChanged();
	}

	// Token: 0x04006261 RID: 25185
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxInt cbxSeed;

	// Token: 0x04006262 RID: 25186
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxInt cbxDetail;

	// Token: 0x04006263 RID: 25187
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxSoftness;

	// Token: 0x04006264 RID: 25188
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxDilate;

	// Token: 0x04006265 RID: 25189
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxFade;

	// Token: 0x04006266 RID: 25190
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData.NoiseSignLayer currentLayer;

	// Token: 0x04006267 RID: 25191
	[PublicizedFrom(EAccessModifier.Private)]
	public float defaultMaxSoftness;

	// Token: 0x04006268 RID: 25192
	[PublicizedFrom(EAccessModifier.Private)]
	public float defaultMinDilate;

	// Token: 0x04006269 RID: 25193
	[PublicizedFrom(EAccessModifier.Private)]
	public float defaultMaxDilate;
}
