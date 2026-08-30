using System;
using UnityEngine;

// Token: 0x02001160 RID: 4448
public class XUiV_Sprite : XUiV_ImageBased
{
	// Token: 0x170010DA RID: 4314
	// (get) Token: 0x06008DEE RID: 36334 RVA: 0x00358A4F File Offset: 0x00356C4F
	public UISprite Sprite
	{
		get
		{
			return this.sprite;
		}
	}

	// Token: 0x170010DB RID: 4315
	// (get) Token: 0x06008DEF RID: 36335 RVA: 0x00358A57 File Offset: 0x00356C57
	// (set) Token: 0x06008DF0 RID: 36336 RVA: 0x00358A5F File Offset: 0x00356C5F
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

	// Token: 0x170010DC RID: 4316
	// (get) Token: 0x06008DF1 RID: 36337 RVA: 0x00358A84 File Offset: 0x00356C84
	// (set) Token: 0x06008DF2 RID: 36338 RVA: 0x00358A8C File Offset: 0x00356C8C
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

	// Token: 0x170010DD RID: 4317
	// (get) Token: 0x06008DF3 RID: 36339 RVA: 0x00358AAA File Offset: 0x00356CAA
	// (set) Token: 0x06008DF4 RID: 36340 RVA: 0x00358AB2 File Offset: 0x00356CB2
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

	// Token: 0x170010DE RID: 4318
	// (get) Token: 0x06008DF5 RID: 36341 RVA: 0x00358AD0 File Offset: 0x00356CD0
	// (set) Token: 0x06008DF6 RID: 36342 RVA: 0x00358AD8 File Offset: 0x00356CD8
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

	// Token: 0x170010DF RID: 4319
	// (get) Token: 0x06008DF7 RID: 36343 RVA: 0x00358AF1 File Offset: 0x00356CF1
	// (set) Token: 0x06008DF8 RID: 36344 RVA: 0x00358AF9 File Offset: 0x00356CF9
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

	// Token: 0x170010E0 RID: 4320
	// (get) Token: 0x06008DF9 RID: 36345 RVA: 0x00358B12 File Offset: 0x00356D12
	// (set) Token: 0x06008DFA RID: 36346 RVA: 0x00358B1A File Offset: 0x00356D1A
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

	// Token: 0x170010E1 RID: 4321
	// (get) Token: 0x06008DFB RID: 36347 RVA: 0x00358B33 File Offset: 0x00356D33
	// (set) Token: 0x06008DFC RID: 36348 RVA: 0x00358B3B File Offset: 0x00356D3B
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

	// Token: 0x06008DFD RID: 36349 RVA: 0x00358B7A File Offset: 0x00356D7A
	public XUiV_Sprite(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x06008DFE RID: 36350 RVA: 0x00358BA5 File Offset: 0x00356DA5
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createComponents(GameObject _go)
	{
		_go.AddComponent<UISprite>();
	}

	// Token: 0x06008DFF RID: 36351 RVA: 0x00358BB0 File Offset: 0x00356DB0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void captureComponents()
	{
		base.captureComponents();
		this.widget = (this.sprite = this.uiTransform.GetComponent<UISprite>());
	}

	// Token: 0x06008E00 RID: 36352 RVA: 0x00358BDD File Offset: 0x00356DDD
	public override void InitView()
	{
		base.InitView();
		this.updateData();
	}

	// Token: 0x06008E01 RID: 36353 RVA: 0x00358BEB File Offset: 0x00356DEB
	public override void SetDefaults(XUiController _parent)
	{
		base.SetDefaults(_parent);
		base.FillCenter = true;
		this.Type = UIBasicSprite.Type.Simple;
		this.FillDirection = UIBasicSprite.FillDirection.Horizontal;
	}

	// Token: 0x06008E02 RID: 36354 RVA: 0x00358C0C File Offset: 0x00356E0C
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

	// Token: 0x06008E03 RID: 36355 RVA: 0x00358D63 File Offset: 0x00356F63
	public void SetSpriteImmediately(string _spriteName)
	{
		this.spriteName = _spriteName;
		this.applyAtlasAndSprite(true);
	}

	// Token: 0x06008E04 RID: 36356 RVA: 0x00358D74 File Offset: 0x00356F74
	public void SetColorImmediately(Color _color)
	{
		if (this.sprite != null)
		{
			this.sprite.color = _color;
		}
	}

	// Token: 0x06008E05 RID: 36357 RVA: 0x00358D90 File Offset: 0x00356F90
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

	// Token: 0x06008E06 RID: 36358 RVA: 0x00358E27 File Offset: 0x00357027
	[XuiXmlAttribute("gradient_start", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeGradientStart(Color _value)
	{
		this.gradientStart = new Color?(_value);
	}

	// Token: 0x06008E07 RID: 36359 RVA: 0x00358E35 File Offset: 0x00357035
	[XuiXmlAttribute("gradient_end", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeGradientEnd(Color _value)
	{
		this.gradientEnd = new Color?(_value);
	}

	// Token: 0x04006837 RID: 26679
	[PublicizedFrom(EAccessModifier.Private)]
	public string uiAtlas = string.Empty;

	// Token: 0x04006838 RID: 26680
	[PublicizedFrom(EAccessModifier.Private)]
	public bool uiAtlasChanged;

	// Token: 0x04006839 RID: 26681
	[PublicizedFrom(EAccessModifier.Protected)]
	public string spriteName = string.Empty;

	// Token: 0x0400683A RID: 26682
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color color = Color.white;

	// Token: 0x0400683B RID: 26683
	[PublicizedFrom(EAccessModifier.Private)]
	public Color? gradientStart;

	// Token: 0x0400683C RID: 26684
	[PublicizedFrom(EAccessModifier.Private)]
	public Color? gradientEnd;

	// Token: 0x0400683D RID: 26685
	[PublicizedFrom(EAccessModifier.Protected)]
	public UIBasicSprite.FillDirection fillDirection;

	// Token: 0x0400683E RID: 26686
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool fillSpritePad;

	// Token: 0x0400683F RID: 26687
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool fillInvert;

	// Token: 0x04006840 RID: 26688
	[PublicizedFrom(EAccessModifier.Protected)]
	public float fillAmount;

	// Token: 0x04006841 RID: 26689
	[PublicizedFrom(EAccessModifier.Protected)]
	public UISprite sprite;
}
