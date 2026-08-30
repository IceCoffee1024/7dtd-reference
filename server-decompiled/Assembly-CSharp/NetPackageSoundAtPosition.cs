using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000857 RID: 2135
[Preserve]
public class NetPackageSoundAtPosition : NetPackage
{
	// Token: 0x06003E6E RID: 15982 RVA: 0x0018A48F File Offset: 0x0018868F
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

	// Token: 0x06003E6F RID: 15983 RVA: 0x0018A4BF File Offset: 0x001886BF
	public override void read(PooledBinaryReader _br)
	{
		this.pos = StreamUtils.ReadVector3(_br);
		this.audioClipName = _br.ReadString();
		this.mode = (AudioRolloffMode)_br.ReadByte();
		this.distance = _br.ReadInt32();
		this.entityId = _br.ReadInt32();
	}

	// Token: 0x06003E70 RID: 15984 RVA: 0x0018A500 File Offset: 0x00188700
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		StreamUtils.Write(_bw, this.pos);
		_bw.Write(this.audioClipName);
		_bw.Write((byte)this.mode);
		_bw.Write(this.distance);
		_bw.Write(this.entityId);
	}

	// Token: 0x06003E71 RID: 15985 RVA: 0x0018A554 File Offset: 0x00188754
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

	// Token: 0x06003E72 RID: 15986 RVA: 0x0017CB1E File Offset: 0x0017AD1E
	public override int GetLength()
	{
		return 40;
	}

	// Token: 0x0400328C RID: 12940
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 pos;

	// Token: 0x0400328D RID: 12941
	[PublicizedFrom(EAccessModifier.Private)]
	public string audioClipName;

	// Token: 0x0400328E RID: 12942
	[PublicizedFrom(EAccessModifier.Private)]
	public AudioRolloffMode mode;

	// Token: 0x0400328F RID: 12943
	[PublicizedFrom(EAccessModifier.Private)]
	public int distance;

	// Token: 0x04003290 RID: 12944
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x04003291 RID: 12945
	[PublicizedFrom(EAccessModifier.Private)]
	public float volumeScale;
}
