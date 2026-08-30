using System;
using UnityEngine;

// Token: 0x0200112B RID: 4395
public abstract class XUiTweenAbs : IXUiElement
{
	// Token: 0x17000FE8 RID: 4072
	// (get) Token: 0x06008AE0 RID: 35552 RVA: 0x0034E625 File Offset: 0x0034C825
	// (set) Token: 0x06008AE1 RID: 35553 RVA: 0x0034E62D File Offset: 0x0034C82D
	[XuiXmlAttribute("repeat", false)]
	public UITweener.Style RepeatStyle
	{
		get
		{
			return this.repeatStyle;
		}
		set
		{
			if (this.repeatStyle == value)
			{
				return;
			}
			this.repeatStyle = value;
			if (this.tweenGeneric != null)
			{
				this.tweenGeneric.style = value;
			}
		}
	}

	// Token: 0x17000FE9 RID: 4073
	// (get) Token: 0x06008AE2 RID: 35554 RVA: 0x0034E65A File Offset: 0x0034C85A
	// (set) Token: 0x06008AE3 RID: 35555 RVA: 0x0034E662 File Offset: 0x0034C862
	[XuiXmlAttribute("duration", false)]
	public float Duration
	{
		get
		{
			return this.duration;
		}
		set
		{
			if (Mathf.Approximately(this.duration, value))
			{
				return;
			}
			this.duration = value;
			if (this.tweenGeneric != null)
			{
				this.tweenGeneric.duration = value;
			}
		}
	}

	// Token: 0x17000FEA RID: 4074
	// (get) Token: 0x06008AE4 RID: 35556 RVA: 0x0034E694 File Offset: 0x0034C894
	// (set) Token: 0x06008AE5 RID: 35557 RVA: 0x0034E69C File Offset: 0x0034C89C
	[XuiXmlAttribute("method", false)]
	public UITweener.Method Method
	{
		get
		{
			return this.method;
		}
		set
		{
			if (this.method == value)
			{
				return;
			}
			this.method = value;
			if (this.tweenGeneric != null)
			{
				this.tweenGeneric.method = value;
			}
		}
	}

	// Token: 0x17000FEB RID: 4075
	// (get) Token: 0x06008AE6 RID: 35558 RVA: 0x0034E6C9 File Offset: 0x0034C8C9
	// (set) Token: 0x06008AE7 RID: 35559 RVA: 0x0034E6D1 File Offset: 0x0034C8D1
	[XuiXmlAttribute("enabled", false)]
	public bool Enabled
	{
		get
		{
			return this.enabled;
		}
		set
		{
			if (this.enabled == value)
			{
				return;
			}
			this.enabled = value;
			if (this.tweenGeneric != null)
			{
				this.tweenGeneric.enabled = value;
			}
		}
	}

	// Token: 0x06008AE8 RID: 35560 RVA: 0x0034E6FE File Offset: 0x0034C8FE
	[XuiXmlAttribute("curve", false)]
	public void AttributeCurve(string _curveString)
	{
		this.curve = XUiTweenAbs.parseCurve(_curveString);
		if (this.tweenGeneric != null)
		{
			this.tweenGeneric.animationCurve = this.curve;
		}
	}

	// Token: 0x06008AE9 RID: 35561 RVA: 0x0034E72B File Offset: 0x0034C92B
	[XuiXmlAttribute("play_forward", false)]
	public void AttributePlayForward(bool _play)
	{
		if (!_play)
		{
			return;
		}
		this.Enabled = true;
		if (this.tweenGeneric != null)
		{
			this.tweenGeneric.PlayForward();
		}
	}

	// Token: 0x06008AEA RID: 35562 RVA: 0x0034E751 File Offset: 0x0034C951
	[XuiXmlAttribute("play_reverse", false)]
	public void AttributePlayReverse(bool _play)
	{
		if (!_play)
		{
			return;
		}
		this.Enabled = true;
		if (this.tweenGeneric != null)
		{
			this.tweenGeneric.PlayReverse();
		}
	}

	// Token: 0x06008AEB RID: 35563 RVA: 0x0034E777 File Offset: 0x0034C977
	[XuiXmlAttribute("set_to_start", false)]
	public void AttributeSetToStart(bool _set)
	{
		if (!_set)
		{
			return;
		}
		this.Enabled = false;
		if (this.tweenGeneric != null)
		{
			this.tweenGeneric.tweenFactor = 0f;
			this.tweenGeneric.Sample(0f, false);
		}
	}

	// Token: 0x06008AEC RID: 35564 RVA: 0x0034E7B3 File Offset: 0x0034C9B3
	[XuiXmlAttribute("set_to_end", false)]
	public void AttributeSetToEnd(bool _set)
	{
		if (!_set)
		{
			return;
		}
		this.Enabled = false;
		if (this.tweenGeneric != null)
		{
			this.tweenGeneric.tweenFactor = 1f;
			this.tweenGeneric.Sample(1f, false);
		}
	}

