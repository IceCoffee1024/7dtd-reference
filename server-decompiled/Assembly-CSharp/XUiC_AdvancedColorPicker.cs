using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E69 RID: 3689
[Preserve]
public class XUiC_AdvancedColorPicker : XUiController
{
	// Token: 0x17000CEF RID: 3311
	// (get) Token: 0x060071B2 RID: 29106 RVA: 0x002B9555 File Offset: 0x002B7755
	// (set) Token: 0x060071B3 RID: 29107 RVA: 0x002B955D File Offset: 0x002B775D
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

	// Token: 0x17000CF0 RID: 3312
	// (get) Token: 0x060071B4 RID: 29108 RVA: 0x002B9575 File Offset: 0x002B7775
	// (set) Token: 0x060071B5 RID: 29109 RVA: 0x002B957D File Offset: 0x002B777D
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

	// Token: 0x17000CF1 RID: 3313
	// (get) Token: 0x060071B6 RID: 29110 RVA: 0x002B9597 File Offset: 0x002B7797
	[XuiXmlBinding("has_copy")]
	public bool HasCopyBuffer
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return XUiC_AdvancedColorPicker.copyBuffer != null;
		}
	}

	// Token: 0x17000CF2 RID: 3314
	// (get) Token: 0x060071B7 RID: 29111 RVA: 0x002B95A3 File Offset: 0x002B77A3
	// (set) Token: 0x060071B8 RID: 29112 RVA: 0x002B95AB File Offset: 0x002B77AB
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

	// Token: 0x060071B9 RID: 29113 RVA: 0x002B95C8 File Offset: 0x002B77C8
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

	// Token: 0x060071BA RID: 29114 RVA: 0x002B972C File Offset: 0x002B792C
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

	// Token: 0x060071BB RID: 29115 RVA: 0x002B975C File Offset: 0x002B795C
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

	// Token: 0x060071BC RID: 29116 RVA: 0x002B9788 File Offset: 0x002B7988
	[PublicizedFrom(EAccessModifier.Private)]
	public void updatePickerRects()
	{
		this.svPickerRect = this.svPicker.GetXUiRect();
		this.hPickerRect = this.hPicker.GetXUiRect();
		this.aPickerRect = this.aPicker.GetXUiRect();
		this.pickersDirty = false;
	}

	// Token: 0x060071BD RID: 29117 RVA: 0x002B97C4 File Offset: 0x002B79C4
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

	// Token: 0x060071BE RID: 29118 RVA: 0x002B97FB File Offset: 0x002B79FB
	[XuiBindEvent("OnPressed", "btnCopy")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCopy_OnPressed(XUiController _sender, int _mouseButton)
	{
		XUiC_AdvancedColorPicker.copyBuffer = new Color?(this.getCurrentColor());
		base.RefreshBindings();
	}

	// Token: 0x060071BF RID: 29119 RVA: 0x002B9813 File Offset: 0x002B7A13
	[XuiBindEvent("OnPressed", "btnPaste")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnPaste_OnPressed(XUiController _sender, int _mouseButton)
	{
		if (XUiC_AdvancedColorPicker.copyBuffer != null)
		{
			this.SetColor(XUiC_AdvancedColorPicker.copyBuffer.Value, true);
		}
	}

	// Token: 0x060071C0 RID: 29120 RVA: 0x002B9832 File Offset: 0x002B7A32
	[XuiBindEvent("OnPress", "btnSampler")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnSampler_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.samplerActive = true;
		GameManager.Instance.StartCoroutine(this.samplerCoroutine());
		this.bindingsDirty = true;
	}

	// Token: 0x060071C1 RID: 29121 RVA: 0x002B9853 File Offset: 0x002B7A53
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

	// Token: 0x060071C2 RID: 29122 RVA: 0x002B9862 File Offset: 0x002B7A62
	[XuiBindEvent("OnPress", "btnSamplerOverlay")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnSamplerOverlay_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.SetColor(this.samplerColor, true);
		this.samplerActive = false;
		this.bindingsDirty = true;
	}

	// Token: 0x060071C3 RID: 29123 RVA: 0x002B987F File Offset: 0x002B7A7F
	[XuiBindEvent("OnRightPress", "btnSamplerOverlay")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnSamplerOverlay_OnRightPressed(XUiController _sender, int _mouseButton)
	{
		this.samplerActive = false;
		this.bindingsDirty = true;
	}

	// Token: 0x060071C4 RID: 29124 RVA: 0x002B988F File Offset: 0x002B7A8F
	[PublicizedFrom(EAccessModifier.Private)]
	public void SamplerColor_OnHover(XUiController _sender, bool _isOver)
	{
		this.hoverOverSampler = _isOver;
	}

	// Token: 0x060071C5 RID: 29125 RVA: 0x002B9898 File Offset: 0x002B7A98
	[PublicizedFrom(EAccessModifier.Private)]
	public void aPicker_MouseUpdate()
	{
		Vector2 relativeMousePos = this.getRelativeMousePos(this.aPickerRect);
		this.alpha = Mathf.Clamp01(relativeMousePos.x);
		this.OnHSVAChanged();
	}

	// Token: 0x060071C6 RID: 29126 RVA: 0x002B98CC File Offset: 0x002B7ACC
	[PublicizedFrom(EAccessModifier.Private)]
	public void hPicker_MouseUpdate()
	{
		Vector2 relativeMousePos = this.getRelativeMousePos(this.hPickerRect);
		this.hue = Mathf.Clamp01(relativeMousePos.x);
		this.OnHSVAChanged();
	}

	// Token: 0x060071C7 RID: 29127 RVA: 0x002B9900 File Offset: 0x002B7B00
	[PublicizedFrom(EAccessModifier.Private)]
	public void svPicker_MouseUpdate()
	{
		Vector2 relativeMousePos = this.getRelativeMousePos(this.svPickerRect);
		this.saturation = Mathf.Clamp01(relativeMousePos.x);
		this.value = Mathf.Clamp01(relativeMousePos.y);
		this.OnHSVAChanged();
	}

	// Token: 0x060071C8 RID: 29128 RVA: 0x002B9944 File Offset: 0x002B7B44
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 getRelativeMousePos(Rect _controllerXUiRect)
	{
		return (this.xui.GetMouseXUiPosition().AsVector2() - _controllerXUiRect.min) / _controllerXUiRect.size;
	}

	// Token: 0x060071C9 RID: 29129 RVA: 0x002B997C File Offset: 0x002B7B7C
	[PublicizedFrom(EAccessModifier.Private)]
	public void aPicker_OnRender(Material _mat)
	{
		_mat.SetVector(XUiC_AdvancedColorPicker._Dimensions, this.aPicker.Size.AsVector2());
		Color currentColor = this.getCurrentColor();
		_mat.SetColor(XUiC_AdvancedColorPicker._Color, currentColor);
		_mat.SetFloat(XUiC_AdvancedColorPicker._CursorSize, this.CursorRadius);
	}

	// Token: 0x060071CA RID: 29130 RVA: 0x002B99D0 File Offset: 0x002B7BD0
	[PublicizedFrom(EAccessModifier.Private)]
	public void hPicker_OnRender(Material _mat)
	{
		_mat.SetVector(XUiC_AdvancedColorPicker._Dimensions, this.hPicker.Size.AsVector2());
		_mat.SetFloat(XUiC_AdvancedColorPicker._Hue, this.hue);
		_mat.SetFloat(XUiC_AdvancedColorPicker._CursorSize, this.CursorRadius);
	}

	// Token: 0x060071CB RID: 29131 RVA: 0x002B9A24 File Offset: 0x002B7C24
	[PublicizedFrom(EAccessModifier.Private)]
	public void svPicker_OnRender(Material _mat)
	{
		_mat.SetVector(XUiC_AdvancedColorPicker._Dimensions, this.svPicker.Size.AsVector2());
		_mat.SetFloat(XUiC_AdvancedColorPicker._Hue, this.hue);
		_mat.SetFloat(XUiC_AdvancedColorPicker._Saturation, this.saturation);
		_mat.SetFloat(XUiC_AdvancedColorPicker._Value, this.value);
		_mat.SetFloat(XUiC_AdvancedColorPicker._CursorSize, this.CursorRadius);
	}

	// Token: 0x060071CC RID: 29132 RVA: 0x002B9A98 File Offset: 0x002B7C98
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

	// Token: 0x060071CD RID: 29133 RVA: 0x002B9B10 File Offset: 0x002B7D10
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

	// Token: 0x060071CE RID: 29134 RVA: 0x002B9C04 File Offset: 0x002B7E04
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

	// Token: 0x060071CF RID: 29135 RVA: 0x002B9C44 File Offset: 0x002B7E44
	[PublicizedFrom(EAccessModifier.Private)]
	public Color getCurrentColor()
	{
		Color color = Color.HSVToRGB(this.hue, this.saturation, this.value);
		color.a = this.alpha;
		return color;
	}

	// Token: 0x060071D0 RID: 29136 RVA: 0x002B9C77 File Offset: 0x002B7E77
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateHsva(Color _color)
	{
		Color.RGBToHSV(_color, out this.hue, out this.saturation, out this.value);
		this.alpha = _color.a;
	}

	// Token: 0x060071D1 RID: 29137 RVA: 0x002B9CA0 File Offset: 0x002B7EA0
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateRgba(Color _color)
	{
		Color32 color = _color;
		this.txtColorR.Text = color.r.ToString();
		this.txtColorG.Text = color.g.ToString();
		this.txtColorB.Text = color.b.ToString();
		this.txtColorA.Text = color.a.ToString();
	}

	// Token: 0x060071D2 RID: 29138 RVA: 0x002B9D10 File Offset: 0x002B7F10
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateHex(Color _color)
	{
		this.txtHex.Text = (this.EditAlpha ? ColorUtility.ToHtmlStringRGBA(_color) : ColorUtility.ToHtmlStringRGB(_color));
	}

	// Token: 0x060071D3 RID: 29139 RVA: 0x002B9D33 File Offset: 0x002B7F33
	[Conditional("DEBUG_LOG_PICKERS")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void debugLog(string _message)
	{
		global::UnityEngine.Debug.Log("[AdvancedColorPicker] " + _message);
	}

	// Token: 0x060071D6 RID: 29142 RVA: 0x002B9E20 File Offset: 0x002B8020
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

	// Token: 0x040055A6 RID: 21926
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _Dimensions = Shader.PropertyToID("_Dimensions");

	// Token: 0x040055A7 RID: 21927
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _Saturation = Shader.PropertyToID("_Saturation");

	// Token: 0x040055A8 RID: 21928
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _Value = Shader.PropertyToID("_Value");

	// Token: 0x040055A9 RID: 21929
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _Hue = Shader.PropertyToID("_Hue");

	// Token: 0x040055AA RID: 21930
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _Color = Shader.PropertyToID("_Color");

	// Token: 0x040055AB RID: 21931
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _CursorSize = Shader.PropertyToID("_CursorSize");

	// Token: 0x040055AC RID: 21932
	[PublicizedFrom(EAccessModifier.Private)]
	public static Color? copyBuffer;

	// Token: 0x040055AD RID: 21933
	[XuiBindComponent("svPicker", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Texture svPicker;

	// Token: 0x040055AE RID: 21934
	[XuiBindComponent("hPicker", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Texture hPicker;

	// Token: 0x040055AF RID: 21935
	[XuiBindComponent("aPicker", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Texture aPicker;

	// Token: 0x040055B0 RID: 21936
	[PublicizedFrom(EAccessModifier.Private)]
	public Rect svPickerRect;

	// Token: 0x040055B1 RID: 21937
	[PublicizedFrom(EAccessModifier.Private)]
	public Rect hPickerRect;

	// Token: 0x040055B2 RID: 21938
	[PublicizedFrom(EAccessModifier.Private)]
	public Rect aPickerRect;

	// Token: 0x040055B3 RID: 21939
	[XuiBindComponent("selectedColor", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite selectedColorSprite;

	// Token: 0x040055B4 RID: 21940
	[XuiBindComponent("samplerColor", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite samplerColorSprite;

	// Token: 0x040055B5 RID: 21941
	[XuiBindComponent("txtColorR", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtColorR;

	// Token: 0x040055B6 RID: 21942
	[XuiBindComponent("txtColorG", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtColorG;

	// Token: 0x040055B7 RID: 21943
	[XuiBindComponent("txtColorB", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtColorB;

	// Token: 0x040055B8 RID: 21944
	[XuiBindComponent("txtColorA", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtColorA;

	// Token: 0x040055B9 RID: 21945
	[XuiBindComponent("txtHex", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtHex;

	// Token: 0x040055BA RID: 21946
	[XuiBindComponent("btnCopy", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_SimpleButton btnCopy;

	// Token: 0x040055BB RID: 21947
	[XuiBindComponent("btnPaste", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_SimpleButton btnPaste;

	// Token: 0x040055BC RID: 21948
	[XuiBindComponent("btnSampler", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiController btnSampler;

	// Token: 0x040055BD RID: 21949
	[XuiBindComponent("btnSamplerOverlay", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiController btnSamplerOverlay;

	// Token: 0x040055BE RID: 21950
	public Action<Color> OnColorChanged;

	// Token: 0x040055BF RID: 21951
	[PublicizedFrom(EAccessModifier.Private)]
	public float hue;

	// Token: 0x040055C0 RID: 21952
	[PublicizedFrom(EAccessModifier.Private)]
	public float saturation = 1f;

	// Token: 0x040055C1 RID: 21953
	[PublicizedFrom(EAccessModifier.Private)]
	public float value = 1f;

	// Token: 0x040055C2 RID: 21954
	[PublicizedFrom(EAccessModifier.Private)]
	public float alpha = 1f;

	// Token: 0x040055C3 RID: 21955
	[PublicizedFrom(EAccessModifier.Private)]
	public float cursorRadius = 10f;

	// Token: 0x040055C4 RID: 21956
	[PublicizedFrom(EAccessModifier.Private)]
	public bool editAlpha = true;

	// Token: 0x040055C5 RID: 21957
	[PublicizedFrom(EAccessModifier.Private)]
	public bool pickersDirty = true;

	// Token: 0x040055C6 RID: 21958
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bindingsDirty = true;

	// Token: 0x040055C7 RID: 21959
	[PublicizedFrom(EAccessModifier.Private)]
	public bool samplerActive;

	// Token: 0x040055C8 RID: 21960
	[PublicizedFrom(EAccessModifier.Private)]
	public Color samplerColor = Color.clear;

	// Token: 0x040055C9 RID: 21961
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Texture2D samplerTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);

	// Token: 0x040055CA RID: 21962
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hoverOverSampler;
}
