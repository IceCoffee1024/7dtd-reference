using System;
using System.Reflection;
using UniLinq;

// Token: 0x020013DD RID: 5085
public static class EnumExtensions
{
	// Token: 0x06009F7E RID: 40830 RVA: 0x003BF87C File Offset: 0x003BDA7C
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
