using System;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001A15 RID: 6677
	public class LayerState : ICountable
	{
		// Token: 0x17001877 RID: 6263
		// (get) Token: 0x0600CA41 RID: 51777 RVA: 0x0001FF5D File Offset: 0x0001E15D
		public int Count
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x0600CA42 RID: 51778 RVA: 0x004A164B File Offset: 0x0049F84B
		public LayerState(Func<float, LayerStateType> _getFunc)
		{
			this.Get = _getFunc;
		}

		// Token: 0x040099BE RID: 39358
		public readonly Func<float, LayerStateType> Get;
	}
}
