using System;
using System.Collections;
using Platform;
using UnityEngine;

// Token: 0x02001159 RID: 4441
public class XUiV_Label : XUiV_LabelBase
{
	// Token: 0x170010AC RID: 4268
	// (get) Token: 0x06008D56 RID: 36182 RVA: 0x003572DE File Offset: 0x003554DE
	// (set) Token: 0x06008D57 RID: 36183 RVA: 0x003572E6 File Offset: 0x003554E6
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

	// Token: 0x170010AD RID: 4269
	// (get) Token: 0x06008D58 RID: 36184 RVA: 0x003572F5 File Offset: 0x003554F5
	// (set) Token: 0x06008D59 RID: 36185 RVA: 0x003572FD File Offset: 0x003554FD
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

	// Token: 0x170010AE RID: 4270
	// (get) Token: 0x06008D5A RID: 36186 RVA: 0x0035730C File Offset: 0x0035550C
	// (set) Token: 0x06008D5B RID: 36187 RVA: 0x00357314 File Offset: 0x00355514
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

	// Token: 0x170010AF RID: 4271
	// (get) Token: 0x06008D5C RID: 36188 RVA: 0x00357323 File Offset: 0x00355523
	// (set) Token: 0x06008D5D RID: 36189 RVA: 0x0035732B File Offset: 0x0035552B
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

	// Token: 0x170010B0 RID: 4272
	// (get) Token: 0x06008D5E RID: 36190 RVA: 0x0035733A File Offset: 0x0035553A
	// (set) Token: 0x06008D5F RID: 36191 RVA: 0x00357342 File Offset: 0x00355542
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

	// Token: 0x170010B1 RID: 4273
	// (get) Token: 0x06008D60 RID: 36192 RVA: 0x0035735B File Offset: 0x0035555B
	// (set) Token: 0x06008D61 RID: 36193 RVA: 0x00357363 File Offset: 0x00355563
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

	// Token: 0x170010B2 RID: 4274
	// (get) Token: 0x06008D62 RID: 36194 RVA: 0x00357381 File Offset: 0x00355581
	// (set) Token: 0x06008D63 RID: 36195 RVA: 0x00357389 File Offset: 0x00355589
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

	// Token: 0x170010B3 RID: 4275
	// (get) Token: 0x06008D64 RID: 36196 RVA: 0x003573A2 File Offset: 0x003555A2
	// (set) Token: 0x06008D65 RID: 36197 RVA: 0x003573AC File Offset: 0x003555AC
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

	// Token: 0x170010B4 RID: 4276
	// (get) Token: 0x06008D66 RID: 36198 RVA: 0x003573FB File Offset: 0x003555FB
	// (set) Token: 0x06008D67 RID: 36199 RVA: 0x00357403 File Offset: 0x00355603
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

	// Token: 0x170010B5 RID: 4277
	// (get) Token: 0x06008D68 RID: 36200 RVA: 0x0035741C File Offset: 0x0035561C
	// (set) Token: 0x06008D69 RID: 36201 RVA: 0x00357424 File Offset: 0x00355624
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

