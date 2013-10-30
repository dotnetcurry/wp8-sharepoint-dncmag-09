using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.SharePoint.Client;
using SharePointConnector.Resources;

namespace SharePointConnector
{
    public partial class MainPage : PhoneApplicationPage
    {
        //private ListCollection _lists;
        private List _list;
        private ListItemCollection _collection;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            Loaded += MainPageLoaded;
        }

        void MainPageLoaded(object sender, RoutedEventArgs e)
        {
            //Get the top level Web site
            var web = App.Context.Web;
            //Fetch the desired list by title inside the web selected above
            _list = web.Lists.GetByTitle("DemoList");
            //Create a query to load all items/fields in that list
            var query = CamlQuery.CreateAllItemsQuery();
            //Fetch the items from thet list using above query
            _collection = _list.GetItems(query);
            //Load the web
            App.Context.Load(web);
            //Load the collection
            App.Context.Load(_collection);
            //Execute the query NOW providing Success & Failure callbacks
            App.Context.ExecuteQueryAsync(SucceededCallback, FailedCallback);
        }
           
        private void FailedCallback(object sender, ClientRequestFailedEventArgs args)
        {
            MessageBox.Show("FAIL");
        }

        private void SucceededCallback(object sender, ClientRequestSucceededEventArgs args)
        {
            //Create temp list to hold items
            var list = new List<string>();
            //Iterate through all the items
            foreach (var item in _collection)
            {
                //Get title and add to temp list
                list.Add(item["Title"].ToString());
            }
            //On UI thread bind temp list to ListBox
            this.Dispatcher.BeginInvoke(() =>
            {
                DemoList.ItemsSource = list;
            });
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}