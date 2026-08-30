using System;
using UnityEngine.Scripting;

// Token: 0x020007F5 RID: 2037
[Preserve]
public class NetPackageInventoryDataResponse : NetPackage
{
	// Token: 0x1700063B RID: 1595
	// (get) Token: 0x06003C01 RID: 15361 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003C02 RID: 15362 RVA: 0x00180004 File Offset: 0x0017E204
	public NetPackageInventoryDataResponse Setup(bool _success, string _errorMsg, Guid _inventoryKey, ItemStack[] _items, Guid _managerToken)
	{
		this.success = _success;
		this.errorMsg = _errorMsg;
		this.inventoryKey = _inventoryKey;
		this.items = _items;
		this.managerToken = _managerToken;
		return this;
	}

	// Token: 0x06003C03 RID: 15363 RVA: 0x0018002C File Offset: 0x0017E22C
	public override void read(PooledBinaryReader _br)
	{
		this.success = _br.ReadBoolean();
		this.errorMsg = _br.ReadString();
		this.inventoryKey = StreamUtils.ReadGuid(_br);
		this.items = ItemStack.ReadArray(_br);
		this.managerToken = StreamUtils.ReadGuid(_br);
	}

	// Token: 0x06003C04 RID: 15364 RVA: 0x0018006C File Offset: 0x0017E26C
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.success);
		_bw.Write(this.errorMsg);
		StreamUtils.Write(_bw, this.inventoryKey);
		ItemStack.WriteArray(_bw, this.items);
		StreamUtils.Write(_bw, this.managerToken);
	}

	// Token: 0x06003C05 RID: 15365 RVA: 0x001800BC File Offset: 0x0017E2BC
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (!this.success)
		{
			Log.Warning("Error during transaction request: " + this.errorMsg);
			return;
		}
		TransactionalInventory transactionalInventory;
		if (InventoryManager.Instance.TryGetTransactionalInventory(this.inventoryKey, out transactionalInventory))
		{
			transactionalInventory.UpdateInventory(this.items, this.managerToken);
			return;
		}
		Log.Error(string.Format("{0} Could not find inventory with key: {1}", "NetPackageInventoryDataResponse", this.inventoryKey));
	}

	// Token: 0x06003C06 RID: 15366 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}

	// Token: 0x04003097 RID: 12439
	[PublicizedFrom(EAccessModifier.Private)]
	public bool success;

	// Token: 0x04003098 RID: 12440
	[PublicizedFrom(EAccessModifier.Private)]
	public string errorMsg = string.Empty;

	// Token: 0x04003099 RID: 12441
	[PublicizedFrom(EAccessModifier.Private)]
	public Guid inventoryKey;

	// Token: 0x0400309A RID: 12442
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack[] items;

	// Token: 0x0400309B RID: 12443
	[PublicizedFrom(EAccessModifier.Private)]
	public Guid managerToken;
}
