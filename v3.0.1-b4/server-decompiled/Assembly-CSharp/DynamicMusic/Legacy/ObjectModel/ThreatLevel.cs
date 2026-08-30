using System;
using MusicUtils.Enums;

namespace DynamicMusic.Legacy.ObjectModel
{
	// Token: 0x02001A75 RID: 6773
	public class ThreatLevel : EnumDictionary<LayerType, Layer>
	{
		// Token: 0x0600CC20 RID: 52256 RVA: 0x004A62BC File Offset: 0x004A44BC
		public ThreatLevel(double _tempo, double _sigHi, double _sigLo)
		{
			this.Tempo = _tempo;
			this.SignatureHi = _sigHi;
			this.SignatureLo = _sigLo;
		}

		// Token: 0x04009AA1 RID: 39585
		public readonly double Tempo;

		// Token: 0x04009AA2 RID: 39586
		public readonly double SignatureHi;

		// Token: 0x04009AA3 RID: 39587
		public readonly double SignatureLo;
	}
}
