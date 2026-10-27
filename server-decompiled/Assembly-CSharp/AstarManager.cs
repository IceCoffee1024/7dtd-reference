using System;
using System.Collections;
using System.Collections.Generic;
using GamePath;
using Pathfinding;
using UnityEngine;

// Token: 0x020008E2 RID: 2274
public class AstarManager : MonoBehaviour
{
	// Token: 0x0600413D RID: 16701 RVA: 0x00199CE8 File Offset: 0x00197EE8
	public static void Init(GameObject obj)
	{
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			return;
		}
		if (GamePrefs.GetString(EnumGamePrefs.GameWorld) == "Empty")
		{
			return;
		}
		Log.Out("AstarManager Init");
		obj.AddComponent<AstarManager>();
		new ASPPathFinderThread().StartWorkerThreads();
	}

	// Token: 0x0600413E RID: 16702 RVA: 0x00199D28 File Offset: 0x00197F28
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		AstarManager.Instance = this;
		if (!AstarPath.active)
		{
			global::UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/AStarPath"), Vector3.zero, Quaternion.identity).transform.SetParent(GameManager.Instance.transform, false);
		}
		this.astar = AstarPath.active;
		ChunkCluster chunkCache = GameManager.Instance.World.ChunkCache;
		chunkCache.OnBlockChangedDelegates += this.OnBlockChanged;
		chunkCache.OnBlockDamagedDelegates += this.OnBlockDamaged;
		this.OriginChanged();
	}

	// Token: 0x0600413F RID: 16703 RVA: 0x00199DB8 File Offset: 0x00197FB8
	public static PathNavigate CreateNavigator(EntityAlive _entity)
	{
		return new ASPPathNavigate(_entity);
	}

	// Token: 0x06004140 RID: 16704 RVA: 0x00199DC0 File Offset: 0x00197FC0
	public static void Cleanup()
	{
		if (!AstarManager.Instance)
		{
			return;
		}
		Log.Out("AstarManager Cleanup");
		ChunkCluster chunkCache = GameManager.Instance.World.ChunkCache;
		chunkCache.OnBlockChangedDelegates -= AstarManager.Instance.OnBlockChanged;
		chunkCache.OnBlockDamagedDelegates -= AstarManager.Instance.OnBlockDamaged;
		PathFinderThread.Instance.Cleanup();
		if (AstarPath.active)
		{
			AstarPath.active.enabled = false;
			global::UnityEngine.Object.Destroy(AstarPath.active.gameObject);
		}
		global::UnityEngine.Object.Destroy(AstarManager.Instance);
		AstarManager.Instance = null;
	}

	// Token: 0x06004141 RID: 16705 RVA: 0x00199E5E File Offset: 0x0019805E
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator Start()
	{
		float elapsedTime = 0f;
		while (this.astar != null)
		{
			if (GamePrefs.GetBool(EnumGamePrefs.DebugStopEnemiesMoving))
			{
				yield return new WaitForSeconds(0.1f);
			}
			else
			{
				yield return new WaitForSeconds(0.1f);
				if (!(GameManager.Instance == null) && GameManager.Instance.World != null)
				{
					elapsedTime += 0.1f;
					if (this.astar.IsAnyWorkItemInProgress)
					{
						this.lastWorkTime = Time.time;
					}
					else if (Time.time - this.lastWorkTime >= 0.4f && !this.astar.IsAnyGraphUpdateInProgress)
					{
						this.UpdateGraphs(elapsedTime);
						int num = this.areaList.Count;
						if (num > 0)
						{
							num = Mathf.Min(20, num);
							int num2 = 0;
							for (int i = 0; i < num; i++)
							{
								AstarManager.Area area = this.areaList[num2];
								area.updateDelay -= elapsedTime;
								if (area.updateDelay > 0f)
								{
									num2++;
								}
								else
								{
									if (area.next == null)
									{
										this.areaList.RemoveAt(num2);
									}
									else
									{
										this.areaList[num2] = area.next;
										num2++;
									}
									Bounds bounds;
									if (!area.isPartial)
									{
										bounds = default(Bounds);
										Vector3 vector = new Vector3((float)area.pos.x, 0f, (float)area.pos.y);
										Vector3 vector2 = vector;
										vector2.x += 16f;
										vector2.z += 16f;
										bounds.SetMinMax(vector, vector2);
									}
									else
									{
										if (!area.hasBlocks)
										{
											goto IL_0294;
										}
										bounds = area.bounds.ToBounds();
									}
									Vector3 vector3 = bounds.center;
									vector3.y = 128f;
									vector3 -= this.worldOrigin;
									bounds.center = vector3;
									Vector3 size = bounds.size;
									size.y = 320f;
									bounds.size = size;
									if (this.graphList.Count > 0)
									{
										LayerGridGraphUpdate layerGridGraphUpdate = new LayerGridGraphUpdate();
										layerGridGraphUpdate.bounds = bounds;
										layerGridGraphUpdate.recalculateNodes = true;
										this.astar.UpdateGraphs(layerGridGraphUpdate);
									}
								}
								IL_0294:;
							}
						}
						elapsedTime = 0f;
					}
				}
			}
		}
		yield break;
	}

	// Token: 0x06004142 RID: 16706 RVA: 0x00199E70 File Offset: 0x00198070
	public void AddLocation(Vector3 pos3d, int size)
	{
		Vector2 vector;
		vector.x = pos3d.x;
		vector.y = pos3d.z;
		AstarManager.Location location = this.FindLocation(vector, size);
		if (location == null)
		{
			location = new AstarManager.Location();
			location.pos = vector;
			location.size = size;
			this.locations.Add(location);
		}
		else
		{
			location.pos = (location.pos + vector) * 0.5f;
		}
		location.duration = 4f;
	}

	// Token: 0x06004143 RID: 16707 RVA: 0x00199EEC File Offset: 0x001980EC
	public void AddLocationLine(Vector3 startPos, Vector3 endPos, int size)
	{
		startPos.y = 0f;
		endPos.y = 0f;
		Vector3 normalized = (endPos - startPos).normalized;
		Vector3 vector = startPos + normalized * ((float)size * 0.4f);
		this.AddLocation(vector, size);
	}

	// Token: 0x06004144 RID: 16708 RVA: 0x00199F40 File Offset: 0x00198140
	[PublicizedFrom(EAccessModifier.Private)]
	public AstarManager.Location FindLocation(Vector2 pos, int size)
	{
		AstarManager.Location location = null;
		float num = (float)(size * size) * 0.040000003f;
		for (int i = 0; i < this.locations.Count; i++)
		{
			AstarManager.Location location2 = this.locations[i];
			if (location2.size >= size)
			{
				float sqrMagnitude = (location2.pos - pos).sqrMagnitude;
				if (sqrMagnitude < num)
				{
					location = location2;
					num = sqrMagnitude;
				}
			}
		}
		return location;
	}

	// Token: 0x06004145 RID: 16709 RVA: 0x00199FA8 File Offset: 0x001981A8
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateGraphs(float deltaTime)
	{
		World world = GameManager.Instance.World;
		this.mergedLocations.Clear();
		List<EntityPlayer> list = world.Players.list;
		for (int i = 0; i < list.Count; i++)
		{
			EntityPlayer entityPlayer = list[i];
			Vector2 vector;
			vector.x = entityPlayer.position.x;
			vector.y = entityPlayer.position.z;
			this.Merge(vector, 76);
		}
		for (int j = 0; j < this.locations.Count; j++)
		{
			AstarManager.Location location = this.locations[j];
			location.duration -= deltaTime;
			if (location.duration <= 0f)
			{
				this.locations.RemoveAt(j);
				j--;
			}
			else
			{
				this.Merge(location.pos, location.size);
			}
		}
		for (int k = 0; k < this.graphList.Count; k++)
		{
			this.graphList[k].IsUsed = false;
		}
		for (int l = 0; l < this.mergedLocations.Count; l++)
		{
			AstarManager.MergedLocations mergedLocations = this.mergedLocations[l];
			AstarVoxelGrid astarVoxelGrid = this.FindClosestGraph(mergedLocations.pos, mergedLocations.size);
			if (astarVoxelGrid == null)
			{
				astarVoxelGrid = this.AddGraph(mergedLocations.size);
				astarVoxelGrid.SetPos(this.LocalPosToGridPos(mergedLocations.pos - this.worldOriginXZ));
			}
			astarVoxelGrid.IsUsed = true;
			this.UpdateGraphPos(astarVoxelGrid, mergedLocations.pos);
		}
		this.UpdateMoveGraph();
		for (int m = 0; m < this.graphList.Count; m++)
		{
			AstarVoxelGrid astarVoxelGrid2 = this.graphList[m];
			if (!astarVoxelGrid2.IsUsed)
			{
				this.MoveGraphRemove(astarVoxelGrid2);
				this.astar.data.RemoveGraph(astarVoxelGrid2);
				this.graphList.RemoveAt(m);
				m--;
			}
		}
	}

	// Token: 0x06004146 RID: 16710 RVA: 0x0019A1A8 File Offset: 0x001983A8
	[PublicizedFrom(EAccessModifier.Private)]
	public void Merge(Vector2 pos, int size)
	{
		bool flag = false;
		for (int i = 0; i < this.mergedLocations.Count; i++)
		{
			AstarManager.MergedLocations mergedLocations = this.mergedLocations[i];
			if (size <= mergedLocations.size && (mergedLocations.pos - pos).sqrMagnitude <= 361f)
			{
				mergedLocations.pos = (mergedLocations.pos + pos) * 0.5f;
				this.mergedLocations[i] = mergedLocations;
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			AstarManager.MergedLocations mergedLocations2;
			mergedLocations2.pos = pos;
			mergedLocations2.size = size;
			this.mergedLocations.Add(mergedLocations2);
		}
	}

	// Token: 0x06004147 RID: 16711 RVA: 0x0019A24C File Offset: 0x0019844C
	[PublicizedFrom(EAccessModifier.Private)]
	public int FindMoveIndex(AstarVoxelGrid graph)
	{
		for (int i = 0; i < this.moveList.Count; i++)
		{
			if (this.moveList[i] == graph)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06004148 RID: 16712 RVA: 0x0019A284 File Offset: 0x00198484
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateGraphPos(AstarVoxelGrid graph, Vector2 pos)
	{
		if (graph.IsMoving())
		{
			return;
		}
		Vector2 vector = pos - this.worldOriginXZ;
		if (graph.IsFullUpdateNeeded)
		{
			Vector3 vector2 = this.LocalPosToGridPos(vector);
			graph.SetPos(vector2);
			return;
		}
		Vector2 vector3 = vector;
		vector3.x -= graph.center.x;
		vector3.y -= graph.center.z;
		if (Vector2.SqrMagnitude(vector3) > 100f)
		{
			graph.GridMovePendingPos = pos;
			if (this.FindMoveIndex(graph) < 0)
			{
				this.moveList.Insert(this.moveList.Count, graph);
			}
		}
	}

	// Token: 0x06004149 RID: 16713 RVA: 0x0019A324 File Offset: 0x00198524
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateMoveGraph()
	{
		if (this.moveCurrent != null)
		{
			if (this.moveCurrent.IsMoving())
			{
				return;
			}
			this.moveCurrent = null;
		}
		if (this.moveList.Count > 0)
		{
			AstarVoxelGrid astarVoxelGrid = this.moveList[0];
			this.moveList.RemoveAt(0);
			this.MoveGraph(astarVoxelGrid, astarVoxelGrid.GridMovePendingPos);
		}
	}

	// Token: 0x0600414A RID: 16714 RVA: 0x0019A384 File Offset: 0x00198584
	[PublicizedFrom(EAccessModifier.Private)]
	public void MoveGraphRemove(AstarVoxelGrid graph)
	{
		if (this.moveCurrent == graph)
		{
			this.moveCurrent = null;
		}
		int num = this.FindMoveIndex(graph);
		if (num >= 0)
		{
			this.moveList.RemoveAt(num);
		}
	}

	// Token: 0x0600414B RID: 16715 RVA: 0x0019A3BC File Offset: 0x001985BC
	[PublicizedFrom(EAccessModifier.Private)]
	public void MoveGraph(AstarVoxelGrid graph, Vector2 pos)
	{
		this.moveCurrent = graph;
		Vector2 vector = pos - this.worldOriginXZ;
		Vector3 vector2 = this.LocalPosToGridPos(vector);
		graph.Move(vector2);
	}

	// Token: 0x0600414C RID: 16716 RVA: 0x0019A3EC File Offset: 0x001985EC
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 LocalPosToGridPos(Vector2 pos)
	{
		Vector3 vector;
		vector.x = Mathf.Round(pos.x);
		vector.z = Mathf.Round(pos.y);
		vector.y = -32f - this.worldOrigin.y;
		return vector;
	}

	// Token: 0x0600414D RID: 16717 RVA: 0x0019A438 File Offset: 0x00198638
	public void OriginChanged()
	{
		this.worldOrigin = Origin.position;
		this.worldOriginXZ.x = this.worldOrigin.x;
		this.worldOriginXZ.y = this.worldOrigin.z;
		for (int i = 0; i < this.graphList.Count; i++)
		{
			this.graphList[i].IsFullUpdateNeeded = true;
		}
	}

	// Token: 0x0600414E RID: 16718 RVA: 0x0019A4A4 File Offset: 0x001986A4
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator Scan()
	{
		if (!this.astar.isScanning)
		{
			this.astar.Scan(null);
		}
		yield return null;
		yield break;
	}

	// Token: 0x0600414F RID: 16719 RVA: 0x0019A4B4 File Offset: 0x001986B4
	public void OnBlockChanged(Vector3i pos, BlockValue bvOld, sbyte densOld, TextureFullArray texOld, BlockValue bvNew)
	{
		Block block = bvOld.Block;
		if (block.isMultiBlock)
		{
			int rotation = (int)bvNew.rotation;
			int length = block.multiBlockPos.Length;
			for (int i = 0; i < length; i++)
			{
				Vector3i vector3i = block.multiBlockPos.Get(i, bvNew.type, rotation);
				vector3i += pos;
				this.UpdateBlock(vector3i, false);
			}
		}
		else if (block.isOversized)
		{
			Vector3i vector3i2;
			Vector3i vector3i3;
			OversizedBlockUtils.GetWorldAlignedBoundsExtents(pos, block.shape.GetRotation(bvOld), block.oversizedBounds, out vector3i2, out vector3i3);
			for (int j = vector3i2.x; j <= vector3i3.x; j++)
			{
				for (int k = vector3i2.y; k <= vector3i3.y; k++)
				{
					for (int l = vector3i2.z; l <= vector3i3.z; l++)
					{
						this.UpdateBlock(new Vector3i(j, k, l), false);
					}
				}
			}
		}
		Block block2 = bvNew.Block;
		BlockCompositeTileEntity blockCompositeTileEntity = block2 as BlockCompositeTileEntity;
		bool flag = blockCompositeTileEntity != null && blockCompositeTileEntity.CompositeData.HasFeature<TEFeatureDoor>();
		if (block2.isMultiBlock)
		{
			int rotation2 = (int)bvNew.rotation;
			int length2 = block2.multiBlockPos.Length;
			for (int m = 0; m < length2; m++)
			{
				Vector3i vector3i4 = block2.multiBlockPos.Get(m, bvNew.type, rotation2);
				vector3i4 += pos;
				this.UpdateBlock(vector3i4, flag);
			}
			return;
		}
		if (block2.isOversized)
		{
			Vector3i vector3i5;
			Vector3i vector3i6;
			OversizedBlockUtils.GetWorldAlignedBoundsExtents(pos, block2.shape.GetRotation(bvNew), block2.oversizedBounds, out vector3i5, out vector3i6);
			for (int n = vector3i5.x; n <= vector3i6.x; n++)
			{
				for (int num = vector3i5.y; num <= vector3i6.y; num++)
				{
					for (int num2 = vector3i5.z; num2 <= vector3i6.z; num2++)
					{
						this.UpdateBlock(new Vector3i(n, num, num2), flag);
					}
				}
			}
			return;
		}
		this.UpdateBlock(pos, flag);
	}

	// Token: 0x06004150 RID: 16720 RVA: 0x0019A6C8 File Offset: 0x001988C8
	public void OnBlockDamaged(BlockValueRef _bvRef, BlockValue _blockValue, int _damage, int _attackerEntityId)
	{
		switch (_bvRef.Type)
		{
		case BlockValueRefType.None:
		case BlockValueRefType.Prop:
			return;
		case BlockValueRefType.Block:
			this.UpdateBlock(_bvRef.BlockPosition, false);
			return;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	// Token: 0x06004151 RID: 16721 RVA: 0x0019A704 File Offset: 0x00198904
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateBlock(Vector3i blockPos, bool isSlowUpdate)
	{
		Vector2i vector2i = new Vector2i(blockPos.x, blockPos.z);
		Vector2i vector2i2 = vector2i;
		vector2i2.x &= -16;
		vector2i2.y &= -16;
		AstarManager.Area area = this.AddAreaBlock(vector2i);
		area.hasBlocks = true;
		area.isSlowUpdate = isSlowUpdate;
		for (int i = 0; i < 4; i++)
		{
			Vector2i vector2i3 = vector2i;
			int num = i * 2;
			vector2i3.x += AstarManager.updateBlockOffsets[num];
			vector2i3.y += AstarManager.updateBlockOffsets[num + 1];
			Vector2i vector2i4 = vector2i3;
			vector2i4.x &= -16;
			vector2i4.y &= -16;
			if (vector2i4.x != vector2i2.x || vector2i4.y != vector2i2.y)
			{
				this.AddAreaBlock(vector2i3);
			}
		}
	}

	// Token: 0x06004152 RID: 16722 RVA: 0x0019A7D4 File Offset: 0x001989D4
	public static void AddBoundsToUpdate(Bounds _bounds)
	{
		if (AstarManager.Instance == null)
		{
			return;
		}
		Vector2i vector2i = new Vector2i(Mathf.FloorToInt(_bounds.min.x), Mathf.FloorToInt(_bounds.min.z));
		AstarManager.Area area = AstarManager.Instance.AddArea(vector2i, true);
		if (!area.isSlowUpdate)
		{
			area.updateDelay = 0f;
		}
	}

	// Token: 0x06004153 RID: 16723 RVA: 0x0019A838 File Offset: 0x00198A38
	[PublicizedFrom(EAccessModifier.Private)]
	public AstarManager.Area AddAreaBlock(Vector2i pos)
	{
		AstarManager.Area area = this.AddArea(pos, false);
		if (!area.isPartial)
		{
			area.isPartial = true;
			area.bounds.min = pos;
			area.bounds.max = pos;
		}
		else
		{
			area.bounds.Encapsulate(pos);
		}
		return area;
	}

	// Token: 0x06004154 RID: 16724 RVA: 0x0019A884 File Offset: 0x00198A84
	[PublicizedFrom(EAccessModifier.Private)]
	public AstarManager.Area AddArea(Vector2i pos, bool noNext)
	{
		pos.x &= -16;
		pos.y &= -16;
		AstarManager.Area area = this.FindArea(pos);
		if (area == null)
		{
			area = new AstarManager.Area();
			area.pos = pos;
			area.updateDelay = 2f;
			this.areaList.Add(area);
			return area;
		}
		if (noNext)
		{
			return area;
		}
		if (area.next != null)
		{
			return area.next;
		}
		if (area.updateDelay < 1.5f)
		{
			AstarManager.Area area2 = new AstarManager.Area();
			area2.pos = pos;
			area2.updateDelay = 2f - area.updateDelay;
			area.next = area2;
			return area2;
		}
		return area;
	}

	// Token: 0x06004155 RID: 16725 RVA: 0x0019A92C File Offset: 0x00198B2C
	[PublicizedFrom(EAccessModifier.Private)]
	public AstarManager.Area FindArea(Vector2i pos)
	{
		for (int i = 0; i < this.areaList.Count; i++)
		{
			AstarManager.Area area = this.areaList[i];
			if (area.pos == pos)
			{
				return area;
			}
		}
		return null;
	}

	// Token: 0x06004156 RID: 16726 RVA: 0x0019A970 File Offset: 0x00198B70
	[PublicizedFrom(EAccessModifier.Private)]
	public AstarVoxelGrid AddGraph(int size)
	{
		AstarVoxelGrid astarVoxelGrid = this.astar.data.AddGraph(typeof(AstarVoxelGrid)) as AstarVoxelGrid;
		this.graphList.Add(astarVoxelGrid);
		astarVoxelGrid.Init();
		astarVoxelGrid.neighbours = NumNeighbours.Four;
		astarVoxelGrid.uniformEdgeCosts = false;
		astarVoxelGrid.inspectorGridMode = InspectorGridMode.Grid;
		astarVoxelGrid.characterHeight = 1.8f;
		astarVoxelGrid.SetDimensions(size, size, 1f);
		astarVoxelGrid.maxClimb = 1.3f;
		astarVoxelGrid.maxSlope = 60f;
		astarVoxelGrid.mergeSpanRange = 0.1f;
		GraphCollision collision = astarVoxelGrid.collision;
		collision.collisionCheck = true;
		collision.type = ColliderType.Capsule;
		collision.diameter = 0.3f;
		collision.height = 1.5f;
		collision.collisionOffset = 0.15f;
		collision.mask = 65536;
		return astarVoxelGrid;
	}

	// Token: 0x06004157 RID: 16727 RVA: 0x0019AA44 File Offset: 0x00198C44
	[PublicizedFrom(EAccessModifier.Private)]
	public AstarVoxelGrid FindClosestGraph(Vector2 pos, int size)
	{
		Vector2 vector = pos - this.worldOriginXZ;
		AstarVoxelGrid astarVoxelGrid = null;
		float num = float.MaxValue;
		for (int i = 0; i < this.graphList.Count; i++)
		{
			AstarVoxelGrid astarVoxelGrid2 = this.graphList[i];
			if (!astarVoxelGrid2.IsUsed && astarVoxelGrid2.size.x >= (float)size)
			{
				Vector2 vector2 = vector;
				vector2.x -= astarVoxelGrid2.center.x;
				vector2.y -= astarVoxelGrid2.center.z;
				float num2 = Vector2.SqrMagnitude(vector2);
				if (num2 < num)
				{
					num = num2;
					astarVoxelGrid = astarVoxelGrid2;
				}
			}
		}
		return astarVoxelGrid;
	}

	// Token: 0x0400351A RID: 13594
	public static AstarManager Instance;

	// Token: 0x0400351B RID: 13595
	public const float cGridHeight = 320f;

	// Token: 0x0400351C RID: 13596
	public const float cGridY = -32f;

	// Token: 0x0400351D RID: 13597
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int cGridXZSize = 76;

	// Token: 0x0400351E RID: 13598
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cMoveDist = 10f;

	// Token: 0x0400351F RID: 13599
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cCharHeight = 1.8f;

	// Token: 0x04003520 RID: 13600
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cCharDiameter = 0.3f;

	// Token: 0x04003521 RID: 13601
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cLocationFindPer = 0.2f;

	// Token: 0x04003522 RID: 13602
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cLocationDuration = 4f;

	// Token: 0x04003523 RID: 13603
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cPlayerMergeDist = 19f;

	// Token: 0x04003524 RID: 13604
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cPlayerMergeDistSq = 361f;

	// Token: 0x04003525 RID: 13605
	public const float cUpdateDeltaTime = 0.1f;

	// Token: 0x04003526 RID: 13606
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AstarPath astar;

	// Token: 0x04003527 RID: 13607
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float lastWorkTime;

	// Token: 0x04003528 RID: 13608
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 worldOrigin;

	// Token: 0x04003529 RID: 13609
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector2 worldOriginXZ;

	// Token: 0x0400352A RID: 13610
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<AstarManager.Area> areaList = new List<AstarManager.Area>();

	// Token: 0x0400352B RID: 13611
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<AstarVoxelGrid> graphList = new List<AstarVoxelGrid>();

	// Token: 0x0400352C RID: 13612
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<AstarManager.Location> locations = new List<AstarManager.Location>();

	// Token: 0x0400352D RID: 13613
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<AstarManager.MergedLocations> mergedLocations = new List<AstarManager.MergedLocations>();

	// Token: 0x0400352E RID: 13614
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<AstarVoxelGrid> moveList = new List<AstarVoxelGrid>();

	// Token: 0x0400352F RID: 13615
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AstarVoxelGrid moveCurrent;

	// Token: 0x04003530 RID: 13616
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int[] updateBlockOffsets = new int[] { -1, 0, 1, 0, 0, -1, 0, 1 };

	// Token: 0x020008E3 RID: 2275
	public class Area
	{
		// Token: 0x04003531 RID: 13617
		public AstarManager.Area next;

		// Token: 0x04003532 RID: 13618
		public Vector2i pos;

		// Token: 0x04003533 RID: 13619
		public AstarManager.Bounds2i bounds;

		// Token: 0x04003534 RID: 13620
		public bool hasBlocks;

		// Token: 0x04003535 RID: 13621
		public bool isPartial;

		// Token: 0x04003536 RID: 13622
		public bool isSlowUpdate;

		// Token: 0x04003537 RID: 13623
		public float updateDelay;
	}

	// Token: 0x020008E4 RID: 2276
	public struct Bounds2i
	{
		// Token: 0x0600415B RID: 16731 RVA: 0x0019AB40 File Offset: 0x00198D40
		public bool Contains(Vector2i pos)
		{
			return pos.x >= this.min.x && pos.x <= this.max.x && pos.y >= this.min.y && pos.y <= this.max.y;
		}

		// Token: 0x0600415C RID: 16732 RVA: 0x0019AB9C File Offset: 0x00198D9C
		public void Encapsulate(Vector2i pos)
		{
			if (pos.x < this.min.x)
			{
				this.min.x = pos.x;
			}
			if (pos.x > this.max.x)
			{
				this.max.x = pos.x;
			}
			if (pos.y < this.min.y)
			{
				this.min.y = pos.y;
			}
			if (pos.y > this.max.y)
			{
				this.max.y = pos.y;
			}
		}

		// Token: 0x0600415D RID: 16733 RVA: 0x0019AC3C File Offset: 0x00198E3C
		public Bounds ToBounds()
		{
			Bounds bounds = default(Bounds);
			bounds.SetMinMax(new Vector3((float)this.min.x, 0f, (float)this.min.y), new Vector3((float)this.max.x + 0.999999f, 0f, (float)this.max.y + 0.999999f));
			return bounds;
		}

		// Token: 0x04003538 RID: 13624
		public Vector2i min;

		// Token: 0x04003539 RID: 13625
		public Vector2i max;
	}

	// Token: 0x020008E5 RID: 2277
	[PublicizedFrom(EAccessModifier.Private)]
	public class Location
	{
		// Token: 0x0400353A RID: 13626
		public Vector2 pos;

		// Token: 0x0400353B RID: 13627
		public int size;

		// Token: 0x0400353C RID: 13628
		public float duration;
	}

	// Token: 0x020008E6 RID: 2278
	[PublicizedFrom(EAccessModifier.Private)]
	public struct MergedLocations
	{
		// Token: 0x0400353D RID: 13629
		public Vector2 pos;

		// Token: 0x0400353E RID: 13630
		public int size;
	}
}
