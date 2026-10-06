using System;
using PrefabVolumes;
using UnityEngine.Scripting;

// Token: 0x020007D2 RID: 2002
[Preserve]
public class NetPackageEditorUpdateVolume : NetPackage
{
	// Token: 0x06003B5C RID: 15196 RVA: 0x0017ECCF File Offset: 0x0017CECF
	public NetPackageEditorUpdateVolume Setup(NetPackageEditorUpdateVolume.EChangeType _changeType, int _prefabInstanceId, int _volumeId, PrefabVolumeAbs _volume)
	{
		this.changeType = _changeType;
		this.prefabInstanceId = _prefabInstanceId;
		this.volumeId = _volumeId;
		this.volume = _volume.Clone();
		return this;
	}

	// Token: 0x06003B5D RID: 15197 RVA: 0x0017ECF4 File Offset: 0x0017CEF4
	public override void read(PooledBinaryReader _br)
	{
		this.changeType = (NetPackageEditorUpdateVolume.EChangeType)_br.ReadByte();
		this.prefabInstanceId = _br.ReadInt32();
		this.volumeId = _br.ReadInt32();
		PrefabVolumeAbs.EVolumeType evolumeType = (PrefabVolumeAbs.EVolumeType)_br.ReadByte();
		this.volume = PrefabVolumeAbs.CreateByType(evolumeType);
		this.volume.Read(_br);
	}

	// Token: 0x06003B5E RID: 15198 RVA: 0x0017ED44 File Offset: 0x0017CF44
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write((byte)this.changeType);
		_bw.Write(this.prefabInstanceId);
		_bw.Write(this.volumeId);
		_bw.Write((byte)this.volume.VolumeType);
		this.volume.Write(_bw);
	}

	// Token: 0x06003B5F RID: 15199 RVA: 0x0017ED9C File Offset: 0x0017CF9C
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

	// Token: 0x04003058 RID: 12376
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageEditorUpdateVolume.EChangeType changeType;

	// Token: 0x04003059 RID: 12377
	[PublicizedFrom(EAccessModifier.Private)]
	public int prefabInstanceId;

	// Token: 0x0400305A RID: 12378
	[PublicizedFrom(EAccessModifier.Private)]
	public int volumeId;

	// Token: 0x0400305B RID: 12379
	[PublicizedFrom(EAccessModifier.Private)]
	public PrefabVolumeAbs volume;

	// Token: 0x020007D3 RID: 2003
	public enum EChangeType
	{
		// Token: 0x0400305D RID: 12381
		Added,
		// Token: 0x0400305E RID: 12382
		Changed
	}
}
