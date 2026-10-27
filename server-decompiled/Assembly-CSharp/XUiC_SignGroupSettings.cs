using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200105A RID: 4186
[Preserve]
public class XUiC_SignGroupSettings : XUiC_SignLayerSettings
{
	// Token: 0x0600845C RID: 33884 RVA: 0x003283C0 File Offset: 0x003265C0
	public override void Init()
	{
		base.Init();
		this.btnBakeColor = base.GetChildById("btnBakeColor").GetChildByType<XUiC_SimpleButton>();
		this.btnBakeColor.OnPressed += this.BtnBakeColor_OnPressed;
		this.btnBakeOffsets = base.GetChildById("btnBakeOffsets").GetChildByType<XUiC_SimpleButton>();
		this.btnBakeOffsets.OnPressed += this.BtnBakeOffsets_OnPressed;
		XUiController childById = base.GetChildById("btnAffectChildren");
		this.btnAffectChildren = ((childById != null) ? childById.GetChildByType<XUiC_SimpleButton>() : null);
		this.btnAffectChildren.OnPressed += delegate(XUiController sender, int button)
		{
			Action<XUiC_SignEditorWindow.AffectMode> onAffectModePressed = this.OnAffectModePressed;
			if (onAffectModePressed == null)
			{
				return;
			}
			onAffectModePressed(XUiC_SignEditorWindow.AffectMode.Children);
		};
		XUiController childById2 = base.GetChildById("btnAffectPivot");
		this.btnAffectPivot = ((childById2 != null) ? childById2.GetChildByType<XUiC_SimpleButton>() : null);
		this.btnAffectPivot.OnPressed += delegate(XUiController sender, int button)
		{
			Action<XUiC_SignEditorWindow.AffectMode> onAffectModePressed2 = this.OnAffectModePressed;
			if (onAffectModePressed2 == null)
			{
				return;
			}
			onAffectModePressed2(XUiC_SignEditorWindow.AffectMode.Pivot);
		};
		this.cbxColorMode = base.GetChildById("cbxColorMode").GetChildByType<XUiC_ComboBoxList<string>>();
		this.cbxColorMode.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Group Color Mode");
		};
		this.cbxOffsetTarget = base.GetChildById("cbxOffsetTarget").GetChildByType<XUiC_ComboBoxList<string>>();
		this.cbxOffsetTarget.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Group Offset Target");
		};
		this.cbxSoftnessOffset = (XUiC_ComboBoxFloat)base.GetChildById("cbxSoftnessOffset").GetChildById("value");
		this.cbxSoftnessOffset.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Group Softness Offset");
		};
		this.cbxDilateOffset = (XUiC_ComboBoxFloat)base.GetChildById("cbxDilateOffset").GetChildById("value");
		this.cbxDilateOffset.OnValueChangedGeneric += delegate(XUiController _)
		{
			this.OnValueChangedGeneric("Changed Group Dilate Offset");
		};
	}

	// Token: 0x0600845D RID: 33885 RVA: 0x00328560 File Offset: 0x00326760
	public void SetAffectModeVisual(XUiC_SignEditorWindow.AffectMode mode)
	{
		bool flag = mode == XUiC_SignEditorWindow.AffectMode.Children;
		bool flag2 = mode == XUiC_SignEditorWindow.AffectMode.Pivot;
		this.btnAffectChildren.Button.Selected = flag;
		this.btnAffectPivot.Button.Selected = flag2;
	}

	// Token: 0x0600845E RID: 33886 RVA: 0x0032859C File Offset: 0x0032679C
	public override void SetLayer(SignData.SignLayer layer)
	{
		this.currentLayer = layer as SignData.GroupSignLayer;
		if (this.currentLayer == null)
		{
			this.cbxColorMode.SelectedIndex = 0;
			this.cbxOffsetTarget.SelectedIndex = 0;
			this.cbxSoftnessOffset.Value = 0.0;
			this.cbxDilateOffset.Value = 0.0;
			return;
		}
		this.cbxColorMode.SelectedIndex = (int)this.currentLayer.colorMode;
		this.cbxOffsetTarget.SelectedIndex = (int)this.currentLayer.offsetTarget;
		this.cbxSoftnessOffset.Value = (double)this.currentLayer.softnessOffset;
		this.cbxDilateOffset.Value = (double)this.currentLayer.dilateOffset;
	}

	// Token: 0x0600845F RID: 33887 RVA: 0x00328658 File Offset: 0x00326858
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
		this.currentLayer.colorMode = (SignData.GroupSignLayer.ColorMode)this.cbxColorMode.SelectedIndex;
		this.currentLayer.offsetTarget = (SignData.GroupSignLayer.OffsetTarget)this.cbxOffsetTarget.SelectedIndex;
		this.currentLayer.softnessOffset = (float)this.cbxSoftnessOffset.Value;
		this.currentLayer.dilateOffset = (float)this.cbxDilateOffset.Value;
		Action onLayerSettingsChanged = this.OnLayerSettingsChanged;
		if (onLayerSettingsChanged == null)
		{
			return;
		}
		onLayerSettingsChanged();
	}

	// Token: 0x06008460 RID: 33888 RVA: 0x003286F0 File Offset: 0x003268F0
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnBakeColor_OnPressed(XUiController _sender, int _mouseButton)
	{
		if (this.currentLayer == null)
		{
			return;
		}
		Action<string, bool> onPreLayerSettingsChanged = this.OnPreLayerSettingsChanged;
		if (onPreLayerSettingsChanged != null)
		{
			onPreLayerSettingsChanged("Baked Group Color", false);
		}
		XUiC_SignGroupSettings.BakeColorRecursive(this.currentLayer, new List<SignDataManager.ColorOperation>());
		this.SetLayer(this.currentLayer);
		Action onLayerSettingsChanged = this.OnLayerSettingsChanged;
		if (onLayerSettingsChanged == null)
		{
			return;
		}
		onLayerSettingsChanged();
	}

	// Token: 0x06008461 RID: 33889 RVA: 0x0032874C File Offset: 0x0032694C
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnBakeOffsets_OnPressed(XUiController _sender, int _mouseButton)
	{
		if (this.currentLayer == null)
		{
			return;
		}
		Action<string, bool> onPreLayerSettingsChanged = this.OnPreLayerSettingsChanged;
		if (onPreLayerSettingsChanged != null)
		{
			onPreLayerSettingsChanged("Baked Group Offsets", false);
		}
		XUiC_SignGroupSettings.BakeOffsetsRecursive(this.currentLayer, default(SignDataManager.GroupOffsets));
		this.SetLayer(this.currentLayer);
		Action onLayerSettingsChanged = this.OnLayerSettingsChanged;
		if (onLayerSettingsChanged == null)
		{
			return;
		}
		onLayerSettingsChanged();
	}

	// Token: 0x06008462 RID: 33890 RVA: 0x003287AC File Offset: 0x003269AC
	[PublicizedFrom(EAccessModifier.Private)]
	public static void BakeColorRecursive(SignData.GroupSignLayer group, List<SignDataManager.ColorOperation> colorOps)
	{
		Color color = group.renderSettings.color;
		bool flag = (group.colorMode == SignData.GroupSignLayer.ColorMode.Multiply && color == Color.white) || (group.colorMode == SignData.GroupSignLayer.ColorMode.Blend && color.a == 0f);
		if (!flag)
		{
			colorOps.Add(new SignDataManager.ColorOperation
			{
				color = group.renderSettings.color,
				mode = group.colorMode
			});
		}
		foreach (SignData.SignLayer signLayer in group.layers)
		{
			SignData.GroupSignLayer groupSignLayer = signLayer as SignData.GroupSignLayer;
			if (groupSignLayer != null)
			{
				XUiC_SignGroupSettings.BakeColorRecursive(groupSignLayer, colorOps);
			}
			else
			{
				signLayer.renderSettings.color = SignDataManager.ColorOperation.EvaluateColor(signLayer.renderSettings.color, colorOps);
			}
		}
		if (!flag)
		{
			colorOps.RemoveAt(colorOps.Count - 1);
		}
		group.colorMode = SignData.GroupSignLayer.ColorMode.Multiply;
		group.renderSettings.color = Color.white;
	}

	// Token: 0x06008463 RID: 33891 RVA: 0x003288C8 File Offset: 0x00326AC8
	[PublicizedFrom(EAccessModifier.Private)]
	public static void BakeOffsetsRecursive(SignData.GroupSignLayer group, SignDataManager.GroupOffsets offsets)
	{
		offsets = offsets.WithOffsets(group.softnessOffset, group.dilateOffset, group.offsetTarget);
		foreach (SignData.SignLayer signLayer in group.layers)
		{
			SignData.GroupSignLayer groupSignLayer = signLayer as SignData.GroupSignLayer;
			if (groupSignLayer != null)
			{
				XUiC_SignGroupSettings.BakeOffsetsRecursive(groupSignLayer, offsets);
			}
			else
			{
				SignData.PolygonSignLayer polygonSignLayer = signLayer as SignData.PolygonSignLayer;
				if (polygonSignLayer == null)
				{
					SignData.TextSignLayer textSignLayer = signLayer as SignData.TextSignLayer;
					if (textSignLayer == null)
					{
						SignData.NoiseSignLayer noiseSignLayer = signLayer as SignData.NoiseSignLayer;
						if (noiseSignLayer != null)
						{
							noiseSignLayer.softness = Mathf.Max(0f, noiseSignLayer.softness + offsets.noiseSoftnessOffset);
							noiseSignLayer.dilate += offsets.noiseDilateOffset;
						}
					}
					else
					{
						textSignLayer.softness = Mathf.Clamp01(textSignLayer.softness + offsets.textSoftnessOffset);
						textSignLayer.dilate = Mathf.Clamp(textSignLayer.dilate + offsets.textDilateOffset, -1f, 1f);
					}
				}
				else
				{
					polygonSignLayer.softness = Mathf.Max(0f, polygonSignLayer.softness + offsets.shapeSoftnessOffset);
					polygonSignLayer.dilate += offsets.shapeDilateOffset;
				}
			}
		}
		group.offsetTarget = SignData.GroupSignLayer.OffsetTarget.All;
		group.softnessOffset = 0f;
		group.dilateOffset = 0f;
	}

	// Token: 0x04006243 RID: 25155
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnBakeColor;

	// Token: 0x04006244 RID: 25156
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnBakeOffsets;

	// Token: 0x04006245 RID: 25157
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnAffectChildren;

	// Token: 0x04006246 RID: 25158
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnAffectPivot;

	// Token: 0x04006247 RID: 25159
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> cbxColorMode;

	// Token: 0x04006248 RID: 25160
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> cbxOffsetTarget;

	// Token: 0x04006249 RID: 25161
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxSoftnessOffset;

	// Token: 0x0400624A RID: 25162
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxDilateOffset;

	// Token: 0x0400624B RID: 25163
	[PublicizedFrom(EAccessModifier.Private)]
	public SignData.GroupSignLayer currentLayer;

	// Token: 0x0400624C RID: 25164
	public Action<XUiC_SignEditorWindow.AffectMode> OnAffectModePressed;
}
