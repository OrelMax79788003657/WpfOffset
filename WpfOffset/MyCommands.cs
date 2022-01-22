using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;


namespace WpfOffset
{
    internal class MyCommands
    {
        public static RoutedCommand Exit { get; set; }

        static MyCommands()
        {
            Exit = new RoutedCommand();
        }

    }
}
