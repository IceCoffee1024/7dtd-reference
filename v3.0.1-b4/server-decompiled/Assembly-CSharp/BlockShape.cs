using System;
using System.Security.Cryptography;
using UnityEngine;

// Token: 0x0200019B RID: 411
public abstract class BlockShape
{
	// Token: 0x06000C6E RID: 3182 RVA: 0x0004DFD8 File Offset: 0x0004C1D8
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

	// Token: 0x06000C6F RID: 3183 RVA: 0x0004E066 File Offset: 0x0004C266
	public virtual void Init(Block _block)
	{
		this.block = _block;
	}

	// Token: 0x06000C70 RID: 3184 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void LateInit()
	{
	}

	// Token: 0x06000C71 RID: 3185 RVA: 0x0004E06F File Offset: 0x0004C26F
	public virtual Quaternion GetRotation(BlockValue _blockValue)
	{
		return Quaternion.identity;
	}

	// Token: 0x06000C72 RID: 3186 RVA: 0x0004E076 File Offset: 0x0004C276
	public virtual Vector3 GetRotationOffset(BlockValue _blockValue)
	{
		return Vector3.zero;
	}

	// Token: 0x06000C73 RID: 3187 RVA: 0x0001FF1E File Offset: 0x0001E11E
	public virtual int[][] GetRotationLookup(int _rotation)
	{
		return null;
	}

	// Token: 0x06000C74 RID: 3188 RVA: 0x0004E07D File Offset: 0x0004C27D
	public virtual byte Rotate(bool _bLeft, int _rotation)
	{
		return (byte)_rotation;
	}

	// Token: 0x06000C75 RID: 3189 RVA: 0x0004E081 File Offset: 0x0004C281
	public virtual BlockValue RotateY(bool _bLeft, BlockValue _blockValue, int _rotCount)
	{
		_blockValue.rotation = (byte)(((int)_blockValue.rotation + _rotCount) & 15);
		return _blockValue;
	}

	// Token: 0x06000C76 RID: 3190 RVA: 0x0004E098 File Offset: 0x0004C298
	public virtual BlockValue MirrorY(bool _bAlongZ, BlockValue _blockValue)
	{
		_blockValue = this.RotateY(true, _blockValue, 1);
		return this.RotateY(true, _blockValue, 1);
	}

	// Token: 0x06000C77 RID: 3191 RVA: 0x0004E0AE File Offset: 0x0004C2AE
	public virtual int getFacesDrawnFullBitfield(BlockValue _blockValue)
	{
		return 255;
	}

	// Token: 0x06000C78 RID: 3192 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public virtual bool isRenderFace(BlockValue _blockValue, BlockFace _face, BlockValue _adjBlockValue)
	{
		return true;
	}

	// Token: 0x06000C79 RID: 3193 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void renderFace(Vector3i _worldPos, BlockValue _blockValue, Vector3 _drawPos, BlockFace _face, Vector3[] _vertices, LightingAround _lightingAround, TextureFullArray _textureFullArray, VoxelMesh[] _meshes, BlockShape.MeshPurpose _purpose = BlockShape.MeshPurpose.World)
	{
	}

	// Token: 0x06000C7A RID: 3194 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void renderFace(Vector3[] _vertices, LightingAround _lightingAround, long _textureFull, VoxelMesh[] _meshes, Vector2 UVdata, BlockShape.MeshPurpose _purpose = BlockShape.MeshPurpose.World)
	{
	}

	// Token: 0x06000C7B RID: 3195 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void renderFull(Vector3i _worldPos, BlockValue _blockValue, Vector3 _drawPos, Vector3[] _vertices, LightingAround _lightingAround, TextureFullArray _textureFullArray, VoxelMesh[] _meshes, BlockShape.MeshPurpose _purpose = BlockShape.MeshPurpose.World)
	{
	}

	// Token: 0x06000C7C RID: 3196 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsRenderDecoration()
	{
		return false;
	}

