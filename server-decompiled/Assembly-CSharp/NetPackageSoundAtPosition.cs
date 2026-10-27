using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000864 RID: 2148
[Preserve]
public class NetPackageSoundAtPosition : NetPackage
{
	// Token: 0x06003E39 RID: 15929 RVA: 0x0018B3E3 File Offset: 0x001895E3
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

	// Token: 0x06003E3A RID: 15930 RVA: 0x0018B413 File Offset: 0x00189613
	public override void read(PooledBinaryReader _br)
	{
		this.pos = StreamUtils.ReadVector3(_br);
		this.audioClipName = _br.ReadString();
		this.mode = (AudioRolloffMode)_br.ReadByte();
		this.distance = _br.ReadInt32();
		this.entityId = _br.ReadInt32();
	}

	// Token: 0x06003E3B RID: 15931 RVA: 0x0018B454 File Offset: 0x00189654
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		StreamUtils.Write(_bw, this.pos);
		_bw.Write(this.audioClipName);
		_bw.Write((byte)this.mode);
		_bw.Write(this.distance);
		_bw.Write(this.entityId);
	}

	// Token: 0x06003E3C RID: 15932 RVA: 0x0018B4A8 File Offset: 0x001896A8
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

	// Token: 0x04003297 RID: 12951
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 pos;

	// Token: 0x04003298 RID: 12952
	[PublicizedFrom(EAccessModifier.Private)]
	public string audioClipName;

	// Token: 0x04003299 RID: 12953
	[PublicizedFrom(EAccessModifier.Private)]
	public AudioRolloffMode mode;

	// Token: 0x0400329A RID: 12954
	[PublicizedFrom(EAccessModifier.Private)]
	public int distance;

	// Token: 0x0400329B RID: 12955
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x0400329C RID: 12956
	[PublicizedFrom(EAccessModifier.Private)]
	public float volumeScale;
}
