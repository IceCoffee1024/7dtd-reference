using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200115B RID: 4443
public abstract class XUiV_LabelBase : XUiView_WidgetBased
{
	// Token: 0x170010B9 RID: 4281
	// (get) Token: 0x06008D7F RID: 36223 RVA: 0x00357785 File Offset: 0x00355985
	// (set) Token: 0x06008D80 RID: 36224 RVA: 0x0035778D File Offset: 0x0035598D
	public NGUIFont UIFont
	{
		get
		{
			return this.uiFont;
		}
		set
		{
			this.uiFont = value;
			base.SetDirty();
		}
	}

	// Token: 0x170010BA RID: 4282
	// (get) Token: 0x06008D81 RID: 36225 RVA: 0x0035779C File Offset: 0x0035599C
	// (set) Token: 0x06008D82 RID: 36226 RVA: 0x003577A4 File Offset: 0x003559A4
	[XuiXmlAttribute("font_size", false)]
	public int FontSize
	{
		get
		{
			return this.fontSize;
		}
		set
		{
			this.fontSize = value;
			base.SetDirty();
		}
	}

	// Token: 0x170010BB RID: 4283
	// (get) Token: 0x06008D83 RID: 36227 RVA: 0x003577B3 File Offset: 0x003559B3
	// (set) Token: 0x06008D84 RID: 36228 RVA: 0x003577BB File Offset: 0x003559BB
	[XuiXmlAttribute("spacing_x", false)]
	public int SpacingX
	{
		get
		{
			return this.spacingX;
		}
		set
		{
			if (value == this.spacingX)
			{
				return;
			}
			this.spacingX = value;
			base.SetDirty();
		}
	}

	// Token: 0x170010BC RID: 4284
	// (get) Token: 0x06008D85 RID: 36229 RVA: 0x003577D4 File Offset: 0x003559D4
	// (set) Token: 0x06008D86 RID: 36230 RVA: 0x003577DC File Offset: 0x003559DC
	[XuiXmlAttribute("spacing_y", false)]
	public int SpacingY
	{
		get
		{
			return this.spacingY;
		}
		set
		{
			if (value == this.spacingY)
			{
				return;
			}
			this.spacingY = value;
			base.SetDirty();
		}
	}

	// Token: 0x170010BD RID: 4285
	// (get) Token: 0x06008D87 RID: 36231 RVA: 0x003577F5 File Offset: 0x003559F5
	// (set) Token: 0x06008D88 RID: 36232 RVA: 0x003577FD File Offset: 0x003559FD
	[XuiXmlAttribute("effect", false)]
	public UILabel.Effect Effect
	{
		get
		{
			return this.effect;
		}
		set
		{
			this.effect = value;
			base.SetDirty();
		}
	}

	// Token: 0x170010BE RID: 4286
	// (get) Token: 0x06008D89 RID: 36233 RVA: 0x0035780C File Offset: 0x00355A0C
	// (set) Token: 0x06008D8A RID: 36234 RVA: 0x00357814 File Offset: 0x00355A14
	[XuiXmlAttribute("effect_color", false)]
	public Color EffectColor
	{
		get
		{
			return this.effectColor;
		}
		set
		{
			this.effectColor = value;
			base.SetDirty();
		}
	}

	// Token: 0x170010BF RID: 4287
	// (get) Token: 0x06008D8B RID: 36235 RVA: 0x00357823 File Offset: 0x00355A23
	// (set) Token: 0x06008D8C RID: 36236 RVA: 0x0035782B File Offset: 0x00355A2B
	[XuiXmlAttribute("effect_distance", false)]
	public Vector2 EffectDistance
	{
		get
		{
			return this.effectDistance;
		}
		set
		{
			this.effectDistance = value;
			base.SetDirty();
		}
	}

	// Token: 0x170010C0 RID: 4288
	// (get) Token: 0x06008D8D RID: 36237 RVA: 0x0035783A File Offset: 0x00355A3A
	// (set) Token: 0x06008D8E RID: 36238 RVA: 0x00357842 File Offset: 0x00355A42
	[XuiXmlAttribute("crispness", false)]
	public UILabel.Crispness Crispness
	{
		get
		{
			return this.crispness;
		}
		set
		{
			this.crispness = value;
			base.SetDirty();
		}
	}

	// Token: 0x170010C1 RID: 4289
	// (get) Token: 0x06008D8F RID: 36239 RVA: 0x00357851 File Offset: 0x00355A51
	// (set) Token: 0x06008D90 RID: 36240 RVA: 0x00357859 File Offset: 0x00355A59
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

	// Token: 0x170010C2 RID: 4290
	// (get) Token: 0x06008D91 RID: 36241 RVA: 0x00357877 File Offset: 0x00355A77
	// (set) Token: 0x06008D92 RID: 36242 RVA: 0x00357884 File Offset: 0x00355A84
	public float Alpha
	{
		get
		{
			return this.color.a;
		}
		set
		{
			if (Mathf.Approximately(this.color.a, value))
			{
				return;
			}
			this.color.a = value;
			base.SetDirty();
		}
	}

