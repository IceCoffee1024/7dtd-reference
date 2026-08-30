using System;
using UnityEngine.Scripting;

// Token: 0x02000982 RID: 2434
[Preserve]
public class ObjectiveFetchKeep : ObjectiveFetch
{
	// Token: 0x06004765 RID: 18277 RVA: 0x001BFD44 File Offset: 0x001BDF44
	public ObjectiveFetchKeep()
	{
		this.KeepItems = true;
	}

	// Token: 0x06004766 RID: 18278 RVA: 0x001BFD54 File Offset: 0x001BDF54
	public override BaseObjective Clone()
	{
		ObjectiveFetchKeep objectiveFetchKeep = new ObjectiveFetchKeep();
		this.CopyValues(objectiveFetchKeep);
		objectiveFetchKeep.KeepItems = this.KeepItems;
		return objectiveFetchKeep;
	}
}
