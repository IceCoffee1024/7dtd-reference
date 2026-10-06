using System;
using UnityEngine;

// Token: 0x02001160 RID: 4448
public class XUiTweenFill : XUiTweenAbs
{
	// Token: 0x17001060 RID: 4192
	// (get) Token: 0x06008C60 RID: 35936 RVA: 0x003578BF File Offset: 0x00355ABF
	// (set) Token: 0x06008C61 RID: 35937 RVA: 0x003578C7 File Offset: 0x00355AC7
	[XuiXmlAttribute("start", false)]
	public float Start
	{
		get
		{
			return this.start;
		}
		set
		{
			this.start = Mathf.Clamp01(value);
			if (this.tween != null)
			{
				this.tween.from = this.start;
			}
		}
	}

	// Token: 0x17001061 RID: 4193
	// (get) Token: 0x06008C62 RID: 35938 RVA: 0x003578F4 File Offset: 0x00355AF4
	// (set) Token: 0x06008C63 RID: 35939 RVA: 0x003578FC File Offset: 0x00355AFC
	[XuiXmlAttribute("end", false)]
	public float End
	{
		get
		{
			return this.end;
		}
		set
		{
			this.end = Mathf.Clamp01(value);
			if (this.tween != null)
			{
				this.tween.to = this.end;
			}
		}
	}

	// Token: 0x06008C64 RID: 35940 RVA: 0x0035792C File Offset: 0x00355B2C
	[XuiXmlAttribute("play_to", false)]
	public void AttributePlayToEnd(float _value)
	{
		if (this.tween == null)
		{
			return;
		}
		if ((double)Mathf.Abs(this.tween.to - _value) < 0.0001)
		{
			return;
		}
		base.Enabled = true;
		this.tween.tweenFactor = 0f;
		this.tween.SetStartToCurrentValue();
		this.End = _value;
		this.tween.PlayForward();
	}

	// Token: 0x06008C65 RID: 35941 RVA: 0x0035799B File Offset: 0x00355B9B
	public XUiTweenFill(XUiView _targetView)
		: base(_targetView)
	{
	}

	// Token: 0x06008C66 RID: 35942 RVA: 0x003579AF File Offset: 0x00355BAF
	public override void CreateTween(GameObject _uiGameObject)
	{
		this.tween = _uiGameObject.AddMissingComponent<TweenFill>();
		this.tween.from = this.start;
		this.tween.to = this.end;
		base.setCommonTweenValues(this.tween);
	}

	// Token: 0x040067BC RID: 26556
	[PublicizedFrom(EAccessModifier.Private)]
	public TweenFill tween;

	// Token: 0x040067BD RID: 26557
	[PublicizedFrom(EAccessModifier.Private)]
	public float start;

	// Token: 0x040067BE RID: 26558
	[PublicizedFrom(EAccessModifier.Private)]
	public float end = 1f;
}
