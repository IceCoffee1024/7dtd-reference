using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E54 RID: 3668
[Preserve]
public class XUiC_AdvancedColorPicker : XUiController
{
	// Token: 0x17000CD6 RID: 3286
	// (get) Token: 0x06007112 RID: 28946 RVA: 0x002B6031 File Offset: 0x002B4231
	// (set) Token: 0x06007113 RID: 28947 RVA: 0x002B6039 File Offset: 0x002B4239
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

	// Token: 0x17000CD7 RID: 3287
	// (get) Token: 0x06007114 RID: 28948 RVA: 0x002B6051 File Offset: 0x002B4251
	// (set) Token: 0x06007115 RID: 28949 RVA: 0x002B6059 File Offset: 0x002B4259
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

	// Token: 0x17000CD8 RID: 3288
	// (get) Token: 0x06007116 RID: 28950 RVA: 0x002B6073 File Offset: 0x002B4273
	[XuiXmlBinding("has_copy")]
	public bool HasCopyBuffer
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return XUiC_AdvancedColorPicker.copyBuffer != null;
		}
	}

	// Token: 0x17000CD9 RID: 3289
	// (get) Token: 0x06007117 RID: 28951 RVA: 0x002B607F File Offset: 0x002B427F
	// (set) Token: 0x06007118 RID: 28952 RVA: 0x002B6087 File Offset: 0x002B4287
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

	// Token: 0x06007119 RID: 28953 RVA: 0x002B60A4 File Offset: 0x002B42A4
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

	// Token: 0x0600711A RID: 28954 RVA: 0x002B6208 File Offset: 0x002B4408
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

	// Token: 0x0600711B RID: 28955 RVA: 0x002B6238 File Offset: 0x002B4438
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

	// Token: 0x0600711C RID: 28956 RVA: 0x002B6264 File Offset: 0x002B4464
	[PublicizedFrom(EAccessModifier.Private)]
	public void updatePickerRects()
	{
		this.svPickerRect = this.svPicker.GetXUiRect();
		this.hPickerRect = this.hPicker.GetXUiRect();
		this.aPickerRect = this.aPicker.GetXUiRect();
		this.pickersDirty = false;
	}

	// Token: 0x0600711D RID: 28957 RVA: 0x002B62A0 File Offset: 0x002B44A0
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

	// Token: 0x0600711E RID: 28958 RVA: 0x002B62D7 File Offset: 0x002B44D7
	[XuiBindEvent("OnPressed", "btnCopy")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCopy_OnPressed(XUiController _sender, int _mouseButton)
	{
		XUiC_AdvancedColorPicker.copyBuffer = new Color?(this.getCurrentColor());
		base.RefreshBindings();
	}

	// Token: 0x0600711F RID: 28959 RVA: 0x002B62EF File Offset: 0x002B44EF
	[XuiBindEvent("OnPressed", "btnPaste")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnPaste_OnPressed(XUiController _sender, int _mouseButton)
	{
		if (XUiC_AdvancedColorPicker.copyBuffer != null)
		{
			this.SetColor(XUiC_AdvancedColorPicker.copyBuffer.Value, true);
		}
	}

	// Token: 0x06007120 RID: 28960 RVA: 0x002B630E File Offset: 0x002B450E
	[XuiBindEvent("OnPress", "btnSampler")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnSampler_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.samplerActive = true;
		GameManager.Instance.StartCoroutine(this.samplerCoroutine());
		this.bindingsDirty = true;
	}

	// Token: 0x06007121 RID: 28961 RVA: 0x002B632F File Offset: 0x002B452F
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

	// Token: 0x06007122 RID: 28962 RVA: 0x002B633E File Offset: 0x002B453E
	[XuiBindEvent("OnPress", "btnSamplerOverlay")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnSamplerOverlay_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.SetColor(this.samplerColor, true);
		this.samplerActive = false;
		this.bindingsDirty = true;
	}

	// Token: 0x06007123 RID: 28963 RVA: 0x002B635B File Offset: 0x002B455B
	[XuiBindEvent("OnRightPress", "btnSamplerOverlay")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnSamplerOverlay_OnRightPressed(XUiController _sender, int _mouseButton)
	{
		this.samplerActive = false;
		this.bindingsDirty = true;
	}

	// Token: 0x06007124 RID: 28964 RVA: 0x002B636B File Offset: 0x002B456B
	[PublicizedFrom(EAccessModifier.Private)]
	public void SamplerColor_OnHover(XUiController _sender, bool _isOver)
	{
		this.hoverOverSampler = _isOver;
	}

	// Token: 0x06007125 RID: 28965 RVA: 0x002B6374 File Offset: 0x002B4574
	[PublicizedFrom(EAccessModifier.Private)]
	public void aPicker_MouseUpdate()
	{
		Vector2 relativeMousePos = this.getRelativeMousePos(this.aPickerRect);
		this.alpha = Mathf.Clamp01(relativeMousePos.x);
		this.OnHSVAChanged();
	}

	// Token: 0x06007126 RID: 28966 RVA: 0x002B63A8 File Offset: 0x002B45A8
	[PublicizedFrom(EAccessModifier.Private)]
	public void hPicker_MouseUpdate()
	{
		Vector2 relativeMousePos = this.getRelativeMousePos(this.hPickerRect);
		this.hue = Mathf.Clamp01(relativeMousePos.x);
		this.OnHSVAChanged();
	}

	// Token: 0x06007127 RID: 28967 RVA: 0x002B63DC File Offset: 0x002B45DC
	[PublicizedFrom(EAccessModifier.Private)]
	public void svPicker_MouseUpdate()
	{
		Vector2 relativeMousePos = this.getRelativeMousePos(this.svPickerRect);
		this.saturation = Mathf.Clamp01(relativeMousePos.x);
		this.value = Mathf.Clamp01(relativeMousePos.y);
		this.OnHSVAChanged();
	}

	// Token: 0x06007128 RID: 28968 RVA: 0x002B6420 File Offset: 0x002B4620
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 getRelativeMousePos(Rect _controllerXUiRect)
	{
		return (this.xui.GetMouseXUiPosition().AsVector2() - _controllerXUiRect.min) / _controllerXUiRect.size;
	}

	// Token: 0x06007129 RID: 28969 RVA: 0x002B6458 File Offset: 0x002B4658
	[PublicizedFrom(EAccessModifier.Private)]
	public void aPicker_OnRender(Material _mat)
	{
		_mat.SetVector(XUiC_AdvancedColorPicker._Dimensions, this.aPicker.Size.AsVector2());
		Color currentColor = this.getCurrentColor();
		_mat.SetColor(XUiC_AdvancedColorPicker._Color, currentColor);
		_mat.SetFloat(XUiC_AdvancedColorPicker._CursorSize, this.CursorRadius);
	}

	// Token: 0x0600712A RID: 28970 RVA: 0x002B64AC File Offset: 0x002B46AC
	[PublicizedFrom(EAccessModifier.Private)]
	public void hPicker_OnRender(Material _mat)
	{
		_mat.SetVector(XUiC_AdvancedColorPicker._Dimensions, this.hPicker.Size.AsVector2());
		_mat.SetFloat(XUiC_AdvancedColorPicker._Hue, this.hue);
		_mat.SetFloat(XUiC_AdvancedColorPicker._CursorSize, this.CursorRadius);
	}

	// Token: 0x0600712B RID: 28971 RVA: 0x002B6500 File Offset: 0x002B4700
	[PublicizedFrom(EAccessModifier.Private)]
	public void svPicker_OnRender(Material _mat)
	{
		_mat.SetVector(XUiC_AdvancedColorPicker._Dimensions, this.svPicker.Size.AsVector2());
		_mat.SetFloat(XUiC_AdvancedColorPicker._Hue, this.hue);
		_mat.SetFloat(XUiC_AdvancedColorPicker._Saturation, this.saturation);
		_mat.SetFloat(XUiC_AdvancedColorPicker._Value, this.value);
		_mat.SetFloat(XUiC_AdvancedColorPicker._CursorSize, this.CursorRadius);
	}

	// Token: 0x0600712C RID: 28972 RVA: 0x002B6574 File Offset: 0x002B4774
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

	// Token: 0x0600712D RID: 28973 RVA: 0x002B65EC File Offset: 0x002B47EC
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

	// Token: 0x0600712E RID: 28974 RVA: 0x002B66E0 File Offset: 0x002B48E0
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

	// Token: 0x0600712F RID: 28975 RVA: 0x002B6720 File Offset: 0x002B4920
	[PublicizedFrom(EAccessModifier.Private)]
	public Color getCurrentColor()
	{
		Color color = Color.HSVToRGB(this.hue, this.saturation, this.value);
		color.a = this.alpha;
		return color;
	}

	// Token: 0x06007130 RID: 28976 RVA: 0x002B6753 File Offset: 0x002B4953
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateHsva(Color _color)
	{
		Color.RGBToHSV(_color, out this.hue, out this.saturation, out this.value);
		this.alpha = _color.a;
	}

	// Token: 0x06007131 RID: 28977 RVA: 0x002B677C File Offset: 0x002B497C
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateRgba(Color _color)
	{
		Color32 color = _color;
		this.txtColorR.Text = color.r.ToString();
		this.txtColorG.Text = color.g.ToString();
		this.txtColorB.Text = color.b.ToString();
		this.txtColorA.Text = color.a.ToString();
	}

	// Token: 0x06007132 RID: 28978 RVA: 0x002B67EC File Offset: 0x002B49EC
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateHex(Color _color)
	{
		this.txtHex.Text = (this.EditAlpha ? ColorUtility.ToHtmlStringRGBA(_color) : ColorUtility.ToHtmlStringRGB(_color));
	}

	// Token: 0x06007133 RID: 28979 RVA: 0x002B680F File Offset: 0x002B4A0F
	[Conditional("DEBUG_LOG_PICKERS")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void debugLog(string _message)
	{
		global::UnityEngine.Debug.Log("[AdvancedColorPicker] " + _message);
	}

	// Token: 0x06007136 RID: 28982 RVA: 0x002B68FC File Offset: 0x002B4AFC
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

	// Token: 0x04005502 RID: 21762
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _Dimensions = Shader.PropertyToID("_Dimensions");

	// Token: 0x04005503 RID: 21763
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _Saturation = Shader.PropertyToID("_Saturation");

	// Token: 0x04005504 RID: 21764
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _Value = Shader.PropertyToID("_Value");

	// Token: 0x04005505 RID: 21765
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _Hue = Shader.PropertyToID("_Hue");

	// Token: 0x04005506 RID: 21766
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _Color = Shader.PropertyToID("_Color");

	// Token: 0x04005507 RID: 21767
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _CursorSize = Shader.PropertyToID("_CursorSize");

	// Token: 0x04005508 RID: 21768
	[PublicizedFrom(EAccessModifier.Private)]
	public static Color? copyBuffer;

	// Token: 0x04005509 RID: 21769
	[XuiBindComponent("svPicker", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Texture svPicker;

	// Token: 0x0400550A RID: 21770
	[XuiBindComponent("hPicker", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Texture hPicker;

	// Token: 0x0400550B RID: 21771
	[XuiBindComponent("aPicker", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Texture aPicker;

	// Token: 0x0400550C RID: 21772
	[PublicizedFrom(EAccessModifier.Private)]
	public Rect svPickerRect;

	// Token: 0x0400550D RID: 21773
	[PublicizedFrom(EAccessModifier.Private)]
	public Rect hPickerRect;

	// Token: 0x0400550E RID: 21774
	[PublicizedFrom(EAccessModifier.Private)]
	public Rect aPickerRect;

	// Token: 0x0400550F RID: 21775
	[XuiBindComponent("selectedColor", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite selectedColorSprite;

	// Token: 0x04005510 RID: 21776
	[XuiBindComponent("samplerColor", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite samplerColorSprite;

	// Token: 0x04005511 RID: 21777
	[XuiBindComponent("txtColorR", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtColorR;

	// Token: 0x04005512 RID: 21778
	[XuiBindComponent("txtColorG", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtColorG;

	// Token: 0x04005513 RID: 21779
	[XuiBindComponent("txtColorB", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtColorB;

	// Token: 0x04005514 RID: 21780
	[XuiBindComponent("txtColorA", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtColorA;

	// Token: 0x04005515 RID: 21781
	[XuiBindComponent("txtHex", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtHex;

	// Token: 0x04005516 RID: 21782
	[XuiBindComponent("btnCopy", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_SimpleButton btnCopy;

	// Token: 0x04005517 RID: 21783
	[XuiBindComponent("btnPaste", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_SimpleButton btnPaste;

	// Token: 0x04005518 RID: 21784
	[XuiBindComponent("btnSampler", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiController btnSampler;

	// Token: 0x04005519 RID: 21785
	[XuiBindComponent("btnSamplerOverlay", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiController btnSamplerOverlay;

	// Token: 0x0400551A RID: 21786
	public Action<Color> OnColorChanged;

	// Token: 0x0400551B RID: 21787
	[PublicizedFrom(EAccessModifier.Private)]
	public float hue;

	// Token: 0x0400551C RID: 21788
	[PublicizedFrom(EAccessModifier.Private)]
	public float saturation = 1f;

	// Token: 0x0400551D RID: 21789
	[PublicizedFrom(EAccessModifier.Private)]
	public float value = 1f;

	// Token: 0x0400551E RID: 21790
	[PublicizedFrom(EAccessModifier.Private)]
	public float alpha = 1f;

	// Token: 0x0400551F RID: 21791
	[PublicizedFrom(EAccessModifier.Private)]
	public float cursorRadius = 10f;

	// Token: 0x04005520 RID: 21792
	[PublicizedFrom(EAccessModifier.Private)]
	public bool editAlpha = true;

	// Token: 0x04005521 RID: 21793
	[PublicizedFrom(EAccessModifier.Private)]
	public bool pickersDirty = true;

	// Token: 0x04005522 RID: 21794
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bindingsDirty = true;

	// Token: 0x04005523 RID: 21795
	[PublicizedFrom(EAccessModifier.Private)]
	public bool samplerActive;

	// Token: 0x04005524 RID: 21796
	[PublicizedFrom(EAccessModifier.Private)]
	public Color samplerColor = Color.clear;

	// Token: 0x04005525 RID: 21797
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Texture2D samplerTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);

	// Token: 0x04005526 RID: 21798
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hoverOverSampler;
}
