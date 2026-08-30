using System;

// Token: 0x0200038B RID: 907
public abstract class DynamicMeshServerData : NetPackage
{
	// Token: 0x06001B05 RID: 6917
	public abstract bool Prechecks();

	// Token: 0x06001B06 RID: 6918 RVA: 0x0005F1CC File Offset: 0x0005D3CC
	[PublicizedFrom(EAccessModifier.Protected)]
	public DynamicMeshServerData()
	{
	}

	// Token: 0x04001166 RID: 4454
	public int X;

	// Token: 0x04001167 RID: 4455
	public int Z;
}