	// Token: 0x170010C3 RID: 4291
	// (get) Token: 0x06008D93 RID: 36243 RVA: 0x003578AC File Offset: 0x00355AAC
	// (set) Token: 0x06008D94 RID: 36244 RVA: 0x003578B4 File Offset: 0x00355AB4
	[XuiXmlAttribute("justify", false)]
	public NGUIText.Alignment Alignment
	{
		get
		{
			return this.alignment;
		}
		set
		{
			if (this.alignment == value)
			{
				return;
			}
			this.alignment = value;
			base.SetDirty();
		}
	}

	// Token: 0x170010C4 RID: 4292
	// (get) Token: 0x06008D95 RID: 36245 RVA: 0x003578CD File Offset: 0x00355ACD
	// (set) Token: 0x06008D96 RID: 36246 RVA: 0x003578D5 File Offset: 0x00355AD5
	[XuiXmlAttribute("upper_case", false)]
	public bool UpperCase
	{
		get
		{
			return this.upperCase;
		}
		set
		{
			if (this.upperCase == value)
			{
				return;
			}
			this.upperCase = value;
			base.SetDirty();
		}
	}

	// Token: 0x170010C5 RID: 4293
	// (get) Token: 0x06008D97 RID: 36247 RVA: 0x003578EE File Offset: 0x00355AEE
	// (set) Token: 0x06008D98 RID: 36248 RVA: 0x003578F6 File Offset: 0x00355AF6
	[XuiXmlAttribute("lower_case", false)]
	public bool LowerCase
	{
		get
		{
			return this.lowerCase;
		}
		set
		{
			if (this.lowerCase == value)
			{
				return;
			}
			this.lowerCase = value;
			base.SetDirty();
		}
	}

	// Token: 0x170010C6 RID: 4294
	// (get) Token: 0x06008D99 RID: 36249 RVA: 0x0035790F File Offset: 0x00355B0F
	// (set) Token: 0x06008D9A RID: 36250 RVA: 0x00357917 File Offset: 0x00355B17
	[XuiXmlAttribute("support_bb_code", false)]
	public bool SupportBbCode
	{
		get
		{
			return this.supportBbCode;
		}
		set
		{
			if (this.supportBbCode == value)
			{
				return;
			}
			this.supportBbCode = value;
			base.SetDirty();
		}
	}

	// Token: 0x170010C7 RID: 4295
	// (get) Token: 0x06008D9B RID: 36251 RVA: 0x00357930 File Offset: 0x00355B30
	public Vector2 PrintedSize
	{
		get
		{
			return this.label.printedSize;
		}
	}

