using System;
using System.Collections;
using GUI_2;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E5C RID: 3676
[Preserve]
public class XUiC_ColorPicker : XUiController
{
	// Token: 0x140000A7 RID: 167
	// (add) Token: 0x0600717A RID: 29050 RVA: 0x002B6F78 File Offset: 0x002B5178
	// (remove) Token: 0x0600717B RID: 29051 RVA: 0x002B6FB0 File Offset: 0x002B51B0
	public event XUiEvent_SelectedColorChanged OnSelectedColorChanged;

	// Token: 0x17000CEC RID: 3308
	// (get) Token: 0x0600717C RID: 29052 RVA: 0x002B6FE5 File Offset: 0x002B51E5
	// (set) Token: 0x0600717D RID: 29053 RVA: 0x002B6FF0 File Offset: 0x002B51F0
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

	// Token: 0x0600717E RID: 29054 RVA: 0x002B70CC File Offset: 0x002B52CC
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

	// Token: 0x0600717F RID: 29055 RVA: 0x002B71D3 File Offset: 0x002B53D3
	public override void OnClose()
	{
		base.OnClose();
		this.hideCallouts();
		this.xui.playerUI.CursorController.Locked = false;
	}

	// Token: 0x06007180 RID: 29056 RVA: 0x002B71F8 File Offset: 0x002B53F8
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

	// Token: 0x06007181 RID: 29057 RVA: 0x002B7420 File Offset: 0x002B5620
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

	// Token: 0x06007182 RID: 29058 RVA: 0x002B74DD File Offset: 0x002B56DD
	[PublicizedFrom(EAccessModifier.Private)]
	public void hideCallouts()
	{
		this.xui.CalloutWindow.ClearCallouts(XUiC_GamepadCalloutWindow.CalloutType.ColorPicker);
		this.showingCallouts = false;
	}

	// Token: 0x06007183 RID: 29059 RVA: 0x002B74F8 File Offset: 0x002B56F8
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

	// Token: 0x06007184 RID: 29060 RVA: 0x002B7507 File Offset: 0x002B5707
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

	// Token: 0x06007185 RID: 29061 RVA: 0x002B7516 File Offset: 0x002B5716
	[PublicizedFrom(EAccessModifier.Private)]
	public Color getColorUnderMouse(Texture2D _tex)
	{
		_tex.ReadPixels(new Rect(Input.mousePosition, Vector2.one), 0, 0);
		_tex.Apply();
		return _tex.GetPixel(0, 0);
	}

	// Token: 0x06007186 RID: 29062 RVA: 0x002B7544 File Offset: 0x002B5744
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

	// Token: 0x06007187 RID: 29063 RVA: 0x002B761C File Offset: 0x002B581C
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

	// Token: 0x06007188 RID: 29064 RVA: 0x002B76DE File Offset: 0x002B58DE
	[PublicizedFrom(EAccessModifier.Private)]
	public void SvPickerC_OnDrag(XUiController _sender, EDragType _dragType, Vector2 _mousePositionDelta)
	{
		GameManager.Instance.StartCoroutine(this.setSatVal());
	}

	// Token: 0x06007189 RID: 29065 RVA: 0x002B76DE File Offset: 0x002B58DE
	[PublicizedFrom(EAccessModifier.Private)]
	public void SvPickerC_OnPress(XUiController _sender, int _mouseButton)
	{
		GameManager.Instance.StartCoroutine(this.setSatVal());
	}

	// Token: 0x0600718A RID: 29066 RVA: 0x002B76F1 File Offset: 0x002B58F1
	[PublicizedFrom(EAccessModifier.Private)]
	public void HPickerC_OnDrag(XUiController _sender, EDragType _dragType, Vector2 _mousePositionDelta)
	{
		GameManager.Instance.StartCoroutine(this.setHue());
	}

	// Token: 0x0600718B RID: 29067 RVA: 0x002B76F1 File Offset: 0x002B58F1
	[PublicizedFrom(EAccessModifier.Private)]
	public void HPickerC_OnPress(XUiController _sender, int _mouseButton)
	{
		GameManager.Instance.StartCoroutine(this.setHue());
	}

	// Token: 0x04005536 RID: 21814
	[PublicizedFrom(EAccessModifier.Private)]
	public Color selectedColor = Color.red;

	// Token: 0x04005537 RID: 21815
	[PublicizedFrom(EAccessModifier.Private)]
	public float saturation;

	// Token: 0x04005538 RID: 21816
	[PublicizedFrom(EAccessModifier.Private)]
	public float vibrance;

	// Token: 0x04005539 RID: 21817
	[PublicizedFrom(EAccessModifier.Private)]
	public float hue;

	// Token: 0x0400553A RID: 21818
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isLocalDirty;

	// Token: 0x0400553B RID: 21819
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Texture svPicker;

	// Token: 0x0400553C RID: 21820
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Texture hPicker;

	// Token: 0x0400553D RID: 21821
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite selectedColorSprite;

	// Token: 0x0400553E RID: 21822
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite selectedColorSatValPointerSprite;

	// Token: 0x0400553F RID: 21823
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showingCallouts;

	// Token: 0x04005540 RID: 21824
	[PublicizedFrom(EAccessModifier.Private)]
	public bool calloutStateHeld;
}
