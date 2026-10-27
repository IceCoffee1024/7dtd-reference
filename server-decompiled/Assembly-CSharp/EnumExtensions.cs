using System;
using System.Reflection;
using UniLinq;

// Token: 0x0200142A RID: 5162
public static class EnumExtensions
{
	// Token: 0x0600A17E RID: 41342 RVA: 0x003CBB60 File Offset: 0x003C9D60
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
