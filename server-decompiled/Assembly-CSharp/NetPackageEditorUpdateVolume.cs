using System;
using PrefabVolumes;
using UnityEngine.Scripting;

// Token: 0x020007DC RID: 2012
[Preserve]
public class NetPackageEditorUpdateVolume : NetPackage
{
	// Token: 0x06003B7F RID: 15231 RVA: 0x0017FDD3 File Offset: 0x0017DFD3
	public NetPackageEditorUpdateVolume Setup(NetPackageEditorUpdateVolume.EChangeType _changeType, int _prefabInstanceId, int _volumeId, PrefabVolumeAbs _volume)
	{
		this.changeType = _changeType;
		this.prefabInstanceId = _prefabInstanceId;
		this.volumeId = _volumeId;
		this.volume = _volume.Clone();
		return this;
	}

	// Token: 0x06003B80 RID: 15232 RVA: 0x0017FDF8 File Offset: 0x0017DFF8
	public override void read(PooledBinaryReader _br)
	{
		this.changeType = (NetPackageEditorUpdateVolume.EChangeType)_br.ReadByte();
		this.prefabInstanceId = _br.ReadInt32();
		this.volumeId = _br.ReadInt32();
		PrefabVolumeAbs.EVolumeType evolumeType = (PrefabVolumeAbs.EVolumeType)_br.ReadByte();
		this.volume = PrefabVolumeAbs.CreateByType(evolumeType);
		this.volume.Read(_br);
	}

	// Token: 0x06003B81 RID: 15233 RVA: 0x0017FE48 File Offset: 0x0017E048
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write((byte)this.changeType);
		_bw.Write(this.prefabInstanceId);
		_bw.Write(this.volumeId);
		_bw.Write((byte)this.volume.VolumeType);
		this.volume.Write(_bw);
	}

	// Token: 0x06003B82 RID: 15234 RVA: 0x0017FEA0 File Offset: 0x0017E0A0
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		if (!_world.IsRemote())
		{
			NetPackageEditorUpdateVolume.EChangeType echangeType = this.changeType;
			if (echangeType != NetPackageEditorUpdateVolume.EChangeType.Added && echangeType == NetPackageEditorUpdateVolume.EChangeType.Changed)
			{
				PrefabVolumeManager.Instance.UpdatePropertiesServer(this.prefabInstanceId, this.volumeId, this.volume);
				return;
			}
			throw new ArgumentOutOfRangeException();
		}
		else
		{
			NetPackageEditorUpdateVolume.EChangeType echangeType = this.changeType;
			if (echangeType <= NetPackageEditorUpdateVolume.EChangeType.Changed)
			{
				PrefabVolumeManager.Instance.AddUpdatePropertiesClient(this.prefabInstanceId, this.volumeId, this.volume);
				return;
			}
			throw new ArgumentOutOfRangeException();
		}
	}

	// Token: 0x04003078 RID: 12408
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageEditorUpdateVolume.EChangeType changeType;

	// Token: 0x04003079 RID: 12409
	[PublicizedFrom(EAccessModifier.Private)]
	public int prefabInstanceId;

	// Token: 0x0400307A RID: 12410
	[PublicizedFrom(EAccessModifier.Private)]
	public int volumeId;

	// Token: 0x0400307B RID: 12411
	[PublicizedFrom(EAccessModifier.Private)]
	public PrefabVolumeAbs volume;

	// Token: 0x020007DD RID: 2013
	public enum EChangeType
	{
		// Token: 0x0400307D RID: 12413
		Added,
		// Token: 0x0400307E RID: 12414
		Changed
	}
}
