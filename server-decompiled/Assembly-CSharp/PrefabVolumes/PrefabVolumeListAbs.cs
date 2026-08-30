using System;
using System.Runtime.CompilerServices;

namespace PrefabVolumes
{
	// Token: 0x02001885 RID: 6277
	public abstract class PrefabVolumeListAbs
	{
		// Token: 0x1700177F RID: 6015
		// (get) Token: 0x0600C157 RID: 49495
		public abstract PrefabVolumeAbs.EVolumeType VolumeType { get; }

		// Token: 0x17001780 RID: 6016
		// (get) Token: 0x0600C158 RID: 49496
		public abstract bool AnyUsedEntry { get; }

		// Token: 0x17001781 RID: 6017
		// (get) Token: 0x0600C159 RID: 49497
		public abstract int Count { get; }

		// Token: 0x0600C15A RID: 49498
		public abstract PrefabVolumeAbs Get(int _index);

		// Token: 0x0600C15B RID: 49499
		public abstract void CopyFrom(PrefabVolumeListAbs _other);

		// Token: 0x0600C15C RID: 49500
		public abstract void ReadFromProperties(DynamicProperties _properties);

		// Token: 0x0600C15D RID: 49501
		public abstract void WriteToProperties(DynamicProperties _properties);

		// Token: 0x0600C15E RID: 49502
		public abstract void SendAllVolumesToClient(ClientInfo _clientInfo, int _prefabInstanceId);

		// Token: 0x0600C15F RID: 49503
		public abstract void Move(Vector3i _moveDistance);

		// Token: 0x0600C160 RID: 49504
		public abstract void RotateY(bool _bLeft, Vector3i _prefabSize);

		// Token: 0x0600C161 RID: 49505
		public abstract bool CanCreateVolume(string _prefabInstanceName, Vector3i _bbPos, Vector3i _startPos, Vector3i _size);

		// Token: 0x0600C162 RID: 49506
		[return: TupleElementNames(new string[] { "volumeIndex", "volume", "box" })]
		public abstract ValueTuple<int, PrefabVolumeAbs, SelectionBox> AddNewVolume(string _prefabInstanceName, Vector3i _bbPos, Vector3i _startPos, Vector3i _size);

		// Token: 0x0600C163 RID: 49507
		[return: TupleElementNames(new string[] { "volumeIndex", "volume", "box" })]
		public abstract ValueTuple<int, PrefabVolumeAbs, SelectionBox> CloneVolume(string _prefabInstanceName, Vector3i _bbPos, int _existingIndex, Vector3i _offset);

		// Token: 0x0600C164 RID: 49508
		public abstract void SetVolume(PrefabInstance _prefabInstance, int _index, PrefabVolumeAbs _volumeSettings);

		// Token: 0x0600C165 RID: 49509
		public abstract void CreateSelectionBoxes(PrefabInstance _prefabInstance);

		// Token: 0x0600C166 RID: 49510
		public abstract void ApplyVolumesToSelectionBoxes(PrefabInstance _prefabInstance);

		// Token: 0x0600C167 RID: 49511
		public abstract void RemoveSelectionBoxes(PrefabInstance _prefabInstance);

		// Token: 0x0600C168 RID: 49512
		public abstract void RemoveVolumes(PrefabInstance _prefabInstance);

		// Token: 0x0600C169 RID: 49513
		public abstract void CopyVolumesIntoWorld(World _world, Chunk _chunk, Vector3i _offset);

		// Token: 0x0600C16A RID: 49514 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public PrefabVolumeListAbs()
		{
		}
	}
}
