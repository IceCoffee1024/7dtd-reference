using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Profiling;
using UnityEngine;

// Token: 0x0200064C RID: 1612
public class MeshGenerator : IMeshGenerator
{
	// Token: 0x0600344C RID: 13388 RVA: 0x0014EBF0 File Offset: 0x0014CDF0
	public MeshGenerator(INeighborBlockCache _nBlocks)
	{
		this.nBlocks = _nBlocks;
	}

	// Token: 0x0600344D RID: 13389 RVA: 0x0014EC8C File Offset: 0x0014CE8C
	public void GenerateMesh(Vector3i _chunkPos, int _layerIdx, VoxelMesh[] _meshes)
	{
		Vector3i vector3i = new Vector3i(0, Utils.FastMax(0, _layerIdx * 16), 0);
		Vector3i vector3i2 = new Vector3i(15, _layerIdx * 16 + 16 - 1, 15);
		this.CreateMesh(_chunkPos, Vector3.zero, vector3i, vector3i2, _meshes, true, false);
		for (int i = 0; i < _meshes.Length; i++)
		{
			VoxelMesh voxelMesh = _meshes[i];
			voxelMesh.Finished();
			if (voxelMesh.m_Uvs.Count > 0 && voxelMesh.m_Normals.Count > 0 && voxelMesh.m_Tangents.Count == 0)
			{
				Utils.CalculateMeshTangents(voxelMesh, i == 5);
			}
		}
	}

	// Token: 0x0600344E RID: 13390 RVA: 0x0014ED1C File Offset: 0x0014CF1C
	public void GenerateMesh(Vector3i _worldStartPos, Vector3i _worldEndPos, VoxelMesh[] _meshes)
	{
		this.CreateMesh(Vector3i.zero, Vector3.zero, _worldStartPos, _worldEndPos, _meshes, true, false);
		for (int i = 0; i < _meshes.Length; i++)
		{
			_meshes[i].Finished();
		}
	}

	// Token: 0x0600344F RID: 13391 RVA: 0x0014ED54 File Offset: 0x0014CF54
	public virtual bool IsLayerEmpty(int _layerIdx)
	{
		return this.IsLayerEmpty(_layerIdx, _layerIdx);
	}

	// Token: 0x06003450 RID: 13392 RVA: 0x0014ED5E File Offset: 0x0014CF5E
	public virtual bool IsLayerEmpty(int _startLayerIdx, int _endLayerIdx)
	{
		return this.mcLayerIsEmpty(_startLayerIdx, _endLayerIdx);
	}

	// Token: 0x06003451 RID: 13393 RVA: 0x0014ED68 File Offset: 0x0014CF68
	[PublicizedFrom(EAccessModifier.Private)]
	public bool mcLayerIsEmpty(int _startLayerIdx, int _endLayerIdx)
	{
		int num = _startLayerIdx * 4;
		int num2 = (_endLayerIdx + 1) * 4;
		this.nBlocks.Init(0, 0);
		IChunk neighborChunk = this.nBlocks.GetNeighborChunk(0, 0);
		if (neighborChunk.IsOnlyTerrainLayer(num) || neighborChunk.IsEmptyLayer(num))
		{
			IChunk neighborChunk2 = this.nBlocks.GetNeighborChunk(1, 0);
			IChunk neighborChunk3 = this.nBlocks.GetNeighborChunk(-1, 0);
			IChunk neighborChunk4 = this.nBlocks.GetNeighborChunk(0, 1);
			IChunk neighborChunk5 = this.nBlocks.GetNeighborChunk(0, -1);
			for (int i = num - 1; i <= num2 + 1; i++)
			{
				if ((!neighborChunk.IsOnlyTerrainLayer(i) && !neighborChunk.IsEmptyLayer(i)) || (!neighborChunk2.IsOnlyTerrainLayer(i) && !neighborChunk2.IsEmptyLayer(i)) || (!neighborChunk3.IsOnlyTerrainLayer(i) && !neighborChunk3.IsEmptyLayer(i)) || (!neighborChunk4.IsOnlyTerrainLayer(i) && !neighborChunk4.IsEmptyLayer(i)) || (!neighborChunk5.IsOnlyTerrainLayer(i) && !neighborChunk5.IsEmptyLayer(i)))
				{
					return false;
				}
			}
			return true;
		}
		return false;
	}

	// Token: 0x06003452 RID: 13394 RVA: 0x0014EE70 File Offset: 0x0014D070
	[PublicizedFrom(EAccessModifier.Private)]
	public Lighting maxLight(Lighting _lMiddle, Lighting _lL1N1, Lighting _lL1N2, Lighting _lL1C, Lighting _lL2, Lighting _lL2N1, Lighting _lL2N2, Lighting _lL2C)
	{
		int num = 0;
		int num2 = 0;
		if (_lL1N1.sun != 0)
		{
			num += (int)_lL1N1.sun;
			num2++;
		}
		if (_lL1N2.sun != 0)
		{
			num += (int)_lL1N2.sun;
			num2++;
		}
		if (_lL1C.sun != 0)
		{
			num += (int)_lL1C.sun;
			num2++;
		}
		if (_lMiddle.sun != 0)
		{
			num += (int)_lMiddle.sun;
			num2++;
		}
		if (_lL2.sun != 0)
		{
			num += (int)_lL2.sun;
			num2++;
		}
		if (_lL2N1.sun != 0)
		{
			num += (int)_lL2N1.sun;
			num2++;
		}
		if (_lL2N2.sun != 0)
		{
			num += (int)_lL2N2.sun;
			num2++;
		}
		if (_lL2C.sun != 0)
		{
			num += (int)_lL2C.sun;
			num2++;
		}
		Lighting lighting;
		lighting.sun = (byte)((float)num / (float)num2);
		lighting.block = 0;
		lighting.stability = 0;
		return lighting;
	}

	// Token: 0x06003453 RID: 13395 RVA: 0x0014EF54 File Offset: 0x0014D154
	[PublicizedFrom(EAccessModifier.Private)]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void SwizzleCopy(ref Vector3[] to, int index0, int index1, int index2, int index3)
	{
		to[0] = this.verticesCube[index0];
		to[1] = this.verticesCube[index1];
		to[2] = this.verticesCube[index2];
		to[3] = this.verticesCube[index3];
	}

	// Token: 0x06003454 RID: 13396 RVA: 0x0014EFB4 File Offset: 0x0014D1B4
	[PublicizedFrom(EAccessModifier.Private)]
	public void RenderLiquidTriNorth(Vector3[] v4Arr, VoxelMesh[] _meshes, int y)
	{
		Vector2 vector = new Vector2(1f, 0f);
		bool flag = this.nBlocks.IsWater(1, y + 1, -1);
		bool flag2 = this.nBlocks.IsWater(-1, y + 1, -1);
		if (flag)
		{
			this.cpyVerts[0] = v4Arr[0];
			this.cpyVerts[1] = v4Arr[1];
			this.cpyVerts[2] = v4Arr[3];
			this.cpyVerts[3] = v4Arr[0];
			Vector3[] array = this.cpyVerts;
			int num = 1;
			array[num].x = array[num].x + 0.75f;
			for (int i = 0; i < 4; i++)
			{
				Vector3[] array2 = this.cpyVerts;
				int num2 = i;
				array2[num2].y = array2[num2].y + -0.5f;
			}
			WaterMeshUtils.RenderFace(this.cpyVerts, this.lightingAround, 0L, _meshes, vector, false);
		}
		if (flag2)
		{
			this.cpyVerts[0] = v4Arr[0];
			this.cpyVerts[1] = v4Arr[1];
			this.cpyVerts[2] = v4Arr[2];
			Vector3[] array3 = this.cpyVerts;
			int num3 = 0;
			array3[num3].x = array3[num3].x - 0.75f;
			this.cpyVerts[3] = this.cpyVerts[0];
			for (int j = 0; j < 4; j++)
			{
				Vector3[] array4 = this.cpyVerts;
				int num4 = j;
				array4[num4].y = array4[num4].y + -0.5f;
			}
			WaterMeshUtils.RenderFace(this.cpyVerts, this.lightingAround, 0L, _meshes, vector, false);
		}
	}

