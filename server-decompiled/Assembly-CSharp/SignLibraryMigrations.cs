using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

// Token: 0x02000AA4 RID: 2724
public static class SignLibraryMigrations
{
	// Token: 0x060051BA RID: 20922 RVA: 0x001F331C File Offset: 0x001F151C
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

	// Token: 0x060051BB RID: 20923 RVA: 0x001F33B0 File Offset: 0x001F15B0
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

	// Token: 0x060051BC RID: 20924 RVA: 0x001F33DC File Offset: 0x001F15DC
	[PublicizedFrom(EAccessModifier.Private)]
	public static XElement VisitLayers(XElement root, Action<XElement, string> visitor)
	{
		foreach (XElement xelement in root.Elements(XNames.sign))
		{
			SignLibraryMigrations.<VisitLayers>g__ProcessLayers|4_0(xelement.Elements("layer"), visitor);
		}
		return root;
	}

	// Token: 0x060051BD RID: 20925 RVA: 0x001290F5 File Offset: 0x001272F5
	[PublicizedFrom(EAccessModifier.Private)]
	public static XElement MigrateV0ToV1(XElement root)
	{
		return root;
	}

	// Token: 0x060051BE RID: 20926 RVA: 0x001F343C File Offset: 0x001F163C
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

	// Token: 0x060051C0 RID: 20928 RVA: 0x001F3498 File Offset: 0x001F1698
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

	// Token: 0x04003F3B RID: 16187
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

	// Token: 0x02000AA5 RID: 2725
	// (Invoke) Token: 0x060051C2 RID: 20930
	public delegate XElement MigrationFunc(XElement root);
}
