using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Collections.Generic
{
	// Token: 0x02001692 RID: 5778
	[DebuggerDisplay("Count={Count}")]
	[Serializable]
	public class HashSetLong : ICollection<long>, IEnumerable<long>, IEnumerable, ISerializable, IDeserializationCallback
	{
		// Token: 0x170015B7 RID: 5559
		// (get) Token: 0x0600B439 RID: 46137 RVA: 0x004333D3 File Offset: 0x004315D3
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x0600B43A RID: 46138 RVA: 0x004333DB File Offset: 0x004315DB
		public HashSetLong()
		{
			this.Init(10, null);
		}

		// Token: 0x0600B43B RID: 46139 RVA: 0x004333EC File Offset: 0x004315EC
		public HashSetLong(IEqualityComparer<long> comparer)
		{
			this.Init(10, comparer);
		}

		// Token: 0x0600B43C RID: 46140 RVA: 0x004333FD File Offset: 0x004315FD
		public HashSetLong(IEnumerable<long> collection)
			: this(collection, null)
		{
		}

		// Token: 0x0600B43D RID: 46141 RVA: 0x00433408 File Offset: 0x00431608
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

		// Token: 0x0600B43E RID: 46142 RVA: 0x00433480 File Offset: 0x00431680
		[PublicizedFrom(EAccessModifier.Protected)]
		public HashSetLong(SerializationInfo info, StreamingContext context)
		{
			this.si = info;
		}

		// Token: 0x0600B43F RID: 46143 RVA: 0x00433490 File Offset: 0x00431690
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

		// Token: 0x0600B440 RID: 46144 RVA: 0x004334E0 File Offset: 0x004316E0
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

		// Token: 0x0600B441 RID: 46145 RVA: 0x00433550 File Offset: 0x00431750
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

		// Token: 0x0600B442 RID: 46146 RVA: 0x00433599 File Offset: 0x00431799
		public void CopyTo(long[] array)
		{
			this.CopyTo(array, 0, this.count);
		}

		// Token: 0x0600B443 RID: 46147 RVA: 0x004335A9 File Offset: 0x004317A9
		public void CopyTo(long[] array, int arrayIndex)
		{
			this.CopyTo(array, arrayIndex, this.count);
		}

		// Token: 0x0600B444 RID: 46148 RVA: 0x004335BC File Offset: 0x004317BC
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

		// Token: 0x0600B445 RID: 46149 RVA: 0x0043363C File Offset: 0x0043183C
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

		// Token: 0x0600B446 RID: 46150 RVA: 0x00433742 File Offset: 0x00431942
		[PublicizedFrom(EAccessModifier.Private)]
		public int GetLinkHashCode(int index)
		{
			return this.links[index].HashCode & int.MinValue;
		}

		// Token: 0x0600B447 RID: 46151 RVA: 0x0043375C File Offset: 0x0043195C
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

		// Token: 0x170015B8 RID: 5560
		// (get) Token: 0x0600B448 RID: 46152 RVA: 0x0043384C File Offset: 0x00431A4C
		public IEqualityComparer<long> Comparer
		{
			get
			{
				return this.comparer;
			}
		}

		// Token: 0x0600B449 RID: 46153 RVA: 0x00433854 File Offset: 0x00431A54
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

		// Token: 0x0600B44A RID: 46154 RVA: 0x004338C0 File Offset: 0x00431AC0
		public bool Contains(long item)
		{
			int num = ((int)item ^ (int)(item >> 32)) | int.MinValue;
			int num2 = (num & int.MaxValue) % this.table.Length;
			return this.SlotsContainsAt(num2, num, item);
		}

		// Token: 0x0600B44B RID: 46155 RVA: 0x004338F8 File Offset: 0x00431AF8
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

		// Token: 0x0600B44C RID: 46156 RVA: 0x00433A0C File Offset: 0x00431C0C
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

		// Token: 0x0600B44D RID: 46157 RVA: 0x00433AC0 File Offset: 0x00431CC0
		public void TrimExcess()
		{
			this.Resize();
		}

		// Token: 0x0600B44E RID: 46158 RVA: 0x00433AC8 File Offset: 0x00431CC8
		public void IntersectWith(IEnumerable<long> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			HashSetLong other_set = this.ToSet(other);
			this.RemoveWhere((long item) => !other_set.Contains(item));
		}

		// Token: 0x0600B44F RID: 46159 RVA: 0x00433B0C File Offset: 0x00431D0C
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

		// Token: 0x0600B450 RID: 46160 RVA: 0x00433B6C File Offset: 0x00431D6C
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

		// Token: 0x0600B451 RID: 46161 RVA: 0x00433BC4 File Offset: 0x00431DC4
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

		// Token: 0x0600B452 RID: 46162 RVA: 0x00433C24 File Offset: 0x00431E24
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

		// Token: 0x0600B453 RID: 46163 RVA: 0x00433CA4 File Offset: 0x00431EA4
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

		// Token: 0x0600B454 RID: 46164 RVA: 0x00433D10 File Offset: 0x00431F10
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

		// Token: 0x0600B455 RID: 46165 RVA: 0x00433D48 File Offset: 0x00431F48
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

		// Token: 0x0600B456 RID: 46166 RVA: 0x00433DA8 File Offset: 0x00431FA8
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

		// Token: 0x0600B457 RID: 46167 RVA: 0x00433E00 File Offset: 0x00432000
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

		// Token: 0x0600B458 RID: 46168 RVA: 0x00433E68 File Offset: 0x00432068
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

		// Token: 0x0600B459 RID: 46169 RVA: 0x00433EAC File Offset: 0x004320AC
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

		// Token: 0x0600B45A RID: 46170 RVA: 0x00433EF0 File Offset: 0x004320F0
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

		// Token: 0x0600B45B RID: 46171 RVA: 0x00433F58 File Offset: 0x00432158
		public bool IsSupersetOf(IEnumerable<long> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			HashSetLong hashSetLong = this.ToSet(other);
			return this.count >= hashSetLong.Count && this.CheckIsSupersetOf(hashSetLong);
		}

		// Token: 0x0600B45C RID: 46172 RVA: 0x00433F94 File Offset: 0x00432194
		public bool IsProperSupersetOf(IEnumerable<long> other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			HashSetLong hashSetLong = this.ToSet(other);
			return this.count > hashSetLong.Count && this.CheckIsSupersetOf(hashSetLong);
		}

		// Token: 0x0600B45D RID: 46173 RVA: 0x00433FCE File Offset: 0x004321CE
		public static IEqualityComparer<HashSetLong> CreateSetComparer()
		{
			return HashSetLong.setComparer;
		}

		// Token: 0x0600B45E RID: 46174 RVA: 0x00433FD8 File Offset: 0x004321D8
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

		// Token: 0x0600B45F RID: 46175 RVA: 0x00434070 File Offset: 0x00432270
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

		// Token: 0x0600B460 RID: 46176 RVA: 0x0043416B File Offset: 0x0043236B
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator<long> System.Collections.Generic.IEnumerable<System.Int64>.GetEnumerator()
		{
			return new HashSetLong.Enumerator(this);
		}

		// Token: 0x170015B9 RID: 5561
		// (get) Token: 0x0600B461 RID: 46177 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool System.Collections.Generic.ICollection<System.Int64>.IsReadOnly
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return false;
			}
		}

		// Token: 0x0600B462 RID: 46178 RVA: 0x00434178 File Offset: 0x00432378
		[PublicizedFrom(EAccessModifier.Private)]
		public void System.Collections.Generic.ICollection<System.Int64>.Add(long item)
		{
			this.Add(item);
		}

		// Token: 0x0600B463 RID: 46179 RVA: 0x0043416B File Offset: 0x0043236B
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return new HashSetLong.Enumerator(this);
		}

		// Token: 0x0600B464 RID: 46180 RVA: 0x00434182 File Offset: 0x00432382
		public HashSetLong.Enumerator GetEnumerator()
		{
			return new HashSetLong.Enumerator(this);
		}

		// Token: 0x040086EF RID: 34543
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public const int INITIAL_SIZE = 10;

		// Token: 0x040086F0 RID: 34544
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public const float DEFAULT_LOAD_FACTOR = 0.9f;

		// Token: 0x040086F1 RID: 34545
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public const int NO_SLOT = -1;

		// Token: 0x040086F2 RID: 34546
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public const int HASH_FLAG = -2147483648;

		// Token: 0x040086F3 RID: 34547
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int[] table;

		// Token: 0x040086F4 RID: 34548
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public HashSetLong.Link[] links;

		// Token: 0x040086F5 RID: 34549
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public long[] slots;

		// Token: 0x040086F6 RID: 34550
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int touched;

		// Token: 0x040086F7 RID: 34551
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int empty_slot;

		// Token: 0x040086F8 RID: 34552
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int count;

		// Token: 0x040086F9 RID: 34553
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int threshold;

		// Token: 0x040086FA RID: 34554
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public IEqualityComparer<long> comparer;

		// Token: 0x040086FB RID: 34555
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public SerializationInfo si;

		// Token: 0x040086FC RID: 34556
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int generation;

		// Token: 0x040086FD RID: 34557
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public static readonly HashSetLong.HashSetEqualityComparer setComparer = new HashSetLong.HashSetEqualityComparer();

		// Token: 0x02001693 RID: 5779
		[PublicizedFrom(EAccessModifier.Private)]
		public struct Link
		{
			// Token: 0x040086FE RID: 34558
			public int HashCode;

			// Token: 0x040086FF RID: 34559
			public int Next;
		}

		// Token: 0x02001694 RID: 5780
		[PublicizedFrom(EAccessModifier.Private)]
		public class HashSetEqualityComparer : IEqualityComparer<HashSetLong>
		{
			// Token: 0x0600B466 RID: 46182 RVA: 0x00434198 File Offset: 0x00432398
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

			// Token: 0x0600B467 RID: 46183 RVA: 0x0043420C File Offset: 0x0043240C
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

		// Token: 0x02001695 RID: 5781
		[Serializable]
		public struct Enumerator : IEnumerator<long>, IEnumerator, IDisposable
		{
			// Token: 0x0600B469 RID: 46185 RVA: 0x0043426C File Offset: 0x0043246C
			[PublicizedFrom(EAccessModifier.Internal)]
			public Enumerator(HashSetLong hashset)
			{
				this = default(HashSetLong.Enumerator);
				this.hashset = hashset;
				this.stamp = hashset.generation;
			}

			// Token: 0x0600B46A RID: 46186 RVA: 0x00434288 File Offset: 0x00432488
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

			// Token: 0x170015BA RID: 5562
			// (get) Token: 0x0600B46B RID: 46187 RVA: 0x004342F6 File Offset: 0x004324F6
			public long Current
			{
				get
				{
					return this.current;
				}
			}

			// Token: 0x170015BB RID: 5563
			// (get) Token: 0x0600B46C RID: 46188 RVA: 0x004342FE File Offset: 0x004324FE
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

			// Token: 0x0600B46D RID: 46189 RVA: 0x00434325 File Offset: 0x00432525
			[PublicizedFrom(EAccessModifier.Private)]
			public void System.Collections.IEnumerator.Reset()
			{
				this.CheckState();
				this.next = 0;
			}

			// Token: 0x0600B46E RID: 46190 RVA: 0x00434334 File Offset: 0x00432534
			public void Dispose()
			{
				this.hashset = null;
			}

			// Token: 0x0600B46F RID: 46191 RVA: 0x0043433D File Offset: 0x0043253D
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

			// Token: 0x04008700 RID: 34560
			[PublicizedFrom(EAccessModifier.Private)]
			[NonSerialized]
			public HashSetLong hashset;

			// Token: 0x04008701 RID: 34561
			[PublicizedFrom(EAccessModifier.Private)]
			[NonSerialized]
			public int next;

			// Token: 0x04008702 RID: 34562
			[PublicizedFrom(EAccessModifier.Private)]
			[NonSerialized]
			public int stamp;

			// Token: 0x04008703 RID: 34563
			[PublicizedFrom(EAccessModifier.Private)]
			[NonSerialized]
			public long current;
		}

		// Token: 0x02001696 RID: 5782
		[PublicizedFrom(EAccessModifier.Private)]
		public static class PrimeHelper
		{
			// Token: 0x0600B470 RID: 46192 RVA: 0x0043436C File Offset: 0x0043256C
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

			// Token: 0x0600B471 RID: 46193 RVA: 0x004343A0 File Offset: 0x004325A0
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

			// Token: 0x0600B472 RID: 46194 RVA: 0x004343D0 File Offset: 0x004325D0
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

			// Token: 0x04008704 RID: 34564
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
