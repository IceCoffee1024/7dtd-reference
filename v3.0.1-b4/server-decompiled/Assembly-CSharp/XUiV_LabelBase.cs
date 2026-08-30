using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001142 RID: 4418
public abstract class XUiV_LabelBase : XUiView_WidgetBased
{
	// Token: 0x17001080 RID: 4224
	// (get) Token: 0x06008C8D RID: 35981 RVA: 0x00352F49 File Offset: 0x00351149
	// (set) Token: 0x06008C8E RID: 35982 RVA: 0x00352F51 File Offset: 0x00351151
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

	// Token: 0x17001081 RID: 4225
	// (get) Token: 0x06008C8F RID: 35983 RVA: 0x00352F60 File Offset: 0x00351160
	// (set) Token: 0x06008C90 RID: 35984 RVA: 0x00352F68 File Offset: 0x00351168
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

	// Token: 0x17001082 RID: 4226
	// (get) Token: 0x06008C91 RID: 35985 RVA: 0x00352F77 File Offset: 0x00351177
	// (set) Token: 0x06008C92 RID: 35986 RVA: 0x00352F7F File Offset: 0x0035117F
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

	// Token: 0x17001083 RID: 4227
	// (get) Token: 0x06008C93 RID: 35987 RVA: 0x00352F98 File Offset: 0x00351198
	// (set) Token: 0x06008C94 RID: 35988 RVA: 0x00352FA0 File Offset: 0x003511A0
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

	// Token: 0x17001084 RID: 4228
	// (get) Token: 0x06008C95 RID: 35989 RVA: 0x00352FB9 File Offset: 0x003511B9
	// (set) Token: 0x06008C96 RID: 35990 RVA: 0x00352FC1 File Offset: 0x003511C1
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

	// Token: 0x17001085 RID: 4229
	// (get) Token: 0x06008C97 RID: 35991 RVA: 0x00352FD0 File Offset: 0x003511D0
	// (set) Token: 0x06008C98 RID: 35992 RVA: 0x00352FD8 File Offset: 0x003511D8
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

	// Token: 0x17001086 RID: 4230
	// (get) Token: 0x06008C99 RID: 35993 RVA: 0x00352FE7 File Offset: 0x003511E7
	// (set) Token: 0x06008C9A RID: 35994 RVA: 0x00352FEF File Offset: 0x003511EF
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

	// Token: 0x17001087 RID: 4231
	// (get) Token: 0x06008C9B RID: 35995 RVA: 0x00352FFE File Offset: 0x003511FE
	// (set) Token: 0x06008C9C RID: 35996 RVA: 0x00353006 File Offset: 0x00351206
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

	// Token: 0x17001088 RID: 4232
	// (get) Token: 0x06008C9D RID: 35997 RVA: 0x00353015 File Offset: 0x00351215
	// (set) Token: 0x06008C9E RID: 35998 RVA: 0x0035301D File Offset: 0x0035121D
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

	// Token: 0x17001089 RID: 4233
	// (get) Token: 0x06008C9F RID: 35999 RVA: 0x0035303B File Offset: 0x0035123B
	// (set) Token: 0x06008CA0 RID: 36000 RVA: 0x00353048 File Offset: 0x00351248
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

	// Token: 0x1700108A RID: 4234
	// (get) Token: 0x06008CA1 RID: 36001 RVA: 0x00353070 File Offset: 0x00351270
	// (set) Token: 0x06008CA2 RID: 36002 RVA: 0x00353078 File Offset: 0x00351278
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

	// Token: 0x1700108B RID: 4235
	// (get) Token: 0x06008CA3 RID: 36003 RVA: 0x00353091 File Offset: 0x00351291
	// (set) Token: 0x06008CA4 RID: 36004 RVA: 0x00353099 File Offset: 0x00351299
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

	// Token: 0x1700108C RID: 4236
	// (get) Token: 0x06008CA5 RID: 36005 RVA: 0x003530B2 File Offset: 0x003512B2
	// (set) Token: 0x06008CA6 RID: 36006 RVA: 0x003530BA File Offset: 0x003512BA
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

	// Token: 0x1700108D RID: 4237
	// (get) Token: 0x06008CA7 RID: 36007 RVA: 0x003530D3 File Offset: 0x003512D3
	// (set) Token: 0x06008CA8 RID: 36008 RVA: 0x003530DB File Offset: 0x003512DB
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

