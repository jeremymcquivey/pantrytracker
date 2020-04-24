import { Ingredient } from "./ingredient";
import { PantryLine } from "./pantryline";

export class ProductGroceryList {
    public Matched: PantryLine[];

    public Unmatched: Ingredient[];

    public Ignored: Ingredient[];
}