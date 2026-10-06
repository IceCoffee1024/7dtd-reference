using System;
using UnityEngine;

// Token: 0x02001163 RID: 4451
public class XUiTweenRotation : XUiTweenAbs
{
	// Token: 0x17001066 RID: 4198
	// (get) Token: 0x06008C75 RID: 35957 RVA: 0x00357C48 File Offset: 0x00355E48
	// (set) Token: 0x06008C76 RID: 35958 RVA: 0x00357C50 File Offset: 0x00355E50
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

	// Token: 0x17001067 RID: 4199
	// (get) Token: 0x06008C77 RID: 35959 RVA: 0x00357C78 File Offset: 0x00355E78
	// (set) Token: 0x06008C78 RID: 35960 RVA: 0x00357C80 File Offset: 0x00355E80
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

	// Token: 0x06008C79 RID: 35961 RVA: 0x00357CA8 File Offset: 0x00355EA8
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

	// Token: 0x06008C7A RID: 35962 RVA: 0x00357D1E File Offset: 0x00355F1E
	public XUiTweenRotation(XUiView _targetView)
		: base(_targetView)
	{
	}

	// Token: 0x06008C7B RID: 35963 RVA: 0x00357D4C File Offset: 0x00355F4C
	public override void CreateTween(GameObject _uiGameObject)
	{
		this.tween = _uiGameObject.AddMissingComponent<TweenRotation>();
		this.tween.from = this.start;
		this.tween.to = this.end;
		base.setCommonTweenValues(this.tween);
	}

	// Token: 0x040067C5 RID: 26565
	[PublicizedFrom(EAccessModifier.Private)]
	public TweenRotation tween;

	// Token: 0x040067C6 RID: 26566
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 start = Vector3.zero;

	// Token: 0x040067C7 RID: 26567
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 end = new Vector3(0f, 0f, 360f);
}
