using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005F8 RID: 1528
[Preserve]
public class ItemClassBlock : ItemClass
{
	// Token: 0x1700051D RID: 1309
	// (get) Token: 0x060031C5 RID: 12741 RVA: 0x00143CBA File Offset: 0x00141EBA
	public override bool IsLimitedItem
	{
		get
		{
			return this.GetBlock().IsLimited;
		}
	}

	// Token: 0x060031C6 RID: 12742 RVA: 0x00143CC8 File Offset: 0x00141EC8
	public ItemClassBlock()
	{
		this.HoldType = new DataItem<int>(7);
		AnimationDelayData.AnimationDelay[this.HoldType.Value] = new AnimationDelayData.AnimationDelays(0, 0f, 0f, 0.31f, 0.31f, true);
		this.Stacknumber = new DataItem<int>(500);
	}

	// Token: 0x060031C7 RID: 12743 RVA: 0x00143D28 File Offset: 0x00141F28
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

	// Token: 0x060031C8 RID: 12744 RVA: 0x00143DD9 File Offset: 0x00141FD9
	public override bool IsActionRunning(ItemInventoryData _data)
	{
		return Time.time - (_data as ItemClassBlock.ItemBlockInventoryData).lastBuildTime < Constants.cBuildIntervall;
	}

	// Token: 0x060031C9 RID: 12745 RVA: 0x00143DF3 File Offset: 0x00141FF3
	[PublicizedFrom(EAccessModifier.Protected)]
	public override ItemInventoryData createItemInventoryData(ItemStack _itemStack, IGameManager _gameManager, EntityAlive _holdingEntity, int _slotIdx)
	{
		return new ItemClassBlock.ItemBlockInventoryData(this, _itemStack, _gameManager, _holdingEntity, _slotIdx);
	}

	// Token: 0x060031CA RID: 12746 RVA: 0x000027FC File Offset: 0x000009FC
	public override void StopHolding(ItemInventoryData _data, Transform _modelTransform)
	{
	}

	// Token: 0x060031CB RID: 12747 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsBlock()
	{
		return true;
	}

	// Token: 0x060031CC RID: 12748 RVA: 0x00143E00 File Offset: 0x00142000
	public override bool IsProp()
	{
		return this.GetBlock().IsProp;
	}

	// Token: 0x060031CD RID: 12749 RVA: 0x00143E0D File Offset: 0x0014200D
	public override Block GetBlock()
	{
		return Block.list[base.Id];
	}

	// Token: 0x060031CE RID: 12750 RVA: 0x00143E1B File Offset: 0x0014201B
	public override string GetItemName()
	{
		return this.GetBlock().GetBlockName();
	}

	// Token: 0x060031CF RID: 12751 RVA: 0x00143E28 File Offset: 0x00142028
	public override string GetLocalizedItemName()
	{
		return this.GetBlock().GetLocalizedBlockName();
	}

	// Token: 0x060031D0 RID: 12752 RVA: 0x00143E35 File Offset: 0x00142035
	public override bool HasAnyTags(FastTags<TagGroup.Global> _tags)
	{
		return this.GetBlock().Tags.Test_AnySet(_tags);
	}

	// Token: 0x060031D1 RID: 12753 RVA: 0x00143E48 File Offset: 0x00142048
	public override bool HasAllTags(FastTags<TagGroup.Global> _tags)
	{
		return this.GetBlock().Tags.Test_AllSet(_tags);
	}

	// Token: 0x060031D2 RID: 12754 RVA: 0x00143E5C File Offset: 0x0014205C
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

	// Token: 0x060031D3 RID: 12755 RVA: 0x00143E8C File Offset: 0x0014208C
	public override Transform CloneModel(GameObject _go, World _world, BlockValue _blockValue, Vector3[] _vertices, Vector3 _position, Transform _parent, BlockShape.MeshPurpose _purpose, TextureFullArray _textureFullArray = default(TextureFullArray))
	{
		return ItemClassBlock.CreateMesh(_go, _world, _blockValue, _vertices, _position, _parent, _purpose, _textureFullArray);
	}

