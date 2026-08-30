using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000619 RID: 1561
[Preserve]
public class BaseLootEntryRequirement
{
	// Token: 0x060032B4 RID: 12980 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Init(XElement e)
	{
	}

	// Token: 0x060032B5 RID: 12981 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public virtual bool CheckRequirement(EntityPlayer player)
	{
		return true;
	}
}
