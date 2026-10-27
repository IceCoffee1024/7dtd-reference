using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace WorldGenerationEngineFinal
{
	// Token: 0x02001751 RID: 5969
	public class DataMap<T> where T : struct
	{
		// Token: 0x0600B9C3 RID: 47555 RVA: 0x00447D40 File Offset: 0x00445F40
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

		// Token: 0x0600B9C4 RID: 47556 RVA: 0x00447D8D File Offset: 0x00445F8D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T Get(int x, int y)
		{
			return this.data[x + y * this.width];
		}

		// Token: 0x0600B9C5 RID: 47557 RVA: 0x00447DA4 File Offset: 0x00445FA4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Set(int x, int y, T _value)
		{
			this.data[x + y * this.width] = _value;
		}

		// Token: 0x0600B9C6 RID: 47558 RVA: 0x00447DBC File Offset: 0x00445FBC
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

		// Token: 0x04008AB9 RID: 35513
		public int width;

		// Token: 0x04008ABA RID: 35514
		[PublicizedFrom(EAccessModifier.Private)]
		public T[] data;
	}
}