	// Token: 0x06003455 RID: 13397 RVA: 0x0014F148 File Offset: 0x0014D348
	[PublicizedFrom(EAccessModifier.Private)]
	public void RenderLiquidTriSouth(Vector3[] v4Arr, VoxelMesh[] _meshes, int y)
	{
		Vector2 vector = new Vector2(1f, 0f);
		bool flag = this.nBlocks.IsWater(1, y + 1, 1);
		bool flag2 = this.nBlocks.IsWater(-1, y + 1, 1);
		if (flag)
		{
			this.cpyVerts[0] = v4Arr[0];
			this.cpyVerts[1] = v4Arr[1];
			this.cpyVerts[2] = v4Arr[2];
			Vector3[] array = this.cpyVerts;
			int num = 0;
			array[num].x = array[num].x + 0.75f;
			this.cpyVerts[3] = this.cpyVerts[0];
			for (int i = 0; i < 4; i++)
			{
				Vector3[] array2 = this.cpyVerts;
				int num2 = i;
				array2[num2].y = array2[num2].y + -0.5f;
			}
			WaterMeshUtils.RenderFace(this.cpyVerts, this.lightingAround, 0L, _meshes, vector, false);
		}
		if (flag2)
		{
			this.cpyVerts[0] = v4Arr[0];
			this.cpyVerts[1] = v4Arr[1];
			this.cpyVerts[2] = v4Arr[3];
			Vector3[] array3 = this.cpyVerts;
			int num3 = 1;
			array3[num3].x = array3[num3].x - 0.75f;
			this.cpyVerts[3] = v4Arr[0];
			for (int j = 0; j < 4; j++)
			{
				Vector3[] array4 = this.cpyVerts;
				int num4 = j;
				array4[num4].y = array4[num4].y + -0.5f;
			}
			WaterMeshUtils.RenderFace(this.cpyVerts, this.lightingAround, 0L, _meshes, vector, false);
		}
	}

	// Token: 0x06003456 RID: 13398 RVA: 0x0014F2DC File Offset: 0x0014D4DC
	[PublicizedFrom(EAccessModifier.Private)]
	public void RenderLiquidTriEast(Vector3[] v4Arr, VoxelMesh[] _meshes, int y)
	{
		Vector2 vector = new Vector2(0f, 1f);
		bool flag = this.nBlocks.IsWater(-1, y + 1, 1);
		bool flag2 = this.nBlocks.IsWater(-1, y + 1, -1);
		if (flag)
		{
			this.cpyVerts[0] = v4Arr[0];
			this.cpyVerts[1] = v4Arr[1];
			this.cpyVerts[2] = v4Arr[2];
			Vector3[] array = this.cpyVerts;
			int num = 0;
			array[num].z = array[num].z + 0.75f;
			this.cpyVerts[3] = this.cpyVerts[0];
			for (int i = 0; i < 4; i++)
			{
				Vector3[] array2 = this.cpyVerts;
				int num2 = i;
				array2[num2].y = array2[num2].y + -0.5f;
			}
			WaterMeshUtils.RenderFace(this.cpyVerts, this.lightingAround, 0L, _meshes, vector, false);
		}
		if (flag2)
		{
			this.cpyVerts[0] = v4Arr[0];
			this.cpyVerts[1] = v4Arr[1];
			this.cpyVerts[2] = v4Arr[3];
			Vector3[] array3 = this.cpyVerts;
			int num3 = 1;
			array3[num3].z = array3[num3].z - 0.75f;
			this.cpyVerts[3] = v4Arr[0];
			for (int j = 0; j < 4; j++)
			{
				Vector3[] array4 = this.cpyVerts;
				int num4 = j;
				array4[num4].y = array4[num4].y + -0.5f;
			}
			WaterMeshUtils.RenderFace(this.cpyVerts, this.lightingAround, 0L, _meshes, vector, false);
		}
	}

	// Token: 0x06003457 RID: 13399 RVA: 0x0014F470 File Offset: 0x0014D670
	[PublicizedFrom(EAccessModifier.Private)]
	public void RenderLiquidTriWest(Vector3[] v4Arr, VoxelMesh[] _meshes, int y)
	{
		Vector2 vector = new Vector2(0f, 1f);
		bool flag = this.nBlocks.IsWater(1, y + 1, 1);
		bool flag2 = this.nBlocks.IsWater(1, y + 1, -1);
		if (flag)
		{
			this.cpyVerts[0] = v4Arr[0];
			this.cpyVerts[1] = v4Arr[1];
			this.cpyVerts[2] = v4Arr[3];
			this.cpyVerts[3] = v4Arr[0];
			Vector3[] array = this.cpyVerts;
			int num = 1;
			array[num].z = array[num].z + 0.75f;
			for (int i = 0; i < 4; i++)
			{
				Vector3[] array2 = this.cpyVerts;
				int num2 = i;
				array2[num2].y = array2[num2].y + -0.5f;
			}
			WaterMeshUtils.RenderFace(this.cpyVerts, this.lightingAround, 0L, _meshes, vector, false);
		}
		if (flag2)
		{
			this.cpyVerts[0] = v4Arr[0];
			this.cpyVerts[1] = v4Arr[1];
			this.cpyVerts[2] = v4Arr[2];
			Vector3[] array3 = this.cpyVerts;
			int num3 = 0;
			array3[num3].z = array3[num3].z - 0.75f;
			this.cpyVerts[3] = this.cpyVerts[0];
			for (int j = 0; j < 4; j++)
			{
				Vector3[] array4 = this.cpyVerts;
				int num4 = j;
				array4[num4].y = array4[num4].y + -0.5f;
			}
			WaterMeshUtils.RenderFace(this.cpyVerts, this.lightingAround, 0L, _meshes, vector, false);
		}
	}

	// Token: 0x06003458 RID: 13400 RVA: 0x0014F604 File Offset: 0x0014D804
	[PublicizedFrom(EAccessModifier.Private)]
	public void RenderLiquidFaceNorth(Vector3[] v4Arr, VoxelMesh[] _meshes, int cpyX, int cpyZ)
	{
		Vector2 vector = new Vector2(1f, 0f);
		this.cpyVerts[0].z = v4Arr[0].z;
		this.cpyVerts[1].z = v4Arr[1].z;
		this.cpyVerts[2].z = v4Arr[2].z;
		this.cpyVerts[3].z = v4Arr[3].z;
		this.cpyVerts[0].x = v4Arr[0].x - (float)cpyZ * 0.25f;
		this.cpyVerts[1].x = this.cpyVerts[0].x - 0.25f;
		this.cpyVerts[2].x = this.cpyVerts[0].x - 0.25f;
		this.cpyVerts[3].x = v4Arr[0].x - (float)cpyZ * 0.25f;
		this.cpyVerts[0].y = -0.5f + v4Arr[0].y + (float)cpyX * 0.25f;
		this.cpyVerts[1].y = -0.5f + v4Arr[0].y + (float)cpyX * 0.25f;
		this.cpyVerts[2].y = this.cpyVerts[0].y + 0.25f;
		this.cpyVerts[3].y = this.cpyVerts[0].y + 0.25f;
		WaterMeshUtils.RenderFace(this.cpyVerts, this.lightingAround, 0L, _meshes, vector, false);
	}

