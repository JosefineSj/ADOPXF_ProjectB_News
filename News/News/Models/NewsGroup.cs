﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace News.Models
{
    public class Locker
    {
        public static object Lock = new object();

    }
    public enum NewsCategory
    {
        business, entertainment, general, health, science, sports, technology
    }
    public class NewsCacheKey
    {
        NewsCategory category;
        string timewindow;

        public string FileName => fname("Cache-" + Key + ".xml");
        public string Key => category.ToString() + timewindow;
        public bool CacheExist => File.Exists(FileName);

        public NewsCacheKey(NewsCategory category, DateTime dt)
        {
            this.category = category;
            timewindow = DateTime.Now.ToString("yyyy-MM-dd-HH-mm");
            //Cache expiration every minute
            //timewindow = DateTime.Now.ToString("yyyy-MM-dd-HH"); //Cache expiration every hour
        }

        public static void Serialize(NewsGroup news, string filename)
        {
            lock (Locker.Lock)
            {
                var xs = new XmlSerializer(typeof(NewsGroup));
                using (Stream s = File.Create(fname(filename)))
                    xs.Serialize(s, news);
            }
        }
        public static NewsGroup Deserialize(string filename)
        {
            lock (Locker.Lock)
            {
                NewsGroup news;
                var xs = new XmlSerializer(typeof(NewsGroup));

                using (Stream s = File.OpenRead(fname(filename)))
                    news = (NewsGroup)xs.Deserialize(s);

                return news;
            }
        }
        static string fname(string name)
        {
            var documentPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            documentPath = Path.Combine(documentPath, "AOOP2", "Project Part B");
            if (!Directory.Exists(documentPath)) Directory.CreateDirectory(documentPath);
            return Path.Combine(documentPath, name);
        }

      }

    [XmlRoot("News", Namespace = "http://mynamespace/test/")]
    public class NewsGroup
    {

        public NewsCategory Category { get; set; }
        public List<NewsItem> Articles { get; set; }



        public static void Serialize(NewsGroup news, string filename)
        {
            lock (Locker.Lock)
            { 
                var xs = new XmlSerializer(typeof(NewsGroup));
                using (Stream s = File.Create(fname(filename)))
                    xs.Serialize(s, news);
            }
        }
        public static NewsGroup Deserialize(string filename)
        {
            lock (Locker.Lock)
            {
                NewsGroup news;
                var xs = new XmlSerializer(typeof(NewsGroup));

                using (Stream s = File.OpenRead(fname(filename)))
                    news = (NewsGroup)xs.Deserialize(s);

                return news;
            }
        }
        static string fname(string name)
        {
            var documentPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            documentPath = Path.Combine(documentPath, "AOOP2", "Project Part B");
            if (!Directory.Exists(documentPath)) Directory.CreateDirectory(documentPath);
            return Path.Combine(documentPath, name);
        }
    }
}