using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E78 RID: 3704
[Preserve]
public class XUiC_AdvancedColorPicker : XUiController
{
	// Token: 0x17000D0A RID: 3338
	// (get) Token: 0x060071AA RID: 29098 RVA: 0x002BC905 File Offset: 0x002BAB05
	// (set) Token: 0x060071AB RID: 29099 RVA: 0x002BC90D File Offset: 0x002BAB0D
	[XuiXmlAttribute("cursor_radius", false)]
	public float CursorRadius
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.cursorRadius;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (Mathf.Approximately(this.cursorRadius, value))
			{
				return;
			}
			this.cursorRadius = value;
		}
	}

	// Token: 0x17000D0B RID: 3339
	// (get) Token: 0x060071AC RID: 29100 RVA: 0x002BC925 File Offset: 0x002BAB25
	// (set) Token: 0x060071AD RID: 29101 RVA: 0x002BC92D File Offset: 0x002BAB2D
	[XuiXmlAttribute("edit_alpha", false)]
	[XuiXmlBinding("show_alpha")]
	public bool EditAlpha
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.editAlpha;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (this.editAlpha == value)
			{
				return;
			}
			this.editAlpha = value;
			this.bindingsDirty = true;
		}
	}

	// Token: 0x17000D0C RID: 3340
	// (get) Token: 0x060071AE RID: 29102 RVA: 0x002BC947 File Offset: 0x002BAB47
	[XuiXmlBinding("has_copy")]
	public bool HasCopyBuffer
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return XUiC_AdvancedColorPicker.copyBuffer != null;
		}
	}

	// Token: 0x17000D0D RID: 3341
	// (get) Token: 0x060071AF RID: 29103 RVA: 0x002BC953 File Offset: 0x002BAB53
	// (set) Token: 0x060071B0 RID: 29104 RVA: 0x002BC95B File Offset: 0x002BAB5B
	[XuiXmlBinding("sampler_active")]
	public bool SamplerActive
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.samplerActive;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (this.samplerActive == value)
			{
				return;
			}
			this.samplerActive = value;
			this.bindingsDirty = true;
		}
	}

	// Token: 0x060071B1 RID: 29105 RVA: 0x002BC978 File Offset: 0x002BAB78
	public override void Init()
	{
		base.Init();
		this.svPicker.CreateMaterial("Game/UI/ColorPickerSV");
		this.hPicker.CreateMaterial("Game/UI/ColorPickerH");
		this.aPicker.CreateMaterial("Game/UI/ColorPickerA");
		XUiV_Texture xuiV_Texture = this.svPicker;
		xuiV_Texture.OnRenderTexture = (UIDrawCall.OnRenderCallback)Delegate.Combine(xuiV_Texture.OnRenderTexture, new UIDrawCall.OnRenderCallback(this.svPicker_OnRender));
		XUiV_Texture xuiV_Texture2 = this.hPicker;
		xuiV_Texture2.OnRenderTexture = (UIDrawCall.OnRenderCallback)Delegate.Combine(xuiV_Texture2.OnRenderTexture, new UIDrawCall.OnRenderCallback(this.hPicker_OnRender));
		XUiV_Texture xuiV_Texture3 = this.aPicker;
		xuiV_Texture3.OnRenderTexture = (UIDrawCall.OnRenderCallback)Delegate.Combine(xuiV_Texture3.OnRenderTexture, new UIDrawCall.OnRenderCallback(this.aPicker_OnRender));
		this.svPicker.Texture = Texture2D.whiteTexture;
		this.hPicker.Texture = Texture2D.whiteTexture;
		this.aPicker.Texture = Texture2D.whiteTexture;
		XUiC_AdvancedColorPicker.<Init>g__SetupPickerEvents|47_0(this.svPicker.Controller, new Action(this.svPicker_MouseUpdate));
		XUiC_AdvancedColorPicker.<Init>g__SetupPickerEvents|47_0(this.hPicker.Controller, new Action(this.hPicker_MouseUpdate));
		XUiC_AdvancedColorPicker.<Init>g__SetupPickerEvents|47_0(this.aPicker.Controller, new Action(this.aPicker_MouseUpdate));
		this.samplerColorSprite.Controller.OnHover += this.SamplerColor_OnHover;
		this.SetColor(Color.cyan, false);
	}

	// Token: 0x060071B2 RID: 29106 RVA: 0x002BCADC File Offset: 0x002BACDC
	public override void OnOpen()
	{
		base.OnOpen();
		this.bindingsDirty = true;
		this.pickersDirty = true;
		this.hoverOverSampler = false;
		if (!this.EditAlpha)
		{
			this.alpha = 1f;
		}
	}

	// Token: 0x060071B3 RID: 29107 RVA: 0x002BCB0C File Offset: 0x002BAD0C
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.bindingsDirty)
		{
			base.RefreshBindings();
			this.bindingsDirty = false;
		}
		if (this.pickersDirty)
		{
			this.updatePickerRects();
		}
	}

	// Token: 0x060071B4 RID: 29108 RVA: 0x002BCB38 File Offset: 0x002BAD38
	[PublicizedFrom(EAccessModifier.Private)]
	public void updatePickerRects()
	{
		this.svPickerRect = this.svPicker.GetXUiRect();
		this.hPickerRect = this.hPicker.GetXUiRect();
		this.aPickerRect = this.aPicker.GetXUiRect();
		this.pickersDirty = false;
	}

	// Token: 0x060071B5 RID: 29109 RVA: 0x002BCB74 File Offset: 0x002BAD74
	public void SetColor(Color _color, bool _notifyListeners = false)
	{
		this.selectedColorSprite.Color = _color;
		this.updateHsva(_color);
		this.updateRgba(_color);
		this.updateHex(_color);
		if (_notifyListeners)
		{
			Action<Color> onColorChanged = this.OnColorChanged;
			if (onColorChanged == null)
			{
				return;
			}
			onColorChanged(_color);
		}
	}

	// Token: 0x060071B6 RID: 29110 RVA: 0x002BCBAB File Offset: 0x002BADAB
	[XuiBindEvent("OnPressed", "btnCopy")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCopy_OnPressed(XUiController _sender, int _mouseButton)
	{
		XUiC_AdvancedColorPicker.copyBuffer = new Color?(this.getCurrentColor());
		base.RefreshBindings();
	}

	// Token: 0x060071B7 RID: 29111 RVA: 0x002BCBC3 File Offset: 0x002BADC3
	[XuiBindEvent("OnPressed", "btnPaste")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnPaste_OnPressed(XUiController _sender, int _mouseButton)
	{
		if (XUiC_AdvancedColorPicker.copyBuffer != null)
		{
			this.SetColor(XUiC_AdvancedColorPicker.copyBuffer.Value, true);
		}
	}

	// Token: 0x060071B8 RID: 29112 RVA: 0x002BCBE2 File Offset: 0x002BADE2
	[XuiBindEvent("OnPress", "btnSampler")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnSampler_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.samplerActive = true;
		GameManager.Instance.StartCoroutine(this.samplerCoroutine());
		this.bindingsDirty = true;
	}

	// Token: 0x060071B9 RID: 29113 RVA: 0x002BCC03 File Offset: 0x002BAE03
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator samplerCoroutine()
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		this.samplerColorSprite.Color = this.selectedColorSprite.Color;
		while (this.samplerActive)
		{
			yield return new WaitForEndOfFrame();
			if (!this.hoverOverSampler && Input.mousePosition.x >= 0f && Input.mousePosition.y >= 0f && Input.mousePosition.x < (float)Screen.width && Input.mousePosition.y < (float)Screen.height)
			{
				this.samplerTexture.ReadPixels(new Rect(Input.mousePosition, Vector2.one), 0, 0);
				this.samplerTexture.Apply();
				this.samplerColor = this.samplerTexture.GetPixel(0, 0);
				this.samplerColorSprite.Color = this.samplerColor;
			}
		}
		yield break;
	}

	// Token: 0x060071BA RID: 29114 RVA: 0x002BCC12 File Offset: 0x002BAE12
	[XuiBindEvent("OnPress", "btnSamplerOverlay")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnSamplerOverlay_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.SetColor(this.samplerColor, true);
		this.samplerActive = false;
		this.bindingsDirty = true;
	}

	// Token: 0x060071BB RID: 29115 RVA: 0x002BCC2F File Offset: 0x002BAE2F
	[XuiBindEvent("OnRightPress", "btnSamplerOverlay")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnSamplerOverlay_OnRightPressed(XUiController _sender, int _mouseButton)
	{
		this.samplerActive = false;
		this.bindingsDirty = true;
	}

	// Token: 0x060071BC RID: 29116 RVA: 0x002BCC3F File Offset: 0x002BAE3F
	[PublicizedFrom(EAccessModifier.Private)]
	public void SamplerColor_OnHover(XUiController _sender, bool _isOver)
	{
		this.hoverOverSampler = _isOver;
	}

	// Token: 0x060071BD RID: 29117 RVA: 0x002BCC48 File Offset: 0x002BAE48
	[PublicizedFrom(EAccessModifier.Private)]
	public void aPicker_MouseUpdate()
	{
		Vector2 relativeMousePos = this.getRelativeMousePos(this.aPickerRect);
		this.alpha = Mathf.Clamp01(relativeMousePos.x);
		this.OnHSVAChanged();
	}

	// Token: 0x060071BE RID: 29118 RVA: 0x002BCC7C File Offset: 0x002BAE7C
	[PublicizedFrom(EAccessModifier.Private)]
	public void hPicker_MouseUpdate()
	{
		Vector2 relativeMousePos = this.getRelativeMousePos(this.hPickerRect);
		this.hue = Mathf.Clamp01(relativeMousePos.x);
		this.OnHSVAChanged();
	}

	// Token: 0x060071BF RID: 29119 RVA: 0x002BCCB0 File Offset: 0x002BAEB0
	[PublicizedFrom(EAccessModifier.Private)]
	public void svPicker_MouseUpdate()
	{
		Vector2 relativeMousePos = this.getRelativeMousePos(this.svPickerRect);
		this.saturation = Mathf.Clamp01(relativeMousePos.x);
		this.value = Mathf.Clamp01(relativeMousePos.y);
		this.OnHSVAChanged();
	}

	// Token: 0x060071C0 RID: 29120 RVA: 0x002BCCF4 File Offset: 0x002BAEF4
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 getRelativeMousePos(Rect _controllerXUiRect)
	{
		return (this.xui.GetMouseXUiPosition().AsVector2() - _controllerXUiRect.min) / _controllerXUiRect.size;
	}

	// Token: 0x060071C1 RID: 29121 RVA: 0x002BCD2C File Offset: 0x002BAF2C
	[PublicizedFrom(EAccessModifier.Private)]
	public void aPicker_OnRender(Material _mat)
	{
		_mat.SetVector(XUiC_AdvancedColorPicker._Dimensions, this.aPicker.Size.AsVector2());
		Color currentColor = this.getCurrentColor();
		_mat.SetColor(XUiC_AdvancedColorPicker._Color, currentColor);
		_mat.SetFloat(XUiC_AdvancedColorPicker._CursorSize, this.CursorRadius);
	}

	// Token: 0x060071C2 RID: 29122 RVA: 0x002BCD80 File Offset: 0x002BAF80
	[PublicizedFrom(EAccessModifier.Private)]
	public void hPicker_OnRender(Material _mat)
	{
		_mat.SetVector(XUiC_AdvancedColorPicker._Dimensions, this.hPicker.Size.AsVector2());
		_mat.SetFloat(XUiC_AdvancedColorPicker._Hue, this.hue);
		_mat.SetFloat(XUiC_AdvancedColorPicker._CursorSize, this.CursorRadius);
	}

	// Token: 0x060071C3 RID: 29123 RVA: 0x002BCDD4 File Offset: 0x002BAFD4
	[PublicizedFrom(EAccessModifier.Private)]
	public void svPicker_OnRender(Material _mat)
	{
		_mat.SetVector(XUiC_AdvancedColorPicker._Dimensions, this.svPicker.Size.AsVector2());
		_mat.SetFloat(XUiC_AdvancedColorPicker._Hue, this.hue);
		_mat.SetFloat(XUiC_AdvancedColorPicker._Saturation, this.saturation);
		_mat.SetFloat(XUiC_AdvancedColorPicker._Value, this.value);
		_mat.SetFloat(XUiC_AdvancedColorPicker._CursorSize, this.CursorRadius);
	}

	// Token: 0x060071C4 RID: 29124 RVA: 0x002BCE48 File Offset: 0x002BB048
	[XuiBindEvent("OnChangeHandler", "txtHex")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnHexChanged(XUiController _sender, string _text, bool _changeFromCode)
	{
		if (_text.Length != 6 && _text.Length != 8)
		{
			return;
		}
		Color color;
		if (!ColorUtility.TryParseHtmlString("#" + _text, out color))
		{
			Log.Error("Failed to parse hex string `" + _text + "`.");
			return;
		}
		if (!_changeFromCode)
		{
			this.selectedColorSprite.Color = color;
			this.updateHsva(color);
			this.updateRgba(color);
			Action<Color> onColorChanged = this.OnColorChanged;
			if (onColorChanged == null)
			{
				return;
			}
			onColorChanged(color);
		}
	}

	// Token: 0x060071C5 RID: 29125 RVA: 0x002BCEC0 File Offset: 0x002BB0C0
	[XuiBindEvent("OnChangeHandler", "txtColorR")]
	[XuiBindEvent("OnChangeHandler", "txtColorG")]
	[XuiBindEvent("OnChangeHandler", "txtColorB")]
	[XuiBindEvent("OnChangeHandler", "txtColorA")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnRGBAChanged(XUiController _sender, string _text, bool _changeFromCode)
	{
		int num;
		if (!int.TryParse(_text, out num))
		{
			return;
		}
		num = Mathf.Clamp(num, 0, 255);
		if (!_text.Equals(num.ToString()))
		{
			((XUiC_TextInput)_sender).Text = num.ToString();
		}
		byte b;
		byte b2;
		byte b3;
		byte b4;
		if (!_changeFromCode && byte.TryParse(this.txtColorR.Text, out b) && byte.TryParse(this.txtColorG.Text, out b2) && byte.TryParse(this.txtColorB.Text, out b3) && byte.TryParse(this.txtColorA.Text, out b4))
		{
			Color color = new Color32(b, b2, b3, b4);
			if (!this.EditAlpha)
			{
				color.a = 1f;
			}
			this.selectedColorSprite.Color = color;
			this.updateHsva(color);
			this.updateHex(color);
			Action<Color> onColorChanged = this.OnColorChanged;
			if (onColorChanged == null)
			{
				return;
			}
			onColorChanged(color);
		}
	}

	// Token: 0x060071C6 RID: 29126 RVA: 0x002BCFB4 File Offset: 0x002BB1B4
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnHSVAChanged()
	{
		Color currentColor = this.getCurrentColor();
		this.selectedColorSprite.Color = currentColor;
		this.updateRgba(currentColor);
		this.updateHex(currentColor);
		Action<Color> onColorChanged = this.OnColorChanged;
		if (onColorChanged == null)
		{
			return;
		}
		onColorChanged(currentColor);
	}

	// Token: 0x060071C7 RID: 29127 RVA: 0x002BCFF4 File Offset: 0x002BB1F4
	[PublicizedFrom(EAccessModifier.Private)]
	public Color getCurrentColor()
	{
		Color color = Color.HSVToRGB(this.hue, this.saturation, this.value);
		color.a = this.alpha;
		return color;
	}

	// Token: 0x060071C8 RID: 29128 RVA: 0x002BD027 File Offset: 0x002BB227
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateHsva(Color _color)
	{
		Color.RGBToHSV(_color, out this.hue, out this.saturation, out this.value);
		this.alpha = _color.a;
	}

	// Token: 0x060071C9 RID: 29129 RVA: 0x002BD050 File Offset: 0x002BB250
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateRgba(Color _color)
	{
		Color32 color = _color;
		this.txtColorR.Text = color.r.ToString();
		this.txtColorG.Text = color.g.ToString();
		this.txtColorB.Text = color.b.ToString();
		this.txtColorA.Text = color.a.ToString();
	}

	// Token: 0x060071CA RID: 29130 RVA: 0x002BD0C0 File Offset: 0x002BB2C0
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateHex(Color _color)
	{
		this.txtHex.Text = (this.EditAlpha ? ColorUtility.ToHtmlStringRGBA(_color) : ColorUtility.ToHtmlStringRGB(_color));
	}

	// Token: 0x060071CB RID: 29131 RVA: 0x002BD0E3 File Offset: 0x002BB2E3
	[Conditional("DEBUG_LOG_PICKERS")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void debugLog(string _message)
	{
		global::UnityEngine.Debug.Log("[AdvancedColorPicker] " + _message);
	}

	// Token: 0x060071CE RID: 29134 RVA: 0x002BD1D0 File Offset: 0x002BB3D0
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void <Init>g__SetupPickerEvents|47_0(XUiController _ctrl, Action _update)
	{
		_ctrl.OnPress += delegate(XUiController _, int _)
		{
			_update();
		};
		_ctrl.OnDrag += delegate(XUiController _, EDragType _, Vector2 _)
		{
			_update();
		};
	}

	// Token: 0x040055DD RID: 21981
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _Dimensions = Shader.PropertyToID("_Dimensions");

	// Token: 0x040055DE RID: 21982
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _Saturation = Shader.PropertyToID("_Saturation");

	// Token: 0x040055DF RID: 21983
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _Value = Shader.PropertyToID("_Value");

	// Token: 0x040055E0 RID: 21984
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _Hue = Shader.PropertyToID("_Hue");

	// Token: 0x040055E1 RID: 21985
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _Color = Shader.PropertyToID("_Color");

	// Token: 0x040055E2 RID: 21986
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _CursorSize = Shader.PropertyToID("_CursorSize");

	// Token: 0x040055E3 RID: 21987
	[PublicizedFrom(EAccessModifier.Private)]
	public static Color? copyBuffer;

	// Token: 0x040055E4 RID: 21988
	[XuiBindComponent("svPicker", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Texture svPicker;

	// Token: 0x040055E5 RID: 21989
	[XuiBindComponent("hPicker", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Texture hPicker;

	// Token: 0x040055E6 RID: 21990
	[XuiBindComponent("aPicker", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Texture aPicker;

	// Token: 0x040055E7 RID: 21991
	[PublicizedFrom(EAccessModifier.Private)]
	public Rect svPickerRect;

	// Token: 0x040055E8 RID: 21992
	[PublicizedFrom(EAccessModifier.Private)]
	public Rect hPickerRect;

	// Token: 0x040055E9 RID: 21993
	[PublicizedFrom(EAccessModifier.Private)]
	public Rect aPickerRect;

	// Token: 0x040055EA RID: 21994
	[XuiBindComponent("selectedColor", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite selectedColorSprite;

	// Token: 0x040055EB RID: 21995
	[XuiBindComponent("samplerColor", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite samplerColorSprite;

	// Token: 0x040055EC RID: 21996
	[XuiBindComponent("txtColorR", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtColorR;

	// Token: 0x040055ED RID: 21997
	[XuiBindComponent("txtColorG", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtColorG;

	// Token: 0x040055EE RID: 21998
	[XuiBindComponent("txtColorB", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtColorB;

	// Token: 0x040055EF RID: 21999
	[XuiBindComponent("txtColorA", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtColorA;

	// Token: 0x040055F0 RID: 22000
	[XuiBindComponent("txtHex", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtHex;

	// Token: 0x040055F1 RID: 22001
	[XuiBindComponent("btnCopy", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_SimpleButton btnCopy;

	// Token: 0x040055F2 RID: 22002
	[XuiBindComponent("btnPaste", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_SimpleButton btnPaste;

	// Token: 0x040055F3 RID: 22003
	[XuiBindComponent("btnSampler", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiController btnSampler;

	// Token: 0x040055F4 RID: 22004
	[XuiBindComponent("btnSamplerOverlay", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiController btnSamplerOverlay;

	// Token: 0x040055F5 RID: 22005
	public Action<Color> OnColorChanged;

	// Token: 0x040055F6 RID: 22006
	[PublicizedFrom(EAccessModifier.Private)]
	public float hue;

	// Token: 0x040055F7 RID: 22007
	[PublicizedFrom(EAccessModifier.Private)]
	public float saturation = 1f;

	// Token: 0x040055F8 RID: 22008
	[PublicizedFrom(EAccessModifier.Private)]
	public float value = 1f;

	// Token: 0x040055F9 RID: 22009
	[PublicizedFrom(EAccessModifier.Private)]
	public float alpha = 1f;

	// Token: 0x040055FA RID: 22010
	[PublicizedFrom(EAccessModifier.Private)]
	public float cursorRadius = 10f;

	// Token: 0x040055FB RID: 22011
	[PublicizedFrom(EAccessModifier.Private)]
	public bool editAlpha = true;

	// Token: 0x040055FC RID: 22012
	[PublicizedFrom(EAccessModifier.Private)]
	public bool pickersDirty = true;

	// Token: 0x040055FD RID: 22013
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bindingsDirty = true;

	// Token: 0x040055FE RID: 22014
	[PublicizedFrom(EAccessModifier.Private)]
	public bool samplerActive;

	// Token: 0x040055FF RID: 22015
	[PublicizedFrom(EAccessModifier.Private)]
	public Color samplerColor = Color.clear;

	// Token: 0x04005600 RID: 22016
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Texture2D samplerTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);

	// Token: 0x04005601 RID: 22017
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hoverOverSampler;
}
