using System;

namespace UnityEngine
{
	// Token: 0x0200024F RID: 591
	[Serializable]
	public struct LazyLoadReference<T> where T : Object
	{
		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x0600192D RID: 6445 RVA: 0x0002A0A7 File Offset: 0x000282A7
		public bool isSet
		{
			get
			{
				return this.m_InstanceID != 0;
			}
		}

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x0600192E RID: 6446 RVA: 0x0002A0B2 File Offset: 0x000282B2
		public bool isBroken
		{
			get
			{
				return this.m_InstanceID != 0 && !Object.DoesObjectWithInstanceIDExist(this.m_InstanceID);
			}
		}

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x0600192F RID: 6447 RVA: 0x0002A0D0 File Offset: 0x000282D0
		// (set) Token: 0x06001930 RID: 6448 RVA: 0x0002A110 File Offset: 0x00028310
		public T asset
		{
			get
			{
				bool flag = this.m_InstanceID == 0;
				T t;
				if (flag)
				{
					t = default(T);
				}
				else
				{
					t = (T)((object)Object.ForceLoadFromInstanceID(this.m_InstanceID));
				}
				return t;
			}
			set
			{
				bool flag = value == null;
				if (flag)
				{
					this.m_InstanceID = 0;
				}
				else
				{
					bool flag2 = !Object.IsPersistent(value);
					if (flag2)
					{
						throw new ArgumentException("Object that does not belong to a persisted asset cannot be set as the target of a LazyLoadReference.");
					}
					this.m_InstanceID = value.GetInstanceID();
				}
			}
		}

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x06001931 RID: 6449 RVA: 0x0002A16A File Offset: 0x0002836A
		// (set) Token: 0x06001932 RID: 6450 RVA: 0x0002A172 File Offset: 0x00028372
		public int instanceID
		{
			get
			{
				return this.m_InstanceID;
			}
			set
			{
				this.m_InstanceID = value;
			}
		}

		// Token: 0x06001933 RID: 6451 RVA: 0x0002A17C File Offset: 0x0002837C
		public LazyLoadReference(T asset)
		{
			bool flag = asset == null;
			if (flag)
			{
				this.m_InstanceID = 0;
			}
			else
			{
				bool flag2 = !Object.IsPersistent(asset);
				if (flag2)
				{
					throw new ArgumentException("Object that does not belong to a persisted asset cannot be set as the target of a LazyLoadReference.");
				}
				this.m_InstanceID = asset.GetInstanceID();
			}
		}

		// Token: 0x06001934 RID: 6452 RVA: 0x0002A1D6 File Offset: 0x000283D6
		public LazyLoadReference(int instanceID)
		{
			this.m_InstanceID = instanceID;
		}

		// Token: 0x06001935 RID: 6453 RVA: 0x0002A1E0 File Offset: 0x000283E0
		public static implicit operator LazyLoadReference<T>(T asset)
		{
			return new LazyLoadReference<T>
			{
				asset = asset
			};
		}

		// Token: 0x06001936 RID: 6454 RVA: 0x0002A204 File Offset: 0x00028404
		public static implicit operator LazyLoadReference<T>(int instanceID)
		{
			return new LazyLoadReference<T>
			{
				instanceID = instanceID
			};
		}

		// Token: 0x040008D4 RID: 2260
		private const int kInstanceID_None = 0;

		// Token: 0x040008D5 RID: 2261
		[SerializeField]
		private int m_InstanceID;
	}
}
