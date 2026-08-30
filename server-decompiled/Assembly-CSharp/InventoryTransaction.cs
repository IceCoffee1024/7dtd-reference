using System;
using System.Collections.Generic;
using System.IO;

// Token: 0x02000572 RID: 1394
public class InventoryTransaction
{
	// Token: 0x06002D87 RID: 11655 RVA: 0x00121A07 File Offset: 0x0011FC07
	[PublicizedFrom(EAccessModifier.Private)]
	public void AddOperation(TransactionalInventory _inventory, InventoryOperation _operation)
	{
		if (!this.InventoryOps.ContainsKey(_inventory))
		{
			this.InventoryOps.Add(_inventory, new InventoryTransaction.OperationData(_inventory.Hash));
		}
		this.InventoryOps[_inventory].Ops.Add(_operation);
	}

	// Token: 0x06002D88 RID: 11656 RVA: 0x00121A45 File Offset: 0x0011FC45
	public InventoryTransaction SetStackAbsolute(TransactionalInventory _inventory, ItemStack _stack, int _index)
	{
		if (_inventory == null)
		{
			Log.Warning("[InventoryTransaction] supplied inventory as null.");
			return this;
		}
		if (_stack.count < 0)
		{
			Log.Warning("[InventoryTransaction] stack count cannot be negative when using SetStackAbsolute.");
			return this;
		}
		this.AddOperation(_inventory, InventoryOperation.CreateSetAbsolute(_stack, _index));
		return this;
	}

	// Token: 0x06002D89 RID: 11657 RVA: 0x00121A7A File Offset: 0x0011FC7A
	public InventoryTransaction SetStackRelative(TransactionalInventory _inventory, ItemStack _stack, int _index)
	{
		if (_inventory == null)
		{
			Log.Warning("[InventoryTransaction] supplied inventory as null.");
			return this;
		}
		this.AddOperation(_inventory, InventoryOperation.CreateSetRelative(_stack, _index));
		return this;
	}

	// Token: 0x06002D8A RID: 11658 RVA: 0x00121A9A File Offset: 0x0011FC9A
	public InventoryTransaction SetStacksAll(TransactionalInventory _inventory, ItemStack[] _newStacks)
	{
		if (_inventory == null)
		{
			Log.Warning("[InventoryTransaction] supplied inventory as null.");
			return this;
		}
		this.AddOperation(_inventory, InventoryOperation.CreateSetAll(_newStacks));
		return this;
	}

	// Token: 0x06002D8B RID: 11659 RVA: 0x00121ABC File Offset: 0x0011FCBC
	public bool Apply(Guid managerToken)
	{
		if (this.InventoryOps.Count == 0)
		{
			return false;
		}
		bool flag = true;
		foreach (KeyValuePair<TransactionalInventory, InventoryTransaction.OperationData> keyValuePair in this.InventoryOps)
		{
			TransactionalInventory key = keyValuePair.Key;
			InventoryTransaction.OperationData value = keyValuePair.Value;
			if (key.Hash != value.InitialHash)
			{
				Log.Warning("[InventoryTransaction] transaction failed, an initial hash was out of date.");
				flag = false;
				break;
			}
			key.StartTransaction(managerToken);
		}
		if (flag)
		{
			foreach (KeyValuePair<TransactionalInventory, InventoryTransaction.OperationData> keyValuePair2 in this.InventoryOps)
			{
				TransactionalInventory key2 = keyValuePair2.Key;
				foreach (InventoryOperation inventoryOperation in keyValuePair2.Value.Ops)
				{
					if (!key2.ProcessOperation(inventoryOperation, managerToken))
					{
						flag = false;
						break;
					}
				}
				if (!flag)
				{
					break;
				}
			}
		}
		foreach (KeyValuePair<TransactionalInventory, InventoryTransaction.OperationData> keyValuePair3 in this.InventoryOps)
		{
			TransactionalInventory key3 = keyValuePair3.Key;
			InventoryTransaction.OperationData value2 = keyValuePair3.Value;
			key3.FinalizeTransaction(flag, managerToken);
			if (flag)
			{
				value2.FinalHash = key3.Hash;
			}
		}
		return flag;
	}

