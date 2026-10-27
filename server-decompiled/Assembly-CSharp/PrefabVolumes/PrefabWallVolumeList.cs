using System;
using System.Collections.Generic;
using System.Text;

namespace PrefabVolumes
{
	// Token: 0x02001911 RID: 6417
	public class PrefabWallVolumeList : PrefabVolumeListAbs<PrefabWallVolumeList, PrefabWallVolume>
	{
		// Token: 0x0600C5A3 RID: 50595 RVA: 0x0048A488 File Offset: 0x00488688
		public PrefabWallVolumeList(Prefab _owner)
			: base(_owner)
		{
		}

		// Token: 0x170018E8 RID: 6376
		// (get) Token: 0x0600C5A4 RID: 50596 RVA: 0x00047076 File Offset: 0x00045276
		public override PrefabVolumeAbs.EVolumeType VolumeType
		{
			get
			{
				return PrefabVolumeAbs.EVolumeType.Wall;
			}
		}

		// Token: 0x170018E9 RID: 6377
		// (get) Token: 0x0600C5A5 RID: 50597 RVA: 0x0048A491 File Offset: 0x00488691
		public override SelectionCategory SelectionCategory
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return SelectionBoxManager.Instance.CategoryWallVolume;
			}
		}

		// Token: 0x0600C5A6 RID: 50598 RVA: 0x0048A4A0 File Offset: 0x004886A0
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

		// Token: 0x0600C5A7 RID: 50599 RVA: 0x0048A590 File Offset: 0x00488790
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

		// Token: 0x0600C5A8 RID: 50600 RVA: 0x0048A69C File Offset: 0x0048889C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override int FindWorldVolume(World _world, Vector3i _min, Vector3i _max)
		{
			return _world.FindWallVolume(_min, _max);
		}

		// Token: 0x0600C5A9 RID: 50601 RVA: 0x0048A6A8 File Offset: 0x004888A8
		[PublicizedFrom(EAccessModifier.Protected)]
		public override int CreateWorldVolume(int _prefabVolumeIndex, PrefabWallVolume _prefabVolume, Vector3i _offset, Vector3i _volumeMin, Vector3i _volumeMax, World _world, Vector3i _volumeWorldMin, Vector3i _volumeWorldMax)
		{
			WallVolume wallVolume = WallVolume.Create(_prefabVolume, _volumeWorldMin, _volumeWorldMax);
			return _world.AddWallVolume(wallVolume);
		}

		// Token: 0x0600C5AA RID: 50602 RVA: 0x0048A6C8 File Offset: 0x004888C8
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void AddWorldVolume(Chunk _chunk, int _volumeIndex)
		{
			_chunk.AddWallVolumeId(_volumeIndex);
		}

		// Token: 0x0600C5AB RID: 50603 RVA: 0x0048A6D1 File Offset: 0x004888D1
		public override void CopyVolumesIntoWorld(World _world, Chunk _chunk, Vector3i _offset)
		{
			base.CopyVolumesIntoWorldCommon(_world, _chunk, _offset, Vector3i.zero);
		}
	}
}
