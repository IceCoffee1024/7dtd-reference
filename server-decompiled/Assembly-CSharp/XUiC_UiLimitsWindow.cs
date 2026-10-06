using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E5A RID: 3674
[Preserve]
public class XUiC_UiLimitsWindow : XUiController
{
	// Token: 0x0600708E RID: 28814 RVA: 0x00291183 File Offset: 0x0028F383
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x0600708F RID: 28815 RVA: 0x002B83FC File Offset: 0x002B65FC
	public override void OnOpen()
	{
		base.OnOpen();
		int manualHeight = global::UnityEngine.Object.FindAnyObjectByType<UIRoot>().manualHeight;
		float scale = this.xui.GetScale();
		this.availableXuiHeight = (float)manualHeight / scale;
		base.RefreshBindings();
	}

	// Token: 0x17000CD1 RID: 3281
	// (get) Token: 0x06007090 RID: 28816 RVA: 0x002B8436 File Offset: 0x002B6636
	[XuiXmlBinding("height")]
	public int Height
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return Mathf.FloorToInt(this.availableXuiHeight);
		}
	}

	// Token: 0x17000CD2 RID: 3282
	// (get) Token: 0x06007091 RID: 28817 RVA: 0x002B8443 File Offset: 0x002B6643
	[XuiXmlBinding("width_5_4")]
	public int Width5x4
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.calcArWidth(5, 4);
		}
	}

	// Token: 0x17000CD3 RID: 3283
	// (get) Token: 0x06007092 RID: 28818 RVA: 0x002B844D File Offset: 0x002B664D
	[XuiXmlBinding("width_4_3")]
	public int Width4x3
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.calcArWidth(4, 3);
		}
	}

	// Token: 0x17000CD4 RID: 3284
	// (get) Token: 0x06007093 RID: 28819 RVA: 0x002B8457 File Offset: 0x002B6657
	[XuiXmlBinding("width_3_2")]
	public int Width3x2
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.calcArWidth(3, 2);
		}
	}

	// Token: 0x17000CD5 RID: 3285
	// (get) Token: 0x06007094 RID: 28820 RVA: 0x002B8461 File Offset: 0x002B6661
	[XuiXmlBinding("width_16_10")]
	public int Width16x10
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.calcArWidth(16, 10);
		}
	}

	// Token: 0x17000CD6 RID: 3286
	// (get) Token: 0x06007095 RID: 28821 RVA: 0x002B846D File Offset: 0x002B666D
	[XuiXmlBinding("width_16_9")]
	public int Width16x9
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.calcArWidth(16, 9);
		}
	}

	// Token: 0x06007096 RID: 28822 RVA: 0x002B847C File Offset: 0x002B667C
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

	// Token: 0x04005542 RID: 21826
	[PublicizedFrom(EAccessModifier.Private)]
	public float availableXuiHeight = 1f;
}