	// Token: 0x1700108E RID: 4238
	// (get) Token: 0x06008CA9 RID: 36009 RVA: 0x003530F4 File Offset: 0x003512F4
	public Vector2 PrintedSize
	{
		get
		{
			return this.label.printedSize;
		}
	}

	// Token: 0x06008CAA RID: 36010 RVA: 0x00353101 File Offset: 0x00351301
	public XUiV_LabelBase(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x06008CAB RID: 36011 RVA: 0x0035312E File Offset: 0x0035132E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createComponents(GameObject _go)
	{
		_go.AddComponent<UILabel>();
	}

	// Token: 0x06008CAC RID: 36012 RVA: 0x00353138 File Offset: 0x00351338
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void captureComponents()
	{
		base.captureComponents();
		this.widget = (this.label = this.uiTransform.GetComponent<UILabel>());
	}

	// Token: 0x06008CAD RID: 36013 RVA: 0x00353168 File Offset: 0x00351368
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

	// Token: 0x06008CAE RID: 36014 RVA: 0x003531C1 File Offset: 0x003513C1
	public override void SetDefaults(XUiController _parent)
	{
		base.SetDefaults(_parent);
		this.Alignment = NGUIText.Alignment.Left;
		this.FontSize = 16;
	}

	// Token: 0x06008CAF RID: 36015 RVA: 0x003531D9 File Offset: 0x003513D9
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

	// Token: 0x06008CB0 RID: 36016 RVA: 0x00353210 File Offset: 0x00351410
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

	// Token: 0x06008CB1 RID: 36017 RVA: 0x00353282 File Offset: 0x00351482
	public override void Update(float _dt)
	{
		if (this.isOver)
		{
			this.updateUrlTooltip();
		}
		base.Update(_dt);
	}

	// Token: 0x06008CB2 RID: 36018 RVA: 0x0035329C File Offset: 0x0035149C
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

	// Token: 0x06008CB3 RID: 36019 RVA: 0x00353378 File Offset: 0x00351578
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

	// Token: 0x06008CB4 RID: 36020 RVA: 0x0035339D File Offset: 0x0035159D
	public void BindToUiInput(UIInput _input)
	{
		_input.label = this.label;
	}

	// Token: 0x06008CB5 RID: 36021 RVA: 0x003533AB File Offset: 0x003515AB
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

	// Token: 0x06008CB6 RID: 36022 RVA: 0x003533EC File Offset: 0x003515EC
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

	// Token: 0x0400674A RID: 26442
	[PublicizedFrom(EAccessModifier.Protected)]
	public UILabel label;

	// Token: 0x0400674B RID: 26443
	[PublicizedFrom(EAccessModifier.Protected)]
	public NGUIFont uiFont;

	// Token: 0x0400674C RID: 26444
	[PublicizedFrom(EAccessModifier.Protected)]
	public int fontSize;

	// Token: 0x0400674D RID: 26445
	[PublicizedFrom(EAccessModifier.Protected)]
	public int spacingX = 1;

	// Token: 0x0400674E RID: 26446
	[PublicizedFrom(EAccessModifier.Protected)]
	public int spacingY;

	// Token: 0x0400674F RID: 26447
	[PublicizedFrom(EAccessModifier.Protected)]
	public UILabel.Effect effect;

	// Token: 0x04006750 RID: 26448
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color effectColor = new Color32(0, 0, 0, 80);

	// Token: 0x04006751 RID: 26449
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector2 effectDistance;

	// Token: 0x04006752 RID: 26450
	[PublicizedFrom(EAccessModifier.Protected)]
	public UILabel.Crispness crispness;

	// Token: 0x04006753 RID: 26451
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color color;

	// Token: 0x04006754 RID: 26452
	[PublicizedFrom(EAccessModifier.Protected)]
	public NGUIText.Alignment alignment;

	// Token: 0x04006755 RID: 26453
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool upperCase;

	// Token: 0x04006756 RID: 26454
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool lowerCase;

	// Token: 0x04006757 RID: 26455
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool supportBbCode = true;

	// Token: 0x04006758 RID: 26456
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool supportUrls;

	// Token: 0x04006759 RID: 26457
	[PublicizedFrom(EAccessModifier.Protected)]
	public HashSet<string> supportedUrlTypes;

	// Token: 0x0400675A RID: 26458
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hadTooltipTextFromUrlHover;
}
