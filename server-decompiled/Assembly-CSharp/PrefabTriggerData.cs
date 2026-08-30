using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020001C9 RID: 457
[Preserve]
public class PrefabTriggerData
{
	// Token: 0x17000111 RID: 273
	// (get) Token: 0x06000E18 RID: 3608 RVA: 0x0005CB8D File Offset: 0x0005AD8D
	// (set) Token: 0x06000E19 RID: 3609 RVA: 0x0005CBA0 File Offset: 0x0005ADA0
	public bool NeedsTriggerUpdate
	{
		get
		{
			return this.needsTriggerTimer != -1f;
		}
		set
		{
			if (this.Owner == null)
			{
				this.Owner = this.world.triggerManager;
			}
			if (value)
			{
				this.Owner.AddToUpdateList(this);
				this.needsTriggerTimer = 3f;
				return;
			}
			this.Owner.RemoveFromUpdateList(this);
			this.needsTriggerTimer = -1f;
		}
	}

	// Token: 0x06000E1A RID: 3610 RVA: 0x0005CBF8 File Offset: 0x0005ADF8
	public PrefabTriggerData(PrefabInstance instance)
	{
		this.PrefabInstance = instance;
		this.world = GameManager.Instance.World;
		this.SetupData();
	}

	// Token: 0x06000E1B RID: 3611 RVA: 0x0005CC60 File Offset: 0x0005AE60
	public void ResetData()
	{
		if (this.TriggeredLayers != null)
		{
			this.TriggeredLayers.Clear();
		}
		if (this.TriggeredByLayers != null)
		{
			this.TriggeredByLayers.Clear();
		}
		this.TriggeredByDictionary.Clear();
		this.TriggeredByVolumes.Clear();
		this.Triggers.Clear();
		this.SetupData();
	}

