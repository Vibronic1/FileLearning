using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
//Выполнил Брестер Андрей Николаевич БББО-07-19
namespace FileLearning
{
    [Serializable]
    internal class Stroka
    {
        public string Strochka
        {
            get;
            set;
        }

        public Stroka(string strochka) => this.Strochka = strochka;
    }

    internal class Human
    {
        public string age;
        public string hight;
        public string weight;
        public string name;

        public Human(string _name, string _age, string _hight, string _weight)
        {
            this.name = _name;
            this.age = _age;
            this.weight = _weight;
            this.hight = _hight;
        }

        public Human() { }
    }

    internal class Program
    {
        static BinaryFormatter binaryFormatter = new BinaryFormatter();
        static void CreateFileTxt(String Adres, String name)
        {
            if (File.Exists(Adres + "\\" + name + ".txt"))
            {
                Console.WriteLine("Файл существует.\n");
                return;
            }
            FileStream fileStream1 = new FileStream(Adres + "\\" + name + ".txt", FileMode.OpenOrCreate);
            Console.WriteLine("Введите содержимое файла.");
            byte[] bytes1 = Encoding.Default.GetBytes(Console.ReadLine());
            fileStream1.Write(bytes1, 0, bytes1.Length);
            Console.WriteLine("Текст записан в файл");
            fileStream1.Close();
            return;
        }
        static void CreateFileJson(String Adres, String name)
        {
            if (File.Exists(Adres + "\\" + name + ".JSON"))
            {
                Console.WriteLine("Файл существует.\n");
                return;
            }
            FileStream fileStream2 = new FileStream(Adres + "\\" + name + ".JSON", FileMode.OpenOrCreate);
            Console.WriteLine("Введите содержимое файла.");
            Stroka stroka = new Stroka(Console.ReadLine());
            binaryFormatter.Serialize((Stream)fileStream2, (object)stroka);
            Console.WriteLine("Текст записан в файл");
            fileStream2.Close();
            return;
        }
        static void CreateFileXml(String Adres, String name)
        {
            if (File.Exists(Adres + "\\" + name + ".xml"))
            {
                Console.WriteLine("Файл существует.\n");
                return;
            }
            FileStream fileStream3 = new FileStream(Adres + "\\" + name + ".xml", FileMode.OpenOrCreate);
            byte[] bytes2 = Encoding.Default.GetBytes("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<Humans>\n</Humans>");
            fileStream3.Write(bytes2, 0, bytes2.Length);
            fileStream3.Close();
            bool flag = true;
            while (flag)
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(Adres + "\\" + name + ".xml");
                XmlElement documentElement = xmlDocument.DocumentElement;
                Console.WriteLine("Введите имя");
                string text1 = Console.ReadLine();
                XmlElement element1 = xmlDocument.CreateElement("Human");
                XmlAttribute attribute = xmlDocument.CreateAttribute("Name");
                XmlElement element2 = xmlDocument.CreateElement("Hight");
                XmlElement element3 = xmlDocument.CreateElement("Wight");
                XmlElement element4 = xmlDocument.CreateElement("Age");
                XmlText textNode1 = xmlDocument.CreateTextNode(text1);
                Console.WriteLine("Введите возраст");
                string text2 = Console.ReadLine();
                XmlText textNode2 = xmlDocument.CreateTextNode(text2);
                Console.WriteLine("Введите рост");
                string text3 = Console.ReadLine();
                XmlText textNode3 = xmlDocument.CreateTextNode(text3);
                Console.WriteLine("Введите вес");
                string text4 = Console.ReadLine();
                XmlText textNode4 = xmlDocument.CreateTextNode(text4);
                attribute.AppendChild((XmlNode)textNode1);
                element2.AppendChild((XmlNode)textNode4);
                element3.AppendChild((XmlNode)textNode3);
                element4.AppendChild((XmlNode)textNode2);
                element1.Attributes.Append(attribute);
                element1.AppendChild((XmlNode)element4);
                element1.AppendChild((XmlNode)element3);
                element1.AppendChild((XmlNode)element2);
                documentElement.AppendChild((XmlNode)element1);
                xmlDocument.Save(Adres + "\\" + name + ".xml");
                Console.WriteLine("Введите 1 если хотите ввести ещё");
                if (!(Console.ReadLine() == "1"))
                    flag = false;
            }
            
