﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultilanguageChat.Common
{
    public interface INavigable
    {
        void Activate(object parameter);

        void Deactivate();
    }
}
