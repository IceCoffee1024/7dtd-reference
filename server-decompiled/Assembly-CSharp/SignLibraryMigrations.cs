using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

// Token: 0x02000AD0 RID: 2768
public static class SignLibraryMigrations
{
	// Token: 0x06005278 RID: 21112 RVA: 0x001F9CCC File Offset: 0x001F7ECC
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

	// Token: 0x06005279 RID: 21113 RVA: 0x001F9D60 File Offset: 0x001F7F60
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

	// Token: 0x0600527A RID: 21114 RVA: 0x001F9D8C File Offset: 0x001F7F8C
	[PublicizedFrom(EAccessModifier.Private)]
	public static XElement VisitLayers(XElement root, Action<XElement, string> visitor)
	{
		foreach (XElement xelement in root.Elements(XNames.sign))
		{
			SignLibraryMigrations.<VisitLayers>g__ProcessLayers|4_0(xelement.Elements("layer"), visitor);
		}
		return root;
	}

	// Token: 0x0600527B RID: 21115 RVA: 0x0012B879 File Offset: 0x00129A79
	[PublicizedFrom(EAccessModifier.Private)]
	public static XElement MigrateV0ToV1(XElement root)
	{
		return root;
	}

	// Token: 0x0600527C RID: 21116 RVA: 0x001F9DEC File Offset: 0x001F7FEC
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

	// Token: 0x0600527E RID: 21118 RVA: 0x001F9E48 File Offset: 0x001F8048
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

	// Token: 0x04004028 RID: 16424
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

	// Token: 0x02000AD1 RID: 2769
	// (Invoke) Token: 0x06005280 RID: 21120
	public delegate XElement MigrationFunc(XElement root);
}
