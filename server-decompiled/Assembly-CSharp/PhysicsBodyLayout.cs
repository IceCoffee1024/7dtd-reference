using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

// Token: 0x02000904 RID: 2308
public class PhysicsBodyLayout
{
	// Token: 0x17000721 RID: 1825
	// (get) Token: 0x0600424C RID: 16972 RVA: 0x001A0EFB File Offset: 0x0019F0FB
	public string Name
	{
		get
		{
			return this.name;
		}
	}

	// Token: 0x17000722 RID: 1826
	// (get) Token: 0x0600424D RID: 16973 RVA: 0x001A0F03 File Offset: 0x0019F103
	public List<PhysicsBodyColliderConfiguration> Colliders
	{
		get
		{
			return this.colliders;
		}
	}

	// Token: 0x0600424E RID: 16974 RVA: 0x001A0F0C File Offset: 0x0019F10C
	[PublicizedFrom(EAccessModifier.Private)]
	public static PhysicsBodyLayout New(string _name)
	{
		if (PhysicsBodyLayout.bodyLayouts.ContainsKey(_name))
		{
			throw new Exception("duplicate physics body!");
		}
		PhysicsBodyLayout physicsBodyLayout = new PhysicsBodyLayout();
		physicsBodyLayout.name = _name;
		PhysicsBodyLayout.bodyLayouts[_name] = physicsBodyLayout;
		return physicsBodyLayout;
	}

	// Token: 0x0600424F RID: 16975 RVA: 0x001A0F4C File Offset: 0x0019F14C
	public static PhysicsBodyLayout New()
	{
		int num = 0;
		string text;
		for (;;)
		{
			text = string.Format("unnamed{0}", num);
			if (!PhysicsBodyLayout.bodyLayouts.ContainsKey(text))
			{
				break;
			}
			num++;
		}
		return PhysicsBodyLayout.New(text);
	}

	// Token: 0x06004250 RID: 16976 RVA: 0x001A0F85 File Offset: 0x0019F185
	public bool Rename(string newName)
	{
		if (PhysicsBodyLayout.bodyLayouts.ContainsKey(newName))
		{
			return false;
		}
		PhysicsBodyLayout.bodyLayouts.Remove(this.name);
		PhysicsBodyLayout.bodyLayouts[newName] = this;
		this.name = newName;
		return true;
	}

	// Token: 0x06004251 RID: 16977 RVA: 0x001A0FBB File Offset: 0x0019F1BB
	public static bool Remove(string _name)
	{
		return PhysicsBodyLayout.bodyLayouts.Remove(_name);
	}

	// Token: 0x06004252 RID: 16978 RVA: 0x001A0FC8 File Offset: 0x0019F1C8
	public static PhysicsBodyLayout Find(string _name)
	{
		PhysicsBodyLayout physicsBodyLayout = null;
		PhysicsBodyLayout.bodyLayouts.TryGetValue(_name, out physicsBodyLayout);
		return physicsBodyLayout;
	}

	// Token: 0x17000723 RID: 1827
	// (get) Token: 0x06004253 RID: 16979 RVA: 0x001A0FE8 File Offset: 0x0019F1E8
	public static PhysicsBodyLayout[] BodyLayouts
	{
		get
		{
			PhysicsBodyLayout[] array = new PhysicsBodyLayout[PhysicsBodyLayout.bodyLayouts.Count];
			PhysicsBodyLayout.bodyLayouts.CopyValuesTo<string, PhysicsBodyLayout>(array);
			return array;
		}
	}

	// Token: 0x06004254 RID: 16980 RVA: 0x001A1011 File Offset: 0x0019F211
	public static void Reset()
	{
		PhysicsBodyLayout.bodyLayouts.Clear();
	}

	// Token: 0x06004255 RID: 16981 RVA: 0x001A1020 File Offset: 0x0019F220
	public static PhysicsBodyLayout Read(XElement _e)
	{
		if (!_e.HasAttribute("name"))
		{
			throw new Exception("Physics body needs a name");
		}
		PhysicsBodyLayout physicsBodyLayout = PhysicsBodyLayout.New(_e.GetAttribute("name"));
		foreach (XElement xelement in _e.Elements("collider"))
		{
			physicsBodyLayout.colliders.Add(PhysicsBodyColliderConfiguration.Read(xelement));
		}
		return physicsBodyLayout;
	}

	// Token: 0x06004256 RID: 16982 RVA: 0x001A10B8 File Offset: 0x0019F2B8
	public void Write(XmlElement _elem)
	{
		XmlElement xmlElement = _elem.AddXmlElement("body").SetAttrib("name", this.name);
		for (int i = 0; i < this.colliders.Count; i++)
		{
			this.colliders[i].Write(xmlElement);
		}
	}

	// Token: 0x040035F2 RID: 13810
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<string, PhysicsBodyLayout> bodyLayouts = new Dictionary<string, PhysicsBodyLayout>();

	// Token: 0x040035F3 RID: 13811
	[PublicizedFrom(EAccessModifier.Private)]
	public List<PhysicsBodyColliderConfiguration> colliders = new List<PhysicsBodyColliderConfiguration>();

	// Token: 0x040035F4 RID: 13812
	[PublicizedFrom(EAccessModifier.Private)]
	public string name;
}
