using System;
using System.Collections;
using System.Linq;
using Unity.XGamingRuntime;

namespace Platform.XBL
{
	// Token: 0x02001BEC RID: 7148
	public class UserPrivilegeHelper
	{
		// Token: 0x0600D40A RID: 54282 RVA: 0x004C89D4 File Offset: 0x004C6BD4
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

		// Token: 0x0600D40B RID: 54283 RVA: 0x004C8A97 File Offset: 0x004C6C97
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

		// Token: 0x0600D40C RID: 54284 RVA: 0x004C8AB4 File Offset: 0x004C6CB4
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

		// Token: 0x0400A132 RID: 41266
		public readonly PrivilegeState Multiplayer;

		// Token: 0x0400A133 RID: 41267
		public readonly PrivilegeState Communications;

		// Token: 0x0400A134 RID: 41268
		public readonly PrivilegeState CrossPlay;

		// Token: 0x0400A135 RID: 41269
		public readonly PrivilegeState UserGeneratedContent;

		// Token: 0x0400A136 RID: 41270
		public readonly PrivilegeState[] AllAllowed;

		// Token: 0x0400A137 RID: 41271
		public readonly PrivilegeState[] MultiplayerAllowed;

		// Token: 0x0400A138 RID: 41272
		public readonly PrivilegeState[] CommunicationAllowed;

		// Token: 0x0400A139 RID: 41273
		public readonly PrivilegeState[] CrossPlayAllowed;
	}
}