            return;
        }
        static void archiving(String Path, String Adres, String Name)
        {
            int index1 = Path.Length - 1;
            Name += "_";
            for (; Path[index1] != '.'; --index1)
            {
                Name += Path[index1];
            }
            Name = Adres + "\\" + Name + ".gz";
            FileStream fileStream4 = new FileStream(Path, FileMode.OpenOrCreate);
            FileStream fileStream5 = File.Create(Name);
            GZipStream gzipStream = new GZipStream((Stream)fileStream5, CompressionMode.Compress);
            fileStream4.CopyTo((Stream)gzipStream);
            Console.WriteLine("Сжатие файла {0} завершено. Исходный размер: {1}  сжатый размер: {2}.", (object)(Path), (object)fileStream4.Length.ToString(), (object)fileStream5.Length.ToString());
            fileStream4.Close();
            fileStream5.Close();
            return;
        }

        static void Reading(String Path)
        {
            int index2 = Path.Length - 1;
            string str14 = "";
            if (File.Exists(Path))
            {
                FileStream fileStream4 = File.OpenRead(Path);
                for (; Path[index2] != '.'; --index2)
                {

                    str14 += Path[index2];
                }
                string str15 = str14;
                if (!(str15 == "txt"))
                {
                    if (!(str15 == "NOSJ"))
                    {
                        if (str15 == "lmx")
                        {
                            
                            List<Human> humanList = new List<Human>();
                            XmlDocument xmlDocument = new XmlDocument();
                            xmlDocument.Load(Path);
                            foreach (XmlElement xmlElement in (XmlNode)xmlDocument.DocumentElement)
                            {
                                Human human = new Human();
                                XmlNode namedItem = xmlElement.Attributes.GetNamedItem("Name");
                                if (namedItem != null)
                                    human.name = namedItem.Value;
                                foreach (XmlNode childNode in xmlElement.ChildNodes)
                                {
                                    if (childNode.Name == "Hight")
                                        human.hight = childNode.InnerText;
                                    if (childNode.Name == "Wight")
                                        human.weight = childNode.InnerText;
                                    if (childNode.Name == "Age")
                                        human.age = childNode.InnerText;
                                }
                                humanList.Add(human);
                            }
                            Console.Write("имя       |вес       |рост      |возраст   |");
                            foreach (Human human in humanList)
                            {
                                Console.Write("\n" + human.name);
                                for (int index3 = 10 - human.name.Length; index3 > 0; --index3)
                                    Console.Write(" ");
                                Console.Write("|" + human.weight);
                                for (int index3 = 10 - human.weight.Length; index3 > 0; --index3)
                                    Console.Write(" ");
                                Console.Write("|" + human.hight);
                                for (int index3 = 10 - human.hight.Length; index3 > 0; --index3)
                                    Console.Write(" ");
                                Console.Write("|" + human.age);
                                for (int index3 = 10 - human.age.Length; index3 > 0; --index3)
                                    Console.Write(" ");
                                Console.Write("|");
                            }
                            Console.Read();
                            return;
                        }
                        return;
                    }
                    Console.WriteLine("Текст из файла: " + ((Stroka)binaryFormatter.Deserialize((Stream)fileStream4)).Strochka);
                    fileStream4.Close();
                    return;
                }
                byte[] numArray = new byte[fileStream4.Length];
                fileStream4.Read(numArray, 0, numArray.Length);
                Console.WriteLine("Текст из файла: " + Encoding.Default.GetString(numArray));
                fileStream4.Close();
                return;
            }
            Console.WriteLine("Файл не найден.");

        }
        static void Unarchiving(String Path, String Adres)
        {
            if (File.Exists(Path))
            {
                using (FileStream fileStream4 = new FileStream(Path, FileMode.OpenOrCreate))
                {
                    int index3 = Path.Length - 1;
                    while (Path[index3] != '.')
                        --index3;
                    Adres += "\\file.";
                    while (Path[index3] != '_')
                    {
                        --index3;
                        if (Path[index3] != '_')
                        {
                            Adres += Path[index3];
                        }
                    }
                    using (FileStream fileStream5 = File.Create(Adres))
                    {
                        using (GZipStream gzipStream = new GZipStream((Stream)fileStream4, CompressionMode.Decompress))
                        {
                            gzipStream.CopyTo((Stream)fileStream5);
                            Console.WriteLine("Восстановлен файл: {0}", (object)(Adres));
                            return;
                        }
                    }
                    fileStream4.Close();
                }
            }
            else
            {
                Console.WriteLine("Файл не найден.");
                return;
            }
        }
        private static void Main(string[] args)
        {
            //глобальные переменные
            String Name, Adres, Path;

            //

            while (true)
            {
                Console.WriteLine("Введите номер задания:\n1-размер дисков\n2-создание файла\n3-создание файла Json\n4-создание файла Xml\n5-архивирование файла \n6-чтение файла\n7-удаление чего-то\n8-деархивирование\n");

                switch (Console.ReadLine())
                {
                    case "1": // Определение Размера дисков
                        foreach (DriveInfo drive in DriveInfo.GetDrives())
                        {
                            Console.WriteLine("Название: " + drive.Name);
                            Console.WriteLine(string.Format("Тип: {0}", (object)drive.DriveType));
                            if (drive.IsReady)
                            {
                                Console.WriteLine(string.Format("Объем диска: {0}", (object)drive.TotalSize));
                                Console.WriteLine(string.Format("Свободное пространство: {0}", (object)drive.TotalFreeSpace));
                                Console.WriteLine("Метка: " + drive.VolumeLabel + "\n");
                            }
                        }
                        break;
                    case "2": //Создание файла .TxT
                        Console.WriteLine("Введите адрес, где вы хотите создать файл");
                        Adres = Console.ReadLine();
                        Console.WriteLine("Введите название файла, который хотите создать");
                        Name = Console.ReadLine();
                        CreateFileTxt(Adres, Name);
                        break;

                    case "3":
                        Console.WriteLine("Введите адрес, где вы хотите создать файл");
                        Adres = Console.ReadLine();
                        Console.WriteLine("Введите название файла, который хотите создать");
                        Name = Console.ReadLine();
                        CreateFileJson(Adres, Name);
                        break;
                    case "4":
                        Console.WriteLine("Введите адрес, где вы хотите создать файл");
                        Adres = Console.ReadLine();
                        Console.WriteLine("Введите название файла, который хотите создать");
                        Name = Console.ReadLine();
                        CreateFileXml(Adres, Name);
                        break;
                    case "5":
                        Console.WriteLine("Введите путь до файла или папки, который хотите заархивировать");
                        Path = Console.ReadLine();
                        Console.WriteLine("Введите путь до папки,в который хотите поместить архив");
                        Adres = Console.ReadLine();
                        Console.WriteLine("Введите название архива");
                        Name = Console.ReadLine();
                        archiving(Path, Adres, Name);
                        break;
                    case "6":
                        Console.WriteLine("Введите адрес, вместе с файлом");
                        Path = Console.ReadLine();
                        Reading(Path);
                        break;
                    case "7":
                        Console.WriteLine("Введите адрес файла, вместе с файлом");
                        string fileName = Console.ReadLine();
                        if (File.Exists(fileName ?? ""))
                        {
                            new FileInfo(fileName).Delete();
                            break;
                        }
                        Console.WriteLine("Файл не найден.");
                        break;
                    case "8":
                        Console.WriteLine("Введите адрес, вместе с архивом");
                        Path = Console.ReadLine();
                        Console.WriteLine("Введите адрес, куда поместить разархивированый файл");
                        Adres = Console.ReadLine();
                        Unarchiving(Path, Adres);
                        break;
                    default:
                        Console.WriteLine("Простите, вы ввели что-то не то, повторите попытку.");
                        break;
                }
            }
        }
    }
}