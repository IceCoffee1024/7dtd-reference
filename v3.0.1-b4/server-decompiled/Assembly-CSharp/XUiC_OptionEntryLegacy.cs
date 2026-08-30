using System;
using UnityEngine.Scripting;

// Token: 0x02000D89 RID: 3465
[Preserve]
public class XUiC_OptionEntryLegacy : XUiC_OptionEntryAbs
{
	// Token: 0x17000B55 RID: 2901
	// (get) Token: 0x0600698B RID: 27019 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsChanged
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000B56 RID: 2902
	// (get) Token: 0x0600698C RID: 27020 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsDefault
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600698D RID: 27021 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void initCurrentValue()
	{
	}

	// Token: 0x0600698E RID: 27022 RVA: 0x000027FC File Offset: 0x000009FC
	public override void DiscardCurrentChange()
	{
	}

	// Token: 0x0600698F RID: 27023 RVA: 0x000027FC File Offset: 0x000009FC
	public override void ApplySelection()
	{
	}

	// Token: 0x06006990 RID: 27024 RVA: 0x000027FC File Offset: 0x000009FC
	public override void ResetToDefault()
	{
	}
}
