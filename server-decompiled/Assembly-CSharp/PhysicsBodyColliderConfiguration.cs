using System;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;

// Token: 0x020008F1 RID: 2289
public class PhysicsBodyColliderConfiguration
{
	// Token: 0x0600424C RID: 16972 RVA: 0x0019E7CC File Offset: 0x0019C9CC
	public PhysicsBodyColliderConfiguration()
	{
	}

	// Token: 0x0600424D RID: 16973 RVA: 0x0019E830 File Offset: 0x0019CA30
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

	// Token: 0x0600424E RID: 16974 RVA: 0x0019E90C File Offset: 0x0019CB0C
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

	// Token: 0x0600424F RID: 16975 RVA: 0x0019EA48 File Offset: 0x0019CC48
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

	// Token: 0x06004250 RID: 16976 RVA: 0x0019EC38 File Offset: 0x0019CE38
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

	// Token: 0x06004251 RID: 16977 RVA: 0x0019EC8C File Offset: 0x0019CE8C
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

	// Token: 0x04003573 RID: 13683
	public string Tag = "";

	// Token: 0x04003574 RID: 13684
	public int CollisionLayer;

	// Token: 0x04003575 RID: 13685
	public int RagdollLayer;

	// Token: 0x04003576 RID: 13686
	public Vector3 CollisionScale = Vector3.one;

	// Token: 0x04003577 RID: 13687
	public Vector3 RagdollScale = Vector3.one;

	// Token: 0x04003578 RID: 13688
	public Vector3 CollisionOffset = Vector3.zero;

	// Token: 0x04003579 RID: 13689
	public Vector3 RagdollOffset = Vector3.zero;

	// Token: 0x0400357A RID: 13690
	public string Path = "";

	// Token: 0x0400357B RID: 13691
	public EnumColliderType Type = EnumColliderType.Detail;

	// Token: 0x0400357C RID: 13692
	public EnumColliderEnabledFlags EnabledFlags = EnumColliderEnabledFlags.All;
}
