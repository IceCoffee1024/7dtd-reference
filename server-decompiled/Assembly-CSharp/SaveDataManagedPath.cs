using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

// Token: 0x02000A29 RID: 2601
public class SaveDataManagedPath : IEquatable<SaveDataManagedPath>, IComparable<SaveDataManagedPath>, IComparable
{
	// Token: 0x06004D76 RID: 19830 RVA: 0x001DD704 File Offset: 0x001DB904
	public SaveDataManagedPath(StringSpan pathRelativeToRoot)
	{
		string text;
		this..ctor(SaveDataManagedPath.TryFormatPath(pathRelativeToRoot.AsSpan(), out text) ? text : pathRelativeToRoot.ToString(), true);
	}

	// Token: 0x06004D77 RID: 19831 RVA: 0x001DD738 File Offset: 0x001DB938
	public SaveDataManagedPath(string pathRelativeToRoot)
		: this(pathRelativeToRoot, false)
	{
	}

	// Token: 0x06004D78 RID: 19832 RVA: 0x001DD744 File Offset: 0x001DB944
	[PublicizedFrom(EAccessModifier.Private)]
	public SaveDataManagedPath(string pathRelativeToRoot, bool alreadyFormatted)
	{
		if (pathRelativeToRoot == null)
		{
			throw new ArgumentNullException("pathRelativeToRoot");
		}
		if (alreadyFormatted)
		{
			this.PathRelativeToRoot = pathRelativeToRoot;
		}
		else
		{
			string text;
			this.PathRelativeToRoot = (SaveDataManagedPath.TryFormatPath(pathRelativeToRoot, out text) ? text : pathRelativeToRoot);
		}
		bool flag;
		try
		{
			flag = Path.IsPathRooted(this.PathRelativeToRoot);
		}
		catch (ArgumentException ex)
		{
			throw new ArgumentException("Failed to check if path was rooted. " + this.PathRelativeToRoot, "pathRelativeToRoot", ex);
		}
		if (flag)
		{
			throw new ArgumentException("Path should not be rooted. " + this.PathRelativeToRoot, "pathRelativeToRoot");
		}
		this.Type = this.GetSaveDataType();
		this.SlotPathRange = this.GetSlotPathRange();
		this.PathRelativeToSlotRange = this.GetPathRelativeToSlotRange();
		this.Slot = new SaveDataSlot(this);
	}

	// Token: 0x06004D79 RID: 19833 RVA: 0x001DD814 File Offset: 0x001DBA14
	[PublicizedFrom(EAccessModifier.Private)]
	public unsafe static bool TryFormatPath(ReadOnlySpan<char> unformattedPath, out string formattedPath)
	{
		ReadOnlySpan<char> readOnlySpan = unformattedPath.Trim(" \\/");
		if (readOnlySpan.Length <= 0)
		{
			formattedPath = string.Empty;
			return true;
		}
		bool flag = false;
		int num = 0;
		bool flag2 = false;
		ReadOnlySpan<char> readOnlySpan2 = readOnlySpan;
		int i = 0;
		while (i < readOnlySpan2.Length)
		{
			char c = (char)(*readOnlySpan2[i]);
			if (c == '/')
			{
				goto IL_004C;
			}
			if (c == '\\')
			{
				flag = true;
				goto IL_004C;
			}
			flag2 = false;
			IL_005B:
			i++;
			continue;
			IL_004C:
			if (!flag2)
			{
				flag2 = true;
				goto IL_005B;
			}
			num++;
			goto IL_005B;
		}
		if (readOnlySpan.Length == unformattedPath.Length && !flag && num == 0)
		{
			formattedPath = null;
			return false;
		}
		fixed (char* pinnableReference = readOnlySpan.GetPinnableReference())
		{
			void* ptr = (void*)pinnableReference;
			ValueTuple<IntPtr, int> valueTuple = new ValueTuple<IntPtr, int>((IntPtr)ptr, readOnlySpan.Length);
			formattedPath = string.Create<ValueTuple<IntPtr, int>>(readOnlySpan.Length - num, valueTuple, delegate(Span<char> span, [TupleElementNames(new string[] { null, "Length" })] ValueTuple<IntPtr, int> data)
			{
				IntPtr item = data.Item1;
				int item2 = data.Item2;
				ReadOnlySpan<char> readOnlySpan3 = new ReadOnlySpan<char>(item.ToPointer(), item2);
				int num2 = 0;
				bool flag3 = false;
				ReadOnlySpan<char> readOnlySpan4 = readOnlySpan3;
				for (int j = 0; j < readOnlySpan4.Length; j++)
				{
					char c2 = (char)(*readOnlySpan4[j]);
					if (c2 == '\\' || c2 == '/')
					{
						if (!flag3)
						{
							flag3 = true;
							*span[num2++] = '/';
						}
					}
					else
					{
						flag3 = false;
						*span[num2++] = c2;
					}
				}
			});
		}
		return true;
	}

