using System;
using System.Collections.Generic;
using AssetManagement;
using UnityEngine;

// Token: 0x02000580 RID: 1408
public sealed class ItemInventoryData
{
	// Token: 0x170004E5 RID: 1253
	// (get) Token: 0x06002DAA RID: 11690 RVA: 0x000A34C7 File Offset: 0x000A16C7
	public World world
	{
		get
		{
			return GameManager.Instance.World;
		}
	}

	// Token: 0x170004E6 RID: 1254
	// (get) Token: 0x06002DAB RID: 11691 RVA: 0x0012367E File Offset: 0x0012187E
	public EntityAlive holdingEntity
	{
		get
		{
			return this.entity;
		}
	}

	// Token: 0x170004E7 RID: 1255
	// (get) Token: 0x06002DAC RID: 11692 RVA: 0x00123686 File Offset: 0x00121886
	public int slotIdx
	{
		get
		{
			return this.toolbeltSlotIdx;
		}
	}

	// Token: 0x170004E8 RID: 1256
	// (get) Token: 0x06002DAD RID: 11693 RVA: 0x0012368E File Offset: 0x0012188E
	public ItemStack itemStack
	{
		get
		{
			return this.stack;
		}
	}

	// Token: 0x170004E9 RID: 1257
	// (get) Token: 0x06002DAE RID: 11694 RVA: 0x00123696 File Offset: 0x00121896
	public ItemValue itemValue
	{
		get
		{
			return this.stack.itemValue;
		}
	}

	// Token: 0x170004EA RID: 1258
	// (get) Token: 0x06002DAF RID: 11695 RVA: 0x001236A3 File Offset: 0x001218A3
	public ItemClass item
	{
		get
		{
			return this.stack.itemValue.ItemClass;
		}
	}

	// Token: 0x170004EB RID: 1259
	// (get) Token: 0x06002DB0 RID: 11696 RVA: 0x001236B5 File Offset: 0x001218B5
	public ItemValue PhysicalItemValue
	{
		get
		{
			return this.physicalItemValue;
		}
	}

	// Token: 0x170004EC RID: 1260
	// (get) Token: 0x06002DB1 RID: 11697 RVA: 0x001236BD File Offset: 0x001218BD
	public Transform model
	{
		get
		{
			if (!(this.heldModel.Instance != null))
			{
				return null;
			}
			return this.heldModel.Instance.transform;
		}
	}

	// Token: 0x170004ED RID: 1261
	// (get) Token: 0x06002DB2 RID: 11698 RVA: 0x001236E4 File Offset: 0x001218E4
	public ItemValue BuiltItemValue
	{
		get
		{
			if (!this.NeedsRebuild)
			{
				return this.stack.itemValue;
			}
			return this.physicalItemValue;
		}
	}

	// Token: 0x170004EE RID: 1262
	// (get) Token: 0x06002DB3 RID: 11699 RVA: 0x00123700 File Offset: 0x00121900
	public bool HasItem
	{
		get
		{
			return !this.stack.IsEmpty();
		}
	}

	// Token: 0x170004EF RID: 1263
	// (get) Token: 0x06002DB4 RID: 11700 RVA: 0x00123710 File Offset: 0x00121910
	// (set) Token: 0x06002DB5 RID: 11701 RVA: 0x00123718 File Offset: 0x00121918
	public bool IsEquipped
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170004F0 RID: 1264
	// (get) Token: 0x06002DB6 RID: 11702 RVA: 0x00123721 File Offset: 0x00121921
	public bool IsBlock
	{
		get
		{
			return this.item != null && this.item.IsBlock();
		}
	}

	// Token: 0x170004F1 RID: 1265
	// (get) Token: 0x06002DB7 RID: 11703 RVA: 0x00123738 File Offset: 0x00121938
	// (set) Token: 0x06002DB8 RID: 11704 RVA: 0x00123781 File Offset: 0x00121981
	public BlockPlacement.EnumPlacement Placement
	{
		get
		{
			BlockPlacement.EnumPlacement enumPlacement = this.placement;
			bool flag = enumPlacement == BlockPlacement.EnumPlacement.Voxel || enumPlacement != BlockPlacement.EnumPlacement.Free || this.item.IsProp();
			bool flag2 = flag;
			if (!LaunchPrefs.EnableProps.Value || !flag2)
			{
				return BlockPlacement.EnumPlacement.Voxel;
			}
			return this.placement;
		}
		set
		{
			this.placement = value;
		}
	}

	// Token: 0x06002DB9 RID: 11705 RVA: 0x0012378A File Offset: 0x0012198A
	public static ItemInventoryData Create(EntityAlive _entity, ItemStack _stack, int _toolbeltSlotIdx)
	{
		return new ItemInventoryData(_entity, _stack, _toolbeltSlotIdx);
	}

