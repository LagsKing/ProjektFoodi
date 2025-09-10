import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ItemService } from '../../services/item.service';
import { Item } from '../../models/item.model';

@Component({
  selector: 'app-item-list',
  templateUrl: './item-list.component.html'
})
export class ItemListComponent implements OnInit {
  items: Item[] = [];
  loading = true;
  currentCategory: string | null = null;

  constructor(private itemService: ItemService, private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.currentCategory = params['category'] || null;
      this.loadItems();
    });
  }

  loadItems() {
    this.itemService.getItems().subscribe({
      next: (data) => {
        this.items = this.currentCategory
          ? data.filter(item => item.category === this.currentCategory)
          : data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error fetching items', err);
        this.loading = false;
      }
    });
  }

  deleteItem(id: number) {
    if (confirm('Czy na pewno chcesz usunąć?')) {
      this.itemService.deleteItem(id).subscribe(() => {
        this.loadItems();
      });
    }
  }
}
