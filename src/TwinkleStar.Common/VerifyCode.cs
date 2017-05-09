﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace TwinkleStar.Common
{
    public class VerifyCode
    {
        private int m_codeW = 90;//image width
        private int m_codeH = 30;//image height
        private int m_fontSize = 16;//font size
        public VerifyCode()
        {
        }

        public VerifyCode(int codeW, int codeH, int fontSize)
        {
            this.m_codeW = codeW;
            this.m_codeH = codeH;
            this.m_fontSize = fontSize;
        }

        public byte[] GetVerifyCode(out string codeStr, bool encrypt = false)
        {
            int codeW = this.m_codeW;
            int codeH = this.m_codeH;
            int fontSize = this.m_fontSize;
            string chkCode = string.Empty;
            //颜色列表，用于验证码、噪线、噪点 
            Color[] color = { Color.Black, Color.Red, Color.Blue, Color.Green, Color.Orange, Color.Brown, Color.Brown, Color.DarkBlue };
            //字体列表，用于验证码 
            string[] font = { "Times New Roman" };
            //验证码的字符集，去掉了一些容易混淆的字符 
            char[] character = { '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'd', 'e', 'f', 'h', 'k', 'm', 'n', 'r', 'x', 'y', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'R', 'S', 'T', 'W', 'X', 'Y' };
            Random rnd = new Random();
            //生成验证码字符串 
            for (int i = 0; i < 4; i++)
            {
                chkCode += character[rnd.Next(character.Length)];
            }
            //写入Session、验证码加密
            //WebHelper.WriteSession("nfine_session_verifycode", Md5.md5(chkCode.ToLower(), 16));
            if (encrypt)
            {//加密
                codeStr = chkCode.ToLower();//todo 加密
            }
            else
            {
                codeStr = chkCode.ToLower();
            }
            
            //创建画布
            Bitmap bmp = new Bitmap(codeW, codeH);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            //画噪线 
            for (int i = 0; i < 3; i++)
            {
                int x1 = rnd.Next(codeW);
                int y1 = rnd.Next(codeH);
                int x2 = rnd.Next(codeW);
                int y2 = rnd.Next(codeH);
                Color clr = color[rnd.Next(color.Length)];
                g.DrawLine(new Pen(clr), x1, y1, x2, y2);
            }
            //画验证码字符串 
            for (int i = 0; i < chkCode.Length; i++)
            {
                string fnt = font[rnd.Next(font.Length)];
                Font ft = new Font(fnt, fontSize);
                Color clr = color[rnd.Next(color.Length)];

                Matrix mtxSave = g.Transform;
                Matrix mtxRotate = g.Transform;
                //旋转字符
                mtxRotate.RotateAt((float)rnd.Next(-25, 25), new PointF((float)i * (codeW / 4) - 5, (float)0));
                g.Transform = mtxRotate;
                g.DrawString(chkCode[i].ToString(), ft, new SolidBrush(clr), (float)i * (codeW / 4) - 5, (float)0);
                g.Transform = mtxSave;
            }
            //将验证码图片写入内存流，并将其以 "image/Png" 格式输出 
            MemoryStream ms = new MemoryStream();
            try
            {
                bmp.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                g.Dispose();
                bmp.Dispose();
            }
        }
    }
}
