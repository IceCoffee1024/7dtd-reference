using System;
using UnityEngine;

// Token: 0x02001147 RID: 4423
public class XUiV_Sprite : XUiV_ImageBased
{
	// Token: 0x170010A1 RID: 4257
	// (get) Token: 0x06008CFC RID: 36092 RVA: 0x00354213 File Offset: 0x00352413
	public UISprite Sprite
	{
		get
		{
			return this.sprite;
		}
	}

	// Token: 0x170010A2 RID: 4258
	// (get) Token: 0x06008CFD RID: 36093 RVA: 0x0035421B File Offset: 0x0035241B
	// (set) Token: 0x06008CFE RID: 36094 RVA: 0x00354223 File Offset: 0x00352423
	[XuiXmlAttribute("atlas", false)]
	public string UIAtlas
	{
		get
		{
			return this.uiAtlas;
		}
		set
		{
			if (this.uiAtlas == value)
			{
				return;
			}
			this.uiAtlas = value;
			this.uiAtlasChanged = true;
			base.SetDirty();
		}
	}

	// Token: 0x170010A3 RID: 4259
	// (get) Token: 0x06008CFF RID: 36095 RVA: 0x00354248 File Offset: 0x00352448
	// (set) Token: 0x06008D00 RID: 36096 RVA: 0x00354250 File Offset: 0x00352450
	[XuiXmlAttribute("sprite", false)]
	public string SpriteName
	{
		get
		{
			return this.spriteName;
		}
		set
		{
			if (this.spriteName == value)
			{
				return;
			}
			this.spriteName = value;
			base.SetDirty();
		}
	}

	// Token: 0x170010A4 RID: 4260
	// (get) Token: 0x06008D01 RID: 36097 RVA: 0x0035426E File Offset: 0x0035246E
	// (set) Token: 0x06008D02 RID: 36098 RVA: 0x00354276 File Offset: 0x00352476
	[XuiXmlAttribute("color", false)]
	public Color Color
	{
		get
		{
			return this.color;
		}
		set
		{
			if (this.color == value)
			{
				return;
			}
			this.color = value;
			base.SetDirty();
		}
	}

	// Token: 0x170010A5 RID: 4261
	// (get) Token: 0x06008D03 RID: 36099 RVA: 0x00354294 File Offset: 0x00352494
	// (set) Token: 0x06008D04 RID: 36100 RVA: 0x0035429C File Offset: 0x0035249C
	[XuiXmlAttribute("filldirection", false)]
	public UIBasicSprite.FillDirection FillDirection
	{
		get
		{
			return this.fillDirection;
		}
		set
		{
			if (this.fillDirection == value)
			{
				return;
			}
			this.fillDirection = value;
			base.SetDirty();
		}
	}

	// Token: 0x170010A6 RID: 4262
	// (get) Token: 0x06008D05 RID: 36101 RVA: 0x003542B5 File Offset: 0x003524B5
	// (set) Token: 0x06008D06 RID: 36102 RVA: 0x003542BD File Offset: 0x003524BD
	[XuiXmlAttribute("fillspritepad", false)]
	public bool FillSpritePad
	{
		get
		{
			return this.fillSpritePad;
		}
		set
		{
			if (this.fillSpritePad == value)
			{
				return;
			}
			this.fillSpritePad = value;
			base.SetDirty();
		}
	}

	// Token: 0x170010A7 RID: 4263
	// (get) Token: 0x06008D07 RID: 36103 RVA: 0x003542D6 File Offset: 0x003524D6
	// (set) Token: 0x06008D08 RID: 36104 RVA: 0x003542DE File Offset: 0x003524DE
	[XuiXmlAttribute("fillinvert", false)]
	public bool FillInvert
	{
		get
		{
			return this.fillInvert;
		}
		set
		{
			if (this.fillInvert == value)
			{
				return;
			}
			this.fillInvert = value;
			base.SetDirty();
		}
	}

	// Token: 0x170010A8 RID: 4264
	// (get) Token: 0x06008D09 RID: 36105 RVA: 0x003542F7 File Offset: 0x003524F7
	// (set) Token: 0x06008D0A RID: 36106 RVA: 0x003542FF File Offset: 0x003524FF
	[XuiXmlAttribute("fill", false)]
	public float Fill
	{
		get
		{
			return this.fillAmount;
		}
		set
		{
			if (Mathf.Approximately(this.fillAmount, value))
			{
				return;
			}
			if ((double)Math.Abs((value - this.fillAmount) / value) < 0.005)
			{
				return;
			}
			this.fillAmount = Mathf.Clamp01(value);
			base.SetDirty();
		}
	}

