using System;
using UnityEngine;

// Token: 0x02001161 RID: 4449
public class XUiTweenHeight : XUiTweenAbs
{
	// Token: 0x17001062 RID: 4194
	// (get) Token: 0x06008C67 RID: 35943 RVA: 0x003579EB File Offset: 0x00355BEB
	// (set) Token: 0x06008C68 RID: 35944 RVA: 0x003579F3 File Offset: 0x00355BF3
	[XuiXmlAttribute("start", false)]
	public int Start
	{
		get
		{
			return this.start;
		}
		set
		{
			this.start = value;
			if (this.tween != null)
			{
				this.tween.from = this.start;
			}
		}
	}

	// Token: 0x17001063 RID: 4195
	// (get) Token: 0x06008C69 RID: 35945 RVA: 0x00357A1B File Offset: 0x00355C1B
	// (set) Token: 0x06008C6A RID: 35946 RVA: 0x00357A23 File Offset: 0x00355C23
	[XuiXmlAttribute("end", false)]
	public int End
	{
		get
		{
			return this.end;
		}
		set
		{
			this.end = value;
			if (this.tween != null)
			{
				this.tween.to = this.end;
			}
		}
	}

	// Token: 0x06008C6B RID: 35947 RVA: 0x00357A4C File Offset: 0x00355C4C
	[XuiXmlAttribute("play_to", false)]
	public void AttributePlayToEnd(int _value)
	{
		if (this.tween == null)
		{
			return;
		}
		if (Mathf.Abs(this.tween.to - _value) < 1)
		{
			return;
		}
		base.Enabled = true;
		this.tween.tweenFactor = 0f;
		this.tween.SetStartToCurrentValue();
		this.End = _value;
		this.tween.PlayForward();
	}

	// Token: 0x06008C6C RID: 35948 RVA: 0x00357AB2 File Offset: 0x00355CB2
	public XUiTweenHeight(XUiView _targetView)
		: base(_targetView)
	{
	}

	// Token: 0x06008C6D RID: 35949 RVA: 0x00357ACB File Offset: 0x00355CCB
	public override void CreateTween(GameObject _uiGameObject)
	{
		this.tween = _uiGameObject.AddMissingComponent<TweenHeight>();
		this.tween.from = this.start;
		this.tween.to = this.end;
		base.setCommonTweenValues(this.tween);
	}

	// Token: 0x040067BF RID: 26559
	[PublicizedFrom(EAccessModifier.Private)]
	public TweenHeight tween;

	// Token: 0x040067C0 RID: 26560
	[PublicizedFrom(EAccessModifier.Private)]
	public int start = 20;

	// Token: 0x040067C1 RID: 26561
	[PublicizedFrom(EAccessModifier.Private)]
	public int end = 40;
}
