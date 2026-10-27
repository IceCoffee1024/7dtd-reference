using System;
using UnityEngine;

// Token: 0x02001175 RID: 4469
public class XUiTweenWidth : XUiTweenAbs
{
	// Token: 0x17001075 RID: 4213
	// (get) Token: 0x06008CD7 RID: 36055 RVA: 0x00359F74 File Offset: 0x00358174
	// (set) Token: 0x06008CD8 RID: 36056 RVA: 0x00359F7C File Offset: 0x0035817C
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

	// Token: 0x17001076 RID: 4214
	// (get) Token: 0x06008CD9 RID: 36057 RVA: 0x00359FA4 File Offset: 0x003581A4
	// (set) Token: 0x06008CDA RID: 36058 RVA: 0x00359FAC File Offset: 0x003581AC
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

	// Token: 0x06008CDB RID: 36059 RVA: 0x00359FD4 File Offset: 0x003581D4
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

	// Token: 0x06008CDC RID: 36060 RVA: 0x0035A03A File Offset: 0x0035823A
	public XUiTweenWidth(XUiView _targetView)
		: base(_targetView)
	{
	}

	// Token: 0x06008CDD RID: 36061 RVA: 0x0035A053 File Offset: 0x00358253
	public override void CreateTween(GameObject _uiGameObject)
	{
		this.tween = _uiGameObject.AddMissingComponent<TweenWidth>();
		this.tween.from = this.start;
		this.tween.to = this.end;
		base.setCommonTweenValues(this.tween);
	}

	// Token: 0x0400680B RID: 26635
	[PublicizedFrom(EAccessModifier.Private)]
	public TweenWidth tween;

	// Token: 0x0400680C RID: 26636
	[PublicizedFrom(EAccessModifier.Private)]
	public int start = 20;

	// Token: 0x0400680D RID: 26637
	[PublicizedFrom(EAccessModifier.Private)]
	public int end = 40;
}
