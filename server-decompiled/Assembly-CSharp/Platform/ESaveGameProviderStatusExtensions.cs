using System;

namespace Platform
{
	// Token: 0x02001B8D RID: 7053
	public static class ESaveGameProviderStatusExtensions
	{
		// Token: 0x0600D2F0 RID: 54000 RVA: 0x004C7302 File Offset: 0x004C5502
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