	// Token: 0x06000C7D RID: 3197 RVA: 0x0004E0B8 File Offset: 0x0004C2B8
	public virtual void renderDecorations(Vector3i _worldPos, BlockValue _blockValue, Vector3 _drawPos, Vector3[] _vertices, LightingAround _lightingAround, TextureFullArray _textureFullArray, VoxelMesh[] _meshes, INeighborBlockCache _nBlocks)
	{
		this.renderFull(_worldPos, _blockValue, _drawPos, _vertices, _lightingAround, _textureFullArray, _meshes, BlockShape.MeshPurpose.World);
	}

	// Token: 0x06000C7E RID: 3198 RVA: 0x0004E0D7 File Offset: 0x0004C2D7
	public virtual int MapSideAndRotationToTextureIdx(BlockValue _blockValue, BlockFace _side)
	{
		return (int)_side;
	}

	// Token: 0x06000C7F RID: 3199 RVA: 0x0004E0DA File Offset: 0x0004C2DA
	public virtual Bounds[] GetBounds(BlockValue _blockValue)
	{
		return this.boundsArr;
	}

	// Token: 0x06000C80 RID: 3200 RVA: 0x0004E0E2 File Offset: 0x0004C2E2
	public virtual Vector2 GetPathOffset(int _rotation)
	{
		return Vector2.zero;
	}

	// Token: 0x06000C81 RID: 3201 RVA: 0x0004E06F File Offset: 0x0004C26F
	public virtual Quaternion GetPreviewRotation()
	{
		return Quaternion.identity;
	}

	// Token: 0x06000C82 RID: 3202 RVA: 0x0004E076 File Offset: 0x0004C276
	public virtual Vector3 GetPreviewPosition()
	{
		return Vector3.zero;
	}

	// Token: 0x06000C83 RID: 3203 RVA: 0x0004E0E9 File Offset: 0x0004C2E9
	public virtual float GetStepHeight(BlockValue blockDef, BlockFace crossingFace)
	{
		return (float)(blockDef.Block.IsCollideMovement ? 1 : 0);
	}

	// Token: 0x06000C84 RID: 3204 RVA: 0x0004E0FE File Offset: 0x0004C2FE
	public virtual bool IsMovementBlocked(BlockValue blockDef, BlockFace crossingFace)
	{
		return this.GetStepHeight(blockDef, crossingFace) > 0.5f;
	}

	// Token: 0x06000C85 RID: 3205 RVA: 0x0004E10F File Offset: 0x0004C30F
	public void SetMinAABB(Vector3 _add)
	{
		this.minBounds = _add;
	}

	// Token: 0x06000C86 RID: 3206 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsTerrain()
	{
		return false;
	}

	// Token: 0x06000C87 RID: 3207 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnBlockValueChanged(WorldBase _world, Vector3i _blockPos, BlockValue _oldBlockValue, BlockValue _newBlockValue)
	{
	}

	// Token: 0x06000C88 RID: 3208 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnBlockAdded(WorldBase world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue)
	{
	}

	// Token: 0x06000C89 RID: 3209 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnBlockRemoved(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue)
	{
	}

	// Token: 0x06000C8A RID: 3210 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnBlockEntityTransformBeforeActivated(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, BlockEntityData _ebcd)
	{
	}

	// Token: 0x06000C8B RID: 3211 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnBlockEntityTransformAfterActivated(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, BlockEntityData _ebcd)
	{
	}

	// Token: 0x06000C8C RID: 3212 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnBlockLoaded(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue)
	{
	}

	// Token: 0x06000C8D RID: 3213 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnBlockUnloaded(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue)
	{
	}

	// Token: 0x06000C8E RID: 3214 RVA: 0x0001FF1E File Offset: 0x0001E11E
	public virtual VoxelMesh GetBoundsMesh(BlockValue _blockValue)
	{
		return null;
	}

	// Token: 0x06000C8F RID: 3215 RVA: 0x0004E0D7 File Offset: 0x0004C2D7
	public virtual BlockFace GetRotatedBlockFace(BlockValue _blockValue, BlockFace _face)
	{
		return _face;
	}

	// Token: 0x06000C90 RID: 3216 RVA: 0x0004E118 File Offset: 0x0004C318
	public virtual void MirrorFace(EnumMirrorAlong _axis, int _sourceRot, int _targetRot, BlockFace _face, out BlockFace _sourceFace, out BlockFace _targetFace)
	{
		_sourceFace = _face;
		_targetFace = _face;
	}

