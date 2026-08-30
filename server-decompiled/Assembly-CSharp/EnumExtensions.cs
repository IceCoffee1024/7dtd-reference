using System;
using System.Reflection;
using UniLinq;

// Token: 0x020013F8 RID: 5112
public static class EnumExtensions
{
	// Token: 0x0600A07B RID: 41083 RVA: 0x003C4BC0 File Offset: 0x003C2DC0
	public static string GetDocumentation(this Enum enumValue)
	{
		MemberInfo memberInfo = enumValue.GetType().GetMember(enumValue.ToString()).FirstOrDefault<MemberInfo>();
		string text;
		if (memberInfo == null)
		{
			text = null;
		}
		else
		{
			Documentation customAttribute = memberInfo.GetCustomAttribute<Documentation>();
			text = ((customAttribute != null) ? customAttribute.Text : null);
		}
		return text ?? enumValue.ToString();
	}
}
