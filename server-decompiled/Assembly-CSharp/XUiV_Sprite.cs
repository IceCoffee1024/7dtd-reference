using System;
using UnityEngine;

// Token: 0x02001178 RID: 4472
public class XUiV_Sprite : XUiV_ImageBased
{
	// Token: 0x1700111E RID: 4382
	// (get) Token: 0x06008E79 RID: 36473 RVA: 0x0035D1F2 File Offset: 0x0035B3F2
	public UISprite Sprite
	{
		get
		{
			return this.sprite;
		}
	}

	// Token: 0x1700111F RID: 4383
	// (get) Token: 0x06008E7A RID: 36474 RVA: 0x0035D1FA File Offset: 0x0035B3FA
	// (set) Token: 0x06008E7B RID: 36475 RVA: 0x0035D202 File Offset: 0x0035B402
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

	// Token: 0x17001120 RID: 4384
	// (get) Token: 0x06008E7C RID: 36476 RVA: 0x0035D227 File Offset: 0x0035B427
	// (set) Token: 0x06008E7D RID: 36477 RVA: 0x0035D22F File Offset: 0x0035B42F
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

	// Token: 0x17001121 RID: 4385
	// (get) Token: 0x06008E7E RID: 36478 RVA: 0x0035D24D File Offset: 0x0035B44D
	// (set) Token: 0x06008E7F RID: 36479 RVA: 0x0035D255 File Offset: 0x0035B455
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

	// Token: 0x17001122 RID: 4386
	// (get) Token: 0x06008E80 RID: 36480 RVA: 0x0035D273 File Offset: 0x0035B473
	// (set) Token: 0x06008E81 RID: 36481 RVA: 0x0035D27B File Offset: 0x0035B47B
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

	// Token: 0x17001123 RID: 4387
	// (get) Token: 0x06008E82 RID: 36482 RVA: 0x0035D294 File Offset: 0x0035B494
	// (set) Token: 0x06008E83 RID: 36483 RVA: 0x0035D29C File Offset: 0x0035B49C
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

	// Token: 0x17001124 RID: 4388
	// (get) Token: 0x06008E84 RID: 36484 RVA: 0x0035D2B5 File Offset: 0x0035B4B5
	// (set) Token: 0x06008E85 RID: 36485 RVA: 0x0035D2BD File Offset: 0x0035B4BD
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

	// Token: 0x17001125 RID: 4389
	// (get) Token: 0x06008E86 RID: 36486 RVA: 0x0035D2D6 File Offset: 0x0035B4D6
	// (set) Token: 0x06008E87 RID: 36487 RVA: 0x0035D2DE File Offset: 0x0035B4DE
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

	// Token: 0x06008E88 RID: 36488 RVA: 0x0035D31D File Offset: 0x0035B51D
	public XUiV_Sprite(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x06008E89 RID: 36489 RVA: 0x0035D348 File Offset: 0x0035B548
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createComponents(GameObject _go)
	{
		_go.AddComponent<UISprite>();
	}

	// Token: 0x06008E8A RID: 36490 RVA: 0x0035D354 File Offset: 0x0035B554
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void captureComponents()
	{
		base.captureComponents();
		this.widget = (this.sprite = this.uiTransform.GetComponent<UISprite>());
	}

	// Token: 0x06008E8B RID: 36491 RVA: 0x0035D381 File Offset: 0x0035B581
	public override void InitView()
	{
		base.InitView();
		this.updateData();
	}

	// Token: 0x06008E8C RID: 36492 RVA: 0x0035D38F File Offset: 0x0035B58F
	public override void SetDefaults(XUiController _parent)
	{
		base.SetDefaults(_parent);
		base.FillCenter = true;
		this.Type = UIBasicSprite.Type.Simple;
		this.FillDirection = UIBasicSprite.FillDirection.Horizontal;
	}

	// Token: 0x06008E8D RID: 36493 RVA: 0x0035D3B0 File Offset: 0x0035B5B0
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

	// Token: 0x06008E8E RID: 36494 RVA: 0x0035D507 File Offset: 0x0035B707
	public void SetSpriteImmediately(string _spriteName)
	{
		this.spriteName = _spriteName;
		this.applyAtlasAndSprite(true);
	}

	// Token: 0x06008E8F RID: 36495 RVA: 0x0035D518 File Offset: 0x0035B718
	public void SetColorImmediately(Color _color)
	{
		if (this.sprite != null)
		{
			this.sprite.color = _color;
		}
	}

	// Token: 0x06008E90 RID: 36496 RVA: 0x0035D534 File Offset: 0x0035B734
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

	// Token: 0x06008E91 RID: 36497 RVA: 0x0035D5CB File Offset: 0x0035B7CB
	[XuiXmlAttribute("gradient_start", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeGradientStart(Color _value)
	{
		this.gradientStart = new Color?(_value);
	}

	// Token: 0x06008E92 RID: 36498 RVA: 0x0035D5D9 File Offset: 0x0035B7D9
	[XuiXmlAttribute("gradient_end", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeGradientEnd(Color _value)
	{
		this.gradientEnd = new Color?(_value);
	}

	// Token: 0x0400689C RID: 26780
	[PublicizedFrom(EAccessModifier.Private)]
	public string uiAtlas = string.Empty;

	// Token: 0x0400689D RID: 26781
	[PublicizedFrom(EAccessModifier.Private)]
	public bool uiAtlasChanged;

	// Token: 0x0400689E RID: 26782
	[PublicizedFrom(EAccessModifier.Protected)]
	public string spriteName = string.Empty;

	// Token: 0x0400689F RID: 26783
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color color = Color.white;

	// Token: 0x040068A0 RID: 26784
	[PublicizedFrom(EAccessModifier.Private)]
	public Color? gradientStart;

	// Token: 0x040068A1 RID: 26785
	[PublicizedFrom(EAccessModifier.Private)]
	public Color? gradientEnd;

	// Token: 0x040068A2 RID: 26786
	[PublicizedFrom(EAccessModifier.Protected)]
	public UIBasicSprite.FillDirection fillDirection;

	// Token: 0x040068A3 RID: 26787
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool fillSpritePad;

	// Token: 0x040068A4 RID: 26788
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool fillInvert;

	// Token: 0x040068A5 RID: 26789
	[PublicizedFrom(EAccessModifier.Protected)]
	public float fillAmount;

	// Token: 0x040068A6 RID: 26790
	[PublicizedFrom(EAccessModifier.Protected)]
	public UISprite sprite;
}
