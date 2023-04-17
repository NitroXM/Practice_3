using System;
using System.Data;
using System.Globalization;
using System.Runtime.Serialization.Formatters;
using Newtonsoft.Json;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Xml.Serialization;

namespace Practice_3
{
     class Program
    {

        static void Main(string[] args)
        {
            string taskschoise;
            bool v = true;
            while (v)
            {
                v = tasks();
            }
            bool tasks()
            {
                Console.WriteLine("Choose which task you want to execute: 1, 2, 3, 4");
                string choise = Console.ReadLine();
                switch (choise)
                {
                    case "1":
                        Task1();
                        break;
                    case "2":
                        Task2();
                        break;
                    case "3":
                        Task3();
                        break;
                    case "4":
                        Task4();
                        break;
                }
                Console.WriteLine("Do you want to choose tasks again? Y/N");
                taskschoise = Console.ReadLine();
                if (taskschoise == "Y")
                {
                    Console.Clear();
                    return true;
                }
                else
                    return false;
            }

            void Task1()
            {
                string file = "test1.csv";                           // The list of transactions is created and sorted by date of trnasaction 
                List<transactions> list = File.ReadAllLines(file)
                    .Skip(1)
                    .Select(transactions.ParseFile)
                    .OrderBy(transaction => transaction.Time)
                    .ToList();

                double total = 0;
                int dayscounter = 0;
                List<double> eachdaysum = new List<double>();     // Here, using complex logic, I create a list containing sum of all transactions for each day 


                for (int i = 0; i < list.Count - 1; i++)
                {
                    if (list[i].Time.Date == list[i + 1].Time.Date)
                    {
                        if (i == list.Count - 2)
                        {
                            total = total + list[i].Sum + list[i + 1].Sum;
                            eachdaysum.Add(total);
                        }
                        else
                            total = total + list[i].Sum;
                    }
                    else
                    {
                        if (i == list.Count - 2)
                        {
                            eachdaysum.Add(total + list[i].Sum);
                            eachdaysum.Add(list[i + 1].Sum);
                        }
                        else
                        {
                            eachdaysum.Add(total + list[i].Sum);
                            total = 0;
                            dayscounter++;
                        }
                    }
                }


                List<DateTime> alldateslist = new List<DateTime>();    // In order to properly display infornation on the screen, I need to create a list containing all distinct dates of transactions 

                for (int i = 0; i < list.Count; i++)
                {
                    alldateslist.Add(list[i].Time.Date);
                }
                List<DateTime> cleardateslist = alldateslist.Distinct().ToList();

                for (int i = 0; i < eachdaysum.Count; i++)
                {
                    Console.WriteLine($"Amount of money spent on {cleardateslist[i].ToString("d", CultureInfo.GetCultureInfo("de-DE"))}: {eachdaysum[i]}");  // Here, I diplay the amount of money spent for each day
                }
            }

            void Task2()
            {

                string path = "productlist.json";                                                           //here I chose which file to use, then I Deserialize it to a list of Products
                var products = JsonConvert.DeserializeObject<List<Product>>(File.ReadAllText(path));
                double lower = 0;
                double higher = 0;                                                                          //I also create variables that will be needed in future
                string categories = "cats";
                string colon1;

                Console.WriteLine("Here is a list of all available products: \r\n");                        //here I display all the available products
                for(int i = 0; i < products.Count - 1; i += 2) 
                {
                    colon1 = $"{products[i].Name} - {products[i].Price} - {products[i].Category}";
                    Console.Write(colon1);
                    string spaced = new string(' ', 50 - colon1.Length);

                    Console.Write(spaced);
                    Console.Write($"{products[i + 1].Name} - {products[i + 1].Price} - {products[i + 1].Category}");
                    Console.WriteLine();
                }
                Console.WriteLine("\r\nPlease choose which criteria to filter the products  by" +               //letting the user choose what kind of filters they want to apply 
            "\r\n1 Price \r\n2 Category \r\n3 Price & Caterory \r\n");

                int choise = int.Parse(Console.ReadLine());
                Console.Clear();
                if (choise == 1)
                {
                    Console.WriteLine("Please select lower price threshold \r\n");
                    lower = double.Parse(Console.ReadLine());
                    Console.Clear();
                    Console.WriteLine("Please select higher price threshold \r\n");
                    higher = double.Parse(Console.ReadLine());
                    Console.Clear();
                }
                else if (choise == 2)
                {
                    Console.WriteLine("Please specify the categories you would like to see (use comma to for separation but don't use any spaces)" +
                                "available categories: Fruit, Vegetable, Meat, Soda, Fish \r\n");
                    categories = Console.ReadLine();
                    Console.Clear();
                }
                else if (choise == 3)
                {
                    Console.WriteLine("Please select lower price threshold \r\n");
                    lower = double.Parse(Console.ReadLine());
                    Console.Clear();
                    Console.WriteLine("Please select higher price threshold \r\n");
                    higher = double.Parse(Console.ReadLine());
                    Console.Clear();

                    Console.WriteLine("Please specify the categories you would like to see (use comma to for separation but don't use any spaces)" +
                                "available categories: Fruit, Vegetable, Meat, Soda, Fish \r\n");
                    categories = Console.ReadLine();
                    Console.Clear();
                }




                List<Predicate<Product>> criteriachoise(int choise, double higher, double lower, string cats)                  //here i create a list of criteria by which the products will be filtered (it depends on user's choise) 
                {
                    if (choise == 1)
                    {
                        var criteria1 = new List<Predicate<Product>> { p => p.Price < higher && p.Price > lower };
                        return criteria1;
                    }
                    else if (choise == 2)
                    {
                        List<string> listofcats = new List<string>(cats.Split(','));
                        var criteria2 = new List<Predicate<Product>> { p => listofcats.Contains(p.Category) };
                        return criteria2;
                    }
                    else if (choise == 3)
                    {

                        List<string> listofcats = new List<string>(cats.Split(','));
                        var criteria3 = new List<Predicate<Product>> { p => (listofcats.Contains(p.Category)) && (p.Price < higher && p.Price > lower) };
                        return criteria3;
                    }
                    else
                    {
                        var criteria666 = new List<Predicate<Product>> { null };
                        return criteria666;
                    }

                }


                if (choise == 1 || choise == 2 || choise == 3)
                {
                    var filteredProducts = products.Where(p => criteriachoise(choise, higher, lower, categories).All(c => c(p)));     //here a list of filtered products is created 
                    Console.WriteLine("These products match your critiria:\r\n");

                    foreach (var product in filteredProducts)
                    {
                        Console.WriteLine($"{product.Name} - {product.Price} - {product.Category}");
                    }
                }
                else Console.WriteLine("Next time enter a correct number \r\n");

            }

            void Task3()
            {
                string filepath = @"img";   //here you can select which folder you would like to use 
                Console.Clear();
                string newfilepath = @"editedimg"; 
                Console.Clear();
                List<Bitmap> images = LoadImages(filepath);        //adding images from the folder to the list of Bitmaps 
                Console.WriteLine("Please choose the desired operation from the list below\r\n" +
                    "1 270 degree clockwise rotation followed by horizontal and vertical mirroring\r\n" +
                    "2 180 degree clockwise rotation without mirroring\r\n" +
                    "3 270 degree clockwise rotation without mirroring\r\n" +
                    "4 180 degree clockwise rotation, followed by a mirror image vertically\r\n" +
                    "5 90 degree clockwise rotation, followed by mirroring horizontally\r\n" +
                    "6 180 degree clockwise rotation, followed by mirroring horizontally\r\n" +
                    "7 90 degree clockwise rotation, followed by a mirror image vertically\r\n");

                int choise3 = int.Parse(Console.ReadLine());
                Console.Clear();


                Action<Bitmap, RotateFlipType> imageOperations = (Bitmap x, RotateFlipType y) => x.RotateFlip(y);    //here I make a delegate that allows to do certain operations with the desired images




                List<string> names = new List<string>();   //here I craete a list of names for edited images 
                for (int i = 1; i < 100; i++)
                {
                    names.Add($"img{i}");
                }

                Process p = new Process();   //A process to launch edited images  

                for (int i = 0; i < images.Count; i++)   //edited desired images using my delegate
                {
                    imageOperations(images[i], (RotateFlipType)choise3);

                    images[i].Save(@$"{newfilepath}\{names[i]}.jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);

                    p.StartInfo = new ProcessStartInfo($@"{newfilepath}\{names[i]}.jpeg")
                    {
                        UseShellExecute = true
                    };
                    p.Start();
                }
            }

            void Task4()
            {

                
                string folderPath = @"files_for_task4";    //you can select path to  your own folder (only .txt files supported)

                
                Func<string, IEnumerable<string>> tokenizer = text =>   //set the tokenizer
                {
                    return text.Split(new[] { ' ', '.', ',', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);  //divide the text into words
                };

                Func<IEnumerable<string>, IDictionary<string, int>> wordCounter = words =>   //set the function for calculating the frequency of words
                {
                    var result = new Dictionary<string, int>();

                    foreach (var word in words)
                    {
                        if (result.ContainsKey(word))
                        {
                            result[word]++;
                        }
                        else
                        {
                            result[word] = 1;
                        }
                    }

                    return result;
                };

                Action<IDictionary<string, int>> printWordCount = wordCount =>   //set the method to display statistics
                {
                    foreach (var item in wordCount.OrderByDescending(i => i.Value))
                    {
                        Console.WriteLine("{0}: {1}", item.Key, item.Value);
                    }
                };

                foreach (var filePath in Directory.GetFiles(folderPath))  //read files and count word frequency
                {
                    var text = File.ReadAllText(filePath);
                    var words = tokenizer(text);
                    var wordCount = wordCounter(words);

                    Console.WriteLine("=== {0} ===", Path.GetFileName(filePath));   //displaying statistics 
                    printWordCount(wordCount);
                    Console.WriteLine();
                }

            }
        }
        static List<Bitmap> LoadImages(string directoryPath)
        {
            List<Bitmap> images = new List<Bitmap>();

            foreach (string filePath in Directory.GetFiles(directoryPath))
            {
                images.Add(new Bitmap(filePath));
            }

            return images;
        }
    }
    
    class Product
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string Category { get; set; }
    }
}