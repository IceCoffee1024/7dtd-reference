using System;
using System.Xml.Linq;

// Token: 0x02000C1C RID: 3100
public class WorldCreationData
{
	// Token: 0x06005E22 RID: 24098 RVA: 0x0024B904 File Offset: 0x00249B04
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

	// Token: 0x06005E23 RID: 24099 RVA: 0x0024B9AC File Offset: 0x00249BAC
	public void Apply(World _world, WorldState _worldState)
	{
		if (this.Properties.Values.ContainsKey("ProviderId"))
		{
			_worldState.providerId = (EnumChunkProviderId)int.Parse(this.Properties.Values["ProviderId"]);
		}
	}

	// Token: 0x04004918 RID: 18712
	public const string PropProviderId = "ProviderId";

	// Token: 0x04004919 RID: 18713
	public const string PropWorldEnvironment = "WorldEnvironment";

	// Token: 0x0400491A RID: 18714
	public const string PropWorldEnvironment_Prefab = "Prefab";

	// Token: 0x0400491B RID: 18715
	public const string PropWorldEnvironment_Class = "Class";

	// Token: 0x0400491C RID: 18716
	public DynamicProperties Properties = new DynamicProperties();
}
