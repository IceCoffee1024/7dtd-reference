using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E4B RID: 3659
[Preserve]
public class XUiC_UiLimitsWindow : XUiController
{
	// Token: 0x06007096 RID: 28822 RVA: 0x0028DC1B File Offset: 0x0028BE1B
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x06007097 RID: 28823 RVA: 0x002B504C File Offset: 0x002B324C
	public override void OnOpen()
	{
		base.OnOpen();
		int manualHeight = global::UnityEngine.Object.FindAnyObjectByType<UIRoot>().manualHeight;
		float scale = this.xui.GetScale();
		this.availableXuiHeight = (float)manualHeight / scale;
		base.RefreshBindings();
	}

	// Token: 0x17000CB6 RID: 3254
	// (get) Token: 0x06007098 RID: 28824 RVA: 0x002B5086 File Offset: 0x002B3286
	[XuiXmlBinding("height")]
	public int Height
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return Mathf.FloorToInt(this.availableXuiHeight);
		}
	}

	// Token: 0x17000CB7 RID: 3255
	// (get) Token: 0x06007099 RID: 28825 RVA: 0x002B5093 File Offset: 0x002B3293
	[XuiXmlBinding("width_5_4")]
	public int Width5x4
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.calcArWidth(5, 4);
		}
	}

	// Token: 0x17000CB8 RID: 3256
	// (get) Token: 0x0600709A RID: 28826 RVA: 0x002B509D File Offset: 0x002B329D
	[XuiXmlBinding("width_4_3")]
	public int Width4x3
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.calcArWidth(4, 3);
		}
	}

	// Token: 0x17000CB9 RID: 3257
	// (get) Token: 0x0600709B RID: 28827 RVA: 0x002B50A7 File Offset: 0x002B32A7
	[XuiXmlBinding("width_3_2")]
	public int Width3x2
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.calcArWidth(3, 2);
		}
	}

	// Token: 0x17000CBA RID: 3258
	// (get) Token: 0x0600709C RID: 28828 RVA: 0x002B50B1 File Offset: 0x002B32B1
	[XuiXmlBinding("width_16_10")]
	public int Width16x10
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.calcArWidth(16, 10);
		}
	}

	// Token: 0x17000CBB RID: 3259
	// (get) Token: 0x0600709D RID: 28829 RVA: 0x002B50BD File Offset: 0x002B32BD
	[XuiXmlBinding("width_16_9")]
	public int Width16x9
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.calcArWidth(16, 9);
		}
	}

	// Token: 0x0600709E RID: 28830 RVA: 0x002B50CC File Offset: 0x002B32CC
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

	// Token: 0x0400550B RID: 21771
	[PublicizedFrom(EAccessModifier.Private)]
	public float availableXuiHeight = 1f;
}
