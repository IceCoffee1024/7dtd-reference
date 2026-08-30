using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace WorldGenerationEngineFinal
{
	// Token: 0x020016CE RID: 5838
	public class DataMap<T> where T : struct
	{
		// Token: 0x0600B5D5 RID: 46549 RVA: 0x0043A0FC File Offset: 0x004382FC
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

		// Token: 0x0600B5D6 RID: 46550 RVA: 0x0043A149 File Offset: 0x00438349
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T Get(int x, int y)
		{
			return this.data[x + y * this.width];
		}

		// Token: 0x0600B5D7 RID: 46551 RVA: 0x0043A160 File Offset: 0x00438360
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Set(int x, int y, T _value)
		{
			this.data[x + y * this.width] = _value;
		}

		// Token: 0x0600B5D8 RID: 46552 RVA: 0x0043A178 File Offset: 0x00438378
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

		// Token: 0x040087EF RID: 34799
		public int width;

		// Token: 0x040087F0 RID: 34800
		[PublicizedFrom(EAccessModifier.Private)]
		public T[] data;
	}
}
