using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200085A RID: 2138
[Preserve]
public class NetPackageSoundAtPosition : NetPackage
{
	// Token: 0x06003E16 RID: 15894 RVA: 0x0018A33B File Offset: 0x0018853B
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

	// Token: 0x06003E17 RID: 15895 RVA: 0x0018A36B File Offset: 0x0018856B
	public override void read(PooledBinaryReader _br)
	{
		this.pos = StreamUtils.ReadVector3(_br);
		this.audioClipName = _br.ReadString();
		this.mode = (AudioRolloffMode)_br.ReadByte();
		this.distance = _br.ReadInt32();
		this.entityId = _br.ReadInt32();
	}

	// Token: 0x06003E18 RID: 15896 RVA: 0x0018A3AC File Offset: 0x001885AC
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		StreamUtils.Write(_bw, this.pos);
		_bw.Write(this.audioClipName);
		_bw.Write((byte)this.mode);
		_bw.Write(this.distance);
		_bw.Write(this.entityId);
	}

	// Token: 0x06003E19 RID: 15897 RVA: 0x0018A400 File Offset: 0x00188600
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

	// Token: 0x04003277 RID: 12919
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 pos;

	// Token: 0x04003278 RID: 12920
	[PublicizedFrom(EAccessModifier.Private)]
	public string audioClipName;

	// Token: 0x04003279 RID: 12921
	[PublicizedFrom(EAccessModifier.Private)]
	public AudioRolloffMode mode;

	// Token: 0x0400327A RID: 12922
	[PublicizedFrom(EAccessModifier.Private)]
	public int distance;

	// Token: 0x0400327B RID: 12923
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x0400327C RID: 12924
	[PublicizedFrom(EAccessModifier.Private)]
	public float volumeScale;
}
