using System;
using System.Collections;
using System.Linq;
using Unity.XGamingRuntime;

namespace Platform.XBL
{
	// Token: 0x02001C13 RID: 7187
	public class UserPrivilegeHelper
	{
		// Token: 0x0600D598 RID: 54680 RVA: 0x004CEDC8 File Offset: 0x004CCFC8
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

		// Token: 0x0600D599 RID: 54681 RVA: 0x004CEE8B File Offset: 0x004CD08B
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

		// Token: 0x0600D59A RID: 54682 RVA: 0x004CEEA8 File Offset: 0x004CD0A8
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

		// Token: 0x0400A263 RID: 41571
		public readonly PrivilegeState Multiplayer;

		// Token: 0x0400A264 RID: 41572
		public readonly PrivilegeState Communications;

		// Token: 0x0400A265 RID: 41573
		public readonly PrivilegeState CrossPlay;

		// Token: 0x0400A266 RID: 41574
		public readonly PrivilegeState UserGeneratedContent;

		// Token: 0x0400A267 RID: 41575
		public readonly PrivilegeState[] AllAllowed;

		// Token: 0x0400A268 RID: 41576
		public readonly PrivilegeState[] MultiplayerAllowed;

		// Token: 0x0400A269 RID: 41577
		public readonly PrivilegeState[] CommunicationAllowed;

		// Token: 0x0400A26A RID: 41578
		public readonly PrivilegeState[] CrossPlayAllowed;
	}
}
