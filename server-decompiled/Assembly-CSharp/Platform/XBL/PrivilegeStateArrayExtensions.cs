using System;
using System.Collections;

namespace Platform.XBL
{
	// Token: 0x02001C47 RID: 7239
	public static class PrivilegeStateArrayExtensions
	{
		// Token: 0x0600D719 RID: 55065 RVA: 0x004D3B04 File Offset: 0x004D1D04
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

		// Token: 0x0600D71A RID: 55066 RVA: 0x004D3B30 File Offset: 0x004D1D30
		public static void ResolveSilent(this PrivilegeState[] privilegeStates)
		{
			for (int i = 0; i < privilegeStates.Length; i++)
			{
				privilegeStates[i].ResolveSilent();
			}
		}

		// Token: 0x0600D71B RID: 55067 RVA: 0x004D3B55 File Offset: 0x004D1D55
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
