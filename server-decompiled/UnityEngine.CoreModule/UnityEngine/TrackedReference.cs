using System;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000263 RID: 611
	[UsedByNativeCode]
	[StructLayout(LayoutKind.Sequential)]
	public class TrackedReference
	{
		// Token: 0x0600199D RID: 6557 RVA: 0x00009E2F File Offset: 0x0000802F
		protected TrackedReference()
		{
		}

		// Token: 0x0600199E RID: 6558 RVA: 0x0002B080 File Offset: 0x00029280
		public static bool operator ==(TrackedReference x, TrackedReference y)
		{
			bool flag = y == null && x == null;
			bool flag2;
			if (flag)
			{
				flag2 = true;
			}
			else
			{
				bool flag3 = y == null;
				if (flag3)
				{
					flag2 = x.m_Ptr == IntPtr.Zero;
				}
				else
				{
					bool flag4 = x == null;
					if (flag4)
					{
						flag2 = y.m_Ptr == IntPtr.Zero;
					}
					else
					{
						flag2 = x.m_Ptr == y.m_Ptr;
					}
				}
			}
			return flag2;
		}

		// Token: 0x0600199F RID: 6559 RVA: 0x0002B0F4 File Offset: 0x000292F4
		public static bool operator !=(TrackedReference x, TrackedReference y)
		{
			return !(x == y);
		}

		// Token: 0x060019A0 RID: 6560 RVA: 0x0002B110 File Offset: 0x00029310
		public override bool Equals(object o)
		{
			return o as TrackedReference == this;
		}

		// Token: 0x060019A1 RID: 6561 RVA: 0x0002B130 File Offset: 0x00029330
		public override int GetHashCode()
		{
			return (int)this.m_Ptr;
		}

		// Token: 0x060019A2 RID: 6562 RVA: 0x0002B150 File Offset: 0x00029350
		public static implicit operator bool(TrackedReference exists)
		{
			return exists != null;
		}

		// Token: 0x040008EF RID: 2287
		internal IntPtr m_Ptr;
	}
}
