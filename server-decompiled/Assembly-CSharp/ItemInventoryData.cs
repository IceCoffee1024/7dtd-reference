using System;
using System.Collections.Generic;
using AssetManagement;
using UnityEngine;

// Token: 0x02000579 RID: 1401
public sealed class ItemInventoryData
{
	// Token: 0x170004E5 RID: 1253
	// (get) Token: 0x06002D9E RID: 11678 RVA: 0x000A32B7 File Offset: 0x000A14B7
	public World world
	{
		get
		{
			return GameManager.Instance.World;
		}
	}

	// Token: 0x170004E6 RID: 1254
	// (get) Token: 0x06002D9F RID: 11679 RVA: 0x00122B02 File Offset: 0x00120D02
	public EntityAlive holdingEntity
	{
		get
		{
			return this.entity;
		}
	}

	// Token: 0x170004E7 RID: 1255
	// (get) Token: 0x06002DA0 RID: 11680 RVA: 0x00122B0A File Offset: 0x00120D0A
	public int slotIdx
	{
		get
		{
			return this.toolbeltSlotIdx;
		}
	}

	// Token: 0x170004E8 RID: 1256
	// (get) Token: 0x06002DA1 RID: 11681 RVA: 0x00122B12 File Offset: 0x00120D12
	public ItemStack itemStack
	{
		get
		{
			return this.stack;
		}
	}

	// Token: 0x170004E9 RID: 1257
	// (get) Token: 0x06002DA2 RID: 11682 RVA: 0x00122B1A File Offset: 0x00120D1A
	public ItemValue itemValue
	{
		get
		{
			return this.stack.itemValue;
		}
	}

	// Token: 0x170004EA RID: 1258
	// (get) Token: 0x06002DA3 RID: 11683 RVA: 0x00122B27 File Offset: 0x00120D27
	public ItemClass item
	{
		get
		{
			return this.stack.itemValue.ItemClass;
		}
	}

	// Token: 0x170004EB RID: 1259
	// (get) Token: 0x06002DA4 RID: 11684 RVA: 0x00122B39 File Offset: 0x00120D39
	public ItemValue PhysicalItemValue
	{
		get
		{
			return this.physicalItemValue;
		}
	}

	// Token: 0x170004EC RID: 1260
	// (get) Token: 0x06002DA5 RID: 11685 RVA: 0x00122B41 File Offset: 0x00120D41
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
	// (get) Token: 0x06002DA6 RID: 11686 RVA: 0x00122B68 File Offset: 0x00120D68
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
	// (get) Token: 0x06002DA7 RID: 11687 RVA: 0x00122B84 File Offset: 0x00120D84
	public bool HasItem
	{
		get
		{
			return !this.stack.IsEmpty();
		}
	}

	// Token: 0x170004EF RID: 1263
	// (get) Token: 0x06002DA8 RID: 11688 RVA: 0x00122B94 File Offset: 0x00120D94
	// (set) Token: 0x06002DA9 RID: 11689 RVA: 0x00122B9C File Offset: 0x00120D9C
	public bool IsEquipped
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170004F0 RID: 1264
	// (get) Token: 0x06002DAA RID: 11690 RVA: 0x00122BA5 File Offset: 0x00120DA5
	public bool IsBlock
	{
		get
		{
			return this.item != null && this.item.IsBlock();
		}
	}

	// Token: 0x170004F1 RID: 1265
	// (get) Token: 0x06002DAB RID: 11691 RVA: 0x00122BBC File Offset: 0x00120DBC
	// (set) Token: 0x06002DAC RID: 11692 RVA: 0x00122C05 File Offset: 0x00120E05
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

	// Token: 0x06002DAD RID: 11693 RVA: 0x00122C0E File Offset: 0x00120E0E
	public static ItemInventoryData Create(EntityAlive _entity, ItemStack _stack, int _toolbeltSlotIdx)
	{
		return new ItemInventoryData(_entity, _stack, _toolbeltSlotIdx);
	}

	// Token: 0x06002DAE RID: 11694 RVA: 0x00122C18 File Offset: 0x00120E18
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemInventoryData(EntityAlive _entity, ItemStack _stack, int _toolbeltSlotIdx)
	{
		this.entity = _entity;
		this.stack = _stack ?? ItemStack.Empty;
		this.toolbeltSlotIdx = _toolbeltSlotIdx;
		this.actionData = new List<ItemActionData> { null, null };
	}

