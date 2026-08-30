using System;
using System.Collections.Generic;

// Token: 0x0200056F RID: 1391
public class InventoryManager
{
	// Token: 0x170004DB RID: 1243
	// (get) Token: 0x06002D73 RID: 11635 RVA: 0x001215CB File Offset: 0x0011F7CB
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

	// Token: 0x06002D74 RID: 11636 RVA: 0x001215E3 File Offset: 0x0011F7E3
	public void Init()
	{
		if (this.inventories.Count > 0)
		{
			Log.Warning("[InventoryManager] had locked entities from a previous session!");
			this.inventories.Clear();
		}
	}

	// Token: 0x06002D75 RID: 11637 RVA: 0x00121608 File Offset: 0x0011F808
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

	// Token: 0x06002D76 RID: 11638 RVA: 0x0012167C File Offset: 0x0011F87C
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

	// Token: 0x06002D77 RID: 11639 RVA: 0x000876FD File Offset: 0x000858FD
	public void TransactionResponse(bool _success, IList<Guid> _keys, IList<ItemStack[]> _inventories)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06002D78 RID: 11640 RVA: 0x0012170C File Offset: 0x0011F90C
	public TransactionalInventory CreateInventoryServer(int _size)
	{
		return this.CreateInventoryServer(ItemStack.CreateArray(_size));
	}

	// Token: 0x06002D79 RID: 11641 RVA: 0x0012171A File Offset: 0x0011F91A
	public TransactionalInventory CreateInventoryServer(ItemStack[] _stacks)
	{
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			throw new InvalidOperationException("[InventoryManager] CreateInventoryServer can only be called on the server.");
		}
		return this.CreateInventory(Guid.NewGuid(), _stacks);
	}

	// Token: 0x06002D7A RID: 11642 RVA: 0x00121740 File Offset: 0x0011F940
	[PublicizedFrom(EAccessModifier.Private)]
	public TransactionalInventory CreateInventory(Guid _key, ItemStack[] _stacks)
	{
		TransactionalInventory transactionalInventory = new TransactionalInventory(_key, _stacks, this.secretToken);
		this.inventories.Add(_key, transactionalInventory);
		return transactionalInventory;
	}

	// Token: 0x06002D7B RID: 11643 RVA: 0x00121769 File Offset: 0x0011F969
	public void RequestInventoryFromServer(TransactionalInventory.KeyHashPair _keyHash)
	{
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
		{
			throw new InvalidOperationException("[InventoryManager] RequestInventoryFromServer can only be called on a client.");
		}
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageInventoryDataRequest>().Setup(_keyHash, this.secretToken), false);
	}

	// Token: 0x06002D7C RID: 11644 RVA: 0x0012179E File Offset: 0x0011F99E
	public bool TryGetTransactionalInventory(Guid _key, out TransactionalInventory _inventory)
	{
		return this.inventories.TryGetValue(_key, out _inventory);
	}

	// Token: 0x06002D7D RID: 11645 RVA: 0x001217AD File Offset: 0x0011F9AD
	public bool ValidateToken(Guid _token)
	{
		bool flag = _token.Equals(this.secretToken);
		if (!flag)
		{
			Log.Warning("[InventoryManager] calling client code provided invalid token.");
		}
		return flag;
	}

	// Token: 0x06002D7E RID: 11646 RVA: 0x001217CC File Offset: 0x0011F9CC
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

	// Token: 0x06002D7F RID: 11647 RVA: 0x00121884 File Offset: 0x0011FA84
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

	// Token: 0x04002370 RID: 9072
	[PublicizedFrom(EAccessModifier.Private)]
	public static InventoryManager _instance;

	// Token: 0x04002371 RID: 9073
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<Guid, TransactionalInventory> inventories = new Dictionary<Guid, TransactionalInventory>();

	// Token: 0x04002372 RID: 9074
	[PublicizedFrom(EAccessModifier.Private)]
	public Guid secretToken = Guid.NewGuid();
}
