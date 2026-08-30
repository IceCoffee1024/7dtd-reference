using System;
using System.Collections.Generic;

// Token: 0x02000574 RID: 1396
public class InventoryManager
{
	// Token: 0x170004DF RID: 1247
	// (get) Token: 0x06002D9E RID: 11678 RVA: 0x00122687 File Offset: 0x00120887
	public static InventoryManager Instance
	{
		get
		{
			if (InventoryManager._instance == null)
			{
				InventoryManager._instance = new InventoryManager();
			}
			return InventoryManager._instance;
		}
	}

	// Token: 0x06002D9F RID: 11679 RVA: 0x0012269F File Offset: 0x0012089F
	public void Init()
	{
		if (this.inventories.Count > 0)
		{
			Log.Warning("[InventoryManager] had locked entities from a previous session!");
			this.inventories.Clear();
		}
	}

	// Token: 0x06002DA0 RID: 11680 RVA: 0x001226C4 File Offset: 0x001208C4
	public void TransactionRequestLocal(InventoryTransaction _tx)
	{
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
		{
			this.TransactionRequestServer(_tx, GameManager.Instance.World.GetPrimaryPlayerId());
			return;
		}
		if (_tx.Apply(this.secretToken))
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageInventoryTransactionRequest>().Setup(_tx), false);
			return;
		}
		LocalPlayerUI.GetUIForPrimaryPlayer().windowManager.CloseAllOpenModalWindows(null, false);
		throw new InvalidOperationException("[InventoryManager] failed to apply transaction locally.");
	}

	// Token: 0x06002DA1 RID: 11681 RVA: 0x00122738 File Offset: 0x00120938
	public void TransactionRequestServer(InventoryTransaction _tx, int _playerId)
	{
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			throw new InvalidOperationException("[InventoryManager] TransactionRequestServer can only be called on the server.");
		}
		bool flag = _tx.Apply(this.secretToken);
		if (flag)
		{
			flag = _tx.ValidateFinalHashes();
		}
		if (!flag)
		{
			Log.Error("[InventoryManager] failed to apply transaction on server.");
			LockManager.Instance.ForceUnlockByPlayer(_playerId);
			return;
		}
		if (GameManager.Instance.World.GetPrimaryPlayerId() != _playerId)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageInventoryTransactionResponse>().Setup(flag, null, null), false, _playerId, -1, -1, null, 192, false);
		}
	}

	// Token: 0x06002DA2 RID: 11682 RVA: 0x00087DB8 File Offset: 0x00085FB8
	public void TransactionResponse(bool _success, IList<Guid> _keys, IList<ItemStack[]> _inventories)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06002DA3 RID: 11683 RVA: 0x001227C8 File Offset: 0x001209C8
	public TransactionalInventory CreateInventoryServer(int _size)
	{
		return this.CreateInventoryServer(ItemStack.CreateArray(_size));
	}

	// Token: 0x06002DA4 RID: 11684 RVA: 0x001227D6 File Offset: 0x001209D6
	public TransactionalInventory CreateInventoryServer(ItemStack[] _stacks)
	{
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			throw new InvalidOperationException("[InventoryManager] CreateInventoryServer can only be called on the server.");
		}
		return this.CreateInventory(Guid.NewGuid(), _stacks);
	}

	// Token: 0x06002DA5 RID: 11685 RVA: 0x001227FC File Offset: 0x001209FC
	[PublicizedFrom(EAccessModifier.Private)]
	public TransactionalInventory CreateInventory(Guid _key, ItemStack[] _stacks)
	{
		TransactionalInventory transactionalInventory = new TransactionalInventory(_key, _stacks, this.secretToken);
		this.inventories.Add(_key, transactionalInventory);
		return transactionalInventory;
	}

	// Token: 0x06002DA6 RID: 11686 RVA: 0x00122825 File Offset: 0x00120A25
	public void RequestInventoryFromServer(TransactionalInventory.KeyHashPair _keyHash)
	{
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
		{
			throw new InvalidOperationException("[InventoryManager] RequestInventoryFromServer can only be called on a client.");
		}
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageInventoryDataRequest>().Setup(_keyHash, this.secretToken), false);
	}

	// Token: 0x06002DA7 RID: 11687 RVA: 0x0012285A File Offset: 0x00120A5A
	public bool TryGetTransactionalInventory(Guid _key, out TransactionalInventory _inventory)
	{
		return this.inventories.TryGetValue(_key, out _inventory);
	}

	// Token: 0x06002DA8 RID: 11688 RVA: 0x00122869 File Offset: 0x00120A69
	public bool ValidateToken(Guid _token)
	{
		bool flag = _token.Equals(this.secretToken);
		if (!flag)
		{
			Log.Warning("[InventoryManager] calling client code provided invalid token.");
		}
		return flag;
	}

	// Token: 0x06002DA9 RID: 11689 RVA: 0x00122888 File Offset: 0x00120A88
	public void ReadInventory(PooledBinaryReader _br, TileEntity.StreamModeRead _mode, ref TransactionalInventory _inventory)
	{
		switch (_mode)
		{
		case TileEntity.StreamModeRead.Persistency:
		{
			Guid guid = StreamUtils.ReadGuid(_br);
			ItemStack[] array = ItemStack.ReadArray(_br);
			if (!this.TryGetTransactionalInventory(guid, out _inventory))
			{
				_inventory = this.CreateInventory(guid, array);
				return;
			}
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && LockManager.Instance.IsLockedServer(_inventory, 0))
			{
				Log.Error("[InventoryManager] Cannot read from storage while target is locked.");
				return;
			}
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
			{
				Log.Error("[InventoryManager] Client tried to read inventory data from persistent storage.");
				return;
			}
			_inventory.UpdateInventory(array, this.secretToken);
			return;
		}
		case TileEntity.StreamModeRead.FromServer:
		{
			Guid guid2 = StreamUtils.ReadGuid(_br);
			int num = _br.ReadInt32();
			if (!this.TryGetTransactionalInventory(guid2, out _inventory))
			{
				_inventory = this.CreateInventory(guid2, ItemStack.CreateArray(num));
			}
			return;
		}
		case TileEntity.StreamModeRead.FromClient:
			return;
		default:
			return;
		}
	}

	// Token: 0x06002DAA RID: 11690 RVA: 0x00122940 File Offset: 0x00120B40
	public void WriteInventory(TransactionalInventory _inventory, PooledBinaryWriter _bw, TileEntity.StreamModeWrite _mode)
	{
		switch (_mode)
		{
		case TileEntity.StreamModeWrite.Persistency:
			StreamUtils.Write(_bw, _inventory.Key);
			ItemStack.WriteArray(_bw, _inventory.GetItemsReadonly());
			return;
		case TileEntity.StreamModeWrite.ToServer:
			return;
		case TileEntity.StreamModeWrite.ToClient:
			StreamUtils.Write(_bw, _inventory.Key);
			_bw.Write(_inventory.Length);
			return;
		default:
			return;
		}
	}

	// Token: 0x040023A2 RID: 9122
	[PublicizedFrom(EAccessModifier.Private)]
	public static InventoryManager _instance;

	// Token: 0x040023A3 RID: 9123
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<Guid, TransactionalInventory> inventories = new Dictionary<Guid, TransactionalInventory>();

	// Token: 0x040023A4 RID: 9124
	[PublicizedFrom(EAccessModifier.Private)]
	public Guid secretToken = Guid.NewGuid();
}
