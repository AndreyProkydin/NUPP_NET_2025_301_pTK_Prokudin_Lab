using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restouran.Common
{
    public interface ICrudService<T> where T : class
    {
        public void Create(T element);
        public T Read(Guid id);
        public IEnumerable<T> ReadAll();
        public void Update(T element);
        public void Remove(T element);
        public void Save(string FilePath);
        public void Load(string FilePath);
    }

}