	// Token: 0x17000866 RID: 2150
	// (get) Token: 0x06004D7A RID: 19834 RVA: 0x001DD901 File Offset: 0x001DBB01
	public SaveDataType Type { get; }

	// Token: 0x06004D7B RID: 19835 RVA: 0x001DD90C File Offset: 0x001DBB0C
	[PublicizedFrom(EAccessModifier.Private)]
	public SaveDataType GetSaveDataType()
	{
		foreach (SaveDataType saveDataType in EnumUtils.Values<SaveDataType>())
		{
			if (!saveDataType.IsRoot())
			{
				string pathRaw = saveDataType.GetPathRaw();
				if (this.PathRelativeToRoot.IndexOf(pathRaw, StringComparison.Ordinal) == 0 && this.PathRelativeToRoot.Length >= pathRaw.Length + 2 && this.PathRelativeToRoot[pathRaw.Length] == '/')
				{
					return saveDataType;
				}
			}
		}
		return SaveDataType.User;
	}

	// Token: 0x17000867 RID: 2151
	// (get) Token: 0x06004D7C RID: 19836 RVA: 0x001DD9A0 File Offset: 0x001DBBA0
	public Range SlotPathRange
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
	}

	// Token: 0x17000868 RID: 2152
	// (get) Token: 0x06004D7D RID: 19837 RVA: 0x001DD9A8 File Offset: 0x001DBBA8
	public StringSpan SlotPath
	{
		get
		{
			string pathRelativeToRoot = this.PathRelativeToRoot;
			Range slotPathRange = this.SlotPathRange;
			int length = pathRelativeToRoot.Length;
			int offset = slotPathRange.Start.GetOffset(length);
			int num = slotPathRange.End.GetOffset(length) - offset;
			return pathRelativeToRoot.Substring(offset, num);
		}
	}

	// Token: 0x17000869 RID: 2153
	// (get) Token: 0x06004D7E RID: 19838 RVA: 0x001DD9FB File Offset: 0x001DBBFB
	public SaveDataSlot Slot { get; }

	// Token: 0x06004D7F RID: 19839 RVA: 0x001DDA04 File Offset: 0x001DBC04
	[PublicizedFrom(EAccessModifier.Private)]
	public Range GetSlotPathRange()
	{
		int num = this.Type.GetSlotPathDepth();
		string pathRaw = this.Type.GetPathRaw();
		if (num <= 0 || pathRaw.Length == 0 || this.PathRelativeToRoot.Length < pathRaw.Length + 2)
		{
			return new Range(pathRaw.Length, pathRaw.Length);
		}
		for (int i = pathRaw.Length + 1; i < this.PathRelativeToRoot.Length; i++)
		{
			if (this.PathRelativeToRoot[i] == '/')
			{
				num--;
				if (num <= 0)
				{
					int num2 = pathRaw.Length + 1;
					int num3 = i;
					return new Range(num2, num3);
				}
			}
		}
		return new Range(pathRaw.Length, pathRaw.Length);
	}

	// Token: 0x1700086A RID: 2154
	// (get) Token: 0x06004D80 RID: 19840 RVA: 0x001DDAD1 File Offset: 0x001DBCD1
	public Range PathRelativeToSlotRange
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
	}

	// Token: 0x1700086B RID: 2155
	// (get) Token: 0x06004D81 RID: 19841 RVA: 0x001DDADC File Offset: 0x001DBCDC
	public StringSpan PathRelativeToSlot
	{
		get
		{
			string pathRelativeToRoot = this.PathRelativeToRoot;
			Range pathRelativeToSlotRange = this.PathRelativeToSlotRange;
			int length = pathRelativeToRoot.Length;
			int offset = pathRelativeToSlotRange.Start.GetOffset(length);
			int num = pathRelativeToSlotRange.End.GetOffset(length) - offset;
			return pathRelativeToRoot.Substring(offset, num);
		}
	}

	// Token: 0x06004D82 RID: 19842 RVA: 0x001DDB30 File Offset: 0x001DBD30
	[PublicizedFrom(EAccessModifier.Private)]
	public Range GetPathRelativeToSlotRange()
	{
		int offset = this.SlotPathRange.End.GetOffset(this.PathRelativeToRoot.Length);
		if (offset >= this.PathRelativeToRoot.Length)
		{
			return new Range(offset, offset);
		}
		int num = ((this.PathRelativeToRoot[offset] == '/') ? (offset + 1) : offset);
		if (num >= this.PathRelativeToRoot.Length)
		{
			return new Range(offset, offset);
		}
		return new Range(num, this.PathRelativeToRoot.Length);
	}

	// Token: 0x06004D83 RID: 19843 RVA: 0x001DDBD1 File Offset: 0x001DBDD1
	public string GetOriginalPath()
	{
		return GameIO.GetNormalizedPath(Path.Combine(SaveDataUtils.s_saveDataRootPathPrefix, this.PathRelativeToRoot));
	}

	// Token: 0x06004D84 RID: 19844 RVA: 0x001DDBE8 File Offset: 0x001DBDE8
	public SaveDataManagedPath GetChildPath(StringSpan childPath)
	{
		return new SaveDataManagedPath(SpanUtils.Concat(this.PathRelativeToRoot, "/", childPath));
	}

	// Token: 0x06004D85 RID: 19845 RVA: 0x001DDC0C File Offset: 0x001DBE0C
	public bool TryGetParentPath(out SaveDataManagedPath parentPath)
	{
		if (this.PathRelativeToRoot.Length <= 0)
		{
			parentPath = null;
			return false;
		}
		int num = this.PathRelativeToRoot.LastIndexOf('/');
		if (num < 0)
		{
			parentPath = SaveDataManagedPath.RootPath;
			return true;
		}
		parentPath = new SaveDataManagedPath(this.PathRelativeToRoot.Substring(0, num));
		return true;
	}

	// Token: 0x06004D86 RID: 19846 RVA: 0x001DDC5C File Offset: 0x001DBE5C
	public bool IsParentOf(SaveDataManagedPath childPath)
	{
		return SaveDataManagedPath.<IsParentOf>g__IsParentOfInternal|28_0(this.PathRelativeToRoot, childPath.PathRelativeToRoot);
	}

	// Token: 0x06004D87 RID: 19847 RVA: 0x001DDC6F File Offset: 0x001DBE6F
	public override string ToString()
	{
		return this.PathRelativeToRoot;
	}

	// Token: 0x06004D88 RID: 19848 RVA: 0x001DDC77 File Offset: 0x001DBE77
	public bool Equals(SaveDataManagedPath other)
	{
		return other != null && (this == other || this.PathRelativeToRoot == other.PathRelativeToRoot);
	}

	// Token: 0x06004D89 RID: 19849 RVA: 0x001DDC95 File Offset: 0x001DBE95
	public override bool Equals(object obj)
	{
		return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((SaveDataManagedPath)obj)));
	}

	// Token: 0x06004D8A RID: 19850 RVA: 0x001DDCC3 File Offset: 0x001DBEC3
	public override int GetHashCode()
	{
		return this.PathRelativeToRoot.GetHashCode();
	}

	// Token: 0x06004D8B RID: 19851 RVA: 0x001DDCD0 File Offset: 0x001DBED0
	public static bool operator ==(SaveDataManagedPath left, SaveDataManagedPath right)
	{
		return object.Equals(left, right);
	}

	// Token: 0x06004D8C RID: 19852 RVA: 0x001DDCD9 File Offset: 0x001DBED9
	public static bool operator !=(SaveDataManagedPath left, SaveDataManagedPath right)
	{
		return !object.Equals(left, right);
	}

	// Token: 0x06004D8D RID: 19853 RVA: 0x001DDCE5 File Offset: 0x001DBEE5
	public int CompareTo(SaveDataManagedPath other)
	{
		if (other == null)
		{
			return 1;
		}
		if (this == other)
		{
			return 0;
		}
		return string.Compare(this.PathRelativeToRoot, other.PathRelativeToRoot, StringComparison.Ordinal);
	}

	// Token: 0x06004D8E RID: 19854 RVA: 0x001DDD04 File Offset: 0x001DBF04
	public int CompareTo(object obj)
	{
		if (obj == null)
		{
			return 1;
		}
		if (this == obj)
		{
			return 0;
		}
		SaveDataManagedPath saveDataManagedPath = obj as SaveDataManagedPath;
		if (saveDataManagedPath == null)
		{
			throw new ArgumentException("Object must be of type SaveDataManagedPath");
		}
		return this.CompareTo(saveDataManagedPath);
	}

	// Token: 0x06004D8F RID: 19855 RVA: 0x001DDD38 File Offset: 0x001DBF38
	public static bool operator <(SaveDataManagedPath left, SaveDataManagedPath right)
	{
		return Comparer<SaveDataManagedPath>.Default.Compare(left, right) < 0;
	}

	// Token: 0x06004D90 RID: 19856 RVA: 0x001DDD49 File Offset: 0x001DBF49
	public static bool operator >(SaveDataManagedPath left, SaveDataManagedPath right)
	{
		return Comparer<SaveDataManagedPath>.Default.Compare(left, right) > 0;
	}

	// Token: 0x06004D91 RID: 19857 RVA: 0x001DDD5A File Offset: 0x001DBF5A
	public static bool operator <=(SaveDataManagedPath left, SaveDataManagedPath right)
	{
		return Comparer<SaveDataManagedPath>.Default.Compare(left, right) <= 0;
	}

	// Token: 0x06004D92 RID: 19858 RVA: 0x001DDD6E File Offset: 0x001DBF6E
	public static bool operator >=(SaveDataManagedPath left, SaveDataManagedPath right)
	{
		return Comparer<SaveDataManagedPath>.Default.Compare(left, right) >= 0;
	}

	// Token: 0x06004D94 RID: 19860 RVA: 0x001DDD94 File Offset: 0x001DBF94
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static bool <IsParentOf>g__IsParentOfInternal|28_0(string parent, string child)
	{
		if (parent.Length >= child.Length)
		{
			return false;
		}
		if (parent.Length == 0)
		{
			return true;
		}
		if (child[parent.Length] != '/')
		{
			return false;
		}
		for (int i = 0; i < parent.Length; i++)
		{
			if (parent[i] != child[i])
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x04003C50 RID: 15440
	public static readonly SaveDataManagedPath RootPath = new SaveDataManagedPath(string.Empty);

	// Token: 0x04003C51 RID: 15441
	public readonly string PathRelativeToRoot;
}
