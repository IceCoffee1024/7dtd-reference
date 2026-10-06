using System;
using UnityEngine;

// Token: 0x0200115C RID: 4444
public abstract class XUiTweenAbs : IXUiElement
{
	// Token: 0x17001056 RID: 4182
	// (get) Token: 0x06008C3D RID: 35901 RVA: 0x00357279 File Offset: 0x00355479
	// (set) Token: 0x06008C3E RID: 35902 RVA: 0x00357281 File Offset: 0x00355481
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

	// Token: 0x17001057 RID: 4183
	// (get) Token: 0x06008C3F RID: 35903 RVA: 0x003572AE File Offset: 0x003554AE
	// (set) Token: 0x06008C40 RID: 35904 RVA: 0x003572B6 File Offset: 0x003554B6
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

	// Token: 0x17001058 RID: 4184
	// (get) Token: 0x06008C41 RID: 35905 RVA: 0x003572E8 File Offset: 0x003554E8
	// (set) Token: 0x06008C42 RID: 35906 RVA: 0x003572F0 File Offset: 0x003554F0
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

	// Token: 0x17001059 RID: 4185
	// (get) Token: 0x06008C43 RID: 35907 RVA: 0x0035731D File Offset: 0x0035551D
	// (set) Token: 0x06008C44 RID: 35908 RVA: 0x00357325 File Offset: 0x00355525
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

	// Token: 0x06008C45 RID: 35909 RVA: 0x00357352 File Offset: 0x00355552
	[XuiXmlAttribute("curve", false)]
	public void AttributeCurve(string _curveString)
	{
		this.curve = XUiTweenAbs.parseCurve(_curveString);
		if (this.tweenGeneric != null)
		{
			this.tweenGeneric.animationCurve = this.curve;
		}
	}

	// Token: 0x06008C46 RID: 35910 RVA: 0x0035737F File Offset: 0x0035557F
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

	// Token: 0x06008C47 RID: 35911 RVA: 0x003573A5 File Offset: 0x003555A5
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

	// Token: 0x06008C48 RID: 35912 RVA: 0x003573CB File Offset: 0x003555CB
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

	// Token: 0x06008C49 RID: 35913 RVA: 0x00357407 File Offset: 0x00355607
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

	// Token: 0x06008C4A RID: 35914 RVA: 0x00357443 File Offset: 0x00355643
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiTweenAbs(XUiView _targetView)
	{
		this.targetView = _targetView;
	}

	// Token: 0x06008C4B RID: 35915 RVA: 0x00357470 File Offset: 0x00355670
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

	// Token: 0x06008C4C RID: 35916
	public abstract void CreateTween(GameObject _uiGameObject);

	// Token: 0x1700105A RID: 4186
	// (get) Token: 0x06008C4D RID: 35917 RVA: 0x003574C0 File Offset: 0x003556C0
	public XUiController Controller
	{
		get
		{
			return this.targetView.Controller;
		}
	}

	// Token: 0x06008C4E RID: 35918 RVA: 0x003574CD File Offset: 0x003556CD
	public string GetXuiHierarchy()
	{
		return XUiUtils.GetXuiHierarchy(this.targetView.Controller);
	}

	// Token: 0x06008C4F RID: 35919 RVA: 0x003574E0 File Offset: 0x003556E0
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

	// Token: 0x1700105B RID: 4187
	// (get) Token: 0x06008C50 RID: 35920 RVA: 0x00357530 File Offset: 0x00355730
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

	// Token: 0x06008C51 RID: 35921 RVA: 0x003575A8 File Offset: 0x003557A8
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

	// Token: 0x040067A6 RID: 26534
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiView targetView;

	// Token: 0x040067A7 RID: 26535
	[PublicizedFrom(EAccessModifier.Private)]
	public UITweener tweenGeneric;

	// Token: 0x040067A8 RID: 26536
	[PublicizedFrom(EAccessModifier.Private)]
	public UITweener.Style repeatStyle;

	// Token: 0x040067A9 RID: 26537
	[PublicizedFrom(EAccessModifier.Protected)]
	public float duration = 1f;

	// Token: 0x040067AA RID: 26538
	[PublicizedFrom(EAccessModifier.Protected)]
	public UITweener.Method method;

	// Token: 0x040067AB RID: 26539
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool enabled;

	// Token: 0x040067AC RID: 26540
	[PublicizedFrom(EAccessModifier.Protected)]
	public AnimationCurve curve = XUiTweenAbs.parseCurve("Linear");

	// Token: 0x0200115D RID: 4445
	public enum ETweenType
	{
		// Token: 0x040067AE RID: 26542
		Alpha,
		// Token: 0x040067AF RID: 26543
		Color,
		// Token: 0x040067B0 RID: 26544
		Fill,
		// Token: 0x040067B1 RID: 26545
		Height,
		// Token: 0x040067B2 RID: 26546
		Position,
		// Token: 0x040067B3 RID: 26547
		Rotation,
		// Token: 0x040067B4 RID: 26548
		Scale,
		// Token: 0x040067B5 RID: 26549
		Width
	}
}
