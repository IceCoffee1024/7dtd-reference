using System;
using System.Diagnostics;

namespace UnityEngine
{
	// Token: 0x0200021D RID: 541
	[Conditional("UNITY_EDITOR")]
	[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
	public class IconAttribute : Attribute
	{
		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x060017DC RID: 6108 RVA: 0x00027A19 File Offset: 0x00025C19
		public string path
		{
			get
			{
				return this.m_IconPath;
			}
		}

		// Token: 0x060017DD RID: 6109 RVA: 0x00002059 File Offset: 0x00000259
		private IconAttribute()
		{
		}

		// Token: 0x060017DE RID: 6110 RVA: 0x00027A21 File Offset: 0x00025C21
		public IconAttribute(string path)
		{
			this.m_IconPath = path;
		}

		// Token: 0x0400088A RID: 2186
		private string m_IconPath;
	}
}
