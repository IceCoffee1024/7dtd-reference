using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Collections.Generic
{
	// Token: 0x02001703 RID: 5891
	[DebuggerDisplay("Count={Count}")]
	[Serializable]
	public class HashSetLong : ICollection<long>, IEnumerable<long>, IEnumerable, ISerializable, IDeserializationCallback
	{
		// Token: 0x17001712 RID: 5906
		// (get) Token: 0x0600B7E7 RID: 47079 RVA: 0x0044010F File Offset: 0x0043E30F
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x0600B7E8 RID: 47080 RVA: 0x00440117 File Offset: 0x0043E317
		public HashSetLong()
		{
			this.Init(10, null);
		}

		// Token: 0x0600B7E9 RID: 47081 RVA: 0x00440128 File Offset: 0x0043E328
		public HashSetLong(IEqualityComparer<long> comparer)
		{
			this.Init(10, comparer);
		}

		// Token: 0x0600B7EA RID: 47082 RVA: 0x00440139 File Offset: 0x0043E339
		public HashSetLong(IEnumerable<long> collection)
			: this(collection, null)
		{
		}

		// Token: 0x0600B7EB RID: 47083 RVA: 0x00440144 File Offset: 0x0043E344
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

		// Token: 0x0600B7EC RID: 47084 RVA: 0x004401BC File Offset: 0x0043E3BC
		[PublicizedFrom(EAccessModifier.Protected)]
		public HashSetLong(SerializationInfo info, StreamingContext context)
		{
			this.si = info;
		}

		// Token: 0x0600B7ED RID: 47085 RVA: 0x004401CC File Offset: 0x0043E3CC
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

		// Token: 0x0600B7EE RID: 47086 RVA: 0x0044021C File Offset: 0x0043E41C
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

		// Token: 0x0600B7EF RID: 47087 RVA: 0x0044028C File Offset: 0x0043E48C
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

		// Token: 0x0600B7F0 RID: 47088 RVA: 0x004402D5 File Offset: 0x0043E4D5
		public void CopyTo(long[] array)
		{
			this.CopyTo(array, 0, this.count);
		}

		// Token: 0x0600B7F1 RID: 47089 RVA: 0x004402E5 File Offset: 0x0043E4E5
		public void CopyTo(long[] array, int arrayIndex)
		{
			this.CopyTo(array, arrayIndex, this.count);
		}

		// Token: 0x0600B7F2 RID: 47090 RVA: 0x004402F8 File Offset: 0x0043E4F8
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

		// Token: 0x0600B7F3 RID: 47091 RVA: 0x00440378 File Offset: 0x0043E578
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

		// Token: 0x0600B7F4 RID: 47092 RVA: 0x0044047E File Offset: 0x0043E67E
		[PublicizedFrom(EAccessModifier.Private)]
		public int GetLinkHashCode(int index)
		{
			return this.links[index].HashCode & int.MinValue;
		}

		// Token: 0x0600B7F5 RID: 47093 RVA: 0x00440498 File Offset: 0x0043E698
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

		// Token: 0x17001713 RID: 5907
		// (get) Token: 0x0600B7F6 RID: 47094 RVA: 0x00440588 File Offset: 0x0043E788
		public IEqualityComparer<long> Comparer
		{
			get
			{
				return this.comparer;
			}
		}

		// Token: 0x0600B7F7 RID: 47095 RVA: 0x00440590 File Offset: 0x0043E790
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

		// Token: 0x0600B7F8 RID: 47096 RVA: 0x004405FC File Offset: 0x0043E7FC
		public bool Contains(long item)
		{
			int num = ((int)item ^ (int)(item >> 32)) | int.MinValue;
			int num2 = (num & int.MaxValue) % this.table.Length;
			return this.SlotsContainsAt(num2, num, item);
		}

		// Token: 0x0600B7F9 RID: 47097 RVA: 0x00440634 File Offset: 0x0043E834
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

		// Token: 0x0600B7FA RID: 47098 RVA: 0x00440748 File Offset: 0x0043E948
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

		// Token: 0x0600B7FB RID: 47099 RVA: 0x004407FC File Offset: 0x0043E9FC
		public void TrimExcess()
		{
			this.Resize();
		}

		// Token: 0x0600B7FC RID: 47100 RVA: 0x00440804 File Offset: 0x0043EA04
		public void IntersectWith(IEnumerable<long> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			HashSetLong other_set = this.ToSet(other);
			this.RemoveWhere((long item) => !other_set.Contains(item));
		}

		// Token: 0x0600B7FD RID: 47101 RVA: 0x00440848 File Offset: 0x0043EA48
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

		// Token: 0x0600B7FE RID: 47102 RVA: 0x004408A8 File Offset: 0x0043EAA8
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

		// Token: 0x0600B7FF RID: 47103 RVA: 0x00440900 File Offset: 0x0043EB00
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

		// Token: 0x0600B800 RID: 47104 RVA: 0x00440960 File Offset: 0x0043EB60
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

		// Token: 0x0600B801 RID: 47105 RVA: 0x004409E0 File Offset: 0x0043EBE0
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

		// Token: 0x0600B802 RID: 47106 RVA: 0x00440A4C File Offset: 0x0043EC4C
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

		// Token: 0x0600B803 RID: 47107 RVA: 0x00440A84 File Offset: 0x0043EC84
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

		// Token: 0x0600B804 RID: 47108 RVA: 0x00440AE4 File Offset: 0x0043ECE4
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

		// Token: 0x0600B805 RID: 47109 RVA: 0x00440B3C File Offset: 0x0043ED3C
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

		// Token: 0x0600B806 RID: 47110 RVA: 0x00440BA4 File Offset: 0x0043EDA4
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

		// Token: 0x0600B807 RID: 47111 RVA: 0x00440BE8 File Offset: 0x0043EDE8
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

		// Token: 0x0600B808 RID: 47112 RVA: 0x00440C2C File Offset: 0x0043EE2C
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

		// Token: 0x0600B809 RID: 47113 RVA: 0x00440C94 File Offset: 0x0043EE94
		public bool IsSupersetOf(IEnumerable<long> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			HashSetLong hashSetLong = this.ToSet(other);
			return this.count >= hashSetLong.Count && this.CheckIsSupersetOf(hashSetLong);
		}

		// Token: 0x0600B80A RID: 47114 RVA: 0x00440CD0 File Offset: 0x0043EED0
		public bool IsProperSupersetOf(IEnumerable<long> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			HashSetLong hashSetLong = this.ToSet(other);
			return this.count > hashSetLong.Count && this.CheckIsSupersetOf(hashSetLong);
		}

		// Token: 0x0600B80B RID: 47115 RVA: 0x00440D0A File Offset: 0x0043EF0A
		public static IEqualityComparer<HashSetLong> CreateSetComparer()
		{
			return HashSetLong.setComparer;
		}

		// Token: 0x0600B80C RID: 47116 RVA: 0x00440D14 File Offset: 0x0043EF14
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

		// Token: 0x0600B80D RID: 47117 RVA: 0x00440DAC File Offset: 0x0043EFAC
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

		// Token: 0x0600B80E RID: 47118 RVA: 0x00440EA7 File Offset: 0x0043F0A7
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator<long> System.Collections.Generic.IEnumerable<System.Int64>.GetEnumerator()
		{
			return new HashSetLong.Enumerator(this);
		}

		// Token: 0x17001714 RID: 5908
		// (get) Token: 0x0600B80F RID: 47119 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool System.Collections.Generic.ICollection<System.Int64>.IsReadOnly
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return false;
			}
		}

		// Token: 0x0600B810 RID: 47120 RVA: 0x00440EB4 File Offset: 0x0043F0B4
		[PublicizedFrom(EAccessModifier.Private)]
		public void System.Collections.Generic.ICollection<System.Int64>.Add(long item)
		{
			this.Add(item);
		}

		// Token: 0x0600B811 RID: 47121 RVA: 0x00440EA7 File Offset: 0x0043F0A7
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return new HashSetLong.Enumerator(this);
		}

		// Token: 0x0600B812 RID: 47122 RVA: 0x00440EBE File Offset: 0x0043F0BE
		public HashSetLong.Enumerator GetEnumerator()
		{
			return new HashSetLong.Enumerator(this);
		}

		// Token: 0x04008983 RID: 35203
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public const int INITIAL_SIZE = 10;

		// Token: 0x04008984 RID: 35204
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public const float DEFAULT_LOAD_FACTOR = 0.9f;

		// Token: 0x04008985 RID: 35205
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public const int NO_SLOT = -1;

		// Token: 0x04008986 RID: 35206
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public const int HASH_FLAG = -2147483648;

		// Token: 0x04008987 RID: 35207
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int[] table;

		// Token: 0x04008988 RID: 35208
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public HashSetLong.Link[] links;

		// Token: 0x04008989 RID: 35209
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public long[] slots;

		// Token: 0x0400898A RID: 35210
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int touched;

		// Token: 0x0400898B RID: 35211
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int empty_slot;

		// Token: 0x0400898C RID: 35212
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int count;

		// Token: 0x0400898D RID: 35213
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int threshold;

		// Token: 0x0400898E RID: 35214
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public IEqualityComparer<long> comparer;

		// Token: 0x0400898F RID: 35215
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public SerializationInfo si;

		// Token: 0x04008990 RID: 35216
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int generation;

		// Token: 0x04008991 RID: 35217
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public static readonly HashSetLong.HashSetEqualityComparer setComparer = new HashSetLong.HashSetEqualityComparer();

		// Token: 0x02001704 RID: 5892
		[PublicizedFrom(EAccessModifier.Private)]
		public struct Link
		{
			// Token: 0x04008992 RID: 35218
			public int HashCode;

			// Token: 0x04008993 RID: 35219
			public int Next;
		}

		// Token: 0x02001705 RID: 5893
		[PublicizedFrom(EAccessModifier.Private)]
		public class HashSetEqualityComparer : IEqualityComparer<HashSetLong>
		{
			// Token: 0x0600B814 RID: 47124 RVA: 0x00440ED4 File Offset: 0x0043F0D4
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

			// Token: 0x0600B815 RID: 47125 RVA: 0x00440F48 File Offset: 0x0043F148
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

		// Token: 0x02001706 RID: 5894
		[Serializable]
		public struct Enumerator : IEnumerator<long>, IEnumerator, IDisposable
		{
			// Token: 0x0600B817 RID: 47127 RVA: 0x00440FA8 File Offset: 0x0043F1A8
			[PublicizedFrom(EAccessModifier.Internal)]
			public Enumerator(HashSetLong hashset)
			{
				this = default(HashSetLong.Enumerator);
				this.hashset = hashset;
				this.stamp = hashset.generation;
			}

			// Token: 0x0600B818 RID: 47128 RVA: 0x00440FC4 File Offset: 0x0043F1C4
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

			// Token: 0x17001715 RID: 5909
			// (get) Token: 0x0600B819 RID: 47129 RVA: 0x00441032 File Offset: 0x0043F232
			public long Current
			{
				get
				{
					return this.current;
				}
			}

			// Token: 0x17001716 RID: 5910
			// (get) Token: 0x0600B81A RID: 47130 RVA: 0x0044103A File Offset: 0x0043F23A
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

			// Token: 0x0600B81B RID: 47131 RVA: 0x00441061 File Offset: 0x0043F261
			[PublicizedFrom(EAccessModifier.Private)]
			public void System.Collections.IEnumerator.Reset()
			{
				this.CheckState();
				this.next = 0;
			}

			// Token: 0x0600B81C RID: 47132 RVA: 0x00441070 File Offset: 0x0043F270
			public void Dispose()
			{
				this.hashset = null;
			}

			// Token: 0x0600B81D RID: 47133 RVA: 0x00441079 File Offset: 0x0043F279
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

			// Token: 0x04008994 RID: 35220
			[PublicizedFrom(EAccessModifier.Private)]
			[NonSerialized]
			public HashSetLong hashset;

			// Token: 0x04008995 RID: 35221
			[PublicizedFrom(EAccessModifier.Private)]
			[NonSerialized]
			public int next;

			// Token: 0x04008996 RID: 35222
			[PublicizedFrom(EAccessModifier.Private)]
			[NonSerialized]
			public int stamp;

			// Token: 0x04008997 RID: 35223
			[PublicizedFrom(EAccessModifier.Private)]
			[NonSerialized]
			public long current;
		}

		// Token: 0x02001707 RID: 5895
		[PublicizedFrom(EAccessModifier.Private)]
		public static class PrimeHelper
		{
			// Token: 0x0600B81E RID: 47134 RVA: 0x004410A8 File Offset: 0x0043F2A8
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

			// Token: 0x0600B81F RID: 47135 RVA: 0x004410DC File Offset: 0x0043F2DC
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

			// Token: 0x0600B820 RID: 47136 RVA: 0x0044110C File Offset: 0x0043F30C
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

			// Token: 0x04008998 RID: 35224
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
