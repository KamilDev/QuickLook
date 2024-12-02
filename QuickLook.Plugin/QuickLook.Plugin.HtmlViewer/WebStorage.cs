using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace QuickLook.Plugin.HtmlViewer
{
    [ComVisible(true)]
    public class WebStorage
    {
        private readonly string _storageFile;
        private Dictionary<string, string> _storage;

        public WebStorage(string storageFile)
        {
            _storageFile = storageFile;
            LoadStorage();
        }

        private void LoadStorage()
        {
            if (File.Exists(_storageFile))
            {
                var json = File.ReadAllText(_storageFile);
                _storage = JsonConvert.DeserializeObject<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
            }
            else
            {
                _storage = new Dictionary<string, string>();
            }
        }

        private void SaveStorage()
        {
            var json = JsonConvert.SerializeObject(_storage);
            File.WriteAllText(_storageFile, json);
        }

        public string GetItem(string key)
        {
            return _storage.TryGetValue(key, out var value) ? value : null;
        }

        public void SetItem(string key, string value)
        {
            _storage[key] = value;
            SaveStorage();
        }

        public void RemoveItem(string key)
        {
            if (_storage.Remove(key))
            {
                SaveStorage();
            }
        }

        public void Clear()
        {
            _storage.Clear();
            SaveStorage();
        }

        public int Length => _storage.Count;
    }
}
