using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Pathfinding;
using UnityEngine;

// Token: 0x020008CE RID: 2254
public class AstarVoxelGrid : LayerGridGraph
{
	// Token: 0x0600414D RID: 16717 RVA: 0x00197D6C File Offset: 0x00195F6C
	public void Init()
	{
		if (AstarVoxelGrid.connectionsPool == null)
		{
			AstarVoxelGrid.connectionsPool = new List<Connection[]>[16];
			for (int i = 0; i < 16; i++)
			{
				AstarVoxelGrid.connectionsPool[i] = new List<Connection[]>();
			}
		}
		this.gridMover = new AstarVoxelGrid.ProceduralGridMover();
	}

	// Token: 0x0600414E RID: 16718 RVA: 0x00197DB0 File Offset: 0x00195FB0
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitScan()
	{
		base.Scan();
	}

	// Token: 0x0600414F RID: 16719 RVA: 0x00197DB8 File Offset: 0x00195FB8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override IEnumerable<Progress> ScanInternal()
	{
		foreach (Progress progress in this.<>n__0())
		{
			yield return progress;
		}
		yield break;
	}

	// Token: 0x06004150 RID: 16720 RVA: 0x00197DC8 File Offset: 0x00195FC8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateArea(GraphUpdateObject o)
	{
		IntRect intRect;
		IntRect intRect2;
		IntRect intRect3;
		bool flag;
		int num;
		base.CalculateAffectedRegions(o, out intRect, out intRect2, out intRect3, out flag, out num);
		IntRect intRect4 = new IntRect(0, 0, this.width - 1, this.depth - 1);
		IntRect intRect5 = IntRect.Intersection(intRect2, intRect4);
		this.collision.Initialize(base.transform, this.nodeSize);
		intRect5 = IntRect.Intersection(intRect3, intRect4);
		for (int i = intRect5.xmin; i <= intRect5.xmax; i++)
		{
			for (int j = intRect5.ymin; j <= intRect5.ymax; j++)
			{
				this.RecalculateCell(i, j, true, false);
			}
		}
		intRect2.Expand(1);
		intRect5 = IntRect.Intersection(intRect2, intRect4);
		for (int k = intRect5.xmin; k <= intRect5.xmax; k++)
		{
			for (int l = intRect5.ymin; l <= intRect5.ymax; l++)
			{
				this.CalculateConnections(k, l);
			}
		}
	}

