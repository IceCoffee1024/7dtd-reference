using System;

namespace Platform
{
	// Token: 0x02001BF0 RID: 7152
	public static class ESaveGameProviderStatusExtensions
	{
		// Token: 0x0600D57C RID: 54652 RVA: 0x004CF912 File Offset: 0x004CDB12
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
