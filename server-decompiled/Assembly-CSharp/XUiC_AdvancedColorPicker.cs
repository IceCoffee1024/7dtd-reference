using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E83 RID: 3715
[Preserve]
public class XUiC_AdvancedColorPicker : XUiController
{
	// Token: 0x17000D10 RID: 3344
	// (get) Token: 0x060071CF RID: 29135 RVA: 0x002BDE71 File Offset: 0x002BC071
	// (set) Token: 0x060071D0 RID: 29136 RVA: 0x002BDE79 File Offset: 0x002BC079
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

	// Token: 0x17000D11 RID: 3345
	// (get) Token: 0x060071D1 RID: 29137 RVA: 0x002BDE91 File Offset: 0x002BC091
	// (set) Token: 0x060071D2 RID: 29138 RVA: 0x002BDE99 File Offset: 0x002BC099
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

	// Token: 0x17000D12 RID: 3346
	// (get) Token: 0x060071D3 RID: 29139 RVA: 0x002BDEB3 File Offset: 0x002BC0B3
	[XuiXmlBinding("has_copy")]
	public bool HasCopyBuffer
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return XUiC_AdvancedColorPicker.copyBuffer != null;
		}
	}

	// Token: 0x17000D13 RID: 3347
	// (get) Token: 0x060071D4 RID: 29140 RVA: 0x002BDEBF File Offset: 0x002BC0BF
	// (set) Token: 0x060071D5 RID: 29141 RVA: 0x002BDEC7 File Offset: 0x002BC0C7
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

	// Token: 0x060071D6 RID: 29142 RVA: 0x002BDEE4 File Offset: 0x002BC0E4
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

	// Token: 0x060071D7 RID: 29143 RVA: 0x002BE048 File Offset: 0x002BC248
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

	// Token: 0x060071D8 RID: 29144 RVA: 0x002BE078 File Offset: 0x002BC278
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

	// Token: 0x060071D9 RID: 29145 RVA: 0x002BE0A4 File Offset: 0x002BC2A4
	[PublicizedFrom(EAccessModifier.Private)]
	public void updatePickerRects()
	{
		this.svPickerRect = this.svPicker.GetXUiRect();
		this.hPickerRect = this.hPicker.GetXUiRect();
		this.aPickerRect = this.aPicker.GetXUiRect();
		this.pickersDirty = false;
	}

	// Token: 0x060071DA RID: 29146 RVA: 0x002BE0E0 File Offset: 0x002BC2E0
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

	// Token: 0x060071DB RID: 29147 RVA: 0x002BE117 File Offset: 0x002BC317
	[XuiBindEvent("OnPressed", "btnCopy")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCopy_OnPressed(XUiController _sender, int _mouseButton)
	{
		XUiC_AdvancedColorPicker.copyBuffer = new Color?(this.getCurrentColor());
		base.RefreshBindings();
	}

	// Token: 0x060071DC RID: 29148 RVA: 0x002BE12F File Offset: 0x002BC32F
	[XuiBindEvent("OnPressed", "btnPaste")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnPaste_OnPressed(XUiController _sender, int _mouseButton)
	{
		if (XUiC_AdvancedColorPicker.copyBuffer != null)
		{
			this.SetColor(XUiC_AdvancedColorPicker.copyBuffer.Value, true);
		}
	}

	// Token: 0x060071DD RID: 29149 RVA: 0x002BE14E File Offset: 0x002BC34E
	[XuiBindEvent("OnPress", "btnSampler")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnSampler_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.samplerActive = true;
		GameManager.Instance.StartCoroutine(this.samplerCoroutine());
		this.bindingsDirty = true;
	}

	// Token: 0x060071DE RID: 29150 RVA: 0x002BE16F File Offset: 0x002BC36F
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

	// Token: 0x060071DF RID: 29151 RVA: 0x002BE17E File Offset: 0x002BC37E
	[XuiBindEvent("OnPress", "btnSamplerOverlay")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnSamplerOverlay_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.SetColor(this.samplerColor, true);
		this.samplerActive = false;
		this.bindingsDirty = true;
	}

	// Token: 0x060071E0 RID: 29152 RVA: 0x002BE19B File Offset: 0x002BC39B
	[XuiBindEvent("OnRightPress", "btnSamplerOverlay")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnSamplerOverlay_OnRightPressed(XUiController _sender, int _mouseButton)
	{
		this.samplerActive = false;
		this.bindingsDirty = true;
	}

	// Token: 0x060071E1 RID: 29153 RVA: 0x002BE1AB File Offset: 0x002BC3AB
	[PublicizedFrom(EAccessModifier.Private)]
	public void SamplerColor_OnHover(XUiController _sender, bool _isOver)
	{
		this.hoverOverSampler = _isOver;
	}

	// Token: 0x060071E2 RID: 29154 RVA: 0x002BE1B4 File Offset: 0x002BC3B4
	[PublicizedFrom(EAccessModifier.Private)]
	public void aPicker_MouseUpdate()
	{
		Vector2 relativeMousePos = this.getRelativeMousePos(this.aPickerRect);
		this.alpha = Mathf.Clamp01(relativeMousePos.x);
		this.OnHSVAChanged();
	}

	// Token: 0x060071E3 RID: 29155 RVA: 0x002BE1E8 File Offset: 0x002BC3E8
	[PublicizedFrom(EAccessModifier.Private)]
	public void hPicker_MouseUpdate()
	{
		Vector2 relativeMousePos = this.getRelativeMousePos(this.hPickerRect);
		this.hue = Mathf.Clamp01(relativeMousePos.x);
		this.OnHSVAChanged();
	}

	// Token: 0x060071E4 RID: 29156 RVA: 0x002BE21C File Offset: 0x002BC41C
	[PublicizedFrom(EAccessModifier.Private)]
	public void svPicker_MouseUpdate()
	{
		Vector2 relativeMousePos = this.getRelativeMousePos(this.svPickerRect);
		this.saturation = Mathf.Clamp01(relativeMousePos.x);
		this.value = Mathf.Clamp01(relativeMousePos.y);
		this.OnHSVAChanged();
	}

	// Token: 0x060071E5 RID: 29157 RVA: 0x002BE260 File Offset: 0x002BC460
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 getRelativeMousePos(Rect _controllerXUiRect)
	{
		return (this.xui.GetMouseXUiPosition().AsVector2() - _controllerXUiRect.min) / _controllerXUiRect.size;
	}

	// Token: 0x060071E6 RID: 29158 RVA: 0x002BE298 File Offset: 0x002BC498
	[PublicizedFrom(EAccessModifier.Private)]
	public void aPicker_OnRender(Material _mat)
	{
		_mat.SetVector(XUiC_AdvancedColorPicker._Dimensions, this.aPicker.Size.AsVector2());
		Color currentColor = this.getCurrentColor();
		_mat.SetColor(XUiC_AdvancedColorPicker._Color, currentColor);
		_mat.SetFloat(XUiC_AdvancedColorPicker._CursorSize, this.CursorRadius);
	}

	// Token: 0x060071E7 RID: 29159 RVA: 0x002BE2EC File Offset: 0x002BC4EC
	[PublicizedFrom(EAccessModifier.Private)]
	public void hPicker_OnRender(Material _mat)
	{
		_mat.SetVector(XUiC_AdvancedColorPicker._Dimensions, this.hPicker.Size.AsVector2());
		_mat.SetFloat(XUiC_AdvancedColorPicker._Hue, this.hue);
		_mat.SetFloat(XUiC_AdvancedColorPicker._CursorSize, this.CursorRadius);
	}

	// Token: 0x060071E8 RID: 29160 RVA: 0x002BE340 File Offset: 0x002BC540
	[PublicizedFrom(EAccessModifier.Private)]
	public void svPicker_OnRender(Material _mat)
	{
		_mat.SetVector(XUiC_AdvancedColorPicker._Dimensions, this.svPicker.Size.AsVector2());
		_mat.SetFloat(XUiC_AdvancedColorPicker._Hue, this.hue);
		_mat.SetFloat(XUiC_AdvancedColorPicker._Saturation, this.saturation);
		_mat.SetFloat(XUiC_AdvancedColorPicker._Value, this.value);
		_mat.SetFloat(XUiC_AdvancedColorPicker._CursorSize, this.CursorRadius);
	}

	// Token: 0x060071E9 RID: 29161 RVA: 0x002BE3B4 File Offset: 0x002BC5B4
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

	// Token: 0x060071EA RID: 29162 RVA: 0x002BE42C File Offset: 0x002BC62C
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

	// Token: 0x060071EB RID: 29163 RVA: 0x002BE520 File Offset: 0x002BC720
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

	// Token: 0x060071EC RID: 29164 RVA: 0x002BE560 File Offset: 0x002BC760
	[PublicizedFrom(EAccessModifier.Private)]
	public Color getCurrentColor()
	{
		Color color = Color.HSVToRGB(this.hue, this.saturation, this.value);
		color.a = this.alpha;
		return color;
	}

	// Token: 0x060071ED RID: 29165 RVA: 0x002BE593 File Offset: 0x002BC793
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateHsva(Color _color)
	{
		Color.RGBToHSV(_color, out this.hue, out this.saturation, out this.value);
		this.alpha = _color.a;
	}

	// Token: 0x060071EE RID: 29166 RVA: 0x002BE5BC File Offset: 0x002BC7BC
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateRgba(Color _color)
	{
		Color32 color = _color;
		this.txtColorR.Text = color.r.ToString();
		this.txtColorG.Text = color.g.ToString();
		this.txtColorB.Text = color.b.ToString();
		this.txtColorA.Text = color.a.ToString();
	}

	// Token: 0x060071EF RID: 29167 RVA: 0x002BE62C File Offset: 0x002BC82C
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateHex(Color _color)
	{
		this.txtHex.Text = (this.EditAlpha ? ColorUtility.ToHtmlStringRGBA(_color) : ColorUtility.ToHtmlStringRGB(_color));
	}

	// Token: 0x060071F0 RID: 29168 RVA: 0x002BE64F File Offset: 0x002BC84F
	[Conditional("DEBUG_LOG_PICKERS")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void debugLog(string _message)
	{
		global::UnityEngine.Debug.Log("[AdvancedColorPicker] " + _message);
	}

	// Token: 0x060071F3 RID: 29171 RVA: 0x002BE73C File Offset: 0x002BC93C
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

	// Token: 0x04005602 RID: 22018
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _Dimensions = Shader.PropertyToID("_Dimensions");

	// Token: 0x04005603 RID: 22019
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _Saturation = Shader.PropertyToID("_Saturation");

	// Token: 0x04005604 RID: 22020
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _Value = Shader.PropertyToID("_Value");

	// Token: 0x04005605 RID: 22021
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _Hue = Shader.PropertyToID("_Hue");

	// Token: 0x04005606 RID: 22022
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _Color = Shader.PropertyToID("_Color");

	// Token: 0x04005607 RID: 22023
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _CursorSize = Shader.PropertyToID("_CursorSize");

	// Token: 0x04005608 RID: 22024
	[PublicizedFrom(EAccessModifier.Private)]
	public static Color? copyBuffer;

	// Token: 0x04005609 RID: 22025
	[XuiBindComponent("svPicker", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Texture svPicker;

	// Token: 0x0400560A RID: 22026
	[XuiBindComponent("hPicker", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Texture hPicker;

	// Token: 0x0400560B RID: 22027
	[XuiBindComponent("aPicker", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Texture aPicker;

	// Token: 0x0400560C RID: 22028
	[PublicizedFrom(EAccessModifier.Private)]
	public Rect svPickerRect;

	// Token: 0x0400560D RID: 22029
	[PublicizedFrom(EAccessModifier.Private)]
	public Rect hPickerRect;

	// Token: 0x0400560E RID: 22030
	[PublicizedFrom(EAccessModifier.Private)]
	public Rect aPickerRect;

	// Token: 0x0400560F RID: 22031
	[XuiBindComponent("selectedColor", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite selectedColorSprite;

	// Token: 0x04005610 RID: 22032
	[XuiBindComponent("samplerColor", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite samplerColorSprite;

	// Token: 0x04005611 RID: 22033
	[XuiBindComponent("txtColorR", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtColorR;

	// Token: 0x04005612 RID: 22034
	[XuiBindComponent("txtColorG", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtColorG;

	// Token: 0x04005613 RID: 22035
	[XuiBindComponent("txtColorB", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtColorB;

	// Token: 0x04005614 RID: 22036
	[XuiBindComponent("txtColorA", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtColorA;

	// Token: 0x04005615 RID: 22037
	[XuiBindComponent("txtHex", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtHex;

	// Token: 0x04005616 RID: 22038
	[XuiBindComponent("btnCopy", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_SimpleButton btnCopy;

	// Token: 0x04005617 RID: 22039
	[XuiBindComponent("btnPaste", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_SimpleButton btnPaste;

	// Token: 0x04005618 RID: 22040
	[XuiBindComponent("btnSampler", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiController btnSampler;

	// Token: 0x04005619 RID: 22041
	[XuiBindComponent("btnSamplerOverlay", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiController btnSamplerOverlay;

	// Token: 0x0400561A RID: 22042
	public Action<Color> OnColorChanged;

	// Token: 0x0400561B RID: 22043
	[PublicizedFrom(EAccessModifier.Private)]
	public float hue;

	// Token: 0x0400561C RID: 22044
	[PublicizedFrom(EAccessModifier.Private)]
	public float saturation = 1f;

	// Token: 0x0400561D RID: 22045
	[PublicizedFrom(EAccessModifier.Private)]
	public float value = 1f;

	// Token: 0x0400561E RID: 22046
	[PublicizedFrom(EAccessModifier.Private)]
	public float alpha = 1f;

	// Token: 0x0400561F RID: 22047
	[PublicizedFrom(EAccessModifier.Private)]
	public float cursorRadius = 10f;

	// Token: 0x04005620 RID: 22048
	[PublicizedFrom(EAccessModifier.Private)]
	public bool editAlpha = true;

	// Token: 0x04005621 RID: 22049
	[PublicizedFrom(EAccessModifier.Private)]
	public bool pickersDirty = true;

	// Token: 0x04005622 RID: 22050
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bindingsDirty = true;

	// Token: 0x04005623 RID: 22051
	[PublicizedFrom(EAccessModifier.Private)]
	public bool samplerActive;

	// Token: 0x04005624 RID: 22052
	[PublicizedFrom(EAccessModifier.Private)]
	public Color samplerColor = Color.clear;

	// Token: 0x04005625 RID: 22053
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Texture2D samplerTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);

	// Token: 0x04005626 RID: 22054
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hoverOverSampler;
}
