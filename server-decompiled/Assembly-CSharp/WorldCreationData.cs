using System;
using System.Xml.Linq;

// Token: 0x02000C0A RID: 3082
public class WorldCreationData
{
	// Token: 0x06005E09 RID: 24073 RVA: 0x00248608 File Offset: 0x00246808
	public WorldCreationData(string _levelDir)
	{
		try
		{
			XDocument xdocument = SdXDocument.Load(_levelDir + "/world.xml");
			if (xdocument.Root == null)
			{
				throw new Exception("No root node in world.xml!");
			}
			foreach (XElement xelement in xdocument.Root.Elements("property"))
			{
				this.Properties.Add(xelement, false);
			}
		}
		catch (Exception)
		{
		}
	}

	// Token: 0x06005E0A RID: 24074 RVA: 0x002486B0 File Offset: 0x002468B0
	public void Apply(World _world, WorldState _worldState)
	{
		if (this.Properties.Values.ContainsKey("ProviderId"))
		{
			_worldState.providerId = (EnumChunkProviderId)int.Parse(this.Properties.Values["ProviderId"]);
		}
	}

	// Token: 0x040048D9 RID: 18649
	public const string PropProviderId = "ProviderId";

	// Token: 0x040048DA RID: 18650
	public const string PropWorldEnvironment = "WorldEnvironment";

	// Token: 0x040048DB RID: 18651
	public const string PropWorldEnvironment_Prefab = "Prefab";

	// Token: 0x040048DC RID: 18652
	public const string PropWorldEnvironment_Class = "Class";

	// Token: 0x040048DD RID: 18653
	public DynamicProperties Properties = new DynamicProperties();
}
