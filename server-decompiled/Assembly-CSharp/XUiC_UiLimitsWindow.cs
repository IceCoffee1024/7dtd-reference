using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E65 RID: 3685
[Preserve]
public class XUiC_UiLimitsWindow : XUiController
{
	// Token: 0x060070B3 RID: 28851 RVA: 0x002926AB File Offset: 0x002908AB
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x060070B4 RID: 28852 RVA: 0x002B9968 File Offset: 0x002B7B68
	public override void OnOpen()
	{
		base.OnOpen();
		int manualHeight = global::UnityEngine.Object.FindAnyObjectByType<UIRoot>().manualHeight;
		float scale = this.xui.GetScale();
		this.availableXuiHeight = (float)manualHeight / scale;
		base.RefreshBindings();
	}

	// Token: 0x17000CD7 RID: 3287
	// (get) Token: 0x060070B5 RID: 28853 RVA: 0x002B99A2 File Offset: 0x002B7BA2
	[XuiXmlBinding("height")]
	public int Height
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return Mathf.FloorToInt(this.availableXuiHeight);
		}
	}

	// Token: 0x17000CD8 RID: 3288
	// (get) Token: 0x060070B6 RID: 28854 RVA: 0x002B99AF File Offset: 0x002B7BAF
	[XuiXmlBinding("width_5_4")]
	public int Width5x4
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.calcArWidth(5, 4);
		}
	}

	// Token: 0x17000CD9 RID: 3289
	// (get) Token: 0x060070B7 RID: 28855 RVA: 0x002B99B9 File Offset: 0x002B7BB9
	[XuiXmlBinding("width_4_3")]
	public int Width4x3
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.calcArWidth(4, 3);
		}
	}

	// Token: 0x17000CDA RID: 3290
	// (get) Token: 0x060070B8 RID: 28856 RVA: 0x002B99C3 File Offset: 0x002B7BC3
	[XuiXmlBinding("width_3_2")]
	public int Width3x2
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.calcArWidth(3, 2);
		}
	}

	// Token: 0x17000CDB RID: 3291
	// (get) Token: 0x060070B9 RID: 28857 RVA: 0x002B99CD File Offset: 0x002B7BCD
	[XuiXmlBinding("width_16_10")]
	public int Width16x10
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.calcArWidth(16, 10);
		}
	}

	// Token: 0x17000CDC RID: 3292
	// (get) Token: 0x060070BA RID: 28858 RVA: 0x002B99D9 File Offset: 0x002B7BD9
	[XuiXmlBinding("width_16_9")]
	public int Width16x9
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.calcArWidth(16, 9);
		}
	}

	// Token: 0x060070BB RID: 28859 RVA: 0x002B99E8 File Offset: 0x002B7BE8
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

	// Token: 0x04005567 RID: 21863
	[PublicizedFrom(EAccessModifier.Private)]
	public float availableXuiHeight = 1f;
}
