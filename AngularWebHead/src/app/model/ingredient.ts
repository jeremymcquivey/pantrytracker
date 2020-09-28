export class Ingredient
{
    recipeId: string;
    index: number;
    quantity: string;
    size: string;
    unit: string;
    name: string;

    public FullSentence(): string {
        return `${this.quantity ?? ''} ${this.size ?? ''} ${this.unit ?? ''} ${this.name ?? ''}`
            .replace('  ', ' ')
            .trim();
    }
}