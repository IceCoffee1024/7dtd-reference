using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005F3 RID: 1523
[Preserve]
public class ItemClassBlock : ItemClass
{
	// Token: 0x17000516 RID: 1302
	// (get) Token: 0x06003195 RID: 12693 RVA: 0x00142BEA File Offset: 0x00140DEA
	public override bool IsLimitedItem
	{
		get
		{
			return this.GetBlock().IsLimited;
		}
	}

	// Token: 0x06003196 RID: 12694 RVA: 0x00142BF8 File Offset: 0x00140DF8
	public ItemClassBlock()
	{
		this.HoldType = new DataItem<int>(7);
		AnimationDelayData.AnimationDelay[this.HoldType.Value] = new AnimationDelayData.AnimationDelays(0, 0f, 0f, 0.31f, 0.31f, true);
		this.Stacknumber = new DataItem<int>(500);
	}

	// Token: 0x06003197 RID: 12695 RVA: 0x00142C58 File Offset: 0x00140E58
	public override void Init()
	{
		base.Init();
		Block block = Block.list[base.Id];
		this.DescriptionKey = block.DescriptionKey;
		this.MadeOfMaterial = block.blockMaterial;
		if (block.CustomIcon != null)
		{
			this.CustomIcon = new DataItem<string>(block.CustomIcon);
		}
		this.NoScrapping = block.NoScrapping;
		this.CustomIconTint = block.CustomIconTint;
		this.SortOrder = block.SortOrder;
		this.CreativeMode = block.CreativeMode;
		this.TraderStageTemplate = block.TraderStageTemplate;
		this.SoundPickup = block.SoundPickup;
		this.SoundPlace = block.SoundPlace;
		this.IsLimited = block.IsLimited;
	}

	// Token: 0x06003198 RID: 12696 RVA: 0x00142D09 File Offset: 0x00140F09
	public override bool IsActionRunning(ItemInventoryData _data)
	{
		return Time.time - (_data as ItemClassBlock.ItemBlockInventoryData).lastBuildTime < Constants.cBuildIntervall;
	}

	// Token: 0x06003199 RID: 12697 RVA: 0x00142D23 File Offset: 0x00140F23
	[PublicizedFrom(EAccessModifier.Protected)]
	public override ItemInventoryData createItemInventoryData(ItemStack _itemStack, IGameManager _gameManager, EntityAlive _holdingEntity, int _slotIdx)
	{
		return new ItemClassBlock.ItemBlockInventoryData(this, _itemStack, _gameManager, _holdingEntity, _slotIdx);
	}

	// Token: 0x0600319A RID: 12698 RVA: 0x000027FC File Offset: 0x000009FC
	public override void StopHolding(ItemInventoryData _data, Transform _modelTransform)
	{
	}

	// Token: 0x0600319B RID: 12699 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsBlock()
	{
		return true;
	}

	// Token: 0x0600319C RID: 12700 RVA: 0x00142D30 File Offset: 0x00140F30
	public override bool IsProp()
	{
		return this.GetBlock().IsProp;
	}

	// Token: 0x0600319D RID: 12701 RVA: 0x00142D3D File Offset: 0x00140F3D
	public override Block GetBlock()
	{
		return Block.list[base.Id];
	}

	// Token: 0x0600319E RID: 12702 RVA: 0x00142D4B File Offset: 0x00140F4B
	public override string GetItemName()
	{
		return this.GetBlock().GetBlockName();
	}

	// Token: 0x0600319F RID: 12703 RVA: 0x00142D58 File Offset: 0x00140F58
	public override string GetLocalizedItemName()
	{
		return this.GetBlock().GetLocalizedBlockName();
	}

	// Token: 0x060031A0 RID: 12704 RVA: 0x00142D65 File Offset: 0x00140F65
	public override bool HasAnyTags(FastTags<TagGroup.Global> _tags)
	{
		return this.GetBlock().Tags.Test_AnySet(_tags);
	}

