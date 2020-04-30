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
}

export class RecipeProductPreference {
    public recipeId: string;
    public matchingText: string;
    public productId: number;
    public varietyId: number;
    public Product: Product;
    public Variety: ProductVariety;
}