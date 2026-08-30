using System;
using UnityEngine.Scripting;

// Token: 0x02000843 RID: 2115
[Preserve]
public class NetPackageSignDataRequest : NetPackage
{
	// Token: 0x1700066D RID: 1645
	// (get) Token: 0x06003DDC RID: 15836 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003DDD RID: 15837 RVA: 0x000027FC File Offset: 0x000009FC
	public override void read(PooledBinaryReader _reader)
	{
	}

	// Token: 0x06003DDE RID: 15838 RVA: 0x001799B3 File Offset: 0x00177BB3
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
	}

	// Token: 0x06003DDF RID: 15839 RVA: 0x00187F94 File Offset: 0x00186194
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		SignDataManager.Instance.SendSignDataToClient(base.Sender);
	}

	// Token: 0x06003DE0 RID: 15840 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}
}