	// Token: 0x06008D6A RID: 36202 RVA: 0x0035743D File Offset: 0x0035563D
	public XUiV_Label(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x06008D6B RID: 36203 RVA: 0x00357454 File Offset: 0x00355654
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

	// Token: 0x06008D6C RID: 36204 RVA: 0x003574D6 File Offset: 0x003556D6
	public override void SetDefaults(XUiController _parent)
	{
		base.SetDefaults(_parent);
		this.overflow = UILabel.Overflow.ShrinkContent;
	}

	// Token: 0x06008D6D RID: 36205 RVA: 0x003574E6 File Offset: 0x003556E6
	public override void Cleanup()
	{
		base.Cleanup();
		IPlatform nativePlatform = PlatformManager.NativePlatform;
		if (((nativePlatform != null) ? nativePlatform.Input : null) != null)
		{
			PlatformManager.NativePlatform.Input.OnLastInputStyleChanged -= this.OnLastInputStyleChanged;
		}
	}

	// Token: 0x06008D6E RID: 36206 RVA: 0x0035751C File Offset: 0x0035571C
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

	// Token: 0x170010B6 RID: 4278
	// (get) Token: 0x06008D6F RID: 36207 RVA: 0x00357536 File Offset: 0x00355736
	public override bool anchorsKeepSize
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.label.overflowMethod != UILabel.Overflow.ResizeFreely;
		}
	}

	// Token: 0x06008D70 RID: 36208 RVA: 0x0035754C File Offset: 0x0035574C
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

	// Token: 0x06008D71 RID: 36209 RVA: 0x003575DA File Offset: 0x003557DA
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnLastInputStyleChanged(PlayerInputManager.InputStyle _obj)
	{
		if (this.parseActions && this.currentTextHasActions)
		{
			this.ForceTextUpdate();
		}
	}

	// Token: 0x06008D72 RID: 36210 RVA: 0x003575F2 File Offset: 0x003557F2
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

	// Token: 0x06008D73 RID: 36211 RVA: 0x00357626 File Offset: 0x00355826
	public void SetTextImmediately(string _text)
	{
		this.text = _text ?? "";
		if (this.label == null)
		{
			return;
		}
		this.label.text = this.getFormattedText(this.text);
	}

	// Token: 0x06008D74 RID: 36212 RVA: 0x0035765E File Offset: 0x0035585E
	public void ForceTextUpdate()
	{
		this.bUpdateText = true;
		base.SetDirty();
	}

	// Token: 0x06008D75 RID: 36213 RVA: 0x0035766D File Offset: 0x0035586D
	[XuiXmlAttribute("text_key", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeTextKey(string _value)
	{
		if (!string.IsNullOrEmpty(_value))
		{
			this.Text = Localization.Get(_value, false, null);
		}
	}

	// Token: 0x06008D76 RID: 36214 RVA: 0x00357688 File Offset: 0x00355888
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.anchoredLeftAndRight || this.anchoredTopAndBottom || base.HasAnchorStringLeftAndRight || base.HasAnchorStringTopAndBottom || (this.Overflow != UILabel.Overflow.ShrinkContent && this.Overflow != UILabel.Overflow.ResizeFreely))
		{
			ThreadManager.StartCoroutine(this.markLabelChanged());
		}
	}

	// Token: 0x06008D77 RID: 36215 RVA: 0x003576D8 File Offset: 0x003558D8
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

	// Token: 0x04006802 RID: 26626
	[PublicizedFrom(EAccessModifier.Protected)]
	public UILabel.Overflow overflow;

	// Token: 0x04006803 RID: 26627
	[PublicizedFrom(EAccessModifier.Protected)]
	public int overflowHeight;

	// Token: 0x04006804 RID: 26628
	[PublicizedFrom(EAccessModifier.Protected)]
	public int overflowWidth;

	// Token: 0x04006805 RID: 26629
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool overflowEllipsis;

	// Token: 0x04006806 RID: 26630
	[PublicizedFrom(EAccessModifier.Protected)]
	public string text = "";

	// Token: 0x04006807 RID: 26631
	[PublicizedFrom(EAccessModifier.Protected)]
	public int maxLineCount;

	// Token: 0x04006808 RID: 26632
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool parseActions;

	// Token: 0x04006809 RID: 26633
	[PublicizedFrom(EAccessModifier.Protected)]
	public string actionsDefaultFormat;

	// Token: 0x0400680A RID: 26634
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool currentTextHasActions;

	// Token: 0x0400680B RID: 26635
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiUtils.ForceLabelInputStyle forceInputStyle;

	// Token: 0x0400680C RID: 26636
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool bUpdateText;

	// Token: 0x0400680D RID: 26637
	[PublicizedFrom(EAccessModifier.Private)]
	public bool useEllipsisAnimator;

	// Token: 0x0400680E RID: 26638
	[PublicizedFrom(EAccessModifier.Private)]
	public TextEllipsisAnimator ellipsisAnimator;

	// Token: 0x0400680F RID: 26639
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly WaitForSecondsRealtime markChangedDelay = new WaitForSecondsRealtime(0.01f);
}
