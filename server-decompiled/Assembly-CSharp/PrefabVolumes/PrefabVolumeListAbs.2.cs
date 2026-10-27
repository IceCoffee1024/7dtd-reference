using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace PrefabVolumes
{
	// Token: 0x02001909 RID: 6409
	public abstract class PrefabVolumeListAbs<TVolumeList, TVolume> : PrefabVolumeListAbs where TVolumeList : PrefabVolumeListAbs<TVolumeList, TVolume> where TVolume : PrefabVolumeAbs<TVolume>, new()
	{
		// Token: 0x170018DE RID: 6366
		// (get) Token: 0x0600C55E RID: 50526
		public abstract SelectionCategory SelectionCategory
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get;
		}

		// Token: 0x170018DF RID: 6367
		// (get) Token: 0x0600C55F RID: 50527 RVA: 0x00488CE8 File Offset: 0x00486EE8
		public override bool AnyUsedEntry
		{
			get
			{
				if (this.List.Count > 0)
				{
					return this.List.FindIndex((TVolume _volume) => _volume.Used) >= 0;
				}
				return false;
			}
		}

		// Token: 0x170018E0 RID: 6368
		// (get) Token: 0x0600C560 RID: 50528 RVA: 0x00488D35 File Offset: 0x00486F35
		public override int Count
		{
			get
			{
				return this.List.Count;
			}
		}

		// Token: 0x170018E1 RID: 6369
		public TVolume this[int _index]
		{
			get
			{
				return this.List[_index];
			}
		}

		// Token: 0x0600C562 RID: 50530 RVA: 0x00488D50 File Offset: 0x00486F50
		public override PrefabVolumeAbs Get(int _index)
		{
			return this[_index];
		}

		// Token: 0x0600C563 RID: 50531 RVA: 0x00488D5E File Offset: 0x00486F5E
		[PublicizedFrom(EAccessModifier.Protected)]
		public PrefabVolumeListAbs(Prefab _owner)
		{
			this.Owner = _owner;
		}

		// Token: 0x0600C564 RID: 50532 RVA: 0x00488D78 File Offset: 0x00486F78
		public override void CopyFrom(PrefabVolumeListAbs _other)
		{
			TVolumeList tvolumeList = _other as TVolumeList;
			if (tvolumeList == null)
			{
				Log.Error(string.Concat(new string[]
				{
					"PrefabVolumeList.CopyFrom: Other list (",
					_other.GetType().Name,
					") is not the same type as the current instance (",
					typeof(TVolumeList).Name,
					")"
				}));
				return;
			}
			for (int i = 0; i < tvolumeList.List.Count; i++)
			{
				this.List.Add(tvolumeList.List[i].CloneGeneric());
			}
		}

		// Token: 0x0600C565 RID: 50533 RVA: 0x00488E24 File Offset: 0x00487024
		public override void SendAllVolumesToClient(ClientInfo _clientInfo, int _prefabInstanceId)
		{
			for (int i = 0; i < this.List.Count; i++)
			{
				_clientInfo.SendPackage(NetPackageManager.GetPackage<NetPackageEditorUpdateVolume>().Setup(NetPackageEditorUpdateVolume.EChangeType.Added, _prefabInstanceId, i, this.List[i]));
			}
		}

		// Token: 0x0600C566 RID: 50534 RVA: 0x00488E6C File Offset: 0x0048706C
		public override void Move(Vector3i _moveDistance)
		{
			foreach (TVolume tvolume in this.List)
			{
				tvolume.Move(_moveDistance);
			}
		}

		// Token: 0x0600C567 RID: 50535 RVA: 0x00488EC4 File Offset: 0x004870C4
		public override void RotateY(bool _bLeft, Vector3i _prefabSize)
		{
			foreach (TVolume tvolume in this.List)
			{
				tvolume.RotateY(_bLeft, _prefabSize);
			}
		}

		// Token: 0x0600C568 RID: 50536 RVA: 0x00488F1C File Offset: 0x0048711C
		[PublicizedFrom(EAccessModifier.Protected)]
		[return: TupleElementNames(new string[] { "volume", "index", "name" })]
		public ValueTuple<TVolume, int, string> PrepareNewEntry(string _prefabInstanceName, Vector3i _startPos, Vector3i _size)
		{
			int num = -1;
			TVolume tvolume = default(TVolume);
			for (int i = 0; i < this.List.Count; i++)
			{
				if (!this.List[i].Used)
				{
					num = i;
					tvolume = this.List[i];
					break;
				}
			}
			if (tvolume == null)
			{
				tvolume = new TVolume();
				num = this.List.Count;
				this.List.Add(tvolume);
			}
			tvolume.Reset();
			tvolume.Use(_startPos, _size);
			return new ValueTuple<TVolume, int, string>(tvolume, num, string.Format("{0}_{1}", _prefabInstanceName, num));
		}

		// Token: 0x0600C569 RID: 50537 RVA: 0x00020119 File Offset: 0x0001E319
		public override bool CanCreateVolume(string _prefabInstanceName, Vector3i _bbPos, Vector3i _startPos, Vector3i _size)
		{
			return true;
		}

		// Token: 0x0600C56A RID: 50538 RVA: 0x00488FC8 File Offset: 0x004871C8
		[return: TupleElementNames(new string[] { "volumeIndex", "volume", "box" })]
		public override ValueTuple<int, PrefabVolumeAbs, SelectionBox> AddNewVolume(string _prefabInstanceName, Vector3i _bbPos, Vector3i _startPos, Vector3i _size)
		{
			ValueTuple<TVolume, int, string> valueTuple = this.PrepareNewEntry(_prefabInstanceName, _startPos, _size);
			TVolume item = valueTuple.Item1;
			int item2 = valueTuple.Item2;
			string item3 = valueTuple.Item3;
			SelectionBox selectionBox = this.AddSelectionBox(item, item3, _bbPos + _startPos);
			SelectionBoxManager.Instance.SetActive(selectionBox, true);
			return new ValueTuple<int, PrefabVolumeAbs, SelectionBox>(item2, item, selectionBox);
		}

		// Token: 0x0600C56B RID: 50539 RVA: 0x0048901C File Offset: 0x0048721C
		[return: TupleElementNames(new string[] { "volumeIndex", "volume", "box" })]
		public override ValueTuple<int, PrefabVolumeAbs, SelectionBox> CloneVolume(string _prefabInstanceName, Vector3i _bbPos, int _existingIndex, Vector3i _offset)
		{
			PrefabVolumeAbs prefabVolumeAbs = this.List[_existingIndex];
			ValueTuple<int, PrefabVolumeAbs, SelectionBox> valueTuple = this.AddNewVolume(_prefabInstanceName, _bbPos, prefabVolumeAbs.startPos + _offset, prefabVolumeAbs.size);
			int item = valueTuple.Item1;
			PrefabVolumeAbs item2 = valueTuple.Item2;
			SelectionBox item3 = valueTuple.Item3;
			prefabVolumeAbs.CopyValues(item2, true);
			item3.UserData = item2;
			return new ValueTuple<int, PrefabVolumeAbs, SelectionBox>(item, item2, item3);
		}

		// Token: 0x0600C56C RID: 50540 RVA: 0x00489084 File Offset: 0x00487284
		public override void SetVolume(PrefabInstance _prefabInstance, int _index, PrefabVolumeAbs _volumeSettings)
		{
			TVolume tvolume = _volumeSettings as TVolume;
			if (tvolume == null)
			{
				throw new ArgumentException("Can not add volume of type " + _volumeSettings.VolumeType.ToStringCached<PrefabVolumeAbs.EVolumeType>() + " to list of type " + this.VolumeType.ToStringCached<PrefabVolumeAbs.EVolumeType>(), "_volumeSettings");
			}
			this.SetVolume(_prefabInstance, _index, tvolume);
		}

		// Token: 0x0600C56D RID: 50541 RVA: 0x004890E0 File Offset: 0x004872E0
		public virtual void SetVolume(PrefabInstance _prefabInstance, int _index, TVolume _volumeSettings)
		{
			while (_index >= this.List.Count)
			{
				this.List.Add(new TVolume());
			}
			bool used = this.List[_index].Used;
			this.List[_index] = _volumeSettings;
			string text = _prefabInstance.name + "_" + _index.ToString();
			if (!_volumeSettings.Used)
			{
				if (used)
				{
					this.SelectionCategory.RemoveBox(text);
				}
				return;
			}
			if (!used)
			{
				SelectionBox selectionBox = this.AddSelectionBox(_volumeSettings, text, _prefabInstance.boundingBoxPosition + _volumeSettings.startPos);
				SelectionBoxManager.Instance.SetActive(selectionBox, true);
				return;
			}
			SelectionBox selectionBox2;
			this.SelectionCategory.TryGetBox(text, out selectionBox2);
			selectionBox2.SetPositionAndSize(_prefabInstance.boundingBoxPosition + _volumeSettings.startPos, _volumeSettings.size);
			selectionBox2.UserData = _volumeSettings;
		}

		// Token: 0x0600C56E RID: 50542 RVA: 0x004891D7 File Offset: 0x004873D7
		public virtual SelectionBox AddSelectionBox(TVolume _volume, string _name, Vector3i _pos)
		{
			SelectionBox selectionBox = this.SelectionCategory.AddBox(_name, _pos, _volume.size, false, false);
			selectionBox.UserData = _volume;
			return selectionBox;
		}

		// Token: 0x0600C56F RID: 50543 RVA: 0x00489200 File Offset: 0x00487400
		public override void CreateSelectionBoxes(PrefabInstance _prefabInstance)
		{
			for (int i = 0; i < this.List.Count; i++)
			{
				TVolume tvolume = this.List[i];
				this.AddSelectionBox(tvolume, _prefabInstance.name + "_" + i.ToString(), _prefabInstance.boundingBoxPosition + tvolume.startPos);
			}
		}

		// Token: 0x0600C570 RID: 50544 RVA: 0x00489268 File Offset: 0x00487468
		public override void ApplyVolumesToSelectionBoxes(PrefabInstance _prefabInstance)
		{
			for (int i = 0; i < this.List.Count; i++)
			{
				TVolume tvolume = this.List[i];
				if (tvolume.Used)
				{
					SelectionBox box = this.SelectionCategory.GetBox(_prefabInstance.name + "_" + i.ToString());
					if (box != null)
					{
						box.SetPositionAndSize(_prefabInstance.boundingBoxPosition + tvolume.startPos, tvolume.size);
					}
				}
			}
		}

		// Token: 0x0600C571 RID: 50545 RVA: 0x004892F8 File Offset: 0x004874F8
		public override void RemoveSelectionBoxes(PrefabInstance _prefabInstance)
		{
			for (int i = 0; i < this.List.Count; i++)
			{
				if (this.List[i].Used)
				{
					this.SelectionCategory.RemoveBox(_prefabInstance.name + "_" + i.ToString());
				}
			}
		}

		// Token: 0x0600C572 RID: 50546 RVA: 0x00489358 File Offset: 0x00487558
		public override void RemoveVolumes(PrefabInstance _prefabInstance)
		{
			for (int i = 0; i < this.List.Count; i++)
			{
				TVolume tvolume = this.List[i];
				tvolume.MarkUnused();
				this.SetVolume(_prefabInstance, i, tvolume);
			}
		}

		// Token: 0x0600C573 RID: 50547 RVA: 0x00100DD1 File Offset: 0x000FEFD1
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual int FindWorldVolume(World _world, Vector3i _min, Vector3i _max)
		{
			return -1;
		}

		// Token: 0x0600C574 RID: 50548 RVA: 0x00100DD1 File Offset: 0x000FEFD1
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual int CreateWorldVolume(int _prefabVolumeIndex, TVolume _prefabVolume, Vector3i _offset, Vector3i _volumeMin, Vector3i _volumeMax, World _world, Vector3i _volumeWorldMin, Vector3i _volumeWorldMax)
		{
			return -1;
		}

		// Token: 0x0600C575 RID: 50549 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void AddWorldVolume(Chunk _chunk, int _volumeIndex)
		{
		}

		// Token: 0x0600C576 RID: 50550 RVA: 0x0048939C File Offset: 0x0048759C
		[PublicizedFrom(EAccessModifier.Protected)]
		public void CopyVolumesIntoWorldCommon(World _world, Chunk _chunk, Vector3i _offset, Vector3i _padding)
		{
			Vector3i vector3i = Vector3i.zero;
			Vector3i vector3i2 = Vector3i.zero;
			if (_chunk != null)
			{
				vector3i = _chunk.GetWorldPos();
				vector3i2 = vector3i + new Vector3i(16, 256, 16);
			}
			for (int i = 0; i < this.List.Count; i++)
			{
				TVolume tvolume = this.List[i];
				if (tvolume.Used && (World.SandboxUseTraderArea == TraderAreaStates.Default || (tvolume.VolumeType != PrefabVolumeAbs.EVolumeType.Teleport && tvolume.VolumeType != PrefabVolumeAbs.EVolumeType.Wall)))
				{
					Vector3i startPos = tvolume.startPos;
					Vector3i vector3i3 = startPos + tvolume.size;
					Vector3i vector3i4 = startPos + _offset;
					Vector3i vector3i5 = vector3i4 + tvolume.size;
					Vector3i vector3i6 = vector3i4 - _padding;
					Vector3i vector3i7 = vector3i5 + _padding;
					if (_chunk != null)
					{
						if (vector3i6.x < vector3i2.x && vector3i7.x > vector3i.x && vector3i6.y < vector3i2.y && vector3i7.y > vector3i.y && vector3i6.z < vector3i2.z && vector3i7.z > vector3i.z)
						{
							int num = this.FindWorldVolume(_world, vector3i4, vector3i5);
							if (num < 0)
							{
								num = this.CreateWorldVolume(i, tvolume, _offset, startPos, vector3i3, _world, vector3i4, vector3i5);
							}
							this.AddWorldVolume(_chunk, num);
						}
					}
					else
					{
						int num2 = this.FindWorldVolume(_world, vector3i4, vector3i5);
						if (num2 < 0)
						{
							num2 = this.CreateWorldVolume(i, tvolume, _offset, startPos, vector3i3, _world, vector3i4, vector3i5);
						}
						int num3 = World.toChunkXZ(vector3i6.x);
						int num4 = World.toChunkXZ(vector3i7.x - 1);
						int num5 = World.toChunkXZ(vector3i6.z);
						int num6 = World.toChunkXZ(vector3i7.z - 1);
						for (int j = num3; j <= num4; j++)
						{
							for (int k = num5; k <= num6; k++)
							{
								Chunk chunk = (Chunk)_world.GetChunkSync(j, k);
								if (chunk != null)
								{
									this.AddWorldVolume(chunk, num2);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600C577 RID: 50551 RVA: 0x000027FC File Offset: 0x000009FC
		public override void CopyVolumesIntoWorld(World _world, Chunk _chunk, Vector3i _offset)
		{
		}

		// Token: 0x0400957F RID: 38271
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly Prefab Owner;

		// Token: 0x04009580 RID: 38272
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly List<TVolume> List = new List<TVolume>();
	}
}
