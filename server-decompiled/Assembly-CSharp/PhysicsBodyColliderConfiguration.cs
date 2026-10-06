using System;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;

// Token: 0x02000902 RID: 2306
public class PhysicsBodyColliderConfiguration
{
	// Token: 0x06004243 RID: 16963 RVA: 0x001A09A8 File Offset: 0x0019EBA8
	public PhysicsBodyColliderConfiguration()
	{
	}

	// Token: 0x06004244 RID: 16964 RVA: 0x001A0A0C File Offset: 0x0019EC0C
	public PhysicsBodyColliderConfiguration(PhysicsBodyColliderConfiguration otherConfig)
	{
		this.Tag = otherConfig.Tag;
		this.CollisionLayer = otherConfig.CollisionLayer;
		this.RagdollLayer = otherConfig.RagdollLayer;
		this.CollisionScale = otherConfig.CollisionScale;
		this.CollisionOffset = otherConfig.CollisionOffset;
		this.RagdollScale = otherConfig.RagdollScale;
		this.RagdollOffset = otherConfig.RagdollOffset;
		this.Path = otherConfig.Path;
		this.Type = otherConfig.Type;
		this.EnabledFlags = otherConfig.EnabledFlags;
	}

	// Token: 0x06004245 RID: 16965 RVA: 0x001A0AE8 File Offset: 0x0019ECE8
	public void Write(XmlElement _elem)
	{
		XmlElement xmlElement = _elem.AddXmlElement("collider");
		xmlElement.AddXmlKeyValueProperty("tag", this.Tag);
		xmlElement.AddXmlKeyValueProperty("path", this.Path);
		xmlElement.AddXmlKeyValueProperty("collisionLayer", this.CollisionLayer.ToString());
		xmlElement.AddXmlKeyValueProperty("ragdollLayer", this.RagdollLayer.ToString());
		xmlElement.AddXmlKeyValueProperty("collisionScale", PhysicsBodyColliderConfiguration.vecToString(this.CollisionScale));
		xmlElement.AddXmlKeyValueProperty("ragdollScale", PhysicsBodyColliderConfiguration.vecToString(this.RagdollScale));
		xmlElement.AddXmlKeyValueProperty("collisionOffset", PhysicsBodyColliderConfiguration.vecToString(this.CollisionOffset));
		xmlElement.AddXmlKeyValueProperty("ragdollOffset", PhysicsBodyColliderConfiguration.vecToString(this.RagdollOffset));
		xmlElement.AddXmlKeyValueProperty("type", this.Type.ToStringCached<EnumColliderType>());
		string text = "";
		if ((this.EnabledFlags & EnumColliderEnabledFlags.Collision) != EnumColliderEnabledFlags.Disabled)
		{
			text += "collision";
		}
		if ((this.EnabledFlags & EnumColliderEnabledFlags.Ragdoll) != EnumColliderEnabledFlags.Disabled)
		{
			if (text.Length == 0)
			{
				text = "ragdoll";
			}
			else
			{
				text += ";ragdoll";
			}
		}
		if (text.Length == 0)
		{
			text = "disabled";
		}
		xmlElement.AddXmlKeyValueProperty("flags", text);
	}

