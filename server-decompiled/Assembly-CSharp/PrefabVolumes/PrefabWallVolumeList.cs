using System;
using System.Collections.Generic;
using System.Text;

namespace PrefabVolumes
{
	// Token: 0x020018E0 RID: 6368
	public class PrefabWallVolumeList : PrefabVolumeListAbs<PrefabWallVolumeList, PrefabWallVolume>
	{
		// Token: 0x0600C48F RID: 50319 RVA: 0x00486ADC File Offset: 0x00484CDC
		public PrefabWallVolumeList(Prefab _owner)
			: base(_owner)
		{
		}

		// Token: 0x170018A0 RID: 6304
		// (get) Token: 0x0600C490 RID: 50320 RVA: 0x000470B6 File Offset: 0x000452B6
		public override PrefabVolumeAbs.EVolumeType VolumeType
		{
			get
			{
				return PrefabVolumeAbs.EVolumeType.Wall;
			}
		}

		// Token: 0x170018A1 RID: 6305
		// (get) Token: 0x0600C491 RID: 50321 RVA: 0x00486AE5 File Offset: 0x00484CE5
		public override SelectionCategory SelectionCategory
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return SelectionBoxManager.Instance.CategoryWallVolume;
			}
		}

		// Token: 0x0600C492 RID: 50322 RVA: 0x00486AF4 File Offset: 0x00484CF4
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

		// Token: 0x0600C493 RID: 50323 RVA: 0x00486BE4 File Offset: 0x00484DE4
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

		// Token: 0x0600C494 RID: 50324 RVA: 0x00486CF0 File Offset: 0x00484EF0
		[PublicizedFrom(EAccessModifier.Protected)]
		public override int FindWorldVolume(World _world, Vector3i _min, Vector3i _max)
		{
			return _world.FindWallVolume(_min, _max);
		}

		// Token: 0x0600C495 RID: 50325 RVA: 0x00486CFC File Offset: 0x00484EFC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override int CreateWorldVolume(int _prefabVolumeIndex, PrefabWallVolume _prefabVolume, Vector3i _offset, Vector3i _volumeMin, Vector3i _volumeMax, World _world, Vector3i _volumeWorldMin, Vector3i _volumeWorldMax)
		{
			WallVolume wallVolume = WallVolume.Create(_prefabVolume, _volumeWorldMin, _volumeWorldMax);
			return _world.AddWallVolume(wallVolume);
		}

		// Token: 0x0600C496 RID: 50326 RVA: 0x00486D1C File Offset: 0x00484F1C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void AddWorldVolume(Chunk _chunk, int _volumeIndex)
		{
			_chunk.AddWallVolumeId(_volumeIndex);
		}

		// Token: 0x0600C497 RID: 50327 RVA: 0x00486D25 File Offset: 0x00484F25
		public override void CopyVolumesIntoWorld(World _world, Chunk _chunk, Vector3i _offset)
		{
			base.CopyVolumesIntoWorldCommon(_world, _chunk, _offset, Vector3i.zero);
		}
	}
}
