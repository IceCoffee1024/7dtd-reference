using System;
using UnityEngine.Scripting;

// Token: 0x02000D9D RID: 3485
[Preserve]
public class XUiC_OptionEntryLegacy : XUiC_OptionEntryAbs
{
	// Token: 0x17000B6D RID: 2925
	// (get) Token: 0x06006A27 RID: 27175 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsChanged
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000B6E RID: 2926
	// (get) Token: 0x06006A28 RID: 27176 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsDefault
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06006A29 RID: 27177 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void initCurrentValue()
	{
	}

	// Token: 0x06006A2A RID: 27178 RVA: 0x000027FC File Offset: 0x000009FC
	public override void DiscardCurrentChange()
	{
	}

	// Token: 0x06006A2B RID: 27179 RVA: 0x000027FC File Offset: 0x000009FC
	public override void ApplySelection()
	{
	}

	// Token: 0x06006A2C RID: 27180 RVA: 0x000027FC File Offset: 0x000009FC
	public override void ResetToDefault()
	{
	}
}
