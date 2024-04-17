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

        private List<string> selectedItems = new List<string>(); // Dodaj pole przechowuj¹ce zaznaczone elementy

        private void OnItemDeleted(object sender, EventArgs e)
        {
            foreach (var selectedItem in selectedItems.ToList()) // U¿yj ToList(), aby unikn¹æ modyfikacji kolekcji podczas iteracji
            {
                collection.Items.Remove(selectedItem);
                selectedItems.Remove(selectedItem); // Usuñ zaznaczony element z listy zaznaczonych elementów
            }
            DataManagement.SaveCollections(collections); // U¿yj kolekcji przekazanej jako argument
            RefreshListView();
        }

        private async void OnItemEdited(object sender, EventArgs e)
        {
            if (ItemsListView.SelectedItem != null)
            {
                // Pobierz indeks zaznaczonego elementu
                int selectedIndex = collection.Items.IndexOf((string)ItemsListView.SelectedItem);

                // Wyœwietl okno dialogowe do edycji nazwy elementu
                string editedItemName = await DisplayPromptAsync("Edit Item", "Enter new item name:", "OK", "Cancel", "Edited Item");

                // SprawdŸ, czy u¿ytkownik wprowadzi³ nazwê elementu
                if (!string.IsNullOrWhiteSpace(editedItemName))
                {
                    // Zaktualizuj nazwê elementu w kolekcji
                    collection.Items[selectedIndex] = editedItemName;

                    // Zapisz zmienion¹ kolekcjê
                    DataManagement.SaveCollections(collections);

                    // Odœwie¿ zawartoœæ ListView
                    RefreshListView();
                }
            }
        }


        // Obs³uga zdarzenia zaznaczenia elementu w ListView
        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                string selectedItem = (string)e.SelectedItem;
                if (!selectedItems.Contains(selectedItem))
                {
                    selectedItems.Add(selectedItem); // Dodaj zaznaczony element do listy zaznaczonych elementów
                }
                else
                {
                    selectedItems.Remove(selectedItem); // Usuñ zaznaczony element z listy zaznaczonych elementów
                }
            }
        }

        // Odœwie¿enie zawartoœci ListView
        private void RefreshListView()
        {
            ItemsListView.ItemsSource = null;
            ItemsListView.ItemsSource = collection.Items;
        }
    }
}
