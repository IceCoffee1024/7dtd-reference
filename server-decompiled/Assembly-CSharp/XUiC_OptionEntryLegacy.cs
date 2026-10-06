using System;
using UnityEngine.Scripting;

// Token: 0x02000DAC RID: 3500
[Preserve]
public class XUiC_OptionEntryLegacy : XUiC_OptionEntryAbs
{
	// Token: 0x17000B88 RID: 2952
	// (get) Token: 0x06006A1F RID: 27167 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsChanged
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000B89 RID: 2953
	// (get) Token: 0x06006A20 RID: 27168 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsDefault
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06006A21 RID: 27169 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void initCurrentValue()
	{
	}

	// Token: 0x06006A22 RID: 27170 RVA: 0x000027FC File Offset: 0x000009FC
	public override void DiscardCurrentChange()
	{
	}

	// Token: 0x06006A23 RID: 27171 RVA: 0x000027FC File Offset: 0x000009FC
	public override void ApplySelection()
	{
	}

	// Token: 0x06006A24 RID: 27172 RVA: 0x000027FC File Offset: 0x000009FC
	public override void ResetToDefault()
	{
	}
}
