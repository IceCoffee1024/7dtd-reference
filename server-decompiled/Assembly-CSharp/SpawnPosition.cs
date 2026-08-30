using System;
using UnityEngine;

// Token: 0x02000BB8 RID: 3000
public struct SpawnPosition : IEquatable<SpawnPosition>
{
	// Token: 0x06005AB1 RID: 23217 RVA: 0x0022DE94 File Offset: 0x0022C094
	public SpawnPosition(bool _bInvalid)
	{
		this.position = Vector3.zero;
		this.heading = 0f;
		this.bInvalid = true;
	}

	// Token: 0x06005AB2 RID: 23218 RVA: 0x0022DEB3 File Offset: 0x0022C0B3
	public SpawnPosition(Vector3i _blockPos, float _heading)
	{
		this.position = _blockPos.ToVector3() + new Vector3(0.5f, 0f, 0.5f);
		this.heading = _heading;
		this.bInvalid = false;
	}

	// Token: 0x06005AB3 RID: 23219 RVA: 0x0022DEE9 File Offset: 0x0022C0E9
	public SpawnPosition(Vector3 _position, float _heading)
	{
		this.position = _position;
		this.heading = _heading;
		this.bInvalid = false;
	}

	// Token: 0x06005AB4 RID: 23220 RVA: 0x0022DF00 File Offset: 0x0022C100
	public Vector3i ToBlockPos()
	{
		return new Vector3i(Utils.Fastfloor(this.position.x), Utils.Fastfloor(this.position.y), Utils.Fastfloor(this.position.z));
	}

	// Token: 0x06005AB5 RID: 23221 RVA: 0x0022DF37 File Offset: 0x0022C137
	public void Read(IBinaryReaderOrWriter _readerOrWriter, uint _version)
	{
		if (_version > 1U && _version < 3U)
		{
			_readerOrWriter.ReadWrite(0);
		}
		this.position = _readerOrWriter.ReadWrite(Vector3.zero);
		this.heading = _readerOrWriter.ReadWrite(0f);
	}

	// Token: 0x06005AB6 RID: 23222 RVA: 0x0022DF6B File Offset: 0x0022C16B
	public void Read(PooledBinaryReader _br, uint _version)
	{
		if (_version > 1U)
		{
			_br.ReadUInt16();
		}
		this.position = new Vector3(_br.ReadSingle(), _br.ReadSingle(), _br.ReadSingle());
		this.heading = _br.ReadSingle();
	}

	// Token: 0x06005AB7 RID: 23223 RVA: 0x0022DFA4 File Offset: 0x0022C1A4
	public void Write(PooledBinaryWriter _bw)
	{
		_bw.Write(0);
		_bw.Write(this.position.x);
		_bw.Write(this.position.y);
		_bw.Write(this.position.z);
		_bw.Write(this.heading);
	}

	// Token: 0x06005AB8 RID: 23224 RVA: 0x0022DFF7 File Offset: 0x0022C1F7
	public bool IsUndef()
	{
		return this.Equals(SpawnPosition.Undef);
	}

	// Token: 0x06005AB9 RID: 23225 RVA: 0x0022E004 File Offset: 0x0022C204
	public bool Equals(SpawnPosition _other)
	{
		return this.position.Equals(_other.position) && this.heading == _other.heading && this.bInvalid == _other.bInvalid;
	}

	// Token: 0x06005ABA RID: 23226 RVA: 0x0022E037 File Offset: 0x0022C237
	public override bool Equals(object obj)
	{
		return obj != null && obj is SpawnPosition && this.Equals((SpawnPosition)obj);
	}

	// Token: 0x06005ABB RID: 23227 RVA: 0x0022E054 File Offset: 0x0022C254
	public override int GetHashCode()
	{
		return (((this.position.GetHashCode() * 397) ^ this.heading.GetHashCode()) * 397) ^ this.bInvalid.GetHashCode();
	}

	// Token: 0x06005ABC RID: 23228 RVA: 0x0022E08B File Offset: 0x0022C28B
	public override string ToString()
	{
		return string.Format("SpawnPoint {0}/{1}", this.position.ToCultureInvariantString(), this.heading.ToCultureInvariantString("0.0"));
	}

	// Token: 0x04004672 RID: 18034
	public static SpawnPosition Undef = new SpawnPosition(true);

	// Token: 0x04004673 RID: 18035
	public Vector3 position;

	// Token: 0x04004674 RID: 18036
	public float heading;

	// Token: 0x04004675 RID: 18037
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bInvalid;
}
