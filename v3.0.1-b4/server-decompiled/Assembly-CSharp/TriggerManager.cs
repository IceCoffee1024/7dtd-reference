using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001C7 RID: 455
public class TriggerManager
{
	// Token: 0x1700010E RID: 270
	// (get) Token: 0x06000DFD RID: 3581 RVA: 0x0005C387 File Offset: 0x0005A587
	// (set) Token: 0x06000DFE RID: 3582 RVA: 0x0005C38F File Offset: 0x0005A58F
	public bool ShowNavObjects
	{
		get
		{
			return this.showNavObjects;
		}
		set
		{
			this.showNavObjects = value;
			this.HandleNavObjects(this.showNavObjects);
		}
	}

	// Token: 0x06000DFF RID: 3583 RVA: 0x0005C3A4 File Offset: 0x0005A5A4
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleNavObjects(bool enabled)
	{
		foreach (PrefabTriggerData prefabTriggerData in this.PrefabDataDict.Values)
		{
			if (enabled)
			{
				prefabTriggerData.SetupTriggerTestNavObjects();
			}
			else
			{
				prefabTriggerData.RemoveTriggerTestNavObjects();
			}
		}
	}

	// Token: 0x06000E00 RID: 3584 RVA: 0x0005C408 File Offset: 0x0005A608
	public void AddPrefabData(PrefabInstance instance, int entityID)
	{
		PrefabTriggerData prefabTriggerData;
		if (!this.PrefabDataDict.ContainsKey(instance))
		{
			prefabTriggerData = new PrefabTriggerData(instance)
			{
				Owner = this
			};
			if (this.ShowNavObjects)
			{
				prefabTriggerData.SetupTriggerTestNavObjects();
			}
			this.PrefabDataDict.Add(instance, prefabTriggerData);
		}
		prefabTriggerData = this.PrefabDataDict[instance];
		prefabTriggerData.RefreshTriggers();
		prefabTriggerData.AddPlayerInArea(entityID);
	}

	// Token: 0x06000E01 RID: 3585 RVA: 0x0005C468 File Offset: 0x0005A668
	public void RefreshTriggers(PrefabInstance instance, FastTags<TagGroup.Global> questTags)
	{
		PrefabTriggerData prefabTriggerData;
		if (!this.PrefabDataDict.ContainsKey(instance))
		{
			prefabTriggerData = new PrefabTriggerData(instance)
			{
				Owner = this
			};
			this.PrefabDataDict.Add(instance, prefabTriggerData);
		}
		else
		{
			prefabTriggerData = this.PrefabDataDict[instance];
			prefabTriggerData.RemoveTriggerTestNavObjects();
			prefabTriggerData.ResetData();
		}
		prefabTriggerData.ResetTriggers();
		prefabTriggerData.RefreshTriggersForQuest(questTags);
		prefabTriggerData.HandleNeedTriggers();
	}

	// Token: 0x06000E02 RID: 3586 RVA: 0x0005C4D0 File Offset: 0x0005A6D0
	public void Trigger(EntityPlayer player, PrefabInstance instance, byte trigger)
	{
		PrefabTriggerData prefabTriggerData;
		if (this.PrefabDataDict.TryGetValue(instance, out prefabTriggerData))
		{
			prefabTriggerData.Trigger(player, trigger);
		}
	}

	// Token: 0x06000E03 RID: 3587 RVA: 0x0005C4F5 File Offset: 0x0005A6F5
	public void TriggerBlocks(EntityPlayer player, PrefabInstance instance, BlockTrigger trigger)
	{
		if (!trigger.HasAnyTriggers())
		{
			return;
		}
		if (this.PrefabDataDict.ContainsKey(instance))
		{
			this.PrefabDataDict[instance].Trigger(player, trigger);
		}
	}

