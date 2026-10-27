using System;
using UnityEngine;

// Token: 0x0200116E RID: 4462
public class XUiTweenAlpha : XUiTweenAbs
{
	// Token: 0x17001067 RID: 4199
	// (get) Token: 0x06008CA6 RID: 36006 RVA: 0x003596D1 File Offset: 0x003578D1
	// (set) Token: 0x06008CA7 RID: 36007 RVA: 0x003596D9 File Offset: 0x003578D9
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

	// Token: 0x17001068 RID: 4200
	// (get) Token: 0x06008CA8 RID: 36008 RVA: 0x00359706 File Offset: 0x00357906
	// (set) Token: 0x06008CA9 RID: 36009 RVA: 0x0035970E File Offset: 0x0035790E
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

	// Token: 0x06008CAA RID: 36010 RVA: 0x0035973C File Offset: 0x0035793C
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

	// Token: 0x06008CAB RID: 36011 RVA: 0x003597AB File Offset: 0x003579AB
	public XUiTweenAlpha(XUiView _targetView)
		: base(_targetView)
	{
	}

	// Token: 0x06008CAC RID: 36012 RVA: 0x003597BF File Offset: 0x003579BF
	public override void CreateTween(GameObject _uiGameObject)
	{
		this.tween = _uiGameObject.AddMissingComponent<TweenAlpha>();
		this.tween.from = this.start;
		this.tween.to = this.end;
		base.setCommonTweenValues(this.tween);
	}

	// Token: 0x040067F6 RID: 26614
	[PublicizedFrom(EAccessModifier.Private)]
	public TweenAlpha tween;

	// Token: 0x040067F7 RID: 26615
	[PublicizedFrom(EAccessModifier.Private)]
	public float start;

	// Token: 0x040067F8 RID: 26616
	[PublicizedFrom(EAccessModifier.Private)]
	public float end = 1f;
}
