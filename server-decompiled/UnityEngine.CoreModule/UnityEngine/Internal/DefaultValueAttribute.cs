using System;

namespace UnityEngine.Internal
{
	// Token: 0x020003DE RID: 990
	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.GenericParameter)]
	[Serializable]
	public class DefaultValueAttribute : Attribute
	{
		// Token: 0x0600214D RID: 8525 RVA: 0x000375B0 File Offset: 0x000357B0
		public DefaultValueAttribute(string value)
		{
			this.DefaultValue = value;
		}

		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x0600214E RID: 8526 RVA: 0x000375C4 File Offset: 0x000357C4
		public object Value
		{
			get
			{
				return this.DefaultValue;
			}
		}

		// Token: 0x0600214F RID: 8527 RVA: 0x000375DC File Offset: 0x000357DC
		public override bool Equals(object obj)
		{
			DefaultValueAttribute defaultValueAttribute = obj as DefaultValueAttribute;
			bool flag = defaultValueAttribute == null;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				bool flag3 = this.DefaultValue == null;
				if (flag3)
				{
					flag2 = defaultValueAttribute.Value == null;
				}
				else
				{
					flag2 = this.DefaultValue.Equals(defaultValueAttribute.Value);
				}
			}
			return flag2;
		}

		// Token: 0x06002150 RID: 8528 RVA: 0x0003762C File Offset: 0x0003582C
		public override int GetHashCode()
		{
			bool flag = this.DefaultValue == null;
			int num;
			if (flag)
			{
				num = base.GetHashCode();
			}
			else
			{
				num = this.DefaultValue.GetHashCode();
			}
			return num;
		}

		// Token: 0x04000B12 RID: 2834
		private object DefaultValue;
	}
}
