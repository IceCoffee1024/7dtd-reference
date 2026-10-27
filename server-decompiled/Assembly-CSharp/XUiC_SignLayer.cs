using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001058 RID: 4184
[Preserve]
public class XUiC_SignLayer : XUiC_SignGridEntry
{
	// Token: 0x17000F8E RID: 3982
	// (get) Token: 0x0600843F RID: 33855 RVA: 0x00327DD7 File Offset: 0x00325FD7
	// (set) Token: 0x06008440 RID: 33856 RVA: 0x00327DDF File Offset: 0x00325FDF
	public GlobalSignId signId
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000F8F RID: 3983
	// (get) Token: 0x06008441 RID: 33857 RVA: 0x00327DE8 File Offset: 0x00325FE8
	// (set) Token: 0x06008442 RID: 33858 RVA: 0x00327DF0 File Offset: 0x00325FF0
	public SignData.SignLayer layer
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000F90 RID: 3984
	// (get) Token: 0x06008443 RID: 33859 RVA: 0x00327DF9 File Offset: 0x00325FF9
	// (set) Token: 0x06008444 RID: 33860 RVA: 0x00327E01 File Offset: 0x00326001
	public int index
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000F91 RID: 3985
	// (get) Token: 0x06008445 RID: 33861 RVA: 0x00327E0C File Offset: 0x0032600C
	public override bool IsSelectable
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			GlobalSignId signId = this.signId;
			return this.signId.IsValid || this.IsPlaceholder;
		}
	}

	// Token: 0x17000F92 RID: 3986
	// (get) Token: 0x06008446 RID: 33862 RVA: 0x00327E38 File Offset: 0x00326038
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

	// Token: 0x17000F93 RID: 3987
	// (get) Token: 0x06008447 RID: 33863 RVA: 0x00327E66 File Offset: 0x00326066
	public bool LayerValid
	{
		get
		{
			return this.layerValid;
		}
	}

	// Token: 0x17000F94 RID: 3988
	// (get) Token: 0x06008448 RID: 33864 RVA: 0x00327E6E File Offset: 0x0032606E
	// (set) Token: 0x06008449 RID: 33865 RVA: 0x00327E76 File Offset: 0x00326076
	public bool IsPlaceholder
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000F95 RID: 3989
	// (get) Token: 0x0600844A RID: 33866 RVA: 0x00327E7F File Offset: 0x0032607F
	public float LeftInsertX
	{
		get
		{
			return this.leftInsertPoint.ViewComponent.UiTransform.position.x;
		}
	}

	// Token: 0x17000F96 RID: 3990
	// (get) Token: 0x0600844B RID: 33867 RVA: 0x00327E9B File Offset: 0x0032609B
	public float RightInsertX
	{
		get
		{
			return this.rightInsertPoint.ViewComponent.UiTransform.position.x;
		}
	}

	// Token: 0x0600844C RID: 33868 RVA: 0x00327EB8 File Offset: 0x003260B8
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

	// Token: 0x0600844D RID: 33869 RVA: 0x00327F8E File Offset: 0x0032618E
	[PublicizedFrom(EAccessModifier.Private)]
	public void RefreshLayerValidity()
	{
		this.layerValid = this.<RefreshLayerValidity>g__IsLayerValid|36_0(this.layer);
	}

	// Token: 0x0600844E RID: 33870 RVA: 0x00327FA4 File Offset: 0x003261A4
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

	// Token: 0x0600844F RID: 33871 RVA: 0x00328010 File Offset: 0x00326210
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

	// Token: 0x06008450 RID: 33872 RVA: 0x00328070 File Offset: 0x00326270
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

	// Token: 0x06008451 RID: 33873 RVA: 0x003280FD File Offset: 0x003262FD
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

	// Token: 0x06008452 RID: 33874 RVA: 0x00328119 File Offset: 0x00326319
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

	// Token: 0x06008453 RID: 33875 RVA: 0x00328134 File Offset: 0x00326334
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

	// Token: 0x06008454 RID: 33876 RVA: 0x003281CC File Offset: 0x003263CC
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetItemNameText(string name)
	{
		this.viewComponent.ToolTip = name;
	}

	// Token: 0x06008455 RID: 33877 RVA: 0x003281DC File Offset: 0x003263DC
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

	// Token: 0x06008456 RID: 33878 RVA: 0x00328309 File Offset: 0x00326509
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

	// Token: 0x06008458 RID: 33880 RVA: 0x00328348 File Offset: 0x00326548
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool <RefreshLayerValidity>g__IsLayerValid|36_0(SignData.SignLayer layer)
	{
		LayerComplexityInfo layerComplexityInfo;
		return layer != null && this.editorWindow.signComplexityInfo.IsValid && this.editorWindow.signComplexityInfo.TryGetLayerComplexityInfo(layer, out layerComplexityInfo) && layerComplexityInfo.MaxCompStackIndex <= 7 && layerComplexityInfo.MaxUVStackIndex <= 7;
	}

	// Token: 0x04006234 RID: 25140
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Texture signMaterial;

	// Token: 0x04006235 RID: 25141
	public Action<XUiC_SignLayer> OnBecameCursorSelected;

	// Token: 0x04006236 RID: 25142
	public Action<XUiC_SignLayer> OnClicked;

	// Token: 0x0400623A RID: 25146
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color32 multiSelectColor;

	// Token: 0x0400623B RID: 25147
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignLayerGrid parentGrind;

	// Token: 0x0400623C RID: 25148
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignEditorWindow editorWindow;

	// Token: 0x0400623D RID: 25149
	[PublicizedFrom(EAccessModifier.Private)]
	public bool layerValid = true;

	// Token: 0x0400623F RID: 25151
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController leftInsertPoint;

	// Token: 0x04006240 RID: 25152
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController rightInsertPoint;
}
