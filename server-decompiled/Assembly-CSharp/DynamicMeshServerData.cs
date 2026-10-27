using System;

// Token: 0x02000394 RID: 916
public abstract class DynamicMeshServerData : NetPackage
{
	// Token: 0x06001B21 RID: 6945
	public abstract bool Prechecks();

	// Token: 0x06001B22 RID: 6946 RVA: 0x0005F9C8 File Offset: 0x0005DBC8
	[PublicizedFrom(EAccessModifier.Protected)]
	public DynamicMeshServerData()
	{
	}

	// Token: 0x04001184 RID: 4484
	public int X;

	// Token: 0x04001185 RID: 4485
	public int Z;
}
