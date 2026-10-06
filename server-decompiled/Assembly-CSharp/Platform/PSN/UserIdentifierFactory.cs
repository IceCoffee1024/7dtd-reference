using System;
using UnityEngine.Scripting;

namespace Platform.PSN
{
	// Token: 0x02001CEB RID: 7403
	[Preserve]
	[UserIdentifierFactory(EPlatformIdentifier.PSN)]
	public class UserIdentifierFactory : AbsUserIdentifierFactory
	{
		// Token: 0x0600DBAD RID: 56237 RVA: 0x004E7638 File Offset: 0x004E5838
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