	// Token: 0x06000E04 RID: 3588 RVA: 0x0005C521 File Offset: 0x0005A721
	public void TriggerBlocks(EntityPlayer player, PrefabInstance instance, TriggerVolume trigger)
	{
		if (!trigger.HasAnyTriggers())
		{
			return;
		}
		if (this.PrefabDataDict.ContainsKey(instance))
		{
			this.PrefabDataDict[instance].Trigger(player, trigger);
		}
	}

	// Token: 0x06000E05 RID: 3589 RVA: 0x0005C54D File Offset: 0x0005A74D
	public void RemovePlayer(PrefabInstance instance, int entityID)
	{
		if (this.PrefabDataDict.ContainsKey(instance))
		{
			this.PrefabDataDict[instance].RemovePlayerInArea(entityID);
		}
	}

	// Token: 0x06000E06 RID: 3590 RVA: 0x0005C56F File Offset: 0x0005A76F
	public void RemovePrefabData(PrefabInstance instance)
	{
		if (this.PrefabDataDict.ContainsKey(instance))
		{
			this.PrefabDataDict[instance].RemoveTriggerTestNavObjects();
			this.PrefabDataDict.Remove(instance);
		}
	}

	// Token: 0x06000E07 RID: 3591 RVA: 0x0005C5A0 File Offset: 0x0005A7A0
	public void Update()
	{
		float deltaTime = Time.deltaTime;
		for (int i = this.UpdateList.Count - 1; i >= 0; i--)
		{
			this.UpdateList[i].Update(deltaTime);
		}
	}

	// Token: 0x06000E08 RID: 3592 RVA: 0x0005C5E0 File Offset: 0x0005A7E0
	public List<byte> GetTriggerLayers()
	{
		List<byte> list = new List<byte>();
		foreach (PrefabTriggerData prefabTriggerData in this.PrefabDataDict.Values)
		{
			for (int i = 0; i < prefabTriggerData.TriggeredLayers.Count; i++)
			{
				if (!list.Contains(prefabTriggerData.TriggeredLayers[i]))
				{
					list.Add(prefabTriggerData.TriggeredLayers[i]);
				}
			}
			for (int j = 0; j < prefabTriggerData.TriggeredByLayers.Count; j++)
			{
				if (!list.Contains(prefabTriggerData.TriggeredByLayers[j]))
				{
					list.Add(prefabTriggerData.TriggeredByLayers[j]);
				}
			}
		}
		return list;
	}

	// Token: 0x06000E09 RID: 3593 RVA: 0x0005C6BC File Offset: 0x0005A8BC
	public void AddToUpdateList(PrefabTriggerData prefabTriggerData)
	{
		if (!this.UpdateList.Contains(prefabTriggerData))
		{
			this.UpdateList.Add(prefabTriggerData);
		}
	}

	// Token: 0x06000E0A RID: 3594 RVA: 0x0005C6D8 File Offset: 0x0005A8D8
	public void RemoveFromUpdateList(PrefabTriggerData prefabTriggerData)
	{
		if (this.UpdateList.Contains(prefabTriggerData))
		{
			this.UpdateList.Remove(prefabTriggerData);
		}
	}

	// Token: 0x06000E0B RID: 3595 RVA: 0x0005C6F8 File Offset: 0x0005A8F8
	public void RemoveFromUpdateList(PrefabInstance instance)
	{
		for (int i = this.UpdateList.Count - 1; i >= 0; i--)
		{
			if (this.UpdateList[i].PrefabInstance == instance)
			{
				this.UpdateList.RemoveAt(i);
			}
		}
	}

	// Token: 0x06000E0C RID: 3596 RVA: 0x0005C740 File Offset: 0x0005A940
	[PublicizedFrom(EAccessModifier.Private)]
	public QuestGeneratorController getGeneratorController(Vector3i rootPos, Chunk chunk)
	{
		QuestGeneratorController questGeneratorController = null;
		BlockEntityData blockEntity = chunk.GetBlockEntity(rootPos);
		if (blockEntity != null && blockEntity.transform != null)
		{
			questGeneratorController = blockEntity.transform.GetComponent<QuestGeneratorController>();
		}
		return questGeneratorController;
	}

