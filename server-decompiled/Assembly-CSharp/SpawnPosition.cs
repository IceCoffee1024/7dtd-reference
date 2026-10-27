using System;
using UnityEngine;

// Token: 0x02000BD5 RID: 3029
public struct SpawnPosition : IEquatable<SpawnPosition>
{
	// Token: 0x06005AEF RID: 23279 RVA: 0x002326EC File Offset: 0x002308EC
	public SpawnPosition(bool _bInvalid)
	{
		this.position = Vector3.zero;
		this.heading = 0f;
		this.bInvalid = true;
	}

	// Token: 0x06005AF0 RID: 23280 RVA: 0x0023270B File Offset: 0x0023090B
	public SpawnPosition(Vector3i _blockPos, float _heading)
	{
		this.position = _blockPos.ToVector3() + new Vector3(0.5f, 0f, 0.5f);
		this.heading = _heading;
		this.bInvalid = false;
	}

	// Token: 0x06005AF1 RID: 23281 RVA: 0x00232741 File Offset: 0x00230941
	public SpawnPosition(Vector3 _position, float _heading)
	{
		this.position = _position;
		this.heading = _heading;
		this.bInvalid = false;
	}

	// Token: 0x06005AF2 RID: 23282 RVA: 0x00232758 File Offset: 0x00230958
	public Vector3i ToBlockPos()
	{
		return new Vector3i(Utils.Fastfloor(this.position.x), Utils.Fastfloor(this.position.y), Utils.Fastfloor(this.position.z));
	}

	// Token: 0x06005AF3 RID: 23283 RVA: 0x0023278F File Offset: 0x0023098F
	public void Read(IBinaryReaderOrWriter _readerOrWriter, uint _version)
	{
		if (_version > 1U && _version < 3U)
		{
			_readerOrWriter.ReadWrite(0);
		}
		this.position = _readerOrWriter.ReadWrite(Vector3.zero);
		this.heading = _readerOrWriter.ReadWrite(0f);
	}

	// Token: 0x06005AF4 RID: 23284 RVA: 0x002327C3 File Offset: 0x002309C3
	public void Read(PooledBinaryReader _br, uint _version)
	{
		if (_version > 1U)
		{
			_br.ReadUInt16();
		}
		this.position = new Vector3(_br.ReadSingle(), _br.ReadSingle(), _br.ReadSingle());
		this.heading = _br.ReadSingle();
	}

	// Token: 0x06005AF5 RID: 23285 RVA: 0x002327FC File Offset: 0x002309FC
	public void Write(PooledBinaryWriter _bw)
	{
		_bw.Write(0);
		_bw.Write(this.position.x);
		_bw.Write(this.position.y);
		_bw.Write(this.position.z);
		_bw.Write(this.heading);
	}

	// Token: 0x06005AF6 RID: 23286 RVA: 0x0023284F File Offset: 0x00230A4F
	public bool IsUndef()
	{
		return this.Equals(SpawnPosition.Undef);
	}

	// Token: 0x06005AF7 RID: 23287 RVA: 0x0023285C File Offset: 0x00230A5C
	public bool Equals(SpawnPosition _other)
	{
		return this.position.Equals(_other.position) && this.heading == _other.heading && this.bInvalid == _other.bInvalid;
	}

	// Token: 0x06005AF8 RID: 23288 RVA: 0x0023288F File Offset: 0x00230A8F
	public override bool Equals(object obj)
	{
		return obj != null && obj is SpawnPosition && this.Equals((SpawnPosition)obj);
	}

	// Token: 0x06005AF9 RID: 23289 RVA: 0x002328AC File Offset: 0x00230AAC
	public override int GetHashCode()
	{
		return (((this.position.GetHashCode() * 397) ^ this.heading.GetHashCode()) * 397) ^ this.bInvalid.GetHashCode();
	}

	// Token: 0x06005AFA RID: 23290 RVA: 0x002328E3 File Offset: 0x00230AE3
	public override string ToString()
	{
		return string.Format("SpawnPoint {0}/{1}", this.position.ToCultureInvariantString(), this.heading.ToCultureInvariantString("0.0"));
	}

	// Token: 0x040046D6 RID: 18134
	public static SpawnPosition Undef = new SpawnPosition(true);

	// Token: 0x040046D7 RID: 18135
	public Vector3 position;

	// Token: 0x040046D8 RID: 18136
	public float heading;

	// Token: 0x040046D9 RID: 18137
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bInvalid;
}
