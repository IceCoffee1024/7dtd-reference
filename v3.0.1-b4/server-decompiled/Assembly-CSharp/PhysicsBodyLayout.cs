using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

// Token: 0x020008F3 RID: 2291
public class PhysicsBodyLayout
{
	// Token: 0x170006F6 RID: 1782
	// (get) Token: 0x06004255 RID: 16981 RVA: 0x0019ED1F File Offset: 0x0019CF1F
	public string Name
	{
		get
		{
			return this.name;
		}
	}

	// Token: 0x170006F7 RID: 1783
	// (get) Token: 0x06004256 RID: 16982 RVA: 0x0019ED27 File Offset: 0x0019CF27
	public List<PhysicsBodyColliderConfiguration> Colliders
	{
		get
		{
			return this.colliders;
		}
	}

	// Token: 0x06004257 RID: 16983 RVA: 0x0019ED30 File Offset: 0x0019CF30
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

	// Token: 0x06004258 RID: 16984 RVA: 0x0019ED70 File Offset: 0x0019CF70
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

	// Token: 0x06004259 RID: 16985 RVA: 0x0019EDA9 File Offset: 0x0019CFA9
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

	// Token: 0x0600425A RID: 16986 RVA: 0x0019EDDF File Offset: 0x0019CFDF
	public static bool Remove(string _name)
	{
		return PhysicsBodyLayout.bodyLayouts.Remove(_name);
	}

	// Token: 0x0600425B RID: 16987 RVA: 0x0019EDEC File Offset: 0x0019CFEC
	public static PhysicsBodyLayout Find(string _name)
	{
		PhysicsBodyLayout physicsBodyLayout = null;
		PhysicsBodyLayout.bodyLayouts.TryGetValue(_name, out physicsBodyLayout);
		return physicsBodyLayout;
	}

	// Token: 0x170006F8 RID: 1784
	// (get) Token: 0x0600425C RID: 16988 RVA: 0x0019EE0C File Offset: 0x0019D00C
	public static PhysicsBodyLayout[] BodyLayouts
	{
		get
		{
			PhysicsBodyLayout[] array = new PhysicsBodyLayout[PhysicsBodyLayout.bodyLayouts.Count];
			PhysicsBodyLayout.bodyLayouts.CopyValuesTo<string, PhysicsBodyLayout>(array);
			return array;
		}
	}

	// Token: 0x0600425D RID: 16989 RVA: 0x0019EE35 File Offset: 0x0019D035
	public static void Reset()
	{
		PhysicsBodyLayout.bodyLayouts.Clear();
	}

	// Token: 0x0600425E RID: 16990 RVA: 0x0019EE44 File Offset: 0x0019D044
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

	// Token: 0x0600425F RID: 16991 RVA: 0x0019EEDC File Offset: 0x0019D0DC
	public void Write(XmlElement _elem)
	{
		XmlElement xmlElement = _elem.AddXmlElement("body").SetAttrib("name", this.name);
		for (int i = 0; i < this.colliders.Count; i++)
		{
			this.colliders[i].Write(xmlElement);
		}
	}

	// Token: 0x0400357D RID: 13693
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<string, PhysicsBodyLayout> bodyLayouts = new Dictionary<string, PhysicsBodyLayout>();

	// Token: 0x0400357E RID: 13694
	[PublicizedFrom(EAccessModifier.Private)]
	public List<PhysicsBodyColliderConfiguration> colliders = new List<PhysicsBodyColliderConfiguration>();

	// Token: 0x0400357F RID: 13695
	[PublicizedFrom(EAccessModifier.Private)]
	public string name;
}
