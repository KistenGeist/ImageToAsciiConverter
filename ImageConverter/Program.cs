using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;

namespace ImageConverter
{
    internal class Program
    {
        static void Main()
        {
            // create needed variables and objects
            string? filePath = ""; 
            bool imageExists = false;
            ImageHandler imgHandler = new();

            do
            {
                // get filepath from user
                Console.Write("Enter filepath of image (.png or .jpg): ");

                filePath = Console.ReadLine();

                // if entered string is not empty
                if (String.IsNullOrEmpty(filePath))
                {
                    Console.Clear();
                    Console.WriteLine("Input was empty.");
                    Console.WriteLine();
                    continue;
                }

                // get Bitmap of Image
                Bitmap? bm = imgHandler.LoadImage(filePath);

                // if bitmap is not empty
                if (bm == null)
                {
                    Console.Clear();
                    Console.WriteLine("Filepath does not exists or file is not .png or .jpg");
                    Console.WriteLine();
                    continue;
                }

                // set variable to break loop
                imageExists = true;


                Console.Clear();
                Console.WriteLine("---------------------------------------------");
                Console.WriteLine("Start converting to ASCII");
                Console.WriteLine("---------------------------------------------");

                // get ascii image --> async
                string result = imgHandler.ConvertImageToASCII(bm);

                // create result file
                CreateFile(filePath, result);

            } while (!imageExists);
        }

        private static void CreateFile(string path, string AsciiImage)
        {
            // Get Directory
            string? resultPath = Path.GetDirectoryName(path);
            
            // get File name with out extension
            string fileName = Path.GetFileNameWithoutExtension(path);
            
            // create path and filename for result file
            resultPath += "\\" + fileName + "_Result.txt";

            // create result file
            using (FileStream fs = File.Create(resultPath))
            {
                // get bytes to write file
                Byte[] text = new UTF8Encoding(true).GetBytes(AsciiImage);
                // write
                fs.Write(text, 0, text.Length);
            }




            Console.WriteLine();
            Console.WriteLine("---------------------------------------------");
            Console.Write("Converting finished: "); 
            Console.Write(resultPath);
            Console.WriteLine();
            Console.WriteLine("---------------------------------------------");
            Console.ReadKey();


            Process.Start("notepad.exe", resultPath);
        }
    }
}
