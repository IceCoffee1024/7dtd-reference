using System;
using System.Reflection;
using UniLinq;

// Token: 0x02001418 RID: 5144
public static class EnumExtensions
{
	// Token: 0x0600A126 RID: 41254 RVA: 0x003C975C File Offset: 0x003C795C
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
