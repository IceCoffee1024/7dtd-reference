using System;
using System.Runtime.CompilerServices;

namespace PrefabVolumes
{
	// Token: 0x020018D7 RID: 6359
	public abstract class PrefabVolumeListAbs
	{
		// Token: 0x17001893 RID: 6291
		// (get) Token: 0x0600C436 RID: 50230
		public abstract PrefabVolumeAbs.EVolumeType VolumeType { get; }

		// Token: 0x17001894 RID: 6292
		// (get) Token: 0x0600C437 RID: 50231
		public abstract bool AnyUsedEntry { get; }

		// Token: 0x17001895 RID: 6293
		// (get) Token: 0x0600C438 RID: 50232
		public abstract int Count { get; }

		// Token: 0x0600C439 RID: 50233
		public abstract PrefabVolumeAbs Get(int _index);

		// Token: 0x0600C43A RID: 50234
		public abstract void CopyFrom(PrefabVolumeListAbs _other);

		// Token: 0x0600C43B RID: 50235
		public abstract void ReadFromProperties(DynamicProperties _properties);

		// Token: 0x0600C43C RID: 50236
		public abstract void WriteToProperties(DynamicProperties _properties);

		// Token: 0x0600C43D RID: 50237
		public abstract void SendAllVolumesToClient(ClientInfo _clientInfo, int _prefabInstanceId);

		// Token: 0x0600C43E RID: 50238
		public abstract void Move(Vector3i _moveDistance);

		// Token: 0x0600C43F RID: 50239
		public abstract void RotateY(bool _bLeft, Vector3i _prefabSize);

		// Token: 0x0600C440 RID: 50240
		public abstract bool CanCreateVolume(string _prefabInstanceName, Vector3i _bbPos, Vector3i _startPos, Vector3i _size);

		// Token: 0x0600C441 RID: 50241
		[return: TupleElementNames(new string[] { "volumeIndex", "volume", "box" })]
		public abstract ValueTuple<int, PrefabVolumeAbs, SelectionBox> AddNewVolume(string _prefabInstanceName, Vector3i _bbPos, Vector3i _startPos, Vector3i _size);

		// Token: 0x0600C442 RID: 50242
		[return: TupleElementNames(new string[] { "volumeIndex", "volume", "box" })]
		public abstract ValueTuple<int, PrefabVolumeAbs, SelectionBox> CloneVolume(string _prefabInstanceName, Vector3i _bbPos, int _existingIndex, Vector3i _offset);

		// Token: 0x0600C443 RID: 50243
		public abstract void SetVolume(PrefabInstance _prefabInstance, int _index, PrefabVolumeAbs _volumeSettings);

		// Token: 0x0600C444 RID: 50244
		public abstract void CreateSelectionBoxes(PrefabInstance _prefabInstance);

		// Token: 0x0600C445 RID: 50245
		public abstract void ApplyVolumesToSelectionBoxes(PrefabInstance _prefabInstance);

		// Token: 0x0600C446 RID: 50246
		public abstract void RemoveSelectionBoxes(PrefabInstance _prefabInstance);

		// Token: 0x0600C447 RID: 50247
		public abstract void RemoveVolumes(PrefabInstance _prefabInstance);

		// Token: 0x0600C448 RID: 50248
		public abstract void CopyVolumesIntoWorld(World _world, Chunk _chunk, Vector3i _offset);

		// Token: 0x0600C449 RID: 50249 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public PrefabVolumeListAbs()
		{
		}
	}
}