	// Token: 0x06000C91 RID: 3217
	public abstract void CalculateCollisionHash(IncrementalHash calcHash);

	// Token: 0x06000C92 RID: 3218 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual int GetVertexCount()
	{
		return 0;
	}

	// Token: 0x06000C93 RID: 3219 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual int GetTriangleCount()
	{
		return 0;
	}

	// Token: 0x06000C94 RID: 3220 RVA: 0x0004E124 File Offset: 0x0004C324
	public virtual string GetName()
	{
		return string.Empty;
	}

	// Token: 0x06000C95 RID: 3221 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool UseRepairDamageState(BlockValue _blockValue)
	{
		return false;
	}

	// Token: 0x04000B32 RID: 2866
	[PublicizedFrom(EAccessModifier.Protected)]
	public static Vector2 uvZero = Vector2.zero;

	// Token: 0x04000B33 RID: 2867
	[PublicizedFrom(EAccessModifier.Protected)]
	public static Vector2 uvOne = Vector2.one;

	// Token: 0x04000B34 RID: 2868
	[PublicizedFrom(EAccessModifier.Protected)]
	public static Vector2 uvRightBot = new Vector2(1f, 0f);

	// Token: 0x04000B35 RID: 2869
	[PublicizedFrom(EAccessModifier.Protected)]
	public static Vector2 uvLeftTop = new Vector2(0f, 1f);

	// Token: 0x04000B36 RID: 2870
	[PublicizedFrom(EAccessModifier.Protected)]
	public static Vector2 uvMiddle = new Vector2(0.5f, 0.5f);

	// Token: 0x04000B37 RID: 2871
	[PublicizedFrom(EAccessModifier.Protected)]
	public static Vector2 uvMidBot = new Vector2(0.5f, 0f);

	// Token: 0x04000B38 RID: 2872
	[PublicizedFrom(EAccessModifier.Protected)]
	public static Vector2 uvMidTop = new Vector2(0.5f, 1f);

	// Token: 0x04000B39 RID: 2873
	[PublicizedFrom(EAccessModifier.Protected)]
	public static Vector2 uvLeftMid = new Vector2(0f, 0.5f);

	// Token: 0x04000B3A RID: 2874
	[PublicizedFrom(EAccessModifier.Protected)]
	public static Vector2 uvRightMid = new Vector2(1f, 0.5f);

	// Token: 0x04000B3B RID: 2875
	[PublicizedFrom(EAccessModifier.Protected)]
	public static Vector4 tngRight = new Vector4(1f, 0f, 0f, 1f);

	// Token: 0x04000B3C RID: 2876
	[PublicizedFrom(EAccessModifier.Protected)]
	public Block block;

	// Token: 0x04000B3D RID: 2877
	[PublicizedFrom(EAccessModifier.Private)]
	public Bounds bounds;

	// Token: 0x04000B3E RID: 2878
	[PublicizedFrom(EAccessModifier.Protected)]
	public Bounds[] boundsArr;

	// Token: 0x04000B3F RID: 2879
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3 minBounds;

	// Token: 0x04000B40 RID: 2880
	public bool IsSolidCube;

	// Token: 0x04000B41 RID: 2881
	public bool IsRotatable;

	// Token: 0x04000B42 RID: 2882
	public bool IsSolidSpace;

	// Token: 0x04000B43 RID: 2883
	public bool IsOmitTerrainSnappingUp;

	// Token: 0x04000B44 RID: 2884
	public bool IsNotifyOnLoadUnload;

	// Token: 0x04000B45 RID: 2885
	public byte LightOpacity;

	// Token: 0x04000B46 RID: 2886
	public int SymmetryType = 1;

	// Token: 0x04000B47 RID: 2887
	public bool Has45DegreeRotations;

	// Token: 0x0200019C RID: 412
	public enum MeshPurpose
	{
		// Token: 0x04000B49 RID: 2889
		World,
		// Token: 0x04000B4A RID: 2890
		Drop,
		// Token: 0x04000B4B RID: 2891
		Hold,
		// Token: 0x04000B4C RID: 2892
		Local,
		// Token: 0x04000B4D RID: 2893
		Preview,
		// Token: 0x04000B4E RID: 2894
		SimplifiedCollisionOnly
	}
}
