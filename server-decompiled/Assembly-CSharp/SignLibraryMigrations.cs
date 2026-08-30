using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

// Token: 0x02000AB5 RID: 2741
public static class SignLibraryMigrations
{
	// Token: 0x06005240 RID: 21056 RVA: 0x001F5DB4 File Offset: 0x001F3FB4
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

	// Token: 0x06005241 RID: 21057 RVA: 0x001F5E48 File Offset: 0x001F4048
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

	// Token: 0x06005242 RID: 21058 RVA: 0x001F5E74 File Offset: 0x001F4074
	[PublicizedFrom(EAccessModifier.Private)]
	public static XElement VisitLayers(XElement root, Action<XElement, string> visitor)
	{
		foreach (XElement xelement in root.Elements(XNames.sign))
		{
			SignLibraryMigrations.<VisitLayers>g__ProcessLayers|4_0(xelement.Elements("layer"), visitor);
		}
		return root;
	}

	// Token: 0x06005243 RID: 21059 RVA: 0x0012A165 File Offset: 0x00128365
	[PublicizedFrom(EAccessModifier.Private)]
	public static XElement MigrateV0ToV1(XElement root)
	{
		return root;
	}

	// Token: 0x06005244 RID: 21060 RVA: 0x001F5ED4 File Offset: 0x001F40D4
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

	// Token: 0x06005246 RID: 21062 RVA: 0x001F5F30 File Offset: 0x001F4130
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

	// Token: 0x04003FCC RID: 16332
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

	// Token: 0x02000AB6 RID: 2742
	// (Invoke) Token: 0x06005248 RID: 21064
	public delegate XElement MigrationFunc(XElement root);
}
