using System;
using UnityEngine;

// Token: 0x02001164 RID: 4452
public class XUiTweenScale : XUiTweenAbs
{
	// Token: 0x17001068 RID: 4200
	// (get) Token: 0x06008C7C RID: 35964 RVA: 0x00357D88 File Offset: 0x00355F88
	// (set) Token: 0x06008C7D RID: 35965 RVA: 0x00357D90 File Offset: 0x00355F90
	[XuiXmlAttribute("start", false)]
	public Vector3 Start
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

	// Token: 0x17001069 RID: 4201
	// (get) Token: 0x06008C7E RID: 35966 RVA: 0x00357DB8 File Offset: 0x00355FB8
	// (set) Token: 0x06008C7F RID: 35967 RVA: 0x00357DC0 File Offset: 0x00355FC0
	[XuiXmlAttribute("end", false)]
	public Vector3 End
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

	// Token: 0x06008C80 RID: 35968 RVA: 0x00357DE8 File Offset: 0x00355FE8
	[XuiXmlAttribute("play_to", false)]
	public void AttributePlayToEnd(Vector3 _value)
	{
		if (this.tween == null)
		{
			return;
		}
		if ((double)(this.tween.to - _value).sqrMagnitude < 0.1)
		{
			return;
		}
		base.Enabled = true;
		this.tween.tweenFactor = 0f;
		this.tween.SetStartToCurrentValue();
		this.End = _value;
		this.tween.PlayForward();
	}

	// Token: 0x06008C81 RID: 35969 RVA: 0x00357E5E File Offset: 0x0035605E
	public XUiTweenScale(XUiView _targetView)
		: base(_targetView)
	{
	}

	// Token: 0x06008C82 RID: 35970 RVA: 0x00357E8C File Offset: 0x0035608C
	public override void CreateTween(GameObject _uiGameObject)
	{
		this.tween = _uiGameObject.AddMissingComponent<TweenScale>();
		this.tween.from = this.start;
		this.tween.to = this.end;
		base.setCommonTweenValues(this.tween);
	}

	// Token: 0x040067C8 RID: 26568
	[PublicizedFrom(EAccessModifier.Private)]
	public TweenScale tween;

	// Token: 0x040067C9 RID: 26569
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 start = Vector3.one;

	// Token: 0x040067CA RID: 26570
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 end = new Vector3(1.25f, 1.25f, 1f);
}
