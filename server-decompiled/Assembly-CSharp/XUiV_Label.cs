using System;
using System.Collections;
using Platform;
using UnityEngine;

// Token: 0x02001181 RID: 4481
public class XUiV_Label : XUiV_LabelBase
{
	// Token: 0x170010EC RID: 4332
	// (get) Token: 0x06008E15 RID: 36373 RVA: 0x0035D7CE File Offset: 0x0035B9CE
	// (set) Token: 0x06008E16 RID: 36374 RVA: 0x0035D7D6 File Offset: 0x0035B9D6
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

	// Token: 0x170010ED RID: 4333
	// (get) Token: 0x06008E17 RID: 36375 RVA: 0x0035D7E5 File Offset: 0x0035B9E5
	// (set) Token: 0x06008E18 RID: 36376 RVA: 0x0035D7ED File Offset: 0x0035B9ED
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

	// Token: 0x170010EE RID: 4334
	// (get) Token: 0x06008E19 RID: 36377 RVA: 0x0035D7FC File Offset: 0x0035B9FC
	// (set) Token: 0x06008E1A RID: 36378 RVA: 0x0035D804 File Offset: 0x0035BA04
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

	// Token: 0x170010EF RID: 4335
	// (get) Token: 0x06008E1B RID: 36379 RVA: 0x0035D813 File Offset: 0x0035BA13
	// (set) Token: 0x06008E1C RID: 36380 RVA: 0x0035D81B File Offset: 0x0035BA1B
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

	// Token: 0x170010F0 RID: 4336
	// (get) Token: 0x06008E1D RID: 36381 RVA: 0x0035D82A File Offset: 0x0035BA2A
	// (set) Token: 0x06008E1E RID: 36382 RVA: 0x0035D832 File Offset: 0x0035BA32
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

	// Token: 0x170010F1 RID: 4337
	// (get) Token: 0x06008E1F RID: 36383 RVA: 0x0035D84B File Offset: 0x0035BA4B
	// (set) Token: 0x06008E20 RID: 36384 RVA: 0x0035D853 File Offset: 0x0035BA53
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

	// Token: 0x170010F2 RID: 4338
	// (get) Token: 0x06008E21 RID: 36385 RVA: 0x0035D871 File Offset: 0x0035BA71
	// (set) Token: 0x06008E22 RID: 36386 RVA: 0x0035D879 File Offset: 0x0035BA79
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

	// Token: 0x170010F3 RID: 4339
	// (get) Token: 0x06008E23 RID: 36387 RVA: 0x0035D892 File Offset: 0x0035BA92
	// (set) Token: 0x06008E24 RID: 36388 RVA: 0x0035D89C File Offset: 0x0035BA9C
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

	// Token: 0x170010F4 RID: 4340
	// (get) Token: 0x06008E25 RID: 36389 RVA: 0x0035D8EB File Offset: 0x0035BAEB
	// (set) Token: 0x06008E26 RID: 36390 RVA: 0x0035D8F3 File Offset: 0x0035BAF3
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

	// Token: 0x170010F5 RID: 4341
	// (get) Token: 0x06008E27 RID: 36391 RVA: 0x0035D90C File Offset: 0x0035BB0C
	// (set) Token: 0x06008E28 RID: 36392 RVA: 0x0035D914 File Offset: 0x0035BB14
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

