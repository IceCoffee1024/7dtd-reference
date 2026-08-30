using System;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001A3C RID: 6716
	public class LayerState : ICountable
	{
		// Token: 0x170018E9 RID: 6377
		// (get) Token: 0x0600CBCA RID: 52170 RVA: 0x00020021 File Offset: 0x0001E221
		public int Count
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x0600CBCB RID: 52171 RVA: 0x004A7A3F File Offset: 0x004A5C3F
		public LayerState(Func<float, LayerStateType> _getFunc)
		{
			this.Get = _getFunc;
		}

		// Token: 0x04009AEF RID: 39663
		public readonly Func<float, LayerStateType> Get;
	}
}
