using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001020 RID: 4128
[Preserve]
public class XUiC_SignLayer : XUiC_SignGridEntry
{
	// Token: 0x17000F1D RID: 3869
	// (get) Token: 0x060082D2 RID: 33490 RVA: 0x0031DEC3 File Offset: 0x0031C0C3
	// (set) Token: 0x060082D3 RID: 33491 RVA: 0x0031DECB File Offset: 0x0031C0CB
	public GlobalSignId signId
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000F1E RID: 3870
	// (get) Token: 0x060082D4 RID: 33492 RVA: 0x0031DED4 File Offset: 0x0031C0D4
	// (set) Token: 0x060082D5 RID: 33493 RVA: 0x0031DEDC File Offset: 0x0031C0DC
	public SignData.SignLayer layer
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000F1F RID: 3871
	// (get) Token: 0x060082D6 RID: 33494 RVA: 0x0031DEE5 File Offset: 0x0031C0E5
	// (set) Token: 0x060082D7 RID: 33495 RVA: 0x0031DEED File Offset: 0x0031C0ED
	public int index
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000F20 RID: 3872
	// (get) Token: 0x060082D8 RID: 33496 RVA: 0x0031DEF8 File Offset: 0x0031C0F8
	public override bool IsSelectable
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			GlobalSignId signId = this.signId;
			return this.signId.IsValid || this.IsPlaceholder;
		}
	}

	// Token: 0x17000F21 RID: 3873
	// (get) Token: 0x060082D9 RID: 33497 RVA: 0x0031DF24 File Offset: 0x0031C124
	public override Color32 BackgroundColor
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.parentGrind == null || !this.parentGrind.MultiSelectedLayerIndices.Contains(this.index))
			{
				return XUiC_SignGridEntry.backgroundColor;
			}
			return this.multiSelectColor;
		}
	}

	// Token: 0x17000F22 RID: 3874
	// (get) Token: 0x060082DA RID: 33498 RVA: 0x0031DF52 File Offset: 0x0031C152
	public bool LayerValid
	{
		get
		{
			return this.layerValid;
		}
	}

	// Token: 0x17000F23 RID: 3875
	// (get) Token: 0x060082DB RID: 33499 RVA: 0x0031DF5A File Offset: 0x0031C15A
	// (set) Token: 0x060082DC RID: 33500 RVA: 0x0031DF62 File Offset: 0x0031C162
	public bool IsPlaceholder
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000F24 RID: 3876
	// (get) Token: 0x060082DD RID: 33501 RVA: 0x0031DF6B File Offset: 0x0031C16B
	public float LeftInsertX
	{
		get
		{
			return this.leftInsertPoint.ViewComponent.UiTransform.position.x;
		}
	}

	// Token: 0x17000F25 RID: 3877
	// (get) Token: 0x060082DE RID: 33502 RVA: 0x0031DF87 File Offset: 0x0031C187
	public float RightInsertX
	{
		get
		{
			return this.rightInsertPoint.ViewComponent.UiTransform.position.x;
		}
	}

	// Token: 0x060082DF RID: 33503 RVA: 0x0031DFA4 File Offset: 0x0031C1A4
	public override void Init()
	{
		base.Init();
		this.signMaterial = base.GetChildById("signMaterial").ViewComponent as XUiV_Texture;
		this.parentGrind = base.GetParentByType<XUiC_SignLayerGrid>();
		this.editorWindow = base.GetParentByType<XUiC_SignEditorWindow>();
		this.signMaterial.CreateMaterial("Game/SignTech/UI");
		XUiV_Texture xuiV_Texture = this.signMaterial;
		xuiV_Texture.OnRenderTexture = (UIDrawCall.OnRenderCallback)Delegate.Combine(xuiV_Texture.OnRenderTexture, new UIDrawCall.OnRenderCallback(this.OnWillRender));
		this.signMaterial.Texture = Texture2D.whiteTexture;
		XUiC_SignEditorWindow xuiC_SignEditorWindow = this.editorWindow;
		xuiC_SignEditorWindow.OnRefreshed = (Action)Delegate.Combine(xuiC_SignEditorWindow.OnRefreshed, new Action(this.RefreshLayerValidity));
		this.rightInsertPoint = base.GetChildById("rightInsertPoint");
		this.leftInsertPoint = base.GetChildById("leftInsertPoint");
	}

	// Token: 0x060082E0 RID: 33504 RVA: 0x0031E07A File Offset: 0x0031C27A
	[PublicizedFrom(EAccessModifier.Private)]
	public void RefreshLayerValidity()
	{
		this.layerValid = this.<RefreshLayerValidity>g__IsLayerValid|36_0(this.layer);
	}

	// Token: 0x060082E1 RID: 33505 RVA: 0x0031E090 File Offset: 0x0031C290
	public void SetAsPlaceholder(GlobalSignId signId, int index)
	{
		this.IsPlaceholder = true;
		this.signId = signId;
		this.layer = null;
		this.index = index;
		this.signMaterial.IsVisible = signId.IsValid;
		base.ViewComponent.Enabled = signId.IsValid;
		this.viewComponent.ToolTip = Localization.Get("lblSignAddLayer", false, null);
		base.RefreshBindings();
	}

	// Token: 0x060082E2 RID: 33506 RVA: 0x0031E0FC File Offset: 0x0031C2FC
	public void SetLayer(GlobalSignId signId, SignData.SignLayer layer, int index)
	{
		this.IsPlaceholder = false;
		this.signId = signId;
		this.layer = layer;
		this.index = index;
		this.signMaterial.IsVisible = signId.IsValid;
		base.ViewComponent.Enabled = signId.IsValid;
		this.RefreshTooltip();
		base.RefreshBindings();
		this.RefreshLayerValidity();
	}

	// Token: 0x060082E3 RID: 33507 RVA: 0x0031E15C File Offset: 0x0031C35C
	public void RefreshTooltip()
	{
		if (!this.signId.IsValid)
		{
			this.viewComponent.ToolTip = string.Empty;
			return;
		}
		if (string.IsNullOrEmpty(this.layer.name))
		{
			this.viewComponent.ToolTip = string.Format("{0} {1}", Localization.Get("xuiPaintLayer", false, null), (this.index + 1).ToString("00"));
			return;
		}
		this.viewComponent.ToolTip = this.layer.name;
	}

	// Token: 0x060082E4 RID: 33508 RVA: 0x0031E1E9 File Offset: 0x0031C3E9
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void HandleClick()
	{
		if (this.IsPlaceholder)
		{
			return;
		}
		Action<XUiC_SignLayer> onClicked = this.OnClicked;
		if (onClicked == null)
		{
			return;
		}
		onClicked(this);
	}

	// Token: 0x060082E5 RID: 33509 RVA: 0x0031E205 File Offset: 0x0031C405
	public override void OnCursorSelected(bool _isActualElement)
	{
		base.OnCursorSelected(_isActualElement);
		Action<XUiC_SignLayer> onBecameCursorSelected = this.OnBecameCursorSelected;
		if (onBecameCursorSelected == null)
		{
			return;
		}
		onBecameCursorSelected(this);
	}

	// Token: 0x060082E6 RID: 33510 RVA: 0x0031E220 File Offset: 0x0031C420
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnWillRender(Material mat)
	{
		if (this.IsPlaceholder)
		{
			SignDataManager.Instance.TryApplyRenderingData(this.signId, 1f, mat, null, SignUIStyle.AddLayer);
			return;
		}
		if (this.layer != null)
		{
			bool flag = this.layer.renderSettings.mode == SignData.SignRenderSettings.Mode.ColorOnly || this.layer.renderSettings.mode == SignData.SignRenderSettings.Mode.ColorAndMask;
			SignUIStyle signUIStyle;
			if (this.layerValid)
			{
				signUIStyle = (flag ? SignUIStyle.LayerThumbnail : SignUIStyle.MaskThumbnail);
			}
			else
			{
				signUIStyle = (flag ? SignUIStyle.LayerThumbnailInvalid : SignUIStyle.MaskThumbnailInvalid);
			}
			SignDataManager.Instance.TryApplyRenderingData(this.signId, 1f, mat, this.layer, signUIStyle);
		}
	}

	// Token: 0x060082E7 RID: 33511 RVA: 0x0031E2B8 File Offset: 0x0031C4B8
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetItemNameText(string name)
	{
		this.viewComponent.ToolTip = name;
	}

	// Token: 0x060082E8 RID: 33512 RVA: 0x0031E2C8 File Offset: 0x0031C4C8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		if (base.GetBindingValueInternal(ref value, bindingName))
		{
			return true;
		}
		if (bindingName == "layernumber")
		{
			value = (this.signId.IsValid ? (this.index + 1).ToString("00") : string.Empty);
			return true;
		}
		if (!(bindingName == "maskicon"))
		{
			if (!(bindingName == "showmaskicon"))
			{
				return false;
			}
			if (!this.signId.IsValid || this.layer == null)
			{
				value = false.ToString();
				return true;
			}
			value = (this.layer.renderSettings.mode > SignData.SignRenderSettings.Mode.ColorOnly).ToString();
			return true;
		}
		else
		{
			if (!this.signId.IsValid || this.layer == null)
			{
				value = "ui_game_symbol_lock";
				return true;
			}
			switch (this.layer.renderSettings.mode)
			{
			case SignData.SignRenderSettings.Mode.ColorOnly:
				value = "ui_game_symbol_lock";
				return true;
			case SignData.SignRenderSettings.Mode.ColorAndMask:
				value = "ui_game_symbol_signs_color_and_mask";
				return true;
			case SignData.SignRenderSettings.Mode.MaskOnly:
				value = "ui_game_symbol_signs_mask_only";
				return true;
			case SignData.SignRenderSettings.Mode.PunchOut:
				value = "ui_game_symbol_signs_punch_out";
				return true;
			default:
				return false;
			}
		}
	}

	// Token: 0x060082E9 RID: 33513 RVA: 0x0031E3F5 File Offset: 0x0031C5F5
	public override bool ParseAttribute(string name, string value)
	{
		if (base.ParseAttribute(name, value))
		{
			return true;
		}
		if (name == "multi_select_color")
		{
			this.multiSelectColor = StringParsers.ParseColor32(value);
			return true;
		}
		return false;
	}

	// Token: 0x060082EB RID: 33515 RVA: 0x0031E434 File Offset: 0x0031C634
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool <RefreshLayerValidity>g__IsLayerValid|36_0(SignData.SignLayer layer)
	{
		LayerComplexityInfo layerComplexityInfo;
		return layer != null && this.editorWindow.signComplexityInfo.IsValid && this.editorWindow.signComplexityInfo.TryGetLayerComplexityInfo(layer, out layerComplexityInfo) && layerComplexityInfo.MaxCompStackIndex <= 7 && layerComplexityInfo.MaxUVStackIndex <= 7;
	}

	// Token: 0x040060F4 RID: 24820
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Texture signMaterial;

	// Token: 0x040060F5 RID: 24821
	public Action<XUiC_SignLayer> OnBecameCursorSelected;

	// Token: 0x040060F6 RID: 24822
	public Action<XUiC_SignLayer> OnClicked;

	// Token: 0x040060FA RID: 24826
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 multiSelectColor;

	// Token: 0x040060FB RID: 24827
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignLayerGrid parentGrind;

	// Token: 0x040060FC RID: 24828
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignEditorWindow editorWindow;

	// Token: 0x040060FD RID: 24829
	[PublicizedFrom(EAccessModifier.Private)]
	public bool layerValid = true;

	// Token: 0x040060FF RID: 24831
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController leftInsertPoint;

	// Token: 0x04006100 RID: 24832
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController rightInsertPoint;
}
