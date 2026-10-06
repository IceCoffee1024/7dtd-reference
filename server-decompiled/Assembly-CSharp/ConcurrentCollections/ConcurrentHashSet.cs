using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace ConcurrentCollections
{
	// Token: 0x02001AD3 RID: 6867
	[DebuggerDisplay("Count = {Count}")]
	public class ConcurrentHashSet<T> : IReadOnlyCollection<T>, IEnumerable<T>, IEnumerable, ICollection<T>
	{
		// Token: 0x17001A01 RID: 6657
		// (get) Token: 0x0600CF2C RID: 53036 RVA: 0x004B13E7 File Offset: 0x004AF5E7
		public static int DefaultConcurrencyLevel
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return Math.Max(2, Environment.ProcessorCount);
			}
		}

		// Token: 0x17001A02 RID: 6658
		// (get) Token: 0x0600CF2D RID: 53037 RVA: 0x004B13F4 File Offset: 0x004AF5F4
		public int Count
		{
			get
			{
				int num = 0;
				int num2 = 0;
				try
				{
					this.AcquireAllLocks(ref num2);
					for (int i = 0; i < this._tables.CountPerLock.Length; i++)
					{
						num += this._tables.CountPerLock[i];
					}
				}
				finally
				{
					this.ReleaseLocks(0, num2);
				}
				return num;
			}
		}

		// Token: 0x17001A03 RID: 6659
		// (get) Token: 0x0600CF2E RID: 53038 RVA: 0x004B145C File Offset: 0x004AF65C
		public bool IsEmpty
		{
			get
			{
				int num = 0;
				try
				{
					this.AcquireAllLocks(ref num);
					for (int i = 0; i < this._tables.CountPerLock.Length; i++)
					{
						if (this._tables.CountPerLock[i] != 0)
						{
							return false;
						}
					}
				}
				finally
				{
					this.ReleaseLocks(0, num);
				}
				return true;
			}
		}

		// Token: 0x0600CF2F RID: 53039 RVA: 0x004B14C4 File Offset: 0x004AF6C4
		public ConcurrentHashSet()
			: this(ConcurrentHashSet<T>.DefaultConcurrencyLevel, 31, true, null)
		{
		}

		// Token: 0x0600CF30 RID: 53040 RVA: 0x004B14D5 File Offset: 0x004AF6D5
		public ConcurrentHashSet(Action<T> onRemovalFaiuire)
			: this(ConcurrentHashSet<T>.DefaultConcurrencyLevel, 31, true, null)
		{
			this.OnRemovalFailure = onRemovalFaiuire;
		}

		// Token: 0x0600CF31 RID: 53041 RVA: 0x004B14ED File Offset: 0x004AF6ED
		public ConcurrentHashSet(int concurrencyLevel, int capacity)
			: this(concurrencyLevel, capacity, false, null)
		{
		}

		// Token: 0x0600CF32 RID: 53042 RVA: 0x004B14F9 File Offset: 0x004AF6F9
		public ConcurrentHashSet(IEnumerable<T> collection)
			: this(collection, null)
		{
		}

		// Token: 0x0600CF33 RID: 53043 RVA: 0x004B1503 File Offset: 0x004AF703
		public ConcurrentHashSet(IEqualityComparer<T> comparer)
			: this(ConcurrentHashSet<T>.DefaultConcurrencyLevel, 31, true, comparer)
		{
		}

		// Token: 0x0600CF34 RID: 53044 RVA: 0x004B1514 File Offset: 0x004AF714
		public ConcurrentHashSet(IEnumerable<T> collection, IEqualityComparer<T> comparer)
			: this(comparer)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			this.InitializeFromCollection(collection);
		}

		// Token: 0x0600CF35 RID: 53045 RVA: 0x004B1532 File Offset: 0x004AF732
		public ConcurrentHashSet(int concurrencyLevel, IEnumerable<T> collection, IEqualityComparer<T> comparer)
			: this(concurrencyLevel, 31, false, comparer)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			this.InitializeFromCollection(collection);
		}

		// Token: 0x0600CF36 RID: 53046 RVA: 0x004B1554 File Offset: 0x004AF754
		public ConcurrentHashSet(int concurrencyLevel, int capacity, IEqualityComparer<T> comparer)
			: this(concurrencyLevel, capacity, false, comparer)
		{
		}

		// Token: 0x0600CF37 RID: 53047 RVA: 0x004B1560 File Offset: 0x004AF760
		[PublicizedFrom(EAccessModifier.Private)]
		public ConcurrentHashSet(int concurrencyLevel, int capacity, bool growLockArray, IEqualityComparer<T> comparer)
		{
			if (concurrencyLevel < 1)
			{
				throw new ArgumentOutOfRangeException("concurrencyLevel");
			}
			if (capacity < 0)
			{
				throw new ArgumentOutOfRangeException("capacity");
			}
			if (capacity < concurrencyLevel)
			{
				capacity = concurrencyLevel;
			}
			object[] array = new object[concurrencyLevel];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new object();
			}
			int[] array2 = new int[array.Length];
			ConcurrentHashSet<T>.Node[] array3 = new ConcurrentHashSet<T>.Node[capacity];
			this._tables = new ConcurrentHashSet<T>.Tables(array3, array, array2);
			this._growLockArray = growLockArray;
			this._budget = array3.Length / array.Length;
			this._comparer = comparer ?? EqualityComparer<T>.Default;
		}

		// Token: 0x0600CF38 RID: 53048 RVA: 0x004B15FA File Offset: 0x004AF7FA
		public bool Add(T item)
		{
			return this.AddInternal(item, this._comparer.GetHashCode(item), true);
		}

		// Token: 0x0600CF39 RID: 53049 RVA: 0x004B1610 File Offset: 0x004AF810
		public void Clear()
		{
			int num = 0;
			try
			{
				this.AcquireAllLocks(ref num);
				ConcurrentHashSet<T>.Tables tables = new ConcurrentHashSet<T>.Tables(new ConcurrentHashSet<T>.Node[31], this._tables.Locks, new int[this._tables.CountPerLock.Length]);
				this._tables = tables;
				this._budget = Math.Max(1, tables.Buckets.Length / tables.Locks.Length);
			}
			finally
			{
				this.ReleaseLocks(0, num);
			}
		}

		// Token: 0x0600CF3A RID: 53050 RVA: 0x004B1698 File Offset: 0x004AF898
		public bool Contains(T item)
		{
			int hashCode = this._comparer.GetHashCode(item);
			ConcurrentHashSet<T>.Tables tables = this._tables;
			int bucket = ConcurrentHashSet<T>.GetBucket(hashCode, tables.Buckets.Length);
			for (ConcurrentHashSet<T>.Node node = Volatile.Read<ConcurrentHashSet<T>.Node>(ref tables.Buckets[bucket]); node != null; node = node.Next)
			{
				if (hashCode == node.Hashcode && this._comparer.Equals(node.Item, item))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600CF3B RID: 53051 RVA: 0x004B170C File Offset: 0x004AF90C
		public bool TryRemove(T item)
		{
			int hashCode = this._comparer.GetHashCode(item);
			for (;;)
			{
				ConcurrentHashSet<T>.Tables tables = this._tables;
				int num;
				int num2;
				ConcurrentHashSet<T>.GetBucketAndLockNo(hashCode, out num, out num2, tables.Buckets.Length, tables.Locks.Length);
				object obj = tables.Locks[num2];
				lock (obj)
				{
					if (tables != this._tables)
					{
						continue;
					}
					ConcurrentHashSet<T>.Node node = null;
					for (ConcurrentHashSet<T>.Node node2 = tables.Buckets[num]; node2 != null; node2 = node2.Next)
					{
						if (hashCode == node2.Hashcode && this._comparer.Equals(node2.Item, item))
						{
							if (node == null)
							{
								Volatile.Write<ConcurrentHashSet<T>.Node>(ref tables.Buckets[num], node2.Next);
							}
							else
							{
								node.Next = node2.Next;
							}
							tables.CountPerLock[num2]--;
							return true;
						}
						node = node2;
					}
				}
				break;
			}
			Action<T> onRemovalFailure = this.OnRemovalFailure;
			if (onRemovalFailure != null)
			{
				onRemovalFailure(item);
			}
			return false;
		}

		// Token: 0x0600CF3C RID: 53052 RVA: 0x004B182C File Offset: 0x004AFA2C
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600CF3D RID: 53053 RVA: 0x004B1834 File Offset: 0x004AFA34
		public IEnumerator<T> GetEnumerator()
		{
			ConcurrentHashSet<T>.Node[] buckets = this._tables.Buckets;
			int num;
			for (int i = 0; i < buckets.Length; i = num + 1)
			{
				ConcurrentHashSet<T>.Node current;
				for (current = Volatile.Read<ConcurrentHashSet<T>.Node>(ref buckets[i]); current != null; current = current.Next)
				{
					yield return current.Item;
				}
				current = null;
				num = i;
			}
			yield break;
		}

		// Token: 0x0600CF3E RID: 53054 RVA: 0x004B1843 File Offset: 0x004AFA43
		[PublicizedFrom(EAccessModifier.Private)]
		public void System.Collections.Generic.ICollection<T>.Add(T item)
		{
			this.Add(item);
		}

		// Token: 0x17001A04 RID: 6660
		// (get) Token: 0x0600CF3F RID: 53055 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool System.Collections.Generic.ICollection<T>.IsReadOnly
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return false;
			}
		}

		// Token: 0x0600CF40 RID: 53056 RVA: 0x004B1850 File Offset: 0x004AFA50
		[PublicizedFrom(EAccessModifier.Private)]
		public void System.Collections.Generic.ICollection<T>.CopyTo(T[] array, int arrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (arrayIndex < 0)
			{
				throw new ArgumentOutOfRangeException("arrayIndex");
			}
			int num = 0;
			try
			{
				this.AcquireAllLocks(ref num);
				int num2 = 0;
				int num3 = 0;
				while (num3 < this._tables.Locks.Length && num2 >= 0)
				{
					num2 += this._tables.CountPerLock[num3];
					num3++;
				}
				if (array.Length - num2 < arrayIndex || num2 < 0)
				{
					throw new ArgumentException("The index is equal to or greater than the length of the array, or the number of elements in the set is greater than the available space from index to the end of the destination array.");
				}
				this.CopyToItems(array, arrayIndex);
			}
			finally
			{
				this.ReleaseLocks(0, num);
			}
		}

		// Token: 0x0600CF41 RID: 53057 RVA: 0x004B18F4 File Offset: 0x004AFAF4
		[PublicizedFrom(EAccessModifier.Private)]
		public bool System.Collections.Generic.ICollection<T>.Remove(T item)
		{
			return this.TryRemove(item);
		}

		// Token: 0x0600CF42 RID: 53058 RVA: 0x004B1900 File Offset: 0x004AFB00
		[PublicizedFrom(EAccessModifier.Private)]
		public void InitializeFromCollection(IEnumerable<T> collection)
		{
			foreach (T t in collection)
			{
				this.AddInternal(t, this._comparer.GetHashCode(t), false);
			}
			if (this._budget == 0)
			{
				this._budget = this._tables.Buckets.Length / this._tables.Locks.Length;
			}
		}

		// Token: 0x0600CF43 RID: 53059 RVA: 0x004B1984 File Offset: 0x004AFB84
		public bool TryFirst(out T returnValue)
		{
			if (this._tables == null || this._tables.Buckets == null || this._tables.Buckets.Length == 0)
			{
				returnValue = default(T);
				return false;
			}
			ConcurrentHashSet<T>.Node node = this._tables.Buckets.FirstOrDefault<ConcurrentHashSet<T>.Node>((ConcurrentHashSet<T>.Node d) => d != null);
			if (node == null)
			{
				returnValue = default(T);
				return false;
			}
			returnValue = node.Item;
			return true;
		}

		// Token: 0x0600CF44 RID: 53060 RVA: 0x004B1A10 File Offset: 0x004AFC10
		public bool TryRemoveFirst(out T returnValue)
		{
			if (this._tables == null || this._tables.Buckets == null || this._tables.Buckets.Length == 0)
			{
				returnValue = default(T);
				return false;
			}
			ConcurrentHashSet<T>.Node node = this._tables.Buckets.FirstOrDefault<ConcurrentHashSet<T>.Node>((ConcurrentHashSet<T>.Node d) => d != null);
			if (node == null)
			{
				returnValue = default(T);
				return false;
			}
			returnValue = node.Item;
			this.TryRemove(returnValue);
			return true;
		}

		// Token: 0x0600CF45 RID: 53061 RVA: 0x004B1AA8 File Offset: 0x004AFCA8
		[PublicizedFrom(EAccessModifier.Private)]
		public bool AddInternal(T item, int hashcode, bool acquireLock)
		{
			checked
			{
				ConcurrentHashSet<T>.Tables tables;
				bool flag;
				for (;;)
				{
					tables = this._tables;
					int num;
					int num2;
					ConcurrentHashSet<T>.GetBucketAndLockNo(hashcode, out num, out num2, tables.Buckets.Length, tables.Locks.Length);
					flag = false;
					bool flag2 = false;
					try
					{
						if (acquireLock)
						{
							Monitor.Enter(tables.Locks[num2], ref flag2);
						}
						if (tables != this._tables)
						{
							continue;
						}
						for (ConcurrentHashSet<T>.Node node = tables.Buckets[num]; node != null; node = node.Next)
						{
							if (hashcode == node.Hashcode && this._comparer.Equals(node.Item, item))
							{
								return false;
							}
						}
						Volatile.Write<ConcurrentHashSet<T>.Node>(ref tables.Buckets[num], new ConcurrentHashSet<T>.Node(item, hashcode, tables.Buckets[num]));
						tables.CountPerLock[num2]++;
						if (tables.CountPerLock[num2] > this._budget)
						{
							flag = true;
						}
					}
					finally
					{
						if (flag2)
						{
							Monitor.Exit(tables.Locks[num2]);
						}
					}
					break;
				}
				if (flag)
				{
					this.GrowTable(tables);
				}
				return true;
			}
		}

		// Token: 0x0600CF46 RID: 53062 RVA: 0x004B1BB4 File Offset: 0x004AFDB4
		[PublicizedFrom(EAccessModifier.Private)]
		public static int GetBucket(int hashcode, int bucketCount)
		{
			return (hashcode & int.MaxValue) % bucketCount;
		}

		// Token: 0x0600CF47 RID: 53063 RVA: 0x004B1BBF File Offset: 0x004AFDBF
		[PublicizedFrom(EAccessModifier.Private)]
		public static void GetBucketAndLockNo(int hashcode, out int bucketNo, out int lockNo, int bucketCount, int lockCount)
		{
			bucketNo = (hashcode & int.MaxValue) % bucketCount;
			lockNo = bucketNo % lockCount;
		}

		// Token: 0x0600CF48 RID: 53064 RVA: 0x004B1BD4 File Offset: 0x004AFDD4
		[PublicizedFrom(EAccessModifier.Private)]
		public void GrowTable(ConcurrentHashSet<T>.Tables tables)
		{
			int num = 0;
			try
			{
				this.AcquireLocks(0, 1, ref num);
				if (tables == this._tables)
				{
					long num2 = 0L;
					for (int i = 0; i < tables.CountPerLock.Length; i++)
					{
						num2 += (long)tables.CountPerLock[i];
					}
					if (num2 < (long)(tables.Buckets.Length / 4))
					{
						this._budget = 2 * this._budget;
						if (this._budget < 0)
						{
							this._budget = int.MaxValue;
						}
					}
					else
					{
						int num3 = 0;
						bool flag = false;
						object[] array;
						checked
						{
							try
							{
								num3 = tables.Buckets.Length * 2 + 1;
								while (num3 % 3 == 0 || num3 % 5 == 0 || num3 % 7 == 0)
								{
									num3 += 2;
								}
								if (num3 > 2146435071)
								{
									flag = true;
								}
							}
							catch (OverflowException)
							{
								flag = true;
							}
							if (flag)
							{
								num3 = 2146435071;
								this._budget = int.MaxValue;
							}
							this.AcquireLocks(1, tables.Locks.Length, ref num);
							array = tables.Locks;
						}
						if (this._growLockArray && tables.Locks.Length < 1024)
						{
							array = new object[tables.Locks.Length * 2];
							Array.Copy(tables.Locks, 0, array, 0, tables.Locks.Length);
							for (int j = tables.Locks.Length; j < array.Length; j++)
							{
								array[j] = new object();
							}
						}
						ConcurrentHashSet<T>.Node[] array2 = new ConcurrentHashSet<T>.Node[num3];
						int[] array3 = new int[array.Length];
						for (int k = 0; k < tables.Buckets.Length; k++)
						{
							checked
							{
								ConcurrentHashSet<T>.Node next;
								for (ConcurrentHashSet<T>.Node node = tables.Buckets[k]; node != null; node = next)
								{
									next = node.Next;
									int num4;
									int num5;
									ConcurrentHashSet<T>.GetBucketAndLockNo(node.Hashcode, out num4, out num5, array2.Length, array.Length);
									array2[num4] = new ConcurrentHashSet<T>.Node(node.Item, node.Hashcode, array2[num4]);
									array3[num5]++;
								}
							}
						}
						this._budget = Math.Max(1, array2.Length / array.Length);
						this._tables = new ConcurrentHashSet<T>.Tables(array2, array, array3);
					}
				}
			}
			finally
			{
				this.ReleaseLocks(0, num);
			}
		}

		// Token: 0x0600CF49 RID: 53065 RVA: 0x004B1E14 File Offset: 0x004B0014
		[PublicizedFrom(EAccessModifier.Private)]
		public void AcquireAllLocks(ref int locksAcquired)
		{
			this.AcquireLocks(0, 1, ref locksAcquired);
			this.AcquireLocks(1, this._tables.Locks.Length, ref locksAcquired);
		}

		// Token: 0x0600CF4A RID: 53066 RVA: 0x004B1E38 File Offset: 0x004B0038
		[PublicizedFrom(EAccessModifier.Private)]
		public void AcquireLocks(int fromInclusive, int toExclusive, ref int locksAcquired)
		{
			object[] locks = this._tables.Locks;
			for (int i = fromInclusive; i < toExclusive; i++)
			{
				bool flag = false;
				try
				{
					Monitor.Enter(locks[i], ref flag);
				}
				finally
				{
					if (flag)
					{
						locksAcquired++;
					}
				}
			}
		}

		// Token: 0x0600CF4B RID: 53067 RVA: 0x004B1E88 File Offset: 0x004B0088
		[PublicizedFrom(EAccessModifier.Private)]
		public void ReleaseLocks(int fromInclusive, int toExclusive)
		{
			for (int i = fromInclusive; i < toExclusive; i++)
			{
				Monitor.Exit(this._tables.Locks[i]);
			}
		}

		// Token: 0x0600CF4C RID: 53068 RVA: 0x004B1EB8 File Offset: 0x004B00B8
		[PublicizedFrom(EAccessModifier.Private)]
		public void CopyToItems(T[] array, int index)
		{
			foreach (ConcurrentHashSet<T>.Node node in this._tables.Buckets)
			{
				while (node != null)
				{
					array[index] = node.Item;
					index++;
					node = node.Next;
				}
			}
		}

		// Token: 0x04009CBB RID: 40123
		[PublicizedFrom(EAccessModifier.Private)]
		public const int DefaultCapacity = 31;

		// Token: 0x04009CBC RID: 40124
		[PublicizedFrom(EAccessModifier.Private)]
		public const int MaxLockNumber = 1024;

		// Token: 0x04009CBD RID: 40125
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly IEqualityComparer<T> _comparer;

		// Token: 0x04009CBE RID: 40126
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool _growLockArray;

		// Token: 0x04009CBF RID: 40127
		[PublicizedFrom(EAccessModifier.Private)]
		public int _budget;

		// Token: 0x04009CC0 RID: 40128
		[PublicizedFrom(EAccessModifier.Private)]
		public volatile ConcurrentHashSet<T>.Tables _tables;

		// Token: 0x04009CC1 RID: 40129
		public Action<T> OnRemovalFailure;

		// Token: 0x02001AD4 RID: 6868
		[PublicizedFrom(EAccessModifier.Private)]
		public class Tables
		{
			// Token: 0x0600CF4D RID: 53069 RVA: 0x004B1F05 File Offset: 0x004B0105
			public Tables(ConcurrentHashSet<T>.Node[] buckets, object[] locks, int[] countPerLock)
			{
				this.Buckets = buckets;
				this.Locks = locks;
				this.CountPerLock = countPerLock;
			}

			// Token: 0x04009CC2 RID: 40130
			public readonly ConcurrentHashSet<T>.Node[] Buckets;

			// Token: 0x04009CC3 RID: 40131
			public readonly object[] Locks;

			// Token: 0x04009CC4 RID: 40132
			public volatile int[] CountPerLock;
		}

		// Token: 0x02001AD5 RID: 6869
		[PublicizedFrom(EAccessModifier.Private)]
		public class Node
		{
			// Token: 0x0600CF4E RID: 53070 RVA: 0x004B1F24 File Offset: 0x004B0124
			public Node(T item, int hashcode, ConcurrentHashSet<T>.Node next)
			{
				this.Item = item;
				this.Hashcode = hashcode;
				this.Next = next;
			}

			// Token: 0x04009CC5 RID: 40133
			public readonly T Item;

			// Token: 0x04009CC6 RID: 40134
			public readonly int Hashcode;

			// Token: 0x04009CC7 RID: 40135
			public volatile ConcurrentHashSet<T>.Node Next;
		}
	}
}
