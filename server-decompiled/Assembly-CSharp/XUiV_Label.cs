using System;
using System.Collections;
using Platform;
using UnityEngine;

// Token: 0x02001171 RID: 4465
public class XUiV_Label : XUiV_LabelBase
{
	// Token: 0x170010E1 RID: 4321
	// (get) Token: 0x06008DC1 RID: 36289 RVA: 0x0035B6FE File Offset: 0x003598FE
	// (set) Token: 0x06008DC2 RID: 36290 RVA: 0x0035B706 File Offset: 0x00359906
	[XuiXmlAttribute("overflow", false)]
	public UILabel.Overflow Overflow
	{
		get
		{
			return this.overflow;
		}
		set
		{
			this.overflow = value;
			base.SetDirty();
		}
	}

	// Token: 0x170010E2 RID: 4322
	// (get) Token: 0x06008DC3 RID: 36291 RVA: 0x0035B715 File Offset: 0x00359915
	// (set) Token: 0x06008DC4 RID: 36292 RVA: 0x0035B71D File Offset: 0x0035991D
	[XuiXmlAttribute("overflow_height", false)]
	public int OverflowHeight
	{
		get
		{
			return this.overflowHeight;
		}
		set
		{
			this.overflowHeight = value;
			base.SetDirty();
		}
	}

	// Token: 0x170010E3 RID: 4323
	// (get) Token: 0x06008DC5 RID: 36293 RVA: 0x0035B72C File Offset: 0x0035992C
	// (set) Token: 0x06008DC6 RID: 36294 RVA: 0x0035B734 File Offset: 0x00359934
	[XuiXmlAttribute("overflow_width", false)]
	public int OverflowWidth
	{
		get
		{
			return this.overflowWidth;
		}
		set
		{
			this.overflowWidth = value;
			base.SetDirty();
		}
	}

	// Token: 0x170010E4 RID: 4324
	// (get) Token: 0x06008DC7 RID: 36295 RVA: 0x0035B743 File Offset: 0x00359943
	// (set) Token: 0x06008DC8 RID: 36296 RVA: 0x0035B74B File Offset: 0x0035994B
	[XuiXmlAttribute("overflow_ellipsis", false)]
	public bool OverflowEllipsis
	{
		get
		{
			return this.overflowEllipsis;
		}
		set
		{
			this.overflowEllipsis = value;
			base.SetDirty();
		}
	}

	// Token: 0x170010E5 RID: 4325
	// (get) Token: 0x06008DC9 RID: 36297 RVA: 0x0035B75A File Offset: 0x0035995A
	// (set) Token: 0x06008DCA RID: 36298 RVA: 0x0035B762 File Offset: 0x00359962
	[XuiXmlAttribute("parse_actions", false)]
	public bool ParseActions
	{
		get
		{
			return this.parseActions;
		}
		set
		{
			if (this.parseActions == value)
			{
				return;
			}
			this.parseActions = value;
			base.SetDirty();
		}
	}

	// Token: 0x170010E6 RID: 4326
	// (get) Token: 0x06008DCB RID: 36299 RVA: 0x0035B77B File Offset: 0x0035997B
	// (set) Token: 0x06008DCC RID: 36300 RVA: 0x0035B783 File Offset: 0x00359983
	[XuiXmlAttribute("actions_default_format", false)]
	public string ActionsDefaultFormat
	{
		get
		{
			return this.actionsDefaultFormat;
		}
		set
		{
			if (this.actionsDefaultFormat == value)
			{
				return;
			}
			this.actionsDefaultFormat = value;
			base.SetDirty();
		}
	}

	// Token: 0x170010E7 RID: 4327
	// (get) Token: 0x06008DCD RID: 36301 RVA: 0x0035B7A1 File Offset: 0x003599A1
	// (set) Token: 0x06008DCE RID: 36302 RVA: 0x0035B7A9 File Offset: 0x003599A9
	[XuiXmlAttribute("force_input_style", false)]
	public XUiUtils.ForceLabelInputStyle ForceInputStyle
	{
		get
		{
			return this.forceInputStyle;
		}
		set
		{
			if (this.forceInputStyle == value)
			{
				return;
			}
			this.forceInputStyle = value;
			base.SetDirty();
		}
	}

