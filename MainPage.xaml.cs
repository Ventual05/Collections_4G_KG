using CollectionManagementSystem.Models;
using Microsoft.Maui.Controls;
using System;

using System.Collections.Generic;

namespace CollectionManagementSystem
{
    public partial class MainPage : ContentPage
    {
        public List<Collection> collections { get; set; } = new List<Collection>(); // Definicja właściwości Collections

        public MainPage()
        {
            InitializeComponent();
            collections = DataManagement.LoadCollections();
            CollectionsListView.ItemsSource = collections;
            
        }
        private async void OnAddCollectionClicked(object sender, EventArgs e)
        {
            // Utwórz okno dialogowe do wprowadzania nazwy kolekcji
            string newCollectionName = await DisplayPromptAsync("New Collection", "Enter collection name:", "OK", "Cancel", "New Collection");

            // Sprawdź, czy użytkownik wprowadził nazwę kolekcji
            if (!string.IsNullOrWhiteSpace(newCollectionName))
            {
                Collection newCollection = new Collection(newCollectionName);
                collections.Add(newCollection);
                DataManagement.SaveCollections(collections);
                CollectionsListView.ItemsSource = null;
                CollectionsListView.ItemsSource = collections;
            }
        }

        private void OnCollectionSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            Collection selectedCollection = (Collection)e.SelectedItem;
            Navigation.PushAsync(new EditCollectionPage(selectedCollection, collections));
        }


        private void OnDeleteCollectionClicked(object sender, EventArgs e)
        {
            if (CollectionsListView.SelectedItem != null)
            {
                Collection selectedCollection = (Collection)CollectionsListView.SelectedItem;
                collections.Remove(selectedCollection); // Usuń wybraną kolekcję z listy
                DataManagement.SaveCollections(collections); // Zapisz zmienioną listę kolekcji
                CollectionsListView.ItemsSource = null;
                CollectionsListView.ItemsSource = collections; // Odśwież widok listy kolekcji
            }
        }

        
    }

}
