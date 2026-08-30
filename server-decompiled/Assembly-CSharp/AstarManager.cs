using System;
using System.Collections;
using System.Collections.Generic;
using GamePath;
using Pathfinding;
using UnityEngine;

// Token: 0x020008C7 RID: 2247
public class AstarManager : MonoBehaviour
{
	// Token: 0x0600411F RID: 16671 RVA: 0x00196A54 File Offset: 0x00194C54
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

	// Token: 0x06004120 RID: 16672 RVA: 0x00196A94 File Offset: 0x00194C94
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

	// Token: 0x06004121 RID: 16673 RVA: 0x00196B24 File Offset: 0x00194D24
	public static PathNavigate CreateNavigator(EntityAlive _entity)
	{
		return new ASPPathNavigate(_entity);
	}

	// Token: 0x06004122 RID: 16674 RVA: 0x00196B2C File Offset: 0x00194D2C
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

	// Token: 0x06004123 RID: 16675 RVA: 0x00196BCA File Offset: 0x00194DCA
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

	// Token: 0x06004124 RID: 16676 RVA: 0x00196BDC File Offset: 0x00194DDC
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

	// Token: 0x06004125 RID: 16677 RVA: 0x00196C58 File Offset: 0x00194E58
	public void AddLocationLine(Vector3 startPos, Vector3 endPos, int size)
	{
		startPos.y = 0f;
		endPos.y = 0f;
		Vector3 normalized = (endPos - startPos).normalized;
		Vector3 vector = startPos + normalized * ((float)size * 0.4f);
		this.AddLocation(vector, size);
	}

	// Token: 0x06004126 RID: 16678 RVA: 0x00196CAC File Offset: 0x00194EAC
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

	// Token: 0x06004127 RID: 16679 RVA: 0x00196D14 File Offset: 0x00194F14
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

	// Token: 0x06004128 RID: 16680 RVA: 0x00196F14 File Offset: 0x00195114
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

	// Token: 0x06004129 RID: 16681 RVA: 0x00196FB8 File Offset: 0x001951B8
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

	// Token: 0x0600412A RID: 16682 RVA: 0x00196FF0 File Offset: 0x001951F0
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

	// Token: 0x0600412B RID: 16683 RVA: 0x00197090 File Offset: 0x00195290
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

	// Token: 0x0600412C RID: 16684 RVA: 0x001970F0 File Offset: 0x001952F0
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

	// Token: 0x0600412D RID: 16685 RVA: 0x00197128 File Offset: 0x00195328
	[PublicizedFrom(EAccessModifier.Private)]
	public void MoveGraph(AstarVoxelGrid graph, Vector2 pos)
	{
		this.moveCurrent = graph;
		Vector2 vector = pos - this.worldOriginXZ;
		Vector3 vector2 = this.LocalPosToGridPos(vector);
		graph.Move(vector2);
	}

	// Token: 0x0600412E RID: 16686 RVA: 0x00197158 File Offset: 0x00195358
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 LocalPosToGridPos(Vector2 pos)
	{
		Vector3 vector;
		vector.x = Mathf.Round(pos.x);
		vector.z = Mathf.Round(pos.y);
		vector.y = -32f - this.worldOrigin.y;
		return vector;
	}

	// Token: 0x0600412F RID: 16687 RVA: 0x001971A4 File Offset: 0x001953A4
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

	// Token: 0x06004130 RID: 16688 RVA: 0x00197210 File Offset: 0x00195410
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

	// Token: 0x06004131 RID: 16689 RVA: 0x00197220 File Offset: 0x00195420
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

	// Token: 0x06004132 RID: 16690 RVA: 0x00197434 File Offset: 0x00195634
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

	// Token: 0x06004133 RID: 16691 RVA: 0x00197470 File Offset: 0x00195670
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

	// Token: 0x06004134 RID: 16692 RVA: 0x00197540 File Offset: 0x00195740
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

	// Token: 0x06004135 RID: 16693 RVA: 0x001975A4 File Offset: 0x001957A4
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

	// Token: 0x06004136 RID: 16694 RVA: 0x001975F0 File Offset: 0x001957F0
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

	// Token: 0x06004137 RID: 16695 RVA: 0x00197698 File Offset: 0x00195898
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

	// Token: 0x06004138 RID: 16696 RVA: 0x001976DC File Offset: 0x001958DC
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

	// Token: 0x06004139 RID: 16697 RVA: 0x001977B0 File Offset: 0x001959B0
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

	// Token: 0x04003485 RID: 13445
	public static AstarManager Instance;

