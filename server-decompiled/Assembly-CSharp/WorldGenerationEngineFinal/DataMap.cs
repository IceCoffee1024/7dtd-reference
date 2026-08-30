using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace WorldGenerationEngineFinal
{
	// Token: 0x020016F4 RID: 5876
	public class DataMap<T> where T : struct
	{
		// Token: 0x0600B758 RID: 46936 RVA: 0x0043FE74 File Offset: 0x0043E074
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

		// Token: 0x0600B759 RID: 46937 RVA: 0x0043FEC1 File Offset: 0x0043E0C1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T Get(int x, int y)
		{
			return this.data[x + y * this.width];
		}

		// Token: 0x0600B75A RID: 46938 RVA: 0x0043FED8 File Offset: 0x0043E0D8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Set(int x, int y, T _value)
		{
			this.data[x + y * this.width] = _value;
		}

		// Token: 0x0600B75B RID: 46939 RVA: 0x0043FEF0 File Offset: 0x0043E0F0
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

		// Token: 0x0400890B RID: 35083
		public int width;

		// Token: 0x0400890C RID: 35084
		[PublicizedFrom(EAccessModifier.Private)]
		public T[] data;
	}
}
