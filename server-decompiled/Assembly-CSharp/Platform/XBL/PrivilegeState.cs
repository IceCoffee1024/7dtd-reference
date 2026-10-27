using System;
using System.Collections;
using Unity.XGamingRuntime;
using Unity.XGamingRuntime.Interop;

namespace Platform.XBL
{
	// Token: 0x02001C7A RID: 7290
	public class PrivilegeState
	{
		// Token: 0x0600D836 RID: 55350 RVA: 0x004D7751 File Offset: 0x004D5951
		public PrivilegeState(XUserHandle userHandle, XUserPrivilege privilege)
		{
			this.m_userHandle = userHandle;
			this.m_privilege = privilege;
			this.m_has = false;
			this.m_denyReason = (XUserPrivilegeDenyReason)4294967295U;
		}

		// Token: 0x17001B77 RID: 7031
		// (get) Token: 0x0600D837 RID: 55351 RVA: 0x004D7775 File Offset: 0x004D5975
		public bool Has
		{
			get
			{
				return this.m_has;
			}
		}

		// Token: 0x17001B78 RID: 7032
		// (get) Token: 0x0600D838 RID: 55352 RVA: 0x004D777D File Offset: 0x004D597D
		public XUserPrivilegeDenyReason DenyReason
		{
			get
			{
				return this.m_denyReason;
			}
		}

		// Token: 0x0600D839 RID: 55353 RVA: 0x004D7788 File Offset: 0x004D5988
		public void ResolveSilent()
		{
			int num = SDK.XUserCheckPrivilege(this.m_userHandle, XUserPrivilegeOptions.None, this.m_privilege, out this.m_has, out this.m_denyReason);
			XblHelpers.LogHR(num, string.Format("{0} checked privilege '{1}' = {2} ({3})", new object[] { "XUserCheckPrivilege", this.m_privilege, this.m_has, this.m_denyReason }), false);
			if (Unity.XGamingRuntime.Interop.HR.SUCCEEDED(num))
			{
				return;
			}
			this.m_has = false;
			this.m_denyReason = (XUserPrivilegeDenyReason)4294967295U;
		}

		// Token: 0x0600D83A RID: 55354 RVA: 0x004D7814 File Offset: 0x004D5A14
		public IEnumerator ResolveWithPrompt(CoroutineCancellationToken _cancellationToken = null)
		{
			this.ResolveSilent();
			if (this.m_has)
			{
				yield break;
			}
			bool uiOpen = true;
			SDK.XUserResolvePrivilegeWithUiAsync(this.m_userHandle, XUserPrivilegeOptions.None, this.m_privilege, delegate(int hr)
			{
				CoroutineCancellationToken cancellationToken2 = _cancellationToken;
				if (cancellationToken2 != null && cancellationToken2.IsCancelled())
				{
					return;
				}
				try
				{
					XblHelpers.LogHR(hr, "XUserResolvePrivilegeWithUiCompleted", false);
				}
				finally
				{
					uiOpen = false;
				}
			});
			while (uiOpen)
			{
				CoroutineCancellationToken cancellationToken = _cancellationToken;
				if (cancellationToken != null && cancellationToken.IsCancelled())
				{
					yield break;
				}
				yield return null;
			}
			this.ResolveSilent();
			yield break;
		}

		// Token: 0x0400A435 RID: 42037
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUserHandle m_userHandle;

		// Token: 0x0400A436 RID: 42038
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUserPrivilege m_privilege;

		// Token: 0x0400A437 RID: 42039
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_has;

		// Token: 0x0400A438 RID: 42040
		[PublicizedFrom(EAccessModifier.Private)]
		public XUserPrivilegeDenyReason m_denyReason;
	}
}
