using System;

namespace Platform
{
	// Token: 0x02001B66 RID: 7014
	public static class ESaveGameProviderStatusExtensions
	{
		// Token: 0x0600D165 RID: 53605 RVA: 0x004C0F0E File Offset: 0x004BF10E
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
