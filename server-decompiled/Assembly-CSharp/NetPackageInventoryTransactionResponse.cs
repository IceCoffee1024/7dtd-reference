using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000803 RID: 2051
[Preserve]
public class NetPackageInventoryTransactionResponse : NetPackage
{
	// Token: 0x17000649 RID: 1609
	// (get) Token: 0x06003C74 RID: 15476 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003C75 RID: 15477 RVA: 0x0018257C File Offset: 0x0018077C
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

	// Token: 0x06003C76 RID: 15478 RVA: 0x001825E8 File Offset: 0x001807E8
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

	// Token: 0x06003C77 RID: 15479 RVA: 0x0018265C File Offset: 0x0018085C
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

	// Token: 0x06003C78 RID: 15480 RVA: 0x000027FC File Offset: 0x000009FC
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
	}

	// Token: 0x06003C79 RID: 15481 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}

	// Token: 0x0400311C RID: 12572
	[PublicizedFrom(EAccessModifier.Private)]
	public bool success;

	// Token: 0x0400311D RID: 12573
	[PublicizedFrom(EAccessModifier.Private)]
	public IList<Guid> keys;

	// Token: 0x0400311E RID: 12574
	[PublicizedFrom(EAccessModifier.Private)]
	public IList<ItemStack[]> inventories;
}
