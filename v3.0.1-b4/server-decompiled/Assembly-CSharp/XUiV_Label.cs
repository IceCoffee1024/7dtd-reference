using System;
using System.Collections;
using Platform;
using UnityEngine;

// Token: 0x02001140 RID: 4416
public class XUiV_Label : XUiV_LabelBase
{
	// Token: 0x17001073 RID: 4211
	// (get) Token: 0x06008C64 RID: 35940 RVA: 0x00352AA2 File Offset: 0x00350CA2
	// (set) Token: 0x06008C65 RID: 35941 RVA: 0x00352AAA File Offset: 0x00350CAA
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

	// Token: 0x17001074 RID: 4212
	// (get) Token: 0x06008C66 RID: 35942 RVA: 0x00352AB9 File Offset: 0x00350CB9
	// (set) Token: 0x06008C67 RID: 35943 RVA: 0x00352AC1 File Offset: 0x00350CC1
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

	// Token: 0x17001075 RID: 4213
	// (get) Token: 0x06008C68 RID: 35944 RVA: 0x00352AD0 File Offset: 0x00350CD0
	// (set) Token: 0x06008C69 RID: 35945 RVA: 0x00352AD8 File Offset: 0x00350CD8
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

	// Token: 0x17001076 RID: 4214
	// (get) Token: 0x06008C6A RID: 35946 RVA: 0x00352AE7 File Offset: 0x00350CE7
	// (set) Token: 0x06008C6B RID: 35947 RVA: 0x00352AEF File Offset: 0x00350CEF
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

	// Token: 0x17001077 RID: 4215
	// (get) Token: 0x06008C6C RID: 35948 RVA: 0x00352AFE File Offset: 0x00350CFE
	// (set) Token: 0x06008C6D RID: 35949 RVA: 0x00352B06 File Offset: 0x00350D06
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

	// Token: 0x17001078 RID: 4216
	// (get) Token: 0x06008C6E RID: 35950 RVA: 0x00352B1F File Offset: 0x00350D1F
	// (set) Token: 0x06008C6F RID: 35951 RVA: 0x00352B27 File Offset: 0x00350D27
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

	// Token: 0x17001079 RID: 4217
	// (get) Token: 0x06008C70 RID: 35952 RVA: 0x00352B45 File Offset: 0x00350D45
	// (set) Token: 0x06008C71 RID: 35953 RVA: 0x00352B4D File Offset: 0x00350D4D
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

	// Token: 0x1700107A RID: 4218
	// (get) Token: 0x06008C72 RID: 35954 RVA: 0x00352B66 File Offset: 0x00350D66
	// (set) Token: 0x06008C73 RID: 35955 RVA: 0x00352B70 File Offset: 0x00350D70
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

	// Token: 0x1700107B RID: 4219
	// (get) Token: 0x06008C74 RID: 35956 RVA: 0x00352BBF File Offset: 0x00350DBF
	// (set) Token: 0x06008C75 RID: 35957 RVA: 0x00352BC7 File Offset: 0x00350DC7
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

	// Token: 0x1700107C RID: 4220
	// (get) Token: 0x06008C76 RID: 35958 RVA: 0x00352BE0 File Offset: 0x00350DE0
	// (set) Token: 0x06008C77 RID: 35959 RVA: 0x00352BE8 File Offset: 0x00350DE8
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

