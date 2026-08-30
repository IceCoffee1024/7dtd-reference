using System;
using System.Collections.Generic;
using System.Text;

namespace PrefabVolumes
{
	// Token: 0x020018B4 RID: 6324
	public class PrefabWallVolumeList : PrefabVolumeListAbs<PrefabWallVolumeList, PrefabWallVolume>
	{
		// Token: 0x0600C337 RID: 49975 RVA: 0x00482594 File Offset: 0x00480794
		public PrefabWallVolumeList(Prefab _owner)
			: base(_owner)
		{
		}

		// Token: 0x170017FE RID: 6142
		// (get) Token: 0x0600C338 RID: 49976 RVA: 0x00046EDA File Offset: 0x000450DA
		public override PrefabVolumeAbs.EVolumeType VolumeType
		{
			get
			{
				return PrefabVolumeAbs.EVolumeType.Wall;
			}
		}

		// Token: 0x170017FF RID: 6143
		// (get) Token: 0x0600C339 RID: 49977 RVA: 0x0048259D File Offset: 0x0048079D
		public override SelectionCategory SelectionCategory
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return SelectionBoxManager.Instance.CategoryWallVolume;
			}
		}

		// Token: 0x0600C33A RID: 49978 RVA: 0x004825AC File Offset: 0x004807AC
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

		// Token: 0x0600C33B RID: 49979 RVA: 0x0048269C File Offset: 0x0048089C
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

		// Token: 0x0600C33C RID: 49980 RVA: 0x004827A8 File Offset: 0x004809A8
		[PublicizedFrom(EAccessModifier.Protected)]
		public override int FindWorldVolume(World _world, Vector3i _min, Vector3i _max)
		{
			return _world.FindWallVolume(_min, _max);
		}

		// Token: 0x0600C33D RID: 49981 RVA: 0x004827B4 File Offset: 0x004809B4
		[PublicizedFrom(EAccessModifier.Protected)]
		public override int CreateWorldVolume(int _prefabVolumeIndex, PrefabWallVolume _prefabVolume, Vector3i _offset, Vector3i _volumeMin, Vector3i _volumeMax, World _world, Vector3i _volumeWorldMin, Vector3i _volumeWorldMax)
		{
			WallVolume wallVolume = WallVolume.Create(_prefabVolume, _volumeWorldMin, _volumeWorldMax);
			return _world.AddWallVolume(wallVolume);
		}

		// Token: 0x0600C33E RID: 49982 RVA: 0x004827D4 File Offset: 0x004809D4
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void AddWorldVolume(Chunk _chunk, int _volumeIndex)
		{
			_chunk.AddWallVolumeId(_volumeIndex);
		}

		// Token: 0x0600C33F RID: 49983 RVA: 0x004827DD File Offset: 0x004809DD
		public override void CopyVolumesIntoWorld(World _world, Chunk _chunk, Vector3i _offset)
		{
			base.CopyVolumesIntoWorldCommon(_world, _chunk, _offset, Vector3i.zero);
		}
	}
}
