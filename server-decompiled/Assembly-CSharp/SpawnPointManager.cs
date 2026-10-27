using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;

// Token: 0x02000BD8 RID: 3032
public class SpawnPointManager : ISelectionBoxCallback
{
	// Token: 0x06005B0A RID: 23306 RVA: 0x00232C43 File Offset: 0x00230E43
	public SpawnPointManager(bool _bAddToSelectionBoxes = true)
	{
		this.bAddToSelectionBoxes = _bAddToSelectionBoxes;
		if (_bAddToSelectionBoxes)
		{
			SelectionBoxManager.Instance.CategoryStartPoint.SetCallback(this);
		}
	}

	// Token: 0x06005B0B RID: 23307 RVA: 0x00232C70 File Offset: 0x00230E70
	public void Cleanup()
	{
		if (this.bAddToSelectionBoxes)
		{
			SelectionBoxManager.Instance.CategoryStartPoint.Clear();
		}
	}

	// Token: 0x06005B0C RID: 23308 RVA: 0x00020119 File Offset: 0x0001E319
	public bool OnSelectionBoxActivated(SelectionBox _box, bool _bActivated)
	{
		return true;
	}

	// Token: 0x06005B0D RID: 23309 RVA: 0x00232C8C File Offset: 0x00230E8C
	public void OnSelectionBoxMoved(SelectionBox _box, Vector3 _moveVector)
	{
		Vector3i vector3i = Vector3i.Parse(_box.name);
		Vector3i vector3i2 = vector3i + new Vector3i(_moveVector);
		_box.SetPositionAndSize(vector3i2, Vector3i.one);
		SelectionCategory category = _box.Category;
		Vector3i vector3i3 = vector3i2;
		category.RenameBox(_box, vector3i3.ToString() ?? "");
		SpawnPoint spawnPoint = this.spawnPointList.Find(vector3i);
		if (spawnPoint != null)
		{
			spawnPoint.spawnPosition.position = vector3i2.ToVector3();
		}
	}

	// Token: 0x06005B0E RID: 23310 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnSelectionBoxSized(SelectionBox _box, int _dTop, int _dBottom, int _dNorth, int _dSouth, int _dEast, int _dWest)
	{
	}

	// Token: 0x06005B0F RID: 23311 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnSelectionBoxMirrored(Vector3i _axis)
	{
	}

	// Token: 0x06005B10 RID: 23312 RVA: 0x00232D04 File Offset: 0x00230F04
	public bool OnSelectionBoxDelete(SelectionBox _box, bool _checkCanDeleteOnly)
	{
		if (_checkCanDeleteOnly)
		{
			return true;
		}
		Vector3i vector3i = Vector3i.Parse(_box.name);
		SpawnPoint spawnPoint = this.spawnPointList.Find(vector3i);
		if (spawnPoint != null)
		{
			this.spawnPointList.Remove(spawnPoint);
		}
		return true;
	}

	// Token: 0x06005B11 RID: 23313 RVA: 0x0014FDC2 File Offset: 0x0014DFC2
	public bool OnSelectionBoxIsAvailable(EnumSelectionBoxAvailabilities _criteria)
	{
		return _criteria == EnumSelectionBoxAvailabilities.CanShowProperties;
	}

	// Token: 0x06005B12 RID: 23314 RVA: 0x00232D40 File Offset: 0x00230F40
	public void OnSelectionBoxShowProperties(bool _bVisible, GUIWindowManager _windowManager)
	{
		_windowManager.SwitchVisible(XUiC_StartPointEditor.ID, true);
	}

	// Token: 0x06005B13 RID: 23315 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnSelectionBoxRotated(SelectionBox _box)
	{
	}

	// Token: 0x06005B14 RID: 23316 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnSelectionBoxUserDataChanged(SelectionBox _box)
	{
	}

	// Token: 0x06005B15 RID: 23317 RVA: 0x00232D50 File Offset: 0x00230F50
	public bool Load(string _path)
	{
		if (!SdFile.Exists(_path + "/spawnpoints.xml"))
		{
			return false;
		}
		try
		{
			foreach (XElement xelement in new XmlFile(_path, "spawnpoints", false, false).XmlDoc.Root.Elements("spawnpoint"))
			{
				Vector3 vector = StringParsers.ParseVector3(xelement.GetAttribute("position"), 0, -1);
				Vector3 vector2 = Vector3.zero;
				if (xelement.HasAttribute("rotation"))
				{
					vector2 = StringParsers.ParseVector3(xelement.GetAttribute("rotation"), 0, -1);
				}
				this.spawnPointList.Add(new SpawnPoint(vector, vector2.y));
			}
		}
		catch (Exception ex)
		{
			Log.Error("Loading spawnpoints xml file for level '" + Path.GetFileName(_path) + "': " + ex.Message);
		}
		return true;
	}

	// Token: 0x06005B16 RID: 23318 RVA: 0x00232E60 File Offset: 0x00231060
	public bool Save(string _path)
	{
		bool flag;
		try
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.CreateXmlDeclaration();
			XmlElement xmlElement = xmlDocument.AddXmlElement("spawnpoints");
			for (int i = 0; i < this.spawnPointList.Count; i++)
			{
				SpawnPoint spawnPoint = this.spawnPointList[i];
				Vector3 position = spawnPoint.spawnPosition.position;
				string text = string.Concat(new string[]
				{
					position.x.ToCultureInvariantString(),
					",",
					position.y.ToCultureInvariantString(),
					",",
					position.z.ToCultureInvariantString()
				});
				xmlElement.AddXmlElement("spawnpoint").SetAttrib("position", text).SetAttrib("rotation", "0," + spawnPoint.spawnPosition.heading.ToCultureInvariantString() + ",0");
			}
			xmlDocument.SdSave(_path + "/spawnpoints.xml");
			flag = true;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			flag = false;
		}
		return flag;
	}

	// Token: 0x040046DE RID: 18142
	public SpawnPointList spawnPointList = new SpawnPointList();

	// Token: 0x040046DF RID: 18143
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly bool bAddToSelectionBoxes;
}
