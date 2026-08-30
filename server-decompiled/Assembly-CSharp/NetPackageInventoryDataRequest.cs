using System;
using UnityEngine.Scripting;

// Token: 0x020007FF RID: 2047
[Preserve]
public class NetPackageInventoryDataRequest : NetPackage
{
	// Token: 0x17000645 RID: 1605
	// (get) Token: 0x06003C59 RID: 15449 RVA: 0x00020021 File Offset: 0x0001E221
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003C5A RID: 15450 RVA: 0x00182258 File Offset: 0x00180458
	public NetPackageInventoryDataRequest Setup(TransactionalInventory.KeyHashPair _keyHash, Guid _managerToken)
	{
		this.keyHash = _keyHash;
		this.managerToken = _managerToken;
		return this;
	}

	// Token: 0x06003C5B RID: 15451 RVA: 0x00182269 File Offset: 0x00180469
	public override void read(PooledBinaryReader _br)
	{
		this.keyHash = TransactionalInventory.KeyHashPair.Read(_br);
		this.managerToken = StreamUtils.ReadGuid(_br);
	}

	// Token: 0x06003C5C RID: 15452 RVA: 0x00182283 File Offset: 0x00180483
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		this.keyHash.Write(_bw);
		StreamUtils.Write(_bw, this.managerToken);
	}

	// Token: 0x06003C5D RID: 15453 RVA: 0x001822A4 File Offset: 0x001804A4
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

	// Token: 0x06003C5E RID: 15454 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}

	// Token: 0x04003114 RID: 12564
	[PublicizedFrom(EAccessModifier.Private)]
	public TransactionalInventory.KeyHashPair keyHash;

	// Token: 0x04003115 RID: 12565
	[PublicizedFrom(EAccessModifier.Private)]
	public Guid managerToken;
}
