import { Ingredient } from "./ingredient";
import { Direction } from "./direction";
import { Product, ProductVariety } from "./pantryline";
import { StringHelper } from "./extensions";

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
    private _plainText: string = '';
    matchType: number;
    product: Product;
    productId: number;
    quantityString: string;
    recipeId: string;
    size: string;
    unit: string;
    variety: ProductVariety;
    varietyId: number;

    public get plainText(): string {
        return StringHelper.TrimExcess(this._plainText);
    } public set plainText(value: string) {
        this._plainText = StringHelper.TrimExcess(value);
    }

    public get fullDescription(): string {
        let str = `${this.quantityString ?? ''} ${this.size ?? ''} ${this.unit ?? ''} ${!!this.variety ? this.variety.description : ''} ${!!this.product ? this.product.name : ''}`;
        return StringHelper.TrimExcess(str);
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