using System;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020004B6 RID: 1206
[Preserve]
public class EntityLootContainer : EntityItem
{
	// Token: 0x17000447 RID: 1095
	// (get) Token: 0x06002650 RID: 9808 RVA: 0x000EACD6 File Offset: 0x000E8ED6
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

	// Token: 0x06002651 RID: 9809 RVA: 0x000EACFC File Offset: 0x000E8EFC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Start()
	{
		base.Start();
		foreach (Collider collider in base.transform.GetComponentsInChildren<Collider>())
		{
			collider.gameObject.tag = "E_BP_Body";
			collider.enabled = true;
			collider.gameObject.layer = 13;
			collider.gameObject.AddMissingComponent<RootTransformRefEntity>().RootTransform = base.transform;
		}
		this.SetDead();
	}

	// Token: 0x06002652 RID: 9810 RVA: 0x000EAD6B File Offset: 0x000E8F6B
	public void SetContent(ItemStack[] _inventory)
	{
		this.SetContent(_inventory, 0);
	}

	// Token: 0x06002653 RID: 9811 RVA: 0x000EAD78 File Offset: 0x000E8F78
	public void SetContent(ItemStack[] _inventory, int _slotCountOverride)
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
		this.bag.SetSlots(array);
		this.bag.Touched = true;
	}

	// Token: 0x06002654 RID: 9812 RVA: 0x000EAE14 File Offset: 0x000E9014
	public override void CopyPropertiesFromEntityClass()
	{
		base.CopyPropertiesFromEntityClass();
		EntityClass entityClass = EntityClass.list[this.entityClass];
		if (entityClass.Properties.Values.ContainsKey(EntityClass.PropTimeStayAfterDeath))
		{
			this.timeStayAfterDeath = (int)(StringParsers.ParseFloat(entityClass.Properties.Values[EntityClass.PropTimeStayAfterDeath], 0, -1, NumberStyles.Any) * 20f);
			return;
		}
		this.timeStayAfterDeath = 100;
	}

	// Token: 0x06002655 RID: 9813 RVA: 0x000EAE88 File Offset: 0x000E9088
	public override void OnUpdateEntity()
	{
		base.OnUpdateEntity();
		try
		{
			if (!(SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer ? LockManager.Instance.IsLockedServer(this, 0) : LockManager.Instance.IsLockedByLocalPlayer(this, 0)))
			{
				if (this.bag.Touched && this.bag.IsEmpty())
				{
					this.removeBackpack();
				}
				else if (this.deathUpdateTime >= this.timeStayAfterDeath - 1)
				{
					this.removeBackpack();
				}
				else
				{
					this.deathUpdateTime++;
				}
			}
		}
		finally
		{
		}
	}

	// Token: 0x06002656 RID: 9814 RVA: 0x000D978C File Offset: 0x000D798C
	public override bool AllowActivationCommand(ReadOnlySpan<char> _commandName, EntityPlayerLocal _playerFocusing)
	{
		if (base.CommandIs(_commandName, "search"))
		{
			return this.bag != null;
		}
		return base.AllowActivationCommand(_commandName, _playerFocusing);
	}

	// Token: 0x06002657 RID: 9815 RVA: 0x000EAF20 File Offset: 0x000E9120
	[PublicizedFrom(EAccessModifier.Private)]
	public void removeBackpack()
	{
		this.deathUpdateTime = this.timeStayAfterDeath;
		this.bRemoved = true;
		this.MarkToUnload();
	}

	// Token: 0x06002658 RID: 9816 RVA: 0x000EAF3B File Offset: 0x000E913B
	public override int DamageEntity(DamageSource _damageSource, int _strength, bool _criticalHit, float impulseScale = 1f)
	{
		if (_strength >= 99999)
		{
			this.removeBackpack();
		}
		return base.DamageEntity(_damageSource, _strength, _criticalHit, impulseScale);
	}

	// Token: 0x06002659 RID: 9817 RVA: 0x000EAF56 File Offset: 0x000E9156
	public override bool IsMarkedForUnload()
	{
		return base.IsMarkedForUnload() && this.bRemoved;
	}

	// Token: 0x0600265A RID: 9818 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createMesh()
	{
	}

	// Token: 0x0600265B RID: 9819 RVA: 0x000EAF68 File Offset: 0x000E9168
	public override string GetLootList()
	{
		if (!(this.OverrideLootList != ""))
		{
			return this.lootList;
		}
		return this.OverrideLootList;
	}

	// Token: 0x0600265C RID: 9820 RVA: 0x000EAF89 File Offset: 0x000E9189
	public override void Write(BinaryWriter _bw, bool _bNetworkWrite)
	{
		base.Write(_bw, _bNetworkWrite);
		_bw.Write(this.OverrideLootList);
		_bw.Write(this.OverrideName);
		this.bag.Write(_bw);
	}

	// Token: 0x0600265D RID: 9821 RVA: 0x000EAFB8 File Offset: 0x000E91B8
	public override void Read(byte _version, BinaryReader _br)
	{
		base.Read(_version, _br);
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
			this.bag.SetSlots(array);
			return;
		}
		if (_version >= 30)
		{
			this.OverrideLootList = _br.ReadString();
			this.OverrideName = _br.ReadString();
		}
		if (_version >= 35)
		{
			this.bag.ReadInto(_br);
		}
	}

	// Token: 0x0600265E RID: 9822 RVA: 0x000EB0D8 File Offset: 0x000E92D8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void HandleNavObject()
	{
		if (EntityClass.list[this.entityClass].NavObject != "")
		{
			this.NavObject = NavObjectManager.Instance.RegisterNavObject(EntityClass.list[this.entityClass].NavObject, this, "", false);
		}
	}

	// Token: 0x0600265F RID: 9823 RVA: 0x000EB132 File Offset: 0x000E9332
	public override string ToString()
	{
		return string.Format("[type={0}, name={1}]", base.GetType().Name, (this.itemClass != null) ? this.itemClass.GetItemName() : "?");
	}

	// Token: 0x06002660 RID: 9824 RVA: 0x000D9B44 File Offset: 0x000D7D44
	public override void OnUnlockedServer(int _unlockingPlayerId, ushort _channel)
	{
		if (this.bag.IsEmpty())
		{
			this.KillLootContainer();
		}
	}

	// Token: 0x04001C6F RID: 7279
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int deathUpdateTime;

	// Token: 0x04001C70 RID: 7280
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int timeStayAfterDeath;

	// Token: 0x04001C71 RID: 7281
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bRemoved;

	// Token: 0x04001C72 RID: 7282
	public string OverrideLootList = "";

	// Token: 0x04001C73 RID: 7283
	public string OverrideName = "";
}
