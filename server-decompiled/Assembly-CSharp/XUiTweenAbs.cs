using System;
using UnityEngine;

// Token: 0x0200116C RID: 4460
public abstract class XUiTweenAbs : IXUiElement
{
	// Token: 0x17001061 RID: 4193
	// (get) Token: 0x06008C91 RID: 35985 RVA: 0x00359325 File Offset: 0x00357525
	// (set) Token: 0x06008C92 RID: 35986 RVA: 0x0035932D File Offset: 0x0035752D
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

	// Token: 0x17001062 RID: 4194
	// (get) Token: 0x06008C93 RID: 35987 RVA: 0x0035935A File Offset: 0x0035755A
	// (set) Token: 0x06008C94 RID: 35988 RVA: 0x00359362 File Offset: 0x00357562
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

	// Token: 0x17001063 RID: 4195
	// (get) Token: 0x06008C95 RID: 35989 RVA: 0x00359394 File Offset: 0x00357594
	// (set) Token: 0x06008C96 RID: 35990 RVA: 0x0035939C File Offset: 0x0035759C
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

	// Token: 0x17001064 RID: 4196
	// (get) Token: 0x06008C97 RID: 35991 RVA: 0x003593C9 File Offset: 0x003575C9
	// (set) Token: 0x06008C98 RID: 35992 RVA: 0x003593D1 File Offset: 0x003575D1
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

	// Token: 0x06008C99 RID: 35993 RVA: 0x003593FE File Offset: 0x003575FE
	[XuiXmlAttribute("curve", false)]
	public void AttributeCurve(string _curveString)
	{
		this.curve = XUiTweenAbs.parseCurve(_curveString);
		if (this.tweenGeneric != null)
		{
			this.tweenGeneric.animationCurve = this.curve;
		}
	}

	// Token: 0x06008C9A RID: 35994 RVA: 0x0035942B File Offset: 0x0035762B
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

	// Token: 0x06008C9B RID: 35995 RVA: 0x00359451 File Offset: 0x00357651
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

	// Token: 0x06008C9C RID: 35996 RVA: 0x00359477 File Offset: 0x00357677
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

	// Token: 0x06008C9D RID: 35997 RVA: 0x003594B3 File Offset: 0x003576B3
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

	// Token: 0x06008C9E RID: 35998 RVA: 0x003594EF File Offset: 0x003576EF
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiTweenAbs(XUiView _targetView)
	{
		this.targetView = _targetView;
	}

	// Token: 0x06008C9F RID: 35999 RVA: 0x0035951C File Offset: 0x0035771C
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

	// Token: 0x06008CA0 RID: 36000
	public abstract void CreateTween(GameObject _uiGameObject);

	// Token: 0x17001065 RID: 4197
	// (get) Token: 0x06008CA1 RID: 36001 RVA: 0x0035956C File Offset: 0x0035776C
	public XUiController Controller
	{
		get
		{
			return this.targetView.Controller;
		}
	}

	// Token: 0x06008CA2 RID: 36002 RVA: 0x00359579 File Offset: 0x00357779
	public string GetXuiHierarchy()
	{
		return XUiUtils.GetXuiHierarchy(this.targetView.Controller);
	}

	// Token: 0x06008CA3 RID: 36003 RVA: 0x0035958C File Offset: 0x0035778C
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

	// Token: 0x17001066 RID: 4198
	// (get) Token: 0x06008CA4 RID: 36004 RVA: 0x003595DC File Offset: 0x003577DC
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

	// Token: 0x06008CA5 RID: 36005 RVA: 0x00359654 File Offset: 0x00357854
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

	// Token: 0x040067E6 RID: 26598
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiView targetView;

	// Token: 0x040067E7 RID: 26599
	[PublicizedFrom(EAccessModifier.Private)]
	public UITweener tweenGeneric;

	// Token: 0x040067E8 RID: 26600
	[PublicizedFrom(EAccessModifier.Private)]
	public UITweener.Style repeatStyle;

	// Token: 0x040067E9 RID: 26601
	[PublicizedFrom(EAccessModifier.Protected)]
	public float duration = 1f;

	// Token: 0x040067EA RID: 26602
	[PublicizedFrom(EAccessModifier.Protected)]
	public UITweener.Method method;

	// Token: 0x040067EB RID: 26603
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool enabled;

	// Token: 0x040067EC RID: 26604
	[PublicizedFrom(EAccessModifier.Protected)]
	public AnimationCurve curve = XUiTweenAbs.parseCurve("Linear");

	// Token: 0x0200116D RID: 4461
	public enum ETweenType
	{
		// Token: 0x040067EE RID: 26606
		Alpha,
		// Token: 0x040067EF RID: 26607
		Color,
		// Token: 0x040067F0 RID: 26608
		Fill,
		// Token: 0x040067F1 RID: 26609
		Height,
		// Token: 0x040067F2 RID: 26610
		Position,
		// Token: 0x040067F3 RID: 26611
		Rotation,
		// Token: 0x040067F4 RID: 26612
		Scale,
		// Token: 0x040067F5 RID: 26613
		Width
	}
}
