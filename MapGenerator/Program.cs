using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapGenerator
{
    public static class Program
    {
        private static Map m;
        private static MapHandler mh;
        private static Form1 form;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            m = new CaveMap(15, 15, 4819, 45);
            mh = new MapHandler(m);
            mh.Generate();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            form = new Form1();

            form.SetImage(mh.GetBitmap());
            form.setText(mh.getDescription());

            form.Refresh();

            Application.Run(form);


        }

        public static void Changesize(int width, int height){
            mh.Resize(width, height);
            mh.Generate();
            form.SetImage(mh.GetBitmap());
            form.setText(mh.getDescription());
            form.Refresh();
        }

        public static void GenerateNew(){
            mh.setSeed(Util.GetRandom(1,10000));
            mh.Generate();
            form.SetImage(mh.GetBitmap());
            form.setText(mh.getDescription());
            form.Refresh();
        }

        public static void DeColor(){
            mh.DeColor();
            form.SetImage(mh.GetBitmap());
            form.setText(mh.getDescription());
            form.Refresh();
        }
    }
}
