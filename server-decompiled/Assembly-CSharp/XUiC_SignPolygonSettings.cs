using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200103D RID: 4157
[Preserve]
public class XUiC_SignPolygonSettings : XUiC_SignLayerSettings
{
	// Token: 0x060083FB RID: 33787 RVA: 0x00323968 File Offset: 0x00321B68
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

	// Token: 0x060083FC RID: 33788 RVA: 0x00323BA8 File Offset: 0x00321DA8
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnModeChanged(XUiController _sender)
	{
		this.UpdateDefaultDilate();
		this.OnValueChangedGeneric("Changed Shape Mode");
	}

	// Token: 0x060083FD RID: 33789 RVA: 0x00323BBC File Offset: 0x00321DBC
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

	// Token: 0x060083FE RID: 33790 RVA: 0x00323CA4 File Offset: 0x00321EA4
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

	// Token: 0x060083FF RID: 33791 RVA: 0x00323E7C File Offset: 0x0032207C
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

	// Token: 0x040061DC RID: 25052
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> cbxMode;

	// Token: 0x040061DD RID: 25053
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxInt cbxSides;

	// Token: 0x040061DE RID: 25054
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxSmoothness;

	// Token: 0x040061DF RID: 25055
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxStarify;

	// Token: 0x040061E0 RID: 25056
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxSoftness;

	// Token: 0x040061E1 RID: 25057
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxDilate;

	// Token: 0x040061E2 RID: 25058
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController cbxFrequencyRow;

	// Token: 0x040061E3 RID: 25059
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxFrequency;

	// Token: 0x040061E4 RID: 25060
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData.PolygonSignLayer currentLayer;

	// Token: 0x040061E5 RID: 25061
	[PublicizedFrom(EAccessModifier.Private)]
	public float defaultMaxSoftness;

	// Token: 0x040061E6 RID: 25062
	[PublicizedFrom(EAccessModifier.Private)]
	public float defaultMinDilate;

	// Token: 0x040061E7 RID: 25063
	[PublicizedFrom(EAccessModifier.Private)]
	public float defaultMaxDilate;
}
