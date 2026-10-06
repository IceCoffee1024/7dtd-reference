using System;
using UnityEngine;

// Token: 0x0200116A RID: 4458
public class XUiV_Button : XUiV_ImageBased
{
	// Token: 0x06008D53 RID: 36179 RVA: 0x0035A418 File Offset: 0x00358618
	public XUiV_Button(XUi _xui, string _id)
		: base(_xui, _id)
	{
		base.UseSelectionBox = false;
	}

	// Token: 0x170010BC RID: 4284
	// (get) Token: 0x06008D54 RID: 36180 RVA: 0x0035A4C3 File Offset: 0x003586C3
	// (set) Token: 0x06008D55 RID: 36181 RVA: 0x0035A4CB File Offset: 0x003586CB
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

	// Token: 0x170010BD RID: 4285
	// (get) Token: 0x06008D56 RID: 36182 RVA: 0x0035A4DA File Offset: 0x003586DA
	// (set) Token: 0x06008D57 RID: 36183 RVA: 0x0035A4E2 File Offset: 0x003586E2
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

	// Token: 0x170010BE RID: 4286
	// (get) Token: 0x06008D58 RID: 36184 RVA: 0x0035A4F7 File Offset: 0x003586F7
	// (set) Token: 0x06008D59 RID: 36185 RVA: 0x0035A4FF File Offset: 0x003586FF
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

	// Token: 0x170010BF RID: 4287
	// (get) Token: 0x06008D5A RID: 36186 RVA: 0x0035A514 File Offset: 0x00358714
	// (set) Token: 0x06008D5B RID: 36187 RVA: 0x0035A535 File Offset: 0x00358735
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

	// Token: 0x170010C0 RID: 4288
	// (get) Token: 0x06008D5C RID: 36188 RVA: 0x0035A54A File Offset: 0x0035874A
	// (set) Token: 0x06008D5D RID: 36189 RVA: 0x0035A552 File Offset: 0x00358752
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

	// Token: 0x170010C1 RID: 4289
	// (get) Token: 0x06008D5E RID: 36190 RVA: 0x0035A567 File Offset: 0x00358767
	// (set) Token: 0x06008D5F RID: 36191 RVA: 0x0035A588 File Offset: 0x00358788
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

	// Token: 0x170010C2 RID: 4290
	// (get) Token: 0x06008D60 RID: 36192 RVA: 0x0035A59D File Offset: 0x0035879D
	// (set) Token: 0x06008D61 RID: 36193 RVA: 0x0035A5A5 File Offset: 0x003587A5
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

	// Token: 0x170010C3 RID: 4291
	// (get) Token: 0x06008D62 RID: 36194 RVA: 0x0035A5BA File Offset: 0x003587BA
	// (set) Token: 0x06008D63 RID: 36195 RVA: 0x0035A5DB File Offset: 0x003587DB
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

	// Token: 0x170010C4 RID: 4292
	// (get) Token: 0x06008D64 RID: 36196 RVA: 0x0035A5F0 File Offset: 0x003587F0
	// (set) Token: 0x06008D65 RID: 36197 RVA: 0x0035A5F8 File Offset: 0x003587F8
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

	// Token: 0x170010C5 RID: 4293
	// (get) Token: 0x06008D66 RID: 36198 RVA: 0x0035A60D File Offset: 0x0035880D
	// (set) Token: 0x06008D67 RID: 36199 RVA: 0x0035A615 File Offset: 0x00358815
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

	// Token: 0x170010C6 RID: 4294
	// (get) Token: 0x06008D68 RID: 36200 RVA: 0x0035A634 File Offset: 0x00358834
	// (set) Token: 0x06008D69 RID: 36201 RVA: 0x0035A63C File Offset: 0x0035883C
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

	// Token: 0x170010C7 RID: 4295
	// (get) Token: 0x06008D6A RID: 36202 RVA: 0x0035A652 File Offset: 0x00358852
	// (set) Token: 0x06008D6B RID: 36203 RVA: 0x0035A65A File Offset: 0x0035885A
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

	// Token: 0x170010C8 RID: 4296
	// (get) Token: 0x06008D6C RID: 36204 RVA: 0x0035A67F File Offset: 0x0035887F
	// (set) Token: 0x06008D6D RID: 36205 RVA: 0x0035A687 File Offset: 0x00358887
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

	// Token: 0x170010C9 RID: 4297
	// (get) Token: 0x06008D6E RID: 36206 RVA: 0x0035A6A6 File Offset: 0x003588A6
	// (set) Token: 0x06008D6F RID: 36207 RVA: 0x0035A6AE File Offset: 0x003588AE
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

	// Token: 0x170010CA RID: 4298
	// (set) Token: 0x06008D70 RID: 36208 RVA: 0x0035A6C0 File Offset: 0x003588C0
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

