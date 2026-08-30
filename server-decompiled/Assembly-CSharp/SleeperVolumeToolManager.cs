using System;
using System.Collections.Generic;
using PrefabVolumes;
using UnityEngine;

// Token: 0x0200136E RID: 4974
public static class SleeperVolumeToolManager
{
	// Token: 0x17001293 RID: 4755
	// (get) Token: 0x06009CD8 RID: 40152 RVA: 0x003B37D8 File Offset: 0x003B19D8
	public static SelectionBox SelectedSleeperBox
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			SelectionBox selection = SelectionBoxManager.Instance.Selection;
			if (selection == null)
			{
				return null;
			}
			if (selection.Category != SelectionBoxManager.Instance.CategorySleeperVolume)
			{
				return null;
			}
			return selection;
		}
	}

	// Token: 0x06009CD9 RID: 40153 RVA: 0x003B3810 File Offset: 0x003B1A10
	public static void RegisterSleeperBlock(BlockValue _bv, Transform _prefabTrans, Vector3i _position)
	{
		BlockSleeper blockSleeper = _bv.Block as BlockSleeper;
		if (blockSleeper == null)
		{
			Log.Warning("SleeperVolumeToolManager RegisterSleeperBlock not sleeper {0}", new object[] { _bv });
			return;
		}
		if (SleeperVolumeToolManager.typeMats.Count == 0)
		{
			Shader shader = Shader.Find("Game/UI/Sleeper");
			foreach (Color color in SleeperVolumeToolManager.typeColors)
			{
				Material material = new Material(shader);
				material.renderQueue = 4001;
				material.color = color;
				SleeperVolumeToolManager.typeMats.Add(material);
			}
		}
		Vector3i worldPos = GameManager.Instance.World.ChunkCache.GetChunkFromWorldPos(_position).GetWorldPos();
		SleeperVolumeToolManager.BlockData blockData = new SleeperVolumeToolManager.BlockData
		{
			block = blockSleeper,
			prefabT = _prefabTrans,
			position = _position
		};
		_prefabTrans.position = _prefabTrans.position + _position.ToVector3() + Vector3.one * 0.5f + Vector3.up * 0.01f;
		if (SleeperVolumeToolManager.GroupGameObject == null)
		{
			SleeperVolumeToolManager.GroupGameObject = new GameObject();
			SleeperVolumeToolManager.GroupGameObject.name = "SleeperVolumeToolManagerPrefabs";
		}
		_prefabTrans.parent = SleeperVolumeToolManager.GroupGameObject.transform;
		List<SleeperVolumeToolManager.BlockData> list;
		if (!SleeperVolumeToolManager.sleepers.TryGetValue(worldPos, out list))
		{
			list = new List<SleeperVolumeToolManager.BlockData>();
			SleeperVolumeToolManager.sleepers.Add(worldPos, list);
		}
		list.Add(blockData);
		SleeperVolumeToolManager.UpdateSleeperBlockVisuals(blockData, SleeperVolumeToolManager.SelectedSleeperBox);
	}

	// Token: 0x06009CDA RID: 40154 RVA: 0x003B3990 File Offset: 0x003B1B90
	public static void UnRegisterSleeperBlock(Vector3i _position)
	{
		if (SleeperVolumeToolManager.sleepers == null)
		{
			return;
		}
		Vector3i worldPos = GameManager.Instance.World.ChunkCache.GetChunkFromWorldPos(_position).GetWorldPos();
		List<SleeperVolumeToolManager.BlockData> list;
		if (SleeperVolumeToolManager.sleepers.TryGetValue(worldPos, out list))
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].position == _position)
				{
					global::UnityEngine.Object.Destroy(list[i].prefabT.gameObject);
					list.RemoveAt(i);
					return;
				}
			}
		}
	}

	// Token: 0x06009CDB RID: 40155 RVA: 0x003B3A14 File Offset: 0x003B1C14
	public static void CleanUp()
	{
		if (SleeperVolumeToolManager.sleepers != null)
		{
			foreach (KeyValuePair<Vector3i, List<SleeperVolumeToolManager.BlockData>> keyValuePair in SleeperVolumeToolManager.sleepers)
			{
				Vector3i vector3i;
				List<SleeperVolumeToolManager.BlockData> list;
				keyValuePair.Deconstruct(out vector3i, out list);
				foreach (SleeperVolumeToolManager.BlockData blockData in list)
				{
					Transform prefabT = blockData.prefabT;
					if (prefabT)
					{
						global::UnityEngine.Object.Destroy(prefabT.gameObject);
					}
				}
			}
			SleeperVolumeToolManager.sleepers.Clear();
		}
		SleeperVolumeToolManager.ClearSleeperVolumes();
	}

	// Token: 0x06009CDC RID: 40156 RVA: 0x003B3AD8 File Offset: 0x003B1CD8
	public static void RegisterSleeperVolume(SelectionBox _selBox)
	{
		if (_selBox == null)
		{
			return;
		}
		if (!(_selBox.UserData is PrefabSleeperVolume))
		{
			return;
		}
		if (!SleeperVolumeToolManager.registeredSleeperVolumes.Contains(_selBox))
		{
			SleeperVolumeToolManager.registeredSleeperVolumes.Add(_selBox);
		}
	}

	// Token: 0x06009CDD RID: 40157 RVA: 0x003B3B0A File Offset: 0x003B1D0A
	public static void UnRegisterSleeperVolume(SelectionBox _selBox)
	{
		SleeperVolumeToolManager.registeredSleeperVolumes.Remove(_selBox);
	}

	// Token: 0x06009CDE RID: 40158 RVA: 0x003B3B18 File Offset: 0x003B1D18
	public static void ClearSleeperVolumes()
	{
		SleeperVolumeToolManager.registeredSleeperVolumes.Clear();
	}

	// Token: 0x06009CDF RID: 40159 RVA: 0x003B3B24 File Offset: 0x003B1D24
	public static void CheckKeys()
	{
		if (!Input.GetKeyDown(KeyCode.RightBracket))
		{
			return;
		}
		if (!SleeperVolumeToolManager.SelectedSleeperBox)
		{
			return;
		}
		PrefabSleeperVolume prefabSleeperVolume = SleeperVolumeToolManager.SelectedSleeperBox.UserData as PrefabSleeperVolume;
		if (prefabSleeperVolume == null)
		{
			return;
		}
		int num;
		PrefabInstance prefabInstance;
		if (!PrefabVolumeManager.GetPrefabIdAndVolumeId(SleeperVolumeToolManager.SelectedSleeperBox.name, out num, out prefabInstance))
		{
			return;
		}
		PrefabSleeperVolume prefabSleeperVolume2 = prefabSleeperVolume.CloneGeneric();
		if (InputUtils.ShiftKeyPressed)
		{
			prefabSleeperVolume2.groupId = 0;
			PrefabVolumeManager.Instance.UpdatePropertiesServer(prefabInstance.id, num, prefabSleeperVolume2);
			Log.Out(string.Format("Set sleeper volume {0} to group ID {1}", prefabSleeperVolume.startPos, 0));
			return;
		}
		short num2 = -1;
		if (SleeperVolumeToolManager.previousSelectionBox != null)
		{
			PrefabSleeperVolume prefabSleeperVolume3 = SleeperVolumeToolManager.previousSelectionBox.UserData as PrefabSleeperVolume;
			if (prefabSleeperVolume3 != null)
			{
				num2 = prefabSleeperVolume3.groupId;
				int num3;
				PrefabInstance prefabInstance2;
				if (num2 == 0 && PrefabVolumeManager.GetPrefabIdAndVolumeId(SleeperVolumeToolManager.previousSelectionBox.name, out num3, out prefabInstance2))
				{
					num2 = prefabInstance2.prefab.FindSleeperVolumeFreeGroupId();
					PrefabSleeperVolume prefabSleeperVolume4 = prefabSleeperVolume3.CloneGeneric();
					prefabSleeperVolume4.groupId = num2;
					PrefabVolumeManager.Instance.UpdatePropertiesServer(prefabInstance2.id, num3, prefabSleeperVolume4);
					Log.Out(string.Format("Set sleeper volume {0} to new group ID {1}", prefabSleeperVolume3.startPos, num2));
				}
			}
		}
		if (num2 >= 0)
		{
			prefabSleeperVolume2.groupId = num2;
			PrefabVolumeManager.Instance.UpdatePropertiesServer(prefabInstance.id, num, prefabSleeperVolume2);
			Log.Out(string.Format("Set sleeper volume {0} to group ID {1}", prefabSleeperVolume.startPos, num2));
		}
	}

	// Token: 0x06009CE0 RID: 40160 RVA: 0x003B3C9B File Offset: 0x003B1E9B
	public static void SetVisible(bool _visible)
	{
		if (_visible)
		{
			SleeperVolumeToolManager.SelectionChanged(null);
			return;
		}
		SleeperVolumeToolManager.hideSleeperBlocks();
	}

	// Token: 0x06009CE1 RID: 40161 RVA: 0x003B3CAC File Offset: 0x003B1EAC
	public static void SelectionChanged(SelectionBox _selBox)
	{
		if (_selBox)
		{
			if (SleeperVolumeToolManager.currentSelectionBox && _selBox != SleeperVolumeToolManager.currentSelectionBox)
			{
				SleeperVolumeToolManager.previousSelectionBox = SleeperVolumeToolManager.currentSelectionBox;
			}
			SleeperVolumeToolManager.currentSelectionBox = _selBox;
		}
		SleeperVolumeToolManager.UpdateAllVisuals();
	}

	// Token: 0x06009CE2 RID: 40162 RVA: 0x003B3CE4 File Offset: 0x003B1EE4
	public static void UpdateAllVisuals()
	{
		SleeperVolumeToolManager.UpdateAllSleeperBlockVisuals();
		SleeperVolumeToolManager.UpdateSleeperVolumeColors();
	}

	// Token: 0x06009CE3 RID: 40163 RVA: 0x003B3CF0 File Offset: 0x003B1EF0
	[PublicizedFrom(EAccessModifier.Private)]
	public static void UpdateSleeperVolumeColors()
	{
		int num = 0;
		if (SleeperVolumeToolManager.SelectedSleeperBox)
		{
			num = (int)((PrefabSleeperVolume)SleeperVolumeToolManager.SelectedSleeperBox.UserData).groupId;
		}
		SelectionCategory categorySleeperVolume = SelectionBoxManager.Instance.CategorySleeperVolume;
		foreach (SelectionBox selectionBox in SleeperVolumeToolManager.registeredSleeperVolumes)
		{
			PrefabSleeperVolume prefabSleeperVolume = (PrefabSleeperVolume)selectionBox.UserData;
			if (prefabSleeperVolume.groupId != 0)
			{
				if ((int)prefabSleeperVolume.groupId == num)
				{
					selectionBox.SetAllFacesColor(SleeperVolumeToolManager.groupSelectedColor, true);
				}
				else
				{
					selectionBox.SetAllFacesColor(SleeperVolumeToolManager.groupColors[(int)prefabSleeperVolume.groupId % SleeperVolumeToolManager.groupColors.Length], true);
				}
			}
			else
			{
				selectionBox.SetAllFacesColor(categorySleeperVolume.colInactive, true);
			}
		}
		if (SleeperVolumeToolManager.SelectedSleeperBox)
		{
			SleeperVolumeToolManager.SelectedSleeperBox.SetAllFacesColor(categorySleeperVolume.colActive, true);
		}
	}

	// Token: 0x06009CE4 RID: 40164 RVA: 0x003B3DE4 File Offset: 0x003B1FE4
	[PublicizedFrom(EAccessModifier.Private)]
	public static void hideSleeperBlocks()
	{
		if (SleeperVolumeToolManager.sleepers == null)
		{
			return;
		}
		foreach (KeyValuePair<Vector3i, List<SleeperVolumeToolManager.BlockData>> keyValuePair in SleeperVolumeToolManager.sleepers)
		{
			Vector3i vector3i;
			List<SleeperVolumeToolManager.BlockData> list;
			keyValuePair.Deconstruct(out vector3i, out list);
			foreach (SleeperVolumeToolManager.BlockData blockData in list)
			{
				Transform prefabT = blockData.prefabT;
				if (prefabT)
				{
					prefabT.gameObject.SetActive(false);
				}
			}
		}
	}

	// Token: 0x06009CE5 RID: 40165 RVA: 0x003B3E98 File Offset: 0x003B2098
	[PublicizedFrom(EAccessModifier.Private)]
	public static void UpdateAllSleeperBlockVisuals()
	{
		if (SleeperVolumeToolManager.sleepers == null)
		{
			return;
		}
		SelectionBox selectedSleeperBox = SleeperVolumeToolManager.SelectedSleeperBox;
		foreach (KeyValuePair<Vector3i, List<SleeperVolumeToolManager.BlockData>> keyValuePair in SleeperVolumeToolManager.sleepers)
		{
			Vector3i vector3i;
			List<SleeperVolumeToolManager.BlockData> list;
			keyValuePair.Deconstruct(out vector3i, out list);
			foreach (SleeperVolumeToolManager.BlockData blockData in list)
			{
				SleeperVolumeToolManager.UpdateSleeperBlockVisuals(blockData, selectedSleeperBox);
			}
		}
	}

	// Token: 0x06009CE6 RID: 40166 RVA: 0x003B3F3C File Offset: 0x003B213C
	[PublicizedFrom(EAccessModifier.Private)]
	public static void UpdateSleeperBlockVisuals(SleeperVolumeToolManager.BlockData _data, SelectionBox _selectedBox)
	{
		Transform prefabT = _data.prefabT;
		if (!SelectionBoxManager.Instance.CategorySleeperVolume.IsVisible() || (_selectedBox == null && !SleeperVolumeToolManager.xRayOn))
		{
			prefabT.gameObject.SetActive(false);
			return;
		}
		Vector3i vector3i = Vector3i.min;
		Vector3i vector3i2 = Vector3i.min;
		if (_selectedBox != null)
		{
			vector3i = Vector3i.FromVector3Rounded(_selectedBox.bounds.min);
			vector3i2 = Vector3i.FromVector3Rounded(_selectedBox.bounds.max);
		}
		Vector3i position = _data.position;
		if (position.x >= vector3i.x && position.x < vector3i2.x && position.y >= vector3i.y && position.y < vector3i2.y && position.z >= vector3i.z && position.z < vector3i2.z)
		{
			int num = 0;
			DynamicPrefabDecorator dynamicPrefabDecorator = GameManager.Instance.World.ChunkCache.ChunkProvider.GetDynamicPrefabDecorator();
			if (dynamicPrefabDecorator == null)
			{
				return;
			}
			PrefabInstance prefabFromWorldPosInside = dynamicPrefabDecorator.GetPrefabFromWorldPosInside(position.x, position.z);
			if (prefabFromWorldPosInside == null)
			{
				Log.Error("No prefab found for SleeperBlock position!\n" + StackTraceUtility.ExtractStackTrace());
				return;
			}
			Vector3i vector3i3 = position - prefabFromWorldPosInside.boundingBoxPosition;
			PrefabSleeperVolume prefabSleeperVolume = prefabFromWorldPosInside.prefab.FindSleeperVolume(vector3i3);
			if (prefabSleeperVolume != null && prefabSleeperVolume.isPriority)
			{
				num = 1;
			}
			if (_data.block.spawnMode == BlockSleeper.eMode.Bandit)
			{
				num = 4;
			}
			if (_data.block.spawnMode == BlockSleeper.eMode.Infested)
			{
				num = 5;
			}
			prefabT.gameObject.SetActive(true);
			SleeperVolumeToolManager.SetMats(prefabT, SleeperVolumeToolManager.typeMats[num]);
			return;
		}
		else
		{
			if (!SleeperVolumeToolManager.InAnyVolume(position))
			{
				prefabT.gameObject.SetActive(true);
				SleeperVolumeToolManager.SetMats(prefabT, SleeperVolumeToolManager.typeMats[2]);
				return;
			}
			if (SleeperVolumeToolManager.xRayOn && _selectedBox == null)
			{
				prefabT.gameObject.SetActive(true);
				SleeperVolumeToolManager.SetMats(prefabT, SleeperVolumeToolManager.typeMats[3]);
				return;
			}
			prefabT.gameObject.SetActive(false);
			return;
		}
	}

	// Token: 0x06009CE7 RID: 40167 RVA: 0x003B4150 File Offset: 0x003B2350
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool InAnyVolume(Vector3i _pos)
	{
		Vector3i zero = Vector3i.zero;
		Vector3i zero2 = Vector3i.zero;
		foreach (SelectionBox selectionBox in SleeperVolumeToolManager.registeredSleeperVolumes)
		{
			zero.RoundToInt(selectionBox.bounds.min);
			zero2.RoundToInt(selectionBox.bounds.max);
			if (_pos.x >= zero.x && _pos.x < zero2.x && _pos.y >= zero.y && _pos.y < zero2.y && _pos.z >= zero.z && _pos.z < zero2.z)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06009CE8 RID: 40168 RVA: 0x003B4230 File Offset: 0x003B2430
	[PublicizedFrom(EAccessModifier.Private)]
	public static void SetMats(Transform _t, Material _mat)
	{
		int num = (SleeperVolumeToolManager.xRayOn ? (-200000000) : (-200000));
		_mat.SetInt("_Offset", num);
		MeshRenderer[] componentsInChildren = _t.GetComponentsInChildren<MeshRenderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].sharedMaterial = _mat;
		}
	}

	// Token: 0x17001294 RID: 4756
	// (get) Token: 0x06009CE9 RID: 40169 RVA: 0x003B427B File Offset: 0x003B247B
	// (set) Token: 0x06009CEA RID: 40170 RVA: 0x003B4284 File Offset: 0x003B2484
	public static bool XRayMode
	{
		get
		{
			return SleeperVolumeToolManager.xRayOn;
		}
		set
		{
			if (SleeperVolumeToolManager.xRayOn == value)
			{
				return;
			}
			SleeperVolumeToolManager.xRayOn = value;
			int num = (SleeperVolumeToolManager.xRayOn ? (-200000000) : (-200000));
			foreach (Material material in SleeperVolumeToolManager.typeMats)
			{
				material.SetInt("_Offset", num);
			}
			SleeperVolumeToolManager.UpdateAllSleeperBlockVisuals();
		}
	}

	// Token: 0x040076B5 RID: 30389
	[PublicizedFrom(EAccessModifier.Private)]
	public static GameObject GroupGameObject;

	// Token: 0x040076B6 RID: 30390
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<Vector3i, List<SleeperVolumeToolManager.BlockData>> sleepers = new Dictionary<Vector3i, List<SleeperVolumeToolManager.BlockData>>();

	// Token: 0x040076B7 RID: 30391
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<SelectionBox> registeredSleeperVolumes = new List<SelectionBox>();

	// Token: 0x040076B8 RID: 30392
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool xRayOn = true;

	// Token: 0x040076B9 RID: 30393
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cActiveIndex = 0;

	// Token: 0x040076BA RID: 30394
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cPriorityIndex = 1;

	// Token: 0x040076BB RID: 30395
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cNoVolumeIndex = 2;

	// Token: 0x040076BC RID: 30396
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cDarkIndex = 3;

	// Token: 0x040076BD RID: 30397
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cBanditIndex = 4;

	// Token: 0x040076BE RID: 30398
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cInfestedIndex = 5;

	// Token: 0x040076BF RID: 30399
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Color[] typeColors = new Color[]
	{
		new Color(1f, 0.6f, 0.1f),
		new Color(0.7f, 0.7f, 0.7f),
		new Color(1f, 0.1f, 1f),
		new Color(0.02f, 0.02f, 0.02f),
		new Color(0.1f, 1f, 0.1f),
		new Color(1f, 0.1f, 0.1f)
	};

	// Token: 0x040076C0 RID: 30400
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<Material> typeMats = new List<Material>();

	// Token: 0x040076C1 RID: 30401
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Color groupSelectedColor = new Color(0.9f, 0.9f, 1f, 0.4f);

	// Token: 0x040076C2 RID: 30402
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Color[] groupColors = new Color[]
	{
		new Color(1f, 0.2f, 0.2f, 0.4f),
		new Color(1f, 0.6f, 0.2f, 0.4f),
		new Color(1f, 1f, 0.2f, 0.4f),
		new Color(0.6f, 1f, 0.2f, 0.4f),
		new Color(0.2f, 1f, 0.2f, 0.4f),
		new Color(0.2f, 1f, 0.6f, 0.4f),
		new Color(0.2f, 1f, 1f, 0.4f),
		new Color(0.2f, 0.6f, 1f, 0.4f)
	};

	// Token: 0x040076C3 RID: 30403
	[PublicizedFrom(EAccessModifier.Private)]
	public static SelectionBox currentSelectionBox;

	// Token: 0x040076C4 RID: 30404
	[PublicizedFrom(EAccessModifier.Private)]
	public static SelectionBox previousSelectionBox;

	// Token: 0x0200136F RID: 4975
	[PublicizedFrom(EAccessModifier.Private)]
	public class BlockData
	{
		// Token: 0x040076C5 RID: 30405
		public BlockSleeper block;

		// Token: 0x040076C6 RID: 30406
		public Transform prefabT;

		// Token: 0x040076C7 RID: 30407
		public Vector3i position;
	}
}
