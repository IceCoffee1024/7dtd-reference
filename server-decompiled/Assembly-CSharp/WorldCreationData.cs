using System;
using System.Xml.Linq;

// Token: 0x02000C27 RID: 3111
public class WorldCreationData
{
	// Token: 0x06005E47 RID: 24135 RVA: 0x0024CE2C File Offset: 0x0024B02C
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

	// Token: 0x06005E48 RID: 24136 RVA: 0x0024CED4 File Offset: 0x0024B0D4
	public void Apply(World _world, WorldState _worldState)
	{
		if (this.Properties.Values.ContainsKey("ProviderId"))
		{
			_worldState.providerId = (EnumChunkProviderId)int.Parse(this.Properties.Values["ProviderId"]);
		}
	}

	// Token: 0x0400493D RID: 18749
	public const string PropProviderId = "ProviderId";

	// Token: 0x0400493E RID: 18750
	public const string PropWorldEnvironment = "WorldEnvironment";

	// Token: 0x0400493F RID: 18751
	public const string PropWorldEnvironment_Prefab = "Prefab";

	// Token: 0x04004940 RID: 18752
	public const string PropWorldEnvironment_Class = "Class";

	// Token: 0x04004941 RID: 18753
	public DynamicProperties Properties = new DynamicProperties();
}