	// Token: 0x06008D71 RID: 36209 RVA: 0x0035A708 File Offset: 0x00358908
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

	// Token: 0x06008D72 RID: 36210 RVA: 0x0035A7A7 File Offset: 0x003589A7
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createComponents(GameObject _go)
	{
		_go.AddComponent<UISprite>();
		_go.AddComponent<TweenScale>().enabled = false;
	}

	// Token: 0x06008D73 RID: 36211 RVA: 0x0035A7BC File Offset: 0x003589BC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void captureComponents()
	{
		base.captureComponents();
		this.widget = (this.sprite = this.uiTransform.GetComponent<UISprite>());
		this.tweenScale = this.uiTransform.GetComponent<TweenScale>();
	}

	// Token: 0x06008D74 RID: 36212 RVA: 0x0035A7FA File Offset: 0x003589FA
	public override void InitView()
	{
		base.EventOnPress = true;
		base.EventOnHover = true;
		base.InitView();
		this.updateData();
		this.Enabled = true;
	}

	// Token: 0x06008D75 RID: 36213 RVA: 0x0035A81D File Offset: 0x00358A1D
	public override void Update(float _dt)
	{
		if (this.isOver && !base.UiTransformIsHovered)
		{
			this.OnHover(false);
		}
		base.Update(_dt);
	}

	// Token: 0x06008D76 RID: 36214 RVA: 0x0035A840 File Offset: 0x00358A40
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

	// Token: 0x06008D77 RID: 36215 RVA: 0x0035A919 File Offset: 0x00358B19
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

	// Token: 0x06008D78 RID: 36216 RVA: 0x0035A94E File Offset: 0x00358B4E
	public override void OnOpen()
	{
		base.OnOpen();
		this.updateCurrentSprite();
		this.uiTransform.localScale = Vector3.one;
	}

	// Token: 0x06008D79 RID: 36217 RVA: 0x0035A96C File Offset: 0x00358B6C
	[XuiXmlAttribute("sprite", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeSprite(string _value)
	{
		this.DefaultSpriteName = _value;
		this.CurrentSpriteName = _value;
	}

	// Token: 0x06008D7A RID: 36218 RVA: 0x0035A97C File Offset: 0x00358B7C
	[XuiXmlAttribute("defaultcolor", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeSprite(Color _value)
	{
		this.DefaultSpriteColor = _value;
		this.CurrentColor = this.defaultSpriteColor;
	}

	// Token: 0x04006826 RID: 26662
	[PublicizedFrom(EAccessModifier.Protected)]
	public string uiAtlas = string.Empty;

	// Token: 0x04006827 RID: 26663
	[PublicizedFrom(EAccessModifier.Protected)]
	public UISprite sprite;

	// Token: 0x04006828 RID: 26664
	[PublicizedFrom(EAccessModifier.Protected)]
	public string defaultSpriteName = string.Empty;

	// Token: 0x04006829 RID: 26665
	[PublicizedFrom(EAccessModifier.Protected)]
	public string hoverSpriteName = string.Empty;

	// Token: 0x0400682A RID: 26666
	[PublicizedFrom(EAccessModifier.Protected)]
	public string selectedSpriteName = string.Empty;

	// Token: 0x0400682B RID: 26667
	[PublicizedFrom(EAccessModifier.Protected)]
	public string disabledSpriteName = string.Empty;

	// Token: 0x0400682C RID: 26668
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color defaultSpriteColor = Color.white;

	// Token: 0x0400682D RID: 26669
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color hoverSpriteColor = Color.white;

	// Token: 0x0400682E RID: 26670
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color selectedSpriteColor = Color.white;

	// Token: 0x0400682F RID: 26671
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color disabledSpriteColor = Color.white;

	// Token: 0x04006830 RID: 26672
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool manualColors;

	// Token: 0x04006831 RID: 26673
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color currentColor = Color.white;

	// Token: 0x04006832 RID: 26674
	[PublicizedFrom(EAccessModifier.Protected)]
	public string currentSpriteName = string.Empty;

	// Token: 0x04006833 RID: 26675
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool selected;

	// Token: 0x04006834 RID: 26676
	[PublicizedFrom(EAccessModifier.Private)]
	public bool lastVisible;

	// Token: 0x04006835 RID: 26677
	[PublicizedFrom(EAccessModifier.Private)]
	public bool colorDirty;

	// Token: 0x04006836 RID: 26678
	[PublicizedFrom(EAccessModifier.Private)]
	public float hoverScale = 1f;

	// Token: 0x04006837 RID: 26679
	[PublicizedFrom(EAccessModifier.Private)]
	public float hoverDuration = 0.25f;

	// Token: 0x04006838 RID: 26680
	[PublicizedFrom(EAccessModifier.Private)]
	public TweenScale tweenScale;
}
