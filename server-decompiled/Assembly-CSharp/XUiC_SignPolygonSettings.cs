using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001024 RID: 4132
[Preserve]
public class XUiC_SignPolygonSettings : XUiC_SignLayerSettings
{
	// Token: 0x06008309 RID: 33545 RVA: 0x0031F140 File Offset: 0x0031D340
	public override void Init()
	{
		base.Init();
		this.cbxMode = base.GetChildById("cbxMode").GetChildByType<XUiC_ComboBoxList<string>>();
		this.cbxMode.OnValueChangedGeneric += this.OnModeChanged;
		this.cbxSides = (XUiC_ComboBoxInt)base.GetChildById("cbxSides").GetChildById("value");
		this.cbxSides.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Polygon Sides");
		};
		this.cbxSmoothness = (XUiC_ComboBoxFloat)base.GetChildById("cbxSmoothness").GetChildById("value");
		this.cbxSmoothness.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Polygon Smoothness");
		};
		this.cbxStarify = (XUiC_ComboBoxFloat)base.GetChildById("cbxStarify").GetChildById("value");
		this.cbxStarify.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Polygon Starify");
		};
		this.cbxSoftness = (XUiC_ComboBoxFloat)base.GetChildById("cbxSoftness").GetChildById("value");
		this.cbxSoftness.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Polygon Softness");
		};
		this.cbxDilate = (XUiC_ComboBoxFloat)base.GetChildById("cbxDilate").GetChildById("value");
		this.cbxDilate.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Polygon Dilation");
		};
		this.cbxFrequencyRow = base.GetChildById("cbxFrequency");
		this.cbxFrequency = (XUiC_ComboBoxFloat)this.cbxFrequencyRow.GetChildById("value");
		this.cbxFrequency.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Polygon Frequency");
		};
		this.defaultMaxSoftness = (float)this.cbxSoftness.Max;
		this.defaultMinDilate = (float)this.cbxDilate.Min;
		this.defaultMaxDilate = (float)this.cbxDilate.Max;
		SignData.PolygonSignLayer polygonSignLayer = new SignData.PolygonSignLayer();
		base.SetDefaultValue("cbxSides", polygonSignLayer.sides);
		base.SetDefaultValue("cbxSmoothness", polygonSignLayer.smoothness);
		base.SetDefaultValue("cbxStarify", polygonSignLayer.starify);
		base.SetDefaultValue("cbxSoftness", polygonSignLayer.softness);
		base.SetDefaultValue("cbxDilate", polygonSignLayer.dilate);
	}

	// Token: 0x0600830A RID: 33546 RVA: 0x0031F380 File Offset: 0x0031D580
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnModeChanged(XUiController _sender)
	{
		this.UpdateDefaultDilate();
		this.OnValueChangedGeneric("Changed Shape Mode");
	}

	// Token: 0x0600830B RID: 33547 RVA: 0x0031F394 File Offset: 0x0031D594
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateDefaultDilate()
	{
		if (this.currentLayer.softness != 0f)
		{
			return;
		}
		bool flag;
		switch (this.currentLayer.shapeMode)
		{
		case SignData.PolygonSignLayer.ShapeMode.Line:
			flag = this.currentLayer.dilate == 0.02f;
			goto IL_0074;
		case SignData.PolygonSignLayer.ShapeMode.Ripple:
			flag = this.currentLayer.dilate == 0.1f;
			goto IL_0074;
		}
		flag = this.currentLayer.dilate == 0f;
		IL_0074:
		if (!flag)
		{
			return;
		}
		switch ((byte)this.cbxMode.SelectedIndex)
		{
		case 2:
			this.cbxDilate.Value = 0.019999999552965164;
			return;
		case 3:
			this.cbxDilate.Value = 0.10000000149011612;
			return;
		}
		this.cbxDilate.Value = 0.0;
	}

	// Token: 0x0600830C RID: 33548 RVA: 0x0031F47C File Offset: 0x0031D67C
	public override void SetLayer(SignData.SignLayer layer)
	{
		this.currentLayer = layer as SignData.PolygonSignLayer;
		if (this.currentLayer == null)
		{
			this.cbxMode.SelectedIndex = 0;
			this.cbxSides.Value = 3L;
			this.cbxSmoothness.Value = 0.0;
			this.cbxStarify.Value = 0.0;
			this.cbxSoftness.Value = 0.0;
			this.cbxDilate.Value = 0.0;
			this.cbxFrequency.Value = 5.0;
			this.cbxFrequencyRow.ViewComponent.IsVisible = false;
			return;
		}
		this.cbxSoftness.Max = (double)Mathf.Max(this.currentLayer.softness, this.defaultMaxSoftness);
		this.cbxDilate.Min = (double)Mathf.Min(this.currentLayer.dilate, this.defaultMinDilate);
		this.cbxDilate.Max = (double)Mathf.Max(this.currentLayer.dilate, this.defaultMaxDilate);
		this.cbxMode.SelectedIndex = (int)this.currentLayer.shapeMode;
		this.cbxSides.Value = (long)this.currentLayer.sides;
		this.cbxSmoothness.Value = (double)this.currentLayer.smoothness;
		this.cbxStarify.Value = (double)this.currentLayer.starify;
		this.cbxSoftness.Value = (double)this.currentLayer.softness;
		this.cbxDilate.Value = (double)this.currentLayer.dilate;
		this.cbxFrequency.Value = (double)this.currentLayer.frequency;
		this.cbxFrequencyRow.ViewComponent.IsVisible = this.currentLayer.shapeMode == SignData.PolygonSignLayer.ShapeMode.Ripple;
	}

	// Token: 0x0600830D RID: 33549 RVA: 0x0031F654 File Offset: 0x0031D854
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
		this.currentLayer.shapeMode = (SignData.PolygonSignLayer.ShapeMode)this.cbxMode.SelectedIndex;
		this.currentLayer.sides = (int)this.cbxSides.Value;
		this.currentLayer.smoothness = (float)this.cbxSmoothness.Value;
		this.currentLayer.starify = (float)this.cbxStarify.Value;
		this.currentLayer.softness = (float)this.cbxSoftness.Value;
		this.currentLayer.dilate = (float)this.cbxDilate.Value;
		this.currentLayer.frequency = (float)this.cbxFrequency.Value;
		this.cbxFrequencyRow.ViewComponent.IsVisible = this.currentLayer.shapeMode == SignData.PolygonSignLayer.ShapeMode.Ripple;
		Action onLayerSettingsChanged = this.OnLayerSettingsChanged;
		if (onLayerSettingsChanged == null)
		{
			return;
		}
		onLayerSettingsChanged();
	}

	// Token: 0x04006115 RID: 24853
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> cbxMode;

	// Token: 0x04006116 RID: 24854
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxInt cbxSides;

	// Token: 0x04006117 RID: 24855
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxSmoothness;

	// Token: 0x04006118 RID: 24856
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxStarify;

	// Token: 0x04006119 RID: 24857
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxSoftness;

	// Token: 0x0400611A RID: 24858
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxDilate;

	// Token: 0x0400611B RID: 24859
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController cbxFrequencyRow;

	// Token: 0x0400611C RID: 24860
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxFrequency;

	// Token: 0x0400611D RID: 24861
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData.PolygonSignLayer currentLayer;

	// Token: 0x0400611E RID: 24862
	[PublicizedFrom(EAccessModifier.Private)]
	public float defaultMaxSoftness;

	// Token: 0x0400611F RID: 24863
	[PublicizedFrom(EAccessModifier.Private)]
	public float defaultMinDilate;

	// Token: 0x04006120 RID: 24864
	[PublicizedFrom(EAccessModifier.Private)]
	public float defaultMaxDilate;
}
