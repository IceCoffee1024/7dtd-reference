using System;
using System.Security.Cryptography;
using UnityEngine;

// Token: 0x0200019E RID: 414
public abstract class BlockShape
{
	// Token: 0x06000C70 RID: 3184 RVA: 0x0004E3B4 File Offset: 0x0004C5B4
	public BlockShape()
	{
		this.bounds = BoundsUtils.BoundsForMinMax(0f, 0f, 0f, 1f, 1f, 1f);
		this.boundsArr = new Bounds[] { this.bounds };
		this.IsSolidCube = true;
		this.IsSolidSpace = true;
		this.IsRotatable = false;
		this.IsOmitTerrainSnappingUp = false;
		this.LightOpacity = byte.MaxValue;
		this.minBounds = Vector3.zero;
	}

	// Token: 0x06000C71 RID: 3185 RVA: 0x0004E442 File Offset: 0x0004C642
	public virtual void Init(Block _block)
	{
		this.block = _block;
	}

	// Token: 0x06000C72 RID: 3186 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void LateInit()
	{
	}

	// Token: 0x06000C73 RID: 3187 RVA: 0x0004E44B File Offset: 0x0004C64B
	public virtual Quaternion GetRotation(BlockValue _blockValue)
	{
		return Quaternion.identity;
	}

	// Token: 0x06000C74 RID: 3188 RVA: 0x0004E452 File Offset: 0x0004C652
	public virtual Vector3 GetRotationOffset(BlockValue _blockValue)
	{
		return Vector3.zero;
	}

	// Token: 0x06000C75 RID: 3189 RVA: 0x000200DA File Offset: 0x0001E2DA
	public virtual int[][] GetRotationLookup(int _rotation)
	{
		return null;
	}

	// Token: 0x06000C76 RID: 3190 RVA: 0x0004E459 File Offset: 0x0004C659
	public virtual byte Rotate(bool _bLeft, int _rotation)
	{
		return (byte)_rotation;
	}

	// Token: 0x06000C77 RID: 3191 RVA: 0x0004E45D File Offset: 0x0004C65D
	public virtual BlockValue RotateY(bool _bLeft, BlockValue _blockValue, int _rotCount)
	{
		_blockValue.rotation = (byte)(((int)_blockValue.rotation + _rotCount) & 15);
		return _blockValue;
	}

	// Token: 0x06000C78 RID: 3192 RVA: 0x0004E474 File Offset: 0x0004C674
	public virtual BlockValue MirrorY(bool _bAlongZ, BlockValue _blockValue)
	{
		_blockValue = this.RotateY(true, _blockValue, 1);
		return this.RotateY(true, _blockValue, 1);
	}

	// Token: 0x06000C79 RID: 3193 RVA: 0x0004E48A File Offset: 0x0004C68A
	public virtual int getFacesDrawnFullBitfield(BlockValue _blockValue)
	{
		return 255;
	}

	// Token: 0x06000C7A RID: 3194 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool isRenderFace(BlockValue _blockValue, BlockFace _face, BlockValue _adjBlockValue)
	{
		return true;
	}

	// Token: 0x06000C7B RID: 3195 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void renderFace(Vector3i _worldPos, BlockValue _blockValue, Vector3 _drawPos, BlockFace _face, Vector3[] _vertices, LightingAround _lightingAround, TextureFullArray _textureFullArray, VoxelMesh[] _meshes, BlockShape.MeshPurpose _purpose = BlockShape.MeshPurpose.World)
	{
	}

	// Token: 0x06000C7C RID: 3196 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void renderFace(Vector3[] _vertices, LightingAround _lightingAround, long _textureFull, VoxelMesh[] _meshes, Vector2 UVdata, BlockShape.MeshPurpose _purpose = BlockShape.MeshPurpose.World)
	{
	}

	// Token: 0x06000C7D RID: 3197 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void renderFull(Vector3i _worldPos, BlockValue _blockValue, Vector3 _drawPos, Vector3[] _vertices, LightingAround _lightingAround, TextureFullArray _textureFullArray, VoxelMesh[] _meshes, BlockShape.MeshPurpose _purpose = BlockShape.MeshPurpose.World)
	{
	}

	// Token: 0x06000C7E RID: 3198 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsRenderDecoration()
	{
		return false;
	}

