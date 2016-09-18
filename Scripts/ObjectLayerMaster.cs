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
		SerializableDictionary<string, List<LayeredObject>> dict = new SerializableDictionary<string, List<LayeredObject>> ();
		public DataSaveLoadMaster dataSaveLoad;
		public RectTransform scrollContent;
		public ObjectLayerEntry prefab;
		private SerializableDictionary<string, bool> layerMap = new SerializableDictionary<string, bool> ();
		private Dictionary<string, ObjectLayerEntry> entryMap = new Dictionary<string, ObjectLayerEntry> ();
		public string folder = "ObjectLayer";
		public string file = "LayerSettings";


		// Use this for initialization
		void Awake ()
		{
//			dict = new SerializableDictionary<string, List<LayeredObject>> ();
//			layerMap = new SerializableDictionary<string, bool> ();
//			entryMap = new Dictionary<string, ObjectLayerEntry> ();

			dataSaveLoad.AddHandler(DataLoadCallback, typeof(SerializableDictionary<string, bool>));

		}

		void Start ()
		{

			FileInfo fi = new FileInfo (dataSaveLoad.GetFilePath (file, folder));

			print (dataSaveLoad.GetFilePath (folder, file));
			if (fi.Exists) {
				dataSaveLoad.Load (fi, typeof(SerializableDictionary<string, bool>));
			}
		}

		public void AddObject (LayeredObject lo)
		{
			string name = lo.layerName;
//			print (name + ":" + lo.name);
			if (!dict.ContainsKey (name)) {
				dict.Add (name, new List<LayeredObject> ());

				ObjectLayerEntry cde = GameObject.Instantiate (prefab) as ObjectLayerEntry;
				cde.transform.SetParent (scrollContent.transform, false);
				cde.Set (name, this);
				cde.toggle.isOn = lo.visible;

				if(entryMap.ContainsKey(name)) {
					entryMap[name] = cde;
				} else {
					entryMap.Add (name, cde);
				}
				
				if(layerMap.ContainsKey(name)) {
					layerMap[name] = lo.visible;
				} else {
					layerMap.Add (name, lo.visible);
				}
			}
			if(!dict[name].Contains(lo)){
				dict [name].Add (lo);
			}

			if(layerMap.ContainsKey(name)) {
				lo.visible = layerMap [name];
				entryMap [name].toggle.isOn = lo.visible;
			}

		}

		public void RemoveObject(LayeredObject lo){
			string name = lo.layerName;
			if (dict.ContainsKey (name)) {
				dict[name].Remove(lo);
			}
		}

		public void SetLayerVisible (string layer)
		{
			SetLayerVisible (true, layer);
		}

		public void SetLayerInVisible (string layer)
		{
			SetLayerVisible (false, layer);
		}

		public void SetLayerVisible (bool v, string layer)
		{
			if (!dict.ContainsKey (layer)) {
				return;
			}

			List<LayeredObject> los = dict [layer]; 
			foreach (LayeredObject lo in los) {
				if(lo != null && lo.gameObject != null){
					lo.SetVisible (v);
				}
			}
			layerMap [layer] = v;
		}

		public void DataLoadCallback (object o)
		{
			SerializableDictionary<string, bool> data = o as SerializableDictionary<string, bool>;
			foreach (string k in data.Keys){
				if(layerMap.ContainsKey(k))
					layerMap[k] = data[k];
				else
					layerMap.Add (k, data[k]);
			}

			foreach (string key in data.Keys) {
				print (key+":"+data[key]);
				SetLayerVisible (data [key], key);
				if(entryMap.ContainsKey(key))
					entryMap [key].toggle.isOn = data [key];

			}
		}


		void OnDisable(){
			dataSaveLoad.Save (file, folder, layerMap);
		}
	}

}