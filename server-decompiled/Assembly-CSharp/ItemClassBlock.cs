using System;
using AssetManagement;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000602 RID: 1538
[Preserve]
public class ItemClassBlock : ItemClass
{
	// Token: 0x17000544 RID: 1348
	// (get) Token: 0x0600321C RID: 12828 RVA: 0x00145388 File Offset: 0x00143588
	public override bool IsLimitedItem
	{
		get
		{
			return this.GetBlock().IsLimited;
		}
	}

	// Token: 0x0600321D RID: 12829 RVA: 0x00145398 File Offset: 0x00143598
	public ItemClassBlock()
	{
		this.HoldType = new DataItem<int>(7);
		AnimationDelayData.AnimationDelay[this.HoldType.Value] = new AnimationDelayData.AnimationDelays(0, 0f, 0f, 0.31f, 0.31f, true);
		this.Stacknumber = new DataItem<int>(500);
	}

	// Token: 0x0600321E RID: 12830 RVA: 0x001453F8 File Offset: 0x001435F8
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

	// Token: 0x0600321F RID: 12831 RVA: 0x001454A9 File Offset: 0x001436A9
	public override bool IsActionRunning(ItemInventoryData _data)
	{
		return Time.time - _data.lastBuildTime < Constants.cBuildIntervall;
	}

	// Token: 0x06003220 RID: 12832 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsBlock()
	{
		return true;
	}

	// Token: 0x06003221 RID: 12833 RVA: 0x001454BE File Offset: 0x001436BE
	public override bool IsProp()
	{
		return this.GetBlock().IsProp;
	}

	// Token: 0x06003222 RID: 12834 RVA: 0x001454CB File Offset: 0x001436CB
	public override Block GetBlock()
	{
		return Block.list[base.Id];
	}

	// Token: 0x06003223 RID: 12835 RVA: 0x001454D9 File Offset: 0x001436D9
	public override string GetItemName()
	{
		return this.GetBlock().GetBlockName();
	}

	// Token: 0x06003224 RID: 12836 RVA: 0x001454E6 File Offset: 0x001436E6
	public override string GetLocalizedItemName()
	{
		return this.GetBlock().GetLocalizedBlockName();
	}

	// Token: 0x06003225 RID: 12837 RVA: 0x001454F3 File Offset: 0x001436F3
	public override bool HasAnyTags(FastTags<TagGroup.Global> _tags)
	{
		return this.GetBlock().Tags.Test_AnySet(_tags);
	}

	// Token: 0x06003226 RID: 12838 RVA: 0x00145506 File Offset: 0x00143706
	public override bool HasAllTags(FastTags<TagGroup.Global> _tags)
	{
		return this.GetBlock().Tags.Test_AllSet(_tags);
	}

	// Token: 0x06003227 RID: 12839 RVA: 0x0014551C File Offset: 0x0014371C
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

	// Token: 0x06003228 RID: 12840 RVA: 0x0014554C File Offset: 0x0014374C
	public override InstanceHandle<GameObject> CloneModel(ItemValue _itemValue, Transform _parent, BlockShape.MeshPurpose _purpose)
	{
		throw new NotImplementedException("ItemClassBlock does not support CloneModel use CreateMesh instead");
	}

	// Token: 0x06003229 RID: 12841 RVA: 0x00145558 File Offset: 0x00143758
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

	// Token: 0x0600322A RID: 12842 RVA: 0x00145658 File Offset: 0x00143858
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

	// Token: 0x0600322B RID: 12843 RVA: 0x00020119 File Offset: 0x0001E319
	public override ItemClass.EnumCrosshairType GetCrosshairType(ItemInventoryData _holdingData)
	{
		return ItemClass.EnumCrosshairType.Plus;
	}

	// Token: 0x0600322C RID: 12844 RVA: 0x00132DA5 File Offset: 0x00130FA5
	public override RenderCubeType GetFocusType(ItemInventoryData _data)
	{
		return RenderCubeType.FullBlockBothSides;
	}

	// Token: 0x0600322D RID: 12845 RVA: 0x001457DA File Offset: 0x001439DA
	public override float GetFocusRange()
	{
		return Constants.cDigAndBuildDistance;
	}

	// Token: 0x0600322E RID: 12846 RVA: 0x001457E4 File Offset: 0x001439E4
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

	// Token: 0x0600322F RID: 12847 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsFocusBlockInside()
	{
		return false;
	}

	// Token: 0x06003230 RID: 12848 RVA: 0x0004E452 File Offset: 0x0004C652
	public override Vector3 GetDroppedCorrectionRotation()
	{
		return Vector3.zero;
	}

	// Token: 0x06003231 RID: 12849 RVA: 0x0014583A File Offset: 0x00143A3A
	public override Vector3 GetCorrectionRotation()
	{
		return new Vector3(90f, 0f, 0f);
	}

	// Token: 0x06003232 RID: 12850 RVA: 0x0004E452 File Offset: 0x0004C652
	public override Vector3 GetCorrectionPosition()
	{
		return Vector3.zero;
	}

	// Token: 0x06003233 RID: 12851 RVA: 0x00145850 File Offset: 0x00143A50
	public override Vector3 GetCorrectionScale()
	{
		return new Vector3(0.1f, 0.1f, 0.1f);
	}

	// Token: 0x06003234 RID: 12852 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool CanHold()
	{
		return false;
	}

	// Token: 0x04002773 RID: 10099
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Vector3 renderOffsetV = new Vector3(-0.5f, -0.5f, -0.5f);
}
