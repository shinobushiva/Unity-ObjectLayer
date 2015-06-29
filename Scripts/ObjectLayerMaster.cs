using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataSaveLoad;
using System.IO;
using UnityEngine.UI;

namespace ObjectLayer
{
	public class ObjectLayerMaster : MonoBehaviour
	{
		SerializableDictionary<string, List<LayeredObject>> dict;


		public DataSaveLoadMaster dataSaveLoad;

		public RectTransform scrollContent;
		public ObjectLayerEntry prefab;

		private SerializableDictionary<string, bool> layerMap;

		private Dictionary<string, ObjectLayerEntry> entryMap;
		
		public string folder = "ObjectLayer";
		public string file = "LayerSettings";


		// Use this for initialization
		void Awake ()
		{
			dict = new SerializableDictionary<string, List<LayeredObject>>();
			layerMap = new SerializableDictionary<string, bool>();
			entryMap = new Dictionary<string, ObjectLayerEntry> ();

			dataSaveLoad.dataLoadHandler += DataLoadCallback;

		}


		void Start(){

			FileInfo fi = new FileInfo (dataSaveLoad.GetFilePath(file, folder));

			print (dataSaveLoad.GetFilePath(folder, file));
			if (fi.Exists) {
				dataSaveLoad.Load (fi, typeof(SerializableDictionary<string, bool>));
			}
		}

		public void AddObject(LayeredObject lo){
			string name = lo.layerName;
			if (!dict.ContainsKey (name)) {
				dict.Add(name, new List<LayeredObject>());

				ObjectLayerEntry cde = GameObject.Instantiate(prefab) as ObjectLayerEntry;
				cde.transform.SetParent(scrollContent.transform, false);
				cde.Set(name, this);

				entryMap.Add (name, cde);

				layerMap.Add (name, true);
			}
			dict[name].Add(lo);
		}

		public void SetLayerVisible(string layer){
			SetLayerVisible (true, layer);
		}
		public void SetLayerInVisible(string layer){
			SetLayerVisible (false, layer);
		}

		public void SetLayerVisible(bool v, string layer){
			List<LayeredObject> los = dict [layer]; 
			foreach (LayeredObject lo in los) {
				lo.SetVisible (v);
			}
			layerMap [layer] = v;

			dataSaveLoad.saveDataUI.fileName.text = file;
			dataSaveLoad.saveDataUI.data = layerMap;
			dataSaveLoad.saveDataUI.Approved (true, folder);

		}


		public void DataLoadCallback (object o)
		{
			SerializableDictionary<string, bool> data = o as SerializableDictionary<string, bool>;
			foreach (string key in data.Keys) {
				SetLayerVisible(data[key], key);
				entryMap [key].toggle.isOn = data[key];

			}
		}

		public void ShowSaveUI ()
		{
			
//			dataSaveLoad.ShowSaveDialog ( null );
		}
		
		public void ShowLoadUI ()
		{
//			dataSaveLoad.ShowLoadDialog (typeof(List<Point>));
		}
		
		// Update is called once per frame
		void Update ()
		{
		
		}
	}

}