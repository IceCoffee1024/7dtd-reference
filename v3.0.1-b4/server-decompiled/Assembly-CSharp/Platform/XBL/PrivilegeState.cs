using System;
using System.Collections;
using Unity.XGamingRuntime;
using Unity.XGamingRuntime.Interop;

namespace Platform.XBL
{
	// Token: 0x02001BF0 RID: 7152
	public class PrivilegeState
	{
		// Token: 0x0600D41C RID: 54300 RVA: 0x004C8D4D File Offset: 0x004C6F4D
		public PrivilegeState(XUserHandle userHandle, XUserPrivilege privilege)
		{
			this.m_userHandle = userHandle;
			this.m_privilege = privilege;
			this.m_has = false;
			this.m_denyReason = (XUserPrivilegeDenyReason)4294967295U;
		}

		// Token: 0x17001A17 RID: 6679
		// (get) Token: 0x0600D41D RID: 54301 RVA: 0x004C8D71 File Offset: 0x004C6F71
		public bool Has
		{
			get
			{
				return this.m_has;
			}
		}

		// Token: 0x17001A18 RID: 6680
		// (get) Token: 0x0600D41E RID: 54302 RVA: 0x004C8D79 File Offset: 0x004C6F79
		public XUserPrivilegeDenyReason DenyReason
		{
			get
			{
				return this.m_denyReason;
			}
		}

		// Token: 0x0600D41F RID: 54303 RVA: 0x004C8D84 File Offset: 0x004C6F84
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

		// Token: 0x0600D420 RID: 54304 RVA: 0x004C8E10 File Offset: 0x004C7010
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

		// Token: 0x0400A146 RID: 41286
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUserHandle m_userHandle;

		// Token: 0x0400A147 RID: 41287
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUserPrivilege m_privilege;

		// Token: 0x0400A148 RID: 41288
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_has;

		// Token: 0x0400A149 RID: 41289
		[PublicizedFrom(EAccessModifier.Private)]
		public XUserPrivilegeDenyReason m_denyReason;
	}
}
