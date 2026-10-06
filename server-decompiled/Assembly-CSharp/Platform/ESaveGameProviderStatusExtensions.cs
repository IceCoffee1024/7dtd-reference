using System;

namespace Platform
{
	// Token: 0x02001BBF RID: 7103
	public static class ESaveGameProviderStatusExtensions
	{
		// Token: 0x0600D468 RID: 54376 RVA: 0x004CBDEA File Offset: 0x004C9FEA
		public static bool IsTerminal(this ESaveGameProviderStatus status)
		{
			switch (status)
			{
			case ESaveGameProviderStatus.Uninitialized:
			case ESaveGameProviderStatus.TemporaryError:
				return false;
			case ESaveGameProviderStatus.Ok:
			case ESaveGameProviderStatus.PermanentError:
				return true;
			default:
				throw new ArgumentOutOfRangeException("status", status, null);
			}
		}
	}
}
