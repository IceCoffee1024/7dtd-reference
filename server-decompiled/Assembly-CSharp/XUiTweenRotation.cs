using System;
using UnityEngine;

// Token: 0x02001173 RID: 4467
public class XUiTweenRotation : XUiTweenAbs
{
	// Token: 0x17001071 RID: 4209
	// (get) Token: 0x06008CC9 RID: 36041 RVA: 0x00359CF4 File Offset: 0x00357EF4
	// (set) Token: 0x06008CCA RID: 36042 RVA: 0x00359CFC File Offset: 0x00357EFC
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

	// Token: 0x17001072 RID: 4210
	// (get) Token: 0x06008CCB RID: 36043 RVA: 0x00359D24 File Offset: 0x00357F24
	// (set) Token: 0x06008CCC RID: 36044 RVA: 0x00359D2C File Offset: 0x00357F2C
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

	// Token: 0x06008CCD RID: 36045 RVA: 0x00359D54 File Offset: 0x00357F54
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

	// Token: 0x06008CCE RID: 36046 RVA: 0x00359DCA File Offset: 0x00357FCA
	public XUiTweenRotation(XUiView _targetView)
		: base(_targetView)
	{
	}

	// Token: 0x06008CCF RID: 36047 RVA: 0x00359DF8 File Offset: 0x00357FF8
	public override void CreateTween(GameObject _uiGameObject)
	{
		this.tween = _uiGameObject.AddMissingComponent<TweenRotation>();
		this.tween.from = this.start;
		this.tween.to = this.end;
		base.setCommonTweenValues(this.tween);
	}

	// Token: 0x04006805 RID: 26629
	[PublicizedFrom(EAccessModifier.Private)]
	public TweenRotation tween;

	// Token: 0x04006806 RID: 26630
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 start = Vector3.zero;

	// Token: 0x04006807 RID: 26631
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 end = new Vector3(0f, 0f, 360f);
}
