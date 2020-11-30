using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Recht.Service
{
    public class TreeUpdate : ITreeUpdate
    {
        public event Action UpdateTree;
        public void Update()
        {
            UpdateTree.Invoke();
        }
    }
}
