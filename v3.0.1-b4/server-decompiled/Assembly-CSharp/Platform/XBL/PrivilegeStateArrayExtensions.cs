using System;
using System.Collections;

namespace Platform.XBL
{
	// Token: 0x02001BEE RID: 7150
	public static class PrivilegeStateArrayExtensions
	{
		// Token: 0x0600D413 RID: 54291 RVA: 0x004C8C28 File Offset: 0x004C6E28
		public static bool Has(this PrivilegeState[] privilegeStates)
		{
			for (int i = 0; i < privilegeStates.Length; i++)
			{
				if (!privilegeStates[i].Has)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600D414 RID: 54292 RVA: 0x004C8C54 File Offset: 0x004C6E54
		public static void ResolveSilent(this PrivilegeState[] privilegeStates)
		{
			for (int i = 0; i < privilegeStates.Length; i++)
			{
				privilegeStates[i].ResolveSilent();
			}
		}

		// Token: 0x0600D415 RID: 54293 RVA: 0x004C8C79 File Offset: 0x004C6E79
		public static IEnumerator ResolveWithPrompt(this PrivilegeState[] privilegeStates, CoroutineCancellationToken _cancellationToken)
		{
			foreach (PrivilegeState privilegeState in privilegeStates)
			{
				yield return privilegeState.ResolveWithPrompt(_cancellationToken);
				if (_cancellationToken != null && _cancellationToken.IsCancelled())
				{
					yield break;
				}
			}
			PrivilegeState[] array = null;
			yield break;
		}
	}
}
