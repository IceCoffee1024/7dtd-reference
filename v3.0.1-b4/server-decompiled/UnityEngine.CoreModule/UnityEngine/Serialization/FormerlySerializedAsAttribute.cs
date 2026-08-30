using System;
using UnityEngine.Scripting;

namespace UnityEngine.Serialization
{
	// Token: 0x0200030B RID: 779
	[RequiredByNativeCode]
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
	public class FormerlySerializedAsAttribute : Attribute
	{
		// Token: 0x06002008 RID: 8200 RVA: 0x00035454 File Offset: 0x00033654
		public FormerlySerializedAsAttribute(string oldName)
		{
			this.m_oldName = oldName;
		}

		// Token: 0x17000621 RID: 1569
		// (get) Token: 0x06002009 RID: 8201 RVA: 0x00035468 File Offset: 0x00033668
		public string oldName
		{
			get
			{
				return this.m_oldName;
			}
		}

		// Token: 0x04000A88 RID: 2696
		private string m_oldName;
	}
}
