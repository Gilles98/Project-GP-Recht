﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Recht.Service
{
    public interface ITreeUpdate
    {
        //treeview updaten
         event Action UpdateTree;
         void Update(); 
    }
}
