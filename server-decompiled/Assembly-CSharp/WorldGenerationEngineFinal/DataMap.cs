using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace WorldGenerationEngineFinal
{
	// Token: 0x02001720 RID: 5920
	public class DataMap<T> where T : struct
	{
		// Token: 0x0600B8AF RID: 47279 RVA: 0x00444394 File Offset: 0x00442594
		public DataMap(int tileWidth, T defaultValue)
		{
			this.width = tileWidth;
			int num = this.width * this.width;
			this.data = new T[num];
			for (int i = 0; i < num; i++)
			{
				this.data[i] = defaultValue;
			}
		}

		// Token: 0x0600B8B0 RID: 47280 RVA: 0x004443E1 File Offset: 0x004425E1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T Get(int x, int y)
		{
			return this.data[x + y * this.width];
		}

		// Token: 0x0600B8B1 RID: 47281 RVA: 0x004443F8 File Offset: 0x004425F8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Set(int x, int y, T _value)
		{
			this.data[x + y * this.width] = _value;
		}

		// Token: 0x0600B8B2 RID: 47282 RVA: 0x00444410 File Offset: 0x00442610
		public void Replace(T _old, T _new)
		{
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			int num = this.width * this.width;
			for (int i = 0; i < num; i++)
			{
				if (@default.Equals(this.data[i], _old))
				{
					this.data[i] = _new;
				}
			}
		}

		// Token: 0x040089D0 RID: 35280
		public int width;

		// Token: 0x040089D1 RID: 35281
		[PublicizedFrom(EAccessModifier.Private)]
		public T[] data;
	}
}
