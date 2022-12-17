using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace camouflage
{
    static class codec
    {
        public static int[] encode(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return null;
            int[] output = new int[str.Length];

            int index = 0;

            foreach (char c in str)
            {
                int i = c;
                if (i > 127)
                    throw new ArgumentException();
                output[index++] = i;
            }
            return output;
        }
      
        public static string decode(int[] input)
        {
            string f = "";
            for (int i = 0; i < input.Length; i++)
            {
                f += (char)input[i];
            }
            return f;
        }

        public static int[] encrypt(int[] message, string seed, int tapPosition)
        {
            int[] outMsg = new int[message.Length];
            LSFR lsfr = new LSFR(seed, tapPosition);

            int index = 0;
            for(int i = 0; i < message.Length; ++i)
            {
                int inpb = message[i];
                int randb = lsfr.GetNext7Bits();

                outMsg[index++] = inpb ^ randb;
            }
            return outMsg;
        }

        public static string decrypt(string message, string seed, int tapPosition)
        {
            LSFR lsfr = new LSFR(seed, tapPosition);
            string OrigMsg = "";

            int start = 0;
            while (start < message.Length)
            {
                string ss = message.Substring(start, 7);
                start += 7;
                int iss = Convert.ToInt32(ss, 2);
                int randb = lsfr.GetNext7Bits();
                char c = (char)Convert.ToInt32(iss ^ randb);
                OrigMsg += c;
            }
            return OrigMsg;
        }

        public static Bitmap HideMessage(string msg,string inputImage,string Register,int TapPos,string outputImage,out bool _OutRes)
        {
            _OutRes = false;
            try
            {
                int[] AsciiCodes = codec.encode(msg);
                int[] encCodes = codec.encrypt(AsciiCodes, Register, TapPos); // Binary numbers

                Bitmap img = new Bitmap(inputImage);
               Bitmap img2 = new Bitmap(img, img.Width, img.Height);

                int Index = 0;

                ImgHelper imgHelper = new ImgHelper(img);
                ImgHelper imgHelper2 = new ImgHelper(img2);

                foreach (int encCode in encCodes)
                {
                    string binCodes = Convert.ToString(encCode, 2);
                    binCodes = binCodes.PadLeft(7, '0');

                    foreach (char binCode in binCodes)
                    {
                        Color pixel = imgHelper.GetNext();
                        StringBuilder binCodePix = new StringBuilder(Convert.ToString(pixel.B, 2));
                        if (binCode == '0')
                        {
                            binCodePix[binCodePix.Length - 1] = '0';
                        }
                        else
                        {
                            binCodePix[binCodePix.Length - 1] = '1';
                        }
                        int newPix = Convert.ToInt32(binCodePix.ToString(), 2);
                        imgHelper2.PutPixel(Color.FromArgb(pixel.A, pixel.R, pixel.G, newPix));
                    }
                    ++Index;
                }
                img2.Save(outputImage);
                _OutRes = true;
                return img2;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public static string RetrieveMessage(int MsgSize, string Register, int TapPos, string ImgPath)
        {
            try
            {
                Bitmap img = new Bitmap(ImgPath);

                string[] outP = new string[MsgSize * 7];
                string sOutP = "";
                int Index = 0;
                for (int i = 0; i < img.Width; i++)   // x
                {
                    if (Index >= outP.Length)
                        break;
                    for (int j = 0; j < img.Height; j++) //y
                    {
                        if (Index >= outP.Length)
                            break;
                        Color pixel = img.GetPixel(i, j);
                        int b = pixel.B;
                        b = b & 1;

                        if (b == 1)
                            outP[Index] = "1";
                        else
                            outP[Index] = "0";
                        sOutP += b;
                        ++Index;
                    }
                }
                string OrigMsg = codec.decrypt(sOutP, Register, TapPos);
                return OrigMsg;
            }
            catch(Exception)
            {
                return "";
            }
        }
    }
}
