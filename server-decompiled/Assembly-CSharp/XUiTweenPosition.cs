using System;
using UnityEngine;

// Token: 0x02001172 RID: 4466
public class XUiTweenPosition : XUiTweenAbs
{
	// Token: 0x1700106F RID: 4207
	// (get) Token: 0x06008CC2 RID: 36034 RVA: 0x00359BB3 File Offset: 0x00357DB3
	// (set) Token: 0x06008CC3 RID: 36035 RVA: 0x00359BBB File Offset: 0x00357DBB
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

	// Token: 0x17001070 RID: 4208
	// (get) Token: 0x06008CC4 RID: 36036 RVA: 0x00359BE3 File Offset: 0x00357DE3
	// (set) Token: 0x06008CC5 RID: 36037 RVA: 0x00359BEB File Offset: 0x00357DEB
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

	// Token: 0x06008CC6 RID: 36038 RVA: 0x00359C14 File Offset: 0x00357E14
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

	// Token: 0x06008CC7 RID: 36039 RVA: 0x00359C8A File Offset: 0x00357E8A
	public XUiTweenPosition(XUiView _targetView)
		: base(_targetView)
	{
	}

	// Token: 0x06008CC8 RID: 36040 RVA: 0x00359CB8 File Offset: 0x00357EB8
	public override void CreateTween(GameObject _uiGameObject)
	{
		this.tween = _uiGameObject.AddMissingComponent<TweenPosition>();
		this.tween.from = this.start;
		this.tween.to = this.end;
		base.setCommonTweenValues(this.tween);
	}

	// Token: 0x04006802 RID: 26626
	[PublicizedFrom(EAccessModifier.Private)]
	public TweenPosition tween;

	// Token: 0x04006803 RID: 26627
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 start = Vector3.zero;

	// Token: 0x04006804 RID: 26628
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 end = new Vector3(0f, 8f, 0f);
}