	// Token: 0x06002DBA RID: 11706 RVA: 0x00123794 File Offset: 0x00121994
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemInventoryData(EntityAlive _entity, ItemStack _stack, int _toolbeltSlotIdx)
	{
		this.entity = _entity;
		this.stack = _stack ?? ItemStack.Empty;
		this.toolbeltSlotIdx = _toolbeltSlotIdx;
		this.actionData = new List<ItemActionData> { null, null };
	}

	// Token: 0x06002DBB RID: 11707 RVA: 0x001237F8 File Offset: 0x001219F8
	public void SetStack(ItemStack _stack)
	{
		this.stack = _stack ?? ItemStack.Empty;
	}

	// Token: 0x170004F2 RID: 1266
	// (get) Token: 0x06002DBC RID: 11708 RVA: 0x0012380A File Offset: 0x00121A0A
	public bool NeedsRebuild
	{
		get
		{
			return (this.physicalItemValue != null || this.item != null) && (this.item == null || !this.itemValue.EqualsExceptUseTimesAndMeta(this.physicalItemValue));
		}
	}

	// Token: 0x06002DBD RID: 11709 RVA: 0x0012383C File Offset: 0x00121A3C
	public bool Rebuild()
	{
		if (this.IsEquipped || !this.NeedsRebuild)
		{
			this.EnsureActionData();
			return false;
		}
		this.Teardown();
		this.Build();
		return true;
	}

	// Token: 0x06002DBE RID: 11710 RVA: 0x00123864 File Offset: 0x00121A64
	public void EnsureActionData()
	{
		ItemClass item = this.item;
		if (item == null)
		{
			return;
		}
		int num = 0;
		while (num < 3 && num < item.Actions.Length)
		{
			if (item.Actions[num] != null)
			{
				while (this.actionData.Count <= num)
				{
					this.actionData.Add(null);
				}
				List<ItemActionData> list = this.actionData;
				int num2 = num;
				if (list[num2] == null)
				{
					list[num2] = item.Actions[num].CreateModifierData(this, num);
				}
			}
			num++;
		}
	}

	// Token: 0x06002DBF RID: 11711 RVA: 0x001238E4 File Offset: 0x00121AE4
	[PublicizedFrom(EAccessModifier.Private)]
	public void Build()
	{
		this.lastBuildTime = 0f;
		this.rotation = 128;
		this.localRot = 0;
		this.damage = 0;
		this.propTransform = PropTransform.identity;
		this.placement = BlockPlacement.EnumPlacement.Voxel;
		this.mode = BlockPlacement.EnumRotationMode.Simple;
		ItemClass item = this.item;
		if (item == null)
		{
			return;
		}
		this.physicalItemValue = this.itemValue.Clone();
		if (item.IsBlock())
		{
			Block block = Block.list[item.Id];
			if (block.HandleFace != BlockFace.None)
			{
				this.mode = BlockPlacement.EnumRotationMode.ToFace;
			}
			if (block.BlockPlacementHelper != BlockPlacement.None)
			{
				this.mode = BlockPlacement.EnumRotationMode.Auto;
			}
		}
		this.actionData[0] = ((item.Actions[0] != null) ? item.Actions[0].CreateModifierData(this, 0) : null);
		this.actionData[1] = ((item.Actions[1] != null) ? item.Actions[1].CreateModifierData(this, 1) : null);
		if (item.Actions[2] != null)
		{
			this.actionData.Add(item.Actions[2].CreateModifierData(this, 2));
		}
	}

	// Token: 0x06002DC0 RID: 11712 RVA: 0x001239FC File Offset: 0x00121BFC
	public void Teardown()
	{
		if (this.physicalItemValue != null)
		{
			ItemClass itemClass = this.physicalItemValue.ItemClass;
			if (itemClass != null)
			{
				itemClass.CleanupHoldingActions(this);
			}
		}
		this.actionData.Clear();
		this.actionData.Add(null);
		this.actionData.Add(null);
		this.physicalItemValue = null;
		this.DestroyModel();
	}

	// Token: 0x06002DC1 RID: 11713 RVA: 0x00123A58 File Offset: 0x00121C58
	public void OnEquipped()
	{
		this.IsEquipped = true;
		this.BuildModel();
	}

	// Token: 0x06002DC2 RID: 11714 RVA: 0x00123A67 File Offset: 0x00121C67
	public void OnUnequipped()
	{
		this.IsEquipped = false;
	}

