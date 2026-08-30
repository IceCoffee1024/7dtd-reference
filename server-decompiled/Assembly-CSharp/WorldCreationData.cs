using System;
using System.Xml.Linq;

// Token: 0x02000BF8 RID: 3064
public class WorldCreationData
{
	// Token: 0x06005D7F RID: 23935 RVA: 0x00245864 File Offset: 0x00243A64
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

	// Token: 0x06005D80 RID: 23936 RVA: 0x0024590C File Offset: 0x00243B0C
	public void Apply(World _world, WorldState _worldState)
	{
		if (this.Properties.Values.ContainsKey("ProviderId"))
		{
			_worldState.providerId = (EnumChunkProviderId)int.Parse(this.Properties.Values["ProviderId"]);
		}
	}

	// Token: 0x0400483D RID: 18493
	public const string PropProviderId = "ProviderId";

	// Token: 0x0400483E RID: 18494
	public const string PropWorldEnvironment = "WorldEnvironment";

	// Token: 0x0400483F RID: 18495
	public const string PropWorldEnvironment_Prefab = "Prefab";

	// Token: 0x04004840 RID: 18496
	public const string PropWorldEnvironment_Class = "Class";

	// Token: 0x04004841 RID: 18497
	public DynamicProperties Properties = new DynamicProperties();
}
