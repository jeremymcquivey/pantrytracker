import { StringHelper } from "./extensions";

export class Ingredient
{
    recipeId: string;
    index: number;
    quantity: string;
    size: string;
    unit: string;
    name: string;

    public FullSentence(): string {
        let str = `${this.quantity ?? ''} ${this.size ?? ''} ${this.unit ?? ''} ${this.name ?? ''}`.trim();
        return StringHelper.TrimExcess(str);
    }
}