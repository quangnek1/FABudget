using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemMaster.Shared.Extensions
{
    public class ImageGenerate
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">Tên hiển thị trong chữ ký. vd: Quang(6156)</param>
        /// <param name="date">Ngày tháng năm</param>
        /// <param name="division">Tên phòng</param>
        /// <returns></returns>
        public static string GenerateImage(string name, string date, string division)
        {
            try
            {
                string img = "";
                // Hardcoding values for testing purposes.
                var widthName = GetWidthOfString(name, 10);
                var widthDate = GetWidthOfString(date, 12);
                var widthDivision = GetWidthOfString(division, 12);


                PointF firstLocation = new PointF(135 / 2 - widthDate / 2, 56);
                PointF secondLocation = new PointF(135 / 2 - widthName / 2, 98);
                PointF thirdLocation = new PointF(135 / 2 - widthDivision / 2, 17);

                //  string fileNameURL = GetFileUrl("Condau_OK");
                using (Image imgBackground = Image.FromFile("Condau_OK.png"))
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        using (Graphics graphics = Graphics.FromImage(imgBackground))
                        {
                            using (Font arialFont = new Font("mspgothic", 10))
                            {
                                graphics.DrawString(date, new Font("mspgothic", 12, FontStyle.Bold), Brushes.Red, firstLocation);
                                graphics.DrawString(name, arialFont, Brushes.Red, secondLocation);
                                graphics.DrawString(division, new Font("mspgothic", 12, FontStyle.Bold), Brushes.Red, thirdLocation);
                            }
                        }
                        string fileName = DateTime.Now.ToString("ddMMyyyy_HHmmss") + "User.png";
                        //    imgBackground.Save(fileName, ImageFormat.Png);
                        imgBackground.Save(m, imgBackground.RawFormat);
                        //image.Save(m, image.RawFormat);
                        byte[] imageBytes = m.ToArray();

                        // Convert byte[] to Base64 String
                        string base64String = Convert.ToBase64String(imageBytes);
                        return base64String;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        private static float GetWidthOfString(string str, int size)
        {
            Bitmap objBitmap = default(Bitmap);
            Graphics objGraphics = default(Graphics);

            objBitmap = new Bitmap(500, 200);
            objGraphics = Graphics.FromImage(objBitmap);

            SizeF stringSize = objGraphics.MeasureString(str, new Font("mspgothic", size));

            objBitmap.Dispose();
            objGraphics.Dispose();
            return stringSize.Width;
        }
    }
}
