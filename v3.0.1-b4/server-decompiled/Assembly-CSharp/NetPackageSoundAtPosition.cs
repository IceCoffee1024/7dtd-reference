using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200084B RID: 2123
[Preserve]
public class NetPackageSoundAtPosition : NetPackage
{
	// Token: 0x06003E09 RID: 15881 RVA: 0x0018845B File Offset: 0x0018665B
	public NetPackageSoundAtPosition Setup(Vector3 _pos, string _audioClipName, AudioRolloffMode _mode, int _distance, int _entityId, float _volumeScale)
	{
		this.pos = _pos;
		this.audioClipName = _audioClipName;
		this.mode = _mode;
		this.distance = _distance;
		this.entityId = _entityId;
		this.volumeScale = _volumeScale;
		return this;
	}

	// Token: 0x06003E0A RID: 15882 RVA: 0x0018848B File Offset: 0x0018668B
	public override void read(PooledBinaryReader _br)
	{
		this.pos = StreamUtils.ReadVector3(_br);
		this.audioClipName = _br.ReadString();
		this.mode = (AudioRolloffMode)_br.ReadByte();
		this.distance = _br.ReadInt32();
		this.entityId = _br.ReadInt32();
	}

	// Token: 0x06003E0B RID: 15883 RVA: 0x001884CC File Offset: 0x001866CC
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		StreamUtils.Write(_bw, this.pos);
		_bw.Write(this.audioClipName);
		_bw.Write((byte)this.mode);
		_bw.Write(this.distance);
		_bw.Write(this.entityId);
	}

	// Token: 0x06003E0C RID: 15884 RVA: 0x00188520 File Offset: 0x00186720
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		if (!_world.IsRemote())
		{
			_world.gameManager.PlaySoundAtPositionServer(this.pos, this.audioClipName, this.mode, this.distance, this.entityId, this.volumeScale);
			return;
		}
		_world.gameManager.PlaySoundAtPositionClient(this.pos, this.audioClipName, this.mode, this.distance, this.volumeScale);
	}

	// Token: 0x06003E0D RID: 15885 RVA: 0x0017A89E File Offset: 0x00178A9E
	public override int GetLength()
	{
		return 40;
	}

	// Token: 0x0400320D RID: 12813
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 pos;

	// Token: 0x0400320E RID: 12814
	[PublicizedFrom(EAccessModifier.Private)]
	public string audioClipName;

	// Token: 0x0400320F RID: 12815
	[PublicizedFrom(EAccessModifier.Private)]
	public AudioRolloffMode mode;

	// Token: 0x04003210 RID: 12816
	[PublicizedFrom(EAccessModifier.Private)]
	public int distance;

	// Token: 0x04003211 RID: 12817
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x04003212 RID: 12818
	[PublicizedFrom(EAccessModifier.Private)]
	public float volumeScale;
}
