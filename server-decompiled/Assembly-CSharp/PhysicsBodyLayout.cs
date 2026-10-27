using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

// Token: 0x0200090E RID: 2318
public class PhysicsBodyLayout
{
	// Token: 0x17000727 RID: 1831
	// (get) Token: 0x0600426F RID: 17007 RVA: 0x001A1FA3 File Offset: 0x001A01A3
	public string Name
	{
		get
		{
			return this.name;
		}
	}

	// Token: 0x17000728 RID: 1832
	// (get) Token: 0x06004270 RID: 17008 RVA: 0x001A1FAB File Offset: 0x001A01AB
	public List<PhysicsBodyColliderConfiguration> Colliders
	{
		get
		{
			return this.colliders;
		}
	}

	// Token: 0x06004271 RID: 17009 RVA: 0x001A1FB4 File Offset: 0x001A01B4
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

	// Token: 0x06004272 RID: 17010 RVA: 0x001A1FF4 File Offset: 0x001A01F4
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

	// Token: 0x06004273 RID: 17011 RVA: 0x001A202D File Offset: 0x001A022D
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

	// Token: 0x06004274 RID: 17012 RVA: 0x001A2063 File Offset: 0x001A0263
	public static bool Remove(string _name)
	{
		return PhysicsBodyLayout.bodyLayouts.Remove(_name);
	}

	// Token: 0x06004275 RID: 17013 RVA: 0x001A2070 File Offset: 0x001A0270
	public static PhysicsBodyLayout Find(string _name)
	{
		PhysicsBodyLayout physicsBodyLayout = null;
		PhysicsBodyLayout.bodyLayouts.TryGetValue(_name, out physicsBodyLayout);
		return physicsBodyLayout;
	}

	// Token: 0x17000729 RID: 1833
	// (get) Token: 0x06004276 RID: 17014 RVA: 0x001A2090 File Offset: 0x001A0290
	public static PhysicsBodyLayout[] BodyLayouts
	{
		get
		{
			PhysicsBodyLayout[] array = new PhysicsBodyLayout[PhysicsBodyLayout.bodyLayouts.Count];
			PhysicsBodyLayout.bodyLayouts.CopyValuesTo<string, PhysicsBodyLayout>(array);
			return array;
		}
	}

	// Token: 0x06004277 RID: 17015 RVA: 0x001A20B9 File Offset: 0x001A02B9
	public static void Reset()
	{
		PhysicsBodyLayout.bodyLayouts.Clear();
	}

	// Token: 0x06004278 RID: 17016 RVA: 0x001A20C8 File Offset: 0x001A02C8
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

	// Token: 0x06004279 RID: 17017 RVA: 0x001A2160 File Offset: 0x001A0360
	public void Write(XmlElement _elem)
	{
		XmlElement xmlElement = _elem.AddXmlElement("body").SetAttrib("name", this.name);
		for (int i = 0; i < this.colliders.Count; i++)
		{
			this.colliders[i].Write(xmlElement);
		}
	}

	// Token: 0x04003612 RID: 13842
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<string, PhysicsBodyLayout> bodyLayouts = new Dictionary<string, PhysicsBodyLayout>();

	// Token: 0x04003613 RID: 13843
	[PublicizedFrom(EAccessModifier.Private)]
	public List<PhysicsBodyColliderConfiguration> colliders = new List<PhysicsBodyColliderConfiguration>();

	// Token: 0x04003614 RID: 13844
	[PublicizedFrom(EAccessModifier.Private)]
	public string name;
}
