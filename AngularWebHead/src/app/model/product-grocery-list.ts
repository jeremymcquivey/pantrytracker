import { RecipeProduct } from "./recipe";
import { Product, ProductVariety } from "./pantryline";

export class ProductGroceryList {
    public matched: RecipeProduct[];

    public unmatched: RecipeProduct[];

    public ignored: RecipeProduct[];
}

export class GroceryItem {
    public id: number;

    public quantity: string;

    public status: number = GroceryItemStatus.Active;

    public product: Product;

    public productId: number;

    public variety: ProductVariety;

    public varietyId: number;

    public unit: string;

    public size: string;

    public code: string;

    public freeformText: string;
}

export enum GroceryItemStatus {
    Active = 0,
    Purchased = 1,
    Archived = 2
}