	// Token: 0x06000C7F RID: 3199 RVA: 0x0004E494 File Offset: 0x0004C694
	public virtual void renderDecorations(Vector3i _worldPos, BlockValue _blockValue, Vector3 _drawPos, Vector3[] _vertices, LightingAround _lightingAround, TextureFullArray _textureFullArray, VoxelMesh[] _meshes, INeighborBlockCache _nBlocks)
	{
		this.renderFull(_worldPos, _blockValue, _drawPos, _vertices, _lightingAround, _textureFullArray, _meshes, BlockShape.MeshPurpose.World);
	}

	// Token: 0x06000C80 RID: 3200 RVA: 0x0004E4B3 File Offset: 0x0004C6B3
	public virtual int MapSideAndRotationToTextureIdx(BlockValue _blockValue, BlockFace _side)
	{
		return (int)_side;
	}

	// Token: 0x06000C81 RID: 3201 RVA: 0x0004E4B6 File Offset: 0x0004C6B6
	public virtual Bounds[] GetBounds(BlockValue _blockValue)
	{
		return this.boundsArr;
	}

	// Token: 0x06000C82 RID: 3202 RVA: 0x0004E4BE File Offset: 0x0004C6BE
	public virtual Vector2 GetPathOffset(int _rotation)
	{
		return Vector2.zero;
	}

	// Token: 0x06000C83 RID: 3203 RVA: 0x0004E44B File Offset: 0x0004C64B
	public virtual Quaternion GetPreviewRotation()
	{
		return Quaternion.identity;
	}

	// Token: 0x06000C84 RID: 3204 RVA: 0x0004E452 File Offset: 0x0004C652
	public virtual Vector3 GetPreviewPosition()
	{
		return Vector3.zero;
	}

	// Token: 0x06000C85 RID: 3205 RVA: 0x0004E4C5 File Offset: 0x0004C6C5
	public virtual float GetStepHeight(BlockValue blockDef, BlockFace crossingFace)
	{
		return (float)(blockDef.Block.IsCollideMovement ? 1 : 0);
	}

	// Token: 0x06000C86 RID: 3206 RVA: 0x0004E4DA File Offset: 0x0004C6DA
	public virtual bool IsMovementBlocked(BlockValue blockDef, BlockFace crossingFace)
	{
		return this.GetStepHeight(blockDef, crossingFace) > 0.5f;
	}

	// Token: 0x06000C87 RID: 3207 RVA: 0x0004E4EB File Offset: 0x0004C6EB
	public void SetMinAABB(Vector3 _add)
	{
		this.minBounds = _add;
	}

	// Token: 0x06000C88 RID: 3208 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsTerrain()
	{
		return false;
	}

	// Token: 0x06000C89 RID: 3209 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnBlockValueChanged(WorldBase _world, Vector3i _blockPos, BlockValue _oldBlockValue, BlockValue _newBlockValue)
	{
	}

	// Token: 0x06000C8A RID: 3210 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnBlockAdded(WorldBase world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue)
	{
	}

	// Token: 0x06000C8B RID: 3211 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnBlockRemoved(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue)
	{
	}

	// Token: 0x06000C8C RID: 3212 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnBlockEntityTransformBeforeActivated(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, BlockEntityData _ebcd)
	{
	}

	// Token: 0x06000C8D RID: 3213 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnBlockEntityTransformAfterActivated(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, BlockEntityData _ebcd)
	{
	}

	// Token: 0x06000C8E RID: 3214 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnBlockLoaded(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue)
	{
	}

	// Token: 0x06000C8F RID: 3215 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnBlockUnloaded(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue)
	{
	}

	// Token: 0x06000C90 RID: 3216 RVA: 0x000200DA File Offset: 0x0001E2DA
	public virtual VoxelMesh GetBoundsMesh(BlockValue _blockValue)
	{
		return null;
	}

	// Token: 0x06000C91 RID: 3217 RVA: 0x0004E4B3 File Offset: 0x0004C6B3
	public virtual BlockFace GetRotatedBlockFace(BlockValue _blockValue, BlockFace _face)
	{
		return _face;
	}

	// Token: 0x06000C92 RID: 3218 RVA: 0x0004E4F4 File Offset: 0x0004C6F4
	public virtual void MirrorFace(EnumMirrorAlong _axis, int _sourceRot, int _targetRot, BlockFace _face, out BlockFace _sourceFace, out BlockFace _targetFace)
	{
		_sourceFace = _face;
		_targetFace = _face;
	}