	// Token: 0x06002DAF RID: 11695 RVA: 0x00122C7C File Offset: 0x00120E7C
	public void SetStack(ItemStack _stack)
	{
		this.stack = _stack ?? ItemStack.Empty;
	}

	// Token: 0x170004F2 RID: 1266
	// (get) Token: 0x06002DB0 RID: 11696 RVA: 0x00122C8E File Offset: 0x00120E8E
	public bool NeedsRebuild
	{
		get
		{
			return (this.physicalItemValue != null || this.item != null) && (this.item == null || !this.itemValue.EqualsExceptUseTimesAndMeta(this.physicalItemValue));
		}
	}

	// Token: 0x06002DB1 RID: 11697 RVA: 0x00122CC0 File Offset: 0x00120EC0
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

	// Token: 0x06002DB2 RID: 11698 RVA: 0x00122CE8 File Offset: 0x00120EE8
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

	// Token: 0x06002DB3 RID: 11699 RVA: 0x00122D68 File Offset: 0x00120F68
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

	// Token: 0x06002DB4 RID: 11700 RVA: 0x00122E80 File Offset: 0x00121080
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

	// Token: 0x06002DB5 RID: 11701 RVA: 0x00122EDC File Offset: 0x001210DC
	public void OnEquipped()
	{
		this.IsEquipped = true;
		this.BuildModel();
	}

	// Token: 0x06002DB6 RID: 11702 RVA: 0x00122EEB File Offset: 0x001210EB
	public void OnUnequipped()
	{
		this.IsEquipped = false;
	}

	// Token: 0x06002DB7 RID: 11703 RVA: 0x00122EF4 File Offset: 0x001210F4
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

	// Token: 0x06002DB8 RID: 11704 RVA: 0x00122FA0 File Offset: 0x001211A0
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

	// Token: 0x06002DB9 RID: 11705 RVA: 0x00123004 File Offset: 0x00121204
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

	// Token: 0x06002DBA RID: 11706 RVA: 0x00123128 File Offset: 0x00121328
	public void UnseatModel()
	{
		if (this.heldModel.Instance != null)
		{
			this.heldModel.Instance.transform.SetParent(this.entity.InactiveItemTransform, false);
			this.heldModel.Instance.SetActive(false);
		}
	}

	// Token: 0x06002DBB RID: 11707 RVA: 0x0012317A File Offset: 0x0012137A
	public InstanceHandle<GameObject> CloneModel()
	{
		return this.heldModel.Clone();
	}

	// Token: 0x0400239B RID: 9115
	public const int NoSlot = -1;

	// Token: 0x0400239C RID: 9116
	public const byte NoRotation = 128;

	// Token: 0x0400239D RID: 9117
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly EntityAlive entity;

	// Token: 0x0400239E RID: 9118
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int toolbeltSlotIdx;

	// Token: 0x0400239F RID: 9119
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack stack;

	// Token: 0x040023A0 RID: 9120
	[PublicizedFrom(EAccessModifier.Private)]
	public InstanceHandle<GameObject> heldModel;

	// Token: 0x040023A1 RID: 9121
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue physicalItemValue;

	// Token: 0x040023A2 RID: 9122
	public List<ItemActionData> actionData;

	// Token: 0x040023A3 RID: 9123
	public int holdingEntitySoundID = -2;

	// Token: 0x040023A4 RID: 9124
	public float lastBuildTime;

	// Token: 0x040023A5 RID: 9125
	public byte rotation;

	// Token: 0x040023A6 RID: 9126
	public BlockPlacement.EnumRotationMode mode = BlockPlacement.EnumRotationMode.Simple;

	// Token: 0x040023A7 RID: 9127
	public int localRot;

	// Token: 0x040023A8 RID: 9128
	public int damage;

	// Token: 0x040023A9 RID: 9129
	public PropTransform propTransform = PropTransform.identity;

	// Token: 0x040023AA RID: 9130
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockPlacement.EnumPlacement placement;

	// Token: 0x0200057A RID: 1402
	public enum SoundPlayType
	{
		// Token: 0x040023AD RID: 9133
		None = -2,
		// Token: 0x040023AE RID: 9134
		IdleReady,
		// Token: 0x040023AF RID: 9135
		Idle
	}
}
