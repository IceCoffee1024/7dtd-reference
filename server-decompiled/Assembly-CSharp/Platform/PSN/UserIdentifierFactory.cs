using System;
using UnityEngine.Scripting;

namespace Platform.PSN
{
	// Token: 0x02001CB9 RID: 7353
	[Preserve]
	[UserIdentifierFactory(EPlatformIdentifier.PSN)]
	public class UserIdentifierFactory : AbsUserIdentifierFactory
	{
		// Token: 0x0600DA33 RID: 55859 RVA: 0x004E2AF0 File Offset: 0x004E0CF0
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
