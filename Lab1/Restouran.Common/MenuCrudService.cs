using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Restouran.Common
{
    public class MenuCrudService : ICrudService<MenuItem>
    {

        private List<MenuItem> _items = new List<MenuItem> ();
        public void Create(MenuItem element)
        {
            element.IdItem = Guid.NewGuid();
            _items.Add(element);
        }

        public void Load(string FilePath)
        {
            if (File.Exists(FilePath))
            {
                string jsonString = File.ReadAllText(FilePath);

                if (!string.IsNullOrEmpty(jsonString))
                {
                    var loadedItems = JsonSerializer.Deserialize<List<MenuItem>>(jsonString);

                    if (loadedItems != null)
                    {
                        _items = loadedItems;
                    }
                }
            }
        }

        public MenuItem Read(Guid id)
        {
            return _items.FirstOrDefault(item => item.IdItem == id);
        }


        public void Remove(MenuItem element)
        {
            var temp = Read(element.IdItem);
            if(temp != null) 
                _items.Remove(temp);
        }

        public void Save(string FilePath)
        {
            string jsonString = JsonSerializer.Serialize(_items);
            File.WriteAllText(FilePath, jsonString);

        }

        public void Update(MenuItem element)
        {
            var temp = Read(element.IdItem);
            if (temp != null)
            {
                var index = _items.IndexOf(temp);
                _items[index] = element;
            }
        }

        public IEnumerable<MenuItem> ReadAll()
        {
            return _items;
        }


    }
}
