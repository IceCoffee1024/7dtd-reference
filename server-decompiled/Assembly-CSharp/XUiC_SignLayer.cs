using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001039 RID: 4153
[Preserve]
public class XUiC_SignLayer : XUiC_SignGridEntry
{
	// Token: 0x17000F56 RID: 3926
	// (get) Token: 0x060083C4 RID: 33732 RVA: 0x003226EB File Offset: 0x003208EB
	// (set) Token: 0x060083C5 RID: 33733 RVA: 0x003226F3 File Offset: 0x003208F3
	public GlobalSignId signId
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000F57 RID: 3927
	// (get) Token: 0x060083C6 RID: 33734 RVA: 0x003226FC File Offset: 0x003208FC
	// (set) Token: 0x060083C7 RID: 33735 RVA: 0x00322704 File Offset: 0x00320904
	public SignData.SignLayer layer
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000F58 RID: 3928
	// (get) Token: 0x060083C8 RID: 33736 RVA: 0x0032270D File Offset: 0x0032090D
	// (set) Token: 0x060083C9 RID: 33737 RVA: 0x00322715 File Offset: 0x00320915
	public int index
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000F59 RID: 3929
	// (get) Token: 0x060083CA RID: 33738 RVA: 0x00322720 File Offset: 0x00320920
	public override bool IsSelectable
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			GlobalSignId signId = this.signId;
			return this.signId.IsValid || this.IsPlaceholder;
		}
	}

	// Token: 0x17000F5A RID: 3930
	// (get) Token: 0x060083CB RID: 33739 RVA: 0x0032274C File Offset: 0x0032094C
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

	// Token: 0x17000F5B RID: 3931
	// (get) Token: 0x060083CC RID: 33740 RVA: 0x0032277A File Offset: 0x0032097A
	public bool LayerValid
	{
		get
		{
			return this.layerValid;
		}
	}

	// Token: 0x17000F5C RID: 3932
	// (get) Token: 0x060083CD RID: 33741 RVA: 0x00322782 File Offset: 0x00320982
	// (set) Token: 0x060083CE RID: 33742 RVA: 0x0032278A File Offset: 0x0032098A
	public bool IsPlaceholder
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000F5D RID: 3933
	// (get) Token: 0x060083CF RID: 33743 RVA: 0x00322793 File Offset: 0x00320993
	public float LeftInsertX
	{
		get
		{
			return this.leftInsertPoint.ViewComponent.UiTransform.position.x;
		}
	}

	// Token: 0x17000F5E RID: 3934
	// (get) Token: 0x060083D0 RID: 33744 RVA: 0x003227AF File Offset: 0x003209AF
	public float RightInsertX
	{
		get
		{
			return this.rightInsertPoint.ViewComponent.UiTransform.position.x;
		}
	}

	// Token: 0x060083D1 RID: 33745 RVA: 0x003227CC File Offset: 0x003209CC
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

	// Token: 0x060083D2 RID: 33746 RVA: 0x003228A2 File Offset: 0x00320AA2
	[PublicizedFrom(EAccessModifier.Private)]
	public void RefreshLayerValidity()
	{
		this.layerValid = this.<RefreshLayerValidity>g__IsLayerValid|36_0(this.layer);
	}

	// Token: 0x060083D3 RID: 33747 RVA: 0x003228B8 File Offset: 0x00320AB8
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

	// Token: 0x060083D4 RID: 33748 RVA: 0x00322924 File Offset: 0x00320B24
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

	// Token: 0x060083D5 RID: 33749 RVA: 0x00322984 File Offset: 0x00320B84
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

	// Token: 0x060083D6 RID: 33750 RVA: 0x00322A11 File Offset: 0x00320C11
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

	// Token: 0x060083D7 RID: 33751 RVA: 0x00322A2D File Offset: 0x00320C2D
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

	// Token: 0x060083D8 RID: 33752 RVA: 0x00322A48 File Offset: 0x00320C48
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

	// Token: 0x060083D9 RID: 33753 RVA: 0x00322AE0 File Offset: 0x00320CE0
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetItemNameText(string name)
	{
		this.viewComponent.ToolTip = name;
	}

	// Token: 0x060083DA RID: 33754 RVA: 0x00322AF0 File Offset: 0x00320CF0
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

	// Token: 0x060083DB RID: 33755 RVA: 0x00322C1D File Offset: 0x00320E1D
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

	// Token: 0x060083DD RID: 33757 RVA: 0x00322C5C File Offset: 0x00320E5C
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool <RefreshLayerValidity>g__IsLayerValid|36_0(SignData.SignLayer layer)
	{
		LayerComplexityInfo layerComplexityInfo;
		return layer != null && this.editorWindow.signComplexityInfo.IsValid && this.editorWindow.signComplexityInfo.TryGetLayerComplexityInfo(layer, out layerComplexityInfo) && layerComplexityInfo.MaxCompStackIndex <= 7 && layerComplexityInfo.MaxUVStackIndex <= 7;
	}

	// Token: 0x040061BB RID: 25019
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Texture signMaterial;

	// Token: 0x040061BC RID: 25020
	public Action<XUiC_SignLayer> OnBecameCursorSelected;

	// Token: 0x040061BD RID: 25021
	public Action<XUiC_SignLayer> OnClicked;

	// Token: 0x040061C1 RID: 25025
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 multiSelectColor;

	// Token: 0x040061C2 RID: 25026
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignLayerGrid parentGrind;

	// Token: 0x040061C3 RID: 25027
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignEditorWindow editorWindow;

	// Token: 0x040061C4 RID: 25028
	[PublicizedFrom(EAccessModifier.Private)]
	public bool layerValid = true;

	// Token: 0x040061C6 RID: 25030
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController leftInsertPoint;

	// Token: 0x040061C7 RID: 25031
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController rightInsertPoint;
}
