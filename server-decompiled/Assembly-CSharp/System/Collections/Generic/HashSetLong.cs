using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Collections.Generic
{
	// Token: 0x020016E4 RID: 5860
	[DebuggerDisplay("Count={Count}")]
	[Serializable]
	public class HashSetLong : ICollection<long>, IEnumerable<long>, IEnumerable, ISerializable, IDeserializationCallback
	{
		// Token: 0x170016CB RID: 5835
		// (get) Token: 0x0600B713 RID: 46867 RVA: 0x0043D66B File Offset: 0x0043B86B
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x0600B714 RID: 46868 RVA: 0x0043D673 File Offset: 0x0043B873
		public HashSetLong()
		{
			this.Init(10, null);
		}

		// Token: 0x0600B715 RID: 46869 RVA: 0x0043D684 File Offset: 0x0043B884
		public HashSetLong(IEqualityComparer<long> comparer)
		{
			this.Init(10, comparer);
		}

		// Token: 0x0600B716 RID: 46870 RVA: 0x0043D695 File Offset: 0x0043B895
		public HashSetLong(IEnumerable<long> collection)
			: this(collection, null)
		{
		}

		// Token: 0x0600B717 RID: 46871 RVA: 0x0043D6A0 File Offset: 0x0043B8A0
		public HashSetLong(IEnumerable<long> collection, IEqualityComparer<long> comparer)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			int num = 0;
			ICollection<long> collection2 = collection as ICollection<long>;
			if (collection2 != null)
			{
				num = collection2.Count;
			}
			this.Init(num, comparer);
			foreach (long num2 in collection)
			{
				this.Add(num2);
			}
		}

		// Token: 0x0600B718 RID: 46872 RVA: 0x0043D718 File Offset: 0x0043B918
		[PublicizedFrom(EAccessModifier.Protected)]
		public HashSetLong(SerializationInfo info, StreamingContext context)
		{
			this.si = info;
		}

		// Token: 0x0600B719 RID: 46873 RVA: 0x0043D728 File Offset: 0x0043B928
		[PublicizedFrom(EAccessModifier.Private)]
		public void Init(int capacity, IEqualityComparer<long> comparer)
		{
			if (capacity < 0)
			{
				throw new ArgumentOutOfRangeException("capacity");
			}
			this.comparer = comparer ?? EqualityComparer<long>.Default;
			if (capacity == 0)
			{
				capacity = 10;
			}
			capacity = (int)((float)capacity / 0.9f) + 1;
			this.InitArrays(capacity);
			this.generation = 0;
		}

		// Token: 0x0600B71A RID: 46874 RVA: 0x0043D778 File Offset: 0x0043B978
		[PublicizedFrom(EAccessModifier.Private)]
		public void InitArrays(int size)
		{
			this.table = new int[size];
			this.links = new HashSetLong.Link[size];
			this.empty_slot = -1;
			this.slots = new long[size];
			this.touched = 0;
			this.threshold = (int)((float)this.table.Length * 0.9f);
			if (this.threshold == 0 && this.table.Length != 0)
			{
				this.threshold = 1;
			}
		}

		// Token: 0x0600B71B RID: 46875 RVA: 0x0043D7E8 File Offset: 0x0043B9E8
		[PublicizedFrom(EAccessModifier.Private)]
		public bool SlotsContainsAt(int index, int hash, long item)
		{
			HashSetLong.Link link;
			for (int num = this.table[index] - 1; num != -1; num = link.Next)
			{
				link = this.links[num];
				if (link.HashCode == hash && item == this.slots[num])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600B71C RID: 46876 RVA: 0x0043D831 File Offset: 0x0043BA31
		public void CopyTo(long[] array)
		{
			this.CopyTo(array, 0, this.count);
		}

		// Token: 0x0600B71D RID: 46877 RVA: 0x0043D841 File Offset: 0x0043BA41
		public void CopyTo(long[] array, int arrayIndex)
		{
			this.CopyTo(array, arrayIndex, this.count);
		}

		// Token: 0x0600B71E RID: 46878 RVA: 0x0043D854 File Offset: 0x0043BA54
		public void CopyTo(long[] array, int arrayIndex, int count)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (arrayIndex < 0)
			{
				throw new ArgumentOutOfRangeException("arrayIndex");
			}
			if (arrayIndex > array.Length)
			{
				throw new ArgumentException("index larger than largest valid index of array");
			}
			if (array.Length - arrayIndex < count)
			{
				throw new ArgumentException("Destination array cannot hold the requested elements!");
			}
			int num = 0;
			int num2 = 0;
			while (num < this.touched && num2 < count)
			{
				if (this.GetLinkHashCode(num) != 0)
				{
					array[arrayIndex++] = this.slots[num];
				}
				num++;
			}
		}

		// Token: 0x0600B71F RID: 46879 RVA: 0x0043D8D4 File Offset: 0x0043BAD4
		[PublicizedFrom(EAccessModifier.Private)]
		public void Resize()
		{
			int num = HashSetLong.PrimeHelper.ToPrime((this.table.Length << 1) | 1);
			int[] array = new int[num];
			HashSetLong.Link[] array2 = new HashSetLong.Link[num];
			for (int i = 0; i < this.table.Length; i++)
			{
				for (int num2 = this.table[i] - 1; num2 != -1; num2 = this.links[num2].Next)
				{
					int num3 = ((array2[num2].HashCode = ((int)this.slots[num2] ^ (int)(this.slots[num2] >> 32)) | int.MinValue) & int.MaxValue) % num;
					array2[num2].Next = array[num3] - 1;
					array[num3] = num2 + 1;
				}
			}
			this.table = array;
			this.links = array2;
			long[] array3 = new long[num];
			Array.Copy(this.slots, 0, array3, 0, this.touched);
			this.slots = array3;
			this.threshold = (int)((float)num * 0.9f);
		}

		// Token: 0x0600B720 RID: 46880 RVA: 0x0043D9DA File Offset: 0x0043BBDA
		[PublicizedFrom(EAccessModifier.Private)]
		public int GetLinkHashCode(int index)
		{
			return this.links[index].HashCode & int.MinValue;
		}

		// Token: 0x0600B721 RID: 46881 RVA: 0x0043D9F4 File Offset: 0x0043BBF4
		public bool Add(long item)
		{
			int num = ((int)item ^ (int)(item >> 32)) | int.MinValue;
			int num2 = (num & int.MaxValue) % this.table.Length;
			if (this.SlotsContainsAt(num2, num, item))
			{
				return false;
			}
			int num3 = this.count + 1;
			this.count = num3;
			if (num3 > this.threshold)
			{
				this.Resize();
				num2 = (num & int.MaxValue) % this.table.Length;
			}
			int num4 = this.empty_slot;
			if (num4 == -1)
			{
				num3 = this.touched;
				this.touched = num3 + 1;
				num4 = num3;
			}
			else
			{
				this.empty_slot = this.links[num4].Next;
			}
			this.links[num4].HashCode = num;
			this.links[num4].Next = this.table[num2] - 1;
			this.table[num2] = num4 + 1;
			this.slots[num4] = item;
			this.generation++;
			return true;
		}

		// Token: 0x170016CC RID: 5836
		// (get) Token: 0x0600B722 RID: 46882 RVA: 0x0043DAE4 File Offset: 0x0043BCE4
		public IEqualityComparer<long> Comparer
		{
			get
			{
				return this.comparer;
			}
		}

		// Token: 0x0600B723 RID: 46883 RVA: 0x0043DAEC File Offset: 0x0043BCEC
		public void Clear()
		{
			this.count = 0;
			Array.Clear(this.table, 0, this.table.Length);
			Array.Clear(this.slots, 0, this.slots.Length);
			Array.Clear(this.links, 0, this.links.Length);
			this.empty_slot = -1;
			this.touched = 0;
			this.generation++;
		}

		// Token: 0x0600B724 RID: 46884 RVA: 0x0043DB58 File Offset: 0x0043BD58
		public bool Contains(long item)
		{
			int num = ((int)item ^ (int)(item >> 32)) | int.MinValue;
			int num2 = (num & int.MaxValue) % this.table.Length;
			return this.SlotsContainsAt(num2, num, item);
		}

		// Token: 0x0600B725 RID: 46885 RVA: 0x0043DB90 File Offset: 0x0043BD90
		public bool Remove(long item)
		{
			int num = ((int)item ^ (int)(item >> 32)) | int.MinValue;
			int num2 = (num & int.MaxValue) % this.table.Length;
			int num3 = this.table[num2] - 1;
			if (num3 == -1)
			{
				return false;
			}
			int num4 = -1;
			do
			{
				HashSetLong.Link link = this.links[num3];
				if (link.HashCode == num && this.slots[num3] == item)
				{
					break;
				}
				num4 = num3;
				num3 = link.Next;
			}
			while (num3 != -1);
			if (num3 == -1)
			{
				return false;
			}
			this.count--;
			if (num4 == -1)
			{
				this.table[num2] = this.links[num3].Next + 1;
			}
			else
			{
				this.links[num4].Next = this.links[num3].Next;
			}
			this.links[num3].Next = this.empty_slot;
			this.empty_slot = num3;
			this.links[num3].HashCode = 0;
			this.slots[num3] = 0L;
			this.generation++;
			return true;
		}

		// Token: 0x0600B726 RID: 46886 RVA: 0x0043DCA4 File Offset: 0x0043BEA4
		public int RemoveWhere(Predicate<long> match)
		{
			if (match == null)
			{
				throw new ArgumentNullException("match");
			}
			List<long> list = new List<long>();
			foreach (long num in this)
			{
				if (match(num))
				{
					list.Add(num);
				}
			}
			foreach (long num2 in list)
			{
				this.Remove(num2);
			}
			return list.Count;
		}

		// Token: 0x0600B727 RID: 46887 RVA: 0x0043DD58 File Offset: 0x0043BF58
		public void TrimExcess()
		{
			this.Resize();
		}

		// Token: 0x0600B728 RID: 46888 RVA: 0x0043DD60 File Offset: 0x0043BF60
		public void IntersectWith(IEnumerable<long> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			HashSetLong other_set = this.ToSet(other);
			this.RemoveWhere((long item) => !other_set.Contains(item));
		}

		// Token: 0x0600B729 RID: 46889 RVA: 0x0043DDA4 File Offset: 0x0043BFA4
		public void ExceptWithHashSetLong(HashSetLong other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			foreach (long num in other)
			{
				this.Remove(num);
			}
		}

		// Token: 0x0600B72A RID: 46890 RVA: 0x0043DE04 File Offset: 0x0043C004
		public void ExceptWith(IEnumerable<long> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			foreach (long num in other)
			{
				this.Remove(num);
			}
		}

		// Token: 0x0600B72B RID: 46891 RVA: 0x0043DE5C File Offset: 0x0043C05C
		public bool Overlaps(IEnumerable<long> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			foreach (long num in other)
			{
				if (this.Contains(num))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600B72C RID: 46892 RVA: 0x0043DEBC File Offset: 0x0043C0BC
		public bool SetEquals(IEnumerable<long> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			HashSetLong hashSetLong = this.ToSet(other);
			if (this.count != hashSetLong.Count)
			{
				return false;
			}
			foreach (long num in this)
			{
				if (!hashSetLong.Contains(num))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600B72D RID: 46893 RVA: 0x0043DF3C File Offset: 0x0043C13C
		public void SymmetricExceptWith(IEnumerable<long> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			foreach (long num in this.ToSet(other))
			{
				if (!this.Add(num))
				{
					this.Remove(num);
				}
			}
		}

		// Token: 0x0600B72E RID: 46894 RVA: 0x0043DFA8 File Offset: 0x0043C1A8
		[PublicizedFrom(EAccessModifier.Private)]
		public HashSetLong ToSet(IEnumerable<long> enumerable)
		{
			HashSetLong hashSetLong = enumerable as HashSetLong;
			if (hashSetLong == null || !this.Comparer.Equals(hashSetLong.Comparer))
			{
				hashSetLong = new HashSetLong(enumerable, this.Comparer);
			}
			return hashSetLong;
		}

		// Token: 0x0600B72F RID: 46895 RVA: 0x0043DFE0 File Offset: 0x0043C1E0
		public void UnionWithHashSetLong(HashSetLong other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			foreach (long num in other)
			{
				this.Add(num);
			}
		}

		// Token: 0x0600B730 RID: 46896 RVA: 0x0043E040 File Offset: 0x0043C240
		public void UnionWith(IEnumerable<long> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			foreach (long num in other)
			{
				this.Add(num);
			}
		}

		// Token: 0x0600B731 RID: 46897 RVA: 0x0043E098 File Offset: 0x0043C298
		[PublicizedFrom(EAccessModifier.Private)]
		public bool CheckIsSubsetOf(HashSetLong other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			foreach (long num in this)
			{
				if (!other.Contains(num))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600B732 RID: 46898 RVA: 0x0043E100 File Offset: 0x0043C300
		public bool IsSubsetOf(IEnumerable<long> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			if (this.count == 0)
			{
				return true;
			}
			HashSetLong hashSetLong = this.ToSet(other);
			return this.count <= hashSetLong.Count && this.CheckIsSubsetOf(hashSetLong);
		}

		// Token: 0x0600B733 RID: 46899 RVA: 0x0043E144 File Offset: 0x0043C344
		public bool IsProperSubsetOf(IEnumerable<long> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			if (this.count == 0)
			{
				return true;
			}
			HashSetLong hashSetLong = this.ToSet(other);
			return this.count < hashSetLong.Count && this.CheckIsSubsetOf(hashSetLong);
		}

		// Token: 0x0600B734 RID: 46900 RVA: 0x0043E188 File Offset: 0x0043C388
		[PublicizedFrom(EAccessModifier.Private)]
		public bool CheckIsSupersetOf(HashSetLong other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			foreach (long num in other)
			{
				if (!this.Contains(num))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600B735 RID: 46901 RVA: 0x0043E1F0 File Offset: 0x0043C3F0
		public bool IsSupersetOf(IEnumerable<long> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			HashSetLong hashSetLong = this.ToSet(other);
			return this.count >= hashSetLong.Count && this.CheckIsSupersetOf(hashSetLong);
		}

		// Token: 0x0600B736 RID: 46902 RVA: 0x0043E22C File Offset: 0x0043C42C
		public bool IsProperSupersetOf(IEnumerable<long> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			HashSetLong hashSetLong = this.ToSet(other);
			return this.count > hashSetLong.Count && this.CheckIsSupersetOf(hashSetLong);
		}

		// Token: 0x0600B737 RID: 46903 RVA: 0x0043E266 File Offset: 0x0043C466
		public static IEqualityComparer<HashSetLong> CreateSetComparer()
		{
			return HashSetLong.setComparer;
		}

		// Token: 0x0600B738 RID: 46904 RVA: 0x0043E270 File Offset: 0x0043C470
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("Version", this.generation);
			info.AddValue("Comparer", this.comparer, typeof(IEqualityComparer<long>));
			info.AddValue("Capacity", (this.table == null) ? 0 : this.table.Length);
			if (this.table != null)
			{
				long[] array = new long[this.count];
				this.CopyTo(array);
				info.AddValue("Elements", array, typeof(long[]));
			}
		}

		// Token: 0x0600B739 RID: 46905 RVA: 0x0043E308 File Offset: 0x0043C508
		public virtual void OnDeserialization(object sender)
		{
			if (this.si != null)
			{
				this.generation = (int)this.si.GetValue("Version", typeof(int));
				this.comparer = (IEqualityComparer<long>)this.si.GetValue("Comparer", typeof(IEqualityComparer<long>));
				int num = (int)this.si.GetValue("Capacity", typeof(int));
				this.empty_slot = -1;
				if (num > 0)
				{
					this.table = new int[num];
					this.slots = new long[num];
					long[] array = (long[])this.si.GetValue("Elements", typeof(long[]));
					if (array == null)
					{
						throw new SerializationException("Missing Elements");
					}
					for (int i = 0; i < array.Length; i++)
					{
						this.Add(array[i]);
					}
				}
				else
				{
					this.table = null;
				}
				this.si = null;
			}
		}

		// Token: 0x0600B73A RID: 46906 RVA: 0x0043E403 File Offset: 0x0043C603
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator<long> System.Collections.Generic.IEnumerable<System.Int64>.GetEnumerator()
		{
			return new HashSetLong.Enumerator(this);
		}

		// Token: 0x170016CD RID: 5837
		// (get) Token: 0x0600B73B RID: 46907 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool System.Collections.Generic.ICollection<System.Int64>.IsReadOnly
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return false;
			}
		}

		// Token: 0x0600B73C RID: 46908 RVA: 0x0043E410 File Offset: 0x0043C610
		[PublicizedFrom(EAccessModifier.Private)]
		public void System.Collections.Generic.ICollection<System.Int64>.Add(long item)
		{
			this.Add(item);
		}

		// Token: 0x0600B73D RID: 46909 RVA: 0x0043E403 File Offset: 0x0043C603
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return new HashSetLong.Enumerator(this);
		}

		// Token: 0x0600B73E RID: 46910 RVA: 0x0043E41A File Offset: 0x0043C61A
		public HashSetLong.Enumerator GetEnumerator()
		{
			return new HashSetLong.Enumerator(this);
		}

		// Token: 0x040088D0 RID: 35024
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public const int INITIAL_SIZE = 10;

		// Token: 0x040088D1 RID: 35025
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public const float DEFAULT_LOAD_FACTOR = 0.9f;

		// Token: 0x040088D2 RID: 35026
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public const int NO_SLOT = -1;

		// Token: 0x040088D3 RID: 35027
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public const int HASH_FLAG = -2147483648;

		// Token: 0x040088D4 RID: 35028
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int[] table;

		// Token: 0x040088D5 RID: 35029
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public HashSetLong.Link[] links;

		// Token: 0x040088D6 RID: 35030
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public long[] slots;

		// Token: 0x040088D7 RID: 35031
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int touched;

		// Token: 0x040088D8 RID: 35032
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int empty_slot;

		// Token: 0x040088D9 RID: 35033
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int count;

		// Token: 0x040088DA RID: 35034
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int threshold;

		// Token: 0x040088DB RID: 35035
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public IEqualityComparer<long> comparer;

		// Token: 0x040088DC RID: 35036
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public SerializationInfo si;

		// Token: 0x040088DD RID: 35037
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int generation;

		// Token: 0x040088DE RID: 35038
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public static readonly HashSetLong.HashSetEqualityComparer setComparer = new HashSetLong.HashSetEqualityComparer();

		// Token: 0x020016E5 RID: 5861
		[PublicizedFrom(EAccessModifier.Private)]
		public struct Link
		{
			// Token: 0x040088DF RID: 35039
			public int HashCode;

			// Token: 0x040088E0 RID: 35040
			public int Next;
		}

		// Token: 0x020016E6 RID: 5862
		[PublicizedFrom(EAccessModifier.Private)]
		public class HashSetEqualityComparer : IEqualityComparer<HashSetLong>
		{
			// Token: 0x0600B740 RID: 46912 RVA: 0x0043E430 File Offset: 0x0043C630
			public bool Equals(HashSetLong lhs, HashSetLong rhs)
			{
				if (lhs == rhs)
				{
					return true;
				}
				if (lhs == null || rhs == null || lhs.Count != rhs.Count)
				{
					return false;
				}
				foreach (long num in lhs)
				{
					if (!rhs.Contains(num))
					{
						return false;
					}
				}
				return true;
			}

			// Token: 0x0600B741 RID: 46913 RVA: 0x0043E4A4 File Offset: 0x0043C6A4
			public int GetHashCode(HashSetLong hashset)
			{
				if (hashset == null)
				{
					return 0;
				}
				IEqualityComparer<long> @default = EqualityComparer<long>.Default;
				int num = 0;
				foreach (long num2 in hashset)
				{
					num ^= @default.GetHashCode(num2);
				}
				return num;
			}
		}

		// Token: 0x020016E7 RID: 5863
		[Serializable]
		public struct Enumerator : IEnumerator<long>, IEnumerator, IDisposable
		{
			// Token: 0x0600B743 RID: 46915 RVA: 0x0043E504 File Offset: 0x0043C704
			[PublicizedFrom(EAccessModifier.Internal)]
			public Enumerator(HashSetLong hashset)
			{
				this = default(HashSetLong.Enumerator);
				this.hashset = hashset;
				this.stamp = hashset.generation;
			}

			// Token: 0x0600B744 RID: 46916 RVA: 0x0043E520 File Offset: 0x0043C720
			public bool MoveNext()
			{
				this.CheckState();
				if (this.next < 0)
				{
					return false;
				}
				while (this.next < this.hashset.touched)
				{
					int num = this.next;
					this.next = num + 1;
					int num2 = num;
					if (this.hashset.GetLinkHashCode(num2) != 0)
					{
						this.current = this.hashset.slots[num2];
						return true;
					}
				}
				this.next = -1;
				return false;
			}

			// Token: 0x170016CE RID: 5838
			// (get) Token: 0x0600B745 RID: 46917 RVA: 0x0043E58E File Offset: 0x0043C78E
			public long Current
			{
				get
				{
					return this.current;
				}
			}

			// Token: 0x170016CF RID: 5839
			// (get) Token: 0x0600B746 RID: 46918 RVA: 0x0043E596 File Offset: 0x0043C796
			public object System.Collections.IEnumerator.Current
			{
				[PublicizedFrom(EAccessModifier.Private)]
				get
				{
					this.CheckState();
					if (this.next <= 0)
					{
						throw new InvalidOperationException("Current is not valid");
					}
					return this.current;
				}
			}

			// Token: 0x0600B747 RID: 46919 RVA: 0x0043E5BD File Offset: 0x0043C7BD
			[PublicizedFrom(EAccessModifier.Private)]
			public void System.Collections.IEnumerator.Reset()
			{
				this.CheckState();
				this.next = 0;
			}

			// Token: 0x0600B748 RID: 46920 RVA: 0x0043E5CC File Offset: 0x0043C7CC
			public void Dispose()
			{
				this.hashset = null;
			}

			// Token: 0x0600B749 RID: 46921 RVA: 0x0043E5D5 File Offset: 0x0043C7D5
			[PublicizedFrom(EAccessModifier.Private)]
			public void CheckState()
			{
				if (this.hashset == null)
				{
					throw new ObjectDisposedException(null);
				}
				if (this.hashset.generation != this.stamp)
				{
					throw new InvalidOperationException("HashSet have been modified while it was iterated over");
				}
			}

			// Token: 0x040088E1 RID: 35041
			[PublicizedFrom(EAccessModifier.Private)]
			[NonSerialized]
			public HashSetLong hashset;

			// Token: 0x040088E2 RID: 35042
			[PublicizedFrom(EAccessModifier.Private)]
			[NonSerialized]
			public int next;

			// Token: 0x040088E3 RID: 35043
			[PublicizedFrom(EAccessModifier.Private)]
			[NonSerialized]
			public int stamp;

			// Token: 0x040088E4 RID: 35044
			[PublicizedFrom(EAccessModifier.Private)]
			[NonSerialized]
			public long current;
		}

		// Token: 0x020016E8 RID: 5864
		[PublicizedFrom(EAccessModifier.Private)]
		public static class PrimeHelper
		{
			// Token: 0x0600B74A RID: 46922 RVA: 0x0043E604 File Offset: 0x0043C804
			[PublicizedFrom(EAccessModifier.Private)]
			public static bool TestPrime(int x)
			{
				if ((x & 1) != 0)
				{
					int num = (int)Math.Sqrt((double)x);
					for (int i = 3; i < num; i += 2)
					{
						if (x % i == 0)
						{
							return false;
						}
					}
					return true;
				}
				return x == 2;
			}

			// Token: 0x0600B74B RID: 46923 RVA: 0x0043E638 File Offset: 0x0043C838
			[PublicizedFrom(EAccessModifier.Private)]
			public static int CalcPrime(int x)
			{
				for (int i = (x & -2) - 1; i < 2147483647; i += 2)
				{
					if (HashSetLong.PrimeHelper.TestPrime(i))
					{
						return i;
					}
				}
				return x;
			}

			// Token: 0x0600B74C RID: 46924 RVA: 0x0043E668 File Offset: 0x0043C868
			public static int ToPrime(int x)
			{
				for (int i = 0; i < HashSetLong.PrimeHelper.primes_table.Length; i++)
				{
					if (x <= HashSetLong.PrimeHelper.primes_table[i])
					{
						return HashSetLong.PrimeHelper.primes_table[i];
					}
				}
				return HashSetLong.PrimeHelper.CalcPrime(x);
			}

			// Token: 0x040088E5 RID: 35045
			[PublicizedFrom(EAccessModifier.Private)]
			public static readonly int[] primes_table = new int[]
			{
				11, 19, 37, 73, 109, 163, 251, 367, 557, 823,
				1237, 1861, 2777, 4177, 6247, 9371, 14057, 21089, 31627, 47431,
				71143, 106721, 160073, 240101, 360163, 540217, 810343, 1215497, 1823231, 2734867,
				4102283, 6153409, 9230113, 13845163
			};
		}
	}
}
