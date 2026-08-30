using System;
using System.Collections;

namespace Platform.XBL
{
	// Token: 0x02001C15 RID: 7189
	public static class PrivilegeStateArrayExtensions
	{
		// Token: 0x0600D5A1 RID: 54689 RVA: 0x004CF01C File Offset: 0x004CD21C
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

		// Token: 0x0600D5A2 RID: 54690 RVA: 0x004CF048 File Offset: 0x004CD248
		public static void ResolveSilent(this PrivilegeState[] privilegeStates)
		{
			for (int i = 0; i < privilegeStates.Length; i++)
			{
				privilegeStates[i].ResolveSilent();
			}
		}

		// Token: 0x0600D5A3 RID: 54691 RVA: 0x004CF06D File Offset: 0x004CD26D
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
