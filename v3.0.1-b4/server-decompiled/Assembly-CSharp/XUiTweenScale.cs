using System;
using UnityEngine;

// Token: 0x02001133 RID: 4403
public class XUiTweenScale : XUiTweenAbs
{
	// Token: 0x17000FFA RID: 4090
	// (get) Token: 0x06008B1F RID: 35615 RVA: 0x0034F134 File Offset: 0x0034D334
	// (set) Token: 0x06008B20 RID: 35616 RVA: 0x0034F13C File Offset: 0x0034D33C
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

	// Token: 0x17000FFB RID: 4091
	// (get) Token: 0x06008B21 RID: 35617 RVA: 0x0034F164 File Offset: 0x0034D364
	// (set) Token: 0x06008B22 RID: 35618 RVA: 0x0034F16C File Offset: 0x0034D36C
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

	// Token: 0x06008B23 RID: 35619 RVA: 0x0034F194 File Offset: 0x0034D394
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

	// Token: 0x06008B24 RID: 35620 RVA: 0x0034F20A File Offset: 0x0034D40A
	public XUiTweenScale(XUiView _targetView)
		: base(_targetView)
	{
	}

	// Token: 0x06008B25 RID: 35621 RVA: 0x0034F238 File Offset: 0x0034D438
	public override void CreateTween(GameObject _uiGameObject)
	{
		this.tween = _uiGameObject.AddMissingComponent<TweenScale>();
		this.tween.from = this.start;
		this.tween.to = this.end;
		base.setCommonTweenValues(this.tween);
	}

	// Token: 0x040066A9 RID: 26281
	[PublicizedFrom(EAccessModifier.Private)]
	public TweenScale tween;

	// Token: 0x040066AA RID: 26282
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 start = Vector3.one;

	// Token: 0x040066AB RID: 26283
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 end = new Vector3(1.25f, 1.25f, 1f);
}
