import { Ingredient } from "./ingredient";
import { Direction } from "./direction";

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