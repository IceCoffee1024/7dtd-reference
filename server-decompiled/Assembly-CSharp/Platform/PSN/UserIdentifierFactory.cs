using System;
using UnityEngine.Scripting;

namespace Platform.PSN
{
	// Token: 0x02001D1C RID: 7452
	[Preserve]
	[UserIdentifierFactory(EPlatformIdentifier.PSN)]
	public class UserIdentifierFactory : AbsUserIdentifierFactory
	{
		// Token: 0x0600DCC1 RID: 56513 RVA: 0x004EB1D8 File Offset: 0x004E93D8
		public override PlatformUserIdentifierAbs FromId(string _idString)
		{
			Log.Out("[PSN] Creating PSN user identifier from: {0}", new object[] { _idString });
			ulong num;
			if (StringParsers.TryParseUInt64(_idString, out num))
			{
				return new UserIdentifierPSN(num);
			}
			Log.Warning("[PSN] Could not parse PSN user from " + _idString);
			return null;
		}
	}
}
