using System;
using System.Collections.Generic;
using PrefabVolumes;
using UnityEngine;

// Token: 0x02001354 RID: 4948
public static class SleeperVolumeToolManager
{
	// Token: 0x1700125A RID: 4698
	// (get) Token: 0x06009BDD RID: 39901 RVA: 0x003AE530 File Offset: 0x003AC730
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

	// Token: 0x06009BDE RID: 39902 RVA: 0x003AE568 File Offset: 0x003AC768
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

	// Token: 0x06009BDF RID: 39903 RVA: 0x003AE6E8 File Offset: 0x003AC8E8
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

	// Token: 0x06009BE0 RID: 39904 RVA: 0x003AE76C File Offset: 0x003AC96C
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

	// Token: 0x06009BE1 RID: 39905 RVA: 0x003AE830 File Offset: 0x003ACA30
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

	// Token: 0x06009BE2 RID: 39906 RVA: 0x003AE862 File Offset: 0x003ACA62
	public static void UnRegisterSleeperVolume(SelectionBox _selBox)
	{
		SleeperVolumeToolManager.registeredSleeperVolumes.Remove(_selBox);
	}

	// Token: 0x06009BE3 RID: 39907 RVA: 0x003AE870 File Offset: 0x003ACA70
	public static void ClearSleeperVolumes()
	{
		SleeperVolumeToolManager.registeredSleeperVolumes.Clear();
	}

	// Token: 0x06009BE4 RID: 39908 RVA: 0x003AE87C File Offset: 0x003ACA7C
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

	// Token: 0x06009BE5 RID: 39909 RVA: 0x003AE9F3 File Offset: 0x003ACBF3
	public static void SetVisible(bool _visible)
	{
		if (_visible)
		{
			SleeperVolumeToolManager.SelectionChanged(null);
			return;
		}
		SleeperVolumeToolManager.hideSleeperBlocks();
	}

	// Token: 0x06009BE6 RID: 39910 RVA: 0x003AEA04 File Offset: 0x003ACC04
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

	// Token: 0x06009BE7 RID: 39911 RVA: 0x003AEA3C File Offset: 0x003ACC3C
	public static void UpdateAllVisuals()
	{
		SleeperVolumeToolManager.UpdateAllSleeperBlockVisuals();
		SleeperVolumeToolManager.UpdateSleeperVolumeColors();
	}

	// Token: 0x06009BE8 RID: 39912 RVA: 0x003AEA48 File Offset: 0x003ACC48
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

	// Token: 0x06009BE9 RID: 39913 RVA: 0x003AEB3C File Offset: 0x003ACD3C
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

	// Token: 0x06009BEA RID: 39914 RVA: 0x003AEBF0 File Offset: 0x003ACDF0
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

	// Token: 0x06009BEB RID: 39915 RVA: 0x003AEC94 File Offset: 0x003ACE94
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

	// Token: 0x06009BEC RID: 39916 RVA: 0x003AEEA8 File Offset: 0x003AD0A8
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

	// Token: 0x06009BED RID: 39917 RVA: 0x003AEF88 File Offset: 0x003AD188
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

	// Token: 0x1700125B RID: 4699
	// (get) Token: 0x06009BEE RID: 39918 RVA: 0x003AEFD3 File Offset: 0x003AD1D3
	// (set) Token: 0x06009BEF RID: 39919 RVA: 0x003AEFDC File Offset: 0x003AD1DC
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

	// Token: 0x040075DD RID: 30173
	[PublicizedFrom(EAccessModifier.Private)]
	public static GameObject GroupGameObject;

	// Token: 0x040075DE RID: 30174
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<Vector3i, List<SleeperVolumeToolManager.BlockData>> sleepers = new Dictionary<Vector3i, List<SleeperVolumeToolManager.BlockData>>();

	// Token: 0x040075DF RID: 30175
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<SelectionBox> registeredSleeperVolumes = new List<SelectionBox>();

	// Token: 0x040075E0 RID: 30176
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool xRayOn = true;

	// Token: 0x040075E1 RID: 30177
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cActiveIndex = 0;

	// Token: 0x040075E2 RID: 30178
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cPriorityIndex = 1;

	// Token: 0x040075E3 RID: 30179
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cNoVolumeIndex = 2;

	// Token: 0x040075E4 RID: 30180
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cDarkIndex = 3;

	// Token: 0x040075E5 RID: 30181
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cBanditIndex = 4;

	// Token: 0x040075E6 RID: 30182
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cInfestedIndex = 5;

	// Token: 0x040075E7 RID: 30183
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

	// Token: 0x040075E8 RID: 30184
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<Material> typeMats = new List<Material>();

	// Token: 0x040075E9 RID: 30185
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Color groupSelectedColor = new Color(0.9f, 0.9f, 1f, 0.4f);

	// Token: 0x040075EA RID: 30186
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

	// Token: 0x040075EB RID: 30187
	[PublicizedFrom(EAccessModifier.Private)]
	public static SelectionBox currentSelectionBox;

	// Token: 0x040075EC RID: 30188
	[PublicizedFrom(EAccessModifier.Private)]
	public static SelectionBox previousSelectionBox;

	// Token: 0x02001355 RID: 4949
	[PublicizedFrom(EAccessModifier.Private)]
	public class BlockData
	{
		// Token: 0x040075ED RID: 30189
		public BlockSleeper block;

		// Token: 0x040075EE RID: 30190
		public Transform prefabT;

		// Token: 0x040075EF RID: 30191
		public Vector3i position;
	}
}
