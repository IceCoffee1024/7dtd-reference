using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000624 RID: 1572
[Preserve]
public class BaseLootEntryRequirement
{
	// Token: 0x0600334A RID: 13130 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Init(XElement e)
	{
	}

	// Token: 0x0600334B RID: 13131 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool CheckRequirement(EntityPlayer player)
	{
		return true;
	}
}
