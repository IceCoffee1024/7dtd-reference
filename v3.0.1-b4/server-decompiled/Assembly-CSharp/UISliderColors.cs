using System;
using UnityEngine;

// Token: 0x0200005B RID: 91
[AddComponentMenu("NGUI/Examples/Slider Colors")]
public class UISliderColors : MonoBehaviour
{
	// Token: 0x060001CD RID: 461 RVA: 0x000109E7 File Offset: 0x0000EBE7
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.mBar = base.GetComponent<UIProgressBar>();
		this.mSprite = base.GetComponent<UIBasicSprite>();
		this.Update();
	}

	// Token: 0x060001CE RID: 462 RVA: 0x00010A08 File Offset: 0x0000EC08
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (this.sprite == null || this.colors.Length == 0)
		{
			return;
		}
		float num = ((this.mBar != null) ? this.mBar.value : this.mSprite.fillAmount);
		num *= (float)(this.colors.Length - 1);
		int num2 = Mathf.FloorToInt(num);
		Color color = this.colors[0];
		if (num2 >= 0)
		{
			if (num2 + 1 < this.colors.Length)
			{
				float num3 = num - (float)num2;
				color = Color.Lerp(this.colors[num2], this.colors[num2 + 1], num3);
			}
			else if (num2 < this.colors.Length)
			{
				color = this.colors[num2];
			}
			else
			{
				color = this.colors[this.colors.Length - 1];
			}
		}
		color.a = this.sprite.color.a;
		this.sprite.color = color;
	}

	// Token: 0x04000276 RID: 630
	public UISprite sprite;

	// Token: 0x04000277 RID: 631
	public Color[] colors = new Color[]
	{
		Color.red,
		Color.yellow,
		Color.green
	};

	// Token: 0x04000278 RID: 632
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public UIProgressBar mBar;

	// Token: 0x04000279 RID: 633
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public UIBasicSprite mSprite;
}
