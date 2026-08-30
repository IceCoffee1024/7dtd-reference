using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Collections.Generic
{
	// Token: 0x020016B8 RID: 5816
	[DebuggerDisplay("Count={Count}")]
	[Serializable]
	public class HashSetLong : ICollection<long>, IEnumerable<long>, IEnumerable, ISerializable, IDeserializationCallback
	{
		// Token: 0x17001629 RID: 5673
		// (get) Token: 0x0600B5BC RID: 46524 RVA: 0x0043914B File Offset: 0x0043734B
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x0600B5BD RID: 46525 RVA: 0x00439153 File Offset: 0x00437353
		public HashSetLong()
		{
			this.Init(10, null);
		}

		// Token: 0x0600B5BE RID: 46526 RVA: 0x00439164 File Offset: 0x00437364
		public HashSetLong(IEqualityComparer<long> comparer)
		{
			this.Init(10, comparer);
		}

		// Token: 0x0600B5BF RID: 46527 RVA: 0x00439175 File Offset: 0x00437375
		public HashSetLong(IEnumerable<long> collection)
			: this(collection, null)
		{
		}

		// Token: 0x0600B5C0 RID: 46528 RVA: 0x00439180 File Offset: 0x00437380
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

		// Token: 0x0600B5C1 RID: 46529 RVA: 0x004391F8 File Offset: 0x004373F8
		[PublicizedFrom(EAccessModifier.Protected)]
		public HashSetLong(SerializationInfo info, StreamingContext context)
		{
			this.si = info;
		}

		// Token: 0x0600B5C2 RID: 46530 RVA: 0x00439208 File Offset: 0x00437408
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

		// Token: 0x0600B5C3 RID: 46531 RVA: 0x00439258 File Offset: 0x00437458
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

		// Token: 0x0600B5C4 RID: 46532 RVA: 0x004392C8 File Offset: 0x004374C8
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

		// Token: 0x0600B5C5 RID: 46533 RVA: 0x00439311 File Offset: 0x00437511
		public void CopyTo(long[] array)
		{
			this.CopyTo(array, 0, this.count);
		}

		// Token: 0x0600B5C6 RID: 46534 RVA: 0x00439321 File Offset: 0x00437521
		public void CopyTo(long[] array, int arrayIndex)
		{
			this.CopyTo(array, arrayIndex, this.count);
		}

		// Token: 0x0600B5C7 RID: 46535 RVA: 0x00439334 File Offset: 0x00437534
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

		// Token: 0x0600B5C8 RID: 46536 RVA: 0x004393B4 File Offset: 0x004375B4
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

		// Token: 0x0600B5C9 RID: 46537 RVA: 0x004394BA File Offset: 0x004376BA
		[PublicizedFrom(EAccessModifier.Private)]
		public int GetLinkHashCode(int index)
		{
			return this.links[index].HashCode & int.MinValue;
		}

		// Token: 0x0600B5CA RID: 46538 RVA: 0x004394D4 File Offset: 0x004376D4
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

		// Token: 0x1700162A RID: 5674
		// (get) Token: 0x0600B5CB RID: 46539 RVA: 0x004395C4 File Offset: 0x004377C4
		public IEqualityComparer<long> Comparer
		{
			get
			{
				return this.comparer;
			}
		}

		// Token: 0x0600B5CC RID: 46540 RVA: 0x004395CC File Offset: 0x004377CC
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

		// Token: 0x0600B5CD RID: 46541 RVA: 0x00439638 File Offset: 0x00437838
		public bool Contains(long item)
		{
			int num = ((int)item ^ (int)(item >> 32)) | int.MinValue;
			int num2 = (num & int.MaxValue) % this.table.Length;
			return this.SlotsContainsAt(num2, num, item);
		}

		// Token: 0x0600B5CE RID: 46542 RVA: 0x00439670 File Offset: 0x00437870
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

		// Token: 0x0600B5CF RID: 46543 RVA: 0x00439784 File Offset: 0x00437984
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

		// Token: 0x0600B5D0 RID: 46544 RVA: 0x00439838 File Offset: 0x00437A38
		public void TrimExcess()
		{
			this.Resize();
		}

		// Token: 0x0600B5D1 RID: 46545 RVA: 0x00439840 File Offset: 0x00437A40
		public void IntersectWith(IEnumerable<long> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			HashSetLong other_set = this.ToSet(other);
			this.RemoveWhere((long item) => !other_set.Contains(item));
		}

		// Token: 0x0600B5D2 RID: 46546 RVA: 0x00439884 File Offset: 0x00437A84
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

		// Token: 0x0600B5D3 RID: 46547 RVA: 0x004398E4 File Offset: 0x00437AE4
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

		// Token: 0x0600B5D4 RID: 46548 RVA: 0x0043993C File Offset: 0x00437B3C
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

		// Token: 0x0600B5D5 RID: 46549 RVA: 0x0043999C File Offset: 0x00437B9C
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

		// Token: 0x0600B5D6 RID: 46550 RVA: 0x00439A1C File Offset: 0x00437C1C
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

		// Token: 0x0600B5D7 RID: 46551 RVA: 0x00439A88 File Offset: 0x00437C88
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

		// Token: 0x0600B5D8 RID: 46552 RVA: 0x00439AC0 File Offset: 0x00437CC0
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

		// Token: 0x0600B5D9 RID: 46553 RVA: 0x00439B20 File Offset: 0x00437D20
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

		// Token: 0x0600B5DA RID: 46554 RVA: 0x00439B78 File Offset: 0x00437D78
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

		// Token: 0x0600B5DB RID: 46555 RVA: 0x00439BE0 File Offset: 0x00437DE0
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

		// Token: 0x0600B5DC RID: 46556 RVA: 0x00439C24 File Offset: 0x00437E24
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

		// Token: 0x0600B5DD RID: 46557 RVA: 0x00439C68 File Offset: 0x00437E68
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

		// Token: 0x0600B5DE RID: 46558 RVA: 0x00439CD0 File Offset: 0x00437ED0
		public bool IsSupersetOf(IEnumerable<long> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			HashSetLong hashSetLong = this.ToSet(other);
			return this.count >= hashSetLong.Count && this.CheckIsSupersetOf(hashSetLong);
		}

		// Token: 0x0600B5DF RID: 46559 RVA: 0x00439D0C File Offset: 0x00437F0C
		public bool IsProperSupersetOf(IEnumerable<long> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			HashSetLong hashSetLong = this.ToSet(other);
			return this.count > hashSetLong.Count && this.CheckIsSupersetOf(hashSetLong);
		}

		// Token: 0x0600B5E0 RID: 46560 RVA: 0x00439D46 File Offset: 0x00437F46
		public static IEqualityComparer<HashSetLong> CreateSetComparer()
		{
			return HashSetLong.setComparer;
		}

		// Token: 0x0600B5E1 RID: 46561 RVA: 0x00439D50 File Offset: 0x00437F50
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

		// Token: 0x0600B5E2 RID: 46562 RVA: 0x00439DE8 File Offset: 0x00437FE8
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

		// Token: 0x0600B5E3 RID: 46563 RVA: 0x00439EE3 File Offset: 0x004380E3
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator<long> System.Collections.Generic.IEnumerable<System.Int64>.GetEnumerator()
		{
			return new HashSetLong.Enumerator(this);
		}

		// Token: 0x1700162B RID: 5675
		// (get) Token: 0x0600B5E4 RID: 46564 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool System.Collections.Generic.ICollection<System.Int64>.IsReadOnly
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return false;
			}
		}

		// Token: 0x0600B5E5 RID: 46565 RVA: 0x00439EF0 File Offset: 0x004380F0
		[PublicizedFrom(EAccessModifier.Private)]
		public void System.Collections.Generic.ICollection<System.Int64>.Add(long item)
		{
			this.Add(item);
		}

		// Token: 0x0600B5E6 RID: 46566 RVA: 0x00439EE3 File Offset: 0x004380E3
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return new HashSetLong.Enumerator(this);
		}

		// Token: 0x0600B5E7 RID: 46567 RVA: 0x00439EFA File Offset: 0x004380FA
		public HashSetLong.Enumerator GetEnumerator()
		{
			return new HashSetLong.Enumerator(this);
		}

		// Token: 0x0400880B RID: 34827
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public const int INITIAL_SIZE = 10;

		// Token: 0x0400880C RID: 34828
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public const float DEFAULT_LOAD_FACTOR = 0.9f;

		// Token: 0x0400880D RID: 34829
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public const int NO_SLOT = -1;

		// Token: 0x0400880E RID: 34830
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public const int HASH_FLAG = -2147483648;

		// Token: 0x0400880F RID: 34831
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int[] table;

		// Token: 0x04008810 RID: 34832
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public HashSetLong.Link[] links;

		// Token: 0x04008811 RID: 34833
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public long[] slots;

		// Token: 0x04008812 RID: 34834
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int touched;

		// Token: 0x04008813 RID: 34835
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int empty_slot;

		// Token: 0x04008814 RID: 34836
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int count;

		// Token: 0x04008815 RID: 34837
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int threshold;

		// Token: 0x04008816 RID: 34838
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public IEqualityComparer<long> comparer;

		// Token: 0x04008817 RID: 34839
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public SerializationInfo si;

		// Token: 0x04008818 RID: 34840
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int generation;

		// Token: 0x04008819 RID: 34841
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public static readonly HashSetLong.HashSetEqualityComparer setComparer = new HashSetLong.HashSetEqualityComparer();

		// Token: 0x020016B9 RID: 5817
		[PublicizedFrom(EAccessModifier.Private)]
		public struct Link
		{
			// Token: 0x0400881A RID: 34842
			public int HashCode;

			// Token: 0x0400881B RID: 34843
			public int Next;
		}

		// Token: 0x020016BA RID: 5818
		[PublicizedFrom(EAccessModifier.Private)]
		public class HashSetEqualityComparer : IEqualityComparer<HashSetLong>
		{
			// Token: 0x0600B5E9 RID: 46569 RVA: 0x00439F10 File Offset: 0x00438110
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

			// Token: 0x0600B5EA RID: 46570 RVA: 0x00439F84 File Offset: 0x00438184
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

		// Token: 0x020016BB RID: 5819
		[Serializable]
		public struct Enumerator : IEnumerator<long>, IEnumerator, IDisposable
		{
			// Token: 0x0600B5EC RID: 46572 RVA: 0x00439FE4 File Offset: 0x004381E4
			[PublicizedFrom(EAccessModifier.Internal)]
			public Enumerator(HashSetLong hashset)
			{
				this = default(HashSetLong.Enumerator);
				this.hashset = hashset;
				this.stamp = hashset.generation;
			}

			// Token: 0x0600B5ED RID: 46573 RVA: 0x0043A000 File Offset: 0x00438200
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

			// Token: 0x1700162C RID: 5676
			// (get) Token: 0x0600B5EE RID: 46574 RVA: 0x0043A06E File Offset: 0x0043826E
			public long Current
			{
				get
				{
					return this.current;
				}
			}

			// Token: 0x1700162D RID: 5677
			// (get) Token: 0x0600B5EF RID: 46575 RVA: 0x0043A076 File Offset: 0x00438276
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

			// Token: 0x0600B5F0 RID: 46576 RVA: 0x0043A09D File Offset: 0x0043829D
			[PublicizedFrom(EAccessModifier.Private)]
			public void System.Collections.IEnumerator.Reset()
			{
				this.CheckState();
				this.next = 0;
			}

			// Token: 0x0600B5F1 RID: 46577 RVA: 0x0043A0AC File Offset: 0x004382AC
			public void Dispose()
			{
				this.hashset = null;
			}

			// Token: 0x0600B5F2 RID: 46578 RVA: 0x0043A0B5 File Offset: 0x004382B5
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

			// Token: 0x0400881C RID: 34844
			[PublicizedFrom(EAccessModifier.Private)]
			[NonSerialized]
			public HashSetLong hashset;

			// Token: 0x0400881D RID: 34845
			[PublicizedFrom(EAccessModifier.Private)]
			[NonSerialized]
			public int next;

			// Token: 0x0400881E RID: 34846
			[PublicizedFrom(EAccessModifier.Private)]
			[NonSerialized]
			public int stamp;

			// Token: 0x0400881F RID: 34847
			[PublicizedFrom(EAccessModifier.Private)]
			[NonSerialized]
			public long current;
		}

		// Token: 0x020016BC RID: 5820
		[PublicizedFrom(EAccessModifier.Private)]
		public static class PrimeHelper
		{
			// Token: 0x0600B5F3 RID: 46579 RVA: 0x0043A0E4 File Offset: 0x004382E4
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

			// Token: 0x0600B5F4 RID: 46580 RVA: 0x0043A118 File Offset: 0x00438318
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

			// Token: 0x0600B5F5 RID: 46581 RVA: 0x0043A148 File Offset: 0x00438348
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

			// Token: 0x04008820 RID: 34848
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
