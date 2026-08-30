using System;
using UnityEngine.Scripting;

// Token: 0x02000800 RID: 2048
[Preserve]
public class NetPackageInventoryDataResponse : NetPackage
{
	// Token: 0x17000646 RID: 1606
	// (get) Token: 0x06003C60 RID: 15456 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003C61 RID: 15457 RVA: 0x001823DC File Offset: 0x001805DC
	public NetPackageInventoryDataResponse Setup(bool _success, string _errorMsg, Guid _inventoryKey, ItemStack[] _items, Guid _managerToken)
	{
		this.success = _success;
		this.errorMsg = _errorMsg;
		this.inventoryKey = _inventoryKey;
		this.items = _items;
		this.managerToken = _managerToken;
		return this;
	}

	// Token: 0x06003C62 RID: 15458 RVA: 0x00182404 File Offset: 0x00180604
	public override void read(PooledBinaryReader _br)
	{
		this.success = _br.ReadBoolean();
		this.errorMsg = _br.ReadString();
		this.inventoryKey = StreamUtils.ReadGuid(_br);
		this.items = ItemStack.ReadArray(_br);
		this.managerToken = StreamUtils.ReadGuid(_br);
	}

	// Token: 0x06003C63 RID: 15459 RVA: 0x00182444 File Offset: 0x00180644
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.success);
		_bw.Write(this.errorMsg);
		StreamUtils.Write(_bw, this.inventoryKey);
		ItemStack.WriteArray(_bw, this.items);
		StreamUtils.Write(_bw, this.managerToken);
	}

	// Token: 0x06003C64 RID: 15460 RVA: 0x00182494 File Offset: 0x00180694
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

	// Token: 0x06003C65 RID: 15461 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}

	// Token: 0x04003116 RID: 12566
	[PublicizedFrom(EAccessModifier.Private)]
	public bool success;

	// Token: 0x04003117 RID: 12567
	[PublicizedFrom(EAccessModifier.Private)]
	public string errorMsg = string.Empty;

	// Token: 0x04003118 RID: 12568
	[PublicizedFrom(EAccessModifier.Private)]
	public Guid inventoryKey;

	// Token: 0x04003119 RID: 12569
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack[] items;

	// Token: 0x0400311A RID: 12570
	[PublicizedFrom(EAccessModifier.Private)]
	public Guid managerToken;
}
