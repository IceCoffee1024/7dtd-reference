using System;
using UnityEngine.Scripting;

// Token: 0x02000802 RID: 2050
[Preserve]
public class NetPackageInventoryTransactionRequest : NetPackage
{
	// Token: 0x17000648 RID: 1608
	// (get) Token: 0x06003C6D RID: 15469 RVA: 0x00020021 File Offset: 0x0001E221
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003C6E RID: 15470 RVA: 0x0018252F File Offset: 0x0018072F
	public NetPackageInventoryTransactionRequest Setup(InventoryTransaction _tx)
	{
		this.tx = _tx;
		return this;
	}

	// Token: 0x06003C6F RID: 15471 RVA: 0x00182539 File Offset: 0x00180739
	public override void read(PooledBinaryReader _br)
	{
		this.tx = InventoryTransaction.Read(_br);
	}

	// Token: 0x06003C70 RID: 15472 RVA: 0x00182547 File Offset: 0x00180747
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		InventoryTransaction.Write(_bw, this.tx);
	}

	// Token: 0x06003C71 RID: 15473 RVA: 0x0018255C File Offset: 0x0018075C
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		InventoryManager.Instance.TransactionRequestServer(this.tx, base.Sender.entityId);
	}

	// Token: 0x06003C72 RID: 15474 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}

	// Token: 0x0400311B RID: 12571
	[PublicizedFrom(EAccessModifier.Private)]
	public InventoryTransaction tx;
}
