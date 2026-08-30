using System;
using PrefabVolumes;
using UnityEngine.Scripting;

// Token: 0x020007CF RID: 1999
[Preserve]
public class NetPackageEditorUpdateVolume : NetPackage
{
	// Token: 0x06003B3E RID: 15166 RVA: 0x0017E73C File Offset: 0x0017C93C
	public NetPackageEditorUpdateVolume Setup(NetPackageEditorUpdateVolume.EChangeType _changeType, int _prefabInstanceId, int _volumeId, PrefabVolumeAbs _volume)
	{
		this.changeType = _changeType;
		this.prefabInstanceId = _prefabInstanceId;
		this.volumeId = _volumeId;
		this.volume = _volume.Clone();
		return this;
	}

	// Token: 0x06003B3F RID: 15167 RVA: 0x0017E764 File Offset: 0x0017C964
	public override void read(PooledBinaryReader _br)
	{
		this.changeType = (NetPackageEditorUpdateVolume.EChangeType)_br.ReadByte();
		this.prefabInstanceId = _br.ReadInt32();
		this.volumeId = _br.ReadInt32();
		PrefabVolumeAbs.EVolumeType evolumeType = (PrefabVolumeAbs.EVolumeType)_br.ReadByte();
		this.volume = PrefabVolumeAbs.CreateByType(evolumeType);
		this.volume.Read(_br);
	}

	// Token: 0x06003B40 RID: 15168 RVA: 0x0017E7B4 File Offset: 0x0017C9B4
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write((byte)this.changeType);
		_bw.Write(this.prefabInstanceId);
		_bw.Write(this.volumeId);
		_bw.Write((byte)this.volume.VolumeType);
		this.volume.Write(_bw);
	}

	// Token: 0x06003B41 RID: 15169 RVA: 0x0017E80C File Offset: 0x0017CA0C
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

	// Token: 0x06003B42 RID: 15170 RVA: 0x0017E883 File Offset: 0x0017CA83
	public override int GetLength()
	{
		return 14 + this.volume.SerializedSize;
	}

	// Token: 0x0400305E RID: 12382
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageEditorUpdateVolume.EChangeType changeType;

	// Token: 0x0400305F RID: 12383
	[PublicizedFrom(EAccessModifier.Private)]
	public int prefabInstanceId;

	// Token: 0x04003060 RID: 12384
	[PublicizedFrom(EAccessModifier.Private)]
	public int volumeId;

	// Token: 0x04003061 RID: 12385
	[PublicizedFrom(EAccessModifier.Private)]
	public PrefabVolumeAbs volume;

	// Token: 0x020007D0 RID: 2000
	public enum EChangeType
	{
		// Token: 0x04003063 RID: 12387
		Added,
		// Token: 0x04003064 RID: 12388
		Changed
	}
}
