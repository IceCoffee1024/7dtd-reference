using System;
using System.Runtime.CompilerServices;

namespace PrefabVolumes
{
	// Token: 0x020018AB RID: 6315
	public abstract class PrefabVolumeListAbs
	{
		// Token: 0x170017F1 RID: 6129
		// (get) Token: 0x0600C2DE RID: 49886
		public abstract PrefabVolumeAbs.EVolumeType VolumeType { get; }

		// Token: 0x170017F2 RID: 6130
		// (get) Token: 0x0600C2DF RID: 49887
		public abstract bool AnyUsedEntry { get; }

		// Token: 0x170017F3 RID: 6131
		// (get) Token: 0x0600C2E0 RID: 49888
		public abstract int Count { get; }

		// Token: 0x0600C2E1 RID: 49889
		public abstract PrefabVolumeAbs Get(int _index);

		// Token: 0x0600C2E2 RID: 49890
		public abstract void CopyFrom(PrefabVolumeListAbs _other);

		// Token: 0x0600C2E3 RID: 49891
		public abstract void ReadFromProperties(DynamicProperties _properties);

		// Token: 0x0600C2E4 RID: 49892
		public abstract void WriteToProperties(DynamicProperties _properties);

		// Token: 0x0600C2E5 RID: 49893
		public abstract void SendAllVolumesToClient(ClientInfo _clientInfo, int _prefabInstanceId);

		// Token: 0x0600C2E6 RID: 49894
		public abstract void Move(Vector3i _moveDistance);

		// Token: 0x0600C2E7 RID: 49895
		public abstract void RotateY(bool _bLeft, Vector3i _prefabSize);

		// Token: 0x0600C2E8 RID: 49896
		public abstract bool CanCreateVolume(string _prefabInstanceName, Vector3i _bbPos, Vector3i _startPos, Vector3i _size);

		// Token: 0x0600C2E9 RID: 49897
		[return: TupleElementNames(new string[] { "volumeIndex", "volume", "box" })]
		public abstract ValueTuple<int, PrefabVolumeAbs, SelectionBox> AddNewVolume(string _prefabInstanceName, Vector3i _bbPos, Vector3i _startPos, Vector3i _size);

		// Token: 0x0600C2EA RID: 49898
		[return: TupleElementNames(new string[] { "volumeIndex", "volume", "box" })]
		public abstract ValueTuple<int, PrefabVolumeAbs, SelectionBox> CloneVolume(string _prefabInstanceName, Vector3i _bbPos, int _existingIndex, Vector3i _offset);

		// Token: 0x0600C2EB RID: 49899
		public abstract void SetVolume(PrefabInstance _prefabInstance, int _index, PrefabVolumeAbs _volumeSettings);

		// Token: 0x0600C2EC RID: 49900
		public abstract void CreateSelectionBoxes(PrefabInstance _prefabInstance);

		// Token: 0x0600C2ED RID: 49901
		public abstract void ApplyVolumesToSelectionBoxes(PrefabInstance _prefabInstance);

		// Token: 0x0600C2EE RID: 49902
		public abstract void RemoveSelectionBoxes(PrefabInstance _prefabInstance);

		// Token: 0x0600C2EF RID: 49903
		public abstract void RemoveVolumes(PrefabInstance _prefabInstance);

		// Token: 0x0600C2F0 RID: 49904
		public abstract void CopyVolumesIntoWorld(World _world, Chunk _chunk, Vector3i _offset);

		// Token: 0x0600C2F1 RID: 49905 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public PrefabVolumeListAbs()
		{
		}
	}
}