	// Token: 0x06008D0B RID: 36107 RVA: 0x0035433E File Offset: 0x0035253E
	public XUiV_Sprite(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x06008D0C RID: 36108 RVA: 0x00354369 File Offset: 0x00352569
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createComponents(GameObject _go)
	{
		_go.AddComponent<UISprite>();
	}

	// Token: 0x06008D0D RID: 36109 RVA: 0x00354374 File Offset: 0x00352574
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void captureComponents()
	{
		base.captureComponents();
		this.widget = (this.sprite = this.uiTransform.GetComponent<UISprite>());
	}

	// Token: 0x06008D0E RID: 36110 RVA: 0x003543A1 File Offset: 0x003525A1
	public override void InitView()
	{
		base.InitView();
		this.updateData();
	}

	// Token: 0x06008D0F RID: 36111 RVA: 0x003543AF File Offset: 0x003525AF
	public override void SetDefaults(XUiController _parent)
	{
		base.SetDefaults(_parent);
		base.FillCenter = true;
		this.Type = UIBasicSprite.Type.Simple;
		this.FillDirection = UIBasicSprite.FillDirection.Horizontal;
	}

	// Token: 0x06008D10 RID: 36112 RVA: 0x003543D0 File Offset: 0x003525D0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void updateData()
	{
		this.applyAtlasAndSprite(false);
		if (this.fillSpritePad)
		{
			XUiUtils.ApplyFillPaddedSprite(this.sprite, this.spriteName);
		}
		this.sprite.fixedAspect = this.keepSourceAspectRatio;
		this.sprite.color = base.opacityModColor(this.color);
		if (this.gradientStart != null)
		{
			this.sprite.gradientTop = this.gradientStart.Value;
			this.sprite.applyGradient = true;
		}
		if (this.gradientEnd != null)
		{
			this.sprite.gradientBottom = this.gradientEnd.Value;
			this.sprite.applyGradient = true;
		}
		if (this.sprite.centerType != (this.fillCenter ? UIBasicSprite.AdvancedType.Sliced : UIBasicSprite.AdvancedType.Invisible))
		{
			this.sprite.centerType = (this.fillCenter ? UIBasicSprite.AdvancedType.Sliced : UIBasicSprite.AdvancedType.Invisible);
		}
		this.sprite.fillDirection = this.fillDirection;
		this.sprite.invert = this.fillInvert;
		if (!Mathf.Approximately(this.sprite.fillAmount, this.fillAmount))
		{
			this.sprite.fillAmount = this.fillAmount;
		}
		this.sprite.type = this.type;
		this.sprite.flip = this.flip;
		base.updateData();
	}

	// Token: 0x06008D11 RID: 36113 RVA: 0x00354527 File Offset: 0x00352727
	public void SetSpriteImmediately(string _spriteName)
	{
		this.spriteName = _spriteName;
		this.applyAtlasAndSprite(true);
	}

	// Token: 0x06008D12 RID: 36114 RVA: 0x00354538 File Offset: 0x00352738
	public void SetColorImmediately(Color _color)
	{
		if (this.sprite != null)
		{
			this.sprite.color = _color;
		}
	}

	// Token: 0x06008D13 RID: 36115 RVA: 0x00354554 File Offset: 0x00352754
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool applyAtlasAndSprite(bool _force = false)
	{
		if (this.sprite == null)
		{
			return false;
		}
		if (!_force && this.sprite.spriteName != null && this.sprite.spriteName == this.spriteName && this.sprite.atlas != null && !this.uiAtlasChanged)
		{
			return false;
		}
		this.uiAtlasChanged = false;
		this.sprite.atlas = this.xui.GetAtlasByName(this.UIAtlas, this.spriteName);
		this.sprite.spriteName = this.spriteName;
		return true;
	}

	// Token: 0x06008D14 RID: 36116 RVA: 0x003545EB File Offset: 0x003527EB
	[XuiXmlAttribute("gradient_start", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeGradientStart(Color _value)
	{
		this.gradientStart = new Color?(_value);
	}

	// Token: 0x06008D15 RID: 36117 RVA: 0x003545F9 File Offset: 0x003527F9
	[XuiXmlAttribute("gradient_end", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeGradientEnd(Color _value)
	{
		this.gradientEnd = new Color?(_value);
	}

	// Token: 0x0400676D RID: 26477
	[PublicizedFrom(EAccessModifier.Private)]
	public string uiAtlas = string.Empty;

	// Token: 0x0400676E RID: 26478
	[PublicizedFrom(EAccessModifier.Private)]
	public bool uiAtlasChanged;

	// Token: 0x0400676F RID: 26479
	[PublicizedFrom(EAccessModifier.Protected)]
	public string spriteName = string.Empty;

	// Token: 0x04006770 RID: 26480
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color color = Color.white;

	// Token: 0x04006771 RID: 26481
	[PublicizedFrom(EAccessModifier.Private)]
	public Color? gradientStart;

	// Token: 0x04006772 RID: 26482
	[PublicizedFrom(EAccessModifier.Private)]
	public Color? gradientEnd;

	// Token: 0x04006773 RID: 26483
	[PublicizedFrom(EAccessModifier.Protected)]
	public UIBasicSprite.FillDirection fillDirection;

	// Token: 0x04006774 RID: 26484
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool fillSpritePad;

	// Token: 0x04006775 RID: 26485
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool fillInvert;

	// Token: 0x04006776 RID: 26486
	[PublicizedFrom(EAccessModifier.Protected)]
	public float fillAmount;

	// Token: 0x04006777 RID: 26487
	[PublicizedFrom(EAccessModifier.Protected)]
	public UISprite sprite;
}
