using System;
using System.Collections;
using GUI_2;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E71 RID: 3697
[Preserve]
public class XUiC_ColorPicker : XUiController
{
	// Token: 0x140000A8 RID: 168
	// (add) Token: 0x0600721A RID: 29210 RVA: 0x002BA49C File Offset: 0x002B869C
	// (remove) Token: 0x0600721B RID: 29211 RVA: 0x002BA4D4 File Offset: 0x002B86D4
	public event XUiEvent_SelectedColorChanged OnSelectedColorChanged;

	// Token: 0x17000D05 RID: 3333
	// (get) Token: 0x0600721C RID: 29212 RVA: 0x002BA509 File Offset: 0x002B8709
	// (set) Token: 0x0600721D RID: 29213 RVA: 0x002BA514 File Offset: 0x002B8714
	public Color SelectedColor
	{
		get
		{
			return this.selectedColor;
		}
		set
		{
			this.selectedColor = value;
			this.hue = (float)HSVUtil.ConvertRgbToHsv(this.selectedColor).H;
			this.saturation = (float)HSVUtil.ConvertRgbToHsv(this.selectedColor).S;
			this.vibrance = (float)HSVUtil.ConvertRgbToHsv(this.selectedColor).V;
			this.setupSaturationVibranceTexture();
			float num = this.saturation * (float)this.svPicker.Size.x;
			float num2 = this.vibrance * (float)this.svPicker.Size.y;
			this.selectedColorSatValPointerSprite.IsVisible = true;
			this.selectedColorSatValPointerSprite.Position = new Vector2i((int)num, -(int)num2);
			this.selectedColorSatValPointerSprite.UiTransform.localPosition = new Vector3(num, -num2);
			this.selectedColorSprite.Color = this.selectedColor;
		}
	}

	// Token: 0x0600721E RID: 29214 RVA: 0x002BA5F0 File Offset: 0x002B87F0
	public override void Init()
	{
		base.Init();
		XUiController childById = base.GetChildById("svPicker");
		XUiController childById2 = base.GetChildById("hPicker");
		XUiController childById3 = base.GetChildById("selectedColorSVPointer");
		if (childById != null)
		{
			XUiV_Texture xuiV_Texture = childById.ViewComponent as XUiV_Texture;
			if (xuiV_Texture != null)
			{
				this.svPicker = xuiV_Texture;
				childById.OnPress += this.SvPickerC_OnPress;
				childById.OnDrag += this.SvPickerC_OnDrag;
			}
		}
		if (childById2 != null)
		{
			XUiV_Texture xuiV_Texture2 = childById2.ViewComponent as XUiV_Texture;
			if (xuiV_Texture2 != null)
			{
				this.hPicker = xuiV_Texture2;
				childById2.OnPress += this.HPickerC_OnPress;
				childById2.OnDrag += this.HPickerC_OnDrag;
			}
		}
		XUiController childById4 = base.GetChildById("selectedColor");
		if (childById4 != null)
		{
			XUiV_Sprite xuiV_Sprite = childById4.ViewComponent as XUiV_Sprite;
			if (xuiV_Sprite != null)
			{
				this.selectedColorSprite = xuiV_Sprite;
			}
		}
		if (childById3 != null)
		{
			XUiV_Sprite xuiV_Sprite2 = childById3.ViewComponent as XUiV_Sprite;
			if (xuiV_Sprite2 != null)
			{
				this.selectedColorSatValPointerSprite = xuiV_Sprite2;
			}
		}
		this.setupHueTexture();
		this.setupSaturationVibranceTexture();
	}

	// Token: 0x0600721F RID: 29215 RVA: 0x002BA6F7 File Offset: 0x002B88F7
	public override void OnClose()
	{
		base.OnClose();
		this.hideCallouts();
		this.xui.playerUI.CursorController.Locked = false;
	}