	// Token: 0x060031A1 RID: 12705 RVA: 0x00142D78 File Offset: 0x00140F78
	public override bool HasAllTags(FastTags<TagGroup.Global> _tags)
	{
		return this.GetBlock().Tags.Test_AllSet(_tags);
	}

	// Token: 0x060031A2 RID: 12706 RVA: 0x00142D8C File Offset: 0x00140F8C
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockValue GetBlockValueFromItemValue(ItemValue _itemValue)
	{
		Block block = this.GetBlock();
		if (block.SelectAlternates)
		{
			return block.GetAltBlockValue(_itemValue.Meta);
		}
		return _itemValue.ToBlockValue(false);
	}

	// Token: 0x060031A3 RID: 12707 RVA: 0x00142DBC File Offset: 0x00140FBC
	public override Transform CloneModel(GameObject _go, World _world, BlockValue _blockValue, Vector3[] _vertices, Vector3 _position, Transform _parent, BlockShape.MeshPurpose _purpose, TextureFullArray _textureFullArray = default(TextureFullArray))
	{
		return ItemClassBlock.CreateMesh(_go, _world, _blockValue, _vertices, _position, _parent, _purpose, _textureFullArray);
	}

	// Token: 0x060031A4 RID: 12708 RVA: 0x00142DD0 File Offset: 0x00140FD0
	public override Transform CloneModel(World _world, ItemValue _itemValue, Vector3 _position, Transform _parent, BlockShape.MeshPurpose _purpose, TextureFullArray _textureFullArray = default(TextureFullArray))
	{
		return ItemClassBlock.CreateMesh(null, _world, this.GetBlockValueFromItemValue(_itemValue), null, _position, _parent, _purpose, _textureFullArray);
	}

	// Token: 0x060031A5 RID: 12709 RVA: 0x00142DE8 File Offset: 0x00140FE8
	public static Transform CreateMesh(GameObject _go, World _world, BlockValue _blockValue, Vector3[] _vertices, Vector3 _worldPos, Transform _parent, BlockShape.MeshPurpose _purpose, TextureFullArray _textureFullArray = default(TextureFullArray))
	{
		if (_purpose == BlockShape.MeshPurpose.Drop)
		{
			GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(LoadManager.LoadAsset<GameObject>("@:Other/Items/Misc/sack_droppedPrefab.prefab", null, null, false, true, false).Asset);
			Transform transform = gameObject.transform;
			transform.SetParent(_parent, false);
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			return gameObject.transform;
		}
		Block block = _blockValue.Block;
		BlockShapeModelEntity blockShapeModelEntity = block.shape as BlockShapeModelEntity;
		if (blockShapeModelEntity != null)
		{
			if (_go == null)
			{
				_go = new GameObject();
				_go.transform.SetParent(_parent, false);
			}
			blockShapeModelEntity.CloneModel(_blockValue, _go.transform);
			return _go.transform;
		}
		int meshIndex = (int)block.MeshIndex;
		Transform transform2 = ItemClassBlock.CreateMeshOfType(_go, _world, _blockValue, _vertices, _worldPos, _parent, _purpose, _textureFullArray, meshIndex);
		if (meshIndex == 0 && (_purpose == BlockShape.MeshPurpose.Preview || _purpose == BlockShape.MeshPurpose.Local || _purpose == BlockShape.MeshPurpose.SimplifiedCollisionOnly))
		{
			Transform transform3 = ItemClassBlock.CreateMeshOfType(_go, _world, _blockValue, _vertices, _worldPos, _parent, _purpose, _textureFullArray, 2);
			if (transform3)
			{
				if (transform2)
				{
					transform3.SetParent(transform2, false);
				}
				else
				{
					transform2 = transform3;
				}
			}
		}
		return transform2;
	}

