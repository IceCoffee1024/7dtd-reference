using System;
using System.Collections;
using Unity.XGamingRuntime;
using Unity.XGamingRuntime.Interop;

namespace Platform.XBL
{
	// Token: 0x02001C49 RID: 7241
	public class PrivilegeState
	{
		// Token: 0x0600D722 RID: 55074 RVA: 0x004D3C29 File Offset: 0x004D1E29
		public PrivilegeState(XUserHandle userHandle, XUserPrivilege privilege)
		{
			this.m_userHandle = userHandle;
			this.m_privilege = privilege;
			this.m_has = false;
			this.m_denyReason = (XUserPrivilegeDenyReason)4294967295U;
		}

		// Token: 0x17001B2F RID: 6959
		// (get) Token: 0x0600D723 RID: 55075 RVA: 0x004D3C4D File Offset: 0x004D1E4D
		public bool Has
		{
			get
			{
				return this.m_has;
			}
		}

		// Token: 0x17001B30 RID: 6960
		// (get) Token: 0x0600D724 RID: 55076 RVA: 0x004D3C55 File Offset: 0x004D1E55
		public XUserPrivilegeDenyReason DenyReason
		{
			get
			{
				return this.m_denyReason;
			}
		}

		// Token: 0x0600D725 RID: 55077 RVA: 0x004D3C60 File Offset: 0x004D1E60
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

		// Token: 0x0600D726 RID: 55078 RVA: 0x004D3CEC File Offset: 0x004D1EEC
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

		// Token: 0x0400A34C RID: 41804
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUserHandle m_userHandle;

		// Token: 0x0400A34D RID: 41805
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUserPrivilege m_privilege;

		// Token: 0x0400A34E RID: 41806
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_has;

		// Token: 0x0400A34F RID: 41807
		[PublicizedFrom(EAccessModifier.Private)]
		public XUserPrivilegeDenyReason m_denyReason;
	}
}
