using System;
using UnityEngine;

// Token: 0x02001131 RID: 4401
public class XUiTweenPosition : XUiTweenAbs
{
	// Token: 0x17000FF6 RID: 4086
	// (get) Token: 0x06008B11 RID: 35601 RVA: 0x0034EEB3 File Offset: 0x0034D0B3
	// (set) Token: 0x06008B12 RID: 35602 RVA: 0x0034EEBB File Offset: 0x0034D0BB
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

	// Token: 0x17000FF7 RID: 4087
	// (get) Token: 0x06008B13 RID: 35603 RVA: 0x0034EEE3 File Offset: 0x0034D0E3
	// (set) Token: 0x06008B14 RID: 35604 RVA: 0x0034EEEB File Offset: 0x0034D0EB
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

	// Token: 0x06008B15 RID: 35605 RVA: 0x0034EF14 File Offset: 0x0034D114
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

	// Token: 0x06008B16 RID: 35606 RVA: 0x0034EF8A File Offset: 0x0034D18A
	public XUiTweenPosition(XUiView _targetView)
		: base(_targetView)
	{
	}

	// Token: 0x06008B17 RID: 35607 RVA: 0x0034EFB8 File Offset: 0x0034D1B8
	public override void CreateTween(GameObject _uiGameObject)
	{
		this.tween = _uiGameObject.AddMissingComponent<TweenPosition>();
		this.tween.from = this.start;
		this.tween.to = this.end;
		base.setCommonTweenValues(this.tween);
	}

	// Token: 0x040066A3 RID: 26275
	[PublicizedFrom(EAccessModifier.Private)]
	public TweenPosition tween;

	// Token: 0x040066A4 RID: 26276
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 start = Vector3.zero;

	// Token: 0x040066A5 RID: 26277
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 end = new Vector3(0f, 8f, 0f);
}