	// Token: 0x04003486 RID: 13446
	public const float cGridHeight = 320f;

	// Token: 0x04003487 RID: 13447
	public const float cGridY = -32f;

	// Token: 0x04003488 RID: 13448
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int cGridXZSize = 76;

	// Token: 0x04003489 RID: 13449
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cMoveDist = 10f;

	// Token: 0x0400348A RID: 13450
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cCharHeight = 1.8f;

	// Token: 0x0400348B RID: 13451
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cCharDiameter = 0.3f;

	// Token: 0x0400348C RID: 13452
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cLocationFindPer = 0.2f;

	// Token: 0x0400348D RID: 13453
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cLocationDuration = 4f;

	// Token: 0x0400348E RID: 13454
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cPlayerMergeDist = 19f;

	// Token: 0x0400348F RID: 13455
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cPlayerMergeDistSq = 361f;

	// Token: 0x04003490 RID: 13456
	public const float cUpdateDeltaTime = 0.1f;

	// Token: 0x04003491 RID: 13457
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AstarPath astar;

	// Token: 0x04003492 RID: 13458
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float lastWorkTime;

	// Token: 0x04003493 RID: 13459
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 worldOrigin;

	// Token: 0x04003494 RID: 13460
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector2 worldOriginXZ;

	// Token: 0x04003495 RID: 13461
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<AstarManager.Area> areaList = new List<AstarManager.Area>();

	// Token: 0x04003496 RID: 13462
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<AstarVoxelGrid> graphList = new List<AstarVoxelGrid>();

	// Token: 0x04003497 RID: 13463
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<AstarManager.Location> locations = new List<AstarManager.Location>();

	// Token: 0x04003498 RID: 13464
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<AstarManager.MergedLocations> mergedLocations = new List<AstarManager.MergedLocations>();

	// Token: 0x04003499 RID: 13465
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<AstarVoxelGrid> moveList = new List<AstarVoxelGrid>();

	// Token: 0x0400349A RID: 13466
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AstarVoxelGrid moveCurrent;

	// Token: 0x0400349B RID: 13467
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int[] updateBlockOffsets = new int[] { -1, 0, 1, 0, 0, -1, 0, 1 };

	// Token: 0x020008C8 RID: 2248
	public class Area
	{
		// Token: 0x0400349C RID: 13468
		public AstarManager.Area next;

		// Token: 0x0400349D RID: 13469
		public Vector2i pos;

		// Token: 0x0400349E RID: 13470
		public AstarManager.Bounds2i bounds;

		// Token: 0x0400349F RID: 13471
		public bool hasBlocks;

		// Token: 0x040034A0 RID: 13472
		public bool isPartial;

		// Token: 0x040034A1 RID: 13473
		public bool isSlowUpdate;

		// Token: 0x040034A2 RID: 13474
		public float updateDelay;
	}

	// Token: 0x020008C9 RID: 2249
	public struct Bounds2i
	{
		// Token: 0x0600413D RID: 16701 RVA: 0x001978AC File Offset: 0x00195AAC
		public bool Contains(Vector2i pos)
		{
			return pos.x >= this.min.x && pos.x <= this.max.x && pos.y >= this.min.y && pos.y <= this.max.y;
		}

		// Token: 0x0600413E RID: 16702 RVA: 0x00197908 File Offset: 0x00195B08
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

		// Token: 0x0600413F RID: 16703 RVA: 0x001979A8 File Offset: 0x00195BA8
		public Bounds ToBounds()
		{
			Bounds bounds = default(Bounds);
			bounds.SetMinMax(new Vector3((float)this.min.x, 0f, (float)this.min.y), new Vector3((float)this.max.x + 0.999999f, 0f, (float)this.max.y + 0.999999f));
			return bounds;
		}

		// Token: 0x040034A3 RID: 13475
		public Vector2i min;

		// Token: 0x040034A4 RID: 13476
		public Vector2i max;
	}

	// Token: 0x020008CA RID: 2250
	[PublicizedFrom(EAccessModifier.Private)]
	public class Location
	{
		// Token: 0x040034A5 RID: 13477
		public Vector2 pos;

		// Token: 0x040034A6 RID: 13478
		public int size;

		// Token: 0x040034A7 RID: 13479
		public float duration;
	}

	// Token: 0x020008CB RID: 2251
	[PublicizedFrom(EAccessModifier.Private)]
	public struct MergedLocations
	{
		// Token: 0x040034A8 RID: 13480
		public Vector2 pos;

		// Token: 0x040034A9 RID: 13481
		public int size;
	}
}
