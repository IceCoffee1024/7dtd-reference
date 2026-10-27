using System;
using UnityEngine;

// Token: 0x0200117A RID: 4474
public class XUiV_Button : XUiV_ImageBased
{
	// Token: 0x06008DA7 RID: 36263 RVA: 0x0035C4C4 File Offset: 0x0035A6C4
	public XUiV_Button(XUi _xui, string _id)
		: base(_xui, _id)
	{
		base.UseSelectionBox = false;
	}

	// Token: 0x170010C7 RID: 4295
	// (get) Token: 0x06008DA8 RID: 36264 RVA: 0x0035C56F File Offset: 0x0035A76F
	// (set) Token: 0x06008DA9 RID: 36265 RVA: 0x0035C577 File Offset: 0x0035A777
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

	// Token: 0x170010C8 RID: 4296
	// (get) Token: 0x06008DAA RID: 36266 RVA: 0x0035C586 File Offset: 0x0035A786
	// (set) Token: 0x06008DAB RID: 36267 RVA: 0x0035C58E File Offset: 0x0035A78E
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

	// Token: 0x170010C9 RID: 4297
	// (get) Token: 0x06008DAC RID: 36268 RVA: 0x0035C5A3 File Offset: 0x0035A7A3
	// (set) Token: 0x06008DAD RID: 36269 RVA: 0x0035C5AB File Offset: 0x0035A7AB
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

	// Token: 0x170010CA RID: 4298
	// (get) Token: 0x06008DAE RID: 36270 RVA: 0x0035C5C0 File Offset: 0x0035A7C0
	// (set) Token: 0x06008DAF RID: 36271 RVA: 0x0035C5E1 File Offset: 0x0035A7E1
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

	// Token: 0x170010CB RID: 4299
	// (get) Token: 0x06008DB0 RID: 36272 RVA: 0x0035C5F6 File Offset: 0x0035A7F6
	// (set) Token: 0x06008DB1 RID: 36273 RVA: 0x0035C5FE File Offset: 0x0035A7FE
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

	// Token: 0x170010CC RID: 4300
	// (get) Token: 0x06008DB2 RID: 36274 RVA: 0x0035C613 File Offset: 0x0035A813
	// (set) Token: 0x06008DB3 RID: 36275 RVA: 0x0035C634 File Offset: 0x0035A834
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

	// Token: 0x170010CD RID: 4301
	// (get) Token: 0x06008DB4 RID: 36276 RVA: 0x0035C649 File Offset: 0x0035A849
	// (set) Token: 0x06008DB5 RID: 36277 RVA: 0x0035C651 File Offset: 0x0035A851
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

	// Token: 0x170010CE RID: 4302
	// (get) Token: 0x06008DB6 RID: 36278 RVA: 0x0035C666 File Offset: 0x0035A866
	// (set) Token: 0x06008DB7 RID: 36279 RVA: 0x0035C687 File Offset: 0x0035A887
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

	// Token: 0x170010CF RID: 4303
	// (get) Token: 0x06008DB8 RID: 36280 RVA: 0x0035C69C File Offset: 0x0035A89C
	// (set) Token: 0x06008DB9 RID: 36281 RVA: 0x0035C6A4 File Offset: 0x0035A8A4
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

	// Token: 0x170010D0 RID: 4304
	// (get) Token: 0x06008DBA RID: 36282 RVA: 0x0035C6B9 File Offset: 0x0035A8B9
	// (set) Token: 0x06008DBB RID: 36283 RVA: 0x0035C6C1 File Offset: 0x0035A8C1
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

	// Token: 0x170010D1 RID: 4305
	// (get) Token: 0x06008DBC RID: 36284 RVA: 0x0035C6E0 File Offset: 0x0035A8E0
	// (set) Token: 0x06008DBD RID: 36285 RVA: 0x0035C6E8 File Offset: 0x0035A8E8
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

	// Token: 0x170010D2 RID: 4306
	// (get) Token: 0x06008DBE RID: 36286 RVA: 0x0035C6FE File Offset: 0x0035A8FE
	// (set) Token: 0x06008DBF RID: 36287 RVA: 0x0035C706 File Offset: 0x0035A906
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

	// Token: 0x170010D3 RID: 4307
	// (get) Token: 0x06008DC0 RID: 36288 RVA: 0x0035C72B File Offset: 0x0035A92B
	// (set) Token: 0x06008DC1 RID: 36289 RVA: 0x0035C733 File Offset: 0x0035A933
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

	// Token: 0x170010D4 RID: 4308
	// (get) Token: 0x06008DC2 RID: 36290 RVA: 0x0035C752 File Offset: 0x0035A952
	// (set) Token: 0x06008DC3 RID: 36291 RVA: 0x0035C75A File Offset: 0x0035A95A
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

	// Token: 0x170010D5 RID: 4309
	// (set) Token: 0x06008DC4 RID: 36292 RVA: 0x0035C76C File Offset: 0x0035A96C
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

