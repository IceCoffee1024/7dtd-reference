using System;
using UnityEngine;

// Token: 0x02000BCA RID: 3018
public struct SpawnPosition : IEquatable<SpawnPosition>
{
	// Token: 0x06005ACA RID: 23242 RVA: 0x00231190 File Offset: 0x0022F390
	public SpawnPosition(bool _bInvalid)
	{
		this.position = Vector3.zero;
		this.heading = 0f;
		this.bInvalid = true;
	}

	// Token: 0x06005ACB RID: 23243 RVA: 0x002311AF File Offset: 0x0022F3AF
	public SpawnPosition(Vector3i _blockPos, float _heading)
	{
		this.position = _blockPos.ToVector3() + new Vector3(0.5f, 0f, 0.5f);
		this.heading = _heading;
		this.bInvalid = false;
	}

	// Token: 0x06005ACC RID: 23244 RVA: 0x002311E5 File Offset: 0x0022F3E5
	public SpawnPosition(Vector3 _position, float _heading)
	{
		this.position = _position;
		this.heading = _heading;
		this.bInvalid = false;
	}

	// Token: 0x06005ACD RID: 23245 RVA: 0x002311FC File Offset: 0x0022F3FC
	public Vector3i ToBlockPos()
	{
		return new Vector3i(Utils.Fastfloor(this.position.x), Utils.Fastfloor(this.position.y), Utils.Fastfloor(this.position.z));
	}

	// Token: 0x06005ACE RID: 23246 RVA: 0x00231233 File Offset: 0x0022F433
	public void Read(IBinaryReaderOrWriter _readerOrWriter, uint _version)
	{
		if (_version > 1U && _version < 3U)
		{
			_readerOrWriter.ReadWrite(0);
		}
		this.position = _readerOrWriter.ReadWrite(Vector3.zero);
		this.heading = _readerOrWriter.ReadWrite(0f);
	}

	// Token: 0x06005ACF RID: 23247 RVA: 0x00231267 File Offset: 0x0022F467
	public void Read(PooledBinaryReader _br, uint _version)
	{
		if (_version > 1U)
		{
			_br.ReadUInt16();
		}
		this.position = new Vector3(_br.ReadSingle(), _br.ReadSingle(), _br.ReadSingle());
		this.heading = _br.ReadSingle();
	}

	// Token: 0x06005AD0 RID: 23248 RVA: 0x002312A0 File Offset: 0x0022F4A0
	public void Write(PooledBinaryWriter _bw)
	{
		_bw.Write(0);
		_bw.Write(this.position.x);
		_bw.Write(this.position.y);
		_bw.Write(this.position.z);
		_bw.Write(this.heading);
	}

	// Token: 0x06005AD1 RID: 23249 RVA: 0x002312F3 File Offset: 0x0022F4F3
	public bool IsUndef()
	{
		return this.Equals(SpawnPosition.Undef);
	}

	// Token: 0x06005AD2 RID: 23250 RVA: 0x00231300 File Offset: 0x0022F500
	public bool Equals(SpawnPosition _other)
	{
		return this.position.Equals(_other.position) && this.heading == _other.heading && this.bInvalid == _other.bInvalid;
	}

	// Token: 0x06005AD3 RID: 23251 RVA: 0x00231333 File Offset: 0x0022F533
	public override bool Equals(object obj)
	{
		return obj != null && obj is SpawnPosition && this.Equals((SpawnPosition)obj);
	}

	// Token: 0x06005AD4 RID: 23252 RVA: 0x00231350 File Offset: 0x0022F550
	public override int GetHashCode()
	{
		return (((this.position.GetHashCode() * 397) ^ this.heading.GetHashCode()) * 397) ^ this.bInvalid.GetHashCode();
	}

	// Token: 0x06005AD5 RID: 23253 RVA: 0x00231387 File Offset: 0x0022F587
	public override string ToString()
	{
		return string.Format("SpawnPoint {0}/{1}", this.position.ToCultureInvariantString(), this.heading.ToCultureInvariantString("0.0"));
	}

	// Token: 0x040046B1 RID: 18097
	public static SpawnPosition Undef = new SpawnPosition(true);

	// Token: 0x040046B2 RID: 18098
	public Vector3 position;

	// Token: 0x040046B3 RID: 18099
	public float heading;

	// Token: 0x040046B4 RID: 18100
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bInvalid;
}
