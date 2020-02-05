export class PantryLine {
    public quantity: string;

    public transactionType: number;

    public product: Product;

    public productId: number;

    public unit: string;

    public size: number;
}

export class Product {
    public name: string;

    public id: number;

    public varieties: ProductVariety[];

    public codes: ProductCode[];
}

export class ProductVariety {
    public id: number;

    public productId: number;

    public description: string;
}

export class ProductCode {
    public id: number;

    public code: string;

    public variety: ProductVariety;

    public varietyId: number;
}