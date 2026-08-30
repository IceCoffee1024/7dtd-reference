using System;
using System.IO;
using System.Runtime.CompilerServices;

// Token: 0x0200058A RID: 1418
public class TransactionalInventory : ILockTarget
{
	// Token: 0x170004F8 RID: 1272
	// (get) Token: 0x06002E7A RID: 11898 RVA: 0x001273C7 File Offset: 0x001255C7
	// (set) Token: 0x06002E7B RID: 11899 RVA: 0x001273CF File Offset: 0x001255CF
	public bool DataReadyLocally
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170004F9 RID: 1273
	// (get) Token: 0x06002E7C RID: 11900 RVA: 0x001273D8 File Offset: 0x001255D8
	// (set) Token: 0x06002E7D RID: 11901 RVA: 0x001273E0 File Offset: 0x001255E0
	public Guid Key
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170004FA RID: 1274
	// (get) Token: 0x06002E7E RID: 11902 RVA: 0x001273E9 File Offset: 0x001255E9
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

	// Token: 0x170004FB RID: 1275
	// (get) Token: 0x06002E7F RID: 11903 RVA: 0x00127411 File Offset: 0x00125611
	public TransactionalInventory.KeyHashPair KeyHash
	{
		get
		{
			return new TransactionalInventory.KeyHashPair(this.Key, this.Hash);
		}
	}

	// Token: 0x170004FC RID: 1276
	// (get) Token: 0x06002E80 RID: 11904 RVA: 0x00127424 File Offset: 0x00125624
	public bool NeedsSaving
	{
		get
		{
			return this.needsSaving;
		}
	}

	// Token: 0x170004FD RID: 1277
	// (get) Token: 0x06002E81 RID: 11905 RVA: 0x0012742C File Offset: 0x0012562C
	public int Length
	{
		get
		{
			return this.items.Length;
		}
	}

	// Token: 0x170004FE RID: 1278
	// (get) Token: 0x06002E82 RID: 11906 RVA: 0x00046EDA File Offset: 0x000450DA
	public LockTargetType LockTargetType
	{
		get
		{
			return LockTargetType.TransactionalInventory;
		}
	}

	// Token: 0x06002E83 RID: 11907 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool IsSharedLock(ushort _channel)
	{
		return false;
	}

	// Token: 0x06002E84 RID: 11908 RVA: 0x00127436 File Offset: 0x00125636
	public TransactionalInventory(Guid _key, ItemStack[] _items, Guid _managerToken)
	{
		this.DataReadyLocally = true;
		this.items = _items;
		this.Key = _key;
		InventoryManager.Instance.ValidateToken(_managerToken);
	}

	// Token: 0x06002E85 RID: 11909 RVA: 0x0012746D File Offset: 0x0012566D
	public ItemStack GetItemReadonly(int _slot)
	{
		if (_slot > this.items.Length)
		{
			return null;
		}
		return this.items[_slot].Clone();
	}

	// Token: 0x06002E86 RID: 11910 RVA: 0x00127489 File Offset: 0x00125689
	public ItemStack[] GetItemsReadonly()
	{
		return ItemStack.Clone(this.items);
	}

	// Token: 0x06002E87 RID: 11911 RVA: 0x00127498 File Offset: 0x00125698
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

	// Token: 0x06002E88 RID: 11912 RVA: 0x001274F4 File Offset: 0x001256F4
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

	// Token: 0x06002E89 RID: 11913 RVA: 0x0012757B File Offset: 0x0012577B
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

	// Token: 0x06002E8A RID: 11914 RVA: 0x001275AC File Offset: 0x001257AC
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

	// Token: 0x06002E8B RID: 11915 RVA: 0x001275E8 File Offset: 0x001257E8
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

	// Token: 0x06002E8C RID: 11916 RVA: 0x0012763C File Offset: 0x0012583C
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

	// Token: 0x06002E8D RID: 11917 RVA: 0x00127688 File Offset: 0x00125888
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

	// Token: 0x06002E8E RID: 11918 RVA: 0x001276E0 File Offset: 0x001258E0
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

	// Token: 0x06002E8F RID: 11919 RVA: 0x0012778B File Offset: 0x0012598B
	public bool TryClearItem(int _idx)
	{
		return this.TrySetItem(_idx, ItemStack.Empty);
	}

