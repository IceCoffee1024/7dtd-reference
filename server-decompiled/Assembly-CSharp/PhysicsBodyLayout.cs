using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

// Token: 0x020008FF RID: 2303
public class PhysicsBodyLayout
{
	// Token: 0x17000702 RID: 1794
	// (get) Token: 0x060042BA RID: 17082 RVA: 0x001A0DF7 File Offset: 0x0019EFF7
	public string Name
	{
		get
		{
			return this.name;
		}
	}

	// Token: 0x17000703 RID: 1795
	// (get) Token: 0x060042BB RID: 17083 RVA: 0x001A0DFF File Offset: 0x0019EFFF
	public List<PhysicsBodyColliderConfiguration> Colliders
	{
		get
		{
			return this.colliders;
		}
	}

	// Token: 0x060042BC RID: 17084 RVA: 0x001A0E08 File Offset: 0x0019F008
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

	// Token: 0x060042BD RID: 17085 RVA: 0x001A0E48 File Offset: 0x0019F048
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

	// Token: 0x060042BE RID: 17086 RVA: 0x001A0E81 File Offset: 0x0019F081
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

	// Token: 0x060042BF RID: 17087 RVA: 0x001A0EB7 File Offset: 0x0019F0B7
	public static bool Remove(string _name)
	{
		return PhysicsBodyLayout.bodyLayouts.Remove(_name);
	}

	// Token: 0x060042C0 RID: 17088 RVA: 0x001A0EC4 File Offset: 0x0019F0C4
	public static PhysicsBodyLayout Find(string _name)
	{
		PhysicsBodyLayout physicsBodyLayout = null;
		PhysicsBodyLayout.bodyLayouts.TryGetValue(_name, out physicsBodyLayout);
		return physicsBodyLayout;
	}

	// Token: 0x17000704 RID: 1796
	// (get) Token: 0x060042C1 RID: 17089 RVA: 0x001A0EE4 File Offset: 0x0019F0E4
	public static PhysicsBodyLayout[] BodyLayouts
	{
		get
		{
			PhysicsBodyLayout[] array = new PhysicsBodyLayout[PhysicsBodyLayout.bodyLayouts.Count];
			PhysicsBodyLayout.bodyLayouts.CopyValuesTo<string, PhysicsBodyLayout>(array);
			return array;
		}
	}

	// Token: 0x060042C2 RID: 17090 RVA: 0x001A0F0D File Offset: 0x0019F10D
	public static void Reset()
	{
		PhysicsBodyLayout.bodyLayouts.Clear();
	}

	// Token: 0x060042C3 RID: 17091 RVA: 0x001A0F1C File Offset: 0x0019F11C
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

	// Token: 0x060042C4 RID: 17092 RVA: 0x001A0FB4 File Offset: 0x0019F1B4
	public void Write(XmlElement _elem)
	{
		XmlElement xmlElement = _elem.AddXmlElement("body").SetAttrib("name", this.name);
		for (int i = 0; i < this.colliders.Count; i++)
		{
			this.colliders[i].Write(xmlElement);
		}
	}

	// Token: 0x040035FE RID: 13822
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<string, PhysicsBodyLayout> bodyLayouts = new Dictionary<string, PhysicsBodyLayout>();

	// Token: 0x040035FF RID: 13823
	[PublicizedFrom(EAccessModifier.Private)]
	public List<PhysicsBodyColliderConfiguration> colliders = new List<PhysicsBodyColliderConfiguration>();

	// Token: 0x04003600 RID: 13824
	[PublicizedFrom(EAccessModifier.Private)]
	public string name;
}
