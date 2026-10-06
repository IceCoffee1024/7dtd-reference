using System;

// Token: 0x02000392 RID: 914
public abstract class DynamicMeshServerData : NetPackage
{
	// Token: 0x06001B1D RID: 6941
	public abstract bool Prechecks();

	// Token: 0x06001B1E RID: 6942 RVA: 0x0005F704 File Offset: 0x0005D904
	[PublicizedFrom(EAccessModifier.Protected)]
	public DynamicMeshServerData()
	{
	}

	// Token: 0x04001174 RID: 4468
	public int X;

	// Token: 0x04001175 RID: 4469
	public int Z;
}
