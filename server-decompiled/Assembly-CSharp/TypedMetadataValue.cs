using System;

// Token: 0x02000584 RID: 1412
public class TypedMetadataValue
{
	// Token: 0x06002E86 RID: 11910 RVA: 0x001268D4 File Offset: 0x00124AD4
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

	// Token: 0x06002E87 RID: 11911 RVA: 0x00126919 File Offset: 0x00124B19
	public TypedMetadataValue.TypeTag GetTypeTag()
	{
		return this.typeTag;
	}

	// Token: 0x06002E88 RID: 11912 RVA: 0x00126921 File Offset: 0x00124B21
	[PublicizedFrom(EAccessModifier.Private)]
	public TypedMetadataValue(object val, TypedMetadataValue.TypeTag tag)
	{
		this.typeTag = tag;
		this.value = val;
	}

	// Token: 0x06002E89 RID: 11913 RVA: 0x00126937 File Offset: 0x00124B37
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

	// Token: 0x06002E8A RID: 11914 RVA: 0x00126951 File Offset: 0x00124B51
	public object GetValue()
	{
		return this.value;
	}

	// Token: 0x06002E8B RID: 11915 RVA: 0x00126959 File Offset: 0x00124B59
	public bool SetValue(object val)
	{
		if (TypedMetadataValue.ValueMatchesTag(val, this.typeTag))
		{
			this.value = val;
			return true;
		}
		return false;
	}

	// Token: 0x06002E8C RID: 11916 RVA: 0x00126973 File Offset: 0x00124B73
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

	// Token: 0x06002E8D RID: 11917 RVA: 0x001269B0 File Offset: 0x00124BB0
	public static void Write(TypedMetadataValue tmv, PooledBinaryWriter writer)
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

	// Token: 0x06002E8E RID: 11918 RVA: 0x00126A20 File Offset: 0x00124C20
	public static TypedMetadataValue Read(PooledBinaryReader reader)
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

	// Token: 0x06002E8F RID: 11919 RVA: 0x00126A78 File Offset: 0x00124C78
	public override bool Equals(object other)
	{
		TypedMetadataValue typedMetadataValue = other as TypedMetadataValue;
		return typedMetadataValue != null && this.typeTag == typedMetadataValue.typeTag && this.value.Equals(typedMetadataValue.value);
	}

	// Token: 0x06002E90 RID: 11920 RVA: 0x000111D7 File Offset: 0x0000F3D7
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	// Token: 0x06002E91 RID: 11921 RVA: 0x00126AB0 File Offset: 0x00124CB0
	public TypedMetadataValue Clone()
	{
		TypedMetadataValue.TypeTag typeTag = this.typeTag;
		return new TypedMetadataValue(this.value, this.typeTag);
	}

	// Token: 0x040023E0 RID: 9184
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly TypedMetadataValue.TypeTag typeTag;

	// Token: 0x040023E1 RID: 9185
	[PublicizedFrom(EAccessModifier.Private)]
	public object value;

	// Token: 0x02000585 RID: 1413
	public enum TypeTag
	{
		// Token: 0x040023E3 RID: 9187
		None,
		// Token: 0x040023E4 RID: 9188
		Float,
		// Token: 0x040023E5 RID: 9189
		Integer,
		// Token: 0x040023E6 RID: 9190
		String
	}
}