	// Token: 0x06008DC5 RID: 36293 RVA: 0x0035C7B4 File Offset: 0x0035A9B4
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

	// Token: 0x06008DC6 RID: 36294 RVA: 0x0035C853 File Offset: 0x0035AA53
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createComponents(GameObject _go)
	{
		_go.AddComponent<UISprite>();
		_go.AddComponent<TweenScale>().enabled = false;
	}

	// Token: 0x06008DC7 RID: 36295 RVA: 0x0035C868 File Offset: 0x0035AA68
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void captureComponents()
	{
		base.captureComponents();
		this.widget = (this.sprite = this.uiTransform.GetComponent<UISprite>());
		this.tweenScale = this.uiTransform.GetComponent<TweenScale>();
	}

	// Token: 0x06008DC8 RID: 36296 RVA: 0x0035C8A6 File Offset: 0x0035AAA6
	public override void InitView()
	{
		base.EventOnPress = true;
		base.EventOnHover = true;
		base.InitView();
		this.updateData();
		this.Enabled = true;
	}

	// Token: 0x06008DC9 RID: 36297 RVA: 0x0035C8C9 File Offset: 0x0035AAC9
	public override void Update(float _dt)
	{
		if (this.isOver && !base.UiTransformIsHovered)
		{
			this.OnHover(false);
		}
		base.Update(_dt);
	}

	// Token: 0x06008DCA RID: 36298 RVA: 0x0035C8EC File Offset: 0x0035AAEC
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

	// Token: 0x06008DCB RID: 36299 RVA: 0x0035C9C5 File Offset: 0x0035ABC5
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

	// Token: 0x06008DCC RID: 36300 RVA: 0x0035C9FA File Offset: 0x0035ABFA
	public override void OnOpen()
	{
		base.OnOpen();
		this.updateCurrentSprite();
		this.uiTransform.localScale = Vector3.one;
	}

	// Token: 0x06008DCD RID: 36301 RVA: 0x0035CA18 File Offset: 0x0035AC18
	[XuiXmlAttribute("sprite", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeSprite(string _value)
	{
		this.DefaultSpriteName = _value;
		this.CurrentSpriteName = _value;
	}

	// Token: 0x06008DCE RID: 36302 RVA: 0x0035CA28 File Offset: 0x0035AC28
	[XuiXmlAttribute("defaultcolor", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeSprite(Color _value)
	{
		this.DefaultSpriteColor = _value;
		this.CurrentColor = this.defaultSpriteColor;
	}

	// Token: 0x04006866 RID: 26726
	[PublicizedFrom(EAccessModifier.Protected)]
	public string uiAtlas = string.Empty;

	// Token: 0x04006867 RID: 26727
	[PublicizedFrom(EAccessModifier.Protected)]
	public UISprite sprite;

	// Token: 0x04006868 RID: 26728
	[PublicizedFrom(EAccessModifier.Protected)]
	public string defaultSpriteName = string.Empty;

	// Token: 0x04006869 RID: 26729
	[PublicizedFrom(EAccessModifier.Protected)]
	public string hoverSpriteName = string.Empty;

	// Token: 0x0400686A RID: 26730
	[PublicizedFrom(EAccessModifier.Protected)]
	public string selectedSpriteName = string.Empty;

	// Token: 0x0400686B RID: 26731
	[PublicizedFrom(EAccessModifier.Protected)]
	public string disabledSpriteName = string.Empty;

	// Token: 0x0400686C RID: 26732
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color defaultSpriteColor = Color.white;

	// Token: 0x0400686D RID: 26733
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color hoverSpriteColor = Color.white;

	// Token: 0x0400686E RID: 26734
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color selectedSpriteColor = Color.white;

	// Token: 0x0400686F RID: 26735
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color disabledSpriteColor = Color.white;

	// Token: 0x04006870 RID: 26736
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool manualColors;

	// Token: 0x04006871 RID: 26737
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color currentColor = Color.white;

	// Token: 0x04006872 RID: 26738
	[PublicizedFrom(EAccessModifier.Protected)]
	public string currentSpriteName = string.Empty;

	// Token: 0x04006873 RID: 26739
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool selected;

	// Token: 0x04006874 RID: 26740
	[PublicizedFrom(EAccessModifier.Private)]
	public bool lastVisible;

	// Token: 0x04006875 RID: 26741
	[PublicizedFrom(EAccessModifier.Private)]
	public bool colorDirty;

	// Token: 0x04006876 RID: 26742
	[PublicizedFrom(EAccessModifier.Private)]
	public float hoverScale = 1f;

	// Token: 0x04006877 RID: 26743
	[PublicizedFrom(EAccessModifier.Private)]
	public float hoverDuration = 0.25f;

	// Token: 0x04006878 RID: 26744
	[PublicizedFrom(EAccessModifier.Private)]
	public TweenScale tweenScale;
}
