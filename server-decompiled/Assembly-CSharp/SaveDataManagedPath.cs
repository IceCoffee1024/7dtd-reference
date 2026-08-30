using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

// Token: 0x02000A15 RID: 2581
public class SaveDataManagedPath : IEquatable<SaveDataManagedPath>, IComparable<SaveDataManagedPath>, IComparable
{
	// Token: 0x06004D83 RID: 19843 RVA: 0x001DB36C File Offset: 0x001D956C
	public SaveDataManagedPath(StringSpan pathRelativeToRoot)
	{
		string text;
		this..ctor(SaveDataManagedPath.TryFormatPath(pathRelativeToRoot.AsSpan(), out text) ? text : pathRelativeToRoot.ToString(), true);
	}

	// Token: 0x06004D84 RID: 19844 RVA: 0x001DB3A0 File Offset: 0x001D95A0
	public SaveDataManagedPath(string pathRelativeToRoot)
		: this(pathRelativeToRoot, false)
	{
	}

	// Token: 0x06004D85 RID: 19845 RVA: 0x001DB3AC File Offset: 0x001D95AC
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

	// Token: 0x06004D86 RID: 19846 RVA: 0x001DB47C File Offset: 0x001D967C
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

	// Token: 0x17000840 RID: 2112
	// (get) Token: 0x06004D87 RID: 19847 RVA: 0x001DB569 File Offset: 0x001D9769
	public SaveDataType Type { get; }

	// Token: 0x06004D88 RID: 19848 RVA: 0x001DB574 File Offset: 0x001D9774
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

	// Token: 0x17000841 RID: 2113
	// (get) Token: 0x06004D89 RID: 19849 RVA: 0x001DB608 File Offset: 0x001D9808
	public Range SlotPathRange
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
	}

	// Token: 0x17000842 RID: 2114
	// (get) Token: 0x06004D8A RID: 19850 RVA: 0x001DB610 File Offset: 0x001D9810
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

	// Token: 0x17000843 RID: 2115
	// (get) Token: 0x06004D8B RID: 19851 RVA: 0x001DB663 File Offset: 0x001D9863
	public SaveDataSlot Slot { get; }

	// Token: 0x06004D8C RID: 19852 RVA: 0x001DB66C File Offset: 0x001D986C
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

	// Token: 0x17000844 RID: 2116
	// (get) Token: 0x06004D8D RID: 19853 RVA: 0x001DB739 File Offset: 0x001D9939
	public Range PathRelativeToSlotRange
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
	}

	// Token: 0x17000845 RID: 2117
	// (get) Token: 0x06004D8E RID: 19854 RVA: 0x001DB744 File Offset: 0x001D9944
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

	// Token: 0x06004D8F RID: 19855 RVA: 0x001DB798 File Offset: 0x001D9998
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

	// Token: 0x06004D90 RID: 19856 RVA: 0x001DB839 File Offset: 0x001D9A39
	public string GetOriginalPath()
	{
		return GameIO.GetNormalizedPath(Path.Combine(SaveDataUtils.s_saveDataRootPathPrefix, this.PathRelativeToRoot));
	}

	// Token: 0x06004D91 RID: 19857 RVA: 0x001DB850 File Offset: 0x001D9A50
	public SaveDataManagedPath GetChildPath(StringSpan childPath)
	{
		return new SaveDataManagedPath(SpanUtils.Concat(this.PathRelativeToRoot, "/", childPath));
	}

	// Token: 0x06004D92 RID: 19858 RVA: 0x001DB874 File Offset: 0x001D9A74
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

	// Token: 0x06004D93 RID: 19859 RVA: 0x001DB8C4 File Offset: 0x001D9AC4
	public bool IsParentOf(SaveDataManagedPath childPath)
	{
		return SaveDataManagedPath.<IsParentOf>g__IsParentOfInternal|28_0(this.PathRelativeToRoot, childPath.PathRelativeToRoot);
	}

	// Token: 0x06004D94 RID: 19860 RVA: 0x001DB8D7 File Offset: 0x001D9AD7
	public override string ToString()
	{
		return this.PathRelativeToRoot;
	}

	// Token: 0x06004D95 RID: 19861 RVA: 0x001DB8DF File Offset: 0x001D9ADF
	public bool Equals(SaveDataManagedPath other)
	{
		return other != null && (this == other || this.PathRelativeToRoot == other.PathRelativeToRoot);
	}

	// Token: 0x06004D96 RID: 19862 RVA: 0x001DB8FD File Offset: 0x001D9AFD
	public override bool Equals(object obj)
	{
		return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((SaveDataManagedPath)obj)));
	}

	// Token: 0x06004D97 RID: 19863 RVA: 0x001DB92B File Offset: 0x001D9B2B
	public override int GetHashCode()
	{
		return this.PathRelativeToRoot.GetHashCode();
	}

	// Token: 0x06004D98 RID: 19864 RVA: 0x001DB938 File Offset: 0x001D9B38
	public static bool operator ==(SaveDataManagedPath left, SaveDataManagedPath right)
	{
		return object.Equals(left, right);
	}

	// Token: 0x06004D99 RID: 19865 RVA: 0x001DB941 File Offset: 0x001D9B41
	public static bool operator !=(SaveDataManagedPath left, SaveDataManagedPath right)
	{
		return !object.Equals(left, right);
	}

	// Token: 0x06004D9A RID: 19866 RVA: 0x001DB94D File Offset: 0x001D9B4D
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

	// Token: 0x06004D9B RID: 19867 RVA: 0x001DB96C File Offset: 0x001D9B6C
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

	// Token: 0x06004D9C RID: 19868 RVA: 0x001DB9A0 File Offset: 0x001D9BA0
	public static bool operator <(SaveDataManagedPath left, SaveDataManagedPath right)
	{
		return Comparer<SaveDataManagedPath>.Default.Compare(left, right) < 0;
	}

	// Token: 0x06004D9D RID: 19869 RVA: 0x001DB9B1 File Offset: 0x001D9BB1
	public static bool operator >(SaveDataManagedPath left, SaveDataManagedPath right)
	{
		return Comparer<SaveDataManagedPath>.Default.Compare(left, right) > 0;
	}

	// Token: 0x06004D9E RID: 19870 RVA: 0x001DB9C2 File Offset: 0x001D9BC2
	public static bool operator <=(SaveDataManagedPath left, SaveDataManagedPath right)
	{
		return Comparer<SaveDataManagedPath>.Default.Compare(left, right) <= 0;
	}

	// Token: 0x06004D9F RID: 19871 RVA: 0x001DB9D6 File Offset: 0x001D9BD6
	public static bool operator >=(SaveDataManagedPath left, SaveDataManagedPath right)
	{
		return Comparer<SaveDataManagedPath>.Default.Compare(left, right) >= 0;
	}

	// Token: 0x06004DA1 RID: 19873 RVA: 0x001DB9FC File Offset: 0x001D9BFC
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

	// Token: 0x04003C22 RID: 15394
	public static readonly SaveDataManagedPath RootPath = new SaveDataManagedPath(string.Empty);

	// Token: 0x04003C23 RID: 15395
	public readonly string PathRelativeToRoot;
}
