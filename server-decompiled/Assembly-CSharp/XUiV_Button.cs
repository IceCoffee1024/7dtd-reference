using System;
using UnityEngine;

// Token: 0x02001139 RID: 4409
public class XUiV_Button : XUiV_ImageBased
{
	// Token: 0x06008BF6 RID: 35830 RVA: 0x003517C4 File Offset: 0x0034F9C4
	public XUiV_Button(XUi _xui, string _id)
		: base(_xui, _id)
	{
		base.UseSelectionBox = false;
	}

	// Token: 0x1700104E RID: 4174
	// (get) Token: 0x06008BF7 RID: 35831 RVA: 0x0035186F File Offset: 0x0034FA6F
	// (set) Token: 0x06008BF8 RID: 35832 RVA: 0x00351877 File Offset: 0x0034FA77
	[XuiXmlAttribute("atlas", false)]
	public string UIAtlas
	{
		get
		{
			return this.uiAtlas;
		}
		set
		{
			this.uiAtlas = value;
			base.SetDirty();
		}
	}

	// Token: 0x1700104F RID: 4175
	// (get) Token: 0x06008BF9 RID: 35833 RVA: 0x00351886 File Offset: 0x0034FA86
	// (set) Token: 0x06008BFA RID: 35834 RVA: 0x0035188E File Offset: 0x0034FA8E
	public string DefaultSpriteName
	{
		get
		{
			return this.defaultSpriteName;
		}
		set
		{
			this.defaultSpriteName = value;
			base.SetDirty();
			this.updateCurrentSprite();
		}
	}

	// Token: 0x17001050 RID: 4176
	// (get) Token: 0x06008BFB RID: 35835 RVA: 0x003518A3 File Offset: 0x0034FAA3
	// (set) Token: 0x06008BFC RID: 35836 RVA: 0x003518AB File Offset: 0x0034FAAB
	public Color DefaultSpriteColor
	{
		get
		{
			return this.defaultSpriteColor;
		}
		set
		{
			this.defaultSpriteColor = value;
			base.SetDirty();
			this.updateCurrentSprite();
		}
	}

	// Token: 0x17001051 RID: 4177
	// (get) Token: 0x06008BFD RID: 35837 RVA: 0x003518C0 File Offset: 0x0034FAC0
	// (set) Token: 0x06008BFE RID: 35838 RVA: 0x003518E1 File Offset: 0x0034FAE1
	[XuiXmlAttribute("hoversprite", false)]
	public string HoverSpriteName
	{
		get
		{
			if (!(this.hoverSpriteName == ""))
			{
				return this.hoverSpriteName;
			}
			return this.defaultSpriteName;
		}
		set
		{
			this.hoverSpriteName = value;
			base.SetDirty();
			this.updateCurrentSprite();
		}
	}

	// Token: 0x17001052 RID: 4178
	// (get) Token: 0x06008BFF RID: 35839 RVA: 0x003518F6 File Offset: 0x0034FAF6
	// (set) Token: 0x06008C00 RID: 35840 RVA: 0x003518FE File Offset: 0x0034FAFE
	[XuiXmlAttribute("hovercolor", false)]
	public Color HoverSpriteColor
	{
		get
		{
			return this.hoverSpriteColor;
		}
		set
		{
			this.hoverSpriteColor = value;
			base.SetDirty();
			this.updateCurrentSprite();
		}
	}

	// Token: 0x17001053 RID: 4179
	// (get) Token: 0x06008C01 RID: 35841 RVA: 0x00351913 File Offset: 0x0034FB13
	// (set) Token: 0x06008C02 RID: 35842 RVA: 0x00351934 File Offset: 0x0034FB34
	[XuiXmlAttribute("selectedsprite", false)]
	public string SelectedSpriteName
	{
		get
		{
			if (!(this.selectedSpriteName == ""))
			{
				return this.selectedSpriteName;
			}
			return this.defaultSpriteName;
		}
		set
		{
			this.selectedSpriteName = value;
			base.SetDirty();
			this.updateCurrentSprite();
		}
	}

