using System;

// Token: 0x02000AA4 RID: 2724
public readonly struct GlobalSignId : IEquatable<GlobalSignId>
{
	// Token: 0x060051BD RID: 20925 RVA: 0x001F51A4 File Offset: 0x001F33A4
	public GlobalSignId(string libraryId, Guid signGuid)
	{
		if (libraryId == null)
		{
			Log.Error("GlobalSignId has been passed a null libraryId. This is unexpected and may indicate malformed data.");
		}
		this.libraryId = libraryId;
		this.signGuid = signGuid;
	}

	// Token: 0x17000902 RID: 2306
	// (get) Token: 0x060051BE RID: 20926 RVA: 0x001F51C1 File Offset: 0x001F33C1
	public static GlobalSignId InvalidId
	{
		get
		{
			return new GlobalSignId(string.Empty, Guid.Empty);
		}
	}

	// Token: 0x17000903 RID: 2307
	// (get) Token: 0x060051BF RID: 20927 RVA: 0x001F51D2 File Offset: 0x001F33D2
	public bool IsValid
	{
		get
		{
			return !string.IsNullOrEmpty(this.libraryId);
		}
	}

	// Token: 0x060051C0 RID: 20928 RVA: 0x001F51E2 File Offset: 0x001F33E2
	public bool Equals(GlobalSignId other)
	{
		return this.libraryId == other.libraryId && this.signGuid == other.signGuid;
	}

	// Token: 0x060051C1 RID: 20929 RVA: 0x001F520C File Offset: 0x001F340C
	public override bool Equals(object obj)
	{
		if (obj is GlobalSignId)
		{
			GlobalSignId globalSignId = (GlobalSignId)obj;
			return this.Equals(globalSignId);
		}
		return false;
	}

	// Token: 0x060051C2 RID: 20930 RVA: 0x001F5231 File Offset: 0x001F3431
	public override int GetHashCode()
	{
		return HashCode.Combine<string, Guid>(this.libraryId, this.signGuid);
	}

	// Token: 0x060051C3 RID: 20931 RVA: 0x001F5244 File Offset: 0x001F3444
	public override string ToString()
	{
		return string.Format("{0}:{1}", this.libraryId ?? "NULL", this.signGuid);
	}

	// Token: 0x060051C4 RID: 20932 RVA: 0x001F526A File Offset: 0x001F346A
	public static bool operator ==(GlobalSignId a, GlobalSignId b)
	{
		return a.Equals(b);
	}

	// Token: 0x060051C5 RID: 20933 RVA: 0x001F5274 File Offset: 0x001F3474
	public static bool operator !=(GlobalSignId a, GlobalSignId b)
	{
		return !a.Equals(b);
	}

	// Token: 0x060051C6 RID: 20934 RVA: 0x001F5284 File Offset: 0x001F3484
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

	// Token: 0x060051C7 RID: 20935 RVA: 0x001F52D0 File Offset: 0x001F34D0
	public unsafe void ToStream(PooledBinaryWriter _bw)
	{
		_bw.Write(this.libraryId);
		Span<byte> span = new Span<byte>(stackalloc byte[(UIntPtr)16], 16);
		this.signGuid.TryWriteBytes(span);
		_bw.Write(span);
	}

	// Token: 0x04003F04 RID: 16132
	public readonly string libraryId;

	// Token: 0x04003F05 RID: 16133
	public readonly Guid signGuid;
}
