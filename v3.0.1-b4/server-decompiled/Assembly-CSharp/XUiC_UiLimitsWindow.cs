using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E36 RID: 3638
[Preserve]
public class XUiC_UiLimitsWindow : XUiController
{
	// Token: 0x06006FF6 RID: 28662 RVA: 0x0028AD37 File Offset: 0x00288F37
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x06006FF7 RID: 28663 RVA: 0x002B1B28 File Offset: 0x002AFD28
	public override void OnOpen()
	{
		base.OnOpen();
		int manualHeight = global::UnityEngine.Object.FindAnyObjectByType<UIRoot>().manualHeight;
		float scale = this.xui.GetScale();
		this.availableXuiHeight = (float)manualHeight / scale;
		base.RefreshBindings();
	}

	// Token: 0x17000C9D RID: 3229
	// (get) Token: 0x06006FF8 RID: 28664 RVA: 0x002B1B62 File Offset: 0x002AFD62
	[XuiXmlBinding("height")]
	public int Height
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return Mathf.FloorToInt(this.availableXuiHeight);
		}
	}

	// Token: 0x17000C9E RID: 3230
	// (get) Token: 0x06006FF9 RID: 28665 RVA: 0x002B1B6F File Offset: 0x002AFD6F
	[XuiXmlBinding("width_5_4")]
	public int Width5x4
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.calcArWidth(5, 4);
		}
	}

	// Token: 0x17000C9F RID: 3231
	// (get) Token: 0x06006FFA RID: 28666 RVA: 0x002B1B79 File Offset: 0x002AFD79
	[XuiXmlBinding("width_4_3")]
	public int Width4x3
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.calcArWidth(4, 3);
		}
	}

	// Token: 0x17000CA0 RID: 3232
	// (get) Token: 0x06006FFB RID: 28667 RVA: 0x002B1B83 File Offset: 0x002AFD83
	[XuiXmlBinding("width_3_2")]
	public int Width3x2
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.calcArWidth(3, 2);
		}
	}

	// Token: 0x17000CA1 RID: 3233
	// (get) Token: 0x06006FFC RID: 28668 RVA: 0x002B1B8D File Offset: 0x002AFD8D
	[XuiXmlBinding("width_16_10")]
	public int Width16x10
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.calcArWidth(16, 10);
		}
	}

	// Token: 0x17000CA2 RID: 3234
	// (get) Token: 0x06006FFD RID: 28669 RVA: 0x002B1B99 File Offset: 0x002AFD99
	[XuiXmlBinding("width_16_9")]
	public int Width16x9
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.calcArWidth(16, 9);
		}
	}

	// Token: 0x06006FFE RID: 28670 RVA: 0x002B1BA8 File Offset: 0x002AFDA8
	[PublicizedFrom(EAccessModifier.Private)]
	public int calcArWidth(int _ratioWidth, int _ratioHeight)
	{
		double uiSizeLimit = GameOptionsManager.GetUiSizeLimit((double)_ratioWidth / (double)_ratioHeight);
		int num = Mathf.RoundToInt((float)((double)(this.availableXuiHeight / (float)_ratioHeight * (float)_ratioWidth) / uiSizeLimit));
		if (num % 2 > 0)
		{
			num--;
		}
		return num;
	}

	// Token: 0x04005467 RID: 21607
	[PublicizedFrom(EAccessModifier.Private)]
	public float availableXuiHeight = 1f;
}