	// Token: 0x17001054 RID: 4180
	// (get) Token: 0x06008C03 RID: 35843 RVA: 0x00351949 File Offset: 0x0034FB49
	// (set) Token: 0x06008C04 RID: 35844 RVA: 0x00351951 File Offset: 0x0034FB51
	[XuiXmlAttribute("selectedcolor", false)]
	public Color SelectedSpriteColor
	{
		get
		{
			return this.selectedSpriteColor;
		}
		set
		{
			this.selectedSpriteColor = value;
			base.SetDirty();
			this.updateCurrentSprite();
		}
	}

	// Token: 0x17001055 RID: 4181
	// (get) Token: 0x06008C05 RID: 35845 RVA: 0x00351966 File Offset: 0x0034FB66
	// (set) Token: 0x06008C06 RID: 35846 RVA: 0x00351987 File Offset: 0x0034FB87
	[XuiXmlAttribute("disabledsprite", false)]
	public string DisabledSpriteName
	{
		get
		{
			if (!(this.disabledSpriteName == ""))
			{
				return this.disabledSpriteName;
			}
			return this.defaultSpriteName;
		}
		set
		{
			this.disabledSpriteName = value;
			base.SetDirty();
			this.updateCurrentSprite();
		}
	}

	// Token: 0x17001056 RID: 4182
	// (get) Token: 0x06008C07 RID: 35847 RVA: 0x0035199C File Offset: 0x0034FB9C
	// (set) Token: 0x06008C08 RID: 35848 RVA: 0x003519A4 File Offset: 0x0034FBA4
	[XuiXmlAttribute("disabledcolor", false)]
	public Color DisabledSpriteColor
	{
		get
		{
			return this.disabledSpriteColor;
		}
		set
		{
			this.disabledSpriteColor = value;
			base.SetDirty();
			this.updateCurrentSprite();
		}
	}

	// Token: 0x17001057 RID: 4183
	// (get) Token: 0x06008C09 RID: 35849 RVA: 0x003519B9 File Offset: 0x0034FBB9
	// (set) Token: 0x06008C0A RID: 35850 RVA: 0x003519C1 File Offset: 0x0034FBC1
	[XuiXmlAttribute("manualcolors", false)]
	public bool ManualColors
	{
		get
		{
			return this.manualColors;
		}
		set
		{
			if (value == this.manualColors)
			{
				return;
			}
			this.manualColors = value;
			base.SetDirty();
			this.updateCurrentSprite();
		}
	}

	// Token: 0x17001058 RID: 4184
	// (get) Token: 0x06008C0B RID: 35851 RVA: 0x003519E0 File Offset: 0x0034FBE0
	// (set) Token: 0x06008C0C RID: 35852 RVA: 0x003519E8 File Offset: 0x0034FBE8
	public Color CurrentColor
	{
		get
		{
			return this.currentColor;
		}
		set
		{
			this.currentColor = value;
			base.SetDirty();
			this.colorDirty = true;
		}
	}

	// Token: 0x17001059 RID: 4185
	// (get) Token: 0x06008C0D RID: 35853 RVA: 0x003519FE File Offset: 0x0034FBFE
	// (set) Token: 0x06008C0E RID: 35854 RVA: 0x00351A06 File Offset: 0x0034FC06
	public string CurrentSpriteName
	{
		get
		{
			return this.currentSpriteName;
		}
		set
		{
			if (value == this.currentSpriteName)
			{
				return;
			}
			this.currentSpriteName = value;
			base.SetDirty();
			this.colorDirty = true;
		}
	}

	// Token: 0x1700105A RID: 4186
	// (get) Token: 0x06008C0F RID: 35855 RVA: 0x00351A2B File Offset: 0x0034FC2B
	// (set) Token: 0x06008C10 RID: 35856 RVA: 0x00351A33 File Offset: 0x0034FC33
	[XuiXmlAttribute("selected", false)]
	public bool Selected
	{
		get
		{
			return this.selected;
		}
		set
		{
			if (this.selected == value)
			{
				return;
			}
			this.selected = value;
			base.SetDirty();
			this.updateCurrentSprite();
		}
	}