	// Token: 0x06002DC3 RID: 11715 RVA: 0x00123A70 File Offset: 0x00121C70
	[PublicizedFrom(EAccessModifier.Private)]
	public void BuildModel()
	{
		if (this.heldModel.Instance != null || this.item == null || !this.item.CanHold())
		{
			return;
		}
		this.heldModel = this.item.CloneModel(this.itemValue, this.entity.InactiveItemTransform, BlockShape.MeshPurpose.Hold);
		if (this.heldModel.Instance != null)
		{
			this.heldModel.Instance.SetActive(false);
		}
		ItemActionData itemActionData = this.actionData[0];
		if (itemActionData != null)
		{
			itemActionData.InitModelTransforms(this);
		}
		ItemActionData itemActionData2 = this.actionData[1];
		if (itemActionData2 != null)
		{
			itemActionData2.InitModelTransforms(this);
		}
	}

	// Token: 0x06002DC4 RID: 11716 RVA: 0x00123B1C File Offset: 0x00121D1C
	[PublicizedFrom(EAccessModifier.Private)]
	public void DestroyModel()
	{
		if (this.heldModel.Instance)
		{
			this.heldModel.Instance.transform.SetParent(null, false);
			this.heldModel.Instance.gameObject.SetActive(false);
			this.heldModel.Destroy();
		}
		this.heldModel = default(InstanceHandle<GameObject>);
	}

	// Token: 0x06002DC5 RID: 11717 RVA: 0x00123B80 File Offset: 0x00121D80
	public void SeatModel()
	{
		Transform model = this.model;
		this.entity.SetHoldingItemTransform(model);
		if (model != null)
		{
			model.position = Vector3.zero;
			model.localPosition = (this.entity.emodel.IsFPV ? Vector3.zero : AnimationGunjointOffsetData.AnimationGunjointOffset[this.item.HoldType.Value].position);
			model.eulerAngles = Vector3.zero;
			model.localEulerAngles = (this.entity.emodel.IsFPV ? Vector3.zero : AnimationGunjointOffsetData.AnimationGunjointOffset[this.item.HoldType.Value].rotation);
			model.localEulerAngles = model.localRotation.eulerAngles;
			if (!this.item.GetCorrectionScale().Equals(Vector3.zero))
			{
				model.localScale = this.item.GetCorrectionScale();
			}
			model.gameObject.SetActive(!this.item.HoldingItemHidden);
		}
		this.entity.ShowHoldingItem(true);
	}

	// Token: 0x06002DC6 RID: 11718 RVA: 0x00123CA4 File Offset: 0x00121EA4
	public void UnseatModel()
	{
		if (this.heldModel.Instance != null)
		{
			this.heldModel.Instance.transform.SetParent(this.entity.InactiveItemTransform, false);
			this.heldModel.Instance.SetActive(false);
		}
	}

	// Token: 0x06002DC7 RID: 11719 RVA: 0x00123CF6 File Offset: 0x00121EF6
	public InstanceHandle<GameObject> CloneModel()
	{
		return this.heldModel.Clone();
	}

	// Token: 0x040023B6 RID: 9142
	public const int NoSlot = -1;

	// Token: 0x040023B7 RID: 9143
	public const byte NoRotation = 128;

	// Token: 0x040023B8 RID: 9144
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly EntityAlive entity;

	// Token: 0x040023B9 RID: 9145
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int toolbeltSlotIdx;

	// Token: 0x040023BA RID: 9146
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack stack;

	// Token: 0x040023BB RID: 9147
	[PublicizedFrom(EAccessModifier.Private)]
	public InstanceHandle<GameObject> heldModel;

	// Token: 0x040023BC RID: 9148
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue physicalItemValue;

	// Token: 0x040023BD RID: 9149
	public List<ItemActionData> actionData;

	// Token: 0x040023BE RID: 9150
	public int holdingEntitySoundID = -2;

	// Token: 0x040023BF RID: 9151
	public float lastBuildTime;

	// Token: 0x040023C0 RID: 9152
	public byte rotation;

	// Token: 0x040023C1 RID: 9153
	public BlockPlacement.EnumRotationMode mode = BlockPlacement.EnumRotationMode.Simple;

	// Token: 0x040023C2 RID: 9154
	public int localRot;

	// Token: 0x040023C3 RID: 9155
	public int damage;

	// Token: 0x040023C4 RID: 9156
	public PropTransform propTransform = PropTransform.identity;

	// Token: 0x040023C5 RID: 9157
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockPlacement.EnumPlacement placement;

	// Token: 0x02000581 RID: 1409
	public enum SoundPlayType
	{
		// Token: 0x040023C8 RID: 9160
		None = -2,
		// Token: 0x040023C9 RID: 9161
		IdleReady,
		// Token: 0x040023CA RID: 9162
		Idle
	}
}
