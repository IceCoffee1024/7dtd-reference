using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000232 RID: 562
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	[UsedByNativeCode]
	public class HelpURLAttribute : Attribute
	{
		// Token: 0x06001856 RID: 6230 RVA: 0x0002850B File Offset: 0x0002670B
		public HelpURLAttribute(string url)
		{
			this.m_Url = url;
			this.m_DispatchingFieldName = "";
			this.m_Dispatcher = false;
		}

		// Token: 0x06001857 RID: 6231 RVA: 0x0002852E File Offset: 0x0002672E
		internal HelpURLAttribute(string defaultURL, string dispatchingFieldName)
		{
			this.m_Url = defaultURL;
			this.m_DispatchingFieldName = dispatchingFieldName;
			this.m_Dispatcher = !string.IsNullOrEmpty(dispatchingFieldName);
		}

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x06001858 RID: 6232 RVA: 0x00028555 File Offset: 0x00026755
		public string URL
		{
			get
			{
				return this.m_Url;
			}
		}

		// Token: 0x0400089F RID: 2207
		internal readonly string m_Url;

		// Token: 0x040008A0 RID: 2208
		internal readonly bool m_Dispatcher;

		// Token: 0x040008A1 RID: 2209
		internal readonly string m_DispatchingFieldName;
	}
}
