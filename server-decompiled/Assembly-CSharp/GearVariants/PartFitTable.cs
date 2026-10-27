using System;
using System.Collections.Generic;
using System.Linq;

namespace GearVariants
{
	// Token: 0x02001747 RID: 5959
	[Serializable]
	public class PartFitTable
	{
		// Token: 0x0600B99C RID: 47516 RVA: 0x00446E7C File Offset: 0x0044507C
		[PublicizedFrom(EAccessModifier.Private)]
		public void EnsureIndex()
		{
			if (this._byWornKey != null && this._builtStamp == this._stamp)
			{
				return;
			}
			this._byWornKey = new Dictionary<string, GearFitEntry>(StringComparer.Ordinal);
			foreach (GearFitEntry gearFitEntry in this.entries.Where<GearFitEntry>((GearFitEntry e) => e != null && !string.IsNullOrEmpty(e.wornKey)))
			{
				this._byWornKey[gearFitEntry.wornKey] = gearFitEntry;
			}
			this._builtStamp = this._stamp;
		}

		// Token: 0x0600B99D RID: 47517 RVA: 0x00446F2C File Offset: 0x0044512C
		public void InvalidateIndex()
		{
			this._stamp++;
			this._byWornKey = null;
		}

		// Token: 0x0600B99E RID: 47518 RVA: 0x00446F43 File Offset: 0x00445143
		public GearFitEntry Find(string wornKey)
		{
			if (string.IsNullOrEmpty(wornKey))
			{
				return null;
			}
			this.EnsureIndex();
			return this._byWornKey.GetValueOrDefault(wornKey);
		}

		// Token: 0x0600B99F RID: 47519 RVA: 0x00446F64 File Offset: 0x00445164
		public string GetVariantOrEmpty(string wornKey, string bodyKey)
		{
			if (string.IsNullOrEmpty(bodyKey))
			{
				return "";
			}
			GearFitEntry gearFitEntry = this.Find(wornKey);
			if (gearFitEntry == null)
			{
				return "";
			}
			IEnumerable<BodyFit> fits = gearFitEntry.fits;
			Func<BodyFit, bool> <>9__0;
			Func<BodyFit, bool> func;
			if ((func = <>9__0) == null)
			{
				func = (<>9__0 = (BodyFit fit) => string.Equals(fit.bodyKey, bodyKey, StringComparison.Ordinal));
			}
			using (IEnumerator<BodyFit> enumerator = fits.Where<BodyFit>(func).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					return enumerator.Current.variant ?? "";
				}
			}
			return "";
		}

		// Token: 0x04008A92 RID: 35474
		public List<GearFitEntry> entries = new List<GearFitEntry>();

		// Token: 0x04008A93 RID: 35475
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public Dictionary<string, GearFitEntry> _byWornKey;

		// Token: 0x04008A94 RID: 35476
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _builtStamp = -1;

		// Token: 0x04008A95 RID: 35477
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int _stamp;
	}
}
