using System;
using System.IO;
using System.Linq;

// Token: 0x020014AD RID: 5293
public class StockFileHashes
{
	// Token: 0x0600A5FB RID: 42491 RVA: 0x003DB9DC File Offset: 0x003D9BDC
	public static bool HasStockXMLs()
	{
		string applicationPath = GameIO.GetApplicationPath();
		bool flag = true;
		foreach (StockFileHashes.HashDef hashDef in StockFileHashes.hashDefinitions)
		{
			if (File.Exists(applicationPath + "/" + hashDef.filename) && !IOUtils.CalcHashSync(applicationPath + "/" + hashDef.filename, "MD5").SequenceEqual<byte>(hashDef.hash))
			{
				Log.Out("Wrong hash on " + hashDef.filename);
				flag = false;
			}
		}
		return flag;
	}

	// Token: 0x04007BA0 RID: 31648
	[PublicizedFrom(EAccessModifier.Private)]
	public static StockFileHashes.HashDef[] hashDefinitions = new StockFileHashes.HashDef[]
	{
		new StockFileHashes.HashDef("Data/Config/archetypes.xml", new byte[]
		{
			129, 149, 214, 213, 160, 126, 29, 253, 240, 10,
			8, 204, 99, 252, 199, 182
		}),
		new StockFileHashes.HashDef("Data/Config/biomes.xml", new byte[]
		{
			194, 85, 165, 41, 188, 8, 172, 220, 66, 3,
			125, 67, 22, 213, 77, 66
		}),
		new StockFileHashes.HashDef("Data/Config/blockplaceholders.xml", new byte[]
		{
			139, 41, 168, 124, 142, 8, 20, 230, 200, 231,
			138, 138, 190, 208, 183, 207
		}),
		new StockFileHashes.HashDef("Data/Config/blocks.xml", new byte[]
		{
			58, 23, 157, 232, 155, 221, 154, 141, 129, 28,
			21, 138, 65, 73, 79, 193
		}),
		new StockFileHashes.HashDef("Data/Config/buffs.xml", new byte[]
		{
			116, 90, 179, 235, 56, 225, 112, 57, 100, 230,
			170, 176, 147, 215, 136, 118
		}),
		new StockFileHashes.HashDef("Data/Config/challenges.xml", new byte[]
		{
			164, 121, 16, 114, 79, 92, 158, 194, 62, 179,
			17, 221, 162, 77, 151, 49
		}),
		new StockFileHashes.HashDef("Data/Config/devicesgameprefs.xml", new byte[]
		{
			196, 158, 114, 144, 153, 61, 251, 22, 157, 95,
			34, 159, 164, 137, 247, 189
		}),
		new StockFileHashes.HashDef("Data/Config/dialogs.xml", new byte[]
		{
			235, 32, 125, 251, 192, 114, 65, 228, 1, 103,
			115, 141, 117, 159, 142, 168
		}),
		new StockFileHashes.HashDef("Data/Config/dmscontent.xml", new byte[]
		{
			146, 234, 236, 139, 101, 159, 66, 55, 76, 140,
			14, 56, 174, 18, 22, 93
		}),
		new StockFileHashes.HashDef("Data/Config/entityclasses.xml", new byte[]
		{
			61, 162, 68, 192, 120, 31, 152, 40, 187, 232,
			252, 133, 127, 86, 208, 230
		}),
		new StockFileHashes.HashDef("Data/Config/entitygroups.xml", new byte[]
		{
			240, 188, 156, 135, 68, 154, 217, 111, 209, 126,
			54, 85, 160, 39, 154, 228
		}),
		new StockFileHashes.HashDef("Data/Config/events.xml", new byte[]
		{
			69, 102, 250, 190, 70, 178, 36, 89, 123, 60,
			95, 204, 74, 75, 181, 17
		}),
		new StockFileHashes.HashDef("Data/Config/gameevents.xml", new byte[]
		{
			93, 117, 27, 185, 193, 85, 77, 133, 106, 42,
			49, 146, 139, 216, 8, 198
		}),
		new StockFileHashes.HashDef("Data/Config/gamestages.xml", new byte[]
		{
			76, 83, 146, 37, 153, byte.MaxValue, 57, 173, 175, 213,
			208, 241, 75, 81, 85, 10
		}),
		new StockFileHashes.HashDef("Data/Config/items.xml", new byte[]
		{
			26, 184, 184, 165, 47, 154, 210, 121, 64, byte.MaxValue,
			12, 183, 51, 114, 34, 199
		}),
		new StockFileHashes.HashDef("Data/Config/item_modifiers.xml", new byte[]
		{
			55, 97, 51, 91, 199, 68, 133, 25, 51, 166,
			46, 109, 61, 45, 107, 151
		}),
		new StockFileHashes.HashDef("Data/Config/loadingscreen.xml", new byte[]
		{
			208, 45, 145, 77, 51, 50, 231, 175, 26, 247,
			106, 197, 251, 82, 209, 80
		}),
		new StockFileHashes.HashDef("Data/Config/loot.xml", new byte[]
		{
			168, 220, 156, 102, 1, 216, 240, 143, 78, 252,
			54, 115, 57, 26, 225, 45
		}),
		new StockFileHashes.HashDef("Data/Config/materials.xml", new byte[]
		{
			128, 141, 183, 99, 200, 240, 79, 65, 81, 66,
			166, 176, 178, 225, 15, 178
		}),
		new StockFileHashes.HashDef("Data/Config/misc.xml", new byte[]
		{
			221, 223, 126, 194, 10, 67, 126, 120, 75, 188,
			233, 72, 150, 146, 41, 136
		}),
		new StockFileHashes.HashDef("Data/Config/music.xml", new byte[]
		{
			81, 85, 5, 120, 112, 228, 213, 50, 7, 73,
			121, 96, 243, 210, 139, 204
		}),
		new StockFileHashes.HashDef("Data/Config/nav_objects.xml", new byte[]
		{
			181, 226, 70, 45, 89, 206, 134, 249, 183, 2,
			53, 181, 134, 213, 105, 81
		}),
		new StockFileHashes.HashDef("Data/Config/npc.xml", new byte[]
		{
			39, 115, 86, 80, 152, 202, 155, 64, 250, 199,
			252, 24, 201, 139, 196, 27
		}),
		new StockFileHashes.HashDef("Data/Config/painting.xml", new byte[]
		{
			149, 174, 31, 175, 31, 238, 140, 122, 184, 224,
			212, 82, 77, 56, 99, 251
		}),
		new StockFileHashes.HashDef("Data/Config/physicsbodies.xml", new byte[]
		{
			123, 240, 13, 225, 25, 233, 87, 207, 179, 189,
			37, 15, 152, 11, 70, 169
		}),
		new StockFileHashes.HashDef("Data/Config/progression.xml", new byte[]
		{
			28, 116, 44, 250, 123, 60, 208, 9, 148, 192,
			99, 57, 94, 245, 94, 176
		}),
		new StockFileHashes.HashDef("Data/Config/qualityinfo.xml", new byte[]
		{
			97, 120, 145, 33, 103, 203, 2, 168, 192, 21,
			25, 195, 68, 215, 251, 211
		}),
		new StockFileHashes.HashDef("Data/Config/quests.xml", new byte[]
		{
			84, 105, 216, 120, 249, 78, 49, 224, 77, 234,
			59, 162, 85, 199, 118, 48
		}),
		new StockFileHashes.HashDef("Data/Config/recipes.xml", new byte[]
		{
			89, 170, 49, 128, 10, 19, 64, 79, 101, 115,
			177, 244, 186, 233, 141, 89
		}),
		new StockFileHashes.HashDef("Data/Config/rwgmixer.xml", new byte[]
		{
			163, 248, 8, 41, 103, 28, 207, 98, 108, 216,
			40, 12, 157, 113, 13, 145
		}),
		new StockFileHashes.HashDef("Data/Config/sandbox_overrides.xml", new byte[]
		{
			21, 211, 79, 241, 118, 82, 75, 217, 111, 77,
			136, 74, 233, 17, 164, 104
		}),
		new StockFileHashes.HashDef("Data/Config/shapes.xml", new byte[]
		{
			217, 222, 243, 93, 36, 150, 57, 2, 1, 225,
			157, 135, 116, 167, 185, 90
		}),
		new StockFileHashes.HashDef("Data/Config/signs.xml", new byte[]
		{
			241, 151, 215, 160, 187, 129, 79, 33, 145, 140,
			116, 42, 155, 195, 126, 16
		}),
		new StockFileHashes.HashDef("Data/Config/sounds.xml", new byte[]
		{
			110, 53, 180, 150, 163, 127, 167, 158, 44, 187,
			61, 30, 129, 2, 87, 88
		}),
		new StockFileHashes.HashDef("Data/Config/spawning.xml", new byte[]
		{
			211, 219, 214, 36, 20, 216, 165, 160, 52, 84,
			156, 218, 206, 73, 88, 167
		}),
		new StockFileHashes.HashDef("Data/Config/subtitles.xml", new byte[]
		{
			43, 226, 138, 234, 5, 182, 101, 56, 134, 12,
			29, 180, 158, 99, 28, 188
		}),
		new StockFileHashes.HashDef("Data/Config/traders.xml", new byte[]
		{
			74, 137, 30, 252, 245, 173, 205, 147, 169, 54,
			37, 67, 36, 158, 71, 22
		}),
		new StockFileHashes.HashDef("Data/Config/twitch.xml", new byte[]
		{
			37, 160, 24, 184, 126, 200, 188, 187, 142, 13,
			183, 134, 206, 174, 38, 110
		}),
		new StockFileHashes.HashDef("Data/Config/twitch_events.xml", new byte[]
		{
			2, 109, 178, 40, 65, 165, 27, 17, 153, 161,
			161, 105, 193, 80, 223, 163
		}),
		new StockFileHashes.HashDef("Data/Config/ui_display.xml", new byte[]
		{
			38, 125, 237, 77, 161, 159, 39, 134, 83, 21,
			78, 240, 100, 244, 50, 217
		}),
		new StockFileHashes.HashDef("Data/Config/utilityai.xml", new byte[]
		{
			214, 167, 177, 22, 147, 156, 88, 129, 207, 113,
			61, 241, 70, 38, 155, 75
		}),
		new StockFileHashes.HashDef("Data/Config/vehicles.xml", new byte[]
		{
			108, 137, 208, 51, 201, 85, 185, 9, 230, 94,
			52, 130, 105, 1, 82, 111
		}),
		new StockFileHashes.HashDef("Data/Config/videos.xml", new byte[]
		{
			111, 28, 117, 204, 29, 196, 6, 21, 182, 82,
			6, 72, 67, 229, 163, 234
		}),
		new StockFileHashes.HashDef("Data/Config/weathersurvival.xml", new byte[]
		{
			151, 245, 70, 52, 118, 190, 215, 193, 5, 166,
			162, 78, 110, 62, 129, 210
		}),
		new StockFileHashes.HashDef("Data/Config/worldglobal.xml", new byte[]
		{
			238, 79, 96, 194, 24, 77, 208, 166, 155, 226,
			190, 215, 20, 216, 134, 75
		}),
		new StockFileHashes.HashDef("Data/Config/XUi_Common/styles.xml", new byte[]
		{
			151, 8, 249, 57, 235, 64, 135, 36, 36, 174,
			246, 19, 115, 202, 254, 239
		}),
		new StockFileHashes.HashDef("Data/Config/XUi_Common/templates.xml", new byte[]
		{
			81, 118, 40, 78, 194, 100, 26, 50, 207, 158,
			159, 70, 115, 10, 76, 79
		}),
		new StockFileHashes.HashDef("Data/Config/XUi_InGame/styles.xml", new byte[]
		{
			85, 175, 136, 53, 223, 110, 185, 11, 201, 158,
			162, 237, 134, 74, 144, 186
		}),
		new StockFileHashes.HashDef("Data/Config/XUi_InGame/templates.xml", new byte[]
		{
			239, 187, 239, 230, 168, 202, 92, 186, 102, 7,
			176, 167, 189, 221, 185, 84
		}),
		new StockFileHashes.HashDef("Data/Config/XUi_InGame/windows.xml", new byte[]
		{
			198, 214, byte.MaxValue, 180, 95, 5, 136, 133, 39, 68,
			101, 25, 2, 155, 92, 15
		}),
		new StockFileHashes.HashDef("Data/Config/XUi_InGame/xui.xml", new byte[]
		{
			206, 120, 193, 22, 130, 164, 166, 7, 87, 101,
			119, 37, 207, 234, 248, 30
		})
	};

	// Token: 0x020014AE RID: 5294
	[PublicizedFrom(EAccessModifier.Private)]
	public struct HashDef
	{
		// Token: 0x0600A5FE RID: 42494 RVA: 0x003DC1A8 File Offset: 0x003DA3A8
		public HashDef(string _filename, byte[] _hash)
		{
			this.filename = _filename;
			this.hash = _hash;
		}

		// Token: 0x04007BA1 RID: 31649
		public string filename;

		// Token: 0x04007BA2 RID: 31650
		public byte[] hash;
	}
}
