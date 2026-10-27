using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001183 RID: 4483
public abstract class XUiV_LabelBase : XUiView_WidgetBased
{
	// Token: 0x170010F9 RID: 4345
	// (get) Token: 0x06008E3E RID: 36414 RVA: 0x0035DC75 File Offset: 0x0035BE75
	// (set) Token: 0x06008E3F RID: 36415 RVA: 0x0035DC7D File Offset: 0x0035BE7D
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

	// Token: 0x170010FA RID: 4346
	// (get) Token: 0x06008E40 RID: 36416 RVA: 0x0035DC8C File Offset: 0x0035BE8C
	// (set) Token: 0x06008E41 RID: 36417 RVA: 0x0035DC94 File Offset: 0x0035BE94
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

	// Token: 0x170010FB RID: 4347
	// (get) Token: 0x06008E42 RID: 36418 RVA: 0x0035DCA3 File Offset: 0x0035BEA3
	// (set) Token: 0x06008E43 RID: 36419 RVA: 0x0035DCAB File Offset: 0x0035BEAB
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

	// Token: 0x170010FC RID: 4348
	// (get) Token: 0x06008E44 RID: 36420 RVA: 0x0035DCC4 File Offset: 0x0035BEC4
	// (set) Token: 0x06008E45 RID: 36421 RVA: 0x0035DCCC File Offset: 0x0035BECC
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

	// Token: 0x170010FD RID: 4349
	// (get) Token: 0x06008E46 RID: 36422 RVA: 0x0035DCE5 File Offset: 0x0035BEE5
	// (set) Token: 0x06008E47 RID: 36423 RVA: 0x0035DCED File Offset: 0x0035BEED
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

	// Token: 0x170010FE RID: 4350
	// (get) Token: 0x06008E48 RID: 36424 RVA: 0x0035DCFC File Offset: 0x0035BEFC
	// (set) Token: 0x06008E49 RID: 36425 RVA: 0x0035DD04 File Offset: 0x0035BF04
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

	// Token: 0x170010FF RID: 4351
	// (get) Token: 0x06008E4A RID: 36426 RVA: 0x0035DD13 File Offset: 0x0035BF13
	// (set) Token: 0x06008E4B RID: 36427 RVA: 0x0035DD1B File Offset: 0x0035BF1B
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

	// Token: 0x17001100 RID: 4352
	// (get) Token: 0x06008E4C RID: 36428 RVA: 0x0035DD2A File Offset: 0x0035BF2A
	// (set) Token: 0x06008E4D RID: 36429 RVA: 0x0035DD32 File Offset: 0x0035BF32
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

	// Token: 0x17001101 RID: 4353
	// (get) Token: 0x06008E4E RID: 36430 RVA: 0x0035DD41 File Offset: 0x0035BF41
	// (set) Token: 0x06008E4F RID: 36431 RVA: 0x0035DD49 File Offset: 0x0035BF49
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

	// Token: 0x17001102 RID: 4354
	// (get) Token: 0x06008E50 RID: 36432 RVA: 0x0035DD67 File Offset: 0x0035BF67
	// (set) Token: 0x06008E51 RID: 36433 RVA: 0x0035DD74 File Offset: 0x0035BF74
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

	// Token: 0x17001103 RID: 4355
	// (get) Token: 0x06008E52 RID: 36434 RVA: 0x0035DD9C File Offset: 0x0035BF9C
	// (set) Token: 0x06008E53 RID: 36435 RVA: 0x0035DDA4 File Offset: 0x0035BFA4
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

	// Token: 0x17001104 RID: 4356
	// (get) Token: 0x06008E54 RID: 36436 RVA: 0x0035DDBD File Offset: 0x0035BFBD
	// (set) Token: 0x06008E55 RID: 36437 RVA: 0x0035DDC5 File Offset: 0x0035BFC5
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

	// Token: 0x17001105 RID: 4357
	// (get) Token: 0x06008E56 RID: 36438 RVA: 0x0035DDDE File Offset: 0x0035BFDE
	// (set) Token: 0x06008E57 RID: 36439 RVA: 0x0035DDE6 File Offset: 0x0035BFE6
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

	// Token: 0x17001106 RID: 4358
	// (get) Token: 0x06008E58 RID: 36440 RVA: 0x0035DDFF File Offset: 0x0035BFFF
	// (set) Token: 0x06008E59 RID: 36441 RVA: 0x0035DE07 File Offset: 0x0035C007
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

