using System;
using MusicUtils.Enums;

namespace DynamicMusic.Legacy.ObjectModel
{
	// Token: 0x02001A9C RID: 6812
	public class ThreatLevel : EnumDictionary<LayerType, Layer>
	{
		// Token: 0x0600CDA9 RID: 52649 RVA: 0x004AC6B0 File Offset: 0x004AA8B0
		public ThreatLevel(double _tempo, double _sigHi, double _sigLo)
		{
			this.Tempo = _tempo;
			this.SignatureHi = _sigHi;
			this.SignatureLo = _sigLo;
		}

		// Token: 0x04009BD2 RID: 39890
		public readonly double Tempo;

		// Token: 0x04009BD3 RID: 39891
		public readonly double SignatureHi;

		// Token: 0x04009BD4 RID: 39892
		public readonly double SignatureLo;
	}
}