	// Token: 0x06000E0D RID: 3597 RVA: 0x0005C778 File Offset: 0x0005A978
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockSwitchController getSwitchController(Vector3i rootPos, Chunk chunk)
	{
		BlockSwitchController blockSwitchController = null;
		BlockEntityData blockEntity = chunk.GetBlockEntity(rootPos);
		if (blockEntity != null && blockEntity.transform != null)
		{
			blockSwitchController = blockEntity.transform.GetComponent<BlockSwitchController>();
		}
		return blockSwitchController;
	}

	// Token: 0x06000E0E RID: 3598 RVA: 0x0005C7AD File Offset: 0x0005A9AD
	public IEnumerator CheckPowerState(BlockTrigger _triggered, FastTags<TagGroup.Global> questTag, Action<bool> callback)
	{
		bool flag = false;
		if (_triggered.HasAnyTriggeredBy())
		{
			World world = GameManager.Instance.World;
			DynamicPrefabDecorator dynamicPrefabDecorator = GameManager.Instance.GetDynamicPrefabDecorator();
			Vector3i vector3i = _triggered.ToWorldPos();
			PrefabInstance prefabAtPosition = dynamicPrefabDecorator.GetPrefabAtPosition(vector3i, null, null);
			bool isRestorePower = questTag.Test_AnySet(QuestEventManager.restorePowerTag);
			if (prefabAtPosition != null)
			{
				PrefabTriggerData pfd;
				if (this.PrefabDataDict.TryGetValue(prefabAtPosition, out pfd))
				{
					int triggerCount = 0;
					foreach (byte triggerIndex in _triggered.TriggeredByIndices)
					{
						bool indexTriggered = false;
						foreach (BlockTrigger blockTrigger in pfd.Triggers)
						{
							if (blockTrigger.TriggersIndices.Contains(triggerIndex))
							{
								Vector3i rootPos = blockTrigger.ToWorldPos();
								Chunk chunk;
								do
								{
									chunk = (Chunk)world.GetChunkFromWorldPos(rootPos);
									if (chunk == null)
									{
										yield return null;
									}
								}
								while (chunk == null);
								QuestGeneratorController generatorController = this.getGeneratorController(rootPos, chunk);
								if (generatorController != null)
								{
									if (!isRestorePower)
									{
										indexTriggered = true;
									}
									else
									{
										indexTriggered = generatorController.IsRunning;
										if (indexTriggered)
										{
											break;
										}
									}
								}
								else
								{
									BlockSwitchController switchController = this.getSwitchController(rootPos, chunk);
									if (switchController != null)
									{
										indexTriggered = switchController.Activated;
									}
									if (indexTriggered)
									{
										break;
									}
								}
								chunk = null;
							}
						}
						List<BlockTrigger>.Enumerator enumerator2 = default(List<BlockTrigger>.Enumerator);
						if (indexTriggered)
						{
							int num = triggerCount;
							triggerCount = num + 1;
						}
					}
					List<byte>.Enumerator enumerator = default(List<byte>.Enumerator);
					if (_triggered.UseOrForMultipleTriggers)
					{
						flag = triggerCount > 0;
					}
					else
					{
						flag = triggerCount == _triggered.TriggeredByIndices.Count;
					}
				}
				pfd = null;
			}
			world = null;
		}
		else
		{
			flag = true;
		}
		callback(flag);
		yield break;
		yield break;
	}

	// Token: 0x04000BFE RID: 3070
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<PrefabInstance, PrefabTriggerData> PrefabDataDict = new Dictionary<PrefabInstance, PrefabTriggerData>();

	// Token: 0x04000BFF RID: 3071
	public List<PrefabTriggerData> UpdateList = new List<PrefabTriggerData>();

	// Token: 0x04000C00 RID: 3072
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showNavObjects;
}
