using System;
using System.Collections;
using Unity.XGamingRuntime;
using Unity.XGamingRuntime.Interop;

namespace Platform.XBL
{
	// Token: 0x02001C17 RID: 7191
	public class PrivilegeState
	{
		// Token: 0x0600D5AA RID: 54698 RVA: 0x004CF141 File Offset: 0x004CD341
		public PrivilegeState(XUserHandle userHandle, XUserPrivilege privilege)
		{
			this.m_userHandle = userHandle;
			this.m_privilege = privilege;
			this.m_has = false;
			this.m_denyReason = (XUserPrivilegeDenyReason)4294967295U;
		}

		// Token: 0x17001A89 RID: 6793
		// (get) Token: 0x0600D5AB RID: 54699 RVA: 0x004CF165 File Offset: 0x004CD365
		public bool Has
		{
			get
			{
				return this.m_has;
			}
		}

		// Token: 0x17001A8A RID: 6794
		// (get) Token: 0x0600D5AC RID: 54700 RVA: 0x004CF16D File Offset: 0x004CD36D
		public XUserPrivilegeDenyReason DenyReason
		{
			get
			{
				return this.m_denyReason;
			}
		}

		// Token: 0x0600D5AD RID: 54701 RVA: 0x004CF178 File Offset: 0x004CD378
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

		// Token: 0x0600D5AE RID: 54702 RVA: 0x004CF204 File Offset: 0x004CD404
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

		// Token: 0x0400A277 RID: 41591
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUserHandle m_userHandle;

		// Token: 0x0400A278 RID: 41592
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUserPrivilege m_privilege;

		// Token: 0x0400A279 RID: 41593
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_has;

		// Token: 0x0400A27A RID: 41594
		[PublicizedFrom(EAccessModifier.Private)]
		public XUserPrivilegeDenyReason m_denyReason;
	}
}
