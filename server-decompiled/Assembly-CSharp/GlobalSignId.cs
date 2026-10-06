using System;

// Token: 0x02000A9A RID: 2714
public readonly struct GlobalSignId : IEquatable<GlobalSignId>
{
	// Token: 0x06005199 RID: 20889 RVA: 0x001F40C8 File Offset: 0x001F22C8
	public GlobalSignId(string libraryId, Guid signGuid)
	{
		if (libraryId == null)
		{
			Log.Error("GlobalSignId has been passed a null libraryId. This is unexpected and may indicate malformed data.");
		}
		this.libraryId = libraryId;
		this.signGuid = signGuid;
	}

	// Token: 0x170008FC RID: 2300
	// (get) Token: 0x0600519A RID: 20890 RVA: 0x001F40E5 File Offset: 0x001F22E5
	public static GlobalSignId InvalidId
	{
		get
		{
			return new GlobalSignId(string.Empty, Guid.Empty);
		}
	}

	// Token: 0x170008FD RID: 2301
	// (get) Token: 0x0600519B RID: 20891 RVA: 0x001F40F6 File Offset: 0x001F22F6
	public bool IsValid
	{
		get
		{
			return !string.IsNullOrEmpty(this.libraryId);
		}
	}

	// Token: 0x0600519C RID: 20892 RVA: 0x001F4106 File Offset: 0x001F2306
	public bool Equals(GlobalSignId other)
	{
		return this.libraryId == other.libraryId && this.signGuid == other.signGuid;
	}

	// Token: 0x0600519D RID: 20893 RVA: 0x001F4130 File Offset: 0x001F2330
	public override bool Equals(object obj)
	{
		if (obj is GlobalSignId)
		{
			GlobalSignId globalSignId = (GlobalSignId)obj;
			return this.Equals(globalSignId);
		}
		return false;
	}

	// Token: 0x0600519E RID: 20894 RVA: 0x001F4155 File Offset: 0x001F2355
	public override int GetHashCode()
	{
		return HashCode.Combine<string, Guid>(this.libraryId, this.signGuid);
	}

	// Token: 0x0600519F RID: 20895 RVA: 0x001F4168 File Offset: 0x001F2368
	public override string ToString()
	{
		return string.Format("{0}:{1}", this.libraryId ?? "NULL", this.signGuid);
	}

	// Token: 0x060051A0 RID: 20896 RVA: 0x001F418E File Offset: 0x001F238E
	public static bool operator ==(GlobalSignId a, GlobalSignId b)
	{
		return a.Equals(b);
	}

	// Token: 0x060051A1 RID: 20897 RVA: 0x001F4198 File Offset: 0x001F2398
	public static bool operator !=(GlobalSignId a, GlobalSignId b)
	{
		return !a.Equals(b);
	}

	// Token: 0x060051A2 RID: 20898 RVA: 0x001F41A8 File Offset: 0x001F23A8
	public unsafe static GlobalSignId FromStream(PooledBinaryReader _br)
	{
		string text = _br.ReadString();
		Span<byte> span = new Span<byte>(stackalloc byte[(UIntPtr)16], 16);
		if (_br.Read(span) != 16)
		{
			Log.Error("Invalid Guid data.");
		}
		Guid guid = new Guid(span);
		return new GlobalSignId(text, guid);
	}

	// Token: 0x060051A3 RID: 20899 RVA: 0x001F41F4 File Offset: 0x001F23F4
	public unsafe void ToStream(PooledBinaryWriter _bw)
	{
		_bw.Write(this.libraryId);
		Span<byte> span = new Span<byte>(stackalloc byte[(UIntPtr)16], 16);
		this.signGuid.TryWriteBytes(span);
		_bw.Write(span);
	}

	// Token: 0x04003EE3 RID: 16099
	public readonly string libraryId;

	// Token: 0x04003EE4 RID: 16100
	public readonly Guid signGuid;
}
