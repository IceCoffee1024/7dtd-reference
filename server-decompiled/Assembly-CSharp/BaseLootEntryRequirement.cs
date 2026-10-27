using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x0200062B RID: 1579
[Preserve]
public class BaseLootEntryRequirement
{
	// Token: 0x06003358 RID: 13144 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Init(XElement e)
	{
	}

	// Token: 0x06003359 RID: 13145 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool CheckRequirement(EntityPlayer player)
	{
		return true;
	}
}