	// Token: 0x06004246 RID: 16966 RVA: 0x001A0C24 File Offset: 0x0019EE24
	public static PhysicsBodyColliderConfiguration Read(XElement _e)
	{
		PhysicsBodyColliderConfiguration physicsBodyColliderConfiguration = new PhysicsBodyColliderConfiguration();
		DynamicProperties dynamicProperties = new DynamicProperties();
		foreach (XElement xelement in _e.Elements("property"))
		{
			dynamicProperties.Add(xelement, false);
		}
		physicsBodyColliderConfiguration.Tag = dynamicProperties.GetString("tag");
		physicsBodyColliderConfiguration.Path = dynamicProperties.GetString("path");
		if (dynamicProperties.Contains("collisionLayer"))
		{
			physicsBodyColliderConfiguration.CollisionLayer = int.Parse(dynamicProperties.GetString("collisionLayer"));
			physicsBodyColliderConfiguration.RagdollLayer = int.Parse(dynamicProperties.GetString("ragdollLayer"));
		}
		else
		{
			physicsBodyColliderConfiguration.CollisionLayer = int.Parse(dynamicProperties.GetString("layer"));
			physicsBodyColliderConfiguration.RagdollLayer = physicsBodyColliderConfiguration.CollisionLayer;
		}
		physicsBodyColliderConfiguration.CollisionScale = PhysicsBodyColliderConfiguration.vecFromString(dynamicProperties.GetString("collisionScale"));
		physicsBodyColliderConfiguration.RagdollScale = PhysicsBodyColliderConfiguration.vecFromString(dynamicProperties.GetString("ragdollScale"));
		physicsBodyColliderConfiguration.CollisionOffset = PhysicsBodyColliderConfiguration.vecFromString(dynamicProperties.GetString("collisionOffset"));
		physicsBodyColliderConfiguration.RagdollOffset = PhysicsBodyColliderConfiguration.vecFromString(dynamicProperties.GetString("ragdollOffset"));
		physicsBodyColliderConfiguration.Type = EnumUtils.Parse<EnumColliderType>(dynamicProperties.GetString("type"), false);
		physicsBodyColliderConfiguration.EnabledFlags = EnumColliderEnabledFlags.Disabled;
		string @string = dynamicProperties.GetString("flags");
		if (@string != "disabled")
		{
			foreach (string text in @string.Split(';', StringSplitOptions.None))
			{
				if (text == "collision")
				{
					physicsBodyColliderConfiguration.EnabledFlags |= EnumColliderEnabledFlags.Collision;
				}
				else if (text == "ragdoll")
				{
					physicsBodyColliderConfiguration.EnabledFlags |= EnumColliderEnabledFlags.Ragdoll;
				}
			}
		}
		if (physicsBodyColliderConfiguration.RagdollLayer == 0 || physicsBodyColliderConfiguration.RagdollLayer == 27)
		{
			physicsBodyColliderConfiguration.RagdollLayer = 21;
		}
		return physicsBodyColliderConfiguration;
	}

	// Token: 0x06004247 RID: 16967 RVA: 0x001A0E14 File Offset: 0x0019F014
	[PublicizedFrom(EAccessModifier.Private)]
	public static string vecToString(Vector3 vec)
	{
		return string.Concat(new string[]
		{
			vec.x.ToCultureInvariantString(),
			" ",
			vec.y.ToCultureInvariantString(),
			" ",
			vec.z.ToCultureInvariantString()
		});
	}

	// Token: 0x06004248 RID: 16968 RVA: 0x001A0E68 File Offset: 0x0019F068
	[PublicizedFrom(EAccessModifier.Private)]
	public static Vector3 vecFromString(string str)
	{
		string[] array = str.Split(' ', StringSplitOptions.None);
		if (array.Length == 3)
		{
			return new Vector3(StringParsers.ParseFloat(array[0], 0, -1, NumberStyles.Any), StringParsers.ParseFloat(array[1], 0, -1, NumberStyles.Any), StringParsers.ParseFloat(array[2], 0, -1, NumberStyles.Any));
		}
		if (array.Length < 1)
		{
			throw new FormatException("Vector3 expected");
		}
		return new Vector3(StringParsers.ParseFloat(array[0], 0, -1, NumberStyles.Any), StringParsers.ParseFloat(array[0], 0, -1, NumberStyles.Any), StringParsers.ParseFloat(array[0], 0, -1, NumberStyles.Any));
	}

	// Token: 0x040035E8 RID: 13800
	public string Tag = "";

	// Token: 0x040035E9 RID: 13801
	public int CollisionLayer;

	// Token: 0x040035EA RID: 13802
	public int RagdollLayer;

	// Token: 0x040035EB RID: 13803
	public Vector3 CollisionScale = Vector3.one;

	// Token: 0x040035EC RID: 13804
	public Vector3 RagdollScale = Vector3.one;

	// Token: 0x040035ED RID: 13805
	public Vector3 CollisionOffset = Vector3.zero;

	// Token: 0x040035EE RID: 13806
	public Vector3 RagdollOffset = Vector3.zero;

	// Token: 0x040035EF RID: 13807
	public string Path = "";

	// Token: 0x040035F0 RID: 13808
	public EnumColliderType Type = EnumColliderType.Detail;

	// Token: 0x040035F1 RID: 13809
	public EnumColliderEnabledFlags EnabledFlags = EnumColliderEnabledFlags.All;
}