	// Token: 0x06007220 RID: 29216 RVA: 0x002BA71C File Offset: 0x002B891C
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.xui.playerUI.CursorController.navigationTarget == this.svPicker && PlatformManager.NativePlatform.Input.CurrentInputStyle != PlayerInputManager.InputStyle.Keyboard)
		{
			if (this.xui.playerUI.playerInput.GUIActions.Submit.IsPressed)
			{
				this.showCallouts(true);
				this.xui.playerUI.CursorController.Locked = true;
				Vector2 value = this.xui.playerUI.playerInput.GUIActions.Nav.Value;
				if (value != Vector2.zero)
				{
					this.vibrance = Mathf.Clamp01(this.vibrance - value.y * 0.05f);
					this.saturation = Mathf.Clamp01(this.saturation + value.x * 0.05f);
					this.isLocalDirty = true;
				}
				float num = this.hue;
				this.hue = Mathf.Clamp(this.hue + this.xui.playerUI.playerInput.GUIActions.PageUp.Value, 0f, 255f);
				this.hue = Mathf.Clamp(this.hue - this.xui.playerUI.playerInput.GUIActions.PageDown.Value, 0f, 255f);
				if (!Mathf.Approximately(this.hue, num))
				{
					this.isLocalDirty = true;
				}
			}
			else
			{
				this.showCallouts(false);
				this.xui.playerUI.CursorController.Locked = false;
			}
		}
		else
		{
			this.hideCallouts();
			this.xui.playerUI.CursorController.Locked = false;
		}
		if (this.isLocalDirty)
		{
			this.isLocalDirty = false;
			this.selectedColor = HSVUtil.ConvertHsvToRgb((double)this.hue, (double)this.saturation, (double)this.vibrance, 1f);
			this.selectedColorSprite.Color = this.selectedColor;
			if (this.OnSelectedColorChanged != null)
			{
				this.OnSelectedColorChanged(this.selectedColor);
			}
		}
	}

	// Token: 0x06007221 RID: 29217 RVA: 0x002BA944 File Offset: 0x002B8B44
	[PublicizedFrom(EAccessModifier.Private)]
	public void showCallouts(bool _held)
	{
		if (this.showingCallouts && this.calloutStateHeld == _held)
		{
			return;
		}
		this.xui.CalloutWindow.ClearCallouts(XUiC_GamepadCalloutWindow.CalloutType.ColorPicker);
		if (_held)
		{
			this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.LeftStick, "igcoColorPickerSaturationVibrance", XUiC_GamepadCalloutWindow.CalloutType.ColorPicker);
			this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.LeftTrigger, "igcoColorPickerHueMinus", XUiC_GamepadCalloutWindow.CalloutType.ColorPicker);
			this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.RightTrigger, "igcoColorPickerHuePlus", XUiC_GamepadCalloutWindow.CalloutType.ColorPicker);
		}
		else
		{
			this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.FaceButtonSouth, "igcoSelectColorPicker", XUiC_GamepadCalloutWindow.CalloutType.ColorPicker);
		}
		this.xui.CalloutWindow.EnableCallouts(XUiC_GamepadCalloutWindow.CalloutType.ColorPicker, 0f);
		this.calloutStateHeld = _held;
		this.showingCallouts = true;
	}

	// Token: 0x06007222 RID: 29218 RVA: 0x002BAA01 File Offset: 0x002B8C01
	[PublicizedFrom(EAccessModifier.Private)]
	public void hideCallouts()
	{
		this.xui.CalloutWindow.ClearCallouts(XUiC_GamepadCalloutWindow.CalloutType.ColorPicker);
		this.showingCallouts = false;
	}

	// Token: 0x06007223 RID: 29219 RVA: 0x002BAA1C File Offset: 0x002B8C1C
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator setSatVal()
	{
		Texture2D tex = new Texture2D(1, 1, TextureFormat.ARGB32, false);
		this.selectedColorSatValPointerSprite.IsVisible = false;
		yield return new WaitForEndOfFrame();
		Color colorUnderMouse = this.getColorUnderMouse(tex);
		this.saturation = (float)HSVUtil.ConvertRgbToHsv(colorUnderMouse).S;
		this.vibrance = (float)HSVUtil.ConvertRgbToHsv(colorUnderMouse).V;
		float num = this.saturation * (float)this.svPicker.Size.x;
		float num2 = this.vibrance * (float)this.svPicker.Size.y;
		this.selectedColorSatValPointerSprite.IsVisible = true;
		this.selectedColorSatValPointerSprite.Position = new Vector2i((int)num, -(int)num2);
		this.selectedColorSatValPointerSprite.UiTransform.localPosition = new Vector3(num, -num2);
		this.isLocalDirty = true;
		yield break;
	}

	// Token: 0x06007224 RID: 29220 RVA: 0x002BAA2B File Offset: 0x002B8C2B
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator setHue()
	{
		Texture2D tex = new Texture2D(1, 1, TextureFormat.ARGB32, false);
		yield return new WaitForEndOfFrame();
		Color colorUnderMouse = this.getColorUnderMouse(tex);
		this.hue = (float)HSVUtil.ConvertRgbToHsv(colorUnderMouse).H;
		this.setupSaturationVibranceTexture();
		this.isLocalDirty = true;
		yield break;
	}

	// Token: 0x06007225 RID: 29221 RVA: 0x002BAA3A File Offset: 0x002B8C3A
	[PublicizedFrom(EAccessModifier.Private)]
	public Color getColorUnderMouse(Texture2D _tex)
	{
		_tex.ReadPixels(new Rect(Input.mousePosition, Vector2.one), 0, 0);
		_tex.Apply();
		return _tex.GetPixel(0, 0);
	}

	// Token: 0x06007226 RID: 29222 RVA: 0x002BAA68 File Offset: 0x002B8C68
	[PublicizedFrom(EAccessModifier.Private)]
	public void setupSaturationVibranceTexture()
	{
		Texture2D texture2D = new Texture2D(this.svPicker.Size.x, this.svPicker.Size.y);
		texture2D.wrapMode = TextureWrapMode.Clamp;
		for (int i = 0; i < this.svPicker.Size.y; i++)
		{
			float num = 1f - (float)(i + 1) / (float)this.svPicker.Size.y;
			for (int j = 0; j < this.svPicker.Size.x; j++)
			{
				float num2 = (float)(j + 1) / (float)this.svPicker.Size.x;
				texture2D.SetPixel(j, i, HSVUtil.ConvertHsvToRgb((double)this.hue, (double)num2, (double)num, 1f));
			}
		}
		texture2D.Apply();
		this.svPicker.Texture = texture2D;
	}

	// Token: 0x06007227 RID: 29223 RVA: 0x002BAB40 File Offset: 0x002B8D40
	[PublicizedFrom(EAccessModifier.Private)]
	public void setupHueTexture()
	{
		Texture2D texture2D = new Texture2D(this.hPicker.Size.x, this.hPicker.Size.y);
		for (int i = 0; i < this.hPicker.Size.y; i++)
		{
			float num = (float)i / ((float)this.hPicker.Size.y - 1f);
			for (int j = 0; j < this.hPicker.Size.x; j++)
			{
				texture2D.SetPixel(j, i, HSVUtil.ConvertHsvToRgb((double)(num * 360f), 1.0, 1.0, 1f));
			}
		}
		texture2D.Apply();
		this.hPicker.Texture = texture2D;
	}

	// Token: 0x06007228 RID: 29224 RVA: 0x002BAC02 File Offset: 0x002B8E02
	[PublicizedFrom(EAccessModifier.Private)]
	public void SvPickerC_OnDrag(XUiController _sender, EDragType _dragType, Vector2 _mousePositionDelta)
	{
		GameManager.Instance.StartCoroutine(this.setSatVal());
	}

	// Token: 0x06007229 RID: 29225 RVA: 0x002BAC02 File Offset: 0x002B8E02
	[PublicizedFrom(EAccessModifier.Private)]
	public void SvPickerC_OnPress(XUiController _sender, int _mouseButton)
	{
		GameManager.Instance.StartCoroutine(this.setSatVal());
	}

	// Token: 0x0600722A RID: 29226 RVA: 0x002BAC15 File Offset: 0x002B8E15
	[PublicizedFrom(EAccessModifier.Private)]
	public void HPickerC_OnDrag(XUiController _sender, EDragType _dragType, Vector2 _mousePositionDelta)
	{
		GameManager.Instance.StartCoroutine(this.setHue());
	}

	// Token: 0x0600722B RID: 29227 RVA: 0x002BAC15 File Offset: 0x002B8E15
	[PublicizedFrom(EAccessModifier.Private)]
	public void HPickerC_OnPress(XUiController _sender, int _mouseButton)
	{
		GameManager.Instance.StartCoroutine(this.setHue());
	}

	// Token: 0x040055DA RID: 21978
	[PublicizedFrom(EAccessModifier.Private)]
	public Color selectedColor = Color.red;

	// Token: 0x040055DB RID: 21979
	[PublicizedFrom(EAccessModifier.Private)]
	public float saturation;

	// Token: 0x040055DC RID: 21980
	[PublicizedFrom(EAccessModifier.Private)]
	public float vibrance;

	// Token: 0x040055DD RID: 21981
	[PublicizedFrom(EAccessModifier.Private)]
	public float hue;

	// Token: 0x040055DE RID: 21982
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isLocalDirty;

	// Token: 0x040055DF RID: 21983
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Texture svPicker;

	// Token: 0x040055E0 RID: 21984
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Texture hPicker;

	// Token: 0x040055E1 RID: 21985
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite selectedColorSprite;

	// Token: 0x040055E2 RID: 21986
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite selectedColorSatValPointerSprite;

	// Token: 0x040055E3 RID: 21987
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showingCallouts;

	// Token: 0x040055E4 RID: 21988
	[PublicizedFrom(EAccessModifier.Private)]
	public bool calloutStateHeld;
}