	// Token: 0x060031D4 RID: 12756 RVA: 0x00143EA0 File Offset: 0x001420A0
	public override Transform CloneModel(World _world, ItemValue _itemValue, Vector3 _position, Transform _parent, BlockShape.MeshPurpose _purpose, TextureFullArray _textureFullArray = default(TextureFullArray))
	{
		return ItemClassBlock.CreateMesh(null, _world, this.GetBlockValueFromItemValue(_itemValue), null, _position, _parent, _purpose, _textureFullArray);
	}

	// Token: 0x060031D5 RID: 12757 RVA: 0x00143EB8 File Offset: 0x001420B8
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

	// Token: 0x060031D6 RID: 12758 RVA: 0x00143FB8 File Offset: 0x001421B8
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

	// Token: 0x060031D7 RID: 12759 RVA: 0x00020021 File Offset: 0x0001E221
	public override ItemClass.EnumCrosshairType GetCrosshairType(ItemInventoryData _holdingData)
	{
		return ItemClass.EnumCrosshairType.Plus;
	}

	// Token: 0x060031D8 RID: 12760 RVA: 0x00131719 File Offset: 0x0012F919
	public override RenderCubeType GetFocusType(ItemInventoryData _data)
	{
		return RenderCubeType.FullBlockBothSides;
	}

	// Token: 0x060031D9 RID: 12761 RVA: 0x0014413A File Offset: 0x0014233A
	public override float GetFocusRange()
	{
		return Constants.cDigAndBuildDistance;
	}

	// Token: 0x060031DA RID: 12762 RVA: 0x00144144 File Offset: 0x00142344
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

	// Token: 0x060031DB RID: 12763 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsFocusBlockInside()
	{
		return false;
	}

	// Token: 0x060031DC RID: 12764 RVA: 0x0004E48E File Offset: 0x0004C68E
	public override Vector3 GetDroppedCorrectionRotation()
	{
		return Vector3.zero;
	}

	// Token: 0x060031DD RID: 12765 RVA: 0x0014419A File Offset: 0x0014239A
	public override Vector3 GetCorrectionRotation()
	{
		return new Vector3(90f, 0f, 0f);
	}

	// Token: 0x060031DE RID: 12766 RVA: 0x0004E48E File Offset: 0x0004C68E
	public override Vector3 GetCorrectionPosition()
	{
		return Vector3.zero;
	}

	// Token: 0x060031DF RID: 12767 RVA: 0x001441B0 File Offset: 0x001423B0
	public override Vector3 GetCorrectionScale()
	{
		return new Vector3(0.1f, 0.1f, 0.1f);
	}

	// Token: 0x060031E0 RID: 12768 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool CanHold()
	{
		return false;
	}

	// Token: 0x04002759 RID: 10073
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte cNoRotation = 128;

	// Token: 0x0400275A RID: 10074
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Vector3 renderOffsetV = new Vector3(-0.5f, -0.5f, -0.5f);

	// Token: 0x020005F9 RID: 1529
	public class ItemBlockInventoryData : ItemInventoryData
	{
		// Token: 0x060031E2 RID: 12770 RVA: 0x001441E4 File Offset: 0x001423E4
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

		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x060031E3 RID: 12771 RVA: 0x0014425C File Offset: 0x0014245C
		// (set) Token: 0x060031E4 RID: 12772 RVA: 0x001442A5 File Offset: 0x001424A5
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

		// Token: 0x0400275B RID: 10075
		public float lastBuildTime;

		// Token: 0x0400275C RID: 10076
		public byte rotation;

		// Token: 0x0400275D RID: 10077
		[PublicizedFrom(EAccessModifier.Private)]
		public BlockPlacement.EnumPlacement m_placement;

		// Token: 0x0400275E RID: 10078
		public BlockPlacement.EnumRotationMode mode = BlockPlacement.EnumRotationMode.Simple;

		// Token: 0x0400275F RID: 10079
		public int localRot;

		// Token: 0x04002760 RID: 10080
		public int damage;

		// Token: 0x04002761 RID: 10081
		public PropTransform propTransform = PropTransform.identity;
	}
}
