using System;
using UnityEngine.Scripting;

// Token: 0x020007F4 RID: 2036
[Preserve]
public class NetPackageInventoryDataRequest : NetPackage
{
	// Token: 0x1700063A RID: 1594
	// (get) Token: 0x06003BFA RID: 15354 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003BFB RID: 15355 RVA: 0x0017FE80 File Offset: 0x0017E080
	public NetPackageInventoryDataRequest Setup(TransactionalInventory.KeyHashPair _keyHash, Guid _managerToken)
	{
		this.keyHash = _keyHash;
		this.managerToken = _managerToken;
		return this;
	}

	// Token: 0x06003BFC RID: 15356 RVA: 0x0017FE91 File Offset: 0x0017E091
	public override void read(PooledBinaryReader _br)
	{
		this.keyHash = TransactionalInventory.KeyHashPair.Read(_br);
		this.managerToken = StreamUtils.ReadGuid(_br);
	}

	// Token: 0x06003BFD RID: 15357 RVA: 0x0017FEAB File Offset: 0x0017E0AB
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		this.keyHash.Write(_bw);
		StreamUtils.Write(_bw, this.managerToken);
	}

	// Token: 0x06003BFE RID: 15358 RVA: 0x0017FECC File Offset: 0x0017E0CC
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		TransactionalInventory transactionalInventory;
		if (!InventoryManager.Instance.TryGetTransactionalInventory(this.keyHash.Key, out transactionalInventory))
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageInventoryDataResponse>().Setup(false, string.Format("{0} Could not find inventory with key: {1}", "NetPackageInventoryDataRequest", this.keyHash.Key), this.keyHash.Key, null, this.managerToken), false, base.Sender.entityId, -1, -1, null, 192, false);
			return;
		}
		if (transactionalInventory.Hash == this.keyHash.Hash)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageInventoryDataResponse>().Setup(true, string.Empty, this.keyHash.Key, null, this.managerToken), false, base.Sender.entityId, -1, -1, null, 192, false);
			return;
		}
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageInventoryDataResponse>().Setup(true, string.Empty, this.keyHash.Key, transactionalInventory.GetItemsReadonly(), this.managerToken), false, base.Sender.entityId, -1, -1, null, 192, false);
	}

	// Token: 0x06003BFF RID: 15359 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}

	// Token: 0x04003095 RID: 12437
	[PublicizedFrom(EAccessModifier.Private)]
	public TransactionalInventory.KeyHashPair keyHash;

	// Token: 0x04003096 RID: 12438
	[PublicizedFrom(EAccessModifier.Private)]
	public Guid managerToken;
}
