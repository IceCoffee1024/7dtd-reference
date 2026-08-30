using System;
using UnityEngine;

// Token: 0x02000BA6 RID: 2982
public struct SpawnPosition : IEquatable<SpawnPosition>
{
	// Token: 0x06005A29 RID: 23081 RVA: 0x0022B12C File Offset: 0x0022932C
	public SpawnPosition(bool _bInvalid)
	{
		this.position = Vector3.zero;
		this.heading = 0f;
		this.bInvalid = true;
	}

	// Token: 0x06005A2A RID: 23082 RVA: 0x0022B14B File Offset: 0x0022934B
	public SpawnPosition(Vector3i _blockPos, float _heading)
	{
		this.position = _blockPos.ToVector3() + new Vector3(0.5f, 0f, 0.5f);
		this.heading = _heading;
		this.bInvalid = false;
	}

	// Token: 0x06005A2B RID: 23083 RVA: 0x0022B181 File Offset: 0x00229381
	public SpawnPosition(Vector3 _position, float _heading)
	{
		this.position = _position;
		this.heading = _heading;
		this.bInvalid = false;
	}

	// Token: 0x06005A2C RID: 23084 RVA: 0x0022B198 File Offset: 0x00229398
	public Vector3i ToBlockPos()
	{
		return new Vector3i(Utils.Fastfloor(this.position.x), Utils.Fastfloor(this.position.y), Utils.Fastfloor(this.position.z));
	}

	// Token: 0x06005A2D RID: 23085 RVA: 0x0022B1CF File Offset: 0x002293CF
	public void Read(IBinaryReaderOrWriter _readerOrWriter, uint _version)
	{
		if (_version > 1U && _version < 3U)
		{
			_readerOrWriter.ReadWrite(0);
		}
		this.position = _readerOrWriter.ReadWrite(Vector3.zero);
		this.heading = _readerOrWriter.ReadWrite(0f);
	}

	// Token: 0x06005A2E RID: 23086 RVA: 0x0022B203 File Offset: 0x00229403
	public void Read(PooledBinaryReader _br, uint _version)
	{
		if (_version > 1U)
		{
			_br.ReadUInt16();
		}
		this.position = new Vector3(_br.ReadSingle(), _br.ReadSingle(), _br.ReadSingle());
		this.heading = _br.ReadSingle();
	}

	// Token: 0x06005A2F RID: 23087 RVA: 0x0022B23C File Offset: 0x0022943C
	public void Write(PooledBinaryWriter _bw)
	{
		_bw.Write(0);
		_bw.Write(this.position.x);
		_bw.Write(this.position.y);
		_bw.Write(this.position.z);
		_bw.Write(this.heading);
	}

	// Token: 0x06005A30 RID: 23088 RVA: 0x0022B28F File Offset: 0x0022948F
	public bool IsUndef()
	{
		return this.Equals(SpawnPosition.Undef);
	}

	// Token: 0x06005A31 RID: 23089 RVA: 0x0022B29C File Offset: 0x0022949C
	public bool Equals(SpawnPosition _other)
	{
		return this.position.Equals(_other.position) && this.heading == _other.heading && this.bInvalid == _other.bInvalid;
	}

	// Token: 0x06005A32 RID: 23090 RVA: 0x0022B2CF File Offset: 0x002294CF
	public override bool Equals(object obj)
	{
		return obj != null && obj is SpawnPosition && this.Equals((SpawnPosition)obj);
	}

	// Token: 0x06005A33 RID: 23091 RVA: 0x0022B2EC File Offset: 0x002294EC
	public override int GetHashCode()
	{
		return (((this.position.GetHashCode() * 397) ^ this.heading.GetHashCode()) * 397) ^ this.bInvalid.GetHashCode();
	}

	// Token: 0x06005A34 RID: 23092 RVA: 0x0022B323 File Offset: 0x00229523
	public override string ToString()
	{
		return string.Format("SpawnPoint {0}/{1}", this.position.ToCultureInvariantString(), this.heading.ToCultureInvariantString("0.0"));
	}

	// Token: 0x040045D6 RID: 17878
	public static SpawnPosition Undef = new SpawnPosition(true);

	// Token: 0x040045D7 RID: 17879
	public Vector3 position;

	// Token: 0x040045D8 RID: 17880
	public float heading;

	// Token: 0x040045D9 RID: 17881
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bInvalid;
}
