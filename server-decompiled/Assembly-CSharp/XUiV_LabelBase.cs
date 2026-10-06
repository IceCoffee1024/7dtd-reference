using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001173 RID: 4467
public abstract class XUiV_LabelBase : XUiView_WidgetBased
{
	// Token: 0x170010EE RID: 4334
	// (get) Token: 0x06008DEA RID: 36330 RVA: 0x0035BBA5 File Offset: 0x00359DA5
	// (set) Token: 0x06008DEB RID: 36331 RVA: 0x0035BBAD File Offset: 0x00359DAD
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

	// Token: 0x170010EF RID: 4335
	// (get) Token: 0x06008DEC RID: 36332 RVA: 0x0035BBBC File Offset: 0x00359DBC
	// (set) Token: 0x06008DED RID: 36333 RVA: 0x0035BBC4 File Offset: 0x00359DC4
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

	// Token: 0x170010F0 RID: 4336
	// (get) Token: 0x06008DEE RID: 36334 RVA: 0x0035BBD3 File Offset: 0x00359DD3
	// (set) Token: 0x06008DEF RID: 36335 RVA: 0x0035BBDB File Offset: 0x00359DDB
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

	// Token: 0x170010F1 RID: 4337
	// (get) Token: 0x06008DF0 RID: 36336 RVA: 0x0035BBF4 File Offset: 0x00359DF4
	// (set) Token: 0x06008DF1 RID: 36337 RVA: 0x0035BBFC File Offset: 0x00359DFC
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

	// Token: 0x170010F2 RID: 4338
	// (get) Token: 0x06008DF2 RID: 36338 RVA: 0x0035BC15 File Offset: 0x00359E15
	// (set) Token: 0x06008DF3 RID: 36339 RVA: 0x0035BC1D File Offset: 0x00359E1D
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

	// Token: 0x170010F3 RID: 4339
	// (get) Token: 0x06008DF4 RID: 36340 RVA: 0x0035BC2C File Offset: 0x00359E2C
	// (set) Token: 0x06008DF5 RID: 36341 RVA: 0x0035BC34 File Offset: 0x00359E34
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

	// Token: 0x170010F4 RID: 4340
	// (get) Token: 0x06008DF6 RID: 36342 RVA: 0x0035BC43 File Offset: 0x00359E43
	// (set) Token: 0x06008DF7 RID: 36343 RVA: 0x0035BC4B File Offset: 0x00359E4B
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

	// Token: 0x170010F5 RID: 4341
	// (get) Token: 0x06008DF8 RID: 36344 RVA: 0x0035BC5A File Offset: 0x00359E5A
	// (set) Token: 0x06008DF9 RID: 36345 RVA: 0x0035BC62 File Offset: 0x00359E62
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

	// Token: 0x170010F6 RID: 4342
	// (get) Token: 0x06008DFA RID: 36346 RVA: 0x0035BC71 File Offset: 0x00359E71
	// (set) Token: 0x06008DFB RID: 36347 RVA: 0x0035BC79 File Offset: 0x00359E79
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

	// Token: 0x170010F7 RID: 4343
	// (get) Token: 0x06008DFC RID: 36348 RVA: 0x0035BC97 File Offset: 0x00359E97
	// (set) Token: 0x06008DFD RID: 36349 RVA: 0x0035BCA4 File Offset: 0x00359EA4
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

	// Token: 0x170010F8 RID: 4344
	// (get) Token: 0x06008DFE RID: 36350 RVA: 0x0035BCCC File Offset: 0x00359ECC
	// (set) Token: 0x06008DFF RID: 36351 RVA: 0x0035BCD4 File Offset: 0x00359ED4
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

	// Token: 0x170010F9 RID: 4345
	// (get) Token: 0x06008E00 RID: 36352 RVA: 0x0035BCED File Offset: 0x00359EED
	// (set) Token: 0x06008E01 RID: 36353 RVA: 0x0035BCF5 File Offset: 0x00359EF5
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

	// Token: 0x170010FA RID: 4346
	// (get) Token: 0x06008E02 RID: 36354 RVA: 0x0035BD0E File Offset: 0x00359F0E
	// (set) Token: 0x06008E03 RID: 36355 RVA: 0x0035BD16 File Offset: 0x00359F16
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

	// Token: 0x170010FB RID: 4347
	// (get) Token: 0x06008E04 RID: 36356 RVA: 0x0035BD2F File Offset: 0x00359F2F
	// (set) Token: 0x06008E05 RID: 36357 RVA: 0x0035BD37 File Offset: 0x00359F37
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

