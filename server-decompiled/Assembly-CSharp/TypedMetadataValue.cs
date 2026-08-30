using System;
using System.IO;

// Token: 0x0200057B RID: 1403
public class TypedMetadataValue
{
	// Token: 0x06002E01 RID: 11777 RVA: 0x001246E4 File Offset: 0x001228E4
	public static TypedMetadataValue.TypeTag StringToTag(string str)
	{
		TypedMetadataValue.TypeTag typeTag;
		if (!(str == "float"))
		{
			if (!(str == "int"))
			{
				if (!(str == "string"))
				{
					typeTag = TypedMetadataValue.TypeTag.None;
				}
				else
				{
					typeTag = TypedMetadataValue.TypeTag.String;
				}
			}
			else
			{
				typeTag = TypedMetadataValue.TypeTag.Integer;
			}
		}
		else
		{
			typeTag = TypedMetadataValue.TypeTag.Float;
		}
		return typeTag;
	}

	// Token: 0x06002E02 RID: 11778 RVA: 0x00124729 File Offset: 0x00122929
	public TypedMetadataValue.TypeTag GetTypeTag()
	{
		return this.typeTag;
	}

	// Token: 0x06002E03 RID: 11779 RVA: 0x00124731 File Offset: 0x00122931
	[PublicizedFrom(EAccessModifier.Private)]
	public TypedMetadataValue(object val, TypedMetadataValue.TypeTag tag)
	{
		this.typeTag = tag;
		this.value = val;
	}

	// Token: 0x06002E04 RID: 11780 RVA: 0x00124747 File Offset: 0x00122947
	public static bool TryCreate(object val, TypedMetadataValue.TypeTag tag, out TypedMetadataValue result)
	{
		if (!TypedMetadataValue.ValueMatchesTag(val, tag))
		{
			result = null;
			return false;
		}
		result = new TypedMetadataValue(val, tag);
		return true;
	}

	// Token: 0x06002E05 RID: 11781 RVA: 0x00124761 File Offset: 0x00122961
	public object GetValue()
	{
		return this.value;
	}

	// Token: 0x06002E06 RID: 11782 RVA: 0x00124769 File Offset: 0x00122969
	public bool SetValue(object val)
	{
		if (TypedMetadataValue.ValueMatchesTag(val, this.typeTag))
		{
			this.value = val;
			return true;
		}
		return false;
	}

	// Token: 0x06002E07 RID: 11783 RVA: 0x00124783 File Offset: 0x00122983
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool ValueMatchesTag(object val, TypedMetadataValue.TypeTag tag)
	{
		if (val == null)
		{
			return false;
		}
		switch (tag)
		{
		case TypedMetadataValue.TypeTag.Float:
			return val is float;
		case TypedMetadataValue.TypeTag.Integer:
			return val is int;
		case TypedMetadataValue.TypeTag.String:
			return val is string;
		default:
			return false;
		}
	}

	// Token: 0x06002E08 RID: 11784 RVA: 0x001247C0 File Offset: 0x001229C0
	public static void Write(TypedMetadataValue tmv, BinaryWriter writer)
	{
		if (tmv == null)
		{
			return;
		}
		writer.Write((int)tmv.typeTag);
		switch (tmv.typeTag)
		{
		case TypedMetadataValue.TypeTag.Float:
			writer.Write((float)tmv.value);
			return;
		case TypedMetadataValue.TypeTag.Integer:
			writer.Write((int)tmv.value);
			return;
		case TypedMetadataValue.TypeTag.String:
			writer.Write((string)tmv.value);
			return;
		default:
			return;
		}
	}

	// Token: 0x06002E09 RID: 11785 RVA: 0x00124830 File Offset: 0x00122A30
	public static TypedMetadataValue Read(BinaryReader reader)
	{
		object obj = null;
		TypedMetadataValue.TypeTag typeTag = (TypedMetadataValue.TypeTag)reader.ReadInt32();
		switch (typeTag)
		{
		case TypedMetadataValue.TypeTag.Float:
			obj = reader.ReadSingle();
			break;
		case TypedMetadataValue.TypeTag.Integer:
			obj = reader.ReadInt32();
			break;
		case TypedMetadataValue.TypeTag.String:
			obj = reader.ReadString();
			break;
		}
		return new TypedMetadataValue(obj, typeTag);
	}

	// Token: 0x06002E0A RID: 11786 RVA: 0x00124888 File Offset: 0x00122A88
	public override bool Equals(object other)
	{
		TypedMetadataValue typedMetadataValue = other as TypedMetadataValue;
		return typedMetadataValue != null && this.typeTag == typedMetadataValue.typeTag && this.value.Equals(typedMetadataValue.value);
	}

	// Token: 0x06002E0B RID: 11787 RVA: 0x000111D7 File Offset: 0x0000F3D7
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	// Token: 0x06002E0C RID: 11788 RVA: 0x001248C0 File Offset: 0x00122AC0
	public TypedMetadataValue Clone()
	{
		TypedMetadataValue.TypeTag typeTag = this.typeTag;
		return new TypedMetadataValue(this.value, this.typeTag);
	}

	// Token: 0x040023AA RID: 9130
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly TypedMetadataValue.TypeTag typeTag;

	// Token: 0x040023AB RID: 9131
	[PublicizedFrom(EAccessModifier.Private)]
	public object value;

	// Token: 0x0200057C RID: 1404
	public enum TypeTag
	{
		// Token: 0x040023AD RID: 9133
		None,
		// Token: 0x040023AE RID: 9134
		Float,
		// Token: 0x040023AF RID: 9135
		Integer,
		// Token: 0x040023B0 RID: 9136
		String
	}
}