	// Token: 0x06008D9C RID: 36252 RVA: 0x0035793D File Offset: 0x00355B3D
	public XUiV_LabelBase(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x06008D9D RID: 36253 RVA: 0x0035796A File Offset: 0x00355B6A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createComponents(GameObject _go)
	{
		_go.AddComponent<UILabel>();
	}

	// Token: 0x06008D9E RID: 36254 RVA: 0x00357974 File Offset: 0x00355B74
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void captureComponents()
	{
		base.captureComponents();
		this.widget = (this.label = this.uiTransform.GetComponent<UILabel>());
	}

	// Token: 0x06008D9F RID: 36255 RVA: 0x003579A4 File Offset: 0x00355BA4
	public override void InitView()
	{
		if (this.supportUrls)
		{
			base.EventOnPress = true;
			base.EventOnHover = true;
			this.controller.OnPress += delegate(XUiController _, int _)
			{
				LabelUrlUtils.HandleLabelUrlClick(this, this.label, this.supportedUrlTypes);
			};
		}
		base.InitView();
		this.label.symbolDepth = this.depth + 1;
		this.updateData();
	}

	// Token: 0x06008DA0 RID: 36256 RVA: 0x003579FD File Offset: 0x00355BFD
	public override void SetDefaults(XUiController _parent)
	{
		base.SetDefaults(_parent);
		this.Alignment = NGUIText.Alignment.Left;
		this.FontSize = 16;
	}

	// Token: 0x06008DA1 RID: 36257 RVA: 0x00357A15 File Offset: 0x00355C15
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnHover(bool _isOver)
	{
		if (!this.supportUrls)
		{
			base.OnHover(_isOver);
			return;
		}
		if (!_isOver)
		{
			base.ToolTip = "";
			this.hadTooltipTextFromUrlHover = false;
		}
		else
		{
			this.updateUrlTooltip();
		}
		base.OnHover(_isOver);
	}

	// Token: 0x06008DA2 RID: 36258 RVA: 0x00357A4C File Offset: 0x00355C4C
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateUrlTooltip()
	{
		bool flag;
		string text;
		LabelUrlUtils.HandleLabelUrlHover(this, this.label, this.supportedUrlTypes, out flag, out text);
		if (!flag)
		{
			if (this.hadTooltipTextFromUrlHover)
			{
				base.ToolTip = "";
				this.hadTooltipTextFromUrlHover = false;
				return;
			}
		}
		else
		{
			if (!string.IsNullOrEmpty(text))
			{
				base.ToolTip = text;
				this.hadTooltipTextFromUrlHover = true;
				return;
			}
			if (this.hadTooltipTextFromUrlHover)
			{
				base.ToolTip = "";
				this.hadTooltipTextFromUrlHover = false;
			}
		}
	}

	// Token: 0x06008DA3 RID: 36259 RVA: 0x00357ABE File Offset: 0x00355CBE
	public override void Update(float _dt)
	{
		if (this.isOver)
		{
			this.updateUrlTooltip();
		}
		base.Update(_dt);
	}

	// Token: 0x06008DA4 RID: 36260 RVA: 0x00357AD8 File Offset: 0x00355CD8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void updateData()
	{
		if (this.uiFont != null)
		{
			this.label.font = this.uiFont;
		}
		this.label.fontSize = this.fontSize;
		this.label.supportEncoding = this.supportBbCode;
		this.label.color = this.color;
		this.label.alignment = this.alignment;
		this.label.keepCrispWhenShrunk = this.crispness;
		this.label.effectStyle = this.effect;
		this.label.effectColor = this.effectColor;
		this.label.effectDistance = this.effectDistance;
		this.label.spacingX = this.spacingX;
		this.label.spacingY = this.spacingY;
		base.updateData();
	}

	// Token: 0x06008DA5 RID: 36261 RVA: 0x00357BB4 File Offset: 0x00355DB4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual string getFormattedText(string _text)
	{
		if (this.upperCase)
		{
			_text = _text.ToUpperWithUserLocale();
		}
		else if (this.lowerCase)
		{
			_text = _text.ToLowerWithUserLocale();
		}
		return _text;
	}

	// Token: 0x06008DA6 RID: 36262 RVA: 0x00357BD9 File Offset: 0x00355DD9
	public void BindToUiInput(UIInput _input)
	{
		_input.label = this.label;
	}

	// Token: 0x06008DA7 RID: 36263 RVA: 0x00357BE7 File Offset: 0x00355DE7
	[XuiXmlAttribute("font_face", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeFontFace(string _value)
	{
		this.UIFont = this.xui.GetUIFontByName(_value, false);
		if (this.UIFont == null)
		{
			Log.Warning("[XUi] Label: Font not found: '" + _value + "', hierarchy: " + base.GetXuiHierarchy());
		}
	}

	// Token: 0x06008DA8 RID: 36264 RVA: 0x00357C28 File Offset: 0x00355E28
	[XuiXmlAttribute("support_urls", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeSupportUrls(string _value)
	{
		if (_value.EqualsCaseInsensitive("false"))
		{
			this.supportUrls = false;
			return;
		}
		this.supportUrls = true;
		if (_value.EqualsCaseInsensitive("true"))
		{
			this.supportedUrlTypes = new HashSet<string> { "HTTP" };
			return;
		}
		this.supportedUrlTypes = new HashSet<string>(_value.Split(",", StringSplitOptions.None));
	}

	// Token: 0x04006814 RID: 26644
	[PublicizedFrom(EAccessModifier.Protected)]
	public UILabel label;

	// Token: 0x04006815 RID: 26645
	[PublicizedFrom(EAccessModifier.Protected)]
	public NGUIFont uiFont;

	// Token: 0x04006816 RID: 26646
	[PublicizedFrom(EAccessModifier.Protected)]
	public int fontSize;

	// Token: 0x04006817 RID: 26647
	[PublicizedFrom(EAccessModifier.Protected)]
	public int spacingX = 1;

	// Token: 0x04006818 RID: 26648
	[PublicizedFrom(EAccessModifier.Protected)]
	public int spacingY;

	// Token: 0x04006819 RID: 26649
	[PublicizedFrom(EAccessModifier.Protected)]
	public UILabel.Effect effect;

	// Token: 0x0400681A RID: 26650
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color effectColor = new Color32(0, 0, 0, 80);

	// Token: 0x0400681B RID: 26651
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector2 effectDistance;

	// Token: 0x0400681C RID: 26652
	[PublicizedFrom(EAccessModifier.Protected)]
	public UILabel.Crispness crispness;

	// Token: 0x0400681D RID: 26653
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color color;

	// Token: 0x0400681E RID: 26654
	[PublicizedFrom(EAccessModifier.Protected)]
	public NGUIText.Alignment alignment;

	// Token: 0x0400681F RID: 26655
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool upperCase;

	// Token: 0x04006820 RID: 26656
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool lowerCase;

	// Token: 0x04006821 RID: 26657
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool supportBbCode = true;

	// Token: 0x04006822 RID: 26658
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool supportUrls;

	// Token: 0x04006823 RID: 26659
	[PublicizedFrom(EAccessModifier.Protected)]
	public HashSet<string> supportedUrlTypes;

	// Token: 0x04006824 RID: 26660
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hadTooltipTextFromUrlHover;
}
