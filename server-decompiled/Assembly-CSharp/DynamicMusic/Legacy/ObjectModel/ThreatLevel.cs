using System;
using MusicUtils.Enums;

namespace DynamicMusic.Legacy.ObjectModel
{
	// Token: 0x02001AC8 RID: 6856
	public class ThreatLevel : EnumDictionary<LayerType, Layer>
	{
		// Token: 0x0600CF01 RID: 52993 RVA: 0x004B0B7C File Offset: 0x004AED7C
		public ThreatLevel(double _tempo, double _sigHi, double _sigLo)
		{
			this.Tempo = _tempo;
			this.SignatureHi = _sigHi;
			this.SignatureLo = _sigLo;
		}

		// Token: 0x04009C98 RID: 40088
		public readonly double Tempo;

		// Token: 0x04009C99 RID: 40089
		public readonly double SignatureHi;

		// Token: 0x04009C9A RID: 40090
		public readonly double SignatureLo;
	}
}
