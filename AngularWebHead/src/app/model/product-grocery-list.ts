import { Ingredient } from "./ingredient";
import { PantryLine } from "./pantryline";

export class ProductGroceryList {
    public matched: PantryLine[];

    public unmatched: Ingredient[];

    public ignored: Ingredient[];
}