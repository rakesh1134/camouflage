using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace camouflage
{
    class ImgHelper
    {
        Bitmap _img;
        int _w;
        int _h;
        System.Collections.Generic.List<Color> _pixels = new System.Collections.Generic.List<Color>();
        int _Curr = 0;
        public ImgHelper(Bitmap img)
        {
            _img = img;
            _w = _img.Width;
            _h = _img.Height;

            InitPixelsArray();
        }

        void InitPixelsArray()
        {
            for (int i = 0; i < _img.Width; i++)   // x
            {
                for (int j = 0; j < _img.Height; j++) //y
                {
                    _pixels.Add(_img.GetPixel(i, j));
                }
            }
        }

        public Color GetNext()
        {
            return _pixels[_Curr++];
        }

        public void PutPixel(Color c)
        {
            int x = _Curr / _h;
            int y = _Curr % _h;
            _img.SetPixel(x,y, c);
            ++_Curr;
        }
    }
}
