using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020007F8 RID: 2040
[Preserve]
public class NetPackageInventoryTransactionResponse : NetPackage
{
	// Token: 0x1700063E RID: 1598
	// (get) Token: 0x06003C15 RID: 15381 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003C16 RID: 15382 RVA: 0x001801A4 File Offset: 0x0017E3A4
	public NetPackageInventoryTransactionResponse Setup(bool _success, IList<Guid> _keys, IList<ItemStack[]> _inventoriesUpdated)
	{
		if (_keys == null != (_inventoriesUpdated == null) || (_keys != null && _keys.Count != _inventoriesUpdated.Count))
		{
			Log.Error("[NetPackageInventoryTransactionResponse] Mismatch in count of supplied keys and inventories.");
			this.success = false;
			this.keys = null;
			this.inventories = null;
			return this;
		}
		this.success = _success;
		this.keys = _keys;
		this.inventories = _inventoriesUpdated;
		return this;
	}

	// Token: 0x06003C17 RID: 15383 RVA: 0x00180210 File Offset: 0x0017E410
	public override void read(PooledBinaryReader _br)
	{
		this.success = _br.ReadBoolean();
		int num = _br.ReadInt32();
		List<Guid> list = new List<Guid>(num);
		List<ItemStack[]> list2 = new List<ItemStack[]>(num);
		for (int i = 0; i < num; i++)
		{
			list.Add(StreamUtils.ReadGuid(_br));
			if (_br.ReadBoolean())
			{
				list2.Add(ItemStack.ReadArray(_br));
			}
			else
			{
				list2.Add(null);
			}
		}
		this.keys = list;
		this.inventories = list2;
	}

	// Token: 0x06003C18 RID: 15384 RVA: 0x00180284 File Offset: 0x0017E484
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		if (this.keys == null || this.inventories == null)
		{
			_bw.Write(false);
			_bw.Write(0);
			return;
		}
		_bw.Write(this.success);
		_bw.Write(this.keys.Count);
		for (int i = 0; i < this.keys.Count; i++)
		{
			StreamUtils.Write(_bw, this.keys[i]);
			bool flag = this.inventories[i] != null;
			_bw.Write(flag);
			if (flag)
			{
				ItemStack.WriteArray(_bw, this.inventories[i]);
			}
		}
	}

	// Token: 0x06003C19 RID: 15385 RVA: 0x000027FC File Offset: 0x000009FC
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
	}

	// Token: 0x06003C1A RID: 15386 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}

	// Token: 0x0400309D RID: 12445
	[PublicizedFrom(EAccessModifier.Private)]
	public bool success;

	// Token: 0x0400309E RID: 12446
	[PublicizedFrom(EAccessModifier.Private)]
	public IList<Guid> keys;

	// Token: 0x0400309F RID: 12447
	[PublicizedFrom(EAccessModifier.Private)]
	public IList<ItemStack[]> inventories;
}
