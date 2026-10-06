using System;
using System.Collections;
using System.Linq;
using Unity.XGamingRuntime;

namespace Platform.XBL
{
	// Token: 0x02001C45 RID: 7237
	public class UserPrivilegeHelper
	{
		// Token: 0x0600D710 RID: 55056 RVA: 0x004D38B0 File Offset: 0x004D1AB0
		public UserPrivilegeHelper(XUserHandle userHandle)
		{
			this.AllAllowed = new PrivilegeState[]
			{
				this.Multiplayer = new PrivilegeState(userHandle, XUserPrivilege.Multiplayer),
				this.Communications = new PrivilegeState(userHandle, XUserPrivilege.Communications),
				this.CrossPlay = new PrivilegeState(userHandle, XUserPrivilege.CrossPlay),
				this.UserGeneratedContent = new PrivilegeState(userHandle, XUserPrivilege.UserGeneratedContent)
			};
			this.MultiplayerAllowed = new PrivilegeState[] { this.Multiplayer, this.UserGeneratedContent };
			this.CommunicationAllowed = new PrivilegeState[] { this.Communications };
			this.CrossPlayAllowed = new PrivilegeState[] { this.CrossPlay };
		}

		// Token: 0x0600D711 RID: 55057 RVA: 0x004D3973 File Offset: 0x004D1B73
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerator ResolveAllowed(bool canPrompt, CoroutineCancellationToken _cancellationToken = null, params PrivilegeState[] privilegeStates)
		{
			if (!canPrompt)
			{
				privilegeStates.ResolveSilent();
				return Enumerable.Empty<object>().GetEnumerator();
			}
			return privilegeStates.ResolveWithPrompt(_cancellationToken);
		}

		// Token: 0x0600D712 RID: 55058 RVA: 0x004D3990 File Offset: 0x004D1B90
		public IEnumerator ResolvePermissions(EUserPerms _perms, bool _canPrompt, CoroutineCancellationToken _cancellationToken = null)
		{
			if (_perms.HasMultiplayer() || _perms.HasHostMultiplayer())
			{
				yield return UserPrivilegeHelper.ResolveAllowed(_canPrompt, _cancellationToken, this.MultiplayerAllowed);
				if (_cancellationToken != null && _cancellationToken.IsCancelled())
				{
					yield break;
				}
			}
			if (_perms.HasCommunication())
			{
				yield return UserPrivilegeHelper.ResolveAllowed(_canPrompt, _cancellationToken, this.CommunicationAllowed);
				if (_cancellationToken != null && _cancellationToken.IsCancelled())
				{
					yield break;
				}
			}
			if (_perms.HasCrossplay())
			{
				yield return UserPrivilegeHelper.ResolveAllowed(_canPrompt, _cancellationToken, this.CrossPlayAllowed);
				if (_cancellationToken != null)
				{
					_cancellationToken.IsCancelled();
				}
				yield break;
			}
			yield break;
		}

		// Token: 0x0400A338 RID: 41784
		public readonly PrivilegeState Multiplayer;

		// Token: 0x0400A339 RID: 41785
		public readonly PrivilegeState Communications;

		// Token: 0x0400A33A RID: 41786
		public readonly PrivilegeState CrossPlay;

		// Token: 0x0400A33B RID: 41787
		public readonly PrivilegeState UserGeneratedContent;

		// Token: 0x0400A33C RID: 41788
		public readonly PrivilegeState[] AllAllowed;

		// Token: 0x0400A33D RID: 41789
		public readonly PrivilegeState[] MultiplayerAllowed;

		// Token: 0x0400A33E RID: 41790
		public readonly PrivilegeState[] CommunicationAllowed;

		// Token: 0x0400A33F RID: 41791
		public readonly PrivilegeState[] CrossPlayAllowed;
	}
}
