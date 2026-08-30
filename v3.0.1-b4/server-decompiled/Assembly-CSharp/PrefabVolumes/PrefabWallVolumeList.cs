using System;
using System.Collections.Generic;
using System.Text;

namespace PrefabVolumes
{
	// Token: 0x0200188E RID: 6286
	public class PrefabWallVolumeList : PrefabVolumeListAbs<PrefabWallVolumeList, PrefabWallVolume>
	{
		// Token: 0x0600C1B0 RID: 49584 RVA: 0x0047C468 File Offset: 0x0047A668
		public PrefabWallVolumeList(Prefab _owner)
			: base(_owner)
		{
		}

		// Token: 0x1700178C RID: 6028
		// (get) Token: 0x0600C1B1 RID: 49585 RVA: 0x00046C7A File Offset: 0x00044E7A
		public override PrefabVolumeAbs.EVolumeType VolumeType
		{
			get
			{
				return PrefabVolumeAbs.EVolumeType.Wall;
			}
		}

		// Token: 0x1700178D RID: 6029
		// (get) Token: 0x0600C1B2 RID: 49586 RVA: 0x0047C471 File Offset: 0x0047A671
		public override SelectionCategory SelectionCategory
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return SelectionBoxManager.Instance.CategoryWallVolume;
			}
		}

		// Token: 0x0600C1B3 RID: 49587 RVA: 0x0047C480 File Offset: 0x0047A680
		public override void ReadFromProperties(DynamicProperties _properties)
		{
			this.List.Clear();
			Dictionary<string, string> values = _properties.Values;
			if (!values.ContainsKey("WallVolumeSize") || !values.ContainsKey("WallVolumeStart"))
			{
				return;
			}
			List<Vector3i> list = StringParsers.ParseList<Vector3i>(values["WallVolumeSize"], '#', (string _s, int _start, int _end) => StringParsers.ParseVector3i(_s, _start, _end, false));
			List<Vector3i> list2 = StringParsers.ParseList<Vector3i>(values["WallVolumeStart"], '#', (string _s, int _start, int _end) => StringParsers.ParseVector3i(_s, _start, _end, false));
			for (int i = 0; i < list2.Count; i++)
			{
				Vector3i vector3i = list2[i];
				Vector3i vector3i2 = ((i < list.Count) ? list[i] : Vector3i.one);
				PrefabWallVolume prefabWallVolume = new PrefabWallVolume();
				prefabWallVolume.Use(vector3i, vector3i2);
				this.List.Add(prefabWallVolume);
			}
		}

		// Token: 0x0600C1B4 RID: 49588 RVA: 0x0047C570 File Offset: 0x0047A770
		public override void WriteToProperties(DynamicProperties _properties)
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			foreach (PrefabWallVolume prefabWallVolume in this.List)
			{
				if (prefabWallVolume.Used)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append('#');
						stringBuilder2.Append('#');
					}
					stringBuilder.Append(prefabWallVolume.size.ToString());
					stringBuilder2.Append(prefabWallVolume.startPos.ToString());
				}
			}
			if (stringBuilder.Length > 0)
			{
				_properties.Values["WallVolumeSize"] = stringBuilder.ToString();
				_properties.Values["WallVolumeStart"] = stringBuilder2.ToString();
				return;
			}
			_properties.Values.Remove("WallVolumeSize");
			_properties.Values.Remove("WallVolumeStart");
		}

		// Token: 0x0600C1B5 RID: 49589 RVA: 0x0047C67C File Offset: 0x0047A87C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override int FindWorldVolume(World _world, Vector3i _min, Vector3i _max)
		{
			return _world.FindWallVolume(_min, _max);
		}

		// Token: 0x0600C1B6 RID: 49590 RVA: 0x0047C688 File Offset: 0x0047A888
		[PublicizedFrom(EAccessModifier.Protected)]
		public override int CreateWorldVolume(int _prefabVolumeIndex, PrefabWallVolume _prefabVolume, Vector3i _offset, Vector3i _volumeMin, Vector3i _volumeMax, World _world, Vector3i _volumeWorldMin, Vector3i _volumeWorldMax)
		{
			WallVolume wallVolume = WallVolume.Create(_prefabVolume, _volumeWorldMin, _volumeWorldMax);
			return _world.AddWallVolume(wallVolume);
		}

		// Token: 0x0600C1B7 RID: 49591 RVA: 0x0047C6A8 File Offset: 0x0047A8A8
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void AddWorldVolume(Chunk _chunk, int _volumeIndex)
		{
			_chunk.AddWallVolumeId(_volumeIndex);
		}

		// Token: 0x0600C1B8 RID: 49592 RVA: 0x0047C6B1 File Offset: 0x0047A8B1
		public override void CopyVolumesIntoWorld(World _world, Chunk _chunk, Vector3i _offset)
		{
			base.CopyVolumesIntoWorldCommon(_world, _chunk, _offset, Vector3i.zero);
		}
	}
}
