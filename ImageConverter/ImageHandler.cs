using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageConverter
{
    internal class ImageHandler
    {
        // characters which are used for different lgiht levels in image
        private readonly string[] charSet = { "  ", " 0", " 1", " 2", " 3", " 4", " 5", " 6", " 7", " 8", " 9" };

        /// <summary>
        /// Gets Image from filepath and returns Bitmap
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public Bitmap? LoadImage(string filePath)
        {
            Bitmap? bm = null;

            if (File.Exists(filePath))
            {
                // get las dot in string --> dot for extension
                int lastDotIndex = filePath.LastIndexOf('.') + 1;
                // get extension
                string extension = filePath.Substring(lastDotIndex, filePath.Length - lastDotIndex);

                // if file is image return bitmap
                if (extension == "png" || extension == "jpg")
                {
                    bm = new Bitmap(filePath);
                }
            }

            return bm;
        }

        /// <summary>
        /// Converts Bitmap to string. 
        /// </summary>
        /// <param name="bm"></param>
        /// <returns></returns>
        public string ConvertImageToASCII(Bitmap bm)
        {
            // get get ascii image async
            string[]? lstRowResults = ConvertImageToASCIIAsync(bm).Result;

            // connect all rows and return result
            return String.Join("\n", lstRowResults);
        }

        /// <summary>
        /// Async function to convert image to ascii text
        /// </summary>
        /// <param name="bm"></param>
        /// <returns></returns>
        public async Task<string[]> ConvertImageToASCIIAsync(Bitmap bm)
        {
            // create list of Task (each entry is one row)
            List<Task<string>> lstTasksRows = new();

            // for each row in image create on async task
            for (int y = 0; y < bm.Height; y++)
            {
                // create async task to get row in ascii
                lstTasksRows.Add(ConvertImageRowToASCIIAsync(y,bm));
            }

            // return when all are finished
            return await Task.WhenAll(lstTasksRows);
        }

        /// <summary>
        /// Async funtion to convert one Bitmap-Row in Ascii Text
        /// </summary>
        /// <param name="y"></param>
        /// <param name="bm"></param>
        /// <returns></returns>
        public async Task<string> ConvertImageRowToASCIIAsync(int y, Bitmap bm)
        {
            // create result
            string result = "";

            // go through each pixel in row
            for (int x = 0; x < bm.Width; x++)
            {
                // get color of pixel
                Color color = bm.GetPixel(x, y);

                // get brightness and round it
                double pixelBrightness = Math.Round(color.GetBrightness(), 1);

                // map brighness to character
                string mappedChar = charSet[Convert.ToInt32(pixelBrightness * 10)];

                // add to result
                result += mappedChar;
            }

            // return row
            return await Task.FromResult(result);
        }
    }
}
