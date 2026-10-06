using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001048 RID: 4168
[Preserve]
public class XUiC_SignLayer : XUiC_SignGridEntry
{
	// Token: 0x17000F83 RID: 3971
	// (get) Token: 0x060083ED RID: 33773 RVA: 0x003260D3 File Offset: 0x003242D3
	// (set) Token: 0x060083EE RID: 33774 RVA: 0x003260DB File Offset: 0x003242DB
	public GlobalSignId signId
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000F84 RID: 3972
	// (get) Token: 0x060083EF RID: 33775 RVA: 0x003260E4 File Offset: 0x003242E4
	// (set) Token: 0x060083F0 RID: 33776 RVA: 0x003260EC File Offset: 0x003242EC
	public SignData.SignLayer layer
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000F85 RID: 3973
	// (get) Token: 0x060083F1 RID: 33777 RVA: 0x003260F5 File Offset: 0x003242F5
	// (set) Token: 0x060083F2 RID: 33778 RVA: 0x003260FD File Offset: 0x003242FD
	public int index
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000F86 RID: 3974
	// (get) Token: 0x060083F3 RID: 33779 RVA: 0x00326108 File Offset: 0x00324308
	public override bool IsSelectable
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			GlobalSignId signId = this.signId;
			return this.signId.IsValid || this.IsPlaceholder;
		}
	}

	// Token: 0x17000F87 RID: 3975
	// (get) Token: 0x060083F4 RID: 33780 RVA: 0x00326134 File Offset: 0x00324334
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

	// Token: 0x17000F88 RID: 3976
	// (get) Token: 0x060083F5 RID: 33781 RVA: 0x00326162 File Offset: 0x00324362
	public bool LayerValid
	{
		get
		{
			return this.layerValid;
		}
	}

	// Token: 0x17000F89 RID: 3977
	// (get) Token: 0x060083F6 RID: 33782 RVA: 0x0032616A File Offset: 0x0032436A
	// (set) Token: 0x060083F7 RID: 33783 RVA: 0x00326172 File Offset: 0x00324372
	public bool IsPlaceholder
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000F8A RID: 3978
	// (get) Token: 0x060083F8 RID: 33784 RVA: 0x0032617B File Offset: 0x0032437B
	public float LeftInsertX
	{
		get
		{
			return this.leftInsertPoint.ViewComponent.UiTransform.position.x;
		}
	}

	// Token: 0x17000F8B RID: 3979
	// (get) Token: 0x060083F9 RID: 33785 RVA: 0x00326197 File Offset: 0x00324397
	public float RightInsertX
	{
		get
		{
			return this.rightInsertPoint.ViewComponent.UiTransform.position.x;
		}
	}

	// Token: 0x060083FA RID: 33786 RVA: 0x003261B4 File Offset: 0x003243B4
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

	// Token: 0x060083FB RID: 33787 RVA: 0x0032628A File Offset: 0x0032448A
	[PublicizedFrom(EAccessModifier.Private)]
	public void RefreshLayerValidity()
	{
		this.layerValid = this.<RefreshLayerValidity>g__IsLayerValid|36_0(this.layer);
	}

	// Token: 0x060083FC RID: 33788 RVA: 0x003262A0 File Offset: 0x003244A0
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

	// Token: 0x060083FD RID: 33789 RVA: 0x0032630C File Offset: 0x0032450C
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

	// Token: 0x060083FE RID: 33790 RVA: 0x0032636C File Offset: 0x0032456C
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

	// Token: 0x060083FF RID: 33791 RVA: 0x003263F9 File Offset: 0x003245F9
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

	// Token: 0x06008400 RID: 33792 RVA: 0x00326415 File Offset: 0x00324615
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

	// Token: 0x06008401 RID: 33793 RVA: 0x00326430 File Offset: 0x00324630
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

	// Token: 0x06008402 RID: 33794 RVA: 0x003264C8 File Offset: 0x003246C8
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetItemNameText(string name)
	{
		this.viewComponent.ToolTip = name;
	}

	// Token: 0x06008403 RID: 33795 RVA: 0x003264D8 File Offset: 0x003246D8
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

	// Token: 0x06008404 RID: 33796 RVA: 0x00326605 File Offset: 0x00324805
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

	// Token: 0x06008406 RID: 33798 RVA: 0x00326644 File Offset: 0x00324844
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool <RefreshLayerValidity>g__IsLayerValid|36_0(SignData.SignLayer layer)
	{
		LayerComplexityInfo layerComplexityInfo;
		return layer != null && this.editorWindow.signComplexityInfo.IsValid && this.editorWindow.signComplexityInfo.TryGetLayerComplexityInfo(layer, out layerComplexityInfo) && layerComplexityInfo.MaxCompStackIndex <= 7 && layerComplexityInfo.MaxUVStackIndex <= 7;
	}

	// Token: 0x040061F6 RID: 25078
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Texture signMaterial;

	// Token: 0x040061F7 RID: 25079
	public Action<XUiC_SignLayer> OnBecameCursorSelected;

	// Token: 0x040061F8 RID: 25080
	public Action<XUiC_SignLayer> OnClicked;

	// Token: 0x040061FC RID: 25084
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 multiSelectColor;

	// Token: 0x040061FD RID: 25085
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignLayerGrid parentGrind;

	// Token: 0x040061FE RID: 25086
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignEditorWindow editorWindow;

	// Token: 0x040061FF RID: 25087
	[PublicizedFrom(EAccessModifier.Private)]
	public bool layerValid = true;

	// Token: 0x04006201 RID: 25089
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController leftInsertPoint;

	// Token: 0x04006202 RID: 25090
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController rightInsertPoint;
}