	// Token: 0x06003459 RID: 13401 RVA: 0x0014F7F4 File Offset: 0x0014D9F4
	[PublicizedFrom(EAccessModifier.Private)]
	public void RenderLiquidFaceSouth(Vector3[] v4Arr, VoxelMesh[] _meshes, int cpyX, int cpyZ)
	{
		Vector2 vector = new Vector2(1f, 0f);
		this.cpyVerts[0].z = v4Arr[0].z;
		this.cpyVerts[1].z = v4Arr[1].z;
		this.cpyVerts[2].z = v4Arr[2].z;
		this.cpyVerts[3].z = v4Arr[3].z;
		this.cpyVerts[0].x = v4Arr[0].x + (float)cpyZ * 0.25f;
		this.cpyVerts[1].x = this.cpyVerts[0].x + 0.25f;
		this.cpyVerts[2].x = this.cpyVerts[0].x + 0.25f;
		this.cpyVerts[3].x = v4Arr[0].x + (float)cpyZ * 0.25f;
		this.cpyVerts[0].y = -0.5f + v4Arr[0].y + (float)cpyX * 0.25f;
		this.cpyVerts[1].y = -0.5f + v4Arr[0].y + (float)cpyX * 0.25f;
		this.cpyVerts[2].y = this.cpyVerts[0].y + 0.25f;
		this.cpyVerts[3].y = this.cpyVerts[0].y + 0.25f;
		WaterMeshUtils.RenderFace(this.cpyVerts, this.lightingAround, 0L, _meshes, vector, false);
	}

	// Token: 0x0600345A RID: 13402 RVA: 0x0014F9E4 File Offset: 0x0014DBE4
	[PublicizedFrom(EAccessModifier.Private)]
	public void RenderLiquidFaceEast(Vector3[] v4Arr, VoxelMesh[] _meshes, int cpyX, int cpyZ)
	{
		Vector2 vector = new Vector2(0f, 1f);
		this.cpyVerts[0].x = v4Arr[0].x;
		this.cpyVerts[1].x = v4Arr[1].x;
		this.cpyVerts[2].x = v4Arr[2].x;
		this.cpyVerts[3].x = v4Arr[3].x;
		this.cpyVerts[0].z = v4Arr[0].z + (float)cpyZ * 0.25f;
		this.cpyVerts[1].z = this.cpyVerts[0].z + 0.25f;
		this.cpyVerts[2].z = this.cpyVerts[0].z + 0.25f;
		this.cpyVerts[3].z = v4Arr[0].z + (float)cpyZ * 0.25f;
		this.cpyVerts[0].y = -0.5f + v4Arr[0].y + (float)cpyX * 0.25f;
		this.cpyVerts[1].y = -0.5f + v4Arr[0].y + (float)cpyX * 0.25f;
		this.cpyVerts[2].y = this.cpyVerts[0].y + 0.25f;
		this.cpyVerts[3].y = this.cpyVerts[0].y + 0.25f;
		WaterMeshUtils.RenderFace(this.cpyVerts, this.lightingAround, 0L, _meshes, vector, false);
	}

	// Token: 0x0600345B RID: 13403 RVA: 0x0014FBD4 File Offset: 0x0014DDD4
	[PublicizedFrom(EAccessModifier.Private)]
	public void RenderLiquidFaceWest(Vector3[] v4Arr, VoxelMesh[] _meshes, int cpyX, int cpyZ)
	{
		Vector2 vector = new Vector2(0f, 1f);
		this.cpyVerts[0].x = v4Arr[0].x;
		this.cpyVerts[1].x = v4Arr[1].x;
		this.cpyVerts[2].x = v4Arr[2].x;
		this.cpyVerts[3].x = v4Arr[3].x;
		this.cpyVerts[0].z = v4Arr[0].z - (float)cpyZ * 0.25f;
		this.cpyVerts[1].z = this.cpyVerts[0].z - 0.25f;
		this.cpyVerts[2].z = this.cpyVerts[0].z - 0.25f;
		this.cpyVerts[3].z = v4Arr[0].z - (float)cpyZ * 0.25f;
		this.cpyVerts[0].y = -0.5f + v4Arr[0].y + (float)cpyX * 0.25f;
		this.cpyVerts[1].y = -0.5f + v4Arr[0].y + (float)cpyX * 0.25f;
		this.cpyVerts[2].y = this.cpyVerts[0].y + 0.25f;
		this.cpyVerts[3].y = this.cpyVerts[0].y + 0.25f;
		WaterMeshUtils.RenderFace(this.cpyVerts, this.lightingAround, 0L, _meshes, vector, false);
	}

	// Token: 0x0600345C RID: 13404 RVA: 0x0014FDC2 File Offset: 0x0014DFC2
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsAir(int type)
	{
		return type == 0;
	}

	// Token: 0x0600345D RID: 13405 RVA: 0x0014FDC8 File Offset: 0x0014DFC8
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsPlant(int type)
	{
		Block block = Block.list[type];
		return block != null && block.blockMaterial != null && block.blockMaterial.IsPlant;
	}

	// Token: 0x0600345E RID: 13406 RVA: 0x0014FDF8 File Offset: 0x0014DFF8
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsTerrain(int type)
	{
		Block block = Block.list[type];
		return block != null && block.shape != null && block.shape.IsTerrain();
	}

	// Token: 0x0600345F RID: 13407 RVA: 0x0014FE28 File Offset: 0x0014E028
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsSolidCube(int type)
	{
		Block block = Block.list[type];
		return block != null && block.shape != null && block.shape.IsSolidCube;
	}

	// Token: 0x06003460 RID: 13408 RVA: 0x0014FE58 File Offset: 0x0014E058
	[PublicizedFrom(EAccessModifier.Private)]
	public void DrawSide(ref MeshGenerator.DrawSideParams p)
	{
		int x = p.posI.x;
		int y = p.posI.y;
		int z = p.posI.z;
		int num = 0;
		int num2 = 0;
		switch (p.face)
		{
		case BlockFace.North:
			num2 = -1;
			break;
		case BlockFace.West:
			num = 1;
			break;
		case BlockFace.South:
			num2 = 1;
			break;
		case BlockFace.East:
			num = -1;
			break;
		}
		BlockValue blockValue = this.nBlocks.Get(num, y, num2);
		Block block = blockValue.Block;
		if (block != null)
		{
			byte stab = this.nBlocks.GetStab(num, y, num2);
			this.lightingAround.SetStab(stab);
			bool flag = this.nBlocks.IsWater(num, y, num2) && MeshGenerator.FacePermitsFlow(blockValue, p.face);
			if (flag && (this.IsTerrain(p.midBV.type) || (!this.IsSolidCube(p.midBV.type) && !this.IsPlant(p.midBV.type) && !p.midBV.isair)))
			{
				bool isWaterLayerStarted = p.isWaterLayerStarted;
				p.isWaterTopDrawn = true;
				p.isWaterLayerStarted = true;
				if (!isWaterLayerStarted && !this.nBlocks.IsWater(num, y + 1, num2))
				{
					this.SwizzleCopy(ref p.v4Arr, 1, 2, 6, 5);
					this.RenderTopWater(p.midBV, p.v4Arr, p._meshes, p.posI, p._worldPos, true);
				}
			}
			switch (p.face)
			{
			case BlockFace.North:
				this.SwizzleCopy(ref p.v4Arr, 3, 0, 1, 2);
				break;
			case BlockFace.West:
				this.SwizzleCopy(ref p.v4Arr, 7, 3, 2, 6);
				break;
			case BlockFace.South:
				this.SwizzleCopy(ref p.v4Arr, 4, 7, 6, 5);
				break;
			case BlockFace.East:
				this.SwizzleCopy(ref p.v4Arr, 0, 4, 5, 1);
				break;
			}
			if (block.shape.isRenderFace(blockValue, p.face, p.midBV))
			{
				Vector3 vector = p.drawPos + Vector3.forward;
				switch (p.face)
				{
				case BlockFace.North:
					vector = p.drawPos - Vector3.forward;
					break;
				case BlockFace.West:
					vector = p.drawPos + Vector3.right;
					break;
				case BlockFace.South:
					vector = p.drawPos + Vector3.forward;
					break;
				case BlockFace.East:
					vector = p.drawPos - Vector3.right;
					break;
				}
				IChunk chunk = this.nBlocks.GetChunk(num, num2);
				int num3 = World.toBlockXZ(x + num);
				int num4 = World.toBlockXZ(z + num2);
				TextureFullArray textureFullArray = chunk.GetTextureFullArray(num3, y, num4);
				block.shape.renderFace(p._worldPos, blockValue, vector, p.face, p.v4Arr, this.lightingAround, textureFullArray, p._meshes, BlockShape.MeshPurpose.World);
			}
			if (flag && !p.isWaterLayerStarted)
			{
				bool flag2 = this.nBlocks.IsWater(num, y + 1, num2);
				Block block2 = this.nBlocks.Get(num, y + 1, num2).Block;
				if (flag2 || block2.shape.IsTerrain())
				{
					for (int i = 0; i < 4; i++)
					{
						for (int j = 0; j < 4; j++)
						{
							switch (p.face)
							{
							case BlockFace.North:
								this.RenderLiquidFaceNorth(p.v4Arr, p._meshes, i, j);
								break;
							case BlockFace.West:
								this.RenderLiquidFaceWest(p.v4Arr, p._meshes, i, j);
								break;
							case BlockFace.South:
								this.RenderLiquidFaceSouth(p.v4Arr, p._meshes, i, j);
								break;
							case BlockFace.East:
								this.RenderLiquidFaceEast(p.v4Arr, p._meshes, i, j);
								break;
							}
						}
					}
					return;
				}
				switch (p.face)
				{
				case BlockFace.North:
					this.RenderLiquidTriNorth(p.v4Arr, p._meshes, y);
					return;
				case BlockFace.West:
					this.RenderLiquidTriWest(p.v4Arr, p._meshes, y);
					break;
				case BlockFace.South:
					this.RenderLiquidTriSouth(p.v4Arr, p._meshes, y);
					return;
				case BlockFace.East:
					this.RenderLiquidTriEast(p.v4Arr, p._meshes, y);
					return;
				default:
					return;
				}
			}
		}
	}