	// Token: 0x060031A6 RID: 12710 RVA: 0x00142EE8 File Offset: 0x001410E8
	public static Transform CreateMeshOfType(GameObject _go, World _world, BlockValue _blockValue, Vector3[] _vertices, Vector3 _worldPos, Transform _parent, BlockShape.MeshPurpose _purpose, TextureFullArray _textureFullArray, int _meshIndex)
	{
		Vector3i vector3i = World.worldToBlockPos(_worldPos);
		byte b;
		byte b2;
		_world.GetSunAndBlockColors(vector3i, out b, out b2);
		VoxelMesh voxelMesh = VoxelMesh.Create(_meshIndex, MeshDescription.meshes[_meshIndex].meshType, 1);
		VoxelMesh[] array = new VoxelMesh[MeshDescription.meshes.Length];
		array[_meshIndex] = voxelMesh;
		Block block = _blockValue.Block;
		block.shape.renderFull(vector3i, _blockValue, ItemClassBlock.renderOffsetV, _vertices, new LightingAround(b, b2, 0), _textureFullArray, array, _purpose);
		if (_purpose == BlockShape.MeshPurpose.SimplifiedCollisionOnly)
		{
			block.shape.renderFull(vector3i, _blockValue, ItemClassBlock.renderOffsetV, _vertices, new LightingAround(b, b2, 0), _textureFullArray, array, _purpose);
			if (!_go)
			{
				_go = new GameObject();
				_go.transform.SetParent(_parent, false);
			}
			_go.name = "Block_" + _blockValue.type.ToString();
			MeshCollider orAddComponent = _go.GetOrAddComponent<MeshCollider>();
			Mesh mesh;
			voxelMesh.CopyToColliders(orAddComponent, out mesh);
			orAddComponent.sharedMesh = mesh;
			return _go.transform;
		}
		if (voxelMesh.m_Vertices.Count == 0)
		{
			return null;
		}
		if (!_go)
		{
			_go = new GameObject();
			_go.transform.SetParent(_parent, false);
			_go.AddComponent<UpdateLightOnChunkMesh>();
		}
		_go.name = "Block_" + _blockValue.type.ToString();
		MeshFilter meshFilter;
		MeshRenderer meshRenderer;
		VoxelMesh.CreateMeshFilter(_meshIndex, 0, _go, "Item", false, out meshFilter, out meshRenderer);
		if (meshFilter != null)
		{
			voxelMesh.CopyToMesh(meshFilter, meshRenderer, 0, null);
		}
		return _go.transform;
	}

	// Token: 0x060031A7 RID: 12711 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override ItemClass.EnumCrosshairType GetCrosshairType(ItemInventoryData _holdingData)
	{
		return ItemClass.EnumCrosshairType.Plus;
	}

	// Token: 0x060031A8 RID: 12712 RVA: 0x001306A9 File Offset: 0x0012E8A9
	public override RenderCubeType GetFocusType(ItemInventoryData _data)
	{
		return RenderCubeType.FullBlockBothSides;
	}

	// Token: 0x060031A9 RID: 12713 RVA: 0x0014306A File Offset: 0x0014126A
	public override float GetFocusRange()
	{
		return Constants.cDigAndBuildDistance;
	}

	// Token: 0x060031AA RID: 12714 RVA: 0x00143074 File Offset: 0x00141274
	public override void ExecuteAction(int _actionIdx, ItemInventoryData _data, bool _bReleased, PlayerActionsLocal _playerActions)
	{
		EntityPlayerLocal entityPlayerLocal = _data.holdingEntity as EntityPlayerLocal;
		if (entityPlayerLocal != null && !entityPlayerLocal.bFirstPersonView)
		{
			entityPlayerLocal.StartTPCameraLockTimer();
		}
		if (_actionIdx == 0)
		{
			GameManager.Instance.GetActiveBlockTool().ExecuteAttackAction(_data, _bReleased, _playerActions);
			return;
		}
		GameManager.Instance.GetActiveBlockTool().ExecuteUseAction(_data, _bReleased, _playerActions);
	}

