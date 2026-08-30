using System;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;

// Token: 0x020008FD RID: 2301
public class PhysicsBodyColliderConfiguration
{
	// Token: 0x060042B1 RID: 17073 RVA: 0x001A08A4 File Offset: 0x0019EAA4
	public PhysicsBodyColliderConfiguration()
	{
	}

	// Token: 0x060042B2 RID: 17074 RVA: 0x001A0908 File Offset: 0x0019EB08
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

	// Token: 0x060042B3 RID: 17075 RVA: 0x001A09E4 File Offset: 0x0019EBE4
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

	// Token: 0x060042B4 RID: 17076 RVA: 0x001A0B20 File Offset: 0x0019ED20
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

	// Token: 0x060042B5 RID: 17077 RVA: 0x001A0D10 File Offset: 0x0019EF10
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

	// Token: 0x060042B6 RID: 17078 RVA: 0x001A0D64 File Offset: 0x0019EF64
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

	// Token: 0x040035F4 RID: 13812
	public string Tag = "";

	// Token: 0x040035F5 RID: 13813
	public int CollisionLayer;

	// Token: 0x040035F6 RID: 13814
	public int RagdollLayer;

	// Token: 0x040035F7 RID: 13815
	public Vector3 CollisionScale = Vector3.one;

	// Token: 0x040035F8 RID: 13816
	public Vector3 RagdollScale = Vector3.one;

	// Token: 0x040035F9 RID: 13817
	public Vector3 CollisionOffset = Vector3.zero;

	// Token: 0x040035FA RID: 13818
	public Vector3 RagdollOffset = Vector3.zero;

	// Token: 0x040035FB RID: 13819
	public string Path = "";

	// Token: 0x040035FC RID: 13820
	public EnumColliderType Type = EnumColliderType.Detail;

	// Token: 0x040035FD RID: 13821
	public EnumColliderEnabledFlags EnabledFlags = EnumColliderEnabledFlags.All;
}