	// Token: 0x06008E29 RID: 36393 RVA: 0x0035D92D File Offset: 0x0035BB2D
	public XUiV_Label(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x06008E2A RID: 36394 RVA: 0x0035D944 File Offset: 0x0035BB44
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

	// Token: 0x06008E2B RID: 36395 RVA: 0x0035D9C6 File Offset: 0x0035BBC6
	public override void SetDefaults(XUiController _parent)
	{
		base.SetDefaults(_parent);
		this.overflow = UILabel.Overflow.ShrinkContent;
	}

	// Token: 0x06008E2C RID: 36396 RVA: 0x0035D9D6 File Offset: 0x0035BBD6
	public override void Cleanup()
	{
		base.Cleanup();
		IPlatform nativePlatform = PlatformManager.NativePlatform;
		if (((nativePlatform != null) ? nativePlatform.Input : null) != null)
		{
			PlatformManager.NativePlatform.Input.OnLastInputStyleChanged -= this.OnLastInputStyleChanged;
		}
	}

	// Token: 0x06008E2D RID: 36397 RVA: 0x0035DA0C File Offset: 0x0035BC0C
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

	// Token: 0x170010F6 RID: 4342
	// (get) Token: 0x06008E2E RID: 36398 RVA: 0x0035DA26 File Offset: 0x0035BC26
	public override bool anchorsKeepSize
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.label.overflowMethod != UILabel.Overflow.ResizeFreely;
		}
	}

	// Token: 0x06008E2F RID: 36399 RVA: 0x0035DA3C File Offset: 0x0035BC3C
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

	// Token: 0x06008E30 RID: 36400 RVA: 0x0035DACA File Offset: 0x0035BCCA
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnLastInputStyleChanged(PlayerInputManager.InputStyle _obj)
	{
		if (this.parseActions && this.currentTextHasActions)
		{
			this.ForceTextUpdate();
		}
	}

	// Token: 0x06008E31 RID: 36401 RVA: 0x0035DAE2 File Offset: 0x0035BCE2
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

	// Token: 0x06008E32 RID: 36402 RVA: 0x0035DB16 File Offset: 0x0035BD16
	public void SetTextImmediately(string _text)
	{
		this.text = _text ?? "";
		if (this.label == null)
		{
			return;
		}
		this.label.text = this.getFormattedText(this.text);
	}

	// Token: 0x06008E33 RID: 36403 RVA: 0x0035DB4E File Offset: 0x0035BD4E
	public void ForceTextUpdate()
	{
		this.bUpdateText = true;
		base.SetDirty();
	}

	// Token: 0x06008E34 RID: 36404 RVA: 0x0035DB5D File Offset: 0x0035BD5D
	[XuiXmlAttribute("text_key", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeTextKey(string _value)
	{
		if (!string.IsNullOrEmpty(_value))
		{
			this.Text = Localization.Get(_value, false, null);
		}
	}

	// Token: 0x06008E35 RID: 36405 RVA: 0x0035DB78 File Offset: 0x0035BD78
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.anchoredLeftAndRight || this.anchoredTopAndBottom || base.HasAnchorStringLeftAndRight || base.HasAnchorStringTopAndBottom || (this.Overflow != UILabel.Overflow.ShrinkContent && this.Overflow != UILabel.Overflow.ResizeFreely))
		{
			ThreadManager.StartCoroutine(this.markLabelChanged());
		}
	}

	// Token: 0x06008E36 RID: 36406 RVA: 0x0035DBC8 File Offset: 0x0035BDC8
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

	// Token: 0x04006897 RID: 26775
	[PublicizedFrom(EAccessModifier.Protected)]
	public UILabel.Overflow overflow;

	// Token: 0x04006898 RID: 26776
	[PublicizedFrom(EAccessModifier.Protected)]
	public int overflowHeight;

	// Token: 0x04006899 RID: 26777
	[PublicizedFrom(EAccessModifier.Protected)]
	public int overflowWidth;

	// Token: 0x0400689A RID: 26778
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool overflowEllipsis;

	// Token: 0x0400689B RID: 26779
	[PublicizedFrom(EAccessModifier.Protected)]
	public string text = "";

	// Token: 0x0400689C RID: 26780
	[PublicizedFrom(EAccessModifier.Protected)]
	public int maxLineCount;

	// Token: 0x0400689D RID: 26781
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool parseActions;

	// Token: 0x0400689E RID: 26782
	[PublicizedFrom(EAccessModifier.Protected)]
	public string actionsDefaultFormat;

	// Token: 0x0400689F RID: 26783
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool currentTextHasActions;

	// Token: 0x040068A0 RID: 26784
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiUtils.ForceLabelInputStyle forceInputStyle;

	// Token: 0x040068A1 RID: 26785
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool bUpdateText;

	// Token: 0x040068A2 RID: 26786
	[PublicizedFrom(EAccessModifier.Private)]
	public bool useEllipsisAnimator;

	// Token: 0x040068A3 RID: 26787
	[PublicizedFrom(EAccessModifier.Private)]
	public TextEllipsisAnimator ellipsisAnimator;

	// Token: 0x040068A4 RID: 26788
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly WaitForSecondsRealtime markChangedDelay = new WaitForSecondsRealtime(0.01f);
}
