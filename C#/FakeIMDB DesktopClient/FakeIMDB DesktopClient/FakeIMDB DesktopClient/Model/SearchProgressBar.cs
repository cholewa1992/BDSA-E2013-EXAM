﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FakeIMDB_DesktopClient.Model
{
    public class SearchProgressBar
    {
        public bool isVisible { get; set; }
        public double value { get; set; }
        public Visibility Visibility { get; set; }
    }
}
