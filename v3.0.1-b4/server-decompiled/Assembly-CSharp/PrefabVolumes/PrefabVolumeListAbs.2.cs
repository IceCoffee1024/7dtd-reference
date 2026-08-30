using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace PrefabVolumes
{
	// Token: 0x02001886 RID: 6278
	public abstract class PrefabVolumeListAbs<TVolumeList, TVolume> : PrefabVolumeListAbs where TVolumeList : PrefabVolumeListAbs<TVolumeList, TVolume> where TVolume : PrefabVolumeAbs<TVolume>, new()
	{
		// Token: 0x17001782 RID: 6018
		// (get) Token: 0x0600C16B RID: 49515
		public abstract SelectionCategory SelectionCategory
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get;
		}

		// Token: 0x17001783 RID: 6019
		// (get) Token: 0x0600C16C RID: 49516 RVA: 0x0047ACC8 File Offset: 0x00478EC8
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

		// Token: 0x17001784 RID: 6020
		// (get) Token: 0x0600C16D RID: 49517 RVA: 0x0047AD15 File Offset: 0x00478F15
		public override int Count
		{
			get
			{
				return this.List.Count;
			}
		}

		// Token: 0x17001785 RID: 6021
		public TVolume this[int _index]
		{
			get
			{
				return this.List[_index];
			}
		}

		// Token: 0x0600C16F RID: 49519 RVA: 0x0047AD30 File Offset: 0x00478F30
		public override PrefabVolumeAbs Get(int _index)
		{
			return this[_index];
		}

		// Token: 0x0600C170 RID: 49520 RVA: 0x0047AD3E File Offset: 0x00478F3E
		[PublicizedFrom(EAccessModifier.Protected)]
		public PrefabVolumeListAbs(Prefab _owner)
		{
			this.Owner = _owner;
		}

		// Token: 0x0600C171 RID: 49521 RVA: 0x0047AD58 File Offset: 0x00478F58
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

		// Token: 0x0600C172 RID: 49522 RVA: 0x0047AE04 File Offset: 0x00479004
		public override void SendAllVolumesToClient(ClientInfo _clientInfo, int _prefabInstanceId)
		{
			for (int i = 0; i < this.List.Count; i++)
			{
				_clientInfo.SendPackage(NetPackageManager.GetPackage<NetPackageEditorUpdateVolume>().Setup(NetPackageEditorUpdateVolume.EChangeType.Added, _prefabInstanceId, i, this.List[i]));
			}
		}

		// Token: 0x0600C173 RID: 49523 RVA: 0x0047AE4C File Offset: 0x0047904C
		public override void Move(Vector3i _moveDistance)
		{
			foreach (TVolume tvolume in this.List)
			{
				tvolume.Move(_moveDistance);
			}
		}

		// Token: 0x0600C174 RID: 49524 RVA: 0x0047AEA4 File Offset: 0x004790A4
		public override void RotateY(bool _bLeft, Vector3i _prefabSize)
		{
			foreach (TVolume tvolume in this.List)
			{
				tvolume.RotateY(_bLeft, _prefabSize);
			}
		}

		// Token: 0x0600C175 RID: 49525 RVA: 0x0047AEFC File Offset: 0x004790FC
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

		// Token: 0x0600C176 RID: 49526 RVA: 0x0001FF5D File Offset: 0x0001E15D
		public override bool CanCreateVolume(string _prefabInstanceName, Vector3i _bbPos, Vector3i _startPos, Vector3i _size)
		{
			return true;
		}

		// Token: 0x0600C177 RID: 49527 RVA: 0x0047AFA8 File Offset: 0x004791A8
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

		// Token: 0x0600C178 RID: 49528 RVA: 0x0047AFFC File Offset: 0x004791FC
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

		// Token: 0x0600C179 RID: 49529 RVA: 0x0047B064 File Offset: 0x00479264
		public override void SetVolume(PrefabInstance _prefabInstance, int _index, PrefabVolumeAbs _volumeSettings)
		{
			TVolume tvolume = _volumeSettings as TVolume;
			if (tvolume == null)
			{
				throw new ArgumentException("Can not add volume of type " + _volumeSettings.VolumeType.ToStringCached<PrefabVolumeAbs.EVolumeType>() + " to list of type " + this.VolumeType.ToStringCached<PrefabVolumeAbs.EVolumeType>(), "_volumeSettings");
			}
			this.SetVolume(_prefabInstance, _index, tvolume);
		}

		// Token: 0x0600C17A RID: 49530 RVA: 0x0047B0C0 File Offset: 0x004792C0
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

		// Token: 0x0600C17B RID: 49531 RVA: 0x0047B1B7 File Offset: 0x004793B7
		public virtual SelectionBox AddSelectionBox(TVolume _volume, string _name, Vector3i _pos)
		{
			SelectionBox selectionBox = this.SelectionCategory.AddBox(_name, _pos, _volume.size, false, false);
			selectionBox.UserData = _volume;
			return selectionBox;
		}

		// Token: 0x0600C17C RID: 49532 RVA: 0x0047B1E0 File Offset: 0x004793E0
		public override void CreateSelectionBoxes(PrefabInstance _prefabInstance)
		{
			for (int i = 0; i < this.List.Count; i++)
			{
				TVolume tvolume = this.List[i];
				this.AddSelectionBox(tvolume, _prefabInstance.name + "_" + i.ToString(), _prefabInstance.boundingBoxPosition + tvolume.startPos);
			}
		}

		// Token: 0x0600C17D RID: 49533 RVA: 0x0047B248 File Offset: 0x00479448
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

		// Token: 0x0600C17E RID: 49534 RVA: 0x0047B2D8 File Offset: 0x004794D8
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

		// Token: 0x0600C17F RID: 49535 RVA: 0x0047B338 File Offset: 0x00479538
		public override void RemoveVolumes(PrefabInstance _prefabInstance)
		{
			for (int i = 0; i < this.List.Count; i++)
			{
				TVolume tvolume = this.List[i];
				tvolume.MarkUnused();
				this.SetVolume(_prefabInstance, i, tvolume);
			}
		}

		// Token: 0x0600C180 RID: 49536 RVA: 0x000FEFA9 File Offset: 0x000FD1A9
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual int FindWorldVolume(World _world, Vector3i _min, Vector3i _max)
		{
			return -1;
		}

		// Token: 0x0600C181 RID: 49537 RVA: 0x000FEFA9 File Offset: 0x000FD1A9
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual int CreateWorldVolume(int _prefabVolumeIndex, TVolume _prefabVolume, Vector3i _offset, Vector3i _volumeMin, Vector3i _volumeMax, World _world, Vector3i _volumeWorldMin, Vector3i _volumeWorldMax)
		{
			return -1;
		}

		// Token: 0x0600C182 RID: 49538 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void AddWorldVolume(Chunk _chunk, int _volumeIndex)
		{
		}

		// Token: 0x0600C183 RID: 49539 RVA: 0x0047B37C File Offset: 0x0047957C
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

		// Token: 0x0600C184 RID: 49540 RVA: 0x000027FC File Offset: 0x000009FC
		public override void CopyVolumesIntoWorld(World _world, Chunk _chunk, Vector3i _offset)
		{
		}

		// Token: 0x040092A7 RID: 37543
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly Prefab Owner;

		// Token: 0x040092A8 RID: 37544
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly List<TVolume> List = new List<TVolume>();
	}
}