	// Token: 0x170010FC RID: 4348
	// (get) Token: 0x06008E06 RID: 36358 RVA: 0x0035BD50 File Offset: 0x00359F50
	public Vector2 PrintedSize
	{
		get
		{
			return this.label.printedSize;
		}
	}

	// Token: 0x06008E07 RID: 36359 RVA: 0x0035BD5D File Offset: 0x00359F5D
	public XUiV_LabelBase(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x06008E08 RID: 36360 RVA: 0x0035BD8A File Offset: 0x00359F8A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createComponents(GameObject _go)
	{
		_go.AddComponent<UILabel>();
	}

	// Token: 0x06008E09 RID: 36361 RVA: 0x0035BD94 File Offset: 0x00359F94
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void captureComponents()
	{
		base.captureComponents();
		this.widget = (this.label = this.uiTransform.GetComponent<UILabel>());
	}

	// Token: 0x06008E0A RID: 36362 RVA: 0x0035BDC4 File Offset: 0x00359FC4
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

	// Token: 0x06008E0B RID: 36363 RVA: 0x0035BE1D File Offset: 0x0035A01D
	public override void SetDefaults(XUiController _parent)
	{
		base.SetDefaults(_parent);
		this.Alignment = NGUIText.Alignment.Left;
		this.FontSize = 16;
	}

	// Token: 0x06008E0C RID: 36364 RVA: 0x0035BE35 File Offset: 0x0035A035
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

	// Token: 0x06008E0D RID: 36365 RVA: 0x0035BE6C File Offset: 0x0035A06C
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

	// Token: 0x06008E0E RID: 36366 RVA: 0x0035BEDE File Offset: 0x0035A0DE
	public override void Update(float _dt)
	{
		if (this.isOver)
		{
			this.updateUrlTooltip();
		}
		base.Update(_dt);
	}

	// Token: 0x06008E0F RID: 36367 RVA: 0x0035BEF8 File Offset: 0x0035A0F8
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

	// Token: 0x06008E10 RID: 36368 RVA: 0x0035BFD4 File Offset: 0x0035A1D4
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

	// Token: 0x06008E11 RID: 36369 RVA: 0x0035BFF9 File Offset: 0x0035A1F9
	public void BindToUiInput(UIInput _input)
	{
		_input.label = this.label;
	}

	// Token: 0x06008E12 RID: 36370 RVA: 0x0035C007 File Offset: 0x0035A207
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

	// Token: 0x06008E13 RID: 36371 RVA: 0x0035C048 File Offset: 0x0035A248
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

	// Token: 0x04006869 RID: 26729
	[PublicizedFrom(EAccessModifier.Protected)]
	public UILabel label;

	// Token: 0x0400686A RID: 26730
	[PublicizedFrom(EAccessModifier.Protected)]
	public NGUIFont uiFont;

	// Token: 0x0400686B RID: 26731
	[PublicizedFrom(EAccessModifier.Protected)]
	public int fontSize;

	// Token: 0x0400686C RID: 26732
	[PublicizedFrom(EAccessModifier.Protected)]
	public int spacingX = 1;

	// Token: 0x0400686D RID: 26733
	[PublicizedFrom(EAccessModifier.Protected)]
	public int spacingY;

	// Token: 0x0400686E RID: 26734
	[PublicizedFrom(EAccessModifier.Protected)]
	public UILabel.Effect effect;

	// Token: 0x0400686F RID: 26735
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color effectColor = new Color32(0, 0, 0, 80);

	// Token: 0x04006870 RID: 26736
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector2 effectDistance;

	// Token: 0x04006871 RID: 26737
	[PublicizedFrom(EAccessModifier.Protected)]
	public UILabel.Crispness crispness;

	// Token: 0x04006872 RID: 26738
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color color;

	// Token: 0x04006873 RID: 26739
	[PublicizedFrom(EAccessModifier.Protected)]
	public NGUIText.Alignment alignment;

	// Token: 0x04006874 RID: 26740
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool upperCase;

	// Token: 0x04006875 RID: 26741
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool lowerCase;

	// Token: 0x04006876 RID: 26742
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool supportBbCode = true;

	// Token: 0x04006877 RID: 26743
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool supportUrls;

	// Token: 0x04006878 RID: 26744
	[PublicizedFrom(EAccessModifier.Protected)]
	public HashSet<string> supportedUrlTypes;

	// Token: 0x04006879 RID: 26745
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hadTooltipTextFromUrlHover;
}
