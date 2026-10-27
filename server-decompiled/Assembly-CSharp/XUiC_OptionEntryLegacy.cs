using System;
using UnityEngine.Scripting;

// Token: 0x02000DB7 RID: 3511
[Preserve]
public class XUiC_OptionEntryLegacy : XUiC_OptionEntryAbs
{
	// Token: 0x17000B8E RID: 2958
	// (get) Token: 0x06006A44 RID: 27204 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsChanged
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000B8F RID: 2959
	// (get) Token: 0x06006A45 RID: 27205 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsDefault
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06006A46 RID: 27206 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void initCurrentValue()
	{
	}

	// Token: 0x06006A47 RID: 27207 RVA: 0x000027FC File Offset: 0x000009FC
	public override void DiscardCurrentChange()
	{
	}

	// Token: 0x06006A48 RID: 27208 RVA: 0x000027FC File Offset: 0x000009FC
	public override void ApplySelection()
	{
	}

	// Token: 0x06006A49 RID: 27209 RVA: 0x000027FC File Offset: 0x000009FC
	public override void ResetToDefault()
	{
	}
}
