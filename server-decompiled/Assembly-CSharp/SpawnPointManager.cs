using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;

// Token: 0x02000BCD RID: 3021
public class SpawnPointManager : ISelectionBoxCallback
{
	// Token: 0x06005AE5 RID: 23269 RVA: 0x002316E7 File Offset: 0x0022F8E7
	public SpawnPointManager(bool _bAddToSelectionBoxes = true)
	{
		this.bAddToSelectionBoxes = _bAddToSelectionBoxes;
		if (_bAddToSelectionBoxes)
		{
			SelectionBoxManager.Instance.CategoryStartPoint.SetCallback(this);
		}
	}

	// Token: 0x06005AE6 RID: 23270 RVA: 0x00231714 File Offset: 0x0022F914
	public void Cleanup()
	{
		if (this.bAddToSelectionBoxes)
		{
			SelectionBoxManager.Instance.CategoryStartPoint.Clear();
		}
	}

	// Token: 0x06005AE7 RID: 23271 RVA: 0x00020119 File Offset: 0x0001E319
	public bool OnSelectionBoxActivated(SelectionBox _box, bool _bActivated)
	{
		return true;
	}

	// Token: 0x06005AE8 RID: 23272 RVA: 0x00231730 File Offset: 0x0022F930
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

	// Token: 0x06005AE9 RID: 23273 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnSelectionBoxSized(SelectionBox _box, int _dTop, int _dBottom, int _dNorth, int _dSouth, int _dEast, int _dWest)
	{
	}

	// Token: 0x06005AEA RID: 23274 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnSelectionBoxMirrored(Vector3i _axis)
	{
	}

	// Token: 0x06005AEB RID: 23275 RVA: 0x002317A8 File Offset: 0x0022F9A8
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

	// Token: 0x06005AEC RID: 23276 RVA: 0x0014F1CE File Offset: 0x0014D3CE
	public bool OnSelectionBoxIsAvailable(EnumSelectionBoxAvailabilities _criteria)
	{
		return _criteria == EnumSelectionBoxAvailabilities.CanShowProperties;
	}

	// Token: 0x06005AED RID: 23277 RVA: 0x002317E4 File Offset: 0x0022F9E4
	public void OnSelectionBoxShowProperties(bool _bVisible, GUIWindowManager _windowManager)
	{
		_windowManager.SwitchVisible(XUiC_StartPointEditor.ID, true);
	}

	// Token: 0x06005AEE RID: 23278 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnSelectionBoxRotated(SelectionBox _box)
	{
	}

	// Token: 0x06005AEF RID: 23279 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnSelectionBoxUserDataChanged(SelectionBox _box)
	{
	}

	// Token: 0x06005AF0 RID: 23280 RVA: 0x002317F4 File Offset: 0x0022F9F4
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

	// Token: 0x06005AF1 RID: 23281 RVA: 0x00231904 File Offset: 0x0022FB04
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

	// Token: 0x040046B9 RID: 18105
	public SpawnPointList spawnPointList = new SpawnPointList();

	// Token: 0x040046BA RID: 18106
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly bool bAddToSelectionBoxes;
}
