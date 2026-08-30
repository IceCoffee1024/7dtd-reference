using System;
using System.IO;

// Token: 0x02000A7A RID: 2682
public readonly struct GlobalSignId : IEquatable<GlobalSignId>
{
	// Token: 0x0600511F RID: 20767 RVA: 0x001EF604 File Offset: 0x001ED804
	public GlobalSignId(string libraryId, Guid signGuid)
	{
		if (libraryId == null)
		{
			Log.Error("GlobalSignId has been passed a null libraryId. This is unexpected and may indicate malformed data.");
		}
		this.libraryId = libraryId;
		this.signGuid = signGuid;
	}

	// Token: 0x170008C8 RID: 2248
	// (get) Token: 0x06005120 RID: 20768 RVA: 0x001EF621 File Offset: 0x001ED821
	public static GlobalSignId InvalidId
	{
		get
		{
			return new GlobalSignId(string.Empty, Guid.Empty);
		}
	}

	// Token: 0x170008C9 RID: 2249
	// (get) Token: 0x06005121 RID: 20769 RVA: 0x001EF632 File Offset: 0x001ED832
	public bool IsValid
	{
		get
		{
			return !string.IsNullOrEmpty(this.libraryId);
		}
	}

	// Token: 0x06005122 RID: 20770 RVA: 0x001EF642 File Offset: 0x001ED842
	public bool Equals(GlobalSignId other)
	{
		return this.libraryId == other.libraryId && this.signGuid == other.signGuid;
	}

	// Token: 0x06005123 RID: 20771 RVA: 0x001EF66C File Offset: 0x001ED86C
	public override bool Equals(object obj)
	{
		if (obj is GlobalSignId)
		{
			GlobalSignId globalSignId = (GlobalSignId)obj;
			return this.Equals(globalSignId);
		}
		return false;
	}

	// Token: 0x06005124 RID: 20772 RVA: 0x001EF691 File Offset: 0x001ED891
	public override int GetHashCode()
	{
		return HashCode.Combine<string, Guid>(this.libraryId, this.signGuid);
	}

	// Token: 0x06005125 RID: 20773 RVA: 0x001EF6A4 File Offset: 0x001ED8A4
	public override string ToString()
	{
		return string.Format("{0}:{1}", this.libraryId ?? "NULL", this.signGuid);
	}

	// Token: 0x06005126 RID: 20774 RVA: 0x001EF6CA File Offset: 0x001ED8CA
	public static bool operator ==(GlobalSignId a, GlobalSignId b)
	{
		return a.Equals(b);
	}

	// Token: 0x06005127 RID: 20775 RVA: 0x001EF6D4 File Offset: 0x001ED8D4
	public static bool operator !=(GlobalSignId a, GlobalSignId b)
	{
		return !a.Equals(b);
	}

	// Token: 0x06005128 RID: 20776 RVA: 0x001EF6E4 File Offset: 0x001ED8E4
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

	// Token: 0x06005129 RID: 20777 RVA: 0x001EF730 File Offset: 0x001ED930
	public unsafe void ToStream(BinaryWriter _bw)
	{
		_bw.Write(this.libraryId);
		Span<byte> span = new Span<byte>(stackalloc byte[(UIntPtr)16], 16);
		this.signGuid.TryWriteBytes(span);
		_bw.Write(span);
	}

	// Token: 0x04003E27 RID: 15911
	public readonly string libraryId;

	// Token: 0x04003E28 RID: 15912
	public readonly Guid signGuid;
}