	// Token: 0x17001107 RID: 4359
	// (get) Token: 0x06008E5A RID: 36442 RVA: 0x0035DE20 File Offset: 0x0035C020
	public Vector2 PrintedSize
	{
		get
		{
			return this.label.printedSize;
		}
	}

	// Token: 0x06008E5B RID: 36443 RVA: 0x0035DE2D File Offset: 0x0035C02D
	public XUiV_LabelBase(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x06008E5C RID: 36444 RVA: 0x0035DE5A File Offset: 0x0035C05A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createComponents(GameObject _go)
	{
		_go.AddComponent<UILabel>();
	}

	// Token: 0x06008E5D RID: 36445 RVA: 0x0035DE64 File Offset: 0x0035C064
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void captureComponents()
	{
		base.captureComponents();
		this.widget = (this.label = this.uiTransform.GetComponent<UILabel>());
	}

	// Token: 0x06008E5E RID: 36446 RVA: 0x0035DE94 File Offset: 0x0035C094
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

	// Token: 0x06008E5F RID: 36447 RVA: 0x0035DEED File Offset: 0x0035C0ED
	public override void SetDefaults(XUiController _parent)
	{
		base.SetDefaults(_parent);
		this.Alignment = NGUIText.Alignment.Left;
		this.FontSize = 16;
	}

	// Token: 0x06008E60 RID: 36448 RVA: 0x0035DF05 File Offset: 0x0035C105
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

	// Token: 0x06008E61 RID: 36449 RVA: 0x0035DF3C File Offset: 0x0035C13C
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

	// Token: 0x06008E62 RID: 36450 RVA: 0x0035DFAE File Offset: 0x0035C1AE
	public override void Update(float _dt)
	{
		if (this.isOver)
		{
			this.updateUrlTooltip();
		}
		base.Update(_dt);
	}

	// Token: 0x06008E63 RID: 36451 RVA: 0x0035DFC8 File Offset: 0x0035C1C8
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

	// Token: 0x06008E64 RID: 36452 RVA: 0x0035E0A4 File Offset: 0x0035C2A4
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

	// Token: 0x06008E65 RID: 36453 RVA: 0x0035E0C9 File Offset: 0x0035C2C9
	public void BindToUiInput(UIInput _input)
	{
		_input.label = this.label;
	}

	// Token: 0x06008E66 RID: 36454 RVA: 0x0035E0D7 File Offset: 0x0035C2D7
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

	// Token: 0x06008E67 RID: 36455 RVA: 0x0035E118 File Offset: 0x0035C318
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

	// Token: 0x040068A9 RID: 26793
	[PublicizedFrom(EAccessModifier.Protected)]
	public UILabel label;

	// Token: 0x040068AA RID: 26794
	[PublicizedFrom(EAccessModifier.Protected)]
	public NGUIFont uiFont;

	// Token: 0x040068AB RID: 26795
	[PublicizedFrom(EAccessModifier.Protected)]
	public int fontSize;

	// Token: 0x040068AC RID: 26796
	[PublicizedFrom(EAccessModifier.Protected)]
	public int spacingX = 1;

	// Token: 0x040068AD RID: 26797
	[PublicizedFrom(EAccessModifier.Protected)]
	public int spacingY;

	// Token: 0x040068AE RID: 26798
	[PublicizedFrom(EAccessModifier.Protected)]
	public UILabel.Effect effect;

	// Token: 0x040068AF RID: 26799
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color effectColor = new Color32(0, 0, 0, 80);

	// Token: 0x040068B0 RID: 26800
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector2 effectDistance;

	// Token: 0x040068B1 RID: 26801
	[PublicizedFrom(EAccessModifier.Protected)]
	public UILabel.Crispness crispness;

	// Token: 0x040068B2 RID: 26802
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color color;

	// Token: 0x040068B3 RID: 26803
	[PublicizedFrom(EAccessModifier.Protected)]
	public NGUIText.Alignment alignment;

	// Token: 0x040068B4 RID: 26804
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool upperCase;

	// Token: 0x040068B5 RID: 26805
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool lowerCase;

	// Token: 0x040068B6 RID: 26806
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool supportBbCode = true;

	// Token: 0x040068B7 RID: 26807
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool supportUrls;

	// Token: 0x040068B8 RID: 26808
	[PublicizedFrom(EAccessModifier.Protected)]
	public HashSet<string> supportedUrlTypes;

	// Token: 0x040068B9 RID: 26809
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hadTooltipTextFromUrlHover;
}
