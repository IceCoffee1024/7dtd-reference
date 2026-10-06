using System;
using System.IO;

// Token: 0x02000C18 RID: 3096
public class WorldBlockTickerEntry
{
	// Token: 0x06005DF1 RID: 24049 RVA: 0x0024ADD0 File Offset: 0x00248FD0
	public WorldBlockTickerEntry(Vector3i _pos, int _id, ulong _scheduledTime)
	{
		long num = WorldBlockTickerEntry.nextTickEntryID;
		WorldBlockTickerEntry.nextTickEntryID = num + 1L;
		this.tickEntryID = num;
		this.worldPos = _pos;
		this.blockID = _id;
		this.scheduledTime = _scheduledTime;
	}

	// Token: 0x06005DF2 RID: 24050 RVA: 0x0024AE04 File Offset: 0x00249004
	public static WorldBlockTickerEntry Read(BinaryReader _br, int _chunkX, int _chunkZ, int _version)
	{
		Vector3i vector3i = new Vector3i((int)_br.ReadByte() + _chunkX * 16, (int)_br.ReadByte(), (int)_br.ReadByte() + _chunkZ * 16);
		int num = (int)_br.ReadUInt16();
		ulong num2 = _br.ReadUInt64();
		_br.ReadUInt16();
		return new WorldBlockTickerEntry(vector3i, num, num2);
	}

	// Token: 0x06005DF3 RID: 24051 RVA: 0x0024AE50 File Offset: 0x00249050
	public void Write(BinaryWriter _bw)
	{
		_bw.Write((byte)World.toBlockXZ(this.worldPos.x));
		_bw.Write((byte)this.worldPos.y);
		_bw.Write((byte)World.toBlockXZ(this.worldPos.z));
		_bw.Write((ushort)this.blockID);
		_bw.Write(this.scheduledTime);
		_bw.Write(0);
	}

	// Token: 0x06005DF4 RID: 24052 RVA: 0x0024AEC0 File Offset: 0x002490C0
	public override bool Equals(object _obj)
	{
		WorldBlockTickerEntry worldBlockTickerEntry = _obj as WorldBlockTickerEntry;
		return worldBlockTickerEntry != null && (this.worldPos.Equals(worldBlockTickerEntry.worldPos) && this.blockID == worldBlockTickerEntry.blockID) && worldBlockTickerEntry.tickEntryID == this.tickEntryID;
	}

	// Token: 0x06005DF5 RID: 24053 RVA: 0x0024AF0D File Offset: 0x0024910D
	public override int GetHashCode()
	{
		return WorldBlockTickerEntry.ToHashCode(this.worldPos, this.blockID);
	}

	// Token: 0x06005DF6 RID: 24054 RVA: 0x0024AF20 File Offset: 0x00249120
	public static int ToHashCode(Vector3i _pos, int _blockID)
	{
		return (_pos.GetHashCode() * 397) ^ _blockID;
	}

	// Token: 0x06005DF7 RID: 24055 RVA: 0x0024AF37 File Offset: 0x00249137
	public long GetChunkKey()
	{
		return WorldChunkCache.MakeChunkKey(World.toChunkXZ(this.worldPos.x), World.toChunkXZ(this.worldPos.z));
	}

	// Token: 0x040048E2 RID: 18658
	public readonly Vector3i worldPos;

	// Token: 0x040048E3 RID: 18659
	public readonly int blockID;

	// Token: 0x040048E4 RID: 18660
	public readonly ulong scheduledTime;

	// Token: 0x040048E5 RID: 18661
	[PublicizedFrom(EAccessModifier.Private)]
	public static long nextTickEntryID;

	// Token: 0x040048E6 RID: 18662
	public readonly long tickEntryID;
}