	// Token: 0x06003461 RID: 13409 RVA: 0x001502AD File Offset: 0x0014E4AD
	[PublicizedFrom(EAccessModifier.Private)]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool FacePermitsFlow(BlockValue bv, BlockFace worldspaceFace)
	{
		return (bv.rotatedWaterFlowMask & BlockFaceFlags.FromBlockFace(worldspaceFace)) == BlockFaceFlag.None;
	}

	// Token: 0x06003462 RID: 13410 RVA: 0x001502C0 File Offset: 0x0014E4C0
	[PublicizedFrom(EAccessModifier.Private)]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool FacePermitsFlow(BlockFaceFlag flags, BlockFace worldspaceFace)
	{
		return (flags & BlockFaceFlags.FromBlockFace(worldspaceFace)) == BlockFaceFlag.None;
	}

	// Token: 0x06003463 RID: 13411 RVA: 0x001502D0 File Offset: 0x0014E4D0
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void CreateMesh(Vector3i _worldPos, Vector3 _drawPosOffset, Vector3i _start, Vector3i _end, VoxelMesh[] _meshes, bool _bCalcAmbientLight, bool _bOnlyDistortVertices)
	{
		if (this.mcLayerIsEmpty(_start.y / 16, _end.y / 16))
		{
			return;
		}
		World world = GameManager.Instance.World;
		Vector3 zero = Vector3.zero;
		Vector3i zero2 = Vector3i.zero;
		Vector3[] array = new Vector3[4];
		this.lightCube.SetBlockCache(this.nBlocks);
		MeshGenerator.DrawSideParams drawSideParams;
		drawSideParams._meshes = _meshes;
		drawSideParams.nBlocks = this.nBlocks;
		drawSideParams.v4Arr = array;
		drawSideParams._worldPos = _worldPos;
		for (int i = _start.x; i <= _end.x; i++)
		{
			drawSideParams.posI.x = i;
			for (int j = _start.z; j <= _end.z; j++)
			{
				drawSideParams.posI.z = j;
				float temperature = VoxelMesh.GetTemperature(world.GetBiome(_worldPos.x + i, _worldPos.z + j));
				this.nBlocks.Init(i, j);
				int num = (int)this.nBlocks.GetChunk(0, 0).GetHeight(ChunkBlockLayerLegacy.CalcOffset(i, j));
				this.heights[1] = (int)this.nBlocks.GetChunk(1, 0).GetHeight(ChunkBlockLayerLegacy.CalcOffset(i + 1, j));
				this.heights[2] = (int)this.nBlocks.GetChunk(0, -1).GetHeight(ChunkBlockLayerLegacy.CalcOffset(i, j - 1));
				this.heights[3] = (int)this.nBlocks.GetChunk(-1, 0).GetHeight(ChunkBlockLayerLegacy.CalcOffset(i - 1, j));
				this.heights[4] = (int)this.nBlocks.GetChunk(0, 1).GetHeight(ChunkBlockLayerLegacy.CalcOffset(i, j + 1));
				for (int k = 1; k < 5; k++)
				{
					if (num < this.heights[k])
					{
						num = this.heights[k];
					}
				}
				num++;
				int num2 = Utils.FastMin(_end.y, num + 1);
				num2 = Utils.FastMin(254, num2);
				drawSideParams.biomeTemperature = temperature;
				drawSideParams.isWaterLayerStarted = false;
				int y = _start.y;
				for (int l = num2; l >= y; l--)
				{
					BlockValue blockValue = this.nBlocks.Get(l);
					Block block = blockValue.Block;
					if (block != null && (blockValue.ischild || !block.shape.IsSolidCube || block.shape.IsTerrain() || (l > 0 && this.nBlocks.IsWater(0, l - 1, 0) && !this.nBlocks.IsWater(-1, l, 0) && !this.nBlocks.IsWater(1, l, 0) && !this.nBlocks.IsWater(0, l, -1) && !this.nBlocks.IsWater(0, l, 1))))
					{
						bool flag = this.nBlocks.IsWater(0, l, 0);
						if (!drawSideParams.isWaterLayerStarted && flag)
						{
							drawSideParams.isWaterLayerStarted = true;
						}
						zero.x = _drawPosOffset.x + (float)i;
						zero.y = _drawPosOffset.y + (float)l;
						zero.z = _drawPosOffset.z + (float)j;
						zero2.x = i;
						zero2.y = l;
						zero2.z = j;
						for (int m = 0; m < 8; m++)
						{
							this.verticesCube[m].x = BlockShapeCube.Cube[m].x + zero.x;
							this.verticesCube[m].y = BlockShapeCube.Cube[m].y + zero.y;
							this.verticesCube[m].z = BlockShapeCube.Cube[m].z + zero.z;
						}
						if (!_bOnlyDistortVertices)
						{
							this.lightCube.SetPosition(zero2);
							Lighting lighting = this.lightCube[0, 0, 0];
							Lighting lighting2 = this.lightCube[-1, 0, 0];
							Lighting lighting3 = this.lightCube[1, 0, 0];
							Lighting lighting4 = this.lightCube[0, -1, 0];
							Lighting lighting5 = this.lightCube[0, 1, 0];
							Lighting lighting6 = this.lightCube[0, 0, -1];
							Lighting lighting7 = this.lightCube[0, 0, 1];
							Lighting lighting8 = this.lightCube[-1, 0, -1];
							Lighting lighting9 = this.lightCube[1, 0, -1];
							Lighting lighting10 = this.lightCube[-1, 0, 1];
							Lighting lighting11 = this.lightCube[1, 0, 1];
							this.lightingAround[LightingAround.Pos.Middle] = lighting;
							this.lightingAround[LightingAround.Pos.X0Y0Z0] = this.maxLight(lighting, lighting6, lighting2, lighting8, lighting4, this.lightCube[0, -1, -1], this.lightCube[-1, -1, 0], this.lightCube[-1, -1, -1]);
							this.lightingAround[LightingAround.Pos.X1Y0Z0] = this.maxLight(lighting, lighting6, lighting3, lighting9, lighting4, this.lightCube[0, -1, -1], this.lightCube[1, -1, 0], this.lightCube[1, -1, -1]);
							this.lightingAround[LightingAround.Pos.X1Y0Z1] = this.maxLight(lighting, lighting7, lighting3, lighting11, lighting4, this.lightCube[0, -1, 1], this.lightCube[1, -1, 0], this.lightCube[1, -1, 1]);
							this.lightingAround[LightingAround.Pos.X0Y0Z1] = this.maxLight(lighting, lighting7, lighting2, lighting10, lighting4, this.lightCube[0, -1, 1], this.lightCube[-1, -1, 0], this.lightCube[-1, -1, 1]);
							this.lightingAround[LightingAround.Pos.X0Y1Z0] = this.maxLight(lighting, lighting6, lighting2, lighting8, lighting5, this.lightCube[0, 1, -1], this.lightCube[-1, 1, 0], this.lightCube[-1, 1, -1]);
							this.lightingAround[LightingAround.Pos.X1Y1Z0] = this.maxLight(lighting, lighting6, lighting3, lighting9, lighting5, this.lightCube[0, 1, -1], this.lightCube[1, 1, 0], this.lightCube[1, 1, -1]);
							this.lightingAround[LightingAround.Pos.X1Y1Z1] = this.maxLight(lighting, lighting7, lighting3, lighting11, lighting5, this.lightCube[0, 1, 1], this.lightCube[1, 1, 0], this.lightCube[1, 1, 1]);
							this.lightingAround[LightingAround.Pos.X0Y1Z1] = this.maxLight(lighting, lighting7, lighting2, lighting10, lighting5, this.lightCube[0, 1, 1], this.lightCube[-1, 1, 0], this.lightCube[-1, 1, 1]);
							int facesDrawnFullBitfield = block.shape.getFacesDrawnFullBitfield(blockValue);
							drawSideParams.midBV = blockValue;
							drawSideParams.isWaterTopDrawn = false;
							drawSideParams.drawPos = zero;
							drawSideParams.posI.y = l;
							if ((facesDrawnFullBitfield & 16) == 0)
							{
								drawSideParams.face = BlockFace.North;
								this.DrawSide(ref drawSideParams);
							}
							if ((facesDrawnFullBitfield & 4) == 0)
							{
								drawSideParams.face = BlockFace.South;
								this.DrawSide(ref drawSideParams);
							}
							if ((facesDrawnFullBitfield & 8) == 0)
							{
								drawSideParams.face = BlockFace.East;
								this.DrawSide(ref drawSideParams);
							}
							if ((facesDrawnFullBitfield & 32) == 0)
							{
								drawSideParams.face = BlockFace.West;
								this.DrawSide(ref drawSideParams);
							}
							if ((facesDrawnFullBitfield & 1) == 0)
							{
								BlockValue blockValue2 = this.nBlocks.Get(l + 1);
								Block block2 = blockValue2.Block;
								if (block2 != null && block2.shape.isRenderFace(blockValue2, BlockFace.Bottom, blockValue))
								{
									if (block2.GetLightValue(blockValue2) <= 0)
									{
										float color_BOTTOM = VoxelMesh.COLOR_BOTTOM;
									}
									else
									{
										float color_TOP = VoxelMesh.COLOR_TOP;
									}
									IChunk chunk = this.nBlocks.GetChunk(0, 0);
									TextureFullArray textureFullArray = chunk.GetTextureFullArray(i, l + 1, j);
									this.SwizzleCopy(ref array, 2, 1, 5, 6);
									byte stability = chunk.GetStability(i, l + 1, j);
									this.lightingAround.SetStab(stability);
									block2.shape.renderFace(_worldPos, blockValue2, zero + Vector3.up, BlockFace.Bottom, array, this.lightingAround, textureFullArray, _meshes, BlockShape.MeshPurpose.World);
								}
							}
							BlockValue blockValue3 = this.nBlocks.Get(l - 1);
							Block block3 = blockValue3.Block;
							if (block3 != null && (facesDrawnFullBitfield & 2) == 0 && block3.shape.isRenderFace(blockValue3, BlockFace.Top, blockValue))
							{
								IChunk chunk2 = this.nBlocks.GetChunk(0, 0);
								TextureFullArray textureFullArray2 = chunk2.GetTextureFullArray(i, l - 1, j);
								this.SwizzleCopy(ref array, 0, 3, 7, 4);
								byte stability2 = chunk2.GetStability(i, l - 1, j);
								this.lightingAround.SetStab(stability2);
								float num3 = 0f;
								if (block3.shape.IsTerrain() && l > 0)
								{
									sbyte density = this.nBlocks.GetChunk(0, 0).GetDensity(i, l, j);
									sbyte density2 = this.nBlocks.GetChunk(0, 0).GetDensity(i, l - 1, j);
									num3 = MarchingCubes.GetDecorationOffsetY(density, density2);
								}
								block3.shape.renderFace(_worldPos, blockValue3, zero + new Vector3(0f, num3 - 1f, 0f), BlockFace.Top, array, this.lightingAround, textureFullArray2, _meshes, BlockShape.MeshPurpose.World);
							}
							if (this.nBlocks.IsWater(0, l - 1, 0) && !flag && !drawSideParams.isWaterTopDrawn)
							{
								IChunk chunk3 = this.nBlocks.GetChunk(0, 0);
								this.SwizzleCopy(ref array, 0, 3, 7, 4);
								byte stability3 = chunk3.GetStability(i, l - 1, j);
								this.lightingAround.SetStab(stability3);
								this.RenderTopWater(blockValue, array, _meshes, zero2, _worldPos, false);
							}
							if (block != null && !blockValue.ischild && block.shape.IsRenderDecoration())
							{
								IChunk chunk4 = this.nBlocks.GetChunk(0, 0);
								float num4 = 0f;
								if (block.IsTerrainDecoration && block3.shape.IsTerrain() && l > 0)
								{
									sbyte density3 = chunk4.GetDensity(i, l, j);
									sbyte density4 = chunk4.GetDensity(i, l - 1, j);
									num4 = MarchingCubes.GetDecorationOffsetY(density3, density4);
								}
								for (int n = 0; n < _meshes.Length; n++)
								{
									_meshes[n].SetTemperature(temperature);
								}
								byte stability4 = chunk4.GetStability(i, l, j);
								this.lightingAround.SetStab(stability4);
								TextureFullArray textureFullArray3 = chunk4.GetTextureFullArray(i, l, j);
								block.RenderDecorations(_worldPos, blockValue, zero + new Vector3(0f, num4, 0f), this.verticesCube, this.lightingAround, textureFullArray3, _meshes, this.nBlocks);
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06003464 RID: 13412 RVA: 0x000027FC File Offset: 0x000009FC
	public void Test()
	{
	}

	// Token: 0x06003465 RID: 13413 RVA: 0x00150DA4 File Offset: 0x0014EFA4
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsRaisable(int type)
	{
		return type != 0 && (this.IsSolidCube(type) || !this.IsPlant(type));
	}

	// Token: 0x06003466 RID: 13414 RVA: 0x00150DC0 File Offset: 0x0014EFC0
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsRaisableNeighbor(int _y, int _x, int _z)
	{
		return this.neighborIsWater[_y, _x, _z] || this.IsRaisable(this.neighborType[_y, _x, _z]);
	}

	// Token: 0x06003467 RID: 13415 RVA: 0x00150DE8 File Offset: 0x0014EFE8
	[PublicizedFrom(EAccessModifier.Private)]
	public bool CanWaterTaper(int type)
	{
		return type == 0 || this.IsPlant(type);
	}

	// Token: 0x06003468 RID: 13416 RVA: 0x00150DF6 File Offset: 0x0014EFF6
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsSolid(int type)
	{
		return this.IsTerrain(type) || (this.IsSolidCube(type) && type != 0);
	}

	// Token: 0x06003469 RID: 13417 RVA: 0x00150E12 File Offset: 0x0014F012
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsAirNeighbor(int _y, int _x, int _z)
	{
		return this.IsAir(this.neighborType[_y, _x, _z]) && !this.neighborIsWater[_y, _x, _z];
	}

	// Token: 0x0600346A RID: 13418 RVA: 0x00150E3D File Offset: 0x0014F03D
	[Conditional("NEW_WATER_MESH_DEBUG")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void WatermeshDebugLog(string message)
	{
		global::UnityEngine.Debug.Log("[watermesh] " + message);
	}

	// Token: 0x0600346B RID: 13419 RVA: 0x00150E50 File Offset: 0x0014F050
	[PublicizedFrom(EAccessModifier.Private)]
	public void RenderTopWater(BlockValue _middleBV, Vector3[] v4Arr, VoxelMesh[] _meshes, Vector3i _blockPos, Vector3i _worldPos, bool isInsideTerrain = false)
	{
		_worldPos + _blockPos;
		int y = _blockPos.y;
		int num = (isInsideTerrain ? y : (y - 1));
		Vector3i vector3i = new Vector3i(_blockPos.x, num, _blockPos.z);
		this.neighborType[0, 1, 1] = 0;
		this.neighborType[1, 1, 1] = _middleBV.type;
		for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				for (int k = 0; k < 3; k++)
				{
					this.neighborIsWater[i, k, j] = this.nBlocks.IsWater(k - 1, num + i, j - 1);
					BlockValue blockValue = this.nBlocks.Get(k - 1, num + i, j - 1);
					this.neighborType[i, k, j] = blockValue.type;
					if (i == 0)
					{
						this.neighborFlags[k, j] = blockValue.rotatedWaterFlowMask;
					}
				}
			}
		}
		int type = _middleBV.type;
		if (!this.IsSolidCube(type) && !this.nBlocks.IsAir(0, y, 0) && !this.neighborIsWater[1, 1, 1] && (this.neighborIsWater[1, 0, 1] || this.neighborIsWater[1, 1, 2] || this.neighborIsWater[1, 2, 1] || this.neighborIsWater[1, 1, 0]))
		{
			return;
		}
		if (this.IsSolid(this.neighborType[0, 0, 0]) && this.IsSolid(this.neighborType[0, 1, 0]) && this.IsSolid(this.neighborType[0, 2, 0]) && this.IsSolid(this.neighborType[0, 2, 1]) && this.IsSolid(this.neighborType[0, 2, 2]) && this.IsSolid(this.neighborType[0, 1, 2]) && this.IsSolid(this.neighborType[0, 0, 2]))
		{
			this.IsSolid(this.neighborType[0, 0, 1]);
		}
		BlockValue blockValue2 = (isInsideTerrain ? _middleBV : this.nBlocks.Get(0, num, 0));
		bool flag = this.<RenderTopWater>g__TryPrepareWaterClippingVolume|63_6(blockValue2);
		using (MeshGenerator.renderTopWaterNewMarker.Auto())
		{
			for (int l = 0; l < this.vertexPositions.Length; l++)
			{
				int num2 = l % 5;
				int num3 = l / 5;
				this.vertexPositions[l] = v4Arr[0] + new Vector3((float)num2 * 0.25f, -1.5f, (float)num3 * 0.25f);
			}
			bool flag2 = this.neighborIsWater[1, 1, 0];
			bool flag3 = this.neighborIsWater[1, 2, 1];
			bool flag4 = this.neighborIsWater[1, 1, 2];
			bool flag5 = this.neighborIsWater[1, 0, 1];
			bool flag10;
			bool flag11;
			bool flag12;
			bool flag13;
			bool flag14;
			bool flag15;
			bool flag16;
			bool flag17;
			using (MeshGenerator.raisableSidesAndCornersMarker.Auto())
			{
				if ((blockValue2.rotatedWaterFlowMask & BlockFaceFlag.Axials) == BlockFaceFlag.Axials)
				{
					bool flag6 = this.<RenderTopWater>g__NeighborIsVisualWater|63_5(1, 0, -1, 0);
					bool flag7 = this.<RenderTopWater>g__NeighborIsVisualWater|63_5(2, 1, 0, -1);
					bool flag8 = this.<RenderTopWater>g__NeighborIsVisualWater|63_5(1, 2, 1, 0);
					bool flag9 = this.<RenderTopWater>g__NeighborIsVisualWater|63_5(0, 1, 0, 1);
					flag10 = (flag6 && this.<RenderTopWater>g__CheckNeighborHasRaisableCorner|63_4(1, 0, BlockFace.West, BlockFace.North)) || (flag9 && this.<RenderTopWater>g__CheckNeighborHasRaisableCorner|63_4(0, 1, BlockFace.East, BlockFace.South));
					flag11 = (flag6 && this.<RenderTopWater>g__CheckNeighborHasRaisableCorner|63_4(1, 0, BlockFace.East, BlockFace.North)) || (flag7 && this.<RenderTopWater>g__CheckNeighborHasRaisableCorner|63_4(2, 1, BlockFace.West, BlockFace.South));
					flag12 = (flag8 && this.<RenderTopWater>g__CheckNeighborHasRaisableCorner|63_4(1, 2, BlockFace.West, BlockFace.South)) || (flag9 && this.<RenderTopWater>g__CheckNeighborHasRaisableCorner|63_4(0, 1, BlockFace.East, BlockFace.North));
					flag13 = (flag8 && this.<RenderTopWater>g__CheckNeighborHasRaisableCorner|63_4(1, 2, BlockFace.East, BlockFace.South)) || (flag7 && this.<RenderTopWater>g__CheckNeighborHasRaisableCorner|63_4(2, 1, BlockFace.West, BlockFace.North));
					flag14 = flag6 || (flag10 && flag11);
					flag15 = flag7 || (flag11 && flag13);
					flag16 = flag8 || (flag13 && flag12);
					flag17 = flag9 || (flag12 && flag10);
				}
				else
				{
					bool flag18 = this.IsTerrain(blockValue2.type);
					bool flag19 = this.<RenderTopWater>g__CheckSolidWall|63_1(1, 1, flag18, BlockFace.South);
					bool flag20 = this.<RenderTopWater>g__CheckSolidWall|63_1(1, 1, flag18, BlockFace.East);
					bool flag21 = this.<RenderTopWater>g__CheckSolidWall|63_1(1, 1, flag18, BlockFace.North);
					bool flag22 = this.<RenderTopWater>g__CheckSolidWall|63_1(1, 1, flag18, BlockFace.West);
					bool flag23 = this.neighborIsWater[0, 1, 1];
					flag14 = flag19 || this.<RenderTopWater>g__CheckRaisableSide|63_2(flag23, 1, 0, this.neighborIsWater[0, 0, 0] || this.neighborIsWater[0, 2, 0]);
					flag15 = flag20 || this.<RenderTopWater>g__CheckRaisableSide|63_2(flag23, 2, 1, this.neighborIsWater[0, 2, 0] || this.neighborIsWater[0, 2, 2]);
					flag16 = flag21 || this.<RenderTopWater>g__CheckRaisableSide|63_2(flag23, 1, 2, this.neighborIsWater[0, 0, 2] || this.neighborIsWater[0, 2, 2]);
					flag17 = flag22 || this.<RenderTopWater>g__CheckRaisableSide|63_2(flag23, 0, 1, this.neighborIsWater[0, 0, 0] || this.neighborIsWater[0, 0, 2]);
					flag10 = flag14 && flag17 && this.<RenderTopWater>g__CheckRaisableCorner|63_3(1, 1, BlockFace.West, BlockFace.South, flag22, flag19);
					flag11 = flag14 && flag15 && this.<RenderTopWater>g__CheckRaisableCorner|63_3(1, 1, BlockFace.East, BlockFace.South, flag20, flag19);
					flag12 = flag16 && flag17 && this.<RenderTopWater>g__CheckRaisableCorner|63_3(1, 1, BlockFace.West, BlockFace.North, flag22, flag21);
					flag13 = flag16 && flag15 && this.<RenderTopWater>g__CheckRaisableCorner|63_3(1, 1, BlockFace.East, BlockFace.North, flag20, flag21);
				}
			}
			bool flag24 = true;
			for (int m = 0; m < this.vertexPositions.Length; m++)
			{
				int num4 = m % 5;
				int num5 = m / 5;
				bool flag25 = num5 == 0;
				bool flag26 = num5 == 4;
				bool flag27 = num4 == 0;
				bool flag28 = num4 == 4;
				if ((flag25 && flag2) || (flag28 && flag3) || (flag26 && flag4) || (flag27 && flag5))
				{
					Vector3[] array = this.vertexPositions;
					int num6 = m;
					array[num6].y = array[num6].y + 1f;
				}
				else
				{
					bool flag29 = num5 < 2;
					bool flag30 = num5 >= 3;
					bool flag31 = num4 < 2;
					bool flag32 = num4 >= 3;
					bool flag33 = !flag29 && !flag30 && !flag31 && !flag32;
					bool flag34 = flag29 && (!flag31 && !flag32);
					bool flag35 = flag30 && (!flag31 && !flag32);
					bool flag36 = flag31 && (!flag29 && !flag30);
					bool flag37 = flag32 && (!flag29 && !flag30);
					bool flag38 = (flag33 && flag24) || (flag34 && flag14) || (flag37 && flag15) || (flag35 && flag16) || (flag36 && flag17) || (flag29 && flag31 && flag10) || (flag29 && flag32 && flag11) || (flag30 && flag31 && flag12) || (flag30 && flag32 && flag13);
					Vector3[] array2 = this.vertexPositions;
					int num7 = m;
					array2[num7].y = array2[num7].y + (flag38 ? 1f : MeshGenerator.cRingHeights[m]);
				}
			}
			if (flag)
			{
				for (int n = 0; n < this.vertexPositions.Length; n++)
				{
					Vector3 vector = this.vertexPositions[n] - vector3i;
					this.waterClippingVolume.ApplyClipping(ref vector);
					this.vertexPositions[n] = vector + vector3i;
				}
			}
			for (int num8 = 0; num8 < 4; num8++)
			{
				for (int num9 = 0; num9 < 4; num9++)
				{
					int num10 = num8 + num9 * 5;
					this.cpyVerts[0] = this.vertexPositions[num10];
					this.cpyVerts[1] = this.vertexPositions[num10 + 1];
					this.cpyVerts[2] = this.vertexPositions[num10 + 1 + 5];
					this.cpyVerts[3] = this.vertexPositions[num10 + 5];
					bool flag39 = num8 < 2 == num9 < 2;
					WaterMeshUtils.RenderFace(this.cpyVerts, this.lightingAround, 0L, _meshes, new Vector2(0f, 0f), flag39);
				}
			}
		}
	}

	// Token: 0x0600346D RID: 13421 RVA: 0x001517D8 File Offset: 0x0014F9D8
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool <RenderTopWater>g__NeighborBlocksFlow|63_0(int x, int z, BlockFace face)
	{
		return !this.IsTerrain(this.neighborType[0, x, z]) && !MeshGenerator.FacePermitsFlow(this.neighborFlags[x, z], face);
	}

	// Token: 0x0600346E RID: 13422 RVA: 0x00151808 File Offset: 0x0014FA08
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool <RenderTopWater>g__CheckSolidWall|63_1(int x, int z, bool isTerrain, BlockFace blockFace)
	{
		if (!isTerrain && !MeshGenerator.FacePermitsFlow(this.neighborFlags[x, z], blockFace))
		{
			return true;
		}
		Vector3i vector3i = BlockFaceFlags.OffsetIForFace(blockFace);
		BlockFace blockFace2 = BlockFaceFlags.OppositeFace(blockFace);
		return this.<RenderTopWater>g__NeighborBlocksFlow|63_0(x + vector3i.x, z + vector3i.z, blockFace2);
	}

	// Token: 0x0600346F RID: 13423 RVA: 0x0015185C File Offset: 0x0014FA5C
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool <RenderTopWater>g__CheckRaisableSide|63_2(bool centerIsWater, int x, int z, bool otherNeighborIsWater)
	{
		if (centerIsWater)
		{
			return this.IsRaisableNeighbor(0, x, z);
		}
		return this.neighborIsWater[0, x, z] || (this.IsRaisable(this.neighborType[0, x, z]) && otherNeighborIsWater);
	}

	// Token: 0x06003470 RID: 13424 RVA: 0x00151894 File Offset: 0x0014FA94
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool <RenderTopWater>g__CheckRaisableCorner|63_3(int x, int z, BlockFace xFace, BlockFace zFace, bool solidWallX, bool solidWallZ)
	{
		int num = ((xFace == BlockFace.East) ? 1 : (-1));
		int num2 = ((zFace == BlockFace.North) ? 1 : (-1));
		int num3 = x + num;
		int num4 = z + num2;
		BlockFace blockFace = BlockFaceFlags.OppositeFace(xFace);
		BlockFace blockFace2 = BlockFaceFlags.OppositeFace(zFace);
		if (this.neighborIsWater[0, num3, num4])
		{
			if ((this.IsRaisableNeighbor(0, x, num4) || this.<RenderTopWater>g__NeighborBlocksFlow|63_0(x, num4, xFace) || this.<RenderTopWater>g__NeighborBlocksFlow|63_0(num3, num4, blockFace)) && (this.IsRaisableNeighbor(0, num3, z) || this.<RenderTopWater>g__NeighborBlocksFlow|63_0(num3, z, zFace) || this.<RenderTopWater>g__NeighborBlocksFlow|63_0(num3, num4, blockFace2)))
			{
				return true;
			}
		}
		else
		{
			if (this.IsRaisable(this.neighborType[0, num3, num4]))
			{
				return true;
			}
			if ((solidWallZ || this.<RenderTopWater>g__NeighborBlocksFlow|63_0(x, num4, xFace) || this.<RenderTopWater>g__NeighborBlocksFlow|63_0(num3, num4, blockFace)) && (solidWallX || this.<RenderTopWater>g__NeighborBlocksFlow|63_0(num3, z, zFace) || this.<RenderTopWater>g__NeighborBlocksFlow|63_0(num3, num4, blockFace2)))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06003471 RID: 13425 RVA: 0x00151978 File Offset: 0x0014FB78
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool <RenderTopWater>g__CheckNeighborHasRaisableCorner|63_4(int x, int z, BlockFace xFace, BlockFace zFace)
	{
		int num = ((xFace == BlockFace.East) ? 1 : (-1));
		int num2 = ((zFace == BlockFace.North) ? 1 : (-1));
		int num3 = x + num;
		int num4 = z + num2;
		BlockFace blockFace = BlockFaceFlags.OppositeFace(xFace);
		BlockFace blockFace2 = BlockFaceFlags.OppositeFace(zFace);
		if (this.neighborIsWater[0, num3, num4])
		{
			return (this.IsRaisableNeighbor(0, x, num4) || this.<RenderTopWater>g__NeighborBlocksFlow|63_0(x, num4, xFace) || this.<RenderTopWater>g__NeighborBlocksFlow|63_0(num3, num4, blockFace)) && (this.IsRaisableNeighbor(0, num3, z) || this.<RenderTopWater>g__NeighborBlocksFlow|63_0(num3, z, zFace) || this.<RenderTopWater>g__NeighborBlocksFlow|63_0(num3, num4, blockFace2));
		}
		if (this.IsRaisable(this.neighborType[0, num3, num4]))
		{
			return true;
		}
		bool flag = this.IsTerrain(this.neighborType[0, x, z]);
		return (this.<RenderTopWater>g__CheckSolidWall|63_1(x, z, flag, zFace) || this.<RenderTopWater>g__NeighborBlocksFlow|63_0(x, num4, xFace) || this.<RenderTopWater>g__NeighborBlocksFlow|63_0(num3, num4, blockFace)) && (this.<RenderTopWater>g__CheckSolidWall|63_1(x, z, flag, xFace) || this.<RenderTopWater>g__NeighborBlocksFlow|63_0(num3, z, zFace) || this.<RenderTopWater>g__NeighborBlocksFlow|63_0(num3, num4, blockFace2));
	}

	// Token: 0x06003472 RID: 13426 RVA: 0x00151A88 File Offset: 0x0014FC88
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool <RenderTopWater>g__NeighborIsVisualWater|63_5(int x, int z, int offsetX, int offsetZ)
	{
		return this.neighborIsWater[0, x, z] || (this.IsRaisable(this.neighborType[0, x, z]) && (this.neighborIsWater[0, x + offsetX, z + offsetZ] || this.neighborIsWater[0, x - offsetX, z - offsetZ] || this.neighborIsWater[0, 1, 1]));
	}

	// Token: 0x06003473 RID: 13427 RVA: 0x00151AFC File Offset: 0x0014FCFC
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool <RenderTopWater>g__TryPrepareWaterClippingVolume|63_6(BlockValue _waterClippingBV)
	{
		if (!_waterClippingBV.Block.WaterClipEnabled)
		{
			return false;
		}
		Plane waterClipPlane = _waterClippingBV.Block.WaterClipPlane;
		Quaternion rotationStatic = BlockShapeNew.GetRotationStatic((int)_waterClippingBV.rotation);
		GeometryUtils.RotatePlaneAroundPoint(ref waterClipPlane, WaterClippingUtils.CubeBounds.center, rotationStatic);
		bool flag = this.neighborIsWater[0, 1, 2];
		bool flag2 = this.neighborIsWater[0, 2, 1];
		bool flag3 = this.neighborIsWater[0, 1, 0];
		bool flag4 = this.neighborIsWater[0, 0, 1];
		if (flag && flag2 && flag3 && flag4)
		{
			return false;
		}
		bool flag5 = this.neighborIsWater[0, 1, 1];
		bool flag6 = false;
		bool flag7 = false;
		if (flag5)
		{
			if (waterClipPlane.GetDistanceToPoint(new Vector3(0.5f, 0.5f, 0.5f)) > 0f)
			{
				flag6 = true;
			}
			else
			{
				flag7 = true;
			}
		}
		if (flag)
		{
			if (waterClipPlane.GetDistanceToPoint(new Vector3(0.5f, 0.5f, 1f)) > 0f)
			{
				flag6 = true;
			}
			else
			{
				flag7 = true;
			}
		}
		if (flag2)
		{
			if (waterClipPlane.GetDistanceToPoint(new Vector3(1f, 0.5f, 0.5f)) > 0f)
			{
				flag6 = true;
			}
			else
			{
				flag7 = true;
			}
		}
		if (flag3)
		{
			if (waterClipPlane.GetDistanceToPoint(new Vector3(0.5f, 0.5f, 0f)) > 0f)
			{
				flag6 = true;
			}
			else
			{
				flag7 = true;
			}
		}
		if (flag4)
		{
			if (waterClipPlane.GetDistanceToPoint(new Vector3(0f, 0.5f, 0.5f)) > 0f)
			{
				flag6 = true;
			}
			else
			{
				flag7 = true;
			}
		}
		if (flag6 == flag7)
		{
			return false;
		}
		if (flag6)
		{
			waterClipPlane.Flip();
		}
		this.waterClippingVolume.Prepare(waterClipPlane);
		return true;
	}

	// Token: 0x04002931 RID: 10545
	[PublicizedFrom(EAccessModifier.Private)]
	public INeighborBlockCache nBlocks;

	// Token: 0x04002932 RID: 10546
	[PublicizedFrom(EAccessModifier.Private)]
	public LightingAround lightingAround = new LightingAround(0, 0, 0);

	// Token: 0x04002933 RID: 10547
	[PublicizedFrom(EAccessModifier.Private)]
	public Lighting3DArray lightCube = new Lighting3DArray();

	// Token: 0x04002934 RID: 10548
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3[] verticesCube = new Vector3[8];

	// Token: 0x04002935 RID: 10549
	[PublicizedFrom(EAccessModifier.Protected)]
	public int[] heights = new int[9];

	// Token: 0x04002936 RID: 10550
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cWaterDensity = 4;

	// Token: 0x04002937 RID: 10551
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cWaterSize = 0.25f;

	// Token: 0x04002938 RID: 10552
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cWaterFloatHeight = -1.5f;

	// Token: 0x04002939 RID: 10553
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3[] cpyVerts = new Vector3[4];

	// Token: 0x0400293A RID: 10554
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker drawSideMarker = new ProfilerMarker("DrawSide");

	// Token: 0x0400293B RID: 10555
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker sboMarker = new ProfilerMarker("sbo");

	// Token: 0x0400293C RID: 10556
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker blockGetMarker = new ProfilerMarker("blockGet");

	// Token: 0x0400293D RID: 10557
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker stabMarker = new ProfilerMarker("Stab");

	// Token: 0x0400293E RID: 10558
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker isWaterMarker = new ProfilerMarker("IsWater");

	// Token: 0x0400293F RID: 10559
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker swizzleCopyMarker = new ProfilerMarker("SwizzleCopy");

	// Token: 0x04002940 RID: 10560
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker isRenderFaceMarker = new ProfilerMarker("isRenderFace");

	// Token: 0x04002941 RID: 10561
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker getChunkTexMarker = new ProfilerMarker("GetChunkTex");

	// Token: 0x04002942 RID: 10562
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker renderFaceMarker = new ProfilerMarker("renderFace");

	// Token: 0x04002943 RID: 10563
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker waterMarker = new ProfilerMarker("water");

	// Token: 0x04002944 RID: 10564
	[PublicizedFrom(EAccessModifier.Private)]
	public const float ringHeight1 = 0.9f;

	// Token: 0x04002945 RID: 10565
	[PublicizedFrom(EAccessModifier.Private)]
	public const float ringHeight1c = 0.796f;

	// Token: 0x04002946 RID: 10566
	[PublicizedFrom(EAccessModifier.Private)]
	public const float ringHeight2 = 1f;

	// Token: 0x04002947 RID: 10567
	[PublicizedFrom(EAccessModifier.Private)]
	public int[,,] neighborType = new int[2, 3, 3];

	// Token: 0x04002948 RID: 10568
	[PublicizedFrom(EAccessModifier.Private)]
	public bool[,,] neighborIsWater = new bool[2, 3, 3];

	// Token: 0x04002949 RID: 10569
	[PublicizedFrom(EAccessModifier.Private)]
	public WaterClippingVolume waterClippingVolume = new WaterClippingVolume();

	// Token: 0x0400294A RID: 10570
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cVertRes = 5;

	// Token: 0x0400294B RID: 10571
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Vector3[] vertexPositions = new Vector3[25];

	// Token: 0x0400294C RID: 10572
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly float[] cRingHeights = new float[]
	{
		0f, 0f, 0f, 0f, 0f, 0f, 0.796f, 0.9f, 0.796f, 0f,
		0f, 0.9f, 1f, 0.9f, 0f, 0f, 0.796f, 0.9f, 0.796f, 0f,
		0f, 0f, 0f, 0f, 0f
	};

	// Token: 0x0400294D RID: 10573
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker renderTopWaterNewMarker = new ProfilerMarker("MeshGenerator.RenderTopWaterNew");

	// Token: 0x0400294E RID: 10574
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker raisableSidesAndCornersMarker = new ProfilerMarker("MeshGenerator.RaisableSidesAndCorners");

	// Token: 0x0400294F RID: 10575
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockFaceFlag[,] neighborFlags = new BlockFaceFlag[3, 3];

	// Token: 0x0200064D RID: 1613
	[PublicizedFrom(EAccessModifier.Private)]
	public struct DrawSideParams
	{
		// Token: 0x04002950 RID: 10576
		public BlockFace face;

		// Token: 0x04002951 RID: 10577
		public BlockValue midBV;

		// Token: 0x04002952 RID: 10578
		public INeighborBlockCache nBlocks;

		// Token: 0x04002953 RID: 10579
		public Vector3[] v4Arr;

		// Token: 0x04002954 RID: 10580
		public float biomeTemperature;

		// Token: 0x04002955 RID: 10581
		public bool isWaterTopDrawn;

		// Token: 0x04002956 RID: 10582
		public bool isWaterLayerStarted;

		// Token: 0x04002957 RID: 10583
		public Vector3 drawPos;

		// Token: 0x04002958 RID: 10584
		public VoxelMesh[] _meshes;

		// Token: 0x04002959 RID: 10585
		public Vector3i posI;

		// Token: 0x0400295A RID: 10586
		public Vector3i _worldPos;
	}
}