	// Token: 0x06002E90 RID: 11920 RVA: 0x00087DB8 File Offset: 0x00085FB8
	public bool ClearAllItems()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06002E91 RID: 11921 RVA: 0x00087DB8 File Offset: 0x00085FB8
	public void Rollback()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06002E92 RID: 11922 RVA: 0x00127799 File Offset: 0x00125999
	public void StartTransaction(Guid _managerToken)
	{
		if (!InventoryManager.Instance.ValidateToken(_managerToken))
		{
			return;
		}
		this.buffer = this.GetItemsReadonly();
	}

	// Token: 0x06002E93 RID: 11923 RVA: 0x001277B5 File Offset: 0x001259B5
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

	// Token: 0x06002E94 RID: 11924 RVA: 0x001277E4 File Offset: 0x001259E4
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

	// Token: 0x06002E95 RID: 11925 RVA: 0x00127834 File Offset: 0x00125A34
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

	// Token: 0x06002E96 RID: 11926 RVA: 0x00127890 File Offset: 0x00125A90
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

	// Token: 0x06002E97 RID: 11927 RVA: 0x00127988 File Offset: 0x00125B88
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

	// Token: 0x06002E98 RID: 11928 RVA: 0x00020021 File Offset: 0x0001E221
	public bool CanLockOnServer(int _lockingPlayerID, ILockContext _context, ushort _channel)
	{
		return true;
	}

	// Token: 0x06002E99 RID: 11929 RVA: 0x00020021 File Offset: 0x0001E221
	public bool CanLockLocally(ILockContext _context, ushort _channel)
	{
		return true;
	}

	// Token: 0x06002E9A RID: 11930 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnLockedServer(bool _success, int _lockingPlayerID, ILockContext _context, ushort _channel)
	{
	}

	// Token: 0x06002E9B RID: 11931 RVA: 0x001279FD File Offset: 0x00125BFD
	public void OnLockedLocal(bool _success, ILockContext _context, ushort _channel)
	{
		throw new NotImplementedException("[TransactionalInventory] This class is designed to be locked with the custom UI action RequestInventoryAndShowUI");
	}

	// Token: 0x06002E9C RID: 11932 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnUnlockedServer(int _unlockingPlayerId, ushort _channel)
	{
	}

	// Token: 0x06002E9D RID: 11933 RVA: 0x00127A09 File Offset: 0x00125C09
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

	// Token: 0x04002402 RID: 9218
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack[] items;

	// Token: 0x04002403 RID: 9219
	[PublicizedFrom(EAccessModifier.Private)]
	public Action inventoryRequestComplete;

	// Token: 0x04002404 RID: 9220
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack[] buffer;

	// Token: 0x04002405 RID: 9221
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hashDirty = true;

	// Token: 0x04002406 RID: 9222
	[PublicizedFrom(EAccessModifier.Private)]
	public int hash;

	// Token: 0x04002407 RID: 9223
	[PublicizedFrom(EAccessModifier.Private)]
	public bool needsSaving = true;

	// Token: 0x0200058B RID: 1419
	public struct KeyHashPair
	{
		// Token: 0x06002E9E RID: 11934 RVA: 0x00127A40 File Offset: 0x00125C40
		public KeyHashPair(Guid _key, int _hash)
		{
			this.Key = _key;
			this.Hash = _hash;
		}

		// Token: 0x06002E9F RID: 11935 RVA: 0x00127A50 File Offset: 0x00125C50
		public static TransactionalInventory.KeyHashPair Read(BinaryReader _br)
		{
			Guid guid = StreamUtils.ReadGuid(_br);
			int num = _br.ReadInt32();
			return new TransactionalInventory.KeyHashPair(guid, num);
		}

		// Token: 0x06002EA0 RID: 11936 RVA: 0x00127A70 File Offset: 0x00125C70
		public void Write(BinaryWriter _bw)
		{
			StreamUtils.Write(_bw, this.Key);
			_bw.Write(this.Hash);
		}

		// Token: 0x04002408 RID: 9224
		public Guid Key;

		// Token: 0x04002409 RID: 9225
		public int Hash;
	}
}
