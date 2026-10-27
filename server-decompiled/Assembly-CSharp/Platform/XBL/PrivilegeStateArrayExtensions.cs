using System;
using System.Collections;

namespace Platform.XBL
{
	// Token: 0x02001C78 RID: 7288
	public static class PrivilegeStateArrayExtensions
	{
		// Token: 0x0600D82D RID: 55341 RVA: 0x004D762C File Offset: 0x004D582C
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

		// Token: 0x0600D82E RID: 55342 RVA: 0x004D7658 File Offset: 0x004D5858
		public static void ResolveSilent(this PrivilegeState[] privilegeStates)
		{
			for (int i = 0; i < privilegeStates.Length; i++)
			{
				privilegeStates[i].ResolveSilent();
			}
		}

		// Token: 0x0600D82F RID: 55343 RVA: 0x004D767D File Offset: 0x004D587D
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
