using System;
using MusicUtils.Enums;

namespace DynamicMusic.Legacy.ObjectModel
{
	// Token: 0x02001AF9 RID: 6905
	public class ThreatLevel : EnumDictionary<LayerType, Layer>
	{
		// Token: 0x0600D015 RID: 53269 RVA: 0x004B4678 File Offset: 0x004B2878
		public ThreatLevel(double _tempo, double _sigHi, double _sigLo)
		{
			this.Tempo = _tempo;
			this.SignatureHi = _sigHi;
			this.SignatureLo = _sigLo;
		}

		// Token: 0x04009D81 RID: 40321
		public readonly double Tempo;

		// Token: 0x04009D82 RID: 40322
		public readonly double SignatureHi;

		// Token: 0x04009D83 RID: 40323
		public readonly double SignatureLo;
	}
}