	// Token: 0x060031AB RID: 12715 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsFocusBlockInside()
	{
		return false;
	}

	// Token: 0x060031AC RID: 12716 RVA: 0x0004E076 File Offset: 0x0004C276
	public override Vector3 GetDroppedCorrectionRotation()
	{
		return Vector3.zero;
	}

	// Token: 0x060031AD RID: 12717 RVA: 0x001430CA File Offset: 0x001412CA
	public override Vector3 GetCorrectionRotation()
	{
		return new Vector3(90f, 0f, 0f);
	}

	// Token: 0x060031AE RID: 12718 RVA: 0x0004E076 File Offset: 0x0004C276
	public override Vector3 GetCorrectionPosition()
	{
		return Vector3.zero;
	}

	// Token: 0x060031AF RID: 12719 RVA: 0x001430E0 File Offset: 0x001412E0
	public override Vector3 GetCorrectionScale()
	{
		return new Vector3(0.1f, 0.1f, 0.1f);
	}

	// Token: 0x060031B0 RID: 12720 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool CanHold()
	{
		return false;
	}

	// Token: 0x04002723 RID: 10019
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte cNoRotation = 128;

	// Token: 0x04002724 RID: 10020
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Vector3 renderOffsetV = new Vector3(-0.5f, -0.5f, -0.5f);

	// Token: 0x020005F4 RID: 1524
	public class ItemBlockInventoryData : ItemInventoryData
	{
		// Token: 0x060031B2 RID: 12722 RVA: 0x00143114 File Offset: 0x00141314
		public ItemBlockInventoryData(ItemClass _item, ItemStack _itemStack, IGameManager _gameManager, EntityAlive _holdingEntity, int _slotIdx)
			: base(_item, _itemStack, _gameManager, _holdingEntity, _slotIdx)
		{
			this.lastBuildTime = 0f;
			this.rotation = 128;
			Block block = Block.list[_item.Id];
			if (block.HandleFace != BlockFace.None)
			{
				this.mode = BlockPlacement.EnumRotationMode.ToFace;
			}
			if (block.BlockPlacementHelper != BlockPlacement.None)
			{
				this.mode = BlockPlacement.EnumRotationMode.Auto;
			}
		}

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x060031B3 RID: 12723 RVA: 0x0014318C File Offset: 0x0014138C
		// (set) Token: 0x060031B4 RID: 12724 RVA: 0x001431D5 File Offset: 0x001413D5
		public BlockPlacement.EnumPlacement Placement
		{
			get
			{
				BlockPlacement.EnumPlacement placement = this.m_placement;
				bool flag = placement == BlockPlacement.EnumPlacement.Voxel || placement != BlockPlacement.EnumPlacement.Free || this.item.IsProp();
				bool flag2 = flag;
				if (!LaunchPrefs.EnableProps.Value || !flag2)
				{
					return BlockPlacement.EnumPlacement.Voxel;
				}
				return this.m_placement;
			}
			set
			{
				this.m_placement = value;
			}
		}

		// Token: 0x04002725 RID: 10021
		public float lastBuildTime;

		// Token: 0x04002726 RID: 10022
		public byte rotation;

		// Token: 0x04002727 RID: 10023
		[PublicizedFrom(EAccessModifier.Private)]
		public BlockPlacement.EnumPlacement m_placement;

		// Token: 0x04002728 RID: 10024
		public BlockPlacement.EnumRotationMode mode = BlockPlacement.EnumRotationMode.Simple;

		// Token: 0x04002729 RID: 10025
		public int localRot;

		// Token: 0x0400272A RID: 10026
		public int damage;

		// Token: 0x0400272B RID: 10027
		public PropTransform propTransform = PropTransform.identity;
	}
}