	// Token: 0x06000E1C RID: 3612 RVA: 0x0005CCBC File Offset: 0x0005AEBC
	[PublicizedFrom(EAccessModifier.Protected)]
	public void SetupData()
	{
		bool flag = GameManager.Instance.World.IsEditor();
		HashSetLong occupiedChunks = this.PrefabInstance.GetOccupiedChunks();
		Vector3i boundingBoxSize = this.PrefabInstance.boundingBoxSize;
		Vector3i boundingBoxPosition = this.PrefabInstance.boundingBoxPosition;
		foreach (long num in occupiedChunks)
		{
			Chunk chunkSync = this.world.ChunkCache.GetChunkSync(num);
			if (chunkSync != null)
			{
				foreach (BlockTrigger blockTrigger in chunkSync.GetBlockTriggers().list)
				{
					Vector3i vector3i = blockTrigger.ToWorldPos();
					if (boundingBoxPosition.x <= vector3i.x && boundingBoxPosition.y <= vector3i.y && boundingBoxPosition.z <= vector3i.z && boundingBoxPosition.x + boundingBoxSize.x > vector3i.x && boundingBoxPosition.y + boundingBoxSize.y > vector3i.y && boundingBoxPosition.z + boundingBoxSize.z > vector3i.z)
					{
						using (List<byte>.Enumerator enumerator3 = blockTrigger.TriggeredByIndices.GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								int num2 = (int)enumerator3.Current;
								List<BlockTrigger> list;
								if (!this.TriggeredByDictionary.TryGetValue(num2, out list))
								{
									list = new List<BlockTrigger>();
									this.TriggeredByDictionary[num2] = list;
								}
								list.Add(blockTrigger);
								if (flag)
								{
									if (this.TriggeredByLayers == null)
									{
										this.TriggeredByLayers = new List<byte>();
									}
									if (!this.TriggeredByLayers.Contains((byte)num2))
									{
										this.TriggeredByLayers.Add((byte)num2);
									}
								}
							}
						}
						using (List<byte>.Enumerator enumerator3 = blockTrigger.TriggersIndices.GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								int num3 = (int)enumerator3.Current;
								if (this.TriggeredLayers == null)
								{
									this.TriggeredLayers = new List<byte>();
								}
								if (!this.TriggeredLayers.Contains((byte)num3))
								{
									this.TriggeredLayers.Add((byte)num3);
								}
							}
						}
						this.Triggers.Add(blockTrigger);
						blockTrigger.TriggerDataOwner = this;
					}
				}
			}
		}
		List<SleeperVolume> sleeperVolumes = this.PrefabInstance.sleeperVolumes;
		for (int i = 0; i < sleeperVolumes.Count; i++)
		{
			SleeperVolume sleeperVolume = sleeperVolumes[i];
			for (int j = 0; j < sleeperVolume.TriggeredByIndices.Count; j++)
			{
				this.AddTriggeredBy(sleeperVolume);
				if (flag)
				{
					if (this.TriggeredByLayers == null)
					{
						this.TriggeredByLayers = new List<byte>();
					}
					byte b = sleeperVolume.TriggeredByIndices[j];
					if (!this.TriggeredByLayers.Contains(b))
					{
						this.TriggeredByLayers.Add(b);
					}
				}
			}
		}
		this.RefreshTriggers();
	}

	// Token: 0x06000E1D RID: 3613 RVA: 0x0005D018 File Offset: 0x0005B218
	public void Update(float deltaTime)
	{
		if (this.needsTriggerTimer != -1f)
		{
			this.needsTriggerTimer -= deltaTime;
			if (this.needsTriggerTimer <= 0f)
			{
				this.HandleNeedTriggers();
				this.NeedsTriggerUpdate = false;
			}
		}
	}

	// Token: 0x06000E1E RID: 3614 RVA: 0x0005D050 File Offset: 0x0005B250
	public void HandleNeedTriggers()
	{
		for (int i = 0; i < this.Triggers.Count; i++)
		{
			if (this.Triggers[i].NeedsTriggered == BlockTrigger.TriggeredStates.NeedsTriggered)
			{
				this.Trigger(null, this.Triggers[i]);
				this.Triggers[i].NeedsTriggered = BlockTrigger.TriggeredStates.HasTriggered;
			}
		}
	}

	// Token: 0x06000E1F RID: 3615 RVA: 0x0005D0AC File Offset: 0x0005B2AC
	public void RefreshTriggers()
	{
		if (!GameManager.Instance.IsEditMode())
		{
			for (int i = 0; i < this.Triggers.Count; i++)
			{
				this.Triggers[i].Refresh(FastTags<TagGroup.Global>.none);
			}
		}
	}

	// Token: 0x06000E20 RID: 3616 RVA: 0x0005D0F4 File Offset: 0x0005B2F4
	public void RefreshTriggersForQuest(FastTags<TagGroup.Global> questTags)
	{
		if (!GameManager.Instance.IsEditMode())
		{
			for (int i = 0; i < this.Triggers.Count; i++)
			{
				this.Triggers[i].Refresh(questTags);
			}
		}
	}

	// Token: 0x06000E21 RID: 3617 RVA: 0x0005D138 File Offset: 0x0005B338
	public void ResetTriggers()
	{
		if (!GameManager.Instance.IsEditMode())
		{
			for (int i = 0; i < this.Triggers.Count; i++)
			{
				this.Triggers[i].NeedsTriggered = BlockTrigger.TriggeredStates.NotTriggered;
			}
		}
	}

	// Token: 0x06000E22 RID: 3618 RVA: 0x0005D179 File Offset: 0x0005B379
	public void AddPlayerInArea(int entityID)
	{
		if (!this.PlayersInArea.Contains(entityID))
		{
			this.PlayersInArea.Add(entityID);
		}
	}

	// Token: 0x06000E23 RID: 3619 RVA: 0x0005D195 File Offset: 0x0005B395
	public void RemovePlayerInArea(int entityID)
	{
		if (this.PlayersInArea.Contains(entityID))
		{
			this.PlayersInArea.Remove(entityID);
		}
	}

	// Token: 0x06000E24 RID: 3620 RVA: 0x0005D1B4 File Offset: 0x0005B3B4
	[PublicizedFrom(EAccessModifier.Protected)]
	public void AddTriggeredBy(SleeperVolume triggeredVolume)
	{
		for (int i = 0; i < triggeredVolume.TriggeredByIndices.Count; i++)
		{
			byte b = triggeredVolume.TriggeredByIndices[i];
			if (!this.TriggeredByVolumes.ContainsKey((int)b))
			{
				this.TriggeredByVolumes.Add((int)b, new List<SleeperVolume>());
			}
			this.TriggeredByVolumes[(int)b].Add(triggeredVolume);
		}
	}

	// Token: 0x06000E25 RID: 3621 RVA: 0x0005D218 File Offset: 0x0005B418
	public void Trigger(EntityPlayer player, byte index)
	{
		List<BlockChangeInfo> list = new List<BlockChangeInfo>();
		World world = GameManager.Instance.World;
		List<BlockTrigger> list2;
		if (this.TriggeredByDictionary.TryGetValue((int)index, out list2))
		{
			for (int i = 0; i < list2.Count; i++)
			{
				list2[i].OnTriggered(player, world, (int)index, list, null);
			}
		}
		List<SleeperVolume> list3;
		if (this.TriggeredByVolumes.TryGetValue((int)index, out list3))
		{
			foreach (SleeperVolume sleeperVolume in list3)
			{
				sleeperVolume.OnTriggered(player, world, (int)index);
			}
		}
		if (list.Count > 0)
		{
			this.UpdateBlocks(list);
		}
	}

	// Token: 0x06000E26 RID: 3622 RVA: 0x0005D2D0 File Offset: 0x0005B4D0
	public void Trigger(EntityPlayer player, BlockTrigger trigger)
	{
		List<BlockChangeInfo> list = new List<BlockChangeInfo>();
		World world = GameManager.Instance.World;
		using (List<byte>.Enumerator enumerator = trigger.TriggersIndices.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				int num = (int)enumerator.Current;
				List<BlockTrigger> list2;
				if (this.TriggeredByDictionary.TryGetValue(num, out list2))
				{
					foreach (BlockTrigger blockTrigger in list2)
					{
						blockTrigger.OnTriggered(player, world, num, list, trigger);
					}
				}
				if (player != null && this.TriggeredByVolumes.ContainsKey(num))
				{
					foreach (SleeperVolume sleeperVolume in this.TriggeredByVolumes[num])
					{
						sleeperVolume.OnTriggered(player, world, num);
					}
				}
			}
		}
		if (list.Count > 0)
		{
			this.UpdateBlocks(list);
		}
	}

	// Token: 0x06000E27 RID: 3623 RVA: 0x0005D3F4 File Offset: 0x0005B5F4
	public void Trigger(EntityPlayer player, TriggerVolume trigger)
	{
		List<BlockChangeInfo> list = new List<BlockChangeInfo>();
		World world = GameManager.Instance.World;
		for (int i = 0; i < trigger.TriggersIndices.Count; i++)
		{
			int num = (int)trigger.TriggersIndices[i];
			if (this.TriggeredByDictionary.ContainsKey(num))
			{
				for (int j = 0; j < this.TriggeredByDictionary[num].Count; j++)
				{
					this.TriggeredByDictionary[num][j].OnTriggered(player, world, num, list, null);
				}
			}
			if (this.TriggeredByVolumes.ContainsKey(num))
			{
				for (int k = 0; k < this.TriggeredByVolumes[num].Count; k++)
				{
					this.TriggeredByVolumes[num][k].OnTriggered(player, world, num);
				}
			}
		}
		if (list.Count > 0)
		{
			this.UpdateBlocks(list);
		}
	}

	// Token: 0x06000E28 RID: 3624 RVA: 0x0005D4DF File Offset: 0x0005B6DF
	[PublicizedFrom(EAccessModifier.Protected)]
	public void UpdateBlocks(List<BlockChangeInfo> blockChanges)
	{
		if (GameManager.Instance.World != null && blockChanges != null)
		{
			GameManager.Instance.World.SetBlocksRPC(blockChanges);
		}
	}

	// Token: 0x06000E29 RID: 3625 RVA: 0x0005D500 File Offset: 0x0005B700
	public void SetupTriggerTestNavObjects()
	{
		this.RemoveTriggerTestNavObjects();
		for (int i = 0; i < this.Triggers.Count; i++)
		{
			NavObject navObject = NavObjectManager.Instance.RegisterNavObject("editor_block_trigger", this.Triggers[i].ToWorldPos().ToVector3Center(), "", false, -1, null);
			navObject.name = this.Triggers[i].TriggerDisplay();
			navObject.OverrideColor = ((this.Triggers[i].TriggeredByIndices.Count > 0) ? Color.blue : Color.red);
		}
	}

	// Token: 0x06000E2A RID: 3626 RVA: 0x0005D5A0 File Offset: 0x0005B7A0
	public void RemoveTriggerTestNavObjects()
	{
		for (int i = 0; i < this.Triggers.Count; i++)
		{
			NavObjectManager.Instance.UnRegisterNavObjectByPosition(this.Triggers[i].ToWorldPos().ToVector3Center(), "editor_block_trigger");
		}
	}

	// Token: 0x04000C11 RID: 3089
	public Dictionary<int, List<BlockTrigger>> TriggeredByDictionary = new Dictionary<int, List<BlockTrigger>>();

	// Token: 0x04000C12 RID: 3090
	public Dictionary<int, List<SleeperVolume>> TriggeredByVolumes = new Dictionary<int, List<SleeperVolume>>();

	// Token: 0x04000C13 RID: 3091
	public PrefabInstance PrefabInstance;

	// Token: 0x04000C14 RID: 3092
	[PublicizedFrom(EAccessModifier.Protected)]
	public World world;

	// Token: 0x04000C15 RID: 3093
	public List<int> PlayersInArea = new List<int>();

	// Token: 0x04000C16 RID: 3094
	public List<byte> TriggeredLayers;

	// Token: 0x04000C17 RID: 3095
	public List<byte> TriggeredByLayers;

	// Token: 0x04000C18 RID: 3096
	public List<BlockTrigger> Triggers = new List<BlockTrigger>();

	// Token: 0x04000C19 RID: 3097
	public TriggerManager Owner;

	// Token: 0x04000C1A RID: 3098
	[PublicizedFrom(EAccessModifier.Private)]
	public float needsTriggerTimer = -1f;
}
