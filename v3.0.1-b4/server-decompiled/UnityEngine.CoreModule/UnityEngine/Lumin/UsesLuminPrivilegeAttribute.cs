using System;

namespace UnityEngine.Lumin
{
	// Token: 0x020003DC RID: 988
	[Obsolete("Lumin is no longer supported in Unity 2022.2")]
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public sealed class UsesLuminPrivilegeAttribute : Attribute
	{
		// Token: 0x0600214A RID: 8522 RVA: 0x00037584 File Offset: 0x00035784
		public UsesLuminPrivilegeAttribute(string privilege)
		{
			this.m_Privilege = privilege;
		}

		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x0600214B RID: 8523 RVA: 0x00037598 File Offset: 0x00035798
		public string privilege
		{
			get
			{
				return this.m_Privilege;
			}
		}

		// Token: 0x04000B11 RID: 2833
		private readonly string m_Privilege;
	}
}
