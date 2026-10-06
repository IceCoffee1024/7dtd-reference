using System;
using UnityEngine;

// Token: 0x0200115E RID: 4446
public class XUiTweenAlpha : XUiTweenAbs
{
	// Token: 0x1700105C RID: 4188
	// (get) Token: 0x06008C52 RID: 35922 RVA: 0x00357625 File Offset: 0x00355825
	// (set) Token: 0x06008C53 RID: 35923 RVA: 0x0035762D File Offset: 0x0035582D
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

	// Token: 0x1700105D RID: 4189
	// (get) Token: 0x06008C54 RID: 35924 RVA: 0x0035765A File Offset: 0x0035585A
	// (set) Token: 0x06008C55 RID: 35925 RVA: 0x00357662 File Offset: 0x00355862
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

	// Token: 0x06008C56 RID: 35926 RVA: 0x00357690 File Offset: 0x00355890
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

	// Token: 0x06008C57 RID: 35927 RVA: 0x003576FF File Offset: 0x003558FF
	public XUiTweenAlpha(XUiView _targetView)
		: base(_targetView)
	{
	}

	// Token: 0x06008C58 RID: 35928 RVA: 0x00357713 File Offset: 0x00355913
	public override void CreateTween(GameObject _uiGameObject)
	{
		this.tween = _uiGameObject.AddMissingComponent<TweenAlpha>();
		this.tween.from = this.start;
		this.tween.to = this.end;
		base.setCommonTweenValues(this.tween);
	}

	// Token: 0x040067B6 RID: 26550
	[PublicizedFrom(EAccessModifier.Private)]
	public TweenAlpha tween;

	// Token: 0x040067B7 RID: 26551
	[PublicizedFrom(EAccessModifier.Private)]
	public float start;

	// Token: 0x040067B8 RID: 26552
	[PublicizedFrom(EAccessModifier.Private)]
	public float end = 1f;
}
