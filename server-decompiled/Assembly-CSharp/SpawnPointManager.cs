using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;

// Token: 0x02000BBB RID: 3003
public class SpawnPointManager : ISelectionBoxCallback
{
	// Token: 0x06005ACC RID: 23244 RVA: 0x0022E3EB File Offset: 0x0022C5EB
	public SpawnPointManager(bool _bAddToSelectionBoxes = true)
	{
		this.bAddToSelectionBoxes = _bAddToSelectionBoxes;
		if (_bAddToSelectionBoxes)
		{
			SelectionBoxManager.Instance.CategoryStartPoint.SetCallback(this);
		}
	}

	// Token: 0x06005ACD RID: 23245 RVA: 0x0022E418 File Offset: 0x0022C618
	public void Cleanup()
	{
		if (this.bAddToSelectionBoxes)
		{
			SelectionBoxManager.Instance.CategoryStartPoint.Clear();
		}
	}

	// Token: 0x06005ACE RID: 23246 RVA: 0x00020021 File Offset: 0x0001E221
	public bool OnSelectionBoxActivated(SelectionBox _box, bool _bActivated)
	{
		return true;
	}

	// Token: 0x06005ACF RID: 23247 RVA: 0x0022E434 File Offset: 0x0022C634
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

	// Token: 0x06005AD0 RID: 23248 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnSelectionBoxSized(SelectionBox _box, int _dTop, int _dBottom, int _dNorth, int _dSouth, int _dEast, int _dWest)
	{
	}

	// Token: 0x06005AD1 RID: 23249 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnSelectionBoxMirrored(Vector3i _axis)
	{
	}

	// Token: 0x06005AD2 RID: 23250 RVA: 0x0022E4AC File Offset: 0x0022C6AC
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

	// Token: 0x06005AD3 RID: 23251 RVA: 0x0014E85E File Offset: 0x0014CA5E
	public bool OnSelectionBoxIsAvailable(EnumSelectionBoxAvailabilities _criteria)
	{
		return _criteria == EnumSelectionBoxAvailabilities.CanShowProperties;
	}

	// Token: 0x06005AD4 RID: 23252 RVA: 0x0022E4E8 File Offset: 0x0022C6E8
	public void OnSelectionBoxShowProperties(bool _bVisible, GUIWindowManager _windowManager)
	{
		_windowManager.SwitchVisible(XUiC_StartPointEditor.ID, true);
	}

	// Token: 0x06005AD5 RID: 23253 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnSelectionBoxRotated(SelectionBox _box)
	{
	}

	// Token: 0x06005AD6 RID: 23254 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnSelectionBoxUserDataChanged(SelectionBox _box)
	{
	}

	// Token: 0x06005AD7 RID: 23255 RVA: 0x0022E4F8 File Offset: 0x0022C6F8
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

	// Token: 0x06005AD8 RID: 23256 RVA: 0x0022E608 File Offset: 0x0022C808
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

	// Token: 0x0400467A RID: 18042
	public SpawnPointList spawnPointList = new SpawnPointList();

	// Token: 0x0400467B RID: 18043
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly bool bAddToSelectionBoxes;
}
