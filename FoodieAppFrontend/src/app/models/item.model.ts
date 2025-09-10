export interface Item {
  id?: number;
  name: string;
  category: string; // Drinki, Piwa, Śniadania/Desery, Obiady
  price: number;
  rating: number;   // 1-10
  location: string;
  imageUrl?: string;
}