	// Token: 0x170010E8 RID: 4328
	// (get) Token: 0x06008DCF RID: 36303 RVA: 0x0035B7C2 File Offset: 0x003599C2
	// (set) Token: 0x06008DD0 RID: 36304 RVA: 0x0035B7CC File Offset: 0x003599CC
	[XuiXmlAttribute("text", false)]
	public string Text
	{
		get
		{
			return this.text;
		}
		set
		{
			if (this.text == value)
			{
				return;
			}
			this.text = value ?? "";
			if (this.ellipsisAnimator == null)
			{
				base.SetDirty();
				this.bUpdateText = true;
				return;
			}
			this.ellipsisAnimator.SetBaseString(value, TextEllipsisAnimator.AnimationMode.All);
		}
	}

	// Token: 0x170010E9 RID: 4329
	// (get) Token: 0x06008DD1 RID: 36305 RVA: 0x0035B81B File Offset: 0x00359A1B
	// (set) Token: 0x06008DD2 RID: 36306 RVA: 0x0035B823 File Offset: 0x00359A23
	[XuiXmlAttribute("max_line_count", false)]
	public int MaxLineCount
	{
		get
		{
			return this.maxLineCount;
		}
		set
		{
			if (value == this.maxLineCount)
			{
				return;
			}
			this.maxLineCount = value;
			base.SetDirty();
		}
	}

	// Token: 0x170010EA RID: 4330
	// (get) Token: 0x06008DD3 RID: 36307 RVA: 0x0035B83C File Offset: 0x00359A3C
	// (set) Token: 0x06008DD4 RID: 36308 RVA: 0x0035B844 File Offset: 0x00359A44
	[XuiXmlAttribute("use_ellipsis_animator", false)]
	public bool UseEllipsisAnimator
	{
		get
		{
			return this.useEllipsisAnimator;
		}
		set
		{
			if (this.useEllipsisAnimator == value)
			{
				return;
			}
			this.useEllipsisAnimator = value;
			base.SetDirty();
		}
	}

