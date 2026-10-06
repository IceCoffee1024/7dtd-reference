using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

// Token: 0x02000AC6 RID: 2758
public static class SignLibraryMigrations
{
	// Token: 0x06005254 RID: 21076 RVA: 0x001F8BF0 File Offset: 0x001F6DF0
	public static XElement Migrate(XElement root, int fromVersion, int toVersion)
	{
		if (toVersion != fromVersion + 1)
		{
			throw new ArgumentException(string.Format("Migrations must be sequential. Cannot migrate from v{0} to v{1}", fromVersion, toVersion));
		}
		SignLibraryMigrations.MigrationFunc migrationFunc;
		if (!SignLibraryMigrations.Migrations.TryGetValue(fromVersion, out migrationFunc))
		{
			throw new InvalidOperationException(string.Format("No migration registered for v{0} to v{1}", fromVersion, toVersion));
		}
		Log.Out(string.Format("  Applying migration: v{0} -> v{1}", fromVersion, toVersion));
		XElement xelement = migrationFunc(root);
		xelement.SetAttributeValue("version", toVersion);
		return xelement;
	}

	// Token: 0x06005255 RID: 21077 RVA: 0x001F8C84 File Offset: 0x001F6E84
	public static bool CanMigrate(int fromVersion, int toVersion)
	{
		for (int i = fromVersion; i < toVersion; i++)
		{
			if (!SignLibraryMigrations.Migrations.ContainsKey(i))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06005256 RID: 21078 RVA: 0x001F8CB0 File Offset: 0x001F6EB0
	[PublicizedFrom(EAccessModifier.Private)]
	public static XElement VisitLayers(XElement root, Action<XElement, string> visitor)
	{
		foreach (XElement xelement in root.Elements(XNames.sign))
		{
			SignLibraryMigrations.<VisitLayers>g__ProcessLayers|4_0(xelement.Elements("layer"), visitor);
		}
		return root;
	}

	// Token: 0x06005257 RID: 21079 RVA: 0x0012AC85 File Offset: 0x00128E85
	[PublicizedFrom(EAccessModifier.Private)]
	public static XElement MigrateV0ToV1(XElement root)
	{
		return root;
	}

	// Token: 0x06005258 RID: 21080 RVA: 0x001F8D10 File Offset: 0x001F6F10
	[PublicizedFrom(EAccessModifier.Private)]
	public static XElement MigrateV1ToV2(XElement root)
	{
		return SignLibraryMigrations.VisitLayers(root, delegate(XElement layer, string layerType)
		{
			if (layerType == "TextSignLayer")
			{
				layer.SetAttributeValue("direction", 0);
				layer.SetAttributeValue("spacing", 1);
			}
		});
	}

	// Token: 0x0600525A RID: 21082 RVA: 0x001F8D6C File Offset: 0x001F6F6C
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void <VisitLayers>g__ProcessLayers|4_0(IEnumerable<XElement> layers, Action<XElement, string> visitor)
	{
		foreach (XElement xelement in layers)
		{
			string attribute = xelement.GetAttribute("type");
			visitor(xelement, attribute);
			if (attribute == "GroupSignLayer")
			{
				SignLibraryMigrations.<VisitLayers>g__ProcessLayers|4_0(xelement.Elements("layer"), visitor);
			}
		}
	}

	// Token: 0x04004007 RID: 16391
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<int, SignLibraryMigrations.MigrationFunc> Migrations = new Dictionary<int, SignLibraryMigrations.MigrationFunc>
	{
		{
			0,
			new SignLibraryMigrations.MigrationFunc(SignLibraryMigrations.MigrateV0ToV1)
		},
		{
			1,
			new SignLibraryMigrations.MigrationFunc(SignLibraryMigrations.MigrateV1ToV2)
		}
	};

	// Token: 0x02000AC7 RID: 2759
	// (Invoke) Token: 0x0600525C RID: 21084
	public delegate XElement MigrationFunc(XElement root);
}
