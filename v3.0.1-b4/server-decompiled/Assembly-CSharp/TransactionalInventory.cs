using System;
using System.IO;
using System.Runtime.CompilerServices;

// Token: 0x02000585 RID: 1413
public class TransactionalInventory : ILockTarget
{
	// Token: 0x170004F2 RID: 1266
	// (get) Token: 0x06002E4C RID: 11852 RVA: 0x001262D7 File Offset: 0x001244D7
	// (set) Token: 0x06002E4D RID: 11853 RVA: 0x001262DF File Offset: 0x001244DF
	public bool DataReadyLocally
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170004F3 RID: 1267
	// (get) Token: 0x06002E4E RID: 11854 RVA: 0x001262E8 File Offset: 0x001244E8
	// (set) Token: 0x06002E4F RID: 11855 RVA: 0x001262F0 File Offset: 0x001244F0
	public Guid Key
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170004F4 RID: 1268
	// (get) Token: 0x06002E50 RID: 11856 RVA: 0x001262F9 File Offset: 0x001244F9
	public int Hash
	{
		get
		{
			if (this.hashDirty)
			{
				this.hash = this.items.GetArrayHashCode();
				this.hashDirty = false;
			}
			return this.hash;
		}
	}

	// Token: 0x170004F5 RID: 1269
	// (get) Token: 0x06002E51 RID: 11857 RVA: 0x00126321 File Offset: 0x00124521
	public TransactionalInventory.KeyHashPair KeyHash
	{
		get
		{
			return new TransactionalInventory.KeyHashPair(this.Key, this.Hash);
		}
	}

	// Token: 0x170004F6 RID: 1270
	// (get) Token: 0x06002E52 RID: 11858 RVA: 0x00126334 File Offset: 0x00124534
	public bool NeedsSaving
	{
		get
		{
			return this.needsSaving;
		}
	}

	// Token: 0x170004F7 RID: 1271
	// (get) Token: 0x06002E53 RID: 11859 RVA: 0x0012633C File Offset: 0x0012453C
	public int Length
	{
		get
		{
			return this.items.Length;
		}
	}

	// Token: 0x170004F8 RID: 1272
	// (get) Token: 0x06002E54 RID: 11860 RVA: 0x00046C7A File Offset: 0x00044E7A
	public LockTargetType LockTargetType
	{
		get
		{
			return LockTargetType.TransactionalInventory;
		}
	}

	// Token: 0x06002E55 RID: 11861 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool IsSharedLock(ushort _channel)
	{
		return false;
	}

	// Token: 0x06002E56 RID: 11862 RVA: 0x00126346 File Offset: 0x00124546
	public TransactionalInventory(Guid _key, ItemStack[] _items, Guid _managerToken)
	{
		this.DataReadyLocally = true;
		this.items = _items;
		this.Key = _key;
		InventoryManager.Instance.ValidateToken(_managerToken);
	}

	// Token: 0x06002E57 RID: 11863 RVA: 0x0012637D File Offset: 0x0012457D
	public ItemStack GetItemReadonly(int _slot)
	{
		if (_slot > this.items.Length)
		{
			return null;
		}
		return this.items[_slot].Clone();
	}

	// Token: 0x06002E58 RID: 11864 RVA: 0x00126399 File Offset: 0x00124599
	public ItemStack[] GetItemsReadonly()
	{
		return ItemStack.Clone(this.items);
	}

	// Token: 0x06002E59 RID: 11865 RVA: 0x001263A8 File Offset: 0x001245A8
	public void ResizeInventory(int _size, Guid _managerToken)
	{
		if (!InventoryManager.Instance.ValidateToken(_managerToken))
		{
			return;
		}
		if (_size < this.items.Length)
		{
			Log.Warning("[TransactionalInventory] Inventory was resized to a smaller size, items may be lost.");
		}
		Array.Copy(this.items, 0, this.items, 0, (_size > this.items.Length) ? this.items.Length : _size);
	}

