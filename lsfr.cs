using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace camouflage
{
    class LSFR
    {
        private int[] _Register = null;
        private int _TapPos = 0;

        public LSFR(string seed, int tapposition)
        {
            int len = seed.Length;
            _Register = new int[len];
            if(0 <= tapposition && tapposition < len)
            {
                _TapPos = tapposition;
               for(int i = 0; i < len; ++i)
                {
                    if (seed[i] == '0')
                        _Register[i] = 0;
                    else if (seed[i] == '1')
                        _Register[i] = 1;
                    else
                    {
                        throw new ArgumentException();
                    }
                }
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public int GetNextBit()
        {
            int MostSignificantBit = _Register[0];
            int BitAtTapPos = _Register[_Register.Length - _TapPos - 1];

            int nextBit = MostSignificantBit ^ BitAtTapPos;

            for(int i = 0; i < _Register.Length-1; ++i)
            {
                _Register[i] = _Register[i + 1];
            }
            _Register[_Register.Length - 1] = nextBit;

            return nextBit;
        }

        public int GetNext7Bits()
        {
            string x = "";
            for(int i = 0; i < 7; ++i)
            {
                x += GetNextBit();
            }
            int ii = Convert.ToInt32(x, 2);
            return ii;
        }

        public string GetRegister()
        {
            string s = "";
            for (int i = 0; i < _Register.Length; ++i)
                s += _Register[i];
            return s;
        }

    }
}