	// Token: 0x06004151 RID: 16721 RVA: 0x000027FC File Offset: 0x000009FC
	[Conditional("DEBUG_PATHGRIDVALIDATE")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void Validate()
	{
	}

	// Token: 0x06004152 RID: 16722 RVA: 0x00197EC4 File Offset: 0x001960C4
	[PublicizedFrom(EAccessModifier.Private)]
	public void SampleHeights(Vector3 pos)
	{
		this.CheckHeights(pos);
		int num = this.heightsUsed / 2;
		for (int i = 0; i < num; i++)
		{
			AstarVoxelGrid.HitData hitData = this.heights[i];
			this.heights[i] = this.heights[this.heightsUsed - 1 - i];
			this.heights[this.heightsUsed - 1 - i] = hitData;
		}
	}

	// Token: 0x06004153 RID: 16723 RVA: 0x00197F34 File Offset: 0x00196134
	[PublicizedFrom(EAccessModifier.Private)]
	public void CheckHeights(Vector3 position)
	{
		World world = GameManager.Instance.World;
		ChunkCluster chunkCache = world.ChunkCache;
		Vector3 position2 = Origin.position;
		int type = BlockValue.Air.type;
		this.heightsUsed = 0;
		int num = 0;
		Vector3 vector = position;
		vector.y += 320f;
		PhysicsScene defaultPhysicsScene = Physics.defaultPhysicsScene;
		Vector3 down = Vector3.down;
		float num2 = float.MaxValue;
		do
		{
			AstarVoxelGrid.HitData hitData;
			hitData.hitFlags = AstarVoxelGrid.HitDataFlags.None;
			hitData.blockerFlags = 4096;
			RaycastHit raycastHit;
			if (!defaultPhysicsScene.Raycast(vector, down, out raycastHit, 320.01f, 1073807360, QueryTriggerInteraction.Ignore))
			{
				goto IL_01FD;
			}
			vector.y = raycastHit.point.y - 0.11f;
			if (raycastHit.point.y > num2)
			{
				hitData.hitFlags |= AstarVoxelGrid.HitDataFlags.InsideOversized;
			}
			else if (raycastHit.collider.CompareTag("T_Block"))
			{
				this.localWorldRayHit.Clear();
				Transform transform = raycastHit.collider.transform;
				Vector3 vector2 = raycastHit.point + Origin.position;
				if (GameUtils.FindMasterBlockForEntityModelBlock(world, down, string.Empty, vector2, transform, this.localWorldRayHit))
				{
					BlockValue blockValue = this.localWorldRayHit.hit.blockValue;
					if (blockValue.Block.isOversized)
					{
						hitData.hitFlags |= AstarVoxelGrid.HitDataFlags.InsideOversized;
						Quaternion rotation = blockValue.Block.shape.GetRotation(blockValue);
						Vector3i vector3i;
						Vector3i vector3i2;
						OversizedBlockUtils.GetWorldAlignedBoundsExtents(this.localWorldRayHit.hit.blockPos, rotation, blockValue.Block.oversizedBounds, out vector3i, out vector3i2);
						num2 = (float)vector3i.y;
					}
				}
			}
			hitData.point = raycastHit.point;
			hitData.point.y = hitData.point.y + 0.05f;
			if (Vector3.Dot(raycastHit.normal, Vector3.up) < 0.707f)
			{
				hitData.hitFlags |= AstarVoxelGrid.HitDataFlags.InvalidCover;
			}
			AstarVoxelGrid.cellHits[num] = hitData;
		}
		while (++num < 512);
		Log.Warning("AstarVoxelGrid CheckHeights too many hits");
		IL_01FD:
		int num3 = Utils.Fastfloor(position.x + position2.x);
		int num4 = Utils.Fastfloor(position.z + position2.z);
		Vector3i vector3i3 = new Vector3i(num3, 0, num4);
		Vector3i vector3i4 = vector3i3;
		IChunk chunkFromWorldPos = world.GetChunkFromWorldPos(vector3i3);
		if (chunkFromWorldPos == null)
		{
			return;
		}
		int num5 = World.toBlockXZ(num3);
		int num6 = World.toBlockXZ(num4);
		int i = 0;
		float num7 = 257f;
		IL_0920:
		while (i < num)
		{
			float num8 = num7;
			AstarVoxelGrid.HitData hitData2 = AstarVoxelGrid.cellHits[i++];
			num7 = hitData2.point.y;
			float num9 = num8 - num7;
			vector3i3.y = Utils.Fastfloor(num7 + position2.y);
			BlockValue blockValue2 = this.GetBlock(chunkFromWorldPos, num5, vector3i3.y, num6);
			int num10 = blockValue2.type;
			Block block = blockValue2.Block;
			if (block.shape.IsTerrain())
			{
				AstarVoxelGrid.HitData[] array = this.heights;
				int num11 = this.heightsUsed;
				this.heightsUsed = num11 + 1;
				array[num11] = hitData2;
			}
			else
			{
				if (num9 > 0.95f)
				{
					if (block.PathType > 0)
					{
						float num12 = (float)Utils.Fastfloor(hitData2.point.y);
						if (hitData2.point.y - num12 > 0.4f)
						{
							vector3i3.y++;
							blockValue2 = this.GetBlock(chunkFromWorldPos, num5, vector3i3.y, num6);
							num10 = blockValue2.type;
							block = blockValue2.Block;
							num7 = num12 + 1.01f;
							hitData2.point.y = num7;
						}
					}
					int num11;
					if (block.PathType > 0)
					{
						hitData2.blockerFlags = 4111;
					}
					else
					{
						if (num10 != type)
						{
							hitData2.blockerFlags |= this.CalcBlockingFlags(hitData2.point, 0.2f);
							Vector2 pathOffset = block.GetPathOffset((int)blockValue2.rotation);
							hitData2.point.x = hitData2.point.x + pathOffset.x;
							hitData2.point.z = hitData2.point.z + pathOffset.y;
						}
						vector3i4.y = vector3i3.y + 1;
						BlockValue block2 = this.GetBlock(chunkFromWorldPos, num5, vector3i4.y, num6);
						Block block3 = block2.Block;
						bool flag = block.HasTag(BlockTags.Door) || block.HasTag(BlockTags.ClosetDoor);
						bool flag2 = false;
						if (flag && block.MaxDamagePlusDowngrades - blockValue2.damage == 1)
						{
							BlockShapeModelEntity blockShapeModelEntity = block.shape as BlockShapeModelEntity;
							if (blockShapeModelEntity != null && !blockShapeModelEntity.IsObstructionForDamageState(blockValue2))
							{
								flag2 = true;
							}
						}
						if (!flag2)
						{
							if (flag)
							{
								if (!block.isMultiBlock || !blockValue2.ischild || blockValue2.parenty == 0)
								{
									hitData2.blockerFlags |= 16384;
								}
							}
							else
							{
								flag = block3.HasTag(BlockTags.Door);
								if (flag && (!block3.isMultiBlock || !block2.ischild || block2.parenty == 0))
								{
									hitData2.blockerFlags |= 16384;
								}
							}
						}
						if (num9 > 2.95f && (block.IsElevator((int)blockValue2.rotation) || block3.IsElevator((int)block2.rotation)))
						{
							hitData2.blockerFlags |= 8192;
							Vector3i vector3i5 = vector3i4;
							BlockValue blockValue3 = block2;
							Block block4 = block3;
							int num13 = (int)(num8 - 1f + position2.y);
							int num14 = 0;
							while (vector3i5.y <= num13)
							{
								if (block4.IsElevator((int)blockValue3.rotation))
								{
									num14 = 0;
								}
								else
								{
									if (!blockValue3.isair || num14 >= 1)
									{
										break;
									}
									num14++;
								}
								vector3i5.y++;
								blockValue3 = this.GetBlock(chunkFromWorldPos, num5, vector3i5.y, num6);
								block4 = blockValue3.Block;
							}
							vector3i5.y -= num14;
							AstarVoxelGrid.HitData hitData3 = default(AstarVoxelGrid.HitData);
							Vector3 vector3 = vector;
							float num15 = num7 + position2.y - -0.2f;
							while ((float)vector3i5.y > num15)
							{
								vector3.y = (float)vector3i5.y - position2.y;
								hitData3.blockerFlags = 8192 | this.CalcBlockingFlags(vector3, 0f);
								hitData3.point.x = vector3.x;
								hitData3.point.z = vector3.z;
								hitData3.point.y = vector3.y + -0.2f;
								hitData3.hitFlags |= AstarVoxelGrid.HitDataFlags.InvalidCover;
								AstarVoxelGrid.HitData[] array2 = this.heights;
								num11 = this.heightsUsed;
								this.heightsUsed = num11 + 1;
								array2[num11] = hitData3;
								vector3i5.y--;
							}
						}
					}
					AstarVoxelGrid.HitData[] array3 = this.heights;
					num11 = this.heightsUsed;
					this.heightsUsed = num11 + 1;
					array3[num11] = hitData2;
				}
				else
				{
					num7 = num8;
				}
				float num16 = float.MinValue;
				if (i < num)
				{
					num16 = AstarVoxelGrid.cellHits[i].point.y;
				}
				for (;;)
				{
					vector3i3.y--;
					vector.y = (float)vector3i3.y - position2.y;
					if (vector.y <= num16)
					{
						goto IL_0920;
					}
					if (vector3i3.y < 0)
					{
						break;
					}
					blockValue2 = this.GetBlock(chunkFromWorldPos, num5, vector3i3.y, num6);
					num10 = blockValue2.type;
					if (num10 == type)
					{
						goto IL_0920;
					}
					block = blockValue2.Block;
					if (block.shape.IsTerrain() || block.IsElevator())
					{
						goto IL_0920;
					}
					if (!block.HasTag(BlockTags.Door) && block.PathType > 0 && block.IsMovementBlocked(world, vector3i3, blockValue2, BlockFace.Top))
					{
						bool flag3 = true;
						for (int j = 0; j < 4; j++)
						{
							Vector2i vector2i = AstarVoxelGrid.neighboursOffsetV2[j];
							Vector3i vector3i6 = vector3i3;
							vector3i6.x += vector2i.x;
							vector3i6.z += vector2i.y;
							blockValue2 = chunkCache.GetBlock(vector3i6);
							block = blockValue2.Block;
							if (block.PathType <= 0 || !block.IsMovementBlocked(world, vector3i6, blockValue2, BlockFace.Top))
							{
								vector3i6.y--;
								blockValue2 = chunkCache.GetBlock(vector3i6);
								block = blockValue2.Block;
								if (block.PathType > 0)
								{
									flag3 = false;
								}
								else if (block.IsMovementBlocked(world, vector3i6, blockValue2, BlockFace.Top))
								{
									Vector3 vector4;
									vector4.x = (float)vector3i6.x - position2.x;
									vector4.y = vector.y + 0.51f;
									vector4.z = (float)vector3i6.z - position2.z;
									RaycastHit raycastHit2;
									if (defaultPhysicsScene.Raycast(vector4, down, out raycastHit2, 1.6f, 1073807360, QueryTriggerInteraction.Ignore))
									{
										flag3 = false;
										break;
									}
									break;
								}
							}
						}
						if (!flag3)
						{
							hitData2.point.y = vector.y + 0.03f;
							hitData2.blockerFlags = 4111;
							AstarVoxelGrid.HitData[] array4 = this.heights;
							int num11 = this.heightsUsed;
							this.heightsUsed = num11 + 1;
							array4[num11] = hitData2;
						}
					}
				}
				i = int.MaxValue;
			}
		}
	}

	// Token: 0x06004154 RID: 16724 RVA: 0x0019886C File Offset: 0x00196A6C
	public override void RecalculateCell(int x, int z, bool resetPenalties = true, bool resetTags = true)
	{
		World world = GameManager.Instance.World;
		if (world == null)
		{
			return;
		}
		if (world.ChunkCache == null)
		{
			return;
		}
		Vector3 vector = base.transform.Transform(new Vector3((float)x + 0.5f, 0f, (float)z + 0.5f));
		this.SampleHeights(vector);
		if (this.heightsUsed > this.layerCount)
		{
			if (this.heightsUsed > 255)
			{
				global::UnityEngine.Debug.LogError("Too many layers " + this.heightsUsed.ToString());
				return;
			}
			base.AddLayers(this.heightsUsed - this.layerCount);
		}
		Vector3 position = Origin.position;
		int num = Utils.Fastfloor(vector.x + position.x);
		int num2 = Utils.Fastfloor(vector.z + position.z);
		Vector3i vector3i = new Vector3i(num, 0, num2);
		IChunk chunkFromWorldPos = world.GetChunkFromWorldPos(vector3i);
		if (chunkFromWorldPos == null)
		{
			return;
		}
		int num3 = World.toBlockXZ(num);
		int num4 = World.toBlockXZ(num2);
		int num5 = this.width * this.depth;
		int num6 = x + z * this.width;
		int i = 0;
		this.validHeights.Clear();
		while (i < this.heightsUsed)
		{
			int num7 = num6 + num5 * i;
			AstarVoxelGrid.VoxelNode voxelNode = (AstarVoxelGrid.VoxelNode)this.nodes[num7];
			if (voxelNode == null)
			{
				voxelNode = (this.nodes[num7] = AstarVoxelGrid.levelGridNodePool.Alloc(false));
				voxelNode.Init(this.active);
				voxelNode.NodeInGridIndex = num6;
				voxelNode.LayerCoordinateInGrid = i;
				voxelNode.GraphIndex = this.graphIndex;
			}
			Vector3 point = this.heights[i].point;
			voxelNode.position = (Int3)point;
			vector3i.y = Utils.Fastfloor(point.y + position.y);
			this.validHeights.Add(vector3i.y);
			voxelNode.ClearCustomConnections(true);
			voxelNode.Walkable = true;
			int num8 = 0;
			int num9 = 0;
			voxelNode.PenaltyHigh = 0;
			voxelNode.PenaltyLow = 0;
			voxelNode.BlockerFlags = this.heights[i].blockerFlags;
			if ((this.heights[i].hitFlags & AstarVoxelGrid.HitDataFlags.InsideOversized) != AstarVoxelGrid.HitDataFlags.None)
			{
				voxelNode.Walkable = false;
			}
			if ((voxelNode.BlockerFlags & 16384) > 0)
			{
				voxelNode.Tag = 3U;
				voxelNode.Penalty = (uint)num8;
				BlockValue block = this.GetBlock(chunkFromWorldPos, num3, vector3i.y, num4);
				int num10 = block.Block.MaxDamagePlusDowngrades - block.damage;
				voxelNode.PenaltyLow = (num10 + 10) * 20 / 3;
			}
			else
			{
				vector3i.y++;
				BlockValue blockValue;
				Block block2;
				if (point.y - (float)Utils.Fastfloor(point.y) > 0.4f)
				{
					vector3i.y++;
					blockValue = this.GetBlock(chunkFromWorldPos, num3, vector3i.y, num4);
					block2 = blockValue.Block;
					if (block2.IsMovementBlocked(world, vector3i, blockValue, BlockFace.None))
					{
						int num11 = block2.MaxDamagePlusDowngrades - blockValue.damage;
						voxelNode.PenaltyHigh = (num11 + 10) * 20;
						if (block2.PathType > 0)
						{
							num8 += voxelNode.PenaltyHigh;
							AstarVoxelGrid.VoxelNode voxelNode2 = voxelNode;
							voxelNode2.BlockerFlags |= 240;
						}
						else
						{
							int num12 = (int)this.CalcBlockingFlags(point, 1.5f);
							AstarVoxelGrid.VoxelNode voxelNode3 = voxelNode;
							voxelNode3.BlockerFlags |= (ushort)((num12 & 15) << 4);
						}
					}
					vector3i.y--;
				}
				blockValue = this.GetBlock(chunkFromWorldPos, num3, vector3i.y, num4);
				block2 = blockValue.Block;
				bool flag = false;
				if (block2.IsMovementBlocked(world, vector3i, blockValue, BlockFace.None))
				{
					int num13 = block2.MaxDamagePlusDowngrades - blockValue.damage;
					voxelNode.PenaltyHigh += (num13 + 10) * 20;
					if (block2.PathType > 0)
					{
						num8 += voxelNode.PenaltyHigh;
						AstarVoxelGrid.VoxelNode voxelNode4 = voxelNode;
						voxelNode4.BlockerFlags |= 240;
					}
					else
					{
						bool flag2 = false;
						int num14 = i + 1;
						if (num14 < this.heightsUsed && Utils.Fastfloor(this.heights[num14].point.y + position.y) == vector3i.y)
						{
							flag2 = true;
							int blockerFlags = (int)this.heights[num14].blockerFlags;
							if ((blockerFlags & 4096) > 0)
							{
								num8 += voxelNode.PenaltyHigh;
								AstarVoxelGrid.VoxelNode voxelNode5 = voxelNode;
								voxelNode5.BlockerFlags |= 240;
							}
							else
							{
								AstarVoxelGrid.VoxelNode voxelNode6 = voxelNode;
								voxelNode6.BlockerFlags |= (ushort)((((blockerFlags >> 8) | blockerFlags) & 15) << 4);
							}
						}
						if (!flag2)
						{
							int num15 = (int)this.CalcBlockingFlags(point, 1f);
							AstarVoxelGrid.VoxelNode voxelNode7 = voxelNode;
							voxelNode7.BlockerFlags |= (ushort)((num15 & 15) << 4);
						}
					}
				}
				vector3i.y--;
				blockValue = this.GetBlock(chunkFromWorldPos, num3, vector3i.y, num4);
				block2 = blockValue.Block;
				if (block2.IsMovementBlocked(world, vector3i, blockValue, BlockFace.None))
				{
					int num16 = block2.MaxDamagePlusDowngrades - blockValue.damage;
					voxelNode.PenaltyLow = (num16 + 10) * 20;
					if (block2.PathType > 0)
					{
						num8 += voxelNode.PenaltyLow;
					}
				}
				if (num9 > 0)
				{
					voxelNode.Tag = 1U;
				}
				else if (flag)
				{
					voxelNode.Tag = 2U;
				}
				else
				{
					voxelNode.Tag = 0U;
				}
				if (block2.IsElevator((int)blockValue.rotation))
				{
					voxelNode.Tag = 4U;
				}
				if (num8 > 268435455)
				{
					Log.Warning("RecalculateCell {0}, id{1} {2}, pen {3}", new object[]
					{
						vector3i,
						block2.blockID,
						block2.GetBlockName(),
						num8
					});
					if (num8 < 0)
					{
						num8 = 0;
					}
					else
					{
						num8 = 268435455;
					}
				}
				voxelNode.Penalty = (uint)num8;
			}
			i++;
		}
		int num17 = num6 + num5 * i;
		while (i < this.layerCount)
		{
			LevelGridNode levelGridNode = this.nodes[num17];
			if (levelGridNode != null)
			{
				levelGridNode.Destroy();
				this.nodes[num17] = null;
				AstarVoxelGrid.levelGridNodePool.Free((AstarVoxelGrid.VoxelNode)levelGridNode);
			}
			num17 += num5;
			i++;
		}
	}

	// Token: 0x06004155 RID: 16725 RVA: 0x00198EB2 File Offset: 0x001970B2
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06004156 RID: 16726 RVA: 0x00198EBC File Offset: 0x001970BC
	[PublicizedFrom(EAccessModifier.Private)]
	public ushort CalcBlockingFlags(Vector3 pos, float offsetY)
	{
		PhysicsScene defaultPhysicsScene = Physics.defaultPhysicsScene;
		int num = 0;
		pos.y += 0.2f + offsetY;
		Vector3 vector;
		vector.y = 0f;
		for (int i = 0; i < 4; i++)
		{
			Vector2i vector2i = AstarVoxelGrid.neighboursOffsetV2[i];
			vector.x = (float)vector2i.x;
			vector.z = (float)vector2i.y;
			Vector3 vector2 = pos - vector * 0.2f;
			RaycastHit raycastHit;
			if (defaultPhysicsScene.SphereCast(vector2, 0.1f, vector, out raycastHit, 0.59f, 1073807360, QueryTriggerInteraction.Ignore))
			{
				if (offsetY > 0.5f || raycastHit.normal.y < 0.643f)
				{
					num |= 1 << i;
				}
				else if (Vector3.Dot(vector, raycastHit.normal) > -0.35f)
				{
					num |= 1 << i;
				}
				else
				{
					num |= 256 << i;
				}
			}
		}
		return (ushort)num;
	}

	// Token: 0x06004157 RID: 16727 RVA: 0x00198FB4 File Offset: 0x001971B4
	public override void CalculateConnections(int x, int z, int layerIndex)
	{
		int num = this.width * this.depth;
		int num2 = z * this.width + x + num * layerIndex;
		AstarVoxelGrid.VoxelNode voxelNode = (AstarVoxelGrid.VoxelNode)this.nodes[num2];
		if (voxelNode == null)
		{
			return;
		}
		voxelNode.ResetAllGridConnections();
		if (!voxelNode.Walkable)
		{
			return;
		}
		Vector3 vector = (Vector3)voxelNode.position;
		World.worldToBlockPos(vector + Origin.position);
		float y = vector.y;
		float num3 = y + this.characterHeight;
		vector.y += 0.5f;
		Vector3i vector3i = World.worldToBlockPos(vector + Origin.position);
		World world = GameManager.Instance.World;
		world.GetBlock(vector3i);
		if ((voxelNode.BlockerFlags & 8192) > 0 && layerIndex + 1 < this.layerCount)
		{
			LevelGridNode levelGridNode = this.nodes[num2 + num];
			if (levelGridNode != null && (float)levelGridNode.position.y * 0.001f - y < 2.1f)
			{
				this.AddConnection(voxelNode, levelGridNode, 500U, 4U, null);
				this.AddConnection(levelGridNode, voxelNode, 250U, 4U, null);
			}
		}
		for (int i = 0; i < 4; i++)
		{
			Vector2i vector2i = AstarVoxelGrid.neighboursOffsetV2[i];
			int num4 = x + vector2i.x;
			if ((ulong)num4 < (ulong)((long)this.width))
			{
				int num5 = z + vector2i.y;
				if ((ulong)num5 < (ulong)((long)this.depth))
				{
					int num6 = num5 * this.width + num4;
					int num7 = 255;
					float num8 = 0f;
					for (int j = 0; j < this.layerCount; j++)
					{
						int num9 = num6 + j * num;
						AstarVoxelGrid.VoxelNode voxelNode2 = (AstarVoxelGrid.VoxelNode)this.nodes[num9];
						if (voxelNode2 != null && voxelNode2.Walkable)
						{
							float num10 = (float)voxelNode2.position.y * 0.001f;
							float num11;
							if (j == this.layerCount - 1 || this.nodes[num9 + num] == null)
							{
								num11 = float.PositiveInfinity;
							}
							else
							{
								num11 = (float)this.nodes[num9 + num].position.y * 0.001f - num10;
								if (num11 <= -0.001f)
								{
									LevelGridNode levelGridNode2 = this.nodes[num9 + num];
									Utils.DrawLine((Vector3)voxelNode.position, (Vector3)voxelNode2.position, new Color(1f, 0f, 1f), new Color(1f, 0.5f, 0f), 3, 5f);
									Utils.DrawLine((Vector3)voxelNode2.position, (Vector3)levelGridNode2.position, new Color(1f, 0f, 0f), new Color(1f, 1f, 0f), 2, 5f);
									Log.Warning("Path node otherHeight bad {0}, {1}, {2}", new object[] { num11, levelGridNode2.position, voxelNode2.position });
								}
							}
							float num12 = num10 - y;
							if (num12 < -0.1f)
							{
								if (num12 >= -9.4f && num3 < num10 + num11)
								{
									num7 = j;
									num8 = num12;
								}
							}
							else
							{
								if (num12 >= 1.51f)
								{
									break;
								}
								if (num11 >= 0.7f)
								{
									if (num12 >= 0.6f)
									{
										if ((voxelNode.BlockerFlags & 15) != 15 && ((int)voxelNode.BlockerFlags & (16 << i)) == 0 && ((int)voxelNode2.BlockerFlags & (17 << (i ^ 2))) == 0)
										{
											if (num12 >= 1.05f || ((int)voxelNode2.BlockerFlags & (256 << i)) == 0)
											{
												if (((int)voxelNode2.BlockerFlags & (256 << (i ^ 2))) == 0)
												{
													this.AddConnection(voxelNode, voxelNode2, (uint)(num12 * 8000f), 0U, null);
													this.AddDummyConnection(voxelNode2, voxelNode);
													if ((voxelNode.BlockerFlags & 8192) == 0)
													{
														num7 = 255;
														break;
													}
													break;
												}
											}
											else
											{
												num7 = j;
												num8 = 0f;
											}
										}
									}
									else
									{
										if (voxelNode2.Tag == 3U)
										{
											Vector3i vector3i2 = World.worldToBlockPos((Vector3)voxelNode2.position + Origin.position);
											Block block = world.GetBlock(vector3i2).Block;
											TEFeatureDoor tefeatureDoor = null;
											if (block.HasTag(BlockTags.Door) && block is BlockCompositeTileEntity)
											{
												TileEntityComposite tileEntityComposite = world.GetTileEntity(vector3i2) as TileEntityComposite;
												if (tileEntityComposite != null)
												{
													tileEntityComposite.TryGetSelfOrFeature<TEFeatureDoor>(out tefeatureDoor);
												}
											}
											this.AddConnection(voxelNode, voxelNode2, 2000U, 3U, tefeatureDoor);
											this.AddDummyConnection(voxelNode2, voxelNode);
										}
										if ((voxelNode2.BlockerFlags & 12288) > 0)
										{
											num7 = j;
											num8 = 0f;
										}
									}
								}
							}
						}
					}
					if (num7 != 255)
					{
						int num13 = num6 + num7 * num;
						AstarVoxelGrid.VoxelNode voxelNode3 = (AstarVoxelGrid.VoxelNode)this.nodes[num13];
						bool flag = false;
						int num14 = (int)(voxelNode.BlockerFlags & 15);
						if (num14 == 0 || num14 == 15)
						{
							num14 = (int)(voxelNode.BlockerFlags & 240);
							if (num14 == 0 || num14 == 240)
							{
								num14 = (int)(voxelNode3.BlockerFlags & 15);
								if (num8 <= -0.95f || num14 == 0 || num14 == 15)
								{
									num14 = (int)(voxelNode3.BlockerFlags & 240);
									if (num14 == 0 || num14 == 240)
									{
										flag = true;
									}
								}
							}
						}
						if (!flag)
						{
							int num15 = 0;
							if (((int)voxelNode.BlockerFlags & (16 << i)) > 0)
							{
								num15 += voxelNode.PenaltyHigh;
							}
							if (((int)voxelNode3.BlockerFlags & (16 << (i ^ 2))) > 0)
							{
								num15 += voxelNode3.PenaltyHigh;
							}
							if (((int)voxelNode.BlockerFlags & (1 << i)) > 0)
							{
								num15 += voxelNode.PenaltyLow;
							}
							if (((int)voxelNode3.BlockerFlags & (1 << (i ^ 2))) > 0 && num8 > -0.95f)
							{
								num15 += voxelNode3.PenaltyLow;
							}
							if (num15 > 0)
							{
								this.AddConnection(voxelNode, voxelNode3, (uint)num15, 1U, null);
								this.AddDummyConnection(voxelNode3, voxelNode);
								num7 = 255;
							}
						}
						voxelNode.SetConnectionValue(i, num7);
					}
				}
			}
		}
	}

	// Token: 0x06004158 RID: 16728 RVA: 0x001995D4 File Offset: 0x001977D4
	[PublicizedFrom(EAccessModifier.Private)]
	public void AddConnection(GridNodeBase node, GridNodeBase other, uint cost, uint tag, object payload)
	{
		Connection[] connections = node.connections;
		int num = 0;
		if (connections != null)
		{
			for (int i = 0; i < connections.Length; i++)
			{
				if (connections[i].node == other)
				{
					connections[i].cost = cost;
					connections[i].tag = tag;
					connections[i].payload = payload;
					return;
				}
			}
			num = connections.Length;
		}
		num++;
		Connection[] array = AstarVoxelGrid.AllocConnection(num);
		for (int j = 0; j < num - 1; j++)
		{
			array[j] = connections[j];
		}
		array[num - 1] = new Connection(other, cost, tag, payload, byte.MaxValue);
		node.connections = array;
	}

	// Token: 0x06004159 RID: 16729 RVA: 0x00199688 File Offset: 0x00197888
	[PublicizedFrom(EAccessModifier.Private)]
	public void AddDummyConnection(GridNodeBase node, GridNodeBase other)
	{
		Connection[] connections = node.connections;
		if (connections != null)
		{
			for (int i = 0; i < connections.Length; i++)
			{
				if (connections[i].node == other)
				{
					return;
				}
			}
		}
		this.AddConnection(node, other, uint.MaxValue, 0U, null);
	}

	// Token: 0x0600415A RID: 16730 RVA: 0x001996C8 File Offset: 0x001978C8
	public static void ClearConnections(GridNodeBase node)
	{
		Connection[] connections = node.connections;
		if (connections != null)
		{
			node.connections = null;
			int num = connections.Length;
			for (int i = 0; i < num; i++)
			{
				AstarVoxelGrid.RemoveConnection((GridNodeBase)connections[i].node, node);
			}
			if (num < 16)
			{
				AstarVoxelGrid.connectionsPool[num].Add(connections);
			}
		}
	}

	// Token: 0x0600415B RID: 16731 RVA: 0x00199720 File Offset: 0x00197920
	[PublicizedFrom(EAccessModifier.Private)]
	public static void RemoveConnection(GridNodeBase node, GridNodeBase other)
	{
		Connection[] connections = node.connections;
		if (connections != null)
		{
			int num = connections.Length;
			int i = 0;
			while (i < num)
			{
				if (connections[i].node == other)
				{
					if (num <= 1)
					{
						node.connections = null;
					}
					else
					{
						Connection[] array = AstarVoxelGrid.AllocConnection(num - 1);
						int j;
						for (j = 0; j < i; j++)
						{
							array[j] = connections[j];
						}
						while (j < array.Length)
						{
							array[j] = connections[j + 1];
							j++;
						}
						node.connections = array;
					}
					if (num < 16)
					{
						AstarVoxelGrid.connectionsPool[num].Add(connections);
						return;
					}
					break;
				}
				else
				{
					i++;
				}
			}
		}
	}

	// Token: 0x0600415C RID: 16732 RVA: 0x001997D0 File Offset: 0x001979D0
	[PublicizedFrom(EAccessModifier.Private)]
	public static Connection[] AllocConnection(int count)
	{
		Connection[] array = null;
		if (count < 16)
		{
			List<Connection[]> list = AstarVoxelGrid.connectionsPool[count];
			int count2 = list.Count;
			if (count2 > 0)
			{
				array = list[count2 - 1];
				list.RemoveAt(count2 - 1);
			}
		}
		if (array == null)
		{
			array = new Connection[count];
		}
		return array;
	}

	// Token: 0x0600415D RID: 16733 RVA: 0x00199815 File Offset: 0x00197A15
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockValue GetBlock(IChunk chunk, int _x, int _y, int _z)
	{
		if (_y >= 256)
		{
			return BlockValue.Air;
		}
		return chunk.GetBlock(_x, _y, _z);
	}

	// Token: 0x0600415E RID: 16734 RVA: 0x0019982F File Offset: 0x00197A2F
	public void SetPos(Vector3 pos)
	{
		this.center = pos;
		this.IsFullUpdateNeeded = false;
		this.InitScan();
	}

	// Token: 0x0600415F RID: 16735 RVA: 0x00199845 File Offset: 0x00197A45
	public void Move(Vector3 targetPos)
	{
		this.gridMover.graph = this;
		this.gridMover.targetPosition = targetPos;
		this.gridMover.UpdateGraph();
	}

	// Token: 0x06004160 RID: 16736 RVA: 0x0019986A File Offset: 0x00197A6A
	public bool IsMoving()
	{
		return this.gridMover.updatingGraph;
	}

	// Token: 0x040034B1 RID: 13489
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cGridHeight = 320f;

	// Token: 0x040034B2 RID: 13490
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cGridHeightPadded = 320.01f;

	// Token: 0x040034B3 RID: 13491
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cCollisionMask = 1073807360;

	// Token: 0x040034B4 RID: 13492
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cLayerMinHeight = 0.7f;

	// Token: 0x040034B5 RID: 13493
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cClimbMinHeight = 0.6f;

	// Token: 0x040034B6 RID: 13494
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cClimbMaxHeight = 1.51f;

	// Token: 0x040034B7 RID: 13495
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cDropOnTopHeight = 0.95f;

	// Token: 0x040034B8 RID: 13496
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cDropMaxHeight = 9.4f;

	// Token: 0x040034B9 RID: 13497
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cJumpPenalty = 8;

	// Token: 0x040034BA RID: 13498
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cDoorPenalty = 2;

	// Token: 0x040034BB RID: 13499
	public const int cPenaltyPerMeter = 1000;

	// Token: 0x040034BC RID: 13500
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cPenaltyHealthBase = 10;

	// Token: 0x040034BD RID: 13501
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cPenaltyHealthScale = 20;

	// Token: 0x040034BE RID: 13502
	public const uint cDummyPenalty = 4294967295U;

	// Token: 0x040034BF RID: 13503
	public const int cTagOpen = 0;

	// Token: 0x040034C0 RID: 13504
	public const int cTagBreak = 1;

	// Token: 0x040034C1 RID: 13505
	public const int cTagLowHeight = 2;

	// Token: 0x040034C2 RID: 13506
	public const int cTagDoor = 3;

	// Token: 0x040034C3 RID: 13507
	public const int cTagLadder = 4;

	// Token: 0x040034C4 RID: 13508
	public const int cTagTest = 8;

	// Token: 0x040034C5 RID: 13509
	public const ushort cBlockerFlagLow0 = 1;

	// Token: 0x040034C6 RID: 13510
	public const ushort cBlockerFlagLow = 15;

	// Token: 0x040034C7 RID: 13511
	public const ushort cBlockerFlagHigh0 = 16;

	// Token: 0x040034C8 RID: 13512
	public const ushort cBlockerFlagHigh = 240;

	// Token: 0x040034C9 RID: 13513
	public const ushort cBlockerFlagHighLow0 = 17;

	// Token: 0x040034CA RID: 13514
	public const ushort cBlockerFlagHighLow = 255;

	// Token: 0x040034CB RID: 13515
	public const ushort cBlockerFlagSlopeDir0 = 256;

	// Token: 0x040034CC RID: 13516
	public const ushort cBlockerFlagFloor = 4096;

	// Token: 0x040034CD RID: 13517
	public const ushort cBlockerFlagLadder = 8192;

	// Token: 0x040034CE RID: 13518
	public const ushort cBlockerFlagDoor = 16384;

	// Token: 0x040034CF RID: 13519
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Vector2i[] neighboursOffsetV2 = new Vector2i[]
	{
		new Vector2i(0, -1),
		new Vector2i(1, 0),
		new Vector2i(0, 1),
		new Vector2i(-1, 0)
	};

	// Token: 0x040034D0 RID: 13520
	public bool IsUsed;

	// Token: 0x040034D1 RID: 13521
	public bool IsFullUpdateNeeded;

	// Token: 0x040034D2 RID: 13522
	[PublicizedFrom(EAccessModifier.Private)]
	public AstarVoxelGrid.ProceduralGridMover gridMover;

	// Token: 0x040034D3 RID: 13523
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cRayHitsMax = 512;

	// Token: 0x040034D4 RID: 13524
	[PublicizedFrom(EAccessModifier.Private)]
	public static AstarVoxelGrid.HitData[] cellHits = new AstarVoxelGrid.HitData[512];

	// Token: 0x040034D5 RID: 13525
	[PublicizedFrom(EAccessModifier.Private)]
	public AstarVoxelGrid.HitData[] heights = new AstarVoxelGrid.HitData[512];

	// Token: 0x040034D6 RID: 13526
	[PublicizedFrom(EAccessModifier.Private)]
	public int heightsUsed;

	// Token: 0x040034D7 RID: 13527
	[PublicizedFrom(EAccessModifier.Private)]
	public static MemoryPooledObject<AstarVoxelGrid.VoxelNode> levelGridNodePool = new MemoryPooledObject<AstarVoxelGrid.VoxelNode>(100000);

	// Token: 0x040034D8 RID: 13528
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cConnectionPoolMax = 16;

	// Token: 0x040034D9 RID: 13529
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<Connection[]>[] connectionsPool;

	// Token: 0x040034DA RID: 13530
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly WorldRayHitInfo localWorldRayHit = new WorldRayHitInfo();

	// Token: 0x040034DB RID: 13531
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly HashSet<int> validHeights = new HashSet<int>();

	// Token: 0x040034DC RID: 13532
	public Vector2 GridMovePendingPos;

	// Token: 0x020008CF RID: 2255
	[Flags]
	public enum HitDataFlags : ushort
	{
		// Token: 0x040034DE RID: 13534
		None = 0,
		// Token: 0x040034DF RID: 13535
		InvalidCover = 1,
		// Token: 0x040034E0 RID: 13536
		InsideOversized = 2
	}

	// Token: 0x020008D0 RID: 2256
	public struct HitData
	{
		// Token: 0x040034E1 RID: 13537
		public Vector3 point;

		// Token: 0x040034E2 RID: 13538
		public ushort blockerFlags;

		// Token: 0x040034E3 RID: 13539
		public AstarVoxelGrid.HitDataFlags hitFlags;
	}

	// Token: 0x020008D1 RID: 2257
	public class VoxelNode : LevelGridNode, IMemoryPoolableObject
	{
		// Token: 0x06004164 RID: 16740 RVA: 0x000027FC File Offset: 0x000009FC
		public void Reset()
		{
		}

		// Token: 0x06004165 RID: 16741 RVA: 0x000027FC File Offset: 0x000009FC
		public void Cleanup()
		{
		}

		// Token: 0x06004166 RID: 16742 RVA: 0x0019991E File Offset: 0x00197B1E
		public override void ClearCustomConnections(bool alsoReverse)
		{
			AstarVoxelGrid.ClearConnections(this);
		}

		// Token: 0x06004167 RID: 16743 RVA: 0x00199928 File Offset: 0x00197B28
		public override void UpdateRecursiveG(Path path, PathNode pathNode, PathHandler handler)
		{
			handler.heap.Add(pathNode);
			pathNode.UpdateG(path);
			LayerGridGraph gridGraph = LevelGridNode.GetGridGraph(base.GraphIndex);
			int[] neighbourOffsets = gridGraph.neighbourOffsets;
			LevelGridNode[] nodes = gridGraph.nodes;
			int nodeInGridIndex = base.NodeInGridIndex;
			for (int i = 0; i < 4; i++)
			{
				int connectionValue = base.GetConnectionValue(i);
				if (connectionValue != 255)
				{
					LevelGridNode levelGridNode = nodes[nodeInGridIndex + neighbourOffsets[i] + gridGraph.lastScannedWidth * gridGraph.lastScannedDepth * connectionValue];
					PathNode pathNode2 = handler.GetPathNode(levelGridNode);
					if (pathNode2 != null && pathNode2.parent == pathNode && pathNode2.pathID == handler.PathID)
					{
						levelGridNode.UpdateRecursiveG(path, pathNode2, handler);
					}
				}
			}
			if (this.connections != null)
			{
				ushort pathID = handler.PathID;
				for (int j = 0; j < this.connections.Length; j++)
				{
					if (this.connections[j].cost != 4294967295U)
					{
						GraphNode node = this.connections[j].node;
						PathNode pathNode3 = handler.GetPathNode(node);
						if (pathNode3.parent == pathNode && pathNode3.pathID == pathID)
						{
							node.UpdateRecursiveG(path, pathNode3, handler);
						}
					}
				}
			}
		}

		// Token: 0x040034E4 RID: 13540
		public int PenaltyHigh;

		// Token: 0x040034E5 RID: 13541
		public int PenaltyLow;

		// Token: 0x040034E6 RID: 13542
		public ushort BlockerFlags;
	}

	// Token: 0x020008D2 RID: 2258
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_procedural_grid_mover.php")]
	public class ProceduralGridMover
	{
		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x06004169 RID: 16745 RVA: 0x00199A5B File Offset: 0x00197C5B
		// (set) Token: 0x0600416A RID: 16746 RVA: 0x00199A63 File Offset: 0x00197C63
		public bool updatingGraph
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600416B RID: 16747 RVA: 0x00199A6C File Offset: 0x00197C6C
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3 PointToGraphSpace(Vector3 p)
		{
			return this.graph.transform.InverseTransform(p);
		}

		// Token: 0x0600416C RID: 16748 RVA: 0x00199A80 File Offset: 0x00197C80
		public void UpdateGraph()
		{
			if (this.updatingGraph)
			{
				return;
			}
			this.updatingGraph = true;
			IEnumerator ie = this.UpdateGraphCoroutine();
			AstarPath.active.AddWorkItem(new AstarWorkItem(delegate(IWorkItemContext context, bool force)
			{
				if (force)
				{
					while (ie.MoveNext())
					{
					}
				}
				bool flag;
				try
				{
					flag = !ie.MoveNext();
				}
				catch (Exception ex)
				{
					global::UnityEngine.Debug.LogException(ex);
					flag = true;
				}
				if (flag)
				{
					this.updatingGraph = false;
				}
				return flag;
			}));
		}

		// Token: 0x0600416D RID: 16749 RVA: 0x00199AD1 File Offset: 0x00197CD1
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator UpdateGraphCoroutine()
		{
			Vector3 vector = this.PointToGraphSpace(this.targetPosition) - this.PointToGraphSpace(this.graph.center);
			vector.x = Mathf.Round(vector.x);
			vector.z = Mathf.Round(vector.z);
			vector.y = 0f;
			if (vector == Vector3.zero)
			{
				yield break;
			}
			Int2 offset = new Int2(-Mathf.RoundToInt(vector.x), -Mathf.RoundToInt(vector.z));
			this.graph.center = this.targetPosition;
			this.graph.UpdateTransform();
			int width = this.graph.width;
			int depth = this.graph.depth;
			int layers = this.graph.LayerCount;
			LayerGridGraph layerGridGraph = this.graph as LayerGridGraph;
			GridNodeBase[] nodes;
			if (layerGridGraph != null)
			{
				GridNodeBase[] array = layerGridGraph.nodes;
				nodes = array;
			}
			else
			{
				GridNodeBase[] array = this.graph.nodes;
				nodes = array;
			}
			if (this.buffer == null || this.buffer.Length != width * depth)
			{
				this.buffer = new GridNodeBase[width * depth];
			}
			if (Mathf.Abs(offset.x) <= width && Mathf.Abs(offset.y) <= depth)
			{
				IntRect recalculateRect = new IntRect(0, 0, offset.x, offset.y);
				if (recalculateRect.xmin > recalculateRect.xmax)
				{
					int xmax = recalculateRect.xmax;
					recalculateRect.xmax = width + recalculateRect.xmin;
					recalculateRect.xmin = width + xmax;
				}
				if (recalculateRect.ymin > recalculateRect.ymax)
				{
					int ymax = recalculateRect.ymax;
					recalculateRect.ymax = depth + recalculateRect.ymin;
					recalculateRect.ymin = depth + ymax;
				}
				IntRect connectionRect = recalculateRect.Expand(1);
				connectionRect = IntRect.Intersection(connectionRect, new IntRect(0, 0, width, depth));
				int widthStart = width - offset.x;
				int destOffset = offset.x;
				if (offset.x < 0)
				{
					widthStart = -offset.x;
					destOffset += width;
				}
				int num9;
				for (int i = 0; i < layers; i = num9 + 1)
				{
					int num = i * width * depth;
					for (int j = 0; j < depth; j++)
					{
						int num2 = j * width;
						int num3 = (j + offset.y + depth) % depth * width;
						int num4 = num + num2;
						Array.Copy(nodes, num4, this.buffer, num3 + destOffset, widthStart);
						Array.Copy(nodes, num4 + widthStart, this.buffer, num3, width - widthStart);
					}
					for (int k = 0; k < depth; k++)
					{
						int num5 = k * width;
						for (int l = 0; l < width; l++)
						{
							int num6 = num5 + l;
							GridNodeBase gridNodeBase = this.buffer[num6];
							if (gridNodeBase != null)
							{
								gridNodeBase.NodeInGridIndex = num6;
							}
							nodes[num + num6] = gridNodeBase;
						}
						int num7;
						int num8;
						if (k >= recalculateRect.ymin && k < recalculateRect.ymax)
						{
							num7 = 0;
							num8 = depth;
						}
						else
						{
							num7 = recalculateRect.xmin;
							num8 = recalculateRect.xmax;
						}
						for (int m = num7; m < num8; m++)
						{
							GridNodeBase gridNodeBase2 = this.buffer[num5 + m];
							if (gridNodeBase2 != null)
							{
								gridNodeBase2.ClearConnections(false);
							}
						}
					}
					if ((i & 7) == 7)
					{
						yield return null;
					}
					num9 = i;
				}
				int yieldEvery = 160;
				int num10 = Mathf.Max(Mathf.Abs(offset.x), Mathf.Abs(offset.y)) * Mathf.Max(width, depth);
				yieldEvery = Mathf.Max(yieldEvery, num10 / 10);
				int counter = 0;
				for (int i = 0; i < depth; i = num9 + 1)
				{
					int num11;
					int num12;
					if (i >= recalculateRect.ymin && i < recalculateRect.ymax)
					{
						num11 = 0;
						num12 = width;
					}
					else
					{
						num11 = recalculateRect.xmin;
						num12 = recalculateRect.xmax;
					}
					for (int n = num11; n < num12; n++)
					{
						this.graph.RecalculateCell(n, i, false, false);
					}
					counter += num12 - num11;
					if (counter > yieldEvery)
					{
						counter = 0;
						yield return null;
					}
					num9 = i;
				}
				yieldEvery *= 48;
				for (int i = 0; i < depth; i = num9 + 1)
				{
					int num13;
					int num14;
					if (i >= connectionRect.ymin && i < connectionRect.ymax)
					{
						num13 = 0;
						num14 = width;
					}
					else
					{
						num13 = connectionRect.xmin;
						num14 = connectionRect.xmax;
					}
					for (int num15 = num13; num15 < num14; num15++)
					{
						this.graph.CalculateConnections(num15, i);
					}
					counter += (num14 - num13) * layers;
					if (counter > yieldEvery)
					{
						counter = 0;
						yield return null;
					}
					num9 = i;
				}
				yield return null;
				for (int num16 = 0; num16 < depth; num16++)
				{
					for (int num17 = 0; num17 < width; num17++)
					{
						if (num17 == 0 || num16 == 0 || num17 == width - 1 || num16 == depth - 1)
						{
							this.graph.CalculateConnections(num17, num16);
						}
					}
				}
				recalculateRect = default(IntRect);
				connectionRect = default(IntRect);
			}
			else
			{
				int counter = Mathf.Max(depth * width / 20, 1000);
				int yieldEvery = 0;
				int num9;
				for (int destOffset = 0; destOffset < depth; destOffset = num9 + 1)
				{
					for (int num18 = 0; num18 < width; num18++)
					{
						this.graph.RecalculateCell(num18, destOffset, true, true);
					}
					yieldEvery += width;
					if (yieldEvery > counter)
					{
						yieldEvery = 0;
						yield return null;
					}
					num9 = destOffset;
				}
				for (int destOffset = 0; destOffset < depth; destOffset = num9 + 1)
				{
					for (int num19 = 0; num19 < width; num19++)
					{
						this.graph.CalculateConnections(num19, destOffset);
					}
					yieldEvery += width;
					if (yieldEvery > counter)
					{
						yieldEvery = 0;
						yield return null;
					}
					num9 = destOffset;
				}
			}
			yield break;
		}

		// Token: 0x040034E7 RID: 13543
		public float updateDistance = 10f;

		// Token: 0x040034E8 RID: 13544
		public Vector3 targetPosition;

		// Token: 0x040034E9 RID: 13545
		public GridGraph graph;

		// Token: 0x040034EA RID: 13546
		[PublicizedFrom(EAccessModifier.Private)]
		public GridNodeBase[] buffer;
	}
}
