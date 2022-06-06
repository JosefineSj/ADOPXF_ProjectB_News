using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using News.Models;
using News.Services;

using System.Web;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace News.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ArticlePage : ContentPage
    {
        NewsService service;
        NewsGroup newsgroup;

        public ArticlePage()
        {
            InitializeComponent();

            service = new NewsService();
            newsgroup = new NewsGroup();
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            headline.Text = $"Todays {Title} Headlines";

            MainThread.BeginInvokeOnMainThread(async () => { await LoadNews(); });

        }

        private async Task LoadNews()
        {

            try
            {
                NewsCategory category = (NewsCategory)Enum.Parse(typeof(NewsCategory), Title, true);

                if (category == NewsCategory.sports)
                {
                    await Task.Delay(7000);
                    throw new Exception();
                }
                var t1 = await service.GetNewsAsync(category);
                var items = t1.Articles;
                NewsList.ItemsSource = items;
                activityIndicator.IsRunning = false;

            }
            catch (Exception)
            {
                await DisplayAlert("Ooops", "Something appears to have gone wrong. Please check your connection and try again", "Yes Sir");

            }

        }


        private async void Refresh_Button(object sender, EventArgs e)
        {
            if (refreshButton.IsVisible == true)
            {
                await progressBar.ProgressTo(1, 2000, Easing.Linear);
                await LoadNews();
                progressBar.IsVisible = true;
            }
            await progressBar.ProgressTo(0, 2000, Easing.Linear);

        }

        private async void NewsList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var newsItem = (NewsItem)e.Item;
                var av = new ArticleView(newsItem.Url);
                await Navigation.PushAsync(av);

            }
            catch (Exception)
            {
                await DisplayAlert("Oh no!", "The newspage is not availabe atm, try again later.", "Okay then" );

            }
           
        }
    }
}