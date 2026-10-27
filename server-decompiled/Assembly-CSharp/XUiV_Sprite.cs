using System;
using UnityEngine;

// Token: 0x02001188 RID: 4488
public class XUiV_Sprite : XUiV_ImageBased
{
	// Token: 0x17001129 RID: 4393
	// (get) Token: 0x06008ECD RID: 36557 RVA: 0x0035F2E2 File Offset: 0x0035D4E2
	public UISprite Sprite
	{
		get
		{
			return this.sprite;
		}
	}

	// Token: 0x1700112A RID: 4394
	// (get) Token: 0x06008ECE RID: 36558 RVA: 0x0035F2EA File Offset: 0x0035D4EA
	// (set) Token: 0x06008ECF RID: 36559 RVA: 0x0035F2F2 File Offset: 0x0035D4F2
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

	// Token: 0x1700112B RID: 4395
	// (get) Token: 0x06008ED0 RID: 36560 RVA: 0x0035F317 File Offset: 0x0035D517
	// (set) Token: 0x06008ED1 RID: 36561 RVA: 0x0035F31F File Offset: 0x0035D51F
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

	// Token: 0x1700112C RID: 4396
	// (get) Token: 0x06008ED2 RID: 36562 RVA: 0x0035F33D File Offset: 0x0035D53D
	// (set) Token: 0x06008ED3 RID: 36563 RVA: 0x0035F345 File Offset: 0x0035D545
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

	// Token: 0x1700112D RID: 4397
	// (get) Token: 0x06008ED4 RID: 36564 RVA: 0x0035F363 File Offset: 0x0035D563
	// (set) Token: 0x06008ED5 RID: 36565 RVA: 0x0035F36B File Offset: 0x0035D56B
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

	// Token: 0x1700112E RID: 4398
	// (get) Token: 0x06008ED6 RID: 36566 RVA: 0x0035F384 File Offset: 0x0035D584
	// (set) Token: 0x06008ED7 RID: 36567 RVA: 0x0035F38C File Offset: 0x0035D58C
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

	// Token: 0x1700112F RID: 4399
	// (get) Token: 0x06008ED8 RID: 36568 RVA: 0x0035F3A5 File Offset: 0x0035D5A5
	// (set) Token: 0x06008ED9 RID: 36569 RVA: 0x0035F3AD File Offset: 0x0035D5AD
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

	// Token: 0x17001130 RID: 4400
	// (get) Token: 0x06008EDA RID: 36570 RVA: 0x0035F3C6 File Offset: 0x0035D5C6
	// (set) Token: 0x06008EDB RID: 36571 RVA: 0x0035F3CE File Offset: 0x0035D5CE
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

	// Token: 0x06008EDC RID: 36572 RVA: 0x0035F40D File Offset: 0x0035D60D
	public XUiV_Sprite(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x06008EDD RID: 36573 RVA: 0x0035F438 File Offset: 0x0035D638
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createComponents(GameObject _go)
	{
		_go.AddComponent<UISprite>();
	}

	// Token: 0x06008EDE RID: 36574 RVA: 0x0035F444 File Offset: 0x0035D644
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void captureComponents()
	{
		base.captureComponents();
		this.widget = (this.sprite = this.uiTransform.GetComponent<UISprite>());
	}

	// Token: 0x06008EDF RID: 36575 RVA: 0x0035F471 File Offset: 0x0035D671
	public override void InitView()
	{
		base.InitView();
		this.updateData();
	}

	// Token: 0x06008EE0 RID: 36576 RVA: 0x0035F47F File Offset: 0x0035D67F
	public override void SetDefaults(XUiController _parent)
	{
		base.SetDefaults(_parent);
		base.FillCenter = true;
		this.Type = UIBasicSprite.Type.Simple;
		this.FillDirection = UIBasicSprite.FillDirection.Horizontal;
	}

	// Token: 0x06008EE1 RID: 36577 RVA: 0x0035F4A0 File Offset: 0x0035D6A0
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

	// Token: 0x06008EE2 RID: 36578 RVA: 0x0035F5F7 File Offset: 0x0035D7F7
	public void SetSpriteImmediately(string _spriteName)
	{
		this.spriteName = _spriteName;
		this.applyAtlasAndSprite(true);
	}

	// Token: 0x06008EE3 RID: 36579 RVA: 0x0035F608 File Offset: 0x0035D808
	public void SetColorImmediately(Color _color)
	{
		if (this.sprite != null)
		{
			this.sprite.color = _color;
		}
	}

	// Token: 0x06008EE4 RID: 36580 RVA: 0x0035F624 File Offset: 0x0035D824
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

	// Token: 0x06008EE5 RID: 36581 RVA: 0x0035F6BB File Offset: 0x0035D8BB
	[XuiXmlAttribute("gradient_start", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeGradientStart(Color _value)
	{
		this.gradientStart = new Color?(_value);
	}

	// Token: 0x06008EE6 RID: 36582 RVA: 0x0035F6C9 File Offset: 0x0035D8C9
	[XuiXmlAttribute("gradient_end", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeGradientEnd(Color _value)
	{
		this.gradientEnd = new Color?(_value);
	}

	// Token: 0x040068DC RID: 26844
	[PublicizedFrom(EAccessModifier.Private)]
	public string uiAtlas = string.Empty;

	// Token: 0x040068DD RID: 26845
	[PublicizedFrom(EAccessModifier.Private)]
	public bool uiAtlasChanged;

	// Token: 0x040068DE RID: 26846
	[PublicizedFrom(EAccessModifier.Protected)]
	public string spriteName = string.Empty;

	// Token: 0x040068DF RID: 26847
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color color = Color.white;

	// Token: 0x040068E0 RID: 26848
	[PublicizedFrom(EAccessModifier.Private)]
	public Color? gradientStart;

	// Token: 0x040068E1 RID: 26849
	[PublicizedFrom(EAccessModifier.Private)]
	public Color? gradientEnd;

	// Token: 0x040068E2 RID: 26850
	[PublicizedFrom(EAccessModifier.Protected)]
	public UIBasicSprite.FillDirection fillDirection;

	// Token: 0x040068E3 RID: 26851
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool fillSpritePad;

	// Token: 0x040068E4 RID: 26852
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool fillInvert;

	// Token: 0x040068E5 RID: 26853
	[PublicizedFrom(EAccessModifier.Protected)]
	public float fillAmount;

	// Token: 0x040068E6 RID: 26854
	[PublicizedFrom(EAccessModifier.Protected)]
	public UISprite sprite;
}