	// Token: 0x06002E5A RID: 11866 RVA: 0x00126404 File Offset: 0x00124604
	public void UpdateInventory(ItemStack[] _items, Guid _managerToken)
	{
		if (!InventoryManager.Instance.ValidateToken(_managerToken))
		{
			return;
		}
		if (_items != null)
		{
			if (this.items == null)
			{
				this.items = ItemStack.Clone(_items);
			}
			else if (_items.Length != this.items.Length)
			{
				this.ResizeInventory(_items.Length, _managerToken);
			}
			else
			{
				Array.Copy(_items, this.items, this.items.Length);
			}
		}
		this.hashDirty = true;
		this.DataReadyLocally = true;
		Action action = this.inventoryRequestComplete;
		if (action != null)
		{
			action();
		}
		this.inventoryRequestComplete = null;
	}

	// Token: 0x06002E5B RID: 11867 RVA: 0x0012648B File Offset: 0x0012468B
	[PublicizedFrom(EAccessModifier.Private)]
	public bool CheckInventoryReady()
	{
		if (!this.DataReadyLocally)
		{
			Log.Warning("[TransactionalInventory] Data has not yet been fetched properly.");
			return false;
		}
		if (!LockManager.Instance.IsLockedByLocalPlayer(this, 0))
		{
			Log.Warning("[TransactionalInventory] Local player does not have a lock for this inventory.");
			return false;
		}
		return true;
	}

