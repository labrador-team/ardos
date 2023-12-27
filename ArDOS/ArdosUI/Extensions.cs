using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using ArDOS.Model;
using ArDOS.Runner;

namespace ArDOS.UI
{
    public static class Extensions
    {
        public static EventHandler RunItem(this ArdosItem item, string pluginPath, IScheduler scheduler)
        {
            return (sender, e) =>
            {
                foreach (var action in item.Actions) Task.Run(action.Run);
                if (item.Refresh) scheduler.Run(pluginPath);
            };
        }

        public static Icon ToIcon(this Image image)
        {
            using Bitmap bitmap = (Bitmap)image;
            return Icon.FromHandle(bitmap.GetHicon());
        }
    }
}
