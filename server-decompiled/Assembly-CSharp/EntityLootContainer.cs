using System;
using Unity.Profiling;
using UnityEngine.Scripting;

// Token: 0x020004C2 RID: 1218
[Preserve]
public class EntityLootContainer : EntityContainerAbs
{
	// Token: 0x17000441 RID: 1089
	// (get) Token: 0x06002677 RID: 9847 RVA: 0x000EBF9A File Offset: 0x000EA19A
	public override string LocalizedEntityName
	{
		get
		{
			if (!string.IsNullOrEmpty(this.OverrideName))
			{
				return Localization.Get(this.OverrideName, false, null);
			}
			return base.LocalizedEntityName;
		}
	}

	// Token: 0x06002678 RID: 9848 RVA: 0x000EBFBD File Offset: 0x000EA1BD
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Awake()
	{
		this.pmEntityType = new ProfilerMarker("EntityLootContainer");
		base.Awake();
	}

	// Token: 0x06002679 RID: 9849 RVA: 0x000EBFD5 File Offset: 0x000EA1D5
	public override void Write(PooledBinaryWriter _bw, StreamModeWrite _eStreamMode)
	{
		base.Write(_bw, _eStreamMode);
		_bw.Write(this.OverrideLootList);
		_bw.Write(this.OverrideName);
		this.bag.Write(_bw, _eStreamMode);
	}

	// Token: 0x0600267A RID: 9850 RVA: 0x000EC004 File Offset: 0x000EA204
	public override void Read(byte _version, PooledBinaryReader _br, StreamModeRead _eStreamMode)
	{
		base.Read(_version, _br, _eStreamMode);
		if (_version < 35)
		{
			int num = (int)_br.ReadUInt16();
			ItemStack[] array = ItemStack.CreateArray(num);
			for (int i = 0; i < num; i++)
			{
				array[i].Read(_br);
			}
			num = (int)_br.ReadUInt16();
			for (int j = 0; j < num; j++)
			{
				new ItemStack().Read(_br);
			}
			if (_version >= 30)
			{
				this.OverrideLootList = _br.ReadString();
				this.OverrideName = _br.ReadString();
			}
			LootContainer lootContainer = LootContainer.GetLootContainer(this.GetLootList(), true);
			int num2 = array.Length;
			if (lootContainer != null)
			{
				num2 = Math.Max(num2, lootContainer.size.x * lootContainer.size.y);
			}
			if (num2 > array.Length)
			{
				ItemStack[] array2 = ItemStack.CreateArray(num2);
				for (int k = 0; k < array.Length; k++)
				{
					array2[k] = array[k];
				}
				array = array2;
			}
			this.bag.SetSlots(array, true);
			return;
		}
		if (_version >= 30)
		{
			this.OverrideLootList = _br.ReadString();
			this.OverrideName = _br.ReadString();
		}
		if (_version >= 35)
		{
			this.bag.ReadInto(_br, _eStreamMode);
		}
	}

	// Token: 0x0600267B RID: 9851 RVA: 0x000EC127 File Offset: 0x000EA327
	public override string GetLootList()
	{
		if (!(this.OverrideLootList != ""))
		{
			return this.lootList;
		}
		return this.OverrideLootList;
	}

	// Token: 0x0600267C RID: 9852 RVA: 0x000EC148 File Offset: 0x000EA348
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void HandleNavObject()
	{
		if (EntityClass.list[this.entityClass].NavObject != "")
		{
			this.NavObject = NavObjectManager.Instance.RegisterNavObject(EntityClass.list[this.entityClass].NavObject, this, "", false);
		}
	}

	// Token: 0x0600267D RID: 9853 RVA: 0x000EC1A4 File Offset: 0x000EA3A4
	public void SetContent(ItemStack[] _inventory, int _slotCountOverride = 0)
	{
		int num = 0;
		if (_slotCountOverride > 0)
		{
			num = _slotCountOverride;
		}
		else
		{
			LootContainer lootContainer = LootContainer.GetLootContainer(this.GetLootList(), true);
			if (lootContainer != null)
			{
				Vector2i size = lootContainer.size;
				num = size.x * size.y;
			}
			num = Math.Max(num, (_inventory != null) ? _inventory.Length : 0);
		}
		if (num <= 0)
		{
			return;
		}
		ItemStack[] array = ItemStack.CreateArray(num);
		int num2 = 0;
		while (_inventory != null && num2 < _inventory.Length && num2 < array.Length)
		{
			array[num2] = _inventory[num2].Clone();
			num2++;
		}
		this.bag.SetSlots(array, true);
		this.bag.ItemGrid.Touch();
	}

	// Token: 0x0600267E RID: 9854 RVA: 0x000EC242 File Offset: 0x000EA442
	public override int DamageEntity(DamageSource _damageSource, int _strength, bool _criticalHit, float impulseScale = 1f)
	{
		if (_strength >= 99999)
		{
			base.removeBackpack("damaged");
		}
		return base.DamageEntity(_damageSource, _strength, _criticalHit, impulseScale);
	}

	// Token: 0x0600267F RID: 9855 RVA: 0x000EC264 File Offset: 0x000EA464
	public override string ToString()
	{
		return string.Concat(new string[]
		{
			"[type=",
			base.GetType().Name,
			", name=",
			(this.itemClass != null) ? this.itemClass.GetItemName() : "?",
			"]"
		});
	}

	// Token: 0x04001CA1 RID: 7329
	public string OverrideLootList = "";

	// Token: 0x04001CA2 RID: 7330
	public string OverrideName = "";
}
