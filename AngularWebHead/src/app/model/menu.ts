export class MenuEntry {
    public id: number;

    public date: Date;

    public recipeId: string;

    public recipeName: string;
}

export class MenuGroup {
    public date: Date;

    public entries: MenuEntry[];
}