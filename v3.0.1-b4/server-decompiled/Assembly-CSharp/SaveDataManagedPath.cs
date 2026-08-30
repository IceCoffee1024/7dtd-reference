using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

// Token: 0x02000A04 RID: 2564
public class SaveDataManagedPath : IEquatable<SaveDataManagedPath>, IComparable<SaveDataManagedPath>, IComparable
{
	// Token: 0x06004CFD RID: 19709 RVA: 0x001D88D4 File Offset: 0x001D6AD4
	public SaveDataManagedPath(StringSpan pathRelativeToRoot)
	{
		string text;
		this..ctor(SaveDataManagedPath.TryFormatPath(pathRelativeToRoot.AsSpan(), out text) ? text : pathRelativeToRoot.ToString(), true);
	}

	// Token: 0x06004CFE RID: 19710 RVA: 0x001D8908 File Offset: 0x001D6B08
	public SaveDataManagedPath(string pathRelativeToRoot)
		: this(pathRelativeToRoot, false)
	{
	}

	// Token: 0x06004CFF RID: 19711 RVA: 0x001D8914 File Offset: 0x001D6B14
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

	// Token: 0x06004D00 RID: 19712 RVA: 0x001D89E4 File Offset: 0x001D6BE4
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

	// Token: 0x1700082E RID: 2094
	// (get) Token: 0x06004D01 RID: 19713 RVA: 0x001D8AD1 File Offset: 0x001D6CD1
	public SaveDataType Type { get; }

	// Token: 0x06004D02 RID: 19714 RVA: 0x001D8ADC File Offset: 0x001D6CDC
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

	// Token: 0x1700082F RID: 2095
	// (get) Token: 0x06004D03 RID: 19715 RVA: 0x001D8B70 File Offset: 0x001D6D70
	public Range SlotPathRange
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
	}

	// Token: 0x17000830 RID: 2096
	// (get) Token: 0x06004D04 RID: 19716 RVA: 0x001D8B78 File Offset: 0x001D6D78
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

	// Token: 0x17000831 RID: 2097
	// (get) Token: 0x06004D05 RID: 19717 RVA: 0x001D8BCB File Offset: 0x001D6DCB
	public SaveDataSlot Slot { get; }

	// Token: 0x06004D06 RID: 19718 RVA: 0x001D8BD4 File Offset: 0x001D6DD4
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

	// Token: 0x17000832 RID: 2098
	// (get) Token: 0x06004D07 RID: 19719 RVA: 0x001D8CA1 File Offset: 0x001D6EA1
	public Range PathRelativeToSlotRange
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
	}

	// Token: 0x17000833 RID: 2099
	// (get) Token: 0x06004D08 RID: 19720 RVA: 0x001D8CAC File Offset: 0x001D6EAC
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

	// Token: 0x06004D09 RID: 19721 RVA: 0x001D8D00 File Offset: 0x001D6F00
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

	// Token: 0x06004D0A RID: 19722 RVA: 0x001D8DA1 File Offset: 0x001D6FA1
	public string GetOriginalPath()
	{
		return GameIO.GetNormalizedPath(Path.Combine(SaveDataUtils.s_saveDataRootPathPrefix, this.PathRelativeToRoot));
	}

	// Token: 0x06004D0B RID: 19723 RVA: 0x001D8DB8 File Offset: 0x001D6FB8
	public SaveDataManagedPath GetChildPath(StringSpan childPath)
	{
		return new SaveDataManagedPath(SpanUtils.Concat(this.PathRelativeToRoot, "/", childPath));
	}

	// Token: 0x06004D0C RID: 19724 RVA: 0x001D8DDC File Offset: 0x001D6FDC
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

	// Token: 0x06004D0D RID: 19725 RVA: 0x001D8E2C File Offset: 0x001D702C
	public bool IsParentOf(SaveDataManagedPath childPath)
	{
		return SaveDataManagedPath.<IsParentOf>g__IsParentOfInternal|28_0(this.PathRelativeToRoot, childPath.PathRelativeToRoot);
	}

	// Token: 0x06004D0E RID: 19726 RVA: 0x001D8E3F File Offset: 0x001D703F
	public override string ToString()
	{
		return this.PathRelativeToRoot;
	}

	// Token: 0x06004D0F RID: 19727 RVA: 0x001D8E47 File Offset: 0x001D7047
	public bool Equals(SaveDataManagedPath other)
	{
		return other != null && (this == other || this.PathRelativeToRoot == other.PathRelativeToRoot);
	}

	// Token: 0x06004D10 RID: 19728 RVA: 0x001D8E65 File Offset: 0x001D7065
	public override bool Equals(object obj)
	{
		return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((SaveDataManagedPath)obj)));
	}

	// Token: 0x06004D11 RID: 19729 RVA: 0x001D8E93 File Offset: 0x001D7093
	public override int GetHashCode()
	{
		return this.PathRelativeToRoot.GetHashCode();
	}

	// Token: 0x06004D12 RID: 19730 RVA: 0x001D8EA0 File Offset: 0x001D70A0
	public static bool operator ==(SaveDataManagedPath left, SaveDataManagedPath right)
	{
		return object.Equals(left, right);
	}

	// Token: 0x06004D13 RID: 19731 RVA: 0x001D8EA9 File Offset: 0x001D70A9
	public static bool operator !=(SaveDataManagedPath left, SaveDataManagedPath right)
	{
		return !object.Equals(left, right);
	}

	// Token: 0x06004D14 RID: 19732 RVA: 0x001D8EB5 File Offset: 0x001D70B5
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

	// Token: 0x06004D15 RID: 19733 RVA: 0x001D8ED4 File Offset: 0x001D70D4
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

	// Token: 0x06004D16 RID: 19734 RVA: 0x001D8F08 File Offset: 0x001D7108
	public static bool operator <(SaveDataManagedPath left, SaveDataManagedPath right)
	{
		return Comparer<SaveDataManagedPath>.Default.Compare(left, right) < 0;
	}

	// Token: 0x06004D17 RID: 19735 RVA: 0x001D8F19 File Offset: 0x001D7119
	public static bool operator >(SaveDataManagedPath left, SaveDataManagedPath right)
	{
		return Comparer<SaveDataManagedPath>.Default.Compare(left, right) > 0;
	}

	// Token: 0x06004D18 RID: 19736 RVA: 0x001D8F2A File Offset: 0x001D712A
	public static bool operator <=(SaveDataManagedPath left, SaveDataManagedPath right)
	{
		return Comparer<SaveDataManagedPath>.Default.Compare(left, right) <= 0;
	}

	// Token: 0x06004D19 RID: 19737 RVA: 0x001D8F3E File Offset: 0x001D713E
	public static bool operator >=(SaveDataManagedPath left, SaveDataManagedPath right)
	{
		return Comparer<SaveDataManagedPath>.Default.Compare(left, right) >= 0;
	}

	// Token: 0x06004D1B RID: 19739 RVA: 0x001D8F64 File Offset: 0x001D7164
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

	// Token: 0x04003B91 RID: 15249
	public static readonly SaveDataManagedPath RootPath = new SaveDataManagedPath(string.Empty);

	// Token: 0x04003B92 RID: 15250
	public readonly string PathRelativeToRoot;
}
