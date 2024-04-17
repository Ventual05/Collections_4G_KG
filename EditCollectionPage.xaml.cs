using CollectionManagementSystem.Models;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CollectionManagementSystem
{
    public partial class EditCollectionPage : ContentPage
    {
        private Collection collection;
        private List<Collection> collections;

        public EditCollectionPage(Collection selectedCollection, List<Collection> allCollections)
        {
            InitializeComponent();
            collection = selectedCollection;
            collections = allCollections;
            ItemsListView.ItemsSource = collection.Items;
        }
        private void OnAddItemClicked(object sender, EventArgs e)
        {
            string newItemName = "New Item";
            collection.Items.Add(newItemName);
            DataManagement.SaveCollections(collections);
            ItemsListView.ItemsSource = null;
            ItemsListView.ItemsSource = collection.Items;
        }

        private List<string> selectedItems = new List<string>(); // Dodaj pole przechowuj�ce zaznaczone elementy

        private void OnItemDeleted(object sender, EventArgs e)
        {
            foreach (var selectedItem in selectedItems.ToList()) // U�yj ToList(), aby unikn�� modyfikacji kolekcji podczas iteracji
            {
                collection.Items.Remove(selectedItem);
                selectedItems.Remove(selectedItem); // Usu� zaznaczony element z listy zaznaczonych element�w
            }
            DataManagement.SaveCollections(collections); // U�yj kolekcji przekazanej jako argument
            RefreshListView();
        }

        private async void OnItemEdited(object sender, EventArgs e)
        {
            if (ItemsListView.SelectedItem != null)
            {
                // Pobierz indeks zaznaczonego elementu
                int selectedIndex = collection.Items.IndexOf((string)ItemsListView.SelectedItem);

                // Wy�wietl okno dialogowe do edycji nazwy elementu
                string editedItemName = await DisplayPromptAsync("Edit Item", "Enter new item name:", "OK", "Cancel", "Edited Item");

                // Sprawd�, czy u�ytkownik wprowadzi� nazw� elementu
                if (!string.IsNullOrWhiteSpace(editedItemName))
                {
                    // Zaktualizuj nazw� elementu w kolekcji
                    collection.Items[selectedIndex] = editedItemName;

                    // Zapisz zmienion� kolekcj�
                    DataManagement.SaveCollections(collections);

                    // Od�wie� zawarto�� ListView
                    RefreshListView();
                }
            }
        }


        // Obs�uga zdarzenia zaznaczenia elementu w ListView
        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                string selectedItem = (string)e.SelectedItem;
                if (!selectedItems.Contains(selectedItem))
                {
                    selectedItems.Add(selectedItem); // Dodaj zaznaczony element do listy zaznaczonych element�w
                }
                else
                {
                    selectedItems.Remove(selectedItem); // Usu� zaznaczony element z listy zaznaczonych element�w
                }
            }
        }

        // Od�wie�enie zawarto�ci ListView
        private void RefreshListView()
        {
            ItemsListView.ItemsSource = null;
            ItemsListView.ItemsSource = collection.Items;
        }
    }
}
