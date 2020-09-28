import { Ingredient } from "./ingredient";
import { Direction } from "./direction";
import { Product, ProductVariety } from "./pantryline";

export class Recipe
{
    id: string;
    ownerId: string;
    title: string;
    credit: string;
    publicState: string;
    rawText: string;
    prepTime: string;
    tags: string[];
    ingredients: Ingredient[];
    directions: Direction[];
}

export class RecipeProduct {
    matchType: number;
    plainText: string;
    product: Product;
    productId: number;
    quantityString: string;
    recipeId: string;
    size: string;
    unit: string;
    variety: ProductVariety;
    varietyId: number;

    public get fullDescription(): string {
        return `${this.quantityString ?? ''} ${this.size ?? ''} ${this.unit ?? ''} ${this.plainText ?? ''}`
            .replace('  ', ' ')
            .trim();
    }
}

export class RecipeProductPreference {
    public recipeId: string;
    public matchingText: string;
    public productId: number;
    public varietyId: number;
    public Product: Product;
    public Variety: ProductVariety;
    public Quantity: string;
    public Size: string;
    public Unit: string;
}