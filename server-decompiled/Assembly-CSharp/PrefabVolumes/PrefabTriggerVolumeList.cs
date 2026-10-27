using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace PrefabVolumes
{
	// Token: 0x02001913 RID: 6419
	public class PrefabTriggerVolumeList : PrefabVolumeListAbs<PrefabTriggerVolumeList, PrefabTriggerVolume>
	{
		// Token: 0x0600C5B0 RID: 50608 RVA: 0x0048A6ED File Offset: 0x004888ED
		public PrefabTriggerVolumeList(Prefab _owner)
			: base(_owner)
		{
		}

		// Token: 0x170018EA RID: 6378
		// (get) Token: 0x0600C5B1 RID: 50609 RVA: 0x000809D4 File Offset: 0x0007EBD4
		public override PrefabVolumeAbs.EVolumeType VolumeType
		{
			get
			{
				return PrefabVolumeAbs.EVolumeType.Trigger;
			}
		}

		// Token: 0x170018EB RID: 6379
		// (get) Token: 0x0600C5B2 RID: 50610 RVA: 0x0048A6F6 File Offset: 0x004888F6
		public override SelectionCategory SelectionCategory
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return SelectionBoxManager.Instance.CategoryTriggerVolume;
			}
		}

		// Token: 0x0600C5B3 RID: 50611 RVA: 0x0048A704 File Offset: 0x00488904
		public override void ReadFromProperties(DynamicProperties _properties)
		{
			this.List.Clear();
			Dictionary<string, string> values = _properties.Values;
			if (!values.ContainsKey("TriggerVolumeSize") || !values.ContainsKey("TriggerVolumeStart"))
			{
				return;
			}
			List<Vector3i> list = StringParsers.ParseList<Vector3i>(values["TriggerVolumeSize"], '#', (string _s, int _start, int _end) => StringParsers.ParseVector3i(_s, _start, _end, false));
			List<Vector3i> list2 = StringParsers.ParseList<Vector3i>(values["TriggerVolumeStart"], '#', (string _s, int _start, int _end) => StringParsers.ParseVector3i(_s, _start, _end, false));
			List<string> list3 = StringParsers.ParseList<string>(values["TriggerVolumeTriggers"], '#', (string _s, int _start, int _end) => _s.Substring(_start, (_end == -1) ? (_s.Length - _start) : (_end + 1 - _start)));
			for (int i = 0; i < list2.Count; i++)
			{
				Vector3i vector3i = list2[i];
				Vector3i vector3i2 = ((i < list.Count) ? list[i] : Vector3i.one);
				PrefabTriggerVolume prefabTriggerVolume = new PrefabTriggerVolume();
				prefabTriggerVolume.Use(vector3i, vector3i2);
				if (list3[i].Trim() != "")
				{
					prefabTriggerVolume.TriggersIndices.AddRange(StringParsers.ParseList<byte>(list3[i], ',', (string _s, int _start, int _end) => StringParsers.ParseUInt8(_s, _start, _end, NumberStyles.Integer)));
				}
				this.List.Add(prefabTriggerVolume);
				this.Owner.HandleAddingTriggerLayers(prefabTriggerVolume);
			}
		}

		// Token: 0x0600C5B4 RID: 50612 RVA: 0x0048A894 File Offset: 0x00488A94
		public override void WriteToProperties(DynamicProperties _properties)
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			StringBuilder stringBuilder3 = new StringBuilder();
			foreach (PrefabTriggerVolume prefabTriggerVolume in this.List)
			{
				if (prefabTriggerVolume.Used)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append('#');
						stringBuilder2.Append('#');
						stringBuilder3.Append('#');
					}
					for (int i = 0; i < prefabTriggerVolume.TriggersIndices.Count; i++)
					{
						if (i > 0)
						{
							stringBuilder3.Append(',');
						}
						stringBuilder3.Append(prefabTriggerVolume.TriggersIndices[i].ToString());
					}
					if (prefabTriggerVolume.TriggersIndices.Count == 0)
					{
						stringBuilder3.Append(" ");
					}
					stringBuilder.Append(prefabTriggerVolume.size.ToString());
					stringBuilder2.Append(prefabTriggerVolume.startPos.ToString());
				}
			}
			if (stringBuilder.Length > 0)
			{
				_properties.Values["TriggerVolumeSize"] = stringBuilder.ToString();
				_properties.Values["TriggerVolumeStart"] = stringBuilder2.ToString();
				_properties.Values["TriggerVolumeTriggers"] = stringBuilder3.ToString();
				return;
			}
			_properties.Values.Remove("TriggerVolumeSize");
			_properties.Values.Remove("TriggerVolumeStart");
			_properties.Values.Remove("TriggerVolumeTriggers");
		}

		// Token: 0x0600C5B5 RID: 50613 RVA: 0x0048AA44 File Offset: 0x00488C44
		[PublicizedFrom(EAccessModifier.Protected)]
		public override int FindWorldVolume(World _world, Vector3i _min, Vector3i _max)
		{
			return _world.FindTriggerVolume(_min, _max);
		}

		// Token: 0x0600C5B6 RID: 50614 RVA: 0x0048AA50 File Offset: 0x00488C50
		[PublicizedFrom(EAccessModifier.Protected)]
		public override int CreateWorldVolume(int _prefabVolumeIndex, PrefabTriggerVolume _prefabVolume, Vector3i _offset, Vector3i _volumeMin, Vector3i _volumeMax, World _world, Vector3i _volumeWorldMin, Vector3i _volumeWorldMax)
		{
			TriggerVolume triggerVolume = TriggerVolume.Create(_prefabVolume, _volumeWorldMin, _volumeWorldMax);
			return _world.AddTriggerVolume(triggerVolume);
		}

		// Token: 0x0600C5B7 RID: 50615 RVA: 0x0048AA70 File Offset: 0x00488C70
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void AddWorldVolume(Chunk _chunk, int _volumeIndex)
		{
			_chunk.AddTriggerVolumeId(_volumeIndex);
		}

		// Token: 0x0600C5B8 RID: 50616 RVA: 0x0048AA79 File Offset: 0x00488C79
		public override void CopyVolumesIntoWorld(World _world, Chunk _chunk, Vector3i _offset)
		{
			base.CopyVolumesIntoWorldCommon(_world, _chunk, _offset, SleeperVolume.chunkPadding);
		}
	}
}
