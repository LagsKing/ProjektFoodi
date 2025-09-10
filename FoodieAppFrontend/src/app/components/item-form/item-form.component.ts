import { Component } from '@angular/core';
import { ItemService } from '../../services/item.service';
import { Item } from '../../models/item.model';

@Component({
  selector: 'app-item-form',
  templateUrl: './item-form.component.html'
})
export class ItemFormComponent {
  newItem: Item = {
    name: '',
    category: 'Drinki',
    price: 0,
    rating: 1,
    location: ''
  };

  categories = ['Drinki', 'Piwa', 'Śniadania/Desery', 'Obiady'];

  constructor(private itemService: ItemService) {}

  addItem() {
    this.itemService.addItem(this.newItem).subscribe({
      next: () => {
        alert('Dodano pozycję!');
        this.newItem = { name: '', category: 'Drinki', price: 0, rating: 1, location: '' };
      },
      error: (err) => console.error('Błąd przy dodawaniu', err)
    });
  }
}
