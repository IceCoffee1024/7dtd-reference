using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x0200011E RID: 286
	[UsedByNativeCode(Name = "ExposedReference")]
	[Serializable]
	public struct ExposedReference<T> where T : Object
	{
		// Token: 0x06000726 RID: 1830 RVA: 0x00009EC8 File Offset: 0x000080C8
		public T Resolve(IExposedPropertyTable resolver)
		{
			bool flag = resolver != null;
			if (flag)
			{
				bool flag2;
				Object referenceValue = resolver.GetReferenceValue(this.exposedName, out flag2);
				bool flag3 = flag2;
				if (flag3)
				{
					return referenceValue as T;
				}
			}
			return this.defaultValue as T;
		}

		// Token: 0x040003A9 RID: 937
		[SerializeField]
		public PropertyName exposedName;

		// Token: 0x040003AA RID: 938
		[SerializeField]
		public Object defaultValue;
	}
}
