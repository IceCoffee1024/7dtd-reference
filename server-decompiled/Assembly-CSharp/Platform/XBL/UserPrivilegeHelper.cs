using System;
using System.Collections;
using System.Linq;
using Unity.XGamingRuntime;

namespace Platform.XBL
{
	// Token: 0x02001C76 RID: 7286
	public class UserPrivilegeHelper
	{
		// Token: 0x0600D824 RID: 55332 RVA: 0x004D73D8 File Offset: 0x004D55D8
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

		// Token: 0x0600D825 RID: 55333 RVA: 0x004D749B File Offset: 0x004D569B
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

		// Token: 0x0600D826 RID: 55334 RVA: 0x004D74B8 File Offset: 0x004D56B8
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

		// Token: 0x0400A421 RID: 42017
		public readonly PrivilegeState Multiplayer;

		// Token: 0x0400A422 RID: 42018
		public readonly PrivilegeState Communications;

		// Token: 0x0400A423 RID: 42019
		public readonly PrivilegeState CrossPlay;

		// Token: 0x0400A424 RID: 42020
		public readonly PrivilegeState UserGeneratedContent;

		// Token: 0x0400A425 RID: 42021
		public readonly PrivilegeState[] AllAllowed;

		// Token: 0x0400A426 RID: 42022
		public readonly PrivilegeState[] MultiplayerAllowed;

		// Token: 0x0400A427 RID: 42023
		public readonly PrivilegeState[] CommunicationAllowed;

		// Token: 0x0400A428 RID: 42024
		public readonly PrivilegeState[] CrossPlayAllowed;
	}
}
