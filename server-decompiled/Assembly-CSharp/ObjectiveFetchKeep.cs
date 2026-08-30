using System;
using UnityEngine.Scripting;

// Token: 0x02000993 RID: 2451
[Preserve]
public class ObjectiveFetchKeep : ObjectiveFetch
{
	// Token: 0x060047EB RID: 18411 RVA: 0x001C2538 File Offset: 0x001C0738
	public ObjectiveFetchKeep()
	{
		this.KeepItems = true;
	}

	// Token: 0x060047EC RID: 18412 RVA: 0x001C2548 File Offset: 0x001C0748
	public override BaseObjective Clone()
	{
		ObjectiveFetchKeep objectiveFetchKeep = new ObjectiveFetchKeep();
		this.CopyValues(objectiveFetchKeep);
		objectiveFetchKeep.KeepItems = this.KeepItems;
		return objectiveFetchKeep;
	}
}
