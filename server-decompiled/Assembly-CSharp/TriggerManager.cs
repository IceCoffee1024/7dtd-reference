using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001C9 RID: 457
public class TriggerManager
{
	// Token: 0x1700010E RID: 270
	// (get) Token: 0x06000E08 RID: 3592 RVA: 0x0005C807 File Offset: 0x0005AA07
	// (set) Token: 0x06000E09 RID: 3593 RVA: 0x0005C80F File Offset: 0x0005AA0F
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

	// Token: 0x06000E0A RID: 3594 RVA: 0x0005C824 File Offset: 0x0005AA24
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

	// Token: 0x06000E0B RID: 3595 RVA: 0x0005C888 File Offset: 0x0005AA88
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

	// Token: 0x06000E0C RID: 3596 RVA: 0x0005C8E8 File Offset: 0x0005AAE8
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

	// Token: 0x06000E0D RID: 3597 RVA: 0x0005C950 File Offset: 0x0005AB50
	public void Trigger(EntityPlayer player, PrefabInstance instance, byte trigger)
	{
		PrefabTriggerData prefabTriggerData;
		if (this.PrefabDataDict.TryGetValue(instance, out prefabTriggerData))
		{
			prefabTriggerData.Trigger(player, trigger);
		}
	}

	// Token: 0x06000E0E RID: 3598 RVA: 0x0005C975 File Offset: 0x0005AB75
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

	// Token: 0x06000E0F RID: 3599 RVA: 0x0005C9A4 File Offset: 0x0005ABA4
	public void TriggerBlocks(EntityPlayer player, PrefabInstance instance, TriggerVolume trigger)
	{
		if (!trigger.HasAnyTriggers())
		{
			return;
		}
		if (instance == null)
		{
			Log.Warning(string.Format("Cannot do {0} for TriggerVolume at {1}. No prefab instance assigned", "TriggerBlocks", trigger.BoxMin));
			return;
		}
		if (this.PrefabDataDict.ContainsKey(instance))
		{
			this.PrefabDataDict[instance].Trigger(player, trigger);
		}
	}

	// Token: 0x06000E10 RID: 3600 RVA: 0x0005C9FE File Offset: 0x0005ABFE
	public void RemovePlayer(PrefabInstance instance, int entityID)
	{
		if (this.PrefabDataDict.ContainsKey(instance))
		{
			this.PrefabDataDict[instance].RemovePlayerInArea(entityID);
		}
	}

	// Token: 0x06000E11 RID: 3601 RVA: 0x0005CA20 File Offset: 0x0005AC20
	public void RemovePrefabData(PrefabInstance instance)
	{
		if (this.PrefabDataDict.ContainsKey(instance))
		{
			this.PrefabDataDict[instance].RemoveTriggerTestNavObjects();
			this.PrefabDataDict.Remove(instance);
		}
	}

	// Token: 0x06000E12 RID: 3602 RVA: 0x0005CA50 File Offset: 0x0005AC50
	public void Update()
	{
		float deltaTime = Time.deltaTime;
		for (int i = this.UpdateList.Count - 1; i >= 0; i--)
		{
			this.UpdateList[i].Update(deltaTime);
		}
	}

	// Token: 0x06000E13 RID: 3603 RVA: 0x0005CA90 File Offset: 0x0005AC90
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

	// Token: 0x06000E14 RID: 3604 RVA: 0x0005CB6C File Offset: 0x0005AD6C
	public void AddToUpdateList(PrefabTriggerData prefabTriggerData)
	{
		if (!this.UpdateList.Contains(prefabTriggerData))
		{
			this.UpdateList.Add(prefabTriggerData);
		}
	}

	// Token: 0x06000E15 RID: 3605 RVA: 0x0005CB88 File Offset: 0x0005AD88
	public void RemoveFromUpdateList(PrefabTriggerData prefabTriggerData)
	{
		if (this.UpdateList.Contains(prefabTriggerData))
		{
			this.UpdateList.Remove(prefabTriggerData);
		}
	}

	// Token: 0x06000E16 RID: 3606 RVA: 0x0005CBA8 File Offset: 0x0005ADA8
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

	// Token: 0x06000E17 RID: 3607 RVA: 0x0005CBF0 File Offset: 0x0005ADF0
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

	// Token: 0x06000E18 RID: 3608 RVA: 0x0005CC28 File Offset: 0x0005AE28
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

	// Token: 0x06000E19 RID: 3609 RVA: 0x0005CC5D File Offset: 0x0005AE5D
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

	// Token: 0x04000C0B RID: 3083
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<PrefabInstance, PrefabTriggerData> PrefabDataDict = new Dictionary<PrefabInstance, PrefabTriggerData>();

	// Token: 0x04000C0C RID: 3084
	public List<PrefabTriggerData> UpdateList = new List<PrefabTriggerData>();

	// Token: 0x04000C0D RID: 3085
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showNavObjects;
}
