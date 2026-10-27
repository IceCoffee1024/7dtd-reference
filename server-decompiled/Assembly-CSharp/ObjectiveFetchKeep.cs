using System;
using UnityEngine.Scripting;

// Token: 0x020009A3 RID: 2467
[Preserve]
public class ObjectiveFetchKeep : ObjectiveFetch
{
	// Token: 0x060047BD RID: 18365 RVA: 0x001C3D66 File Offset: 0x001C1F66
	public ObjectiveFetchKeep()
	{
		this.KeepItems = true;
	}

	// Token: 0x060047BE RID: 18366 RVA: 0x001C3D78 File Offset: 0x001C1F78
	public override BaseObjective Clone()
	{
		ObjectiveFetchKeep objectiveFetchKeep = new ObjectiveFetchKeep();
		this.CopyValues(objectiveFetchKeep);
		objectiveFetchKeep.KeepItems = this.KeepItems;
		return objectiveFetchKeep;
	}
}
