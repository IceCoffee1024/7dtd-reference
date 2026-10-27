using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace PrefabVolumes
{
	// Token: 0x02001915 RID: 6421
	public class PrefabMarkerVolumeList : PrefabVolumeListAbs<PrefabMarkerVolumeList, Marker>
	{
		// Token: 0x0600C5BF RID: 50623 RVA: 0x0048AA95 File Offset: 0x00488C95
		public PrefabMarkerVolumeList(Prefab _owner)
			: base(_owner)
		{
		}

		// Token: 0x170018EC RID: 6380
		// (get) Token: 0x0600C5C0 RID: 50624 RVA: 0x00080BF2 File Offset: 0x0007EDF2
		public override PrefabVolumeAbs.EVolumeType VolumeType
		{
			get
			{
				return PrefabVolumeAbs.EVolumeType.Marker;
			}
		}

		// Token: 0x170018ED RID: 6381
		// (get) Token: 0x0600C5C1 RID: 50625 RVA: 0x0048AA9E File Offset: 0x00488C9E
		public override SelectionCategory SelectionCategory
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return SelectionBoxManager.Instance.CategoryPOIMarker;
			}
		}

		// Token: 0x0600C5C2 RID: 50626 RVA: 0x0048AAAC File Offset: 0x00488CAC
		public override void ReadFromProperties(DynamicProperties _properties)
		{
			this.List.Clear();
			Dictionary<string, string> values = _properties.Values;
			if (!values.ContainsKey("POIMarkerSize") || !values.ContainsKey("POIMarkerStart"))
			{
				return;
			}
			List<Vector3i> list = StringParsers.ParseList<Vector3i>(values["POIMarkerSize"], '#', (string _s, int _start, int _end) => StringParsers.ParseVector3i(_s, _start, _end, false));
			List<Vector3i> list2 = StringParsers.ParseList<Vector3i>(values["POIMarkerStart"], '#', (string _s, int _start, int _end) => StringParsers.ParseVector3i(_s, _start, _end, false));
			List<Marker.MarkerTypes> list3 = new List<Marker.MarkerTypes>();
			if (values.ContainsKey("POIMarkerType"))
			{
				string[] array = values["POIMarkerType"].Split(',', StringSplitOptions.None);
				for (int i = 0; i < array.Length; i++)
				{
					Marker.MarkerTypes markerTypes;
					if (EnumUtils.TryParse<Marker.MarkerTypes>(array[i], out markerTypes, true))
					{
						list3.Add(markerTypes);
					}
				}
			}
			List<FastTags<TagGroup.Poi>> list4 = new List<FastTags<TagGroup.Poi>>();
			if (values.ContainsKey("POIMarkerTags"))
			{
				foreach (string text in values["POIMarkerTags"].Split('#', StringSplitOptions.None))
				{
					list4.Add((text.Length > 0) ? FastTags<TagGroup.Poi>.Parse(text) : FastTags<TagGroup.Poi>.none);
				}
			}
			List<string> list5 = new List<string>();
			if (values.ContainsKey("POIMarkerGroup"))
			{
				list5.AddRange(values["POIMarkerGroup"].Split(',', StringSplitOptions.None));
			}
			List<string> list6 = new List<string>();
			if (values.ContainsKey("POIMarkerPartToSpawn"))
			{
				list6.AddRange(values["POIMarkerPartToSpawn"].Split(',', StringSplitOptions.None));
			}
			List<int> list7 = new List<int>();
			if (values.ContainsKey("POIMarkerPartRotations"))
			{
				string[] array = values["POIMarkerPartRotations"].Split(',', StringSplitOptions.None);
				for (int i = 0; i < array.Length; i++)
				{
					int num;
					StringParsers.TryParseSInt32(array[i], out num);
					list7.Add(num);
				}
			}
			List<float> list8 = new List<float>();
			if (values.ContainsKey("POIMarkerPartSpawnChance"))
			{
				string[] array = values["POIMarkerPartSpawnChance"].Split(',', StringSplitOptions.None);
				for (int i = 0; i < array.Length; i++)
				{
					float num2;
					if (StringParsers.TryParseFloat(array[i], out num2))
					{
						list8.Add(num2);
					}
					else
					{
						list8.Add(0f);
					}
				}
			}
			for (int j = 0; j < list2.Count; j++)
			{
				Marker marker = new Marker();
				marker.Use(list2[j], (j < list.Count) ? list[j] : default(Vector3i));
				if (j < list3.Count)
				{
					marker.MarkerType = list3[j];
				}
				if (j < list5.Count)
				{
					marker.GroupName = list5[j];
				}
				if (j < list4.Count)
				{
					marker.Tags = list4[j];
				}
				if (j < list6.Count)
				{
					marker.PartToSpawn = list6[j];
				}
				if (j < list7.Count)
				{
					marker.Rotations = (byte)list7[j];
				}
				if (j < list8.Count)
				{
					marker.PartChanceToSpawn = list8[j];
				}
				this.List.Add(marker);
			}
		}

		// Token: 0x0600C5C3 RID: 50627 RVA: 0x0048AE0C File Offset: 0x0048900C
		public override void WriteToProperties(DynamicProperties _properties)
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			StringBuilder stringBuilder3 = new StringBuilder();
			StringBuilder stringBuilder4 = new StringBuilder();
			StringBuilder stringBuilder5 = new StringBuilder();
			StringBuilder stringBuilder6 = new StringBuilder();
			StringBuilder stringBuilder7 = new StringBuilder();
			StringBuilder stringBuilder8 = new StringBuilder();
			foreach (Marker marker in this.List)
			{
				if (marker.Used)
				{
					if (stringBuilder2.Length > 0)
					{
						stringBuilder.Append('#');
						stringBuilder2.Append('#');
						stringBuilder3.Append(',');
						stringBuilder4.Append('#');
						stringBuilder5.Append(',');
						stringBuilder6.Append(',');
						stringBuilder7.Append(',');
						stringBuilder8.Append(',');
					}
					stringBuilder.Append(marker.size.ToString());
					stringBuilder2.Append(marker.startPos.ToString());
					stringBuilder3.Append(marker.GroupName);
					stringBuilder4.Append(marker.Tags.ToString());
					stringBuilder5.Append(marker.MarkerType.ToString());
					stringBuilder6.Append(marker.PartToSpawn);
					stringBuilder7.Append(marker.Rotations.ToString());
					stringBuilder8.Append(marker.PartChanceToSpawn.ToString(CultureInfo.InvariantCulture));
				}
			}
			if (stringBuilder.Length > 0)
			{
				_properties.Values["POIMarkerSize"] = stringBuilder.ToString();
				_properties.Values["POIMarkerStart"] = stringBuilder2.ToString();
				_properties.Values["POIMarkerGroup"] = stringBuilder3.ToString();
				_properties.Values["POIMarkerTags"] = stringBuilder4.ToString();
				_properties.Values["POIMarkerType"] = stringBuilder5.ToString();
				_properties.Values["POIMarkerPartToSpawn"] = stringBuilder6.ToString();
				_properties.Values["POIMarkerPartRotations"] = stringBuilder7.ToString();
				_properties.Values["POIMarkerPartSpawnChance"] = stringBuilder8.ToString();
				return;
			}
			_properties.Values.Remove("POIMarkerSize");
			_properties.Values.Remove("POIMarkerStart");
			_properties.Values.Remove("POIMarkerGroup");
			_properties.Values.Remove("POIMarkerTags");
			_properties.Values.Remove("POIMarkerType");
			_properties.Values.Remove("POIMarkerPartToSpawn");
			_properties.Values.Remove("POIMarkerPartRotations");
			_properties.Values.Remove("POIMarkerPartSpawnChance");
		}

		// Token: 0x0600C5C4 RID: 50628 RVA: 0x0048B114 File Offset: 0x00489314
		[return: TupleElementNames(new string[] { "volumeIndex", "volume", "box" })]
		public override ValueTuple<int, PrefabVolumeAbs, SelectionBox> AddNewVolume(string _prefabInstanceName, Vector3i _bbPos, Vector3i _startPos, Vector3i _size)
		{
			ValueTuple<Marker, int, string> valueTuple = base.PrepareNewEntry(_prefabInstanceName, _startPos, _size);
			Marker item = valueTuple.Item1;
			int item2 = valueTuple.Item2;
			string item3 = valueTuple.Item3;
			SelectionBox selectionBox = this.AddSelectionBox(item, item3, _bbPos + _startPos);
			SelectionBoxManager.Instance.SetActive(selectionBox, true);
			return new ValueTuple<int, PrefabVolumeAbs, SelectionBox>(item2, item, selectionBox);
		}

		// Token: 0x0600C5C5 RID: 50629 RVA: 0x0048B164 File Offset: 0x00489364
		public override void SetVolume(PrefabInstance _prefabInstance, int _index, Marker _volumeSettings)
		{
			base.SetVolume(_prefabInstance, _index, _volumeSettings);
			if (!_volumeSettings.Used)
			{
				return;
			}
			string text = _prefabInstance.name + "_" + _index.ToString();
			SelectionBox selectionBox;
			this.SelectionCategory.TryGetBox(text, out selectionBox);
			SelectionBox selectionBox2 = selectionBox;
			int num;
			switch (_volumeSettings.Rotations)
			{
			case 1:
				num = ((_volumeSettings.MarkerType == Marker.MarkerTypes.PartSpawn) ? 90 : 270);
				break;
			case 2:
				num = 180;
				break;
			case 3:
				num = ((_volumeSettings.MarkerType == Marker.MarkerTypes.PartSpawn) ? 270 : 90);
				break;
			default:
				num = 0;
				break;
			}
			selectionBox2.FacingDirection = (float)num;
		}

		// Token: 0x0600C5C6 RID: 50630 RVA: 0x0048B208 File Offset: 0x00489408
		public override SelectionBox AddSelectionBox(Marker _volume, string _name, Vector3i _pos)
		{
			SelectionBox selectionBox = base.AddSelectionBox(_volume, _name, _pos);
			selectionBox.bDrawDirection = true;
			selectionBox.bAlwaysDrawDirection = true;
			SelectionBox selectionBox2 = selectionBox;
			int num;
			switch (_volume.Rotations)
			{
			case 1:
				num = ((_volume.MarkerType == Marker.MarkerTypes.PartSpawn) ? 90 : 270);
				break;
			case 2:
				num = 180;
				break;
			case 3:
				num = ((_volume.MarkerType == Marker.MarkerTypes.PartSpawn) ? 270 : 90);
				break;
			default:
				num = 0;
				break;
			}
			selectionBox2.FacingDirection = (float)num;
			selectionBox.Category.SetVisible(true);
			return selectionBox;
		}
	}
}
