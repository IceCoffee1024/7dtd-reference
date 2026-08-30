using System;
using UnityEngine;

// Token: 0x02001132 RID: 4402
public class XUiTweenRotation : XUiTweenAbs
{
	// Token: 0x17000FF8 RID: 4088
	// (get) Token: 0x06008B18 RID: 35608 RVA: 0x0034EFF4 File Offset: 0x0034D1F4
	// (set) Token: 0x06008B19 RID: 35609 RVA: 0x0034EFFC File Offset: 0x0034D1FC
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

	// Token: 0x17000FF9 RID: 4089
	// (get) Token: 0x06008B1A RID: 35610 RVA: 0x0034F024 File Offset: 0x0034D224
	// (set) Token: 0x06008B1B RID: 35611 RVA: 0x0034F02C File Offset: 0x0034D22C
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

	// Token: 0x06008B1C RID: 35612 RVA: 0x0034F054 File Offset: 0x0034D254
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

	// Token: 0x06008B1D RID: 35613 RVA: 0x0034F0CA File Offset: 0x0034D2CA
	public XUiTweenRotation(XUiView _targetView)
		: base(_targetView)
	{
	}

	// Token: 0x06008B1E RID: 35614 RVA: 0x0034F0F8 File Offset: 0x0034D2F8
	public override void CreateTween(GameObject _uiGameObject)
	{
		this.tween = _uiGameObject.AddMissingComponent<TweenRotation>();
		this.tween.from = this.start;
		this.tween.to = this.end;
		base.setCommonTweenValues(this.tween);
	}

	// Token: 0x040066A6 RID: 26278
	[PublicizedFrom(EAccessModifier.Private)]
	public TweenRotation tween;

	// Token: 0x040066A7 RID: 26279
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 start = Vector3.zero;

	// Token: 0x040066A8 RID: 26280
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 end = new Vector3(0f, 0f, 360f);
}