	// Token: 0x06008DD5 RID: 36309 RVA: 0x0035B85D File Offset: 0x00359A5D
	public XUiV_Label(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x06008DD6 RID: 36310 RVA: 0x0035B874 File Offset: 0x00359A74
	public override void InitView()
	{
		base.InitView();
		if (this.useEllipsisAnimator)
		{
			if (this.supportBbCode)
			{
				this.ellipsisAnimator = new TextEllipsisAnimator(this.text, this, this.label);
			}
			else
			{
				Log.Warning("[XUi] Not enabling EllipsisAnimator on label, requires support_bb_code to be true. On " + base.Controller.GetParentWindow().ID + "." + base.ID);
			}
		}
		PlatformManager.NativePlatform.Input.OnLastInputStyleChanged += this.OnLastInputStyleChanged;
	}

	// Token: 0x06008DD7 RID: 36311 RVA: 0x0035B8F6 File Offset: 0x00359AF6
	public override void SetDefaults(XUiController _parent)
	{
		base.SetDefaults(_parent);
		this.overflow = UILabel.Overflow.ShrinkContent;
	}

	// Token: 0x06008DD8 RID: 36312 RVA: 0x0035B906 File Offset: 0x00359B06
	public override void Cleanup()
	{
		base.Cleanup();
		IPlatform nativePlatform = PlatformManager.NativePlatform;
		if (((nativePlatform != null) ? nativePlatform.Input : null) != null)
		{
			PlatformManager.NativePlatform.Input.OnLastInputStyleChanged -= this.OnLastInputStyleChanged;
		}
	}

	// Token: 0x06008DD9 RID: 36313 RVA: 0x0035B93C File Offset: 0x00359B3C
	public override void Update(float _dt)
	{
		base.Update(_dt);
		TextEllipsisAnimator textEllipsisAnimator = this.ellipsisAnimator;
		if (textEllipsisAnimator == null)
		{
			return;
		}
		textEllipsisAnimator.GetNextAnimatedString(_dt);
	}

	// Token: 0x170010EB RID: 4331
	// (get) Token: 0x06008DDA RID: 36314 RVA: 0x0035B956 File Offset: 0x00359B56
	public override bool anchorsKeepSize
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.label.overflowMethod != UILabel.Overflow.ResizeFreely;
		}
	}

	// Token: 0x06008DDB RID: 36315 RVA: 0x0035B96C File Offset: 0x00359B6C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void updateData()
	{
		if (this.bUpdateText)
		{
			this.label.text = this.getFormattedText(this.text);
			this.bUpdateText = false;
		}
		this.label.overflowMethod = this.overflow;
		this.label.overflowWidth = this.overflowWidth;
		this.label.overflowHeight = this.overflowHeight;
		this.label.overflowEllipsis = this.overflowEllipsis;
		this.label.maxLineCount = this.maxLineCount;
		base.updateData();
	}

	// Token: 0x06008DDC RID: 36316 RVA: 0x0035B9FA File Offset: 0x00359BFA
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnLastInputStyleChanged(PlayerInputManager.InputStyle _obj)
	{
		if (this.parseActions && this.currentTextHasActions)
		{
			this.ForceTextUpdate();
		}
	}

	// Token: 0x06008DDD RID: 36317 RVA: 0x0035BA12 File Offset: 0x00359C12
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getFormattedText(string _text)
	{
		_text = base.getFormattedText(_text);
		if (this.parseActions)
		{
			this.currentTextHasActions = XUiUtils.ParseActionsMarkup(this.xui, _text, out _text, this.actionsDefaultFormat, this.forceInputStyle);
		}
		return _text;
	}

	// Token: 0x06008DDE RID: 36318 RVA: 0x0035BA46 File Offset: 0x00359C46
	public void SetTextImmediately(string _text)
	{
		this.text = _text ?? "";
		if (this.label == null)
		{
			return;
		}
		this.label.text = this.getFormattedText(this.text);
	}

	// Token: 0x06008DDF RID: 36319 RVA: 0x0035BA7E File Offset: 0x00359C7E
	public void ForceTextUpdate()
	{
		this.bUpdateText = true;
		base.SetDirty();
	}

	// Token: 0x06008DE0 RID: 36320 RVA: 0x0035BA8D File Offset: 0x00359C8D
	[XuiXmlAttribute("text_key", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeTextKey(string _value)
	{
		if (!string.IsNullOrEmpty(_value))
		{
			this.Text = Localization.Get(_value, false, null);
		}
	}

	// Token: 0x06008DE1 RID: 36321 RVA: 0x0035BAA8 File Offset: 0x00359CA8
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.anchoredLeftAndRight || this.anchoredTopAndBottom || base.HasAnchorStringLeftAndRight || base.HasAnchorStringTopAndBottom || (this.Overflow != UILabel.Overflow.ShrinkContent && this.Overflow != UILabel.Overflow.ResizeFreely))
		{
			ThreadManager.StartCoroutine(this.markLabelChanged());
		}
	}

	// Token: 0x06008DE2 RID: 36322 RVA: 0x0035BAF8 File Offset: 0x00359CF8
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator markLabelChanged()
	{
		int num;
		for (int i = 0; i < 11; i = num + 1)
		{
			yield return XUiV_Label.markChangedDelay;
			this.label.MarkAsChanged();
			num = i;
		}
		yield break;
	}

	// Token: 0x04006857 RID: 26711
	[PublicizedFrom(EAccessModifier.Protected)]
	public UILabel.Overflow overflow;

	// Token: 0x04006858 RID: 26712
	[PublicizedFrom(EAccessModifier.Protected)]
	public int overflowHeight;

	// Token: 0x04006859 RID: 26713
	[PublicizedFrom(EAccessModifier.Protected)]
	public int overflowWidth;

	// Token: 0x0400685A RID: 26714
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool overflowEllipsis;

	// Token: 0x0400685B RID: 26715
	[PublicizedFrom(EAccessModifier.Protected)]
	public string text = "";

	// Token: 0x0400685C RID: 26716
	[PublicizedFrom(EAccessModifier.Protected)]
	public int maxLineCount;

	// Token: 0x0400685D RID: 26717
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool parseActions;

	// Token: 0x0400685E RID: 26718
	[PublicizedFrom(EAccessModifier.Protected)]
	public string actionsDefaultFormat;

	// Token: 0x0400685F RID: 26719
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool currentTextHasActions;

	// Token: 0x04006860 RID: 26720
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiUtils.ForceLabelInputStyle forceInputStyle;

	// Token: 0x04006861 RID: 26721
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool bUpdateText;

	// Token: 0x04006862 RID: 26722
	[PublicizedFrom(EAccessModifier.Private)]
	public bool useEllipsisAnimator;

	// Token: 0x04006863 RID: 26723
	[PublicizedFrom(EAccessModifier.Private)]
	public TextEllipsisAnimator ellipsisAnimator;

	// Token: 0x04006864 RID: 26724
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly WaitForSecondsRealtime markChangedDelay = new WaitForSecondsRealtime(0.01f);
}