	// Token: 0x1700105B RID: 4187
	// (get) Token: 0x06008C11 RID: 35857 RVA: 0x00351A52 File Offset: 0x0034FC52
	// (set) Token: 0x06008C12 RID: 35858 RVA: 0x00351A5A File Offset: 0x0034FC5A
	[XuiXmlAttribute("hoverscale", false)]
	public float HoverScale
	{
		get
		{
			return this.hoverScale;
		}
		set
		{
			this.hoverScale = value;
			base.SetDirty();
		}
	}

	// Token: 0x1700105C RID: 4188
	// (set) Token: 0x06008C13 RID: 35859 RVA: 0x00351A6C File Offset: 0x0034FC6C
	public override bool Enabled
	{
		set
		{
			bool enabled = this.enabled;
			base.Enabled = value;
			if (value != enabled)
			{
				this.updateCurrentSprite();
				if (!value)
				{
					TweenScale tweenScale = this.tweenScale;
					if (tweenScale != null)
					{
						tweenScale.PlayReverse();
					}
				}
				if (!this.gamepadSelectableSetFromAttributes)
				{
					base.IsNavigatable = value;
				}
			}
		}
	}

	// Token: 0x06008C14 RID: 35860 RVA: 0x00351AB4 File Offset: 0x0034FCB4
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateCurrentSprite()
	{
		if (!this.Enabled)
		{
			if (!this.manualColors)
			{
				this.CurrentColor = this.disabledSpriteColor;
			}
			this.CurrentSpriteName = this.DisabledSpriteName;
			return;
		}
		if (this.Selected)
		{
			if (!this.manualColors)
			{
				this.CurrentColor = this.selectedSpriteColor;
			}
			this.CurrentSpriteName = this.SelectedSpriteName;
			return;
		}
		if (!this.manualColors)
		{
			this.CurrentColor = (this.isOver ? this.hoverSpriteColor : this.defaultSpriteColor);
		}
		this.CurrentSpriteName = (this.isOver ? this.HoverSpriteName : this.DefaultSpriteName);
	}

	// Token: 0x06008C15 RID: 35861 RVA: 0x00351B53 File Offset: 0x0034FD53
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createComponents(GameObject _go)
	{
		_go.AddComponent<UISprite>();
		_go.AddComponent<TweenScale>().enabled = false;
	}

	// Token: 0x06008C16 RID: 35862 RVA: 0x00351B68 File Offset: 0x0034FD68
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void captureComponents()
	{
		base.captureComponents();
		this.widget = (this.sprite = this.uiTransform.GetComponent<UISprite>());
		this.tweenScale = this.uiTransform.GetComponent<TweenScale>();
	}

	// Token: 0x06008C17 RID: 35863 RVA: 0x00351BA6 File Offset: 0x0034FDA6
	public override void InitView()
	{
		base.EventOnPress = true;
		base.EventOnHover = true;
		base.InitView();
		this.updateData();
		this.Enabled = true;
	}

	// Token: 0x06008C18 RID: 35864 RVA: 0x00351BC9 File Offset: 0x0034FDC9
	public override void Update(float _dt)
	{
		if (this.isOver && !base.UiTransformIsHovered)
		{
			this.OnHover(false);
		}
		base.Update(_dt);
	}

	// Token: 0x06008C19 RID: 35865 RVA: 0x00351BEC File Offset: 0x0034FDEC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void updateData()
	{
		this.sprite.spriteName = this.currentSpriteName;
		this.sprite.atlas = this.xui.GetAtlasByName(this.uiAtlas, this.currentSpriteName);
		this.sprite.color = base.opacityModColor(this.currentColor);
		if (!Mathf.Approximately(this.hoverScale, 1f))
		{
			this.tweenScale.from = Vector3.one;
			this.tweenScale.to = Vector3.one * this.hoverScale;
			this.tweenScale.duration = this.hoverDuration;
		}
		this.sprite.centerType = UIBasicSprite.AdvancedType.Sliced;
		this.sprite.type = this.type;
		this.sprite.flip = this.flip;
		base.updateData();
	}

