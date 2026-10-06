using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F3D RID: 3901
[Preserve]
public class XUiC_EditorStat : XUiController
{
	// Token: 0x17000E5D RID: 3677
	// (get) Token: 0x060079F0 RID: 31216 RVA: 0x002E1D59 File Offset: 0x002DFF59
	public bool hasPrefabLoaded
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return PrefabEditModeManager.Instance.IsActive() && PrefabEditModeManager.Instance.VoxelPrefab != null && PrefabEditModeManager.Instance.VoxelPrefab.location.Type != PathAbstractions.EAbstractedLocationType.None;
		}
	}

	// Token: 0x17000E5E RID: 3678
	// (get) Token: 0x060079F1 RID: 31217 RVA: 0x002E1D8F File Offset: 0x002DFF8F
	public Prefab selectedPrefab
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			DynamicPrefabDecorator dynamicPrefabDecorator = GameManager.Instance.GetDynamicPrefabDecorator();
			if (dynamicPrefabDecorator == null)
			{
				return null;
			}
			PrefabInstance activePrefab = dynamicPrefabDecorator.ActivePrefab;
			if (activePrefab == null)
			{
				return null;
			}
			return activePrefab.prefab;
		}
	}

	// Token: 0x17000E5F RID: 3679
	// (get) Token: 0x060079F2 RID: 31218 RVA: 0x002E1DB1 File Offset: 0x002DFFB1
	public bool hasPrefabSelected
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.selectedPrefab != null;
		}
	}

	// Token: 0x17000E60 RID: 3680
	// (get) Token: 0x060079F3 RID: 31219 RVA: 0x002E1DBC File Offset: 0x002DFFBC
	// (set) Token: 0x060079F4 RID: 31220 RVA: 0x002E1DC3 File Offset: 0x002DFFC3
	public static WorldStats ManualStats
	{
		get
		{
			return XUiC_EditorStat.manualStats;
		}
		set
		{
			XUiC_EditorStat.ManualStatsUpdateTime = DateTime.Now;
			XUiC_EditorStat.manualStats = value;
		}
	}

	// Token: 0x060079F5 RID: 31221 RVA: 0x0029936D File Offset: 0x0029756D
	public override void OnOpen()
	{
		base.OnOpen();
		this.IsDirty = true;
	}

	// Token: 0x060079F6 RID: 31222 RVA: 0x002E1DD8 File Offset: 0x002DFFD8
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty || Time.time - this.lastDirtyTime >= 1f)
		{
			this.lootContainers = 0;
			this.fetchLootContainers = 0;
			this.restorePowerNodes = 0;
			this.totalBlockEntities = 0;
			this.hasSelection = false;
			this.selectionSize = default(Vector3i);
			if (this.hasPrefabLoaded)
			{
				SelectionBox selection = SelectionBoxManager.Instance.Selection;
				if (selection != null)
				{
					this.selectionSize = selection.GetScale();
					this.hasSelection = true;
				}
				if (this.hasLootStat)
				{
					PrefabEditModeManager.Instance.GetLootAndFetchLootContainerCount(out this.lootContainers, out this.fetchLootContainers, out this.restorePowerNodes);
				}
				if (this.hasBlockEntitiesStat)
				{
					foreach (ChunkGameObject chunkGameObject in GameManager.Instance.World.m_ChunkManager.GetUsedChunkGameObjects())
					{
						this.totalBlockEntities += this.countBlockEntities(chunkGameObject.transform);
					}
				}
			}
			base.RefreshBindings();
			this.IsDirty = false;
			this.lastDirtyTime = Time.time;
		}
	}

	// Token: 0x060079F7 RID: 31223 RVA: 0x002E1F18 File Offset: 0x002E0118
	[PublicizedFrom(EAccessModifier.Private)]
	public int countBlockEntities(Transform _t)
	{
		int num = 0;
		for (int i = 0; i < _t.childCount; i++)
		{
			Transform child = _t.GetChild(i);
			if (child.name == "_BlockEntities")
			{
				num += child.childCount;
			}
			else
			{
				num += this.countBlockEntities(child);
			}
		}
		return num;
	}

	// Token: 0x060079F8 RID: 31224 RVA: 0x002E1F68 File Offset: 0x002E0168
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		Prefab prefab = (this.hasPrefabLoaded ? PrefabEditModeManager.Instance.VoxelPrefab : this.selectedPrefab);
		bool flag = prefab != null;
		bool flag2 = ((prefab != null) ? prefab.RenderingCostStats : null) != null;
		bool flag3 = XUiC_EditorStat.ManualStats != null;
		uint num = <PrivateImplementationDetails>.ComputeStringHash(_bindingName);
		if (num <= 2389907569U)
		{
			if (num <= 1501340122U)
			{
				if (num <= 1007605620U)
				{
					if (num != 122989552U)
					{
						if (num != 856865979U)
						{
							if (num == 1007605620U)
							{
								if (_bindingName == "has_selected_prefab")
								{
									_value = this.hasPrefabSelected.ToString();
									return true;
								}
							}
						}
						else if (_bindingName == "show_quest_clear_count")
						{
							_value = ((prefab != null) ? prefab.ShowQuestClearCount.ToString() : null) ?? "";
							return true;
						}
					}
					else if (_bindingName == "drawcalls")
					{
						_value = this.batchesFormatter.Format(this.drawcallsSum / 20);
						return true;
					}
				}
				else if (num != 1281920084U)
				{
					if (num != 1330804927U)
					{
						if (num == 1501340122U)
						{
							if (_bindingName == "has_selection")
							{
								_value = this.hasSelection.ToString();
								return true;
							}
						}
					}
					else if (_bindingName == "selection_size")
					{
						_value = (this.hasSelection ? this.selectionSizeFormatter.Format(this.selectionSize) : "-");
						return true;
					}
				}
				else if (_bindingName == "loot_containers")
				{
					this.hasLootStat = true;
					_value = this.lootFormatter.Format(this.lootContainers);
					return true;
				}
			}
			else if (num <= 1871983191U)
			{
				if (num != 1506237936U)
				{
					if (num != 1724383115U)
					{
						if (num == 1871983191U)
						{
							if (_bindingName == "tris")
							{
								_value = "";
								return true;
							}
						}
					}
					else if (_bindingName == "statsVertices")
					{
						_value = (flag2 ? this.statsVertsFormatter.Format(prefab.RenderingCostStats.TotalVertices) : "-");
						return true;
					}
				}
				else if (_bindingName == "has_loaded_prefab")
				{
					_value = this.hasPrefabLoaded.ToString();
					return true;
				}
			}
			else if (num != 2084430828U)
			{
				if (num != 2297592010U)
				{
					if (num == 2389907569U)
					{
						if (_bindingName == "statsTriangles")
						{
							_value = (flag2 ? this.statsTrisFormatter.Format(prefab.RenderingCostStats.TotalTriangles) : "-");
							return true;
						}
					}
				}
				else if (_bindingName == "statsManualUpdateTime")
				{
					_value = (flag3 ? this.statsManualUpdateTimeFormatter.Format(XUiC_EditorStat.ManualStatsUpdateTime.ToLocalTime()) : "<not captured>");
					return true;
				}
			}
			else if (_bindingName == "restorepower_nodes")
			{
				this.hasLootStat = true;
				_value = this.restorepowerFormatter.Format(this.restorePowerNodes);
				return true;
			}
		}
		else if (num <= 3034812289U)
		{
			if (num <= 2659686822U)
			{
				if (num != 2502672311U)
				{
					if (num != 2524445748U)
					{
						if (num == 2659686822U)
						{
							if (_bindingName == "prefab_volume")
							{
								_value = (flag ? prefab.size.Volume().ToString() : "");
								return true;
							}
						}
					}
					else if (_bindingName == "block_entities")
					{
						this.hasBlockEntitiesStat = true;
						_value = this.blockentitiesFormatter.Format(this.totalBlockEntities);
						return true;
					}
				}
				else if (_bindingName == "loaded_prefab_name")
				{
					_value = ((prefab != null) ? prefab.PrefabName : null) ?? "";
					return true;
				}
			}
			else if (num != 2708140049U)
			{
				if (num != 2723945129U)
				{
					if (num == 3034812289U)
					{
						if (_bindingName == "statsManualLightsVolume")
						{
							if (!flag3)
							{
								_value = "-";
								return true;
							}
							float lightsVolume = XUiC_EditorStat.ManualStats.LightsVolume;
							int num2 = ((prefab != null) ? prefab.size.Volume() : 0);
							_value = string.Format("{0:F0} ({1:P1})", lightsVolume, lightsVolume / (float)num2);
							return true;
						}
					}
				}
				else if (_bindingName == "statsManualVertices")
				{
					_value = (flag3 ? this.statsManualVertsFormatter.Format(XUiC_EditorStat.ManualStats.TotalVertices) : "-");
					return true;
				}
			}
			else if (_bindingName == "difficulty_tier")
			{
				_value = ((prefab != null) ? prefab.DifficultyTier.ToString() : null) ?? "";
				return true;
			}
		}
		else if (num <= 3347817698U)
		{
			if (num != 3088255484U)
			{
				if (num != 3167497763U)
				{
					if (num == 3347817698U)
					{
						if (_bindingName == "fetchloot_containers")
						{
							this.hasLootStat = true;
							_value = this.fetchlootFormatter.Format(this.fetchLootContainers);
							return true;
						}
					}
				}
				else if (_bindingName == "verts")
				{
					_value = "";
					return true;
				}
			}
			else if (_bindingName == "sleeper_info")
			{
				_value = ((prefab != null) ? prefab.CalcSleeperInfo() : null) ?? "";
				return true;
			}
		}
		else if (num <= 3964975885U)
		{
			if (num != 3656015704U)
			{
				if (num == 3964975885U)
				{
					if (_bindingName == "prefab_size")
					{
						_value = (flag ? this.prefabSizeFormatter.Format(prefab.size) : "");
						return true;
					}
				}
			}
			else if (_bindingName == "loaded_prefab_changed")
			{
				_value = ((this.hasPrefabLoaded && PrefabEditModeManager.Instance.NeedsSaving) ? "*" : "");
				return true;
			}
		}
		else if (num != 4048861779U)
		{
			if (num == 4061059603U)
			{
				if (_bindingName == "statsManualTriangles")
				{
					_value = (flag3 ? this.statsManualTrisFormatter.Format(XUiC_EditorStat.ManualStats.TotalTriangles) : "-");
					return true;
				}
			}
		}
		else if (_bindingName == "statsLightsVolume")
		{
			if (!flag2)
			{
				_value = "-";
				return true;
			}
			float lightsVolume2 = prefab.RenderingCostStats.LightsVolume;
			int num3 = prefab.size.Volume();
			_value = string.Format("{0:F0} ({1:P1})", lightsVolume2, lightsVolume2 / (float)num3);
			return true;
		}
		return base.GetBindingValueInternal(ref _value, _bindingName);
	}

	// Token: 0x04005A4C RID: 23116
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastDirtyTime;

	// Token: 0x04005A4D RID: 23117
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasLootStat;

	// Token: 0x04005A4E RID: 23118
	[PublicizedFrom(EAccessModifier.Private)]
	public int lootContainers;

	// Token: 0x04005A4F RID: 23119
	[PublicizedFrom(EAccessModifier.Private)]
	public int fetchLootContainers;

	// Token: 0x04005A50 RID: 23120
	[PublicizedFrom(EAccessModifier.Private)]
	public int restorePowerNodes;

	// Token: 0x04005A51 RID: 23121
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasBlockEntitiesStat;

	// Token: 0x04005A52 RID: 23122
	[PublicizedFrom(EAccessModifier.Private)]
	public int totalBlockEntities;

	// Token: 0x04005A53 RID: 23123
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasSelection;

	// Token: 0x04005A54 RID: 23124
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i selectionSize;

	// Token: 0x04005A55 RID: 23125
	[PublicizedFrom(EAccessModifier.Private)]
	public const int DC_AVERAGE_FRAMES = 20;

	// Token: 0x04005A56 RID: 23126
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int[] drawcallsBuf = new int[20];

	// Token: 0x04005A57 RID: 23127
	[PublicizedFrom(EAccessModifier.Private)]
	public int drawcallsBufIndex;

	// Token: 0x04005A58 RID: 23128
	[PublicizedFrom(EAccessModifier.Private)]
	public int drawcallsSum;

	// Token: 0x04005A59 RID: 23129
	[PublicizedFrom(EAccessModifier.Private)]
	public static WorldStats manualStats;

	// Token: 0x04005A5A RID: 23130
	[PublicizedFrom(EAccessModifier.Private)]
	public static DateTime ManualStatsUpdateTime;

	// Token: 0x04005A5B RID: 23131
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<Vector3i> prefabSizeFormatter = new CachedStringFormatter<Vector3i>((Vector3i _i) => _i.ToString());

	// Token: 0x04005A5C RID: 23132
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<Vector3i> selectionSizeFormatter = new CachedStringFormatter<Vector3i>((Vector3i _i) => _i.ToString());

	// Token: 0x04005A5D RID: 23133
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt lootFormatter = new CachedStringFormatterInt();

	// Token: 0x04005A5E RID: 23134
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt fetchlootFormatter = new CachedStringFormatterInt();

	// Token: 0x04005A5F RID: 23135
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt restorepowerFormatter = new CachedStringFormatterInt();

	// Token: 0x04005A60 RID: 23136
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt blockentitiesFormatter = new CachedStringFormatterInt();

	// Token: 0x04005A61 RID: 23137
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<int> vertsFormatter = new CachedStringFormatter<int>((int _i) => ((double)_i).FormatNumberWithMetricPrefix(true, 3));

	// Token: 0x04005A62 RID: 23138
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<int> trisFormatter = new CachedStringFormatter<int>((int _i) => ((double)_i).FormatNumberWithMetricPrefix(true, 3));

	// Token: 0x04005A63 RID: 23139
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt batchesFormatter = new CachedStringFormatterInt();

	// Token: 0x04005A64 RID: 23140
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt statsVertsFormatter = new CachedStringFormatterInt();

	// Token: 0x04005A65 RID: 23141
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt statsTrisFormatter = new CachedStringFormatterInt();

	// Token: 0x04005A66 RID: 23142
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<DateTime> statsManualUpdateTimeFormatter = new CachedStringFormatter<DateTime>((DateTime _dt) => _dt.ToString(Utils.StandardCulture));

	// Token: 0x04005A67 RID: 23143
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt statsManualVertsFormatter = new CachedStringFormatterInt();

	// Token: 0x04005A68 RID: 23144
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt statsManualTrisFormatter = new CachedStringFormatterInt();
}
