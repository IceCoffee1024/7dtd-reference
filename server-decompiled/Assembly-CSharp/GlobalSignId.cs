using System;
using System.IO;

// Token: 0x02000A8B RID: 2699
public readonly struct GlobalSignId : IEquatable<GlobalSignId>
{
	// Token: 0x060051A5 RID: 20901 RVA: 0x001F209C File Offset: 0x001F029C
	public GlobalSignId(string libraryId, Guid signGuid)
	{
		if (libraryId == null)
		{
			Log.Error("GlobalSignId has been passed a null libraryId. This is unexpected and may indicate malformed data.");
		}
		this.libraryId = libraryId;
		this.signGuid = signGuid;
	}

	// Token: 0x170008DA RID: 2266
	// (get) Token: 0x060051A6 RID: 20902 RVA: 0x001F20B9 File Offset: 0x001F02B9
	public static GlobalSignId InvalidId
	{
		get
		{
			return new GlobalSignId(string.Empty, Guid.Empty);
		}
	}

	// Token: 0x170008DB RID: 2267
	// (get) Token: 0x060051A7 RID: 20903 RVA: 0x001F20CA File Offset: 0x001F02CA
	public bool IsValid
	{
		get
		{
			return !string.IsNullOrEmpty(this.libraryId);
		}
	}

	// Token: 0x060051A8 RID: 20904 RVA: 0x001F20DA File Offset: 0x001F02DA
	public bool Equals(GlobalSignId other)
	{
		return this.libraryId == other.libraryId && this.signGuid == other.signGuid;
	}

	// Token: 0x060051A9 RID: 20905 RVA: 0x001F2104 File Offset: 0x001F0304
	public override bool Equals(object obj)
	{
		if (obj is GlobalSignId)
		{
			GlobalSignId globalSignId = (GlobalSignId)obj;
			return this.Equals(globalSignId);
		}
		return false;
	}

	// Token: 0x060051AA RID: 20906 RVA: 0x001F2129 File Offset: 0x001F0329
	public override int GetHashCode()
	{
		return HashCode.Combine<string, Guid>(this.libraryId, this.signGuid);
	}

	// Token: 0x060051AB RID: 20907 RVA: 0x001F213C File Offset: 0x001F033C
	public override string ToString()
	{
		return string.Format("{0}:{1}", this.libraryId ?? "NULL", this.signGuid);
	}

	// Token: 0x060051AC RID: 20908 RVA: 0x001F2162 File Offset: 0x001F0362
	public static bool operator ==(GlobalSignId a, GlobalSignId b)
	{
		return a.Equals(b);
	}

	// Token: 0x060051AD RID: 20909 RVA: 0x001F216C File Offset: 0x001F036C
	public static bool operator !=(GlobalSignId a, GlobalSignId b)
	{
		return !a.Equals(b);
	}

	// Token: 0x060051AE RID: 20910 RVA: 0x001F217C File Offset: 0x001F037C
	public unsafe static GlobalSignId FromStream(BinaryReader _br)
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

	// Token: 0x060051AF RID: 20911 RVA: 0x001F21C8 File Offset: 0x001F03C8
	public unsafe void ToStream(BinaryWriter _bw)
	{
		_bw.Write(this.libraryId);
		Span<byte> span = new Span<byte>(stackalloc byte[(UIntPtr)16], 16);
		this.signGuid.TryWriteBytes(span);
		_bw.Write(span);
	}

	// Token: 0x04003EB8 RID: 16056
	public readonly string libraryId;

	// Token: 0x04003EB9 RID: 16057
	public readonly Guid signGuid;
}