	// Token: 0x06008AED RID: 35565 RVA: 0x0034E7EF File Offset: 0x0034C9EF
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiTweenAbs(XUiView _targetView)
	{
		this.targetView = _targetView;
	}

	// Token: 0x06008AEE RID: 35566 RVA: 0x0034E81C File Offset: 0x0034CA1C
	[PublicizedFrom(EAccessModifier.Protected)]
	public void setCommonTweenValues(UITweener _tween)
	{
		this.tweenGeneric = _tween;
		_tween.style = this.repeatStyle;
		_tween.duration = this.duration;
		_tween.method = this.method;
		_tween.animationCurve = this.curve;
		_tween.enabled = this.enabled;
	}

	// Token: 0x06008AEF RID: 35567
	public abstract void CreateTween(GameObject _uiGameObject);

	// Token: 0x17000FEC RID: 4076
	// (get) Token: 0x06008AF0 RID: 35568 RVA: 0x0034E86C File Offset: 0x0034CA6C
	public XUiController Controller
	{
		get
		{
			return this.targetView.Controller;
		}
	}

	// Token: 0x06008AF1 RID: 35569 RVA: 0x0034E879 File Offset: 0x0034CA79
	public string GetXuiHierarchy()
	{
		return XUiUtils.GetXuiHierarchy(this.targetView.Controller);
	}

	// Token: 0x06008AF2 RID: 35570 RVA: 0x0034E88C File Offset: 0x0034CA8C
	public void ParseInitialAttributeValue(string _attribute, string _value)
	{
		if (_value.Contains("{"))
		{
			new BindingInfoNcalc(this, _attribute, _value);
			return;
		}
		if (ParsingMethodCache.Instance.TryParseDirect(this, _attribute, _value))
		{
			return;
		}
		Log.Error("[XUi] Unknown Tween attribute '" + _attribute + "', hierarchy: " + this.GetXuiHierarchy());
	}

	// Token: 0x17000FED RID: 4077
	// (get) Token: 0x06008AF3 RID: 35571 RVA: 0x0034E8DC File Offset: 0x0034CADC
	public static AnimationCurve SecondHalfLinear
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return new AnimationCurve(new Keyframe[]
			{
				new Keyframe(0f, 0f, 0f, 0f),
				new Keyframe(0.5f, 0f, 0f, 2f),
				new Keyframe(1f, 1f, 2f, 2f)
			});
		}
	}

	// Token: 0x06008AF4 RID: 35572 RVA: 0x0034E954 File Offset: 0x0034CB54
	[PublicizedFrom(EAccessModifier.Protected)]
	public static AnimationCurve parseCurve(string _curveString)
	{
		AnimationCurve animationCurve;
		if (!(_curveString == "EaseInOut"))
		{
			if (!(_curveString == "Linear"))
			{
				if (!(_curveString == "SecondHalfLinear"))
				{
					throw new ArgumentOutOfRangeException();
				}
				animationCurve = XUiTweenAbs.SecondHalfLinear;
			}
			else
			{
				animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
			}
		}
		else
		{
			animationCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
		}
		return animationCurve;
	}

	// Token: 0x04006687 RID: 26247
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiView targetView;

	// Token: 0x04006688 RID: 26248
	[PublicizedFrom(EAccessModifier.Private)]
	public UITweener tweenGeneric;

	// Token: 0x04006689 RID: 26249
	[PublicizedFrom(EAccessModifier.Private)]
	public UITweener.Style repeatStyle;

	// Token: 0x0400668A RID: 26250
	[PublicizedFrom(EAccessModifier.Protected)]
	public float duration = 1f;

	// Token: 0x0400668B RID: 26251
	[PublicizedFrom(EAccessModifier.Protected)]
	public UITweener.Method method;

	// Token: 0x0400668C RID: 26252
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool enabled;

	// Token: 0x0400668D RID: 26253
	[PublicizedFrom(EAccessModifier.Protected)]
	public AnimationCurve curve = XUiTweenAbs.parseCurve("Linear");

	// Token: 0x0200112C RID: 4396
	public enum ETweenType
	{
		// Token: 0x0400668F RID: 26255
		Alpha,
		// Token: 0x04006690 RID: 26256
		Color,
		// Token: 0x04006691 RID: 26257
		Fill,
		// Token: 0x04006692 RID: 26258
		Height,
		// Token: 0x04006693 RID: 26259
		Position,
		// Token: 0x04006694 RID: 26260
		Rotation,
		// Token: 0x04006695 RID: 26261
		Scale,
		// Token: 0x04006696 RID: 26262
		Width
	}
}