	// Token: 0x06002E5C RID: 11868 RVA: 0x001264BC File Offset: 0x001246BC
	public bool IsEmpty()
	{
		if (!this.CheckInventoryReady())
		{
			return false;
		}
		for (int i = 0; i < this.items.Length; i++)
		{
			if (!this.items[i].IsEmpty())
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06002E5D RID: 11869 RVA: 0x001264F8 File Offset: 0x001246F8
	public int HasItem(ItemValue _itemValue)
	{
		if (!this.CheckInventoryReady())
		{
			return -1;
		}
		if (_itemValue == null)
		{
			return -1;
		}
		for (int i = 0; i < this.items.Length; i++)
		{
			if (_itemValue.ItemClass.Equals(this.items[i].itemValue.ItemClass))
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06002E5E RID: 11870 RVA: 0x0012654C File Offset: 0x0012474C
	public bool TrySetItem(int _idx, ItemStack _stack)
	{
		if (!this.CheckInventoryReady())
		{
			return false;
		}
		if (_idx < 0 || _idx >= this.items.Length)
		{
			Log.Warning("[TransactionalInventory] Supplied index was out of bounds.");
			return false;
		}
		InventoryTransaction inventoryTransaction = new InventoryTransaction().SetStackAbsolute(this, _stack, _idx);
		InventoryManager.Instance.TransactionRequestLocal(inventoryTransaction);
		return true;
	}

	// Token: 0x06002E5F RID: 11871 RVA: 0x00126598 File Offset: 0x00124798
	public bool TryAddItem(ItemStack _stack)
	{
		if (!this.CheckInventoryReady())
		{
			return false;
		}
		for (int i = 0; i < this.items.Length; i++)
		{
			if (this.items[i].IsEmpty())
			{
				InventoryTransaction inventoryTransaction = new InventoryTransaction().SetStackAbsolute(this, _stack, i);
				InventoryManager.Instance.TransactionRequestLocal(inventoryTransaction);
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002E60 RID: 11872 RVA: 0x001265F0 File Offset: 0x001247F0
	[return: TupleElementNames(new string[] { "success", "stacked", "total" })]
	public ValueTuple<bool, int, int> TryStackItem(int _startIndex, ItemStack _stack)
	{
		if (!this.CheckInventoryReady())
		{
			return new ValueTuple<bool, int, int>(false, 0, _stack.count);
		}
		int count = _stack.count;
		InventoryTransaction inventoryTransaction = new InventoryTransaction();
		for (int i = _startIndex; i < this.items.Length; i++)
		{
			int num;
			if (!this.items[i].CanStackPartlyWith(_stack, out num))
			{
				return new ValueTuple<bool, int, int>(false, 0, count);
			}
			ItemStack itemStack = _stack.Clone();
			itemStack.count = num;
			_stack.count -= num;
			inventoryTransaction.SetStackRelative(this, itemStack, i);
			if (_stack.count == 0)
			{
				break;
			}
		}
		InventoryManager.Instance.TransactionRequestLocal(inventoryTransaction);
		return new ValueTuple<bool, int, int>(true, count - _stack.count, count);
	}

	// Token: 0x06002E61 RID: 11873 RVA: 0x0012669B File Offset: 0x0012489B
	public bool TryClearItem(int _idx)
	{
		return this.TrySetItem(_idx, ItemStack.Empty);
	}

	// Token: 0x06002E62 RID: 11874 RVA: 0x000876FD File Offset: 0x000858FD
	public bool ClearAllItems()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06002E63 RID: 11875 RVA: 0x000876FD File Offset: 0x000858FD
	public void Rollback()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06002E64 RID: 11876 RVA: 0x001266A9 File Offset: 0x001248A9
	public void StartTransaction(Guid _managerToken)
	{
		if (!InventoryManager.Instance.ValidateToken(_managerToken))
		{
			return;
		}
		this.buffer = this.GetItemsReadonly();
	}

	// Token: 0x06002E65 RID: 11877 RVA: 0x001266C5 File Offset: 0x001248C5
	public void FinalizeTransaction(bool _success, Guid _managerToken)
	{
		if (!InventoryManager.Instance.ValidateToken(_managerToken))
		{
			return;
		}
		if (_success)
		{
			this.UpdateInventory(this.buffer, _managerToken);
			this.needsSaving = true;
		}
		this.buffer = null;
	}

	// Token: 0x06002E66 RID: 11878 RVA: 0x001266F4 File Offset: 0x001248F4
	public bool ProcessOperation(InventoryOperation _operation, Guid _managerToken)
	{
		if (!InventoryManager.Instance.ValidateToken(_managerToken))
		{
			return false;
		}
		switch (_operation.Operation)
		{
		case InventoryOperation.EnumOperation.SetAbsolute:
			return this.ProcessSetAbsolute(_operation);
		case InventoryOperation.EnumOperation.SetRelative:
			return this.ProcessSetRelative(_operation);
		case InventoryOperation.EnumOperation.SetAll:
			return this.ProcessSetAll(_operation);
		default:
			return false;
		}
	}

	// Token: 0x06002E67 RID: 11879 RVA: 0x00126744 File Offset: 0x00124944
	[PublicizedFrom(EAccessModifier.Private)]
	public bool ProcessSetAbsolute(InventoryOperation _op)
	{
		if (this.buffer == null || _op.Operation != InventoryOperation.EnumOperation.SetAbsolute)
		{
			return false;
		}
		if (_op.Index >= this.buffer.Length)
		{
			return false;
		}
		if (_op.Stack.count < 0)
		{
			return false;
		}
		this.buffer[_op.Index] = _op.Stack.Clone();
		return true;
	}

	// Token: 0x06002E68 RID: 11880 RVA: 0x001267A0 File Offset: 0x001249A0
	[PublicizedFrom(EAccessModifier.Private)]
	public bool ProcessSetRelative(InventoryOperation _op)
	{
		if (this.buffer == null || _op.Operation != InventoryOperation.EnumOperation.SetRelative)
		{
			return false;
		}
		if (_op.Index >= this.buffer.Length)
		{
			return false;
		}
		ItemStack itemStack = this.buffer[_op.Index];
		if (_op.Stack.count > 0)
		{
			if (!_op.Stack.CanStackWith(itemStack, false))
			{
				return false;
			}
			this.buffer[_op.Index].count = _op.Stack.count + itemStack.count;
		}
		else
		{
			if (!_op.Stack.itemValue.Equals(itemStack.itemValue))
			{
				return false;
			}
			int num = _op.Stack.count + itemStack.count;
			if (num == 0)
			{
				this.buffer[_op.Index] = ItemStack.Empty;
			}
			else
			{
				if (num <= 0)
				{
					return false;
				}
				this.buffer[_op.Index].count = _op.Stack.count + itemStack.count;
			}
		}
		return true;
	}

	// Token: 0x06002E69 RID: 11881 RVA: 0x00126898 File Offset: 0x00124A98
	[PublicizedFrom(EAccessModifier.Private)]
	public bool ProcessSetAll(InventoryOperation _op)
	{
		if (this.buffer == null || _op.NewStacks == null || _op.Operation != InventoryOperation.EnumOperation.SetAll)
		{
			return false;
		}
		if (_op.NewStacks.Length != this.buffer.Length)
		{
			return false;
		}
		for (int i = 0; i < this.buffer.Length; i++)
		{
			if (_op.Stack.count < 0)
			{
				return false;
			}
			this.buffer[i] = _op.NewStacks[i].Clone();
		}
		return true;
	}

	// Token: 0x06002E6A RID: 11882 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public bool CanLockOnServer(int _lockingPlayerID, ILockContext _context, ushort _channel)
	{
		return true;
	}

	// Token: 0x06002E6B RID: 11883 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public bool CanLockLocally(ILockContext _context, ushort _channel)
	{
		return true;
	}

	// Token: 0x06002E6C RID: 11884 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnLockedServer(bool _success, int _lockingPlayerID, ILockContext _context, ushort _channel)
	{
	}

	// Token: 0x06002E6D RID: 11885 RVA: 0x0012690D File Offset: 0x00124B0D
	public void OnLockedLocal(bool _success, ILockContext _context, ushort _channel)
	{
		throw new NotImplementedException("[TransactionalInventory] This class is designed to be locked with the custom UI action RequestInventoryAndShowUI");
	}

	// Token: 0x06002E6E RID: 11886 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnUnlockedServer(int _unlockingPlayerId, ushort _channel)
	{
	}

	// Token: 0x06002E6F RID: 11887 RVA: 0x00126919 File Offset: 0x00124B19
	public void RequestInventoryAndShowUI(bool _success, Action _showUi)
	{
		if (!_success)
		{
			return;
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
		{
			this.DataReadyLocally = false;
			this.inventoryRequestComplete = _showUi;
			InventoryManager.Instance.RequestInventoryFromServer(this.KeyHash);
			return;
		}
		_showUi();
	}

	// Token: 0x040023CE RID: 9166
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack[] items;

	// Token: 0x040023CF RID: 9167
	[PublicizedFrom(EAccessModifier.Private)]
	public Action inventoryRequestComplete;

	// Token: 0x040023D0 RID: 9168
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack[] buffer;

	// Token: 0x040023D1 RID: 9169
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hashDirty = true;

	// Token: 0x040023D2 RID: 9170
	[PublicizedFrom(EAccessModifier.Private)]
	public int hash;

	// Token: 0x040023D3 RID: 9171
	[PublicizedFrom(EAccessModifier.Private)]
	public bool needsSaving = true;

	// Token: 0x02000586 RID: 1414
	public struct KeyHashPair
	{
		// Token: 0x06002E70 RID: 11888 RVA: 0x00126950 File Offset: 0x00124B50
		public KeyHashPair(Guid _key, int _hash)
		{
			this.Key = _key;
			this.Hash = _hash;
		}

		// Token: 0x06002E71 RID: 11889 RVA: 0x00126960 File Offset: 0x00124B60
		public static TransactionalInventory.KeyHashPair Read(BinaryReader _br)
		{
			Guid guid = StreamUtils.ReadGuid(_br);
			int num = _br.ReadInt32();
			return new TransactionalInventory.KeyHashPair(guid, num);
		}

		// Token: 0x06002E72 RID: 11890 RVA: 0x00126980 File Offset: 0x00124B80
		public void Write(BinaryWriter _bw)
		{
			StreamUtils.Write(_bw, this.Key);
			_bw.Write(this.Hash);
		}

		// Token: 0x040023D4 RID: 9172
		public Guid Key;

		// Token: 0x040023D5 RID: 9173
		public int Hash;
	}
}
