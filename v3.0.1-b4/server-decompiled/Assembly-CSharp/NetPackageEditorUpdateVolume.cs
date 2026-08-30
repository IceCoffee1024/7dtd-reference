using System;
using PrefabVolumes;
using UnityEngine.Scripting;

// Token: 0x020007C4 RID: 1988
[Preserve]
public class NetPackageEditorUpdateVolume : NetPackage
{
	// Token: 0x06003ADF RID: 15071 RVA: 0x0017C364 File Offset: 0x0017A564
	public NetPackageEditorUpdateVolume Setup(NetPackageEditorUpdateVolume.EChangeType _changeType, int _prefabInstanceId, int _volumeId, PrefabVolumeAbs _volume)
	{
		this.changeType = _changeType;
		this.prefabInstanceId = _prefabInstanceId;
		this.volumeId = _volumeId;
		this.volume = _volume.Clone();
		return this;
	}

	// Token: 0x06003AE0 RID: 15072 RVA: 0x0017C38C File Offset: 0x0017A58C
	public override void read(PooledBinaryReader _br)
	{
		this.changeType = (NetPackageEditorUpdateVolume.EChangeType)_br.ReadByte();
		this.prefabInstanceId = _br.ReadInt32();
		this.volumeId = _br.ReadInt32();
		PrefabVolumeAbs.EVolumeType evolumeType = (PrefabVolumeAbs.EVolumeType)_br.ReadByte();
		this.volume = PrefabVolumeAbs.CreateByType(evolumeType);
		this.volume.Read(_br);
	}

	// Token: 0x06003AE1 RID: 15073 RVA: 0x0017C3DC File Offset: 0x0017A5DC
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write((byte)this.changeType);
		_bw.Write(this.prefabInstanceId);
		_bw.Write(this.volumeId);
		_bw.Write((byte)this.volume.VolumeType);
		this.volume.Write(_bw);
	}

	// Token: 0x06003AE2 RID: 15074 RVA: 0x0017C434 File Offset: 0x0017A634
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

	// Token: 0x06003AE3 RID: 15075 RVA: 0x0017C4AB File Offset: 0x0017A6AB
	public override int GetLength()
	{
		return 14 + this.volume.SerializedSize;
	}

	// Token: 0x04002FDF RID: 12255
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageEditorUpdateVolume.EChangeType changeType;

	// Token: 0x04002FE0 RID: 12256
	[PublicizedFrom(EAccessModifier.Private)]
	public int prefabInstanceId;

	// Token: 0x04002FE1 RID: 12257
	[PublicizedFrom(EAccessModifier.Private)]
	public int volumeId;

	// Token: 0x04002FE2 RID: 12258
	[PublicizedFrom(EAccessModifier.Private)]
	public PrefabVolumeAbs volume;

	// Token: 0x020007C5 RID: 1989
	public enum EChangeType
	{
		// Token: 0x04002FE4 RID: 12260
		Added,
		// Token: 0x04002FE5 RID: 12261
		Changed
	}
}
