using System;
using UnityEngine.Scripting;

// Token: 0x02000999 RID: 2457
[Preserve]
public class ObjectiveFetchKeep : ObjectiveFetch
{
	// Token: 0x0600479A RID: 18330 RVA: 0x001C2CE2 File Offset: 0x001C0EE2
	public ObjectiveFetchKeep()
	{
		this.KeepItems = true;
	}

	// Token: 0x0600479B RID: 18331 RVA: 0x001C2CF4 File Offset: 0x001C0EF4
	public override BaseObjective Clone()
	{
		ObjectiveFetchKeep objectiveFetchKeep = new ObjectiveFetchKeep();
		this.CopyValues(objectiveFetchKeep);
		objectiveFetchKeep.KeepItems = this.KeepItems;
		return objectiveFetchKeep;
	}
}