	// Token: 0x06002D8C RID: 11660 RVA: 0x00121C5C File Offset: 0x0011FE5C
	public bool ValidateFinalHashes()
	{
		bool flag = true;
		foreach (KeyValuePair<TransactionalInventory, InventoryTransaction.OperationData> keyValuePair in this.InventoryOps)
		{
			TransactionalInventory key = keyValuePair.Key;
			InventoryTransaction.OperationData value = keyValuePair.Value;
			if (value.FinalHash != key.Hash)
			{
				flag = false;
				Log.Error(string.Format("[{0}] transaction failed, final hash was mismatched. Expected: {1}, Acutal: {2}.", "InventoryTransaction", key.Hash, value.FinalHash));
			}
		}
		return flag;
	}

	// Token: 0x06002D8D RID: 11661 RVA: 0x00121CF8 File Offset: 0x0011FEF8
	public static InventoryTransaction Read(BinaryReader _br)
	{
		InventoryTransaction inventoryTransaction = new InventoryTransaction();
		bool flag = false;
		int num = _br.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			Guid guid = StreamUtils.ReadGuid(_br);
			int num2 = _br.ReadInt32();
			int num3 = _br.ReadInt32();
			InventoryTransaction.OperationData operationData = new InventoryTransaction.OperationData(num2)
			{
				FinalHash = num3
			};
			int num4 = _br.ReadInt32();
			for (int j = 0; j < num4; j++)
			{
				operationData.Ops.Add(InventoryOperation.Read(_br));
			}
			TransactionalInventory transactionalInventory;
			if (!InventoryManager.Instance.TryGetTransactionalInventory(guid, out transactionalInventory))
			{
				Log.Error(string.Format("[{0}] Could not find inventory with key {1}", "InventoryTransaction", guid));
				flag = true;
			}
			else
			{
				inventoryTransaction.InventoryOps[transactionalInventory] = operationData;
			}
		}
		if (flag)
		{
			inventoryTransaction.InventoryOps.Clear();
		}
		return inventoryTransaction;
	}

	// Token: 0x06002D8E RID: 11662 RVA: 0x00121DC4 File Offset: 0x0011FFC4
	public static void Write(BinaryWriter _bw, InventoryTransaction tx)
	{
		if (tx == null || tx.InventoryOps == null || tx.InventoryOps.Count == 0)
		{
			_bw.Write(0);
			return;
		}
		_bw.Write(tx.InventoryOps.Count);
		foreach (KeyValuePair<TransactionalInventory, InventoryTransaction.OperationData> keyValuePair in tx.InventoryOps)
		{
			TransactionalInventory key = keyValuePair.Key;
			InventoryTransaction.OperationData value = keyValuePair.Value;
			StreamUtils.Write(_bw, key.Key);
			_bw.Write(value.InitialHash);
			_bw.Write(value.FinalHash);
			_bw.Write(value.Ops.Count);
			foreach (InventoryOperation inventoryOperation in value.Ops)
			{
				inventoryOperation.Write(_bw);
			}
		}
	}

	// Token: 0x0400237B RID: 9083
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<TransactionalInventory, InventoryTransaction.OperationData> InventoryOps = new Dictionary<TransactionalInventory, InventoryTransaction.OperationData>();

	// Token: 0x02000573 RID: 1395
	[PublicizedFrom(EAccessModifier.Private)]
	public class OperationData
	{
		// Token: 0x06002D90 RID: 11664 RVA: 0x00121EE3 File Offset: 0x001200E3
		public OperationData(int _initialHash)
		{
			this.InitialHash = _initialHash;
			this.FinalHash = 0;
			this.Ops = new List<InventoryOperation>();
		}

		// Token: 0x0400237C RID: 9084
		public int InitialHash;

		// Token: 0x0400237D RID: 9085
		public int FinalHash;

		// Token: 0x0400237E RID: 9086
		public List<InventoryOperation> Ops;
	}
}
