﻿using News.Models;
using News.Services;
using System;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace News.Views
{
    public partial class ArticleView : ContentPage
    {
        //Here is where you show the news in Full page


        public ArticleView()
        {
            InitializeComponent();

        }
        
        public ArticleView(string Url)
        {
            InitializeComponent();
            BindingContext = new UrlWebViewSource
            {
                Url = HttpUtility.UrlDecode(Url)
            };
        }

    }
}



