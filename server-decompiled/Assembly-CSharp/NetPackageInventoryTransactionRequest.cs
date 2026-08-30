using System;
using UnityEngine.Scripting;

// Token: 0x020007F7 RID: 2039
[Preserve]
public class NetPackageInventoryTransactionRequest : NetPackage
{
	// Token: 0x1700063D RID: 1597
	// (get) Token: 0x06003C0E RID: 15374 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003C0F RID: 15375 RVA: 0x00180157 File Offset: 0x0017E357
	public NetPackageInventoryTransactionRequest Setup(InventoryTransaction _tx)
	{
		this.tx = _tx;
		return this;
	}

	// Token: 0x06003C10 RID: 15376 RVA: 0x00180161 File Offset: 0x0017E361
	public override void read(PooledBinaryReader _br)
	{
		this.tx = InventoryTransaction.Read(_br);
	}

	// Token: 0x06003C11 RID: 15377 RVA: 0x0018016F File Offset: 0x0017E36F
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		InventoryTransaction.Write(_bw, this.tx);
	}

	// Token: 0x06003C12 RID: 15378 RVA: 0x00180184 File Offset: 0x0017E384
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		InventoryManager.Instance.TransactionRequestServer(this.tx, base.Sender.entityId);
	}

	// Token: 0x06003C13 RID: 15379 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}

	// Token: 0x0400309C RID: 12444
	[PublicizedFrom(EAccessModifier.Private)]
	public InventoryTransaction tx;
}
