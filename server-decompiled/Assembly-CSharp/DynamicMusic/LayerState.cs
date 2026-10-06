using System;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001A68 RID: 6760
	public class LayerState : ICountable
	{
		// Token: 0x1700198B RID: 6539
		// (get) Token: 0x0600CD22 RID: 52514 RVA: 0x00020119 File Offset: 0x0001E319
		public int Count
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x0600CD23 RID: 52515 RVA: 0x004ABF0B File Offset: 0x004AA10B
		public LayerState(Func<float, LayerStateType> _getFunc)
		{
			this.Get = _getFunc;
		}

		// Token: 0x04009BB5 RID: 39861
		public readonly Func<float, LayerStateType> Get;
	}
}
