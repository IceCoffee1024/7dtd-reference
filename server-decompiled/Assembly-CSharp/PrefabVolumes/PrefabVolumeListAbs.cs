using System;
using System.Runtime.CompilerServices;

namespace PrefabVolumes
{
	// Token: 0x02001908 RID: 6408
	public abstract class PrefabVolumeListAbs
	{
		// Token: 0x170018DB RID: 6363
		// (get) Token: 0x0600C54A RID: 50506
		public abstract PrefabVolumeAbs.EVolumeType VolumeType { get; }

		// Token: 0x170018DC RID: 6364
		// (get) Token: 0x0600C54B RID: 50507
		public abstract bool AnyUsedEntry { get; }

		// Token: 0x170018DD RID: 6365
		// (get) Token: 0x0600C54C RID: 50508
		public abstract int Count { get; }

		// Token: 0x0600C54D RID: 50509
		public abstract PrefabVolumeAbs Get(int _index);

		// Token: 0x0600C54E RID: 50510
		public abstract void CopyFrom(PrefabVolumeListAbs _other);

		// Token: 0x0600C54F RID: 50511
		public abstract void ReadFromProperties(DynamicProperties _properties);

		// Token: 0x0600C550 RID: 50512
		public abstract void WriteToProperties(DynamicProperties _properties);

		// Token: 0x0600C551 RID: 50513
		public abstract void SendAllVolumesToClient(ClientInfo _clientInfo, int _prefabInstanceId);

		// Token: 0x0600C552 RID: 50514
		public abstract void Move(Vector3i _moveDistance);

		// Token: 0x0600C553 RID: 50515
		public abstract void RotateY(bool _bLeft, Vector3i _prefabSize);

		// Token: 0x0600C554 RID: 50516
		public abstract bool CanCreateVolume(string _prefabInstanceName, Vector3i _bbPos, Vector3i _startPos, Vector3i _size);

		// Token: 0x0600C555 RID: 50517
		[return: TupleElementNames(new string[] { "volumeIndex", "volume", "box" })]
		public abstract ValueTuple<int, PrefabVolumeAbs, SelectionBox> AddNewVolume(string _prefabInstanceName, Vector3i _bbPos, Vector3i _startPos, Vector3i _size);

		// Token: 0x0600C556 RID: 50518
		[return: TupleElementNames(new string[] { "volumeIndex", "volume", "box" })]
		public abstract ValueTuple<int, PrefabVolumeAbs, SelectionBox> CloneVolume(string _prefabInstanceName, Vector3i _bbPos, int _existingIndex, Vector3i _offset);

		// Token: 0x0600C557 RID: 50519
		public abstract void SetVolume(PrefabInstance _prefabInstance, int _index, PrefabVolumeAbs _volumeSettings);

		// Token: 0x0600C558 RID: 50520
		public abstract void CreateSelectionBoxes(PrefabInstance _prefabInstance);

		// Token: 0x0600C559 RID: 50521
		public abstract void ApplyVolumesToSelectionBoxes(PrefabInstance _prefabInstance);

		// Token: 0x0600C55A RID: 50522
		public abstract void RemoveSelectionBoxes(PrefabInstance _prefabInstance);

		// Token: 0x0600C55B RID: 50523
		public abstract void RemoveVolumes(PrefabInstance _prefabInstance);

		// Token: 0x0600C55C RID: 50524
		public abstract void CopyVolumesIntoWorld(World _world, Chunk _chunk, Vector3i _offset);

		// Token: 0x0600C55D RID: 50525 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public PrefabVolumeListAbs()
		{
		}
	}
}
