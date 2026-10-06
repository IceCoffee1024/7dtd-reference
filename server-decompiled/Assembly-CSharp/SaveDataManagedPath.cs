using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

// Token: 0x02000A1F RID: 2591
public class SaveDataManagedPath : IEquatable<SaveDataManagedPath>, IComparable<SaveDataManagedPath>, IComparable
{
	// Token: 0x06004D52 RID: 19794 RVA: 0x001DC61C File Offset: 0x001DA81C
	public SaveDataManagedPath(StringSpan pathRelativeToRoot)
	{
		string text;
		this..ctor(SaveDataManagedPath.TryFormatPath(pathRelativeToRoot.AsSpan(), out text) ? text : pathRelativeToRoot.ToString(), true);
	}

	// Token: 0x06004D53 RID: 19795 RVA: 0x001DC650 File Offset: 0x001DA850
	public SaveDataManagedPath(string pathRelativeToRoot)
		: this(pathRelativeToRoot, false)
	{
	}

	// Token: 0x06004D54 RID: 19796 RVA: 0x001DC65C File Offset: 0x001DA85C
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

	// Token: 0x06004D55 RID: 19797 RVA: 0x001DC72C File Offset: 0x001DA92C
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

	// Token: 0x17000860 RID: 2144
	// (get) Token: 0x06004D56 RID: 19798 RVA: 0x001DC819 File Offset: 0x001DAA19
	public SaveDataType Type { get; }

	// Token: 0x06004D57 RID: 19799 RVA: 0x001DC824 File Offset: 0x001DAA24
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

	// Token: 0x17000861 RID: 2145
	// (get) Token: 0x06004D58 RID: 19800 RVA: 0x001DC8B8 File Offset: 0x001DAAB8
	public Range SlotPathRange
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
	}

	// Token: 0x17000862 RID: 2146
	// (get) Token: 0x06004D59 RID: 19801 RVA: 0x001DC8C0 File Offset: 0x001DAAC0
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

	// Token: 0x17000863 RID: 2147
	// (get) Token: 0x06004D5A RID: 19802 RVA: 0x001DC913 File Offset: 0x001DAB13
	public SaveDataSlot Slot { get; }

	// Token: 0x06004D5B RID: 19803 RVA: 0x001DC91C File Offset: 0x001DAB1C
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

	// Token: 0x17000864 RID: 2148
	// (get) Token: 0x06004D5C RID: 19804 RVA: 0x001DC9E9 File Offset: 0x001DABE9
	public Range PathRelativeToSlotRange
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
	}

	// Token: 0x17000865 RID: 2149
	// (get) Token: 0x06004D5D RID: 19805 RVA: 0x001DC9F4 File Offset: 0x001DABF4
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

	// Token: 0x06004D5E RID: 19806 RVA: 0x001DCA48 File Offset: 0x001DAC48
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

	// Token: 0x06004D5F RID: 19807 RVA: 0x001DCAE9 File Offset: 0x001DACE9
	public string GetOriginalPath()
	{
		return GameIO.GetNormalizedPath(Path.Combine(SaveDataUtils.s_saveDataRootPathPrefix, this.PathRelativeToRoot));
	}

	// Token: 0x06004D60 RID: 19808 RVA: 0x001DCB00 File Offset: 0x001DAD00
	public SaveDataManagedPath GetChildPath(StringSpan childPath)
	{
		return new SaveDataManagedPath(SpanUtils.Concat(this.PathRelativeToRoot, "/", childPath));
	}

	// Token: 0x06004D61 RID: 19809 RVA: 0x001DCB24 File Offset: 0x001DAD24
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

	// Token: 0x06004D62 RID: 19810 RVA: 0x001DCB74 File Offset: 0x001DAD74
	public bool IsParentOf(SaveDataManagedPath childPath)
	{
		return SaveDataManagedPath.<IsParentOf>g__IsParentOfInternal|28_0(this.PathRelativeToRoot, childPath.PathRelativeToRoot);
	}

	// Token: 0x06004D63 RID: 19811 RVA: 0x001DCB87 File Offset: 0x001DAD87
	public override string ToString()
	{
		return this.PathRelativeToRoot;
	}

	// Token: 0x06004D64 RID: 19812 RVA: 0x001DCB8F File Offset: 0x001DAD8F
	public bool Equals(SaveDataManagedPath other)
	{
		return other != null && (this == other || this.PathRelativeToRoot == other.PathRelativeToRoot);
	}

	// Token: 0x06004D65 RID: 19813 RVA: 0x001DCBAD File Offset: 0x001DADAD
	public override bool Equals(object obj)
	{
		return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((SaveDataManagedPath)obj)));
	}

	// Token: 0x06004D66 RID: 19814 RVA: 0x001DCBDB File Offset: 0x001DADDB
	public override int GetHashCode()
	{
		return this.PathRelativeToRoot.GetHashCode();
	}

	// Token: 0x06004D67 RID: 19815 RVA: 0x001DCBE8 File Offset: 0x001DADE8
	public static bool operator ==(SaveDataManagedPath left, SaveDataManagedPath right)
	{
		return object.Equals(left, right);
	}

	// Token: 0x06004D68 RID: 19816 RVA: 0x001DCBF1 File Offset: 0x001DADF1
	public static bool operator !=(SaveDataManagedPath left, SaveDataManagedPath right)
	{
		return !object.Equals(left, right);
	}

	// Token: 0x06004D69 RID: 19817 RVA: 0x001DCBFD File Offset: 0x001DADFD
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

	// Token: 0x06004D6A RID: 19818 RVA: 0x001DCC1C File Offset: 0x001DAE1C
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

	// Token: 0x06004D6B RID: 19819 RVA: 0x001DCC50 File Offset: 0x001DAE50
	public static bool operator <(SaveDataManagedPath left, SaveDataManagedPath right)
	{
		return Comparer<SaveDataManagedPath>.Default.Compare(left, right) < 0;
	}

	// Token: 0x06004D6C RID: 19820 RVA: 0x001DCC61 File Offset: 0x001DAE61
	public static bool operator >(SaveDataManagedPath left, SaveDataManagedPath right)
	{
		return Comparer<SaveDataManagedPath>.Default.Compare(left, right) > 0;
	}

	// Token: 0x06004D6D RID: 19821 RVA: 0x001DCC72 File Offset: 0x001DAE72
	public static bool operator <=(SaveDataManagedPath left, SaveDataManagedPath right)
	{
		return Comparer<SaveDataManagedPath>.Default.Compare(left, right) <= 0;
	}

	// Token: 0x06004D6E RID: 19822 RVA: 0x001DCC86 File Offset: 0x001DAE86
	public static bool operator >=(SaveDataManagedPath left, SaveDataManagedPath right)
	{
		return Comparer<SaveDataManagedPath>.Default.Compare(left, right) >= 0;
	}

	// Token: 0x06004D70 RID: 19824 RVA: 0x001DCCAC File Offset: 0x001DAEAC
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

	// Token: 0x04003C2F RID: 15407
	public static readonly SaveDataManagedPath RootPath = new SaveDataManagedPath(string.Empty);

	// Token: 0x04003C30 RID: 15408
	public readonly string PathRelativeToRoot;
}