	// Token: 0x06008C78 RID: 35960 RVA: 0x00352C01 File Offset: 0x00350E01
	public XUiV_Label(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x06008C79 RID: 35961 RVA: 0x00352C18 File Offset: 0x00350E18
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

	// Token: 0x06008C7A RID: 35962 RVA: 0x00352C9A File Offset: 0x00350E9A
	public override void SetDefaults(XUiController _parent)
	{
		base.SetDefaults(_parent);
		this.overflow = UILabel.Overflow.ShrinkContent;
	}

	// Token: 0x06008C7B RID: 35963 RVA: 0x00352CAA File Offset: 0x00350EAA
	public override void Cleanup()
	{
		base.Cleanup();
		IPlatform nativePlatform = PlatformManager.NativePlatform;
		if (((nativePlatform != null) ? nativePlatform.Input : null) != null)
		{
			PlatformManager.NativePlatform.Input.OnLastInputStyleChanged -= this.OnLastInputStyleChanged;
		}
	}

	// Token: 0x06008C7C RID: 35964 RVA: 0x00352CE0 File Offset: 0x00350EE0
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

	// Token: 0x1700107D RID: 4221
	// (get) Token: 0x06008C7D RID: 35965 RVA: 0x00352CFA File Offset: 0x00350EFA
	public override bool anchorsKeepSize
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.label.overflowMethod != UILabel.Overflow.ResizeFreely;
		}
	}

	// Token: 0x06008C7E RID: 35966 RVA: 0x00352D10 File Offset: 0x00350F10
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

	// Token: 0x06008C7F RID: 35967 RVA: 0x00352D9E File Offset: 0x00350F9E
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnLastInputStyleChanged(PlayerInputManager.InputStyle _obj)
	{
		if (this.parseActions && this.currentTextHasActions)
		{
			this.ForceTextUpdate();
		}
	}

	// Token: 0x06008C80 RID: 35968 RVA: 0x00352DB6 File Offset: 0x00350FB6
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

	// Token: 0x06008C81 RID: 35969 RVA: 0x00352DEA File Offset: 0x00350FEA
	public void SetTextImmediately(string _text)
	{
		this.text = _text ?? "";
		if (this.label == null)
		{
			return;
		}
		this.label.text = this.getFormattedText(this.text);
	}

	// Token: 0x06008C82 RID: 35970 RVA: 0x00352E22 File Offset: 0x00351022
	public void ForceTextUpdate()
	{
		this.bUpdateText = true;
		base.SetDirty();
	}

	// Token: 0x06008C83 RID: 35971 RVA: 0x00352E31 File Offset: 0x00351031
	[XuiXmlAttribute("text_key", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeTextKey(string _value)
	{
		if (!string.IsNullOrEmpty(_value))
		{
			this.Text = Localization.Get(_value, false, null);
		}
	}

	// Token: 0x06008C84 RID: 35972 RVA: 0x00352E4C File Offset: 0x0035104C
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.anchoredLeftAndRight || this.anchoredTopAndBottom || base.HasAnchorStringLeftAndRight || base.HasAnchorStringTopAndBottom || (this.Overflow != UILabel.Overflow.ShrinkContent && this.Overflow != UILabel.Overflow.ResizeFreely))
		{
			ThreadManager.StartCoroutine(this.markLabelChanged());
		}
	}

	// Token: 0x06008C85 RID: 35973 RVA: 0x00352E9C File Offset: 0x0035109C
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

	// Token: 0x04006738 RID: 26424
	[PublicizedFrom(EAccessModifier.Protected)]
	public UILabel.Overflow overflow;

	// Token: 0x04006739 RID: 26425
	[PublicizedFrom(EAccessModifier.Protected)]
	public int overflowHeight;

	// Token: 0x0400673A RID: 26426
	[PublicizedFrom(EAccessModifier.Protected)]
	public int overflowWidth;

	// Token: 0x0400673B RID: 26427
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool overflowEllipsis;

	// Token: 0x0400673C RID: 26428
	[PublicizedFrom(EAccessModifier.Protected)]
	public string text = "";

	// Token: 0x0400673D RID: 26429
	[PublicizedFrom(EAccessModifier.Protected)]
	public int maxLineCount;

	// Token: 0x0400673E RID: 26430
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool parseActions;

	// Token: 0x0400673F RID: 26431
	[PublicizedFrom(EAccessModifier.Protected)]
	public string actionsDefaultFormat;

	// Token: 0x04006740 RID: 26432
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool currentTextHasActions;

	// Token: 0x04006741 RID: 26433
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiUtils.ForceLabelInputStyle forceInputStyle;

	// Token: 0x04006742 RID: 26434
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool bUpdateText;

	// Token: 0x04006743 RID: 26435
	[PublicizedFrom(EAccessModifier.Private)]
	public bool useEllipsisAnimator;

	// Token: 0x04006744 RID: 26436
	[PublicizedFrom(EAccessModifier.Private)]
	public TextEllipsisAnimator ellipsisAnimator;

	// Token: 0x04006745 RID: 26437
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly WaitForSecondsRealtime markChangedDelay = new WaitForSecondsRealtime(0.01f);
}
