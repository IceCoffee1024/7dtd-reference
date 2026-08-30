using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x0200021A RID: 538
	[UsedByNativeCode]
	public struct PropertyName : IEquatable<PropertyName>
	{
		// Token: 0x060017B4 RID: 6068 RVA: 0x000276B2 File Offset: 0x000258B2
		public PropertyName(string name)
		{
			this = new PropertyName(PropertyNameUtils.PropertyNameFromString(name));
		}

		// Token: 0x060017B5 RID: 6069 RVA: 0x000276C2 File Offset: 0x000258C2
		public PropertyName(PropertyName other)
		{
			this.id = other.id;
		}

		// Token: 0x060017B6 RID: 6070 RVA: 0x000276D1 File Offset: 0x000258D1
		public PropertyName(int id)
		{
			this.id = id;
		}

		// Token: 0x060017B7 RID: 6071 RVA: 0x000276DC File Offset: 0x000258DC
		public static bool IsNullOrEmpty(PropertyName prop)
		{
			return prop.id == 0;
		}

		// Token: 0x060017B8 RID: 6072 RVA: 0x000276F8 File Offset: 0x000258F8
		public static bool operator ==(PropertyName lhs, PropertyName rhs)
		{
			return lhs.id == rhs.id;
		}

		// Token: 0x060017B9 RID: 6073 RVA: 0x00027718 File Offset: 0x00025918
		public static bool operator !=(PropertyName lhs, PropertyName rhs)
		{
			return lhs.id != rhs.id;
		}

		// Token: 0x060017BA RID: 6074 RVA: 0x0002773C File Offset: 0x0002593C
		public override int GetHashCode()
		{
			return this.id;
		}

		// Token: 0x060017BB RID: 6075 RVA: 0x00027754 File Offset: 0x00025954
		public override bool Equals(object other)
		{
			return other is PropertyName && this.Equals((PropertyName)other);
		}

		// Token: 0x060017BC RID: 6076 RVA: 0x00027780 File Offset: 0x00025980
		public bool Equals(PropertyName other)
		{
			return this == other;
		}

		// Token: 0x060017BD RID: 6077 RVA: 0x000277A0 File Offset: 0x000259A0
		public static implicit operator PropertyName(string name)
		{
			return new PropertyName(name);
		}

		// Token: 0x060017BE RID: 6078 RVA: 0x000277B8 File Offset: 0x000259B8
		public static implicit operator PropertyName(int id)
		{
			return new PropertyName(id);
		}

		// Token: 0x060017BF RID: 6079 RVA: 0x000277D0 File Offset: 0x000259D0
		public override string ToString()
		{
			return string.Format("Unknown:{0}", this.id);
		}

		// Token: 0x04000885 RID: 2181
		internal int id;
	}
}