	// Token: 0x06008C1A RID: 35866 RVA: 0x00351CC5 File Offset: 0x0034FEC5
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnHover(bool _isOver)
	{
		base.OnHover(_isOver);
		this.updateCurrentSprite();
		if (this.Enabled && !Mathf.Approximately(this.hoverScale, 1f))
		{
			this.tweenScale.Play(_isOver);
		}
	}

	// Token: 0x06008C1B RID: 35867 RVA: 0x00351CFA File Offset: 0x0034FEFA
	public override void OnOpen()
	{
		base.OnOpen();
		this.updateCurrentSprite();
		this.uiTransform.localScale = Vector3.one;
	}

	// Token: 0x06008C1C RID: 35868 RVA: 0x00351D18 File Offset: 0x0034FF18
	[XuiXmlAttribute("sprite", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeSprite(string _value)
	{
		this.DefaultSpriteName = _value;
		this.CurrentSpriteName = _value;
	}

	// Token: 0x06008C1D RID: 35869 RVA: 0x00351D28 File Offset: 0x0034FF28
	[XuiXmlAttribute("defaultcolor", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeSprite(Color _value)
	{
		this.DefaultSpriteColor = _value;
		this.CurrentColor = this.defaultSpriteColor;
	}

	// Token: 0x04006707 RID: 26375
	[PublicizedFrom(EAccessModifier.Protected)]
	public string uiAtlas = string.Empty;

	// Token: 0x04006708 RID: 26376
	[PublicizedFrom(EAccessModifier.Protected)]
	public UISprite sprite;

	// Token: 0x04006709 RID: 26377
	[PublicizedFrom(EAccessModifier.Protected)]
	public string defaultSpriteName = string.Empty;

	// Token: 0x0400670A RID: 26378
	[PublicizedFrom(EAccessModifier.Protected)]
	public string hoverSpriteName = string.Empty;

	// Token: 0x0400670B RID: 26379
	[PublicizedFrom(EAccessModifier.Protected)]
	public string selectedSpriteName = string.Empty;

	// Token: 0x0400670C RID: 26380
	[PublicizedFrom(EAccessModifier.Protected)]
	public string disabledSpriteName = string.Empty;

	// Token: 0x0400670D RID: 26381
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color defaultSpriteColor = Color.white;

	// Token: 0x0400670E RID: 26382
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color hoverSpriteColor = Color.white;

	// Token: 0x0400670F RID: 26383
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color selectedSpriteColor = Color.white;

	// Token: 0x04006710 RID: 26384
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color disabledSpriteColor = Color.white;

	// Token: 0x04006711 RID: 26385
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool manualColors;

	// Token: 0x04006712 RID: 26386
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color currentColor = Color.white;

	// Token: 0x04006713 RID: 26387
	[PublicizedFrom(EAccessModifier.Protected)]
	public string currentSpriteName = string.Empty;

	// Token: 0x04006714 RID: 26388
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool selected;

	// Token: 0x04006715 RID: 26389
	[PublicizedFrom(EAccessModifier.Private)]
	public bool lastVisible;

	// Token: 0x04006716 RID: 26390
	[PublicizedFrom(EAccessModifier.Private)]
	public bool colorDirty;

	// Token: 0x04006717 RID: 26391
	[PublicizedFrom(EAccessModifier.Private)]
	public float hoverScale = 1f;

	// Token: 0x04006718 RID: 26392
	[PublicizedFrom(EAccessModifier.Private)]
	public float hoverDuration = 0.25f;

	// Token: 0x04006719 RID: 26393
	[PublicizedFrom(EAccessModifier.Private)]
	public TweenScale tweenScale;
}
