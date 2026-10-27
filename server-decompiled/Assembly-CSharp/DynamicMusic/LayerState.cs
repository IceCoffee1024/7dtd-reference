using System;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001A99 RID: 6809
	public class LayerState : ICountable
	{
		// Token: 0x170019D3 RID: 6611
		// (get) Token: 0x0600CE36 RID: 52790 RVA: 0x00020119 File Offset: 0x0001E319
		public int Count
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x0600CE37 RID: 52791 RVA: 0x004AFA07 File Offset: 0x004ADC07
		public LayerState(Func<float, LayerStateType> _getFunc)
		{
			this.Get = _getFunc;
		}

		// Token: 0x04009C9E RID: 40094
		public readonly Func<float, LayerStateType> Get;
	}
}