	// Token: 0x06000C93 RID: 3219
	public abstract void CalculateCollisionHash(IncrementalHash calcHash);

	// Token: 0x06000C94 RID: 3220 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual int GetVertexCount()
	{
		return 0;
	}

	// Token: 0x06000C95 RID: 3221 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual int GetTriangleCount()
	{
		return 0;
	}

	// Token: 0x06000C96 RID: 3222 RVA: 0x0004E500 File Offset: 0x0004C700
	public virtual string GetName()
	{
		return string.Empty;
	}

	// Token: 0x06000C97 RID: 3223 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool UseRepairDamageState(BlockValue _blockValue)
	{
		return false;
	}

	// Token: 0x04000B38 RID: 2872
	[PublicizedFrom(EAccessModifier.Protected)]
	public static Vector2 uvZero = Vector2.zero;

	// Token: 0x04000B39 RID: 2873
	[PublicizedFrom(EAccessModifier.Protected)]
	public static Vector2 uvOne = Vector2.one;

	// Token: 0x04000B3A RID: 2874
	[PublicizedFrom(EAccessModifier.Protected)]
	public static Vector2 uvRightBot = new Vector2(1f, 0f);

	// Token: 0x04000B3B RID: 2875
	[PublicizedFrom(EAccessModifier.Protected)]
	public static Vector2 uvLeftTop = new Vector2(0f, 1f);

	// Token: 0x04000B3C RID: 2876
	[PublicizedFrom(EAccessModifier.Protected)]
	public static Vector2 uvMiddle = new Vector2(0.5f, 0.5f);

	// Token: 0x04000B3D RID: 2877
	[PublicizedFrom(EAccessModifier.Protected)]
	public static Vector2 uvMidBot = new Vector2(0.5f, 0f);

	// Token: 0x04000B3E RID: 2878
	[PublicizedFrom(EAccessModifier.Protected)]
	public static Vector2 uvMidTop = new Vector2(0.5f, 1f);

	// Token: 0x04000B3F RID: 2879
	[PublicizedFrom(EAccessModifier.Protected)]
	public static Vector2 uvLeftMid = new Vector2(0f, 0.5f);

	// Token: 0x04000B40 RID: 2880
	[PublicizedFrom(EAccessModifier.Protected)]
	public static Vector2 uvRightMid = new Vector2(1f, 0.5f);

	// Token: 0x04000B41 RID: 2881
	[PublicizedFrom(EAccessModifier.Protected)]
	public static Vector4 tngRight = new Vector4(1f, 0f, 0f, 1f);

	// Token: 0x04000B42 RID: 2882
	[PublicizedFrom(EAccessModifier.Protected)]
	public Block block;

	// Token: 0x04000B43 RID: 2883
	[PublicizedFrom(EAccessModifier.Private)]
	public Bounds bounds;

	// Token: 0x04000B44 RID: 2884
	[PublicizedFrom(EAccessModifier.Protected)]
	public Bounds[] boundsArr;

	// Token: 0x04000B45 RID: 2885
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3 minBounds;

	// Token: 0x04000B46 RID: 2886
	public bool IsSolidCube;

	// Token: 0x04000B47 RID: 2887
	public bool IsRotatable;

	// Token: 0x04000B48 RID: 2888
	public bool IsSolidSpace;

	// Token: 0x04000B49 RID: 2889
	public bool IsOmitTerrainSnappingUp;

	// Token: 0x04000B4A RID: 2890
	public bool IsNotifyOnLoadUnload;

	// Token: 0x04000B4B RID: 2891
	public byte LightOpacity;

	// Token: 0x04000B4C RID: 2892
	public int SymmetryType = 1;

	// Token: 0x04000B4D RID: 2893
	public bool Has45DegreeRotations;

	// Token: 0x0200019F RID: 415
	public enum MeshPurpose
	{
		// Token: 0x04000B4F RID: 2895
		World,
		// Token: 0x04000B50 RID: 2896
		Drop,
		// Token: 0x04000B51 RID: 2897
		Hold,
		// Token: 0x04000B52 RID: 2898
		Local,
		// Token: 0x04000B53 RID: 2899
		Preview,
		// Token: 0x04000B54 RID: 2900
		SimplifiedCollisionOnly
	}
